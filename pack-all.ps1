<#!
.SYNOPSIS
  Packs all Lazy.Crud projects and gathers .nupkg/.snupkg into a versioned folder. Optionally publishes to NuGet.

.DESCRIPTION
  Detects version automatically (from Application.DTO csproj) if not provided.
  Creates artifacts/nuget/<version>/ and copies packages there.
  Can interactively publish all or selected packages to nuget.org.

.PARAMETER Version
  Optional explicit version. If omitted, the script reads the first project file's <Version> element.

.PARAMETER OutputRoot
  Root folder for artifacts. Default: artifacts/nuget

.PARAMETER SkipRestore
  Skips the initial dotnet restore if specified.

.PARAMETER Publish
  When specified, asks whether to publish all packages or only selected ones after packing.

.PARAMETER ApiKey
  NuGet API key. If omitted will use the default provided key. (You may override for security.)

.PARAMETER NoSymbols
  Skips pushing .snupkg symbol packages.

.EXAMPLE
  pwsh scripts/pack-all.ps1 -Publish
  Packs and then interactively publishes.

.EXAMPLE
  pwsh scripts/pack-all.ps1 -Version 1.0.10 -Publish -ApiKey YOUR_KEY

#>
[CmdletBinding()] param(
  [string]$Version,
  [string]$OutputRoot = 'artifacts/nuget',
  [switch]$SkipRestore,
  [switch]$Publish,
  [string]$ApiKey = 'KEY',
  [switch]$NoSymbols
)

$ErrorActionPreference = 'Stop'
$projects = @(
  'src/Builder/Builder.Application.DTO/Lazy.Crud.Builder.Application.DTO.csproj',
  'src/Builder/Builder.Api.Queries/Lazy.Crud.Builder.Api.Queries.csproj',
  'src/Builder/Builder.Application.DTO.Http.Models/Lazy.Crud.Builder.Application.DTO.Http.Models.csproj',
  'src/Builder/Builder.Application.Validators/Lazy.Crud.Builder.Application.Validators.csproj',
  'src/Builder/Builder.Enumeration/Lazy.Crud.Builder.Enumeration.csproj',
  'src/CrossCutting/CrossCutting.Utils/Lazy.Crud.CrossCutting.Infra.Utils.csproj',
  'src/Builder/Builder.Application/Lazy.Crud.Builder.Application.csproj',
  'src/Builder/Builder.Domain/Lazy.Crud.Builder.Domain.csproj',
  'src/Builder/Builder.Infra.Data/Lazy.Crud.Builder.Infra.Data.csproj',
  'src/Builder/Builder.Infra.IoC/Lazy.Crud.Builder.Infra.IoC.csproj',
  'src/CrossCutting/CrossCutting.Domain/Lazy.Crud.CrossCutting.Domain.csproj',
  'src/CrossCutting/CrossCutting.Infra.Log/Lazy.Crud.CrossCutting.Infra.Log.csproj',
  'src/CrossCutting/CrossCutting.Application.Mail/Lazy.Crud.CrossCutting.Application.Mail.csproj',
  'src/CrossCutting/Lazy.Crud.CrossCutting.Domain/Lazy.Crud.CrossCutting.Domain.csproj'
)

function Get-VersionFromCsproj([string]$csprojPath) {
  if (!(Test-Path $csprojPath)) { throw "Project file not found: $csprojPath" }
  $xml = [xml](Get-Content -Raw $csprojPath)
  $verNode = $xml.Project.PropertyGroup.Version | Select-Object -First 1
  if (-not $verNode) { throw "<Version> element not found in $csprojPath" }
  return $verNode.Trim()
}

if (-not $Version) {
  Write-Host '[INFO] Detecting version from first project...' -ForegroundColor Cyan
  $Version = Get-VersionFromCsproj $projects[0]
  Write-Host "[INFO] Detected version: $Version" -ForegroundColor Green
}

$dest = Join-Path $OutputRoot $Version
if (!(Test-Path $dest)) { New-Item -ItemType Directory -Path $dest | Out-Null }

