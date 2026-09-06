<#
.SYNOPSIS
  Builds the platform, generates and validates the platform descriptor, packs every Lazy CRUD
  package (runtime packages, Lazy.Crud.Builder.Platform, Lazy.Crud.Builder.Templates and the
  deprecated Lazy.Crud.Templates alias) into artifacts/nuget/<version>/, and optionally publishes.

.DESCRIPTION
  The version comes from the single central source, the LazyCrudVersion property in
  Directory.Build.props; no project declares its own version. Every pack failure fails the run
  (native exit codes are checked explicitly). Publishing never uses a stored credential: pass
  -ApiKey or set NUGET_API_KEY.

.PARAMETER Version
  Optional version override (for local experiments). Defaults to LazyCrudVersion.

.PARAMETER OutputRoot
  Artifacts root. Default: artifacts/nuget

.PARAMETER SkipRestore
  Skip the initial dotnet restore.

.PARAMETER SkipValidation
  Skip descriptor/package validation (never in CI).

.PARAMETER Publish
  Push the produced packages after packing (non-interactive; every .nupkg in the version folder).

.PARAMETER ApiKey
  NuGet API key for -Publish. Falls back to $env:NUGET_API_KEY. Never has a default value.

.EXAMPLE
  pwsh pack-all.ps1
  pwsh pack-all.ps1 -Publish   # requires NUGET_API_KEY
#>
[CmdletBinding()] param(
  [string]$Version,
  [string]$OutputRoot = 'artifacts/nuget',
  [switch]$SkipRestore,
  [switch]$SkipValidation,
  [switch]$Publish,
  [string]$ApiKey
)

$ErrorActionPreference = 'Stop'
$repoRoot = $PSScriptRoot
Set-Location $repoRoot

# Projects that ship as packages. Order matters only for readability; each is packed independently.
$projects = @(
  'src/Platform/Lazy.Crud.Builder.Platform/Lazy.Crud.Builder.Platform.csproj',
  'src/Templates/LazyCrud.Templates/Lazy.Crud.Builder.Templates.csproj',
  'src/Templates/LazyCrud.Templates.Alias/Lazy.Crud.Templates.csproj',
  'src/CrossCutting/CrossCutting.Infra.Utils/Lazy.Crud.CrossCutting.Infra.Utils.csproj',
  'src/CrossCutting/CrossCutting.Domain/Lazy.Crud.CrossCutting.Domain.csproj',
  'src/CrossCutting/CrossCutting.Infra.Log/Lazy.Crud.CrossCutting.Infra.Log.csproj',
  'src/CrossCutting/CrossCutting.Application.Mail/Lazy.Crud.CrossCutting.Application.Mail.csproj',
  'src/Builder/Builder.Enumeration/Lazy.Crud.Builder.Enumeration.csproj',
  'src/Builder/Builder.Api.Queries/Lazy.Crud.Builder.Api.Queries.csproj',
  'src/Builder/Builder.Application.DTO.Http.Models/Lazy.Crud.Builder.Application.DTO.Http.Models.csproj',
  'src/Builder/Builder.Application.Validators/Lazy.Crud.Builder.Application.Validators.csproj',
  'src/Builder/Builder.Application.DTO/Lazy.Crud.Builder.Application.DTO.csproj',
  'src/Builder/Builder.Domain/Lazy.Crud.Builder.Domain.csproj',
  'src/Builder/Builder.Application/Lazy.Crud.Builder.Application.csproj',
  'src/Builder/Builder.Infra.Data/Lazy.Crud.Builder.Infra.Data.csproj',
  'src/Builder/Builder.Infra.IoC/Lazy.Crud.Builder.Infra.IoC.csproj',
  'src/Builder/Builder.Api/Lazy.Crud.Builder.Api.csproj',
  'src/Builder/Builder.WebApi/Lazy.Crud.Builder.WebApi.csproj'
)

function Get-CentralVersion {
  $props = Join-Path $repoRoot 'Directory.Build.props'
  $xml = [xml](Get-Content -Raw $props)
  $node = $xml.Project.PropertyGroup | ForEach-Object { $_.LazyCrudVersion } | Where-Object { $_ } | ForEach-Object { if ($_ -is [System.Xml.XmlElement]) { $_.InnerText } else { [string]$_ } } | Select-Object -First 1
  if (-not $node) { throw "LazyCrudVersion is not defined in Directory.Build.props (the single version source)." }
  return ([string]$node).Trim()
}

