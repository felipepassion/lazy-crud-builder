#!/usr/bin/env pwsh
<#
.SYNOPSIS
  The Lazy CRUD end-to-end battery: one unattended command that proves the platform from public-
  equivalent artifacts through the real Visual Studio T4 toolchain, with deterministic exit codes
  and machine-readable evidence.

.DESCRIPTION
  Runs, in order: prerequisites (.NET SDK, Visual Studio T4 toolchain), pack (public-equivalent
  artifacts), and the scenarios T1, P1-P7, R1-R5, F1-F5, T2, C1 (see docs/harness.md). Every run
  writes artifacts/e2e/<yyyyMMdd-HHmmss>/summary.json (source of truth), digest.txt, logs/ and
  reports/, and prints a final greppable line: RESULT: OK (passed=N failed=0) or RESULT: FAIL (...).
  Intentional-failure scenarios (F1, F4, T2) are recorded as passes when they fail as designed.

  Exit codes (shared table): 0 success, 1 assertion failure, 3 prerequisite unavailable,
  4 invalid fixture or sandbox, 5 timeout, 6 contract mismatch, 7 ownership conflict, 8 harness failure.

.PARAMETER Scenario
  Run a single scenario id (for example F1). Prerequisites still run.

.PARAMETER SkipPack
  Reuse artifacts/nuget/<version> instead of packing (only when it already exists).

.PARAMETER SkipMixedVersionPack
  Skip packing the second version used by R3 (R3 is then reported as skipped, which fails the run
  unless -Scenario selects something else).

.PARAMETER Configuration
  Build configuration (default Release).

.PARAMETER SandboxRoot
  Directory that receives the per-run sandbox folder (default: %LOCALAPPDATA%/lazycrud/e2e). Use a short path outside
  AppData when the harness runs from a process with MSIX package identity (AppData is virtualized for such processes).
#>
[CmdletBinding()]
param(
  [string]$Scenario,
  [switch]$SkipPack,
  [switch]$SkipMixedVersionPack,
  [string]$Configuration = 'Release',
  [int]$ScenarioTimeoutMinutes = 25,
  [int]$GlobalTimeoutMinutes = 150,
  [string]$SandboxRoot
)

$ErrorActionPreference = 'Stop'
$repo = Split-Path $PSScriptRoot -Parent
Set-Location $repo
$stamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$runDir = Join-Path $repo "artifacts/e2e/$stamp"
$logDir = Join-Path $runDir 'logs'
$reportDir = Join-Path $runDir 'reports'
New-Item -ItemType Directory -Force -Path $logDir, $reportDir | Out-Null
# Sandboxes live under LocalApplicationData, not %TEMP%: project.tt treats any template under a directory
# named "Temp" as temporary and deletes it after writing, and devenv cannot open solutions beyond 260 characters.
if ($SandboxRoot) { $sandboxRoot = Join-Path $SandboxRoot $stamp } else { $sandboxRoot = Join-Path ([Environment]::GetFolderPath('LocalApplicationData')) "lazycrud/e2e/$stamp" }
New-Item -ItemType Directory -Force -Path $sandboxRoot | Out-Null
$globalDeadline = (Get-Date).AddMinutes($GlobalTimeoutMinutes)
$script:ExitCode = 0

function Log([string]$m) { $line = "[{0}] {1}" -f (Get-Date -Format 'HH:mm:ss'), $m; Write-Host $line; Add-Content -Path (Join-Path $runDir 'harness.log') -Value $line }

function Get-CentralVersion {
  $xml = [xml](Get-Content -Raw (Join-Path $repo 'Directory.Build.props'))
  $v = $xml.Project.PropertyGroup | ForEach-Object { $_.LazyCrudVersion } | Where-Object { $_ } | ForEach-Object { if ($_ -is [System.Xml.XmlElement]) { $_.InnerText } else { [string]$_ } } | Select-Object -First 1
  return ([string]$v).Trim()
}
$version = Get-CentralVersion
$mixedVersion = ($version -replace '(\d+)\.(\d+)\.(\d+).*', { "{0}.{1}.{2}" -f $_.Groups[1].Value, $_.Groups[2].Value, ([int]$_.Groups[3].Value + 1) })
$tool = Join-Path $repo "src/Platform/Lazy.Crud.Builder.Platform.Tool/bin/$Configuration/net10.0/Lazy.Crud.Builder.Platform.Tool.dll"
$platformDll = Join-Path $repo "src/Platform/Lazy.Crud.Builder.Platform/bin/$Configuration/netstandard2.0/Lazy.Crud.Builder.Platform.dll"
$feed = Join-Path $repo "artifacts/nuget/$version"
$mixedFeed = Join-Path $repo "artifacts/nuget/$mixedVersion"
$legacyFeed = Join-Path $repo 'artifacts/nuget/1.0.35'
$fixture = Join-Path $repo 'tests/fixtures/reference-aggregate'
$contracts = Join-Path $repo 'contracts'
$descriptorPath = Join-Path $repo 'artifacts/platform/lazycrud.platform.json'

# ------------------------------------------------------------------ process helpers ----------------
class ProcResult { [int]$ExitCode; [string]$Output; [string]$LogPath }
$script:procIndex = 0

function Invoke-Proc([string]$file, [string[]]$arguments, [string]$cwd, [int]$timeoutSec, [string]$label, [hashtable]$env = @{}) {
  $script:procIndex++
  $logPath = Join-Path $logDir ("{0:D3}-{1}.log" -f $script:procIndex, ($label -replace '[^A-Za-z0-9_.-]', '_'))
  $psi = [System.Diagnostics.ProcessStartInfo]::new($file)
  foreach ($a in $arguments) { $psi.ArgumentList.Add($a) }
  $psi.WorkingDirectory = $cwd
  $psi.RedirectStandardOutput = $true
  $psi.RedirectStandardError = $true
  $psi.UseShellExecute = $false
  $psi.CreateNoWindow = $true
  foreach ($k in $env.Keys) { $psi.Environment[$k] = [string]$env[$k] }
  $p = [System.Diagnostics.Process]::Start($psi)
  $stdout = $p.StandardOutput.ReadToEndAsync()
  $stderr = $p.StandardError.ReadToEndAsync()
  if (-not $p.WaitForExit($timeoutSec * 1000)) {
    try { $p.Kill($true) } catch {}
    Set-Content -Path $logPath -Value ("TIMEOUT after {0}s`n{1}`n{2}" -f $timeoutSec, $stdout.Result, $stderr.Result)
    throw [System.TimeoutException]::new("$label exceeded ${timeoutSec}s (log: $logPath)")
  }
  $p.WaitForExit()
  $out = $stdout.Result + "`n--- stderr ---`n" + $stderr.Result
  Set-Content -Path $logPath -Value ("$ $file $($arguments -join ' ')`nexit=$($p.ExitCode)`n$out")
  $r = [ProcResult]::new(); $r.ExitCode = $p.ExitCode; $r.Output = $out; $r.LogPath = $logPath
  return $r
}

function Invoke-Tool([string[]]$arguments, [string]$label, [int]$timeoutSec = 1500, [hashtable]$env = @{}) {
  return Invoke-Proc 'dotnet' (@($tool) + $arguments) $repo $timeoutSec $label $env
}

