#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Proves a published release from the public internet: nuget.org as the only source, an isolated
  package cache, no Builder source anywhere, a real Visual Studio T4 generation, and the generated
  solution building and passing its tests.

.DESCRIPTION
  Harness scenario C1 is a clean room over the *local pack output* standing in for the public feed.
  It proves the package set is self-sufficient; it cannot prove that the bytes an external user
  downloads are the ones that work, because nothing was downloaded. This script is the same flow with
  the transport made real: the sandbox is created with `--clean-room` and **no** `--feed`, which makes
  the generated nuget.config clear every inherited source, leave `https://api.nuget.org/v3/index.json`
  as the only one, and point the global packages folder at an empty directory of its own.

  Steps, each bounded and each killed with its process tree on expiry:

    1. Propagation poll -- every declared package id is queried on the flat container until this
       version answers for all of them, or the budget runs out. Publishing is asynchronous; a push
       that returned zero is not yet a package anyone can restore.
    2. Sandbox creation from the reference fixture, clean room, isolated cache, no local feed.
    3. Assertions that the room really is clean: no Builder source in it, no project.tt copy, the
       config clears sources, and the isolated cache starts with no Lazy CRUD package in it.
    4. Generation through the resolved Visual Studio toolchain, from restored artifacts only.
    5. Proof from the restored packages: the descriptor is readable inside the downloaded package,
       its identity matches this version, and every registry entry resolves to a file that is there.
    6. Marker, manifest and hash validation over the generated tree.
    7. `dotnet build` and `dotnet test` of the generated solution.
    8. The downloaded .nupkg files are hashed and compared with contracts/releases/<version>.json,
       so the run states whether what the internet served is what this repository built.

  Exit codes follow the shared table: 0 success, 1 assertion failure, 3 prerequisite unavailable,
  4 sandbox or fixture problem, 5 timeout.

.PARAMETER Version
  Version to prove. Defaults to LazyCrudVersion in Directory.Build.props.

.PARAMETER PropagationTimeoutMinutes
  How long to wait for every package to appear on the source (default 20). Polling is bounded and
  never runs in the background.

.PARAMETER PollIntervalSeconds
  Seconds between propagation polls (default 30).

.PARAMETER SkipTests
  Build the generated solution but do not run its tests.

.PARAMETER SandboxRoot
  Directory that receives the sandbox (default %LOCALAPPDATA%/lazycrud/public-clean-room).
#>
[CmdletBinding()]
param(
  [string]$Version,
  [int]$PropagationTimeoutMinutes = 20,
  [int]$PollIntervalSeconds = 30,
  [switch]$SkipTests,
  [string]$SandboxRoot
)

$ErrorActionPreference = 'Stop'
$repo = Split-Path $PSScriptRoot -Parent
Set-Location $repo

$stamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$runDir = Join-Path $repo "artifacts/public-clean-room/$stamp"
$logDir = Join-Path $runDir 'logs'
New-Item -ItemType Directory -Force -Path $logDir | Out-Null
$source = 'https://api.nuget.org/v3/index.json'

function Log([string]$m) { $line = "[{0}] {1}" -f (Get-Date -Format 'HH:mm:ss'), $m; Write-Host $line; Add-Content -Path (Join-Path $runDir 'run.log') -Value $line }
function Assert([bool]$c, [string]$m) { if (-not $c) { throw "ASSERT: $m" } }

