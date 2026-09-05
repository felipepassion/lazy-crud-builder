<#
.SYNOPSIS
  Publishes every .nupkg of the most recent version folder under the artifacts root to a NuGet feed.

.DESCRIPTION
  Scans the artifacts root (artifacts/nuget by default), picks the highest version sub-folder,
  and pushes every .nupkg in it. The API key is NEVER stored in the repository: pass -ApiKey
  explicitly or set the NUGET_API_KEY environment variable (the CI secret). The script fails
  with a clear message when neither is present.

.PARAMETER OutputRoot
  Artifacts root. Default: artifacts/nuget

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
  [string]$ApiKey,
  [string]$Source = 'https://api.nuget.org/v3/index.json',
  [switch]$NoSkipDuplicate
)

$ErrorActionPreference = 'Stop'

if (-not $ApiKey) { $ApiKey = $env:NUGET_API_KEY }
if (-not $ApiKey) {
  Write-Error 'No NuGet API key: pass -ApiKey or set the NUGET_API_KEY environment variable. Credentials are never stored in this repository.'
  exit 2
}

if (!(Test-Path $OutputRoot)) {
  throw "Artifacts folder not found: $OutputRoot"
}

$versionDirs = Get-ChildItem -Path $OutputRoot -Directory |
  Where-Object { $_.Name -match '^\d+\.\d+' } |
  Sort-Object {
    try { [version](($_.Name -split '-')[0]) } catch { [version]'0.0.0' }
  } -Descending

if ($versionDirs.Count -eq 0) {
  throw "No version sub-folder found in: $OutputRoot"
}

$latestDir = $versionDirs[0]
Write-Host "[INFO] Latest version detected: $($latestDir.Name)" -ForegroundColor Green
Write-Host "[INFO] Folder: $($latestDir.FullName)" -ForegroundColor Cyan

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
Write-Host "  Version:   $($latestDir.Name)"
Write-Host "  Succeeded: $successCount"
Write-Host "  Failed:    $failCount"
if ($failed.Count -gt 0) {
  Write-Host "  Failed packages:" -ForegroundColor Red
  $failed | ForEach-Object { Write-Host "    - $_" -ForegroundColor Red }
}
Write-Host '=============================' -ForegroundColor Cyan

if ($failCount -gt 0) { exit 1 }
Write-Host '[DONE] Publish complete.' -ForegroundColor Green