# The working-tree snapshot taken before and after the run. git.exe inspects the console when its standard
# handles are attached to one, and after the run has driven devenv.exe the harness's console can be gone:
# git then dies with "No process is on the other end of the pipe" and a completed run is reported as a
# harness failure. Redirecting all three handles keeps git away from the console entirely, and an
# unreadable snapshot returns $null so the TREE check reports "not verified" instead of killing the run.
function Get-WorkingTreeState {
  try {
    $psi = [System.Diagnostics.ProcessStartInfo]::new('git')
    foreach ($a in @('--no-pager', 'status', '--porcelain', '--untracked-files=all')) { $psi.ArgumentList.Add($a) }
    $psi.WorkingDirectory = $repo
    $psi.RedirectStandardInput = $true
    $psi.RedirectStandardOutput = $true
    $psi.RedirectStandardError = $true
    $psi.UseShellExecute = $false
    $psi.CreateNoWindow = $true
    $p = [System.Diagnostics.Process]::Start($psi)
    $p.StandardInput.Close()
    $stdout = $p.StandardOutput.ReadToEndAsync()
    [void]$p.StandardError.ReadToEndAsync()
    if (-not $p.WaitForExit(60000)) { try { $p.Kill($true) } catch {}; return $null }
    if ($p.ExitCode -ne 0) { return $null }
    return (($stdout.Result -split "`r?`n") | Where-Object { $_ -and $_ -notmatch '^\?\? artifacts/' -and $_ -notmatch ' artifacts/' }) -join "`n"
  }
  catch { return $null }
}

function Read-JsonOutput([ProcResult]$r) {
  $text = $r.Output -replace '(?s)\n--- stderr ---.*$', ''
  $start = $text.IndexOf('{')
  if ($start -lt 0) { throw "no JSON in output (log: $($r.LogPath))" }
  return $text.Substring($start) | ConvertFrom-Json -Depth 32
}

function Assert([bool]$condition, [string]$message) { if (-not $condition) { throw "ASSERT: $message" } }

function Get-Diags($report, [string]$code) { @($report.Diagnostics | Where-Object { $_.Code -eq $code }) }

function New-Sandbox([string]$name, [string[]]$extra, [string]$templatesArg = 'nuget') {
  $out = Join-Path $sandboxRoot $name
  $args = @('sandbox', 'create', '--output', $out, '--aggregate', 'Products', '--fixture', $fixture, '--version', $version, '--feed', $feed, '--templates', $templatesArg) + $extra
  $r = Invoke-Tool $args "sandbox-$name" 600
  if ($r.ExitCode -ne 0) { throw [System.IO.InvalidDataException]::new("sandbox '$name' could not be created (exit $($r.ExitCode), log $($r.LogPath))") }
  Add-Content -Path (Join-Path $runDir 'sandboxes.txt') -Value "$name`t$out"
  return $out
}

function Invoke-Generate([string]$sandbox, [string]$label, [string[]]$extra = @(), [hashtable]$env = @{}) {
  $reportPath = Join-Path $reportDir "$label.json"
  $r = Invoke-Tool (@('generate', '--solution', $sandbox, '--aggregate', 'Products', '--report', $reportPath) + $extra) "generate-$label" ($ScenarioTimeoutMinutes * 60 - 60) $env
  $report = Get-Content -Raw $reportPath | ConvertFrom-Json -Depth 32
  return @{ Exit = $r.ExitCode; Report = $report; Proc = $r }
}

function Get-Manifest([string]$sandbox) {
  $path = Join-Path $sandbox '.lazycrud/manifests/Products.manifest.json'
  Assert (Test-Path $path) "manifest not found: $path"
  return @{ Path = $path; Doc = (Get-Content -Raw $path | ConvertFrom-Json -Depth 16); Text = (Get-Content -Raw $path) }
}

function Get-ContentHash([string]$path) { return [Lazy.Crud.Builder.Platform.Hashing.ContentHasher]::Compute([System.IO.File]::ReadAllText($path)) }

function Assert-Transformed($result, [string]$label) {
  # Transformation is proven, never inferred from a build: templates were transformed, outputs exist with markers and manifest entries.
  Assert ($result.Report.TransformedTemplates.Count -gt 0) "${label}:no template was transformed"
  Assert ($result.Report.DomainGatePassed -eq $true) "${label}:the Domain gate did not pass"
  Assert ($result.Report.Manifest -ne $null -and $result.Report.Manifest.Files -gt 0) "${label}:the manifest records no files"
  Assert ($result.Report.Toolchain.resolved -eq $true) "${label}:no resolved toolchain recorded"
}

# ------------------------------------------------------------------ scenario registry --------------
$scenarios = New-Object System.Collections.ArrayList
function Add-Scenario([string]$Id, [string]$Title, [scriptblock]$Action, [switch]$IntentionalFailure) {
  [void]$scenarios.Add(@{ Id = $Id; Title = $Title; Action = $Action; IntentionalFailure = [bool]$IntentionalFailure })
}
$state = @{}   # shared state between scenarios (sandbox paths, hashes)

# ==================================================================================================
# Scenarios
# ==================================================================================================

Add-Scenario 'T1' 'Toolchain resolved through the supported mechanism and recorded' {
  $r = Invoke-Tool @('toolchain') 'toolchain' 120
  Assert ($r.ExitCode -eq 0) "toolchain exit $($r.ExitCode)"
  $tc = Read-JsonOutput $r
  Assert ($tc.resolved -eq $true) 'not resolved'
  Assert ($tc.discoverySource -in @('vswhere', 'setup-configuration-api', 'override')) "unexpected discovery source $($tc.discoverySource)"
  Assert (Test-Path $tc.textTransformPath) 'T4 tooling path does not exist'
  Assert (Test-Path $tc.devenvPath) 'devenv path does not exist'
  Assert ($tc.channel -in @('release', 'prerelease')) 'channel missing'
  $state.Toolchain = $tc
  Copy-Item $r.LogPath (Join-Path $reportDir 'T1-toolchain.log')
}

Add-Scenario 'P1' 'Sample aggregate created and every declared layer generated from public-equivalent packages' {
  $sbx = New-Sandbox 'sbx-main' @()
  $state.Main = $sbx
  $g = Invoke-Generate $sbx 'P1'
  Assert ($g.Exit -eq 0) "generate exit $($g.Exit) ($($g.Report.ExitCodeName))"
  Assert-Transformed $g 'P1'
  $m = Get-Manifest $sbx
  $descriptor = Get-Content -Raw $descriptorPath | ConvertFrom-Json -Depth 32
  foreach ($layer in ($descriptor.layers | Where-Object { $_.templatePackageId })) {
    $count = @($m.Doc.files | Where-Object { $_.layer -eq $layer.id }).Count
    Assert ($count -gt 0) "no manifest entry for layer $($layer.id)"
  }
  foreach ($t in $descriptor.testLayers) {
    Assert (@($m.Doc.files | Where-Object { $_.layer -eq $t.id }).Count -gt 0) "no manifest entry for test layer $($t.id)"
  }
  Assert ($g.Report.TemplateSource.Kind -eq 'NuGet') 'templates did not come from packages'
  $state.P1 = $g
}