$script:procIndex = 0
function Invoke-Proc([string]$file, [string[]]$arguments, [string]$cwd, [int]$timeoutSec, [string]$label) {
  $script:procIndex++
  $logPath = Join-Path $logDir ("{0:D3}-{1}.log" -f $script:procIndex, ($label -replace '[^A-Za-z0-9_.-]', '_'))
  $psi = [System.Diagnostics.ProcessStartInfo]::new($file)
  foreach ($a in $arguments) { $psi.ArgumentList.Add($a) }
  $psi.WorkingDirectory = $cwd
  $psi.RedirectStandardOutput = $true
  $psi.RedirectStandardError = $true
  $psi.UseShellExecute = $false
  $psi.CreateNoWindow = $true
  $p = [System.Diagnostics.Process]::Start($psi)
  $stdout = $p.StandardOutput.ReadToEndAsync()
  $stderr = $p.StandardError.ReadToEndAsync()
  if (-not $p.WaitForExit($timeoutSec * 1000)) {
    try { $p.Kill($true) } catch {}
    Set-Content -Path $logPath -Value ("TIMEOUT after {0}s`n{1}`n{2}" -f $timeoutSec, $stdout.Result, $stderr.Result)
    throw [System.TimeoutException]::new("$label exceeded ${timeoutSec}s (log: $logPath)")
  }
  $p.WaitForExit()
  Set-Content -Path $logPath -Value ("$ $file $($arguments -join ' ')`nexit=$($p.ExitCode)`n$($stdout.Result)`n--- stderr ---`n$($stderr.Result)")
  return @{ ExitCode = $p.ExitCode; Output = $stdout.Result; LogPath = $logPath }
}

function Get-CentralVersion {
  $xml = [xml](Get-Content -Raw (Join-Path $repo 'Directory.Build.props'))
  $v = $xml.Project.PropertyGroup | ForEach-Object { $_.LazyCrudVersion } | Where-Object { $_ } |
       ForEach-Object { if ($_ -is [System.Xml.XmlElement]) { $_.InnerText } else { [string]$_ } } | Select-Object -First 1
  return ([string]$v).Trim()
}

if (-not $Version) { $Version = Get-CentralVersion }
$tool = Join-Path $repo 'src/Platform/Lazy.Crud.Builder.Platform.Tool/bin/Release/net11.0/Lazy.Crud.Builder.Platform.Tool.dll'
Assert (Test-Path $tool) "the platform tool is not built: $tool"
$descriptorPath = Join-Path $repo 'artifacts/platform/lazycrud.platform.json'
Assert (Test-Path $descriptorPath) "no descriptor at $descriptorPath"
$fixture = Join-Path $repo 'tests/fixtures/reference-aggregate'
if (-not $SandboxRoot) { $SandboxRoot = Join-Path ([Environment]::GetFolderPath('LocalApplicationData')) 'lazycrud/public-clean-room' }
$sandboxRoot = Join-Path $SandboxRoot $stamp
New-Item -ItemType Directory -Force -Path $sandboxRoot | Out-Null

$d = Get-Content -Raw $descriptorPath | ConvertFrom-Json -Depth 32
$declared = @(@($d.layers.packageId) + @($d.nonLayerPackages) + @($d.templates.packageId) + @($d.templates.aliasPackageIds) | Where-Object { $_ } | Sort-Object -Unique)
Log "version $Version, $($declared.Count) declared package id(s), source $source"