function Invoke-Native([string]$label, [string[]]$arguments) {
  Write-Host "[$label] dotnet $($arguments -join ' ')" -ForegroundColor DarkGray
  & dotnet @arguments
  if ($LASTEXITCODE -ne 0) { throw "$label failed with exit code $LASTEXITCODE" }
}

if (-not $Version) {
  $Version = Get-CentralVersion
  Write-Host "[INFO] Central version (LazyCrudVersion): $Version" -ForegroundColor Green
} else {
  Write-Host "[INFO] Version override: $Version" -ForegroundColor Yellow
}

$dest = Join-Path (Join-Path $repoRoot $OutputRoot) $Version
if (Test-Path $dest) {
  Write-Host "[CLEAN] Removing previous output at: $dest" -ForegroundColor Cyan
  Remove-Item -Path $dest -Recurse -Force
}
New-Item -ItemType Directory -Path $dest | Out-Null

$versionArgs = @("-p:LazyCrudVersion=$Version")

if (-not $SkipRestore) {
  Invoke-Native 'RESTORE' (@('restore', 'LazyCrudBuilder.sln') + $versionArgs)
}

# 1. Build the whole solution once (generates the descriptor via the Platform tool AfterBuild target).
Invoke-Native 'BUILD' (@('build', 'LazyCrudBuilder.sln', '-c', 'Release', '--no-restore', '--nologo', '-v', 'q') + $versionArgs)

# 2. Validate the descriptor against the tree before packing.
$tool = Join-Path $repoRoot 'src/Platform/Lazy.Crud.Builder.Platform.Tool/bin/Release/net10.0/Lazy.Crud.Builder.Platform.Tool.dll'
if (-not (Test-Path $tool)) { throw "Platform tool not found after build: $tool" }
if (-not $SkipValidation) {
  Invoke-Native 'VALIDATE:tree' @($tool, 'validate', 'tree', '--repo', $repoRoot)
}

# 3. Pack each project; any non-zero exit code fails the run.
$failed = @()
foreach ($p in $projects) {
  Write-Host "[PACK] $p" -ForegroundColor Yellow
  & dotnet pack $p -c Release --no-build --no-restore -p:BuildProjectReferences=false -o $dest --nologo -v q @versionArgs -p:IncludeSymbols=false -p:IncludeSource=false
  if ($LASTEXITCODE -ne 0) {
    Write-Host "[PACK] FAILED ($LASTEXITCODE): $p" -ForegroundColor Red
    $failed += $p
  }
}
if ($failed.Count -gt 0) {
  Write-Host "[RESULT] $($failed.Count) project(s) failed to pack:" -ForegroundColor Red
  $failed | ForEach-Object { Write-Host "  - $_" -ForegroundColor Red }
  exit 1
}

$allPkgs = Get-ChildItem -Path $dest -Filter *.nupkg -File | Where-Object { $_.Name -notmatch '\.symbols\.' }
Write-Host "[RESULT] $($allPkgs.Count) package(s) in: $dest" -ForegroundColor Green
$allPkgs | ForEach-Object { Write-Host " - $($_.Name)" }

# 4. Validate the produced package set against the descriptor (declared packages, template paths, collisions).
if (-not $SkipValidation) {
  Invoke-Native 'VALIDATE:packages' @($tool, 'validate', 'packages', '--repo', $repoRoot, '--packages', $dest)
}

# 5. Optional publish: credential from the parameter or the environment only.
if ($Publish) {
  if (-not $ApiKey) { $ApiKey = $env:NUGET_API_KEY }
  if (-not $ApiKey) {
    # Write-Error throws under ErrorActionPreference Stop, so the documented exit code never
    # ran: the message goes to the error stream by hand and the script exits 2 as declared.
    [Console]::Error.WriteLine('No NuGet API key: pass -ApiKey or set the NUGET_API_KEY environment variable. Credentials are never stored in this repository.')
    exit 2
  }
  $source = 'https://api.nuget.org/v3/index.json'
  $pushFailed = 0
  foreach ($pkg in $allPkgs) {
    Write-Host "[PUSH] $($pkg.Name) -> $source" -ForegroundColor Yellow
    & dotnet nuget push $pkg.FullName --api-key $ApiKey --source $source --skip-duplicate 2>&1 | ForEach-Object { Write-Host "  $_" }
    if ($LASTEXITCODE -ne 0) { Write-Host "[PUSH] FAILED ($LASTEXITCODE): $($pkg.Name)" -ForegroundColor Red; $pushFailed++ }
  }
  if ($pushFailed -gt 0) { exit 1 }
  Write-Host '[DONE] Publish complete.' -ForegroundColor Green
}

Write-Host '[DONE] Pack complete.' -ForegroundColor Green
exit 0