Add-Scenario 'P2' 'Marker v2 on every generated file; GeneratedOnce carries the user-owned directive; Manual files unmarked' {
  $m = Get-Manifest $state.Main
  $seenOnce = $false
  foreach ($f in $m.Doc.files) {
    if ($f.unresolved) { continue }
    $path = Join-Path $state.Main $f.relativePath
    Assert (Test-Path $path) "recorded file missing: $($f.relativePath)"
    $syntax = [Lazy.Crud.Builder.Platform.Markers.CommentSyntax]::ForFileName($path)
    if ($null -eq $syntax) { continue }   # unmarkable type: tracked, not marked
    $info = $null
    $ok = [Lazy.Crud.Builder.Platform.Markers.GeneratedCodeMarker]::TryParse([System.IO.File]::ReadAllText($path), [ref]$info)
    Assert ($ok -and $info.IsV2 -and $info.Problems.Count -eq 0) "no valid marker v2 on $($f.relativePath)"
    Assert ($info.Get('TemplateId') -eq $f.templateId) "marker templateId disagrees with manifest for $($f.relativePath)"
    Assert ($info.Get('Ownership') -eq $f.ownership) "marker ownership disagrees with manifest for $($f.relativePath)"
    Assert ($f.ownership -in @('GeneratorOwned', 'GeneratedOnce')) "unexpected ownership $($f.ownership)"
    $text = [System.IO.File]::ReadAllText($path)
    if ($f.ownership -eq 'GeneratedOnce') { $seenOnce = $true; Assert ($text -match 'user-owned' -and $text -notmatch 'overwritten') "GeneratedOnce directive wrong on $($f.relativePath)" }
    else { Assert ($text -match 'overwritten') "GeneratorOwned directive wrong on $($f.relativePath)" }
  }
  Assert $seenOnce 'no GeneratedOnce file was generated'
  $program = Join-Path $state.Main 'src/Products/Products.Api/Program.cs'
  $info2 = $null
  Assert (-not [Lazy.Crud.Builder.Platform.Markers.GeneratedCodeMarker]::TryParse([System.IO.File]::ReadAllText($program), [ref]$info2)) 'Manual file Program.cs carries a marker'
}

Add-Scenario 'P3' 'Manifest is v2, relative forward-slash paths, populated identity, hash and contract version; validates against the published schema' {
  $m = Get-Manifest $state.Main
  Assert ($m.Doc.schemaVersion -eq 2) 'schemaVersion is not 2'
  Assert ($m.Doc.contractVersion -eq 1) 'contractVersion missing'
  Assert ($m.Doc.generatorVersion -eq $version) "generatorVersion $($m.Doc.generatorVersion) != $version"
  Assert ($m.Text -notmatch '[A-Za-z]:\\' -and $m.Text -notmatch '\\\\') 'absolute or backslash path in manifest'
  Assert ($m.Text -notmatch 'FullPath') 'v1 FullPath present'
  foreach ($f in $m.Doc.files) {
    Assert (-not [string]::IsNullOrEmpty($f.templateId)) "templateId empty for $($f.relativePath)"
    Assert (-not [string]::IsNullOrEmpty($f.ownership)) "ownership empty for $($f.relativePath)"
    Assert ($f.contentHash -match '^[a-f0-9]{64}$') "contentHash empty for $($f.relativePath)"
    Assert ($f.hashAlgorithm -eq 'SHA-256') "hashAlgorithm wrong for $($f.relativePath)"
    Assert ($f.hashScope -in @('body-lf-marker-excluded', 'body-lf')) "hashScope wrong for $($f.relativePath)"
    Assert (-not [string]::IsNullOrEmpty($f.layer)) "layer empty for $($f.relativePath)"
  }
  $r = Invoke-Tool @('validate', 'manifest', '--file', $m.Path, '--repo', $repo) 'validate-manifest-P3' 120
  Assert ($r.ExitCode -eq 0) "manifest does not validate against the published schema (log $($r.LogPath))"
  Copy-Item $m.Path (Join-Path $reportDir 'P3-Products.manifest.json')
}

Add-Scenario 'P4' 'Recorded hashes match the files on disk using the frozen rule and the published vectors' {
  $vectors = (Get-Content -Raw (Join-Path $contracts 'fixtures/v1/hash-vectors.json') | ConvertFrom-Json -Depth 8).vectors
  foreach ($v in $vectors) {
    $h = [Lazy.Crud.Builder.Platform.Hashing.ContentHasher]::Compute($v.input)
    Assert ($h.Value -eq $v.expectedHash) "published vector $($v.id) does not reproduce with the production implementation"
  }
  $m = Get-Manifest $state.Main
  $state.HashesBefore = @{}
  foreach ($f in $m.Doc.files) {
    $path = Join-Path $state.Main $f.relativePath
    $h = Get-ContentHash $path
    Assert ($h.Value -eq $f.contentHash) "hash mismatch for $($f.relativePath)"
    Assert ($h.Scope -eq $f.hashScope) "scope mismatch for $($f.relativePath)"
    $state.HashesBefore[$f.relativePath] = $h.Value
  }
}

Add-Scenario 'P5' 'Generated solution builds and its generated tests pass' {
  $b = Invoke-Proc 'dotnet' @('build', (Join-Path $state.Main 'Sandbox.sln'), '-c', 'Debug', '--nologo', '-v', 'q') $state.Main 1200 'build-P5'
  Assert ($b.ExitCode -eq 0) "build failed (log $($b.LogPath))"
  $t = Invoke-Proc 'dotnet' @('test', (Join-Path $state.Main 'Sandbox.sln'), '-c', 'Debug', '--no-build', '--nologo', '-v', 'q') $state.Main 1200 'test-P5'
  Assert ($t.ExitCode -eq 0) "generated tests failed (log $($t.LogPath))"
  Assert ($t.Output -match 'Passed!' -or $t.Output -match 'Passed:\s*[1-9]') 'no passing tests reported'
  Assert ($t.Output -notmatch 'Failed:\s*[1-9]') 'failing tests reported'
}

Add-Scenario 'P6' 'Regeneration without model change is stable: no drift, unchanged content' {
  $g = Invoke-Generate $state.Main 'P6'
  Assert ($g.Exit -eq 0) "regeneration exit $($g.Exit)"
  Assert ((Get-Diags $g.Report 'LC2001').Count -eq 0) 'drift reported on unchanged regeneration'
  Assert ((Get-Diags $g.Report 'LC2002').Count -eq 0) 'unverified reported on unchanged regeneration'
  $m = Get-Manifest $state.Main
  foreach ($f in $m.Doc.files) {
    if ($f.ownership -eq 'GeneratedOnce') { continue }  # seeded once; the generator never rewrites it
    $h = Get-ContentHash (Join-Path $state.Main $f.relativePath)
    Assert ($h.Value -eq $state.HashesBefore[$f.relativePath]) "content changed on regeneration: $($f.relativePath)"
  }
}

