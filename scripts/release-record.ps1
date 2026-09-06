#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Writes the machine-readable release record for one version: what was produced, from which commit,
  and which of it is actually on the public source.

.DESCRIPTION
  A consuming repository (the LazyCrudExtension VSIX and CLI) needs one file that answers "which
  version do I target, what is in it, and is it really published?" without reading a changelog. That
  file is contracts/releases/<version>.json, produced here from three facts that cannot be guessed:

    - the package set physically produced in artifacts/nuget/<version>, with the SHA-256 of each
      .nupkg, so a consumer can prove the file it restored is the file this repository built;
    - the descriptor of the same build (platformVersion, contractVersion, schema versions, package
      ids and the templates package with its aliases), so the record never disagrees with the
      contract it accompanies;
    - the public source itself: every package id is queried on nuget.org and the record states
      whether that exact version is there. `--skip-duplicate` returning zero is not publication, and
      this file never claims it is.

  The record is a fact sheet, not a promise: `status` is `published` only when every declared package
  answers for this version on the public source, and `prepared` otherwise.

.PARAMETER Version
  Version to record. Defaults to LazyCrudVersion in Directory.Build.props.

.PARAMETER PackagesDirectory
  Where the .nupkg files are. Default: artifacts/nuget/<version>.

.PARAMETER Source
  Public source to verify against. Default: https://api.nuget.org/v3/index.json

.PARAMETER SkipSourceCheck
  Do not query the source; every package is recorded as unverified. For offline runs only.

.PARAMETER TimeoutSeconds
  Per-request timeout for the source check (default 30). The whole check is bounded by it per id.
#>
[CmdletBinding()]
param(
  [string]$Version,
  [string]$PackagesDirectory,
  [string]$Source = 'https://api.nuget.org/v3/index.json',
  [switch]$SkipSourceCheck,
  [int]$TimeoutSeconds = 30
)

$ErrorActionPreference = 'Stop'
$repo = Split-Path $PSScriptRoot -Parent
Set-Location $repo

function Get-CentralVersion {
  $xml = [xml](Get-Content -Raw (Join-Path $repo 'Directory.Build.props'))
  $v = $xml.Project.PropertyGroup | ForEach-Object { $_.LazyCrudVersion } | Where-Object { $_ } |
       ForEach-Object { if ($_ -is [System.Xml.XmlElement]) { $_.InnerText } else { [string]$_ } } | Select-Object -First 1
  if (-not $v) { throw 'LazyCrudVersion is not defined in Directory.Build.props' }
  return ([string]$v).Trim()
}

if (-not $Version) { $Version = Get-CentralVersion }
if (-not $PackagesDirectory) { $PackagesDirectory = Join-Path $repo "artifacts/nuget/$Version" }
if (-not (Test-Path $PackagesDirectory)) { Write-Error "no package directory: $PackagesDirectory (run pack-all.ps1 first)"; exit 4 }

$descriptorPath = Join-Path $repo 'artifacts/platform/lazycrud.platform.json'
if (-not (Test-Path $descriptorPath)) { Write-Error "no descriptor: $descriptorPath (build the solution first)"; exit 4 }
$d = Get-Content -Raw $descriptorPath | ConvertFrom-Json -Depth 32

# ---- declared package set: the descriptor is the authority, never a list retyped here -------------
$declared = [System.Collections.Generic.List[string]]::new()
foreach ($l in $d.layers) { if ($l.packageId) { [void]$declared.Add([string]$l.packageId) } }
foreach ($p in $d.nonLayerPackages) { [void]$declared.Add([string]$p) }
$declared = @($declared | Sort-Object -Unique)

# ---- produced files -------------------------------------------------------------------------------
$files = @(Get-ChildItem -Path $PackagesDirectory -Filter *.nupkg -File | Where-Object { $_.Name -notmatch '\.symbols\.' } | Sort-Object Name)

function Get-PackageIdFromFile([string]$name, [string]$version) {
  $suffix = ".$version.nupkg"
  if ($name.EndsWith($suffix, [StringComparison]::OrdinalIgnoreCase)) { return $name.Substring(0, $name.Length - $suffix.Length) }
  return $null
}

function Test-PublishedOnSource([string]$id, [string]$version, [int]$timeout) {
  # The flat container is the registration-independent view of what a restore can actually download.
  $url = "https://api.nuget.org/v3-flatcontainer/$($id.ToLowerInvariant())/index.json"
  try {
    $r = Invoke-RestMethod -Uri $url -TimeoutSec $timeout -ErrorAction Stop
    return @{ known = $true; published = ([bool](@($r.versions) -contains $version)); versions = @($r.versions) }
  } catch {
    if ($_.Exception.Response -and $_.Exception.Response.StatusCode.value__ -eq 404) { return @{ known = $true; published = $false; versions = @() } }
    return @{ known = $false; published = $false; versions = @() }
  }
}