if (-not $SkipRestore) {
  Write-Host '[STEP] dotnet restore (solution/global)' -ForegroundColor Cyan
  dotnet restore | Write-Host
}

$failed = @()
foreach ($p in $projects) {
  Write-Host "[PACK] $p" -ForegroundColor Yellow
  try {
    dotnet pack -c Release $p --no-restore | Write-Host
  } catch {
    Write-Warning "Failed pack: $p"
    $failed += $p
    continue
  }
  $projDir = Split-Path $p -Parent
  $binPath = Join-Path $projDir 'bin/Release'
  if (Test-Path $binPath) {
    Get-ChildItem -Path $binPath -Recurse -Include *.nupkg, *.snupkg | ForEach-Object {
      Copy-Item $_.FullName -Destination $dest -Force
    }
  } else {
    Write-Warning "bin path missing: $binPath"
  }
}

Write-Host "[RESULT] Packages copied to: $dest" -ForegroundColor Green
Write-Host '[LIST]' -ForegroundColor Cyan
$allPkgs = Get-ChildItem $dest -File -Include *.nupkg | Where-Object { $_.Name -notmatch '\.symbols\.' }
$allPkgs | ForEach-Object { Write-Host " - $($_.Name)" }

if ($failed.Count -gt 0) {
  Write-Warning "Some projects failed: $($failed -join ', ')"
  if (-not $Publish) { exit 1 }
}

function Publish-Packages([System.IO.FileInfo[]]$packages, [System.IO.FileInfo[]]$symbolPackages) {
  if (-not $ApiKey) { throw 'API key is required for publish.' }
  Write-Host '[PUBLISH] Using nuget.org feed' -ForegroundColor Cyan
  foreach ($pkg in $packages) {
    Write-Host "[PUSH] $($pkg.Name)" -ForegroundColor Yellow
    dotnet nuget push $pkg.FullName -k $ApiKey -s https://api.nuget.org/v3/index.json --skip-duplicate | Write-Host
  }
  if (-not $NoSymbols -and $symbolPackages.Count -gt 0) {
    Write-Host '[PUBLISH] Symbols (.snupkg)' -ForegroundColor Cyan
    foreach ($sp in $symbolPackages) {
      Write-Host "[PUSH][SYM] $($sp.Name)" -ForegroundColor DarkYellow
      dotnet nuget push $sp.FullName -k $ApiKey -s https://api.nuget.org/v3/index.json --skip-duplicate | Write-Host
    }
  }
}

if ($Publish) {
  Write-Host ''
  Write-Host '[INTERACTIVE PUBLISH]' -ForegroundColor Cyan
  # enumerate packages with index
  $indexed = @()
  for ($i = 0; $i -lt $allPkgs.Count; $i++) {
    $indexed += [PSCustomObject]@{ Index = $i + 1; Name = $allPkgs[$i].Name; File = $allPkgs[$i] }
  }
  $indexed | ForEach-Object { Write-Host "[$($_.Index)] $($_.Name)" }
  Write-Host "Digite 'all' para publicar todos, ou números separados por vírgula (ex: 1,3,5)." -ForegroundColor Gray
  $sel = Read-Host 'Seleção'
  if ($sel -match '^(all|a)$') {
    $toPublish = $allPkgs
  } else {
    $nums = $sel -split ',' | ForEach-Object { $_.Trim() } | Where-Object { $_ -match '^[0-9]+$' } | ForEach-Object { [int]$_ }
    $valid = $indexed | Where-Object { $nums -contains $_.Index } | ForEach-Object { $_.File }
    if (-not $valid -or $valid.Count -eq 0) {
      Write-Warning 'Nenhum índice válido selecionado. Abortando publish.'
      exit 1
    }
    $toPublish = $valid
  }
  $symbolPkgs = Get-ChildItem $dest -File -Include *.snupkg
  Publish-Packages -packages $toPublish -symbolPackages $symbolPkgs
}

Write-Host '[DONE] Pack process complete.' -ForegroundColor Green
if ($Publish) { Write-Host '[DONE] Publish process complete.' -ForegroundColor Green }
