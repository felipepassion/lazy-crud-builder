#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Regenerates the reference aggregate (Products) through the real Visual Studio T4 toolchain in a
  temporary sandbox and compares the output with tests/golden/Products (or re-blesses it).

.DESCRIPTION
  This is the golden-output regression test. It uses the working-tree templates (local template
  development mode pointing at src/ProjectFiles) so every generator change is measured against the
  committed golden files. Header regions (leading comment lines) are compared separately from
  bodies: a header-only difference is reported as such, any body difference fails.

.PARAMETER Capture
  Re-bless: replace tests/golden/Products with the freshly generated output.

.PARAMETER PackageVersion
  Version of the Lazy CRUD runtime packages the sandbox restores (default: LazyCrudVersion).

.PARAMETER Feed
  Local folder feed with the runtime packages (default: artifacts/nuget/<PackageVersion>).

.PARAMETER NoTemplatesPackage
  Do not reference Lazy.Crud.Builder.Templates from the sandbox (legacy package sets).

.PARAMETER Configuration
  Build configuration of the Platform tool and library to use (default: Release).

.EXAMPLE
  pwsh scripts/golden.ps1
  pwsh scripts/golden.ps1 -Capture
#>
[CmdletBinding()]
param(
  [switch]$Capture,
  [string]$PackageVersion,
  [string]$Feed,
  [switch]$NoTemplatesPackage,
  [string]$Configuration = 'Release',
  [string]$Sandbox
)

$ErrorActionPreference = 'Stop'
$repo = Split-Path $PSScriptRoot -Parent
Set-Location $repo

function Get-CentralVersion {
  $xml = [xml](Get-Content -Raw (Join-Path $repo 'Directory.Build.props'))
  $v = $xml.Project.PropertyGroup | ForEach-Object { $_.LazyCrudVersion } | Where-Object { $_ } | ForEach-Object { if ($_ -is [System.Xml.XmlElement]) { $_.InnerText } else { [string]$_ } } | Select-Object -First 1
  if (-not $v) { throw 'LazyCrudVersion not found in Directory.Build.props' }
  return ([string]$v).Trim()
}

if (-not $PackageVersion) { $PackageVersion = Get-CentralVersion }
if (-not $Feed) { $Feed = Join-Path $repo "artifacts/nuget/$PackageVersion" }
if (-not (Test-Path $Feed)) { throw "package feed not found: $Feed (run pack-all.ps1 first, or pass -Feed)" }

$tool = Join-Path $repo "src/Platform/Lazy.Crud.Builder.Platform.Tool/bin/$Configuration/net10.0/Lazy.Crud.Builder.Platform.Tool.dll"
if (-not (Test-Path $tool)) { throw "Platform tool not built: $tool (dotnet build LazyCrudBuilder.sln -c $Configuration)" }
$platformAssembly = Join-Path $repo "src/Platform/Lazy.Crud.Builder.Platform/bin/$Configuration/netstandard2.0/Lazy.Crud.Builder.Platform.dll"
$templates = Join-Path $repo 'src/ProjectFiles'
$golden = Join-Path $repo 'tests/golden/Products'
# Not under %TEMP%: project.tt deletes any wrapper under a directory named "Temp" after writing.
if (-not $Sandbox) { $Sandbox = Join-Path ([Environment]::GetFolderPath('LocalApplicationData')) ("lazycrud/golden/" + (Get-Date -Format 'yyyyMMdd-HHmmss')) }

Write-Host "[golden] sandbox: $Sandbox" -ForegroundColor Cyan
$sandboxArgs = @('sandbox', 'create', '--output', $Sandbox, '--aggregate', 'Products', '--fixture', (Join-Path $repo 'tests/fixtures/reference-aggregate'),
  '--version', $PackageVersion, '--feed', $Feed, '--templates', "local:$templates", '--platform-assembly', $platformAssembly)
if ($NoTemplatesPackage) { $sandboxArgs += '--no-templates-package' }
& dotnet $tool @sandboxArgs | Out-Null
if ($LASTEXITCODE -ne 0) { Write-Error "sandbox creation failed ($LASTEXITCODE)"; exit 4 }

Write-Host '[golden] generating through the Visual Studio T4 toolchain...' -ForegroundColor Cyan
$report = Join-Path $Sandbox 'generate-report.json'
& dotnet $tool generate --solution $Sandbox --aggregate Products --templates "local:$templates" --report $report | Out-Null
$gen = $LASTEXITCODE
if ($gen -ne 0) { Write-Error "generation failed with exit code $gen (see $report)"; exit $gen }

# Collect generated files: everything the templates wrote under the aggregate (excluding project files, wrappers and build output).
$aggRoot = Join-Path $Sandbox 'src/Products'
$files = Get-ChildItem -Path $aggRoot -Recurse -File | Where-Object {
  $_.FullName -notmatch '\\(bin|obj)\\' -and $_.Extension -in @('.cs', '.bat', '.razor', '.css') -and
  $_.Name -notmatch '\.generated\.(cs|bat)$' -and $_.Name -ne 'SmokeTest.cs' -and $_.Name -ne 'Program.cs' -and
  $_.FullName -notmatch '\\Aggregates\\ProductsAgg\\Entities\\(Product|ProductCategory)\.cs$' -and $_.Name -ne 'ProductsEnumerations.cs'
}