$packages = [System.Collections.Generic.List[object]]::new()
$missing = [System.Collections.Generic.List[string]]::new()
$extra = [System.Collections.Generic.List[string]]::new()
$producedIds = [System.Collections.Generic.List[string]]::new()

foreach ($f in $files) {
  $id = Get-PackageIdFromFile $f.Name $Version
  if (-not $id) { [void]$extra.Add($f.Name); continue }
  [void]$producedIds.Add($id)
  $entry = [ordered]@{
    packageId = $id
    version   = $Version
    fileName  = $f.Name
    sizeBytes = $f.Length
    sha256    = (Get-FileHash -Algorithm SHA256 -Path $f.FullName).Hash.ToLowerInvariant()
    declared  = ($declared -contains $id)
  }
  if ($SkipSourceCheck) {
    $entry.publishedOnSource = $null
    $entry.sourceCheck = 'skipped'
  } else {
    $probe = Test-PublishedOnSource $id $Version $TimeoutSeconds
    $entry.publishedOnSource = $(if ($probe.known) { $probe.published } else { $null })
    $entry.sourceCheck = $(if ($probe.known) { 'queried' } else { 'unreachable' })
    $entry.latestOnSource = $(if ($probe.versions.Count -gt 0) { $probe.versions[-1] } else { $null })
  }
  [void]$packages.Add([pscustomobject]$entry)
}
foreach ($id in $declared) { if ($producedIds -notcontains $id) { [void]$missing.Add($id) } }
foreach ($id in $producedIds) { if ($declared -notcontains $id) { [void]$extra.Add($id) } }

$allPublished = ($packages.Count -gt 0) -and -not ($packages | Where-Object { $_.publishedOnSource -ne $true })
$commit = (& git -C $repo rev-parse HEAD).Trim()
$commitOnRemote = $false
try {
  $remoteBranches = @(& git -C $repo branch -r --contains $commit 2>$null | Where-Object { $_ -and $_.Trim() })
  $commitOnRemote = ($remoteBranches.Count -gt 0)
} catch { $commitOnRemote = $false }

$record = [ordered]@{
  schemaVersion            = 1
  version                  = $Version
  status                   = $(if ($allPublished) { 'published' } else { 'prepared' })
  generatedAtUtc           = (Get-Date).ToUniversalTime().ToString('o')
  source                   = $Source
  platformVersion          = [string]$d.platformVersion
  contractVersion          = [int]$d.contractVersion
  descriptorSchemaVersion  = [int]$d.schemaVersion
  manifestSchemaVersion    = [int]$d.compatibility.manifestSchemaVersion
  templatesPackageId       = [string]$d.templates.packageId
  templatesAliasPackageIds = @($d.templates.aliasPackageIds)
  commit                   = $commit
  commitReachableFromRemote = $commitOnRemote
  repositoryUrl            = 'https://github.com/felipepassion/lazy-crud-builder'
  packageCount             = $packages.Count
  declaredCount            = $declared.Count
  missingPackages          = @($missing)
  undeclaredPackages       = @($extra | Sort-Object -Unique)
  packages                 = @($packages)
}

$outDir = Join-Path $repo 'contracts/releases'
New-Item -ItemType Directory -Force -Path $outDir | Out-Null
$outFile = Join-Path $outDir "$Version.json"
$json = ($record | ConvertTo-Json -Depth 8)
[System.IO.File]::WriteAllText($outFile, $json + "`n", (New-Object System.Text.UTF8Encoding($false)))

Write-Host "release record: $outFile"
Write-Host ("  status={0}  packages={1}/{2}  commit={3}{4}" -f $record.status, $packages.Count, $declared.Count, $commit.Substring(0, 12), $(if ($commitOnRemote) { '' } else { ' (not on the remote)' }))
if ($missing.Count -gt 0) { Write-Host "  MISSING: $($missing -join ', ')" -ForegroundColor Red }
if ($extra.Count -gt 0) { Write-Host "  UNDECLARED: $(($extra | Sort-Object -Unique) -join ', ')" -ForegroundColor Red }
$unpublished = @($packages | Where-Object { $_.publishedOnSource -ne $true } | ForEach-Object { $_.packageId })
if (-not $SkipSourceCheck -and $unpublished.Count -gt 0) { Write-Host "  not on ${Source}: $($unpublished -join ', ')" -ForegroundColor Yellow }

if ($missing.Count -gt 0 -or $extra.Count -gt 0) { exit 1 }
exit 0