Add-Scenario 'P7' 'Documented quickstart path from published packages only' {
  $out = Join-Path $sandboxRoot 'sbx-quickstart'
  $r = Invoke-Tool @('scaffold', '--output', $out, '--aggregate', 'Shop', '--version', $version, '--feed', $feed) 'scaffold-P7' 600
  Assert ($r.ExitCode -eq 0) "scaffold failed (log $($r.LogPath))"
  Add-Content -Path (Join-Path $runDir 'sandboxes.txt') -Value "sbx-quickstart`t$out"
  $reportPath = Join-Path $reportDir 'P7.json'
  $g = Invoke-Tool @('generate', '--solution', $out, '--aggregate', 'Shop', '--report', $reportPath) 'generate-P7' ($ScenarioTimeoutMinutes * 60 - 60)
  Assert ($g.ExitCode -eq 0) "quickstart generation exit $($g.ExitCode)"
  $b = Invoke-Proc 'dotnet' @('build', (Join-Path $out 'Sandbox.sln'), '-c', 'Debug', '--nologo', '-v', 'q') $out 1200 'build-P7'
  Assert ($b.ExitCode -eq 0) "quickstart build failed (log $($b.LogPath))"
  Assert ((Get-ChildItem (Join-Path $out 'src/Shop/Shop.Domain/LazyCode') -Filter '*.cs').Count -gt 0) 'no Domain output'
}

Add-Scenario 'R1' 'Published v1 manifest fixture accepted, upgraded to v2 matching the expected output, no drift or unverified failure' {
  $sbx = New-Sandbox 'sbx-legacy' @()
  $state.Legacy = $sbx
  $g0 = Invoke-Generate $sbx 'R1-seed'
  Assert ($g0.Exit -eq 0) "seed generation exit $($g0.Exit)"
  $m = Get-Manifest $sbx
  # Seed the published v1 shape: the fixture's FullPath entries re-anchored on this sandbox, plus its unresolvable entry and empty hashes.
  $v1Fixture = Get-Content -Raw (Join-Path $contracts 'fixtures/v1/manifest-v1-input.json')
  $expected = Get-Content -Raw (Join-Path $contracts 'fixtures/v1/manifest-v1-migrated.json') | ConvertFrom-Json -Depth 16
  $entries = @()
  foreach ($f in ($m.Doc.files | Where-Object { $_.ownership -eq 'GeneratorOwned' })) {
    $full = (Join-Path $sbx $f.relativePath) -replace '/', '\'
    $entries += "    { `"FullPath`": `"$($full -replace '\\', '\\')`", `"TemplateName`": `"Unknown`", `"Layer`": `"$($f.layer)`", `"ContentHash`": `"`" }"
  }
  $entries += "    { `"FullPath`": `"E:\\repos\\felipepassion\\lazy-crud-builder\\LazyCode\\Products\\DefaultEntities.generated.cs`", `"TemplateName`": `"OutputRedirection`", `"Layer`": `"Redirected`", `"ContentHash`": `"`" }"
  $v1 = "{`n  `"AggregateName`": `"Products`",`n  `"GeneratedAtUtc`": `"2026-05-09T09:44:52.3962907Z`",`n  `"GeneratorVersion`": `"1.0`",`n  `"GeneratedFiles`": [`n" + ($entries -join ",`n") + "`n  ]`n}"
  Set-Content -Path $m.Path -Value $v1 -Encoding UTF8
  Copy-Item $m.Path (Join-Path $reportDir 'R1-seeded-v1.manifest.json')
  $g = Invoke-Generate $sbx 'R1'
  Assert ($g.Exit -eq 0) "regeneration over v1 manifest exit $($g.Exit) ($($g.Report.ExitCodeName))"
  Assert ((Get-Diags $g.Report 'LC2001').Count -eq 0) 'drift reported'
  Assert ((Get-Diags $g.Report 'LC2002').Count -eq 0) 'unverified failure reported'
  Assert ((Get-Diags $g.Report 'LC1009').Count -ge 1) 'migration diagnostic missing'
  Assert ((Get-Diags $g.Report 'LC1006').Count -ge 1) 'unresolved path warning missing'
  Assert ((Get-Diags $g.Report 'LC2008').Count -ge 1) 'no adoption diagnostic'
  $m2 = Get-Manifest $sbx
  Assert ($m2.Doc.schemaVersion -eq 2) 'manifest not upgraded to v2'
  Assert ($m2.Text -notmatch 'E:\\' -and $m2.Text -notmatch 'FullPath') 'absolute path or FullPath survived migration'
  $unresolved = @($m2.Doc.files | Where-Object { $_.unresolved -eq $true })
  Assert ($unresolved.Count -eq 1 -and $unresolved[0].fileName -eq 'DefaultEntities.generated.cs' -and $unresolved[0].originalPathSha256 -eq $expected.files[2].originalPathSha256) 'unresolved entry does not match the published expected output'
  foreach ($f in ($m2.Doc.files | Where-Object { -not $_.unresolved })) { Assert ($f.contentHash -match '^[a-f0-9]{64}$') "hash not populated after upgrade: $($f.relativePath)" }
  Copy-Item $m2.Path (Join-Path $reportDir 'R1-upgraded-v2.manifest.json')
}

Add-Scenario 'R2' 'Deprecated templates package id resolves with a deprecation notice' {
  $sbx = New-Sandbox 'sbx-alias' @('--templates-package', 'Lazy.Crud.Templates')
  $g = Invoke-Generate $sbx 'R2'
  Assert ($g.Exit -eq 0) "generation with alias exit $($g.Exit) ($($g.Report.ExitCodeName))"
  Assert ((Get-Diags $g.Report 'LC3003').Count -ge 1) 'deprecation notice LC3003 missing'
  Assert ($g.Report.TemplateSource.RequestedPackageId -eq 'Lazy.Crud.Templates') 'requested id not recorded'
  Assert ($g.Report.TemplateSource.Location -match 'lazy\.crud\.builder\.templates') 'templates did not resolve from the canonical package'
  Assert-Transformed $g 'R2'
}

Add-Scenario 'R3' 'Supported mixed package versions restore, build and generate; descriptor winner reported' {
  if (-not (Test-Path $mixedFeed)) { throw [System.IO.InvalidDataException]::new("second version feed missing: $mixedFeed") }
  $sbx = New-Sandbox 'sbx-mixed' @('--feed', $mixedFeed, '--override', "Lazy.Crud.Builder.Api=$version", '--override', "Lazy.Crud.Builder.WebApi=$version", '--version', $mixedVersion)
  $g = Invoke-Generate $sbx 'R3'
  Assert ($g.Exit -eq 0) "mixed-version generation exit $($g.Exit) ($($g.Report.ExitCodeName))"
  Assert ($g.Report.Descriptor.PlatformVersion -eq $mixedVersion) "descriptor winner is $($g.Report.Descriptor.PlatformVersion), expected $mixedVersion"
  Assert ((Get-Diags $g.Report 'LC5002').Count -ge 1) 'descriptor conflict diagnostic missing for mixed versions'
  $b = Invoke-Proc 'dotnet' @('build', (Join-Path $sbx 'Sandbox.sln'), '-c', 'Debug', '--nologo', '-v', 'q') $sbx 1200 'build-R3'
  Assert ($b.ExitCode -eq 0) "mixed-version build failed (log $($b.LogPath))"
  Assert-Transformed $g 'R3'
}

