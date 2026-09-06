<#
.SYNOPSIS
  Publishes every .nupkg of the most recent version folder under the artifacts root to a NuGet feed.

.DESCRIPTION
  Pushes every .nupkg of one version folder under the artifacts root. The version is the central
  one -- LazyCrudVersion in Directory.Build.props, the single source every package is built from --
  and not "the highest folder present": the E2E harness packs a second, higher version to exercise
  mixed-version restore, so picking the highest is how a test artifact reaches nuget.org, where
  nothing can be taken back. Pass -Version to publish something else deliberately.

  The API key is NEVER stored in the repository: pass -ApiKey explicitly or set the NUGET_API_KEY
  environment variable (the CI secret). The script fails with a clear message when neither is
  present.

.PARAMETER OutputRoot
  Artifacts root. Default: artifacts/nuget

.PARAMETER Version
  Version folder to publish. Defaults to LazyCrudVersion in Directory.Build.props.

.PARAMETER ApiKey
  NuGet API key. Falls back to $env:NUGET_API_KEY. Required for the push.

.PARAMETER Source
  Target feed. Default: https://api.nuget.org/v3/index.json

.PARAMETER NoSkipDuplicate
  Do not pass --skip-duplicate to dotnet nuget push.

.EXAMPLE
  $env:NUGET_API_KEY = '<key from your secret store>'
  pwsh publish-all.ps1
#>
[CmdletBinding()] param(
  [string]$OutputRoot = 'artifacts/nuget',
  [string]$Version,
  [string]$ApiKey,
  [string]$Source = 'https://api.nuget.org/v3/index.json',
  [switch]$NoSkipDuplicate
)

$ErrorActionPreference = 'Stop'

if (-not $ApiKey) { $ApiKey = $env:NUGET_API_KEY }
if (-not $ApiKey) {
  # Write-Error throws under ErrorActionPreference Stop, so the documented exit code never
  # ran: the message goes to the error stream by hand and the script exits 2 as declared.
  [Console]::Error.WriteLine('No NuGet API key: pass -ApiKey or set the NUGET_API_KEY environment variable. Credentials are never stored in this repository.')
  exit 2
}

if (!(Test-Path $OutputRoot)) {
  throw "Artifacts folder not found: $OutputRoot"
}

if (-not $Version) {
  $propsPath = Join-Path $PSScriptRoot 'Directory.Build.props'
  $xml = [xml](Get-Content -Raw $propsPath)
  $Version = ($xml.Project.PropertyGroup | ForEach-Object { $_.LazyCrudVersion } | Where-Object { $_ } |
              ForEach-Object { if ($_ -is [System.Xml.XmlElement]) { $_.InnerText } else { [string]$_ } } | Select-Object -First 1)
  if (-not $Version) { throw "LazyCrudVersion is not defined in $propsPath (the single version source)." }
  $Version = ([string]$Version).Trim()
  Write-Host "[INFO] Central version (LazyCrudVersion): $Version" -ForegroundColor Green
} else {
  Write-Host "[INFO] Version override: $Version" -ForegroundColor Yellow
}

$latestDir = Get-Item -Path (Join-Path $OutputRoot $Version) -ErrorAction SilentlyContinue
if (-not $latestDir) {
  $available = (Get-ChildItem -Path $OutputRoot -Directory -ErrorAction SilentlyContinue | Where-Object { $_.Name -match '^\d+\.\d+' } | Select-Object -ExpandProperty Name) -join ', '
  throw "No package folder for version ${Version} under ${OutputRoot} (present: ${available}). Run pack-all.ps1 first, or pass -Version deliberately."
}
Write-Host "[INFO] Folder: $($latestDir.FullName)" -ForegroundColor Cyan

$wrongVersion = @(Get-ChildItem -Path $latestDir.FullName -Filter *.nupkg -File | Where-Object { $_.Name -notmatch [regex]::Escape(".$Version.nupkg") + '$' })
if ($wrongVersion.Count -gt 0) {
  throw "The folder holds package(s) that are not version ${Version}: $(($wrongVersion | Select-Object -ExpandProperty Name) -join ', '). Repack before publishing."
}

$packages = Get-ChildItem -Path $latestDir.FullName -Filter *.nupkg -File |
  Where-Object { $_.Name -notmatch '\.symbols\.' }

if ($packages.Count -eq 0) {
  throw "No .nupkg found in: $($latestDir.FullName)"
}

Write-Host "[INFO] Packages found: $($packages.Count)" -ForegroundColor Cyan
$packages | ForEach-Object { Write-Host "  - $($_.Name)" }

$successCount = 0
$failCount    = 0
$failed       = @()

foreach ($pkg in $packages) {
  $printable = @('dotnet','nuget','push', "`"$($pkg.FullName)`"", '--api-key', '***', '--source', $Source) -join ' '
  Write-Host "[PUSH] $printable" -ForegroundColor Yellow

  $pushArgs = @('nuget','push', $pkg.FullName, '--api-key', $ApiKey, '--source', $Source)
  if (-not $NoSkipDuplicate) { $pushArgs += '--skip-duplicate' }

  & dotnet @pushArgs 2>&1 | ForEach-Object { Write-Host "  $_" }

  if ($LASTEXITCODE -ne 0) {
    Write-Warning "[FAIL] push failed ($LASTEXITCODE) for $($pkg.Name)"
    $failCount++
    $failed += $pkg.Name
  } else {
    Write-Host "[OK] $($pkg.Name)" -ForegroundColor Green
    $successCount++
  }
}

Write-Host ''
Write-Host '========== SUMMARY ==========' -ForegroundColor Cyan
Write-Host "  Version:   $Version"
Write-Host "  Succeeded: $successCount"
Write-Host "  Failed:    $failCount"
if ($failed.Count -gt 0) {
  Write-Host "  Failed packages:" -ForegroundColor Red
  $failed | ForEach-Object { Write-Host "    - $_" -ForegroundColor Red }
}
Write-Host '=============================' -ForegroundColor Cyan

if ($failCount -gt 0) { exit 1 }
Write-Host '[DONE] Publish complete.' -ForegroundColor Green