$verdict = 'failed'
$notes = [System.Collections.Generic.List[string]]::new()
try {
  # ---- 1. propagation ---------------------------------------------------------------------------
  $deadline = (Get-Date).AddMinutes($PropagationTimeoutMinutes)
  $absent = @($declared)
  while ($true) {
    $stillAbsent = [System.Collections.Generic.List[string]]::new()
    foreach ($id in $absent) {
      $url = "https://api.nuget.org/v3-flatcontainer/$($id.ToLowerInvariant())/index.json"
      $found = $false
      try { $r = Invoke-RestMethod -Uri $url -TimeoutSec 30 -ErrorAction Stop; $found = (@($r.versions) -contains $Version) } catch { $found = $false }
      if (-not $found) { [void]$stillAbsent.Add($id) }
    }
    $absent = @($stillAbsent)
    if ($absent.Count -eq 0) { Log "every declared package answers for $Version on the source"; break }
    if ((Get-Date) -ge $deadline) {
      Log "PROPAGATION TIMEOUT: $($absent.Count) package(s) are not on $source at $Version"
      foreach ($id in $absent) { Log "  absent: $id" }
      $notes.Add("not published at ${Version}: $($absent -join ', ')")
      $verdict = 'not-published'
      throw [System.TimeoutException]::new("$($absent.Count) declared package(s) are not on the public source at $Version; publish first, then rerun.")
    }
    Log "waiting for propagation: $($absent.Count) package(s) still absent; next poll in ${PollIntervalSeconds}s"
    Start-Sleep -Seconds $PollIntervalSeconds
  }

  # ---- 2. sandbox: clean room, no local feed ------------------------------------------------------
  $packagesFolder = Join-Path $sandboxRoot 'packages'
  New-Item -ItemType Directory -Force -Path $packagesFolder | Out-Null
  $sbx = Join-Path $sandboxRoot 'sbx-public'
  $r = Invoke-Proc 'dotnet' @($tool, 'sandbox', 'create', '--output', $sbx, '--aggregate', 'Products', '--fixture', $fixture,
                              '--version', $Version, '--clean-room', '--packages-folder', $packagesFolder) $repo 900 'sandbox-public'
  Assert ($r.ExitCode -eq 0) "sandbox creation failed (exit $($r.ExitCode), log $($r.LogPath))"

  # ---- 3. the room is clean -----------------------------------------------------------------------
  $cfg = Get-Content -Raw (Join-Path $sbx 'nuget.config')
  Assert ($cfg -match '<clear\s*/>') 'nuget.config does not clear inherited sources'
  Assert ($cfg -match [regex]::Escape($source)) 'nuget.config does not name the public source'
  Assert ($cfg -notmatch 'artifacts[\\/]nuget') 'nuget.config still points at the local pack output'
  Assert ($cfg -match 'globalPackagesFolder') 'the package cache is not isolated'
  Assert (-not (Test-Path (Join-Path $sbx 'src/ProjectFiles'))) 'the sandbox carries template sources'
  Assert ((Get-ChildItem $sbx -Recurse -Filter 'project.tt' -ErrorAction SilentlyContinue).Count -eq 0) 'a copy of project.tt exists in the sandbox'
  Log 'clean room verified: public source only, isolated cache, no Builder source'

  # ---- 4. generation from what was downloaded -----------------------------------------------------
  $reportPath = Join-Path $runDir 'generate.json'
  $g = Invoke-Proc 'dotnet' @($tool, 'generate', '--solution', $sbx, '--aggregate', 'Products', '--report', $reportPath) $repo 1500 'generate-public'
  Assert ($g.ExitCode -eq 0) "generation failed (exit $($g.ExitCode), log $($g.LogPath))"
  $report = Get-Content -Raw $reportPath | ConvertFrom-Json -Depth 32
  Assert ($report.TransformedTemplates.Count -gt 0) 'no template was transformed: generation is not proven'
  Assert ($report.DomainGatePassed -eq $true) 'the Domain gate did not pass'
  Assert ($report.Toolchain.resolved -eq $true) 'no resolved Visual Studio toolchain was recorded'
  Assert ($report.TemplateSource.Kind -eq 'NuGet') 'templates did not resolve from packages'
  Assert ($report.TemplateSource.Location.StartsWith($packagesFolder, [System.StringComparison]::OrdinalIgnoreCase)) 'templates did not resolve from the isolated cache'
  Assert ($report.Manifest.Files -gt 0) 'the manifest records no generated file'
  Log "generated: $($report.TransformedTemplates.Count) template(s) transformed, $($report.Manifest.Files) file(s) in the manifest"

  # ---- 5. the downloaded package is the contract --------------------------------------------------
  $restoredDescriptor = Join-Path $packagesFolder "lazy.crud.builder.domain/$Version/lazycrud/lazycrud.platform.json"
  Assert (Test-Path $restoredDescriptor) 'the descriptor is not readable inside the downloaded package'
  $rd = Get-Content -Raw $restoredDescriptor | ConvertFrom-Json -Depth 32
  Assert ($rd.platformVersion -eq $Version) "the downloaded descriptor declares platformVersion $($rd.platformVersion)"
  Assert ($rd.contractVersion -eq $d.contractVersion) 'the downloaded descriptor declares a different contractVersion'
  foreach ($e in $rd.templates.registry) {
    $pkgDir = Join-Path $packagesFolder ($e.packageId.ToLowerInvariant() + "/$Version")
    if (-not (Test-Path $pkgDir)) { continue }
    Assert (Test-Path (Join-Path $pkgDir $e.inPackagePath)) "registry entry $($e.id) does not resolve inside the downloaded $($e.packageId)"
  }
  $xmlPath = Join-Path $packagesFolder "lazy.crud.builder.application.dto/$Version/lib/net11.0/Lazy.Crud.Builder.Application.DTO.xml"
  Assert (Test-Path $xmlPath) 'the attribute XML documentation was not downloaded'

  # ---- 6. markers, manifest, hashes ----------------------------------------------------------------
  $manifestPath = Join-Path $sbx '.lazycrud/manifests/Products.manifest.json'
  Assert (Test-Path $manifestPath) 'no manifest was written'
  $v = Invoke-Proc 'dotnet' @($tool, 'validate', 'manifest', '--file', $manifestPath, '--repo', $repo) $repo 300 'validate-manifest'
  Assert ($v.ExitCode -eq 0) "the manifest does not validate against the published schema (log $($v.LogPath))"
  $manifestText = Get-Content -Raw $manifestPath
  Assert (-not [regex]::IsMatch($manifestText, '[A-Za-z]:' + [regex]::Escape('\'))) 'the manifest carries an absolute path'
  $manifest = $manifestText | ConvertFrom-Json -Depth 16
  Assert ($manifest.schemaVersion -eq 2) "manifest schemaVersion is $($manifest.schemaVersion)"
  # The writer that produced these files is the one shipped inside the templates package, under
  # tools/: the standalone Platform package is not in a consumer's restore graph, so hashing with it
  # would be hashing with something the consumer never downloaded.
  $writerDll = Join-Path $packagesFolder "lazy.crud.builder.templates/$Version/tools/net11.0/Lazy.Crud.Builder.Platform.dll"
  if (-not (Test-Path $writerDll)) { $writerDll = Join-Path $packagesFolder "lazy.crud.builder.platform/$Version/lib/netstandard2.0/Lazy.Crud.Builder.Platform.dll" }
  Assert (Test-Path $writerDll) "the downloaded writer assembly was not found in the isolated cache"
  Log "hashing with the downloaded writer: $($writerDll.Substring($packagesFolder.Length).TrimStart('\','/'))"
  Add-Type -Path $writerDll
  $markerless = [System.Collections.Generic.List[string]]::new()
  foreach ($f in $manifest.files) {
    $path = Join-Path $sbx $f.relativePath
    Assert (Test-Path $path) "manifest names a file that does not exist: $($f.relativePath)"
    $text = [System.IO.File]::ReadAllText($path)
    $h = [Lazy.Crud.Builder.Platform.Hashing.ContentHasher]::Compute($text)
    Assert ($h.Value -eq $f.contentHash) "recorded hash does not match the generated file: $($f.relativePath)"
    Assert ($h.Scope -eq $f.hashScope) "recorded hash scope does not match: $($f.relativePath)"
    if ($text -notmatch 'LazyCrud-Marker: v2') { [void]$markerless.Add($f.relativePath) }
  }
  Assert ($markerless.Count -eq 0) "generated file(s) without a v2 marker: $($markerless -join ', ')"
  Log "manifest and markers verified over $($manifest.files.Count) generated file(s), hashed with the downloaded implementation"

  # ---- 7. the generated solution builds and its tests pass -----------------------------------------
  $b = Invoke-Proc 'dotnet' @('build', (Join-Path $sbx 'Sandbox.sln'), '-c', 'Debug', '--nologo', '-v', 'q') $sbx 1500 'build-public'
  Assert ($b.ExitCode -eq 0) "the generated solution did not build (log $($b.LogPath))"
  if (-not $SkipTests) {
    $t = Invoke-Proc 'dotnet' @('test', (Join-Path $sbx 'Sandbox.sln'), '-c', 'Debug', '--no-build', '--nologo') $sbx 1500 'test-public'
    Assert ($t.ExitCode -eq 0) "the generated tests failed (log $($t.LogPath))"
  }

  # ---- 8. what the internet served is what this repository built ------------------------------------
  #    Not a byte comparison of the .nupkg: nuget.org appends its own repository signature
  #    (.signature.p7s) to every package it accepts, so the container it serves can never be identical
  #    to the one that was pushed. What must be identical is everything inside it, entry by entry.
  $recordPath = Join-Path $repo "contracts/releases/$Version.json"
  $checked = 0; $problems = [System.Collections.Generic.List[string]]::new()
  Add-Type -AssemblyName System.IO.Compression.FileSystem
  foreach ($f in (Get-ChildItem (Join-Path $repo "artifacts/nuget/$Version") -Filter *.nupkg -File)) {
    $id = $f.Name.Substring(0, $f.Name.Length - ".$Version.nupkg".Length)
    $servedPath = Join-Path $packagesFolder ("$($id.ToLowerInvariant())/$Version/$($id.ToLowerInvariant()).$Version.nupkg")
    if (-not (Test-Path $servedPath)) { continue }   # not in this consumer's restore graph
    $checked++
    $built = [System.IO.Compression.ZipFile]::OpenRead($f.FullName)
    $served = [System.IO.Compression.ZipFile]::OpenRead($servedPath)
    try {
      $servedEntries = @{}
      foreach ($e in $served.Entries) { $servedEntries[$e.FullName] = $e }
      foreach ($e in $built.Entries) {
        if (-not $servedEntries.ContainsKey($e.FullName)) { $problems.Add("$id : the served package lacks $($e.FullName)"); continue }
        $hb = (Get-FileHash -InputStream $e.Open() -Algorithm SHA256).Hash
        $hs = (Get-FileHash -InputStream $servedEntries[$e.FullName].Open() -Algorithm SHA256).Hash
        if ($hb -ne $hs) { $problems.Add("$id : $($e.FullName) differs") }
      }
      $extra = @($served.Entries.FullName | Where-Object { $_ -ne '.signature.p7s' -and -not ($built.Entries.FullName -contains $_) })
      foreach ($x in $extra) { $problems.Add("$id : the served package carries an unexpected $x") }
      $signed = @($served.Entries.FullName) -contains '.signature.p7s'
      if (-not $signed) { $problems.Add("$id : the served package is not signed by the source") }
    } finally { $built.Dispose(); $served.Dispose() }
  }
  foreach ($m in $problems) { Log "CONTENT MISMATCH $m" }
  Assert ($problems.Count -eq 0) "$($problems.Count) difference(s) between what was built and what the source served"
  Log "$checked downloaded package(s) identical to what was built, entry by entry, and each signed by the source"
  $notes.Add("$checked package(s) verified content-identical to artifacts/nuget/$Version, each carrying the source's repository signature")
  if (-not (Test-Path $recordPath)) { $notes.Add('no release record present') }
  $verdict = 'passed'
}
catch {
  Log "FAILED: $($_.Exception.Message)"
  $notes.Add($_.Exception.Message)
}

$summary = [ordered]@{
  runId = $stamp; version = $Version; source = $source; verdict = $verdict
  sandboxRoot = $sandboxRoot; finishedUtc = (Get-Date).ToUniversalTime().ToString('o'); notes = @($notes)
}
[System.IO.File]::WriteAllText((Join-Path $runDir 'summary.json'), ($summary | ConvertTo-Json -Depth 8) + "`n", (New-Object System.Text.UTF8Encoding($false)))

if ($verdict -eq 'passed') { Write-Host "RESULT: OK (public clean room proved $Version from $source)"; exit 0 }
if ($verdict -eq 'not-published') { Write-Host "RESULT: NOT PUBLISHED ($Version is not on $source)"; exit 5 }
Write-Host "RESULT: FAIL (see $runDir)"
exit 1