Add-Scenario 'R4' 'Reference aggregate output matches the golden files apart from header blocks' {
  $r = Invoke-Proc 'pwsh' @('-NoProfile', '-File', (Join-Path $repo 'scripts/golden.ps1'), '-Configuration', $Configuration, '-Sandbox', (Join-Path $sandboxRoot 'sbx-golden')) $repo ($ScenarioTimeoutMinutes * 60 - 60) 'golden-R4'
  Assert ($r.ExitCode -eq 0) "golden comparison failed (exit $($r.ExitCode), log $($r.LogPath))"
  Assert ($r.Output -match 'RESULT: OK') 'golden script did not report OK'
}

Add-Scenario 'R5' 'Multi-layer consumer restores cleanly with exactly one copy of each shared template' {
  $assets = Get-ChildItem (Join-Path $state.Main 'src/Products') -Recurse -Filter 'project.assets.json' | Where-Object { $_.FullName -match 'Products\.Api\\obj' } | Select-Object -First 1
  Assert ($null -ne $assets) 'project.assets.json of Products.Api not found'
  $doc = Get-Content -Raw $assets.FullName | ConvertFrom-Json -Depth 32
  $owners = @()
  foreach ($target in $doc.targets.PSObject.Properties) {
    foreach ($lib in $target.Value.PSObject.Properties) {
      if ($lib.Name -notmatch '^Lazy\.Crud\.') { continue }
      $cf = $lib.Value.contentFiles
      if ($null -eq $cf) { continue }
      foreach ($p in $cf.PSObject.Properties) { if ($p.Name -match 'LazyCrudTemplates/project\.tt$') { $owners += $lib.Name } }
    }
    break
  }
  $owners = @($owners | Sort-Object -Unique)
  Assert ($owners.Count -eq 1 -and $owners[0] -match '^Lazy\.Crud\.Builder\.Templates/') "project.tt is delivered by $($owners -join ', ') (expected only Lazy.Crud.Builder.Templates)"
  $layerPackages = @($doc.libraries.PSObject.Properties | Where-Object { $_.Name -match '^Lazy\.Crud\.Builder\.(Domain|Application|Infra\.Data|Infra\.IoC|Api)/' })
  Assert ($layerPackages.Count -ge 2) 'the Api project does not reference two or more layer packages transitively'
  $restore = Invoke-Proc 'dotnet' @('restore', (Join-Path $state.Main 'Sandbox.sln'), '--force', '-v', 'n') $state.Main 600 'restore-R5'
  Assert ($restore.ExitCode -eq 0) 'restore failed'
  Assert ($restore.Output -notmatch 'NU1603|NU1605|duplicate') 'restore reported duplicate or downgrade warnings'
}

Add-Scenario 'F1' 'External modification of a GeneratorOwned file is detected and blocked; the edit survives' -IntentionalFailure {
  $m = Get-Manifest $state.Main
  $target = ($m.Doc.files | Where-Object { $_.ownership -eq 'GeneratorOwned' -and $_.relativePath -match 'Filters\.cs$' } | Select-Object -First 1)
  Assert ($null -ne $target) 'no GeneratorOwned Filters file to modify'
  $path = Join-Path $state.Main $target.relativePath
  Add-Content -Path $path -Value "`r`n// external edit by a human or an AI agent`r`n"
  $state.Drifted = $target.relativePath
  $state.DriftedText = [System.IO.File]::ReadAllText($path)
  $g = Invoke-Generate $state.Main 'F1'
  Assert ($g.Exit -eq 7) "expected exit 7 (OwnershipConflict) but got $($g.Exit) ($($g.Report.ExitCodeName))"
  $d = Get-Diags $g.Report 'LC2001'
  Assert ($d.Count -ge 1) 'drift diagnostic LC2001 missing'
  Assert (@($d | Where-Object { $_.Path -eq $target.relativePath }).Count -eq 1) "LC2001 does not name $($target.relativePath)"
  Assert ($d[0].Data.expectedHash.Length -eq 64 -and $d[0].Data.actualHash.Length -eq 64 -and $d[0].Data.templateId -eq $target.templateId) 'drift diagnostic lacks hashes or templateId'
  Assert ($d[0].Message -match 'force' -and $d[0].Message -match 'skip-modified') 'drift diagnostic does not name the resolutions'
  Assert ((Get-Diags $g.Report 'LC2002').Count -eq 0) 'unverified reported instead of drift'
  Assert ([System.IO.File]::ReadAllText($path) -eq $state.DriftedText) 'the external edit was lost'
  Assert ($g.Report.TransformedTemplates.Count -eq 0) 'templates were transformed despite the drift pre-check'
}

Add-Scenario 'F2' 'Force flag resolves the drift deliberately and reports the overwrite' {
  $path = Join-Path $state.Main $state.Drifted
  $g = Invoke-Generate $state.Main 'F2' @('--resolution', 'force')
  Assert ($g.Exit -eq 0) "force regeneration exit $($g.Exit) ($($g.Report.ExitCodeName))"
  Assert ([System.IO.File]::ReadAllText($path) -notmatch 'external edit') 'the file was not regenerated'
  $d = Get-Diags $g.Report 'LC2006'
  Assert (@($d | Where-Object { $_.Path -eq $state.Drifted }).Count -ge 1) 'forced overwrite not reported for the file'
}

Add-Scenario 'F3' 'Skip-modified flag preserves the modification and generates the rest' {
  $path = Join-Path $state.Main $state.Drifted
  Add-Content -Path $path -Value "`r`n// second external edit`r`n"
  $edited = [System.IO.File]::ReadAllText($path)
  $g = Invoke-Generate $state.Main 'F3' @('--resolution', 'skip-modified')
  Assert ($g.Exit -eq 0) "skip-modified regeneration exit $($g.Exit) ($($g.Report.ExitCodeName))"
  Assert ([System.IO.File]::ReadAllText($path) -eq $edited) 'the modification was lost'
  $d = Get-Diags $g.Report 'LC2007'
  Assert (@($d | Where-Object { $_.Path -eq $state.Drifted }).Count -ge 1) 'skip not reported for the file'
  Assert ($g.Report.TransformedTemplates.Count -gt 0) 'other templates were not generated'
  $m = Get-Manifest $state.Main
  $other = $m.Doc.files | Where-Object { $_.ownership -eq 'GeneratorOwned' -and $_.relativePath -ne $state.Drifted -and $_.relativePath -match 'DomainCommandHandlers\.cs$' } | Select-Object -First 1
  Assert ((Get-ContentHash (Join-Path $state.Main $other.relativePath)).Value -eq $other.contentHash) 'other files were not regenerated consistently'
  # Restore the file for later scenarios.
  $g2 = Invoke-Generate $state.Main 'F3-restore' @('--resolution', 'force')
  Assert ($g2.Exit -eq 0) 'restore regeneration failed'
}