function Normalize([string]$text, [string]$name) {
  $t = $text -replace "`r`n", "`n"
  if ($name -like 'ProductsAgg.cs' -or $name -like '*.bat') {
    # MiniMigrations.tt embeds DateTime.Now: mask the timestamp token.
    $t = [regex]::Replace($t, 'migrations add \d{4}_\d{1,2}_\d{1,2}_\d{1,2}_\d{1,2}_\d{1,2}', 'migrations add {TIMESTAMP}')
  }
  return $t
}

function Split-Header([string]$normalized) {
  $lines = $normalized -split "`n"
  $i = 0
  while ($i -lt $lines.Count -and ($lines[$i].Trim() -eq '' -or $lines[$i].TrimStart().StartsWith('//') -or $lines[$i].TrimStart().StartsWith('/*') -or $lines[$i].TrimStart().StartsWith('*') -or $lines[$i].TrimStart().StartsWith('@*') -or $lines[$i].TrimStart().StartsWith('REM ') -or $lines[$i].TrimStart().StartsWith('<#/*') -or $lines[$i].TrimStart().StartsWith('*/#>'))) { $i++ }
  return @{ Header = ($lines[0..([Math]::Max($i - 1, 0))] -join "`n"); Body = (($lines | Select-Object -Skip $i) -join "`n") }
}

if ($Capture) {
  if (Test-Path $golden) { Remove-Item -Recurse -Force $golden }
  foreach ($f in $files) {
    $rel = $f.FullName.Substring($aggRoot.Length).TrimStart('\', '/')
    $dest = Join-Path $golden $rel
    New-Item -ItemType Directory -Force -Path (Split-Path $dest -Parent) | Out-Null
    [System.IO.File]::WriteAllText($dest, (Normalize ([System.IO.File]::ReadAllText($f.FullName)) $f.Name), [System.Text.UTF8Encoding]::new($false))
  }
  Write-Host "[golden] captured $($files.Count) file(s) into $golden" -ForegroundColor Green
  Write-Host 'Commit the golden files with an explicit reason in the commit message (see tests/golden/README.md).' -ForegroundColor Yellow
  exit 0
}

if (-not (Test-Path $golden)) { Write-Error "golden directory missing: $golden (run with -Capture to bless)"; exit 4 }
$expected = Get-ChildItem -Path $golden -Recurse -File
$bodyDiffs = @(); $headerDiffs = @(); $missing = @(); $extra = @()
$actualByRel = @{}
foreach ($f in $files) { $actualByRel[$f.FullName.Substring($aggRoot.Length).TrimStart('\', '/')] = $f }
foreach ($e in $expected) {
  $rel = $e.FullName.Substring($golden.Length).TrimStart('\', '/')
  if (-not $actualByRel.ContainsKey($rel)) { $missing += $rel; continue }
  $exp = Split-Header (Normalize ([System.IO.File]::ReadAllText($e.FullName)) $e.Name)
  $act = Split-Header (Normalize ([System.IO.File]::ReadAllText($actualByRel[$rel].FullName)) $e.Name)
  if ($exp.Body -ne $act.Body) { $bodyDiffs += $rel }
  elseif ($exp.Header -ne $act.Header) { $headerDiffs += $rel }
  $actualByRel.Remove($rel)
}
$extra = @($actualByRel.Keys)

Write-Host "[golden] compared $($expected.Count) golden file(s): body differences=$($bodyDiffs.Count) header-only differences=$($headerDiffs.Count) missing=$($missing.Count) unexpected=$($extra.Count)"
$headerDiffs | ForEach-Object { Write-Host "  header-only: $_" -ForegroundColor DarkYellow }
$bodyDiffs | ForEach-Object { Write-Host "  BODY DIFF:   $_" -ForegroundColor Red }
$missing | ForEach-Object { Write-Host "  MISSING:     $_" -ForegroundColor Red }
$extra | ForEach-Object { Write-Host "  UNEXPECTED:  $_" -ForegroundColor Red }
$summary = @{ compared = $expected.Count; bodyDifferences = $bodyDiffs; headerOnlyDifferences = $headerDiffs; missing = $missing; unexpected = $extra; sandbox = $Sandbox; report = $report } | ConvertTo-Json -Depth 4
Set-Content -Path (Join-Path $Sandbox 'golden-summary.json') -Value $summary
if ($bodyDiffs.Count -gt 0 -or $missing.Count -gt 0 -or $extra.Count -gt 0) { Write-Host 'RESULT: FAIL (golden output differs beyond header blocks)' -ForegroundColor Red; exit 1 }
Write-Host 'RESULT: OK (golden output unchanged apart from header blocks)' -ForegroundColor Green
exit 0