Add-Scenario 'F4' 'Unverified file with differing content is protected with the unverified code, not the drift code' -IntentionalFailure {
  $m = Get-Manifest $state.Legacy
  $target = ($m.Doc.files | Where-Object { $_.ownership -eq 'GeneratorOwned' -and $_.relativePath -match 'DomainCommandModels\.cs$' } | Select-Object -First 1)
  Assert ($null -ne $target) 'no target file'
  $path = Join-Path $state.Legacy $target.relativePath
  # Simulate the v1 world: the entry has no trustworthy hash and the content differs from what the generator produces.
  $full = ($path -replace '/', '\')
  $v1 = "{ `"AggregateName`": `"Products`", `"GeneratorVersion`": `"1.0`", `"GeneratedFiles`": [ { `"FullPath`": `"$($full -replace '\\', '\\')`", `"TemplateName`": `"Unknown`", `"Layer`": `"Domain`", `"ContentHash`": `"`" } ] }"
  Set-Content -Path $m.Path -Value $v1 -Encoding UTF8
  # AppendAllText, not Add-Content: Add-Content would append its own line terminator as well, and F5
  # removes exactly this string to restore the generated content.
  [System.IO.File]::AppendAllText($path, "`r`n// pre-manifest customization`r`n")
  $before = [System.IO.File]::ReadAllText($path)
  $g = Invoke-Generate $state.Legacy 'F4'
  Assert ($g.Exit -eq 7) "expected exit 7 but got $($g.Exit) ($($g.Report.ExitCodeName))"
  $d = Get-Diags $g.Report 'LC2002'
  Assert (@($d | Where-Object { $_.Path -eq $target.relativePath }).Count -ge 1) 'unverified diagnostic LC2002 does not name the file'
  Assert (@(Get-Diags $g.Report 'LC2001' | Where-Object { $_.Path -eq $target.relativePath }).Count -eq 0) 'drift code reported instead of unverified'
  Assert ($d[0].Message -match 'baseline') 'unverified message does not explain the missing baseline'
  Assert ([System.IO.File]::ReadAllText($path) -eq $before) 'the file was changed'
  $state.F4Path = $path
}

Add-Scenario 'F5' 'Unverified file with identical content is adopted silently and its hash recorded' {
  # Remove the customization so the content is what the generator produces, keep the v1 (no-hash) manifest state.
  $text = [System.IO.File]::ReadAllText($state.F4Path) -replace "\r\n// pre-manifest customization\r\n", ''
  [System.IO.File]::WriteAllText($state.F4Path, $text)
  $m = Get-Manifest $state.Legacy
  $full = ($state.F4Path -replace '/', '\')
  Set-Content -Path $m.Path -Value "{ `"AggregateName`": `"Products`", `"GeneratorVersion`": `"1.0`", `"GeneratedFiles`": [ { `"FullPath`": `"$($full -replace '\\', '\\')`", `"TemplateName`": `"Unknown`", `"Layer`": `"Domain`", `"ContentHash`": `"`" } ] }" -Encoding UTF8
  $g = Invoke-Generate $state.Legacy 'F5'
  Assert ($g.Exit -eq 0) "adoption run exit $($g.Exit) ($($g.Report.ExitCodeName))"
  Assert ((Get-Diags $g.Report 'LC2002').Count -eq 0) 'unverified failure reported'
  Assert ((Get-Diags $g.Report 'LC2008').Count -ge 1) 'adoption diagnostic LC2008 missing'
  $m2 = Get-Manifest $state.Legacy
  $entry = $m2.Doc.files | Where-Object { $_.relativePath -eq (($state.F4Path.Substring($state.Legacy.Length).TrimStart('\', '/')) -replace '\\', '/') }
  Assert ($null -ne $entry -and $entry.contentHash -match '^[a-f0-9]{64}$') 'adopted file has no recorded hash'
}

Add-Scenario 'T2' 'Unresolvable toolchain fails early with the prerequisite code and an actionable diagnostic; nothing generated' -IntentionalFailure {
  $sbx = New-Sandbox 'sbx-noVS' @()
  $reportPath = Join-Path $reportDir 'T2.json'
  $r = Invoke-Tool @('generate', '--solution', $sbx, '--aggregate', 'Products', '--report', $reportPath) 'generate-T2' 600 @{ LAZYCRUD_VS_SIMULATE_UNAVAILABLE = '1' }
  Assert ($r.ExitCode -eq 3) "expected exit 3 (PrerequisiteUnavailable) but got $($r.ExitCode)"
  $report = Get-Content -Raw $reportPath | ConvertFrom-Json -Depth 32
  $d = Get-Diags $report 'LC4001'
  Assert ($d.Count -eq 1) 'LC4001 missing'
  Assert ($d[0].Data.requirement -and $d[0].Data.searched -and $d[0].Data.component -eq 'Microsoft.VisualStudio.Component.TextTemplating') 'diagnostic lacks requirement/searched/component'
  Assert ($d[0].Message -match 'Install Visual Studio') 'diagnostic lacks the remedy'
  Assert ($report.TransformedTemplates.Count -eq 0) 'templates were transformed'
  Assert (-not (Test-Path (Join-Path $sbx 'src/Products/Products.Domain/LazyCode'))) 'output was produced without a toolchain'
  Assert (-not (Test-Path (Join-Path $sbx '.lazycrud/manifests/Products.manifest.json'))) 'a manifest was written'
  Assert ($report.Succeeded -eq $false) 'the run reported success'
}

Add-Scenario 'C1' 'Clean room: public-equivalent feed only, isolated cache, no local source, local mode refused, generation from restored artifacts' {
  $packagesFolder = Join-Path $sandboxRoot 'clean-room-packages'
  New-Item -ItemType Directory -Force -Path $packagesFolder | Out-Null
  Assert ((Get-ChildItem $packagesFolder -Directory | Where-Object { $_.Name -like 'lazy.crud.*' }).Count -eq 0) 'isolated cache already contains Lazy CRUD packages'
  $sbx = New-Sandbox 'sbx-cleanroom' @('--clean-room', '--packages-folder', $packagesFolder)
  $cfg = Get-Content -Raw (Join-Path $sbx 'nuget.config')
  Assert ($cfg -match '<clear\s*/>') 'nuget.config does not clear inherited sources'
  Assert ($cfg -match 'globalPackagesFolder') 'nuget.config does not isolate the package cache'
  Assert ($cfg -match 'packageSourceMapping') 'Lazy CRUD packages are not pinned to the feed under test'
  Assert (-not (Test-Path (Join-Path $sbx 'src/ProjectFiles'))) 'sandbox contains template sources'
  Assert ((Get-ChildItem $sbx -Recurse -Filter 'project.tt' -ErrorAction SilentlyContinue).Count -eq 0) 'a copy of project.tt exists in the sandbox'
  # Every package the descriptor declares is present in the feed under test.
  $descriptor = Get-Content -Raw $descriptorPath | ConvertFrom-Json -Depth 32
  $declared = @($descriptor.layers.packageId) + @($descriptor.nonLayerPackages) + @($descriptor.templates.packageId) + @($descriptor.templates.aliasPackageIds) | Sort-Object -Unique
  foreach ($id in $declared) { Assert ((Get-ChildItem $feed -Filter "$id.$version.nupkg").Count -eq 1) "declared package missing from the feed under test: $id" }
  # Local mode must be refused.
  $reportLocal = Join-Path $reportDir 'C1-local-refused.json'
  $rl = Invoke-Tool @('generate', '--solution', $sbx, '--aggregate', 'Products', '--templates', "local:$(Join-Path $repo 'src/ProjectFiles')", '--report', $reportLocal) 'generate-C1-local' 600
  Assert ($rl.ExitCode -ne 0) 'local template mode was accepted in the clean room'
  $rlReport = Get-Content -Raw $reportLocal | ConvertFrom-Json -Depth 32
  Assert ((Get-Diags $rlReport 'LC3001').Count -ge 1) 'local mode refusal did not report LC3001'
  # Generation from restored artifacts only, through the resolved Visual Studio toolchain.
  $g = Invoke-Generate $sbx 'C1'
  Assert ($g.Exit -eq 0) "clean-room generation exit $($g.Exit) ($($g.Report.ExitCodeName))"
  Assert-Transformed $g 'C1'
  Assert ($g.Report.TemplateSource.Kind -eq 'NuGet' -and $g.Report.TemplateSource.Location.StartsWith($packagesFolder, [System.StringComparison]::OrdinalIgnoreCase)) 'templates did not resolve from the isolated cache'
  # Descriptor readable from a restored package; registry entries resolve; XML docs present.
  $restoredDescriptor = Join-Path $packagesFolder "lazy.crud.builder.domain/$version/lazycrud/lazycrud.platform.json"
  Assert (Test-Path $restoredDescriptor) 'descriptor not readable from a restored package'
  $rd = Get-Content -Raw $restoredDescriptor | ConvertFrom-Json -Depth 32
  Assert ($rd.schemaVersion -eq 1 -and $rd.contractVersion -eq 1 -and $rd.platformVersion -eq $version) 'restored descriptor identity wrong'
  Assert ($rd.compatibility.hostRequirements.t4Component -eq 'Microsoft.VisualStudio.Component.TextTemplating') 'host requirements missing from the restored descriptor'
  $restoredIds = Get-ChildItem $packagesFolder -Directory | Select-Object -ExpandProperty Name
  foreach ($e in $rd.templates.registry) {
    $pkgDir = Join-Path $packagesFolder ($e.packageId.ToLowerInvariant() + "/$version")
    if (-not (Test-Path $pkgDir)) { continue }   # package not needed by this consumer
    Assert (Test-Path (Join-Path $pkgDir $e.inPackagePath)) "registry entry $($e.id) does not resolve in $($e.packageId)"
  }
  Assert (Test-Path (Join-Path $packagesFolder "lazy.crud.builder.application.dto/$version/lib/net10.0/Lazy.Crud.Builder.Application.DTO.xml")) 'attribute XML documentation not restored'
  $xml = [xml](Get-Content -Raw (Join-Path $packagesFolder "lazy.crud.builder.application.dto/$version/lib/net10.0/Lazy.Crud.Builder.Application.DTO.xml"))
  foreach ($a in $rd.attributes) {
    $member = $xml.doc.members.member | Where-Object { $_.name -eq $a.documentationId }
    Assert ($null -ne $member -and -not [string]::IsNullOrWhiteSpace($member.summary) -and -not [string]::IsNullOrWhiteSpace($member.category)) "documentationId does not resolve to summary and category: $($a.documentationId)"
  }
  $b = Invoke-Proc 'dotnet' @('build', (Join-Path $sbx 'Sandbox.sln'), '-c', 'Debug', '--nologo', '-v', 'q') $sbx 1200 'build-C1'
  Assert ($b.ExitCode -eq 0) "clean-room build failed (log $($b.LogPath))"
  Assert ($g.Report.Toolchain.resolved -eq $true) 'clean-room run did not record the resolved toolchain'
}

# ==================================================================================================
# Runner
# ==================================================================================================
$summary = [ordered]@{
  runId = $stamp; startedUtc = (Get-Date).ToUniversalTime().ToString('o'); repository = $repo; platformVersion = $version; contractVersion = 1
  configuration = $Configuration; sandboxRoot = $sandboxRoot; toolchain = $null; packageVersions = @{}; scenarios = @(); passed = 0; failed = 0; skipped = 0; exitCode = 0; verdict = ''
}
$results = New-Object System.Collections.ArrayList
$treeBefore = Get-WorkingTreeState

function Finish([int]$code) {
  $summary.passed = @($results | Where-Object { $_.status -eq 'passed' }).Count
  $summary.failed = @($results | Where-Object { $_.status -in @('failed', 'timeout') }).Count
  $summary.skipped = @($results | Where-Object { $_.status -eq 'skipped' }).Count
  $summary.scenarios = @($results)
  $summary.exitCode = $code
  $summary.exitCodeName = @{0='Success';1='AssertionFailure';3='PrerequisiteUnavailable';4='InvalidFixtureOrSandbox';5='Timeout';6='ContractMismatch';7='OwnershipConflict';8='HarnessFailure'}[$code]
  $summary.finishedUtc = (Get-Date).ToUniversalTime().ToString('o')
  $summary.verdict = if ($code -eq 0) { "RESULT: OK (passed=$($summary.passed) failed=$($summary.failed))" } else { "RESULT: FAIL (passed=$($summary.passed) failed=$($summary.failed) exit=$code $($summary.exitCodeName))" }
  $summary | ConvertTo-Json -Depth 8 | Set-Content -Path (Join-Path $runDir 'summary.json')
  $digest = @()
  $digest += "Lazy CRUD E2E run $stamp  platform $version  contract 1  configuration $Configuration"
  $digest += "toolchain: " + ($(if ($summary.toolchain) { "$($summary.toolchain.displayName) $($summary.toolchain.installationVersion) [$($summary.toolchain.channel)] T4: $($summary.toolchain.textTransformPath)" } else { 'unresolved' }))
  $digest += ''
  foreach ($s in $results) { $digest += ("{0,-7} {1,-4} {2,8:F1}s  {3}{4}" -f $s.status.ToUpper(), $s.id, $s.durationSeconds, $s.title, $(if ($s.failureReason) { "  -> " + $s.failureReason } else { '' })) }
  $digest += ''
  $digest += "sandboxes: $sandboxRoot"
  $digest += "evidence: $runDir"
  $digest += $summary.verdict
  Set-Content -Path (Join-Path $runDir 'digest.txt') -Value ($digest -join "`n")
  Write-Host ''
  Write-Host ($digest -join "`n")
  exit $code
}

try {
  Log "run directory: $runDir"
  # ---- prerequisites: never skipped, never faked -------------------------------------------------
  $sdk = Invoke-Proc 'dotnet' @('--version') $repo 60 'dotnet-version'
  if ($sdk.ExitCode -ne 0) { Log 'PREREQUISITE: .NET SDK unavailable'; Finish 3 }
  Log "dotnet SDK $($sdk.Output.Split("`n")[0].Trim())"

  if (-not (Test-Path $tool)) {
    Log 'building the Platform tool'
    $b = Invoke-Proc 'dotnet' @('build', 'LazyCrudBuilder.sln', '-c', $Configuration, '--nologo', '-v', 'q') $repo 1200 'build-solution'
    if ($b.ExitCode -ne 0) { Log "harness failure: solution build failed (log $($b.LogPath))"; Finish 8 }
  }
  $tc = Invoke-Tool @('toolchain') 'prereq-toolchain' 120
  if ($tc.ExitCode -ne 0) {
    Log 'PREREQUISITE: the Visual Studio T4 toolchain could not be resolved. Generation is not skipped and not faked.'
    Log ($tc.Output.Split("`n") | Select-Object -Last 6) -join "`n"
    Finish 3
  }
  $summary.toolchain = Read-JsonOutput $tc
  Log "toolchain: $($summary.toolchain.displayName) $($summary.toolchain.installationVersion) [$($summary.toolchain.channel)] $($summary.toolchain.textTransformPath)"
  # Load a copy: Add-Type keeps the file open for the life of the process, and the harness packs
  # the solution afterwards, which would fail to overwrite the locked build output.
  $platformDllCopy = Join-Path $runDir 'Lazy.Crud.Builder.Platform.dll'
  Copy-Item -Path $platformDll -Destination $platformDllCopy -Force
  Add-Type -Path $platformDllCopy

  # ---- pack public-equivalent artifacts --------------------------------------------------------
  if (-not $SkipPack -or -not (Test-Path $feed)) {
    Log "packing version $version (public-equivalent artifacts)"
    $p = Invoke-Proc 'pwsh' @('-NoProfile', '-File', (Join-Path $repo 'pack-all.ps1')) $repo 1800 'pack-all'
    if ($p.ExitCode -ne 0) { Log "harness failure: pack-all.ps1 failed (log $($p.LogPath))"; Finish 8 }
  } else { Log "reusing packages in $feed" }

  # NuGet caches an extracted package by id and version, so a rebuilt package of the same version is
  # shadowed by the previous extraction. The harness validates the artifacts it just produced, so the
  # extracted copies of every Lazy CRUD package at the versions under test are removed first.
  $globalPackages = Join-Path $env:USERPROFILE '.nuget/packages'
  if (Test-Path $globalPackages) {
    $purged = 0
    foreach ($v in @($version, $mixedVersion)) {
      foreach ($dir in Get-ChildItem $globalPackages -Directory -Filter 'lazy.crud.*' -ErrorAction SilentlyContinue) {
        $extracted = Join-Path $dir.FullName $v
        if (Test-Path $extracted) { Remove-Item -Recurse -Force $extracted -ErrorAction SilentlyContinue; $purged++ }
      }
    }
    Log "purged $purged extracted package folder(s) of the versions under test from the global package cache"
  }
  if (-not $SkipMixedVersionPack -and (-not $Scenario -or $Scenario -eq 'R3')) {
    Log "packing second version $mixedVersion for the mixed-version scenario"
    $p2 = Invoke-Proc 'pwsh' @('-NoProfile', '-File', (Join-Path $repo 'pack-all.ps1'), '-Version', $mixedVersion, '-SkipValidation') $repo 1800 'pack-mixed'
    if ($p2.ExitCode -ne 0) { Log "harness failure: pack of $mixedVersion failed (log $($p2.LogPath))"; Finish 8 }
    # Rebuild the current version so the tool, descriptor and fixtures reflect LazyCrudVersion again.
    $p3 = Invoke-Proc 'dotnet' @('build', 'LazyCrudBuilder.sln', '-c', $Configuration, '--nologo', '-v', 'q') $repo 1200 'rebuild-current'
    if ($p3.ExitCode -ne 0) { Log 'harness failure: rebuild after mixed pack failed'; Finish 8 }
  }
  $summary.packageVersions = @{ platform = $version; mixed = $mixedVersion; legacy = '1.0.35' }
  foreach ($n in (Get-ChildItem $feed -Filter '*.nupkg' | Select-Object -ExpandProperty Name)) { Add-Content -Path (Join-Path $runDir 'packages.txt') -Value $n }

  # ---- scenarios ----------------------------------------------------------------------------------
  $selected = if ($Scenario) { @($scenarios | Where-Object { $_.Id -eq $Scenario }) } else { @($scenarios) }
  if ($Scenario -and $selected.Count -eq 0) { Log "unknown scenario '$Scenario'"; Finish 2 }
  $failureExit = 0
  foreach ($s in $selected) {
    if ((Get-Date) -gt $globalDeadline) { [void]$results.Add(@{ id = $s.Id; title = $s.Title; status = 'timeout'; durationSeconds = 0; failureReason = 'global timeout budget exceeded'; intentionalFailure = $s.IntentionalFailure }); if ($failureExit -eq 0) { $failureExit = 5 }; continue }
    Log "=== $($s.Id): $($s.Title)$(if ($s.IntentionalFailure) { ' [intentional failure]' })"
    $sw = [System.Diagnostics.Stopwatch]::StartNew()
    $entry = @{ id = $s.Id; title = $s.Title; status = 'passed'; durationSeconds = 0.0; failureReason = $null; intentionalFailure = $s.IntentionalFailure }
    try {
      & $s.Action
      Log "--- $($s.Id): PASS ($([Math]::Round($sw.Elapsed.TotalSeconds, 1))s)"
    }
    catch [System.TimeoutException] {
      $entry.status = 'timeout'; $entry.failureReason = $_.Exception.Message; if ($failureExit -eq 0) { $failureExit = 5 }
      Log "--- $($s.Id): TIMEOUT $($_.Exception.Message)"
    }
    catch [System.IO.InvalidDataException] {
      $entry.status = 'failed'; $entry.failureReason = $_.Exception.Message; if ($failureExit -eq 0) { $failureExit = 4 }
      Log "--- $($s.Id): FAIL (sandbox) $($_.Exception.Message)"
    }
    catch {
      $entry.status = 'failed'; $entry.failureReason = $_.Exception.Message; if ($failureExit -eq 0) { $failureExit = 1 }
      Log "--- $($s.Id): FAIL $($_.Exception.Message)"
      if ($_.ScriptStackTrace) { Add-Content -Path (Join-Path $runDir 'harness.log') -Value $_.ScriptStackTrace }
    }
    $entry.durationSeconds = [Math]::Round($sw.Elapsed.TotalSeconds, 1)
    [void]$results.Add($entry)
  }

  # ---- repository unchanged apart from artifacts ---------------------------------------------------
  $treeAfter = Get-WorkingTreeState
  if ($null -eq $treeBefore -or $null -eq $treeAfter) {
    Log 'TREE: the working tree could not be read with git; the check is reported as skipped, not as a pass'
    [void]$results.Add(@{ id = 'TREE'; title = 'Builder working tree unchanged apart from artifacts/'; status = 'skipped'; durationSeconds = 0; failureReason = 'git status unavailable'; intentionalFailure = $false })
  }
  elseif ($treeAfter -ne $treeBefore) {
    Log 'FAIL: the Builder working tree changed during the run (apart from artifacts/)'
    Log $treeAfter
    if ($failureExit -eq 0) { $failureExit = 1 }
    [void]$results.Add(@{ id = 'TREE'; title = 'Builder working tree unchanged apart from artifacts/'; status = 'failed'; durationSeconds = 0; failureReason = 'working tree changed'; intentionalFailure = $false })
  } else {
    [void]$results.Add(@{ id = 'TREE'; title = 'Builder working tree unchanged apart from artifacts/'; status = 'passed'; durationSeconds = 0; failureReason = $null; intentionalFailure = $false })
  }
  Finish $failureExit
}
catch {
  Log "HARNESS FAILURE: $($_.Exception.Message)"
  Log $_.ScriptStackTrace
  Finish 8
}
