<#!
.SYNOPSIS
  Empacota todos os projetos Lazy.Crud e reúne apenas arquivos .nupkg (sem símbolos) em uma pasta versionada. Opcionalmente publica no NuGet.

.DESCRIPTION
  Detecta versão automaticamente (do primeiro csproj listado) se não fornecida.
  Cria artifacts/nuget/<versao>/ e copia somente os pacotes principais (.nupkg).
  Publicação interativa opcional apenas de .nupkg (nenhum .snupkg é gerado).

.PARAMETER Version
  Versão explícita opcional. Se omitida, lida do primeiro projeto (<Version> no csproj).

.PARAMETER OutputRoot
  Pasta raiz para artefatos. Padrão: artifacts/nuget

.PARAMETER SkipRestore
  Pula o dotnet restore inicial se especificado.

.PARAMETER Publish
  Quando especificado, pergunta se publica todos ou somente selecionados após o pack.

.PARAMETER ApiKey
  Chave de API NuGet. Se omitida usa a chave padrão.

.EXAMPLE
  pwsh scripts/pack-all.ps1 -Publish
  Empacota e depois publica interativamente (somente .nupkg).

.EXAMPLE
  pwsh scripts/pack-all.ps1 -Version 1.0.10 -Publish -ApiKey SUA_CHAVE
  ./pack-all.ps1 -Publish -ApiKey
  Empacota e publica apenas .nupkg.
#>
[CmdletBinding()] param(
  [string]$Version,
  [string]$OutputRoot = 'artifacts/nuget',
  [switch]$SkipRestore,
  [switch]$Publish,
  [string]$ApiKey = ''
)

$ErrorActionPreference = 'Stop'
$projects = @(
  'src/Builder/Builder.Infra.IoC/Lazy.Crud.Builder.Infra.IoC.csproj'
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
# Limpeza do destino antes de empacotar para evitar lixo de execuções anteriores
if (Test-Path $dest) {
  Write-Host "[CLEAN] Removing previous output at: $dest" -ForegroundColor Cyan
  try {
    Remove-Item -Path $dest -Recurse -Force -ErrorAction Stop
  } catch {
    Write-Warning "Falha ao limpar '$dest': $($_.Exception.Message)"
  }
}
# Recria a pasta de destino
if (!(Test-Path $dest)) { New-Item -ItemType Directory -Path $dest | Out-Null }

if (-not $SkipRestore) {
  Write-Host '[STEP] dotnet restore (solution/global)' -ForegroundColor Cyan
  dotnet restore | Write-Host
}

$failed = @()
foreach ($p in $projects) {
  Write-Host "[PACK] $p" -ForegroundColor Yellow
  try {
    # Sempre desativa símbolos e source.
    dotnet pack -c Release $p --no-restore -o $dest /p:IncludeSymbols=false /p:IncludeSource=false | Write-Host
  } catch {
    Write-Warning "Failed pack: $p"
    $failed += $p
    continue
  }
}

Write-Host "[RESULT] Packages placed in: $dest" -ForegroundColor Green
Write-Host '[LIST]' -ForegroundColor Cyan
$allPkgs = Get-ChildItem -Path $dest -Filter *.nupkg -File | Where-Object { $_.Name -notmatch '\.symbols\.' }
$allPkgs | ForEach-Object { Write-Host " - $($_.Name)" }

if ($failed.Count -gt 0) {
  Write-Warning "Some projects failed: $($failed -join ', ')"
  if (-not $Publish) { exit 1 }
}

function Invoke-PushPackage([string]$packagePath) {
  $source = 'https://api.nuget.org/v3/index.json'
  $printable = @('dotnet','nuget','push', '"' + $packagePath + '"', '--api-key', '***', '--source', $source) -join ' '
  Write-Host "[PUSH CMD] $printable" -ForegroundColor DarkCyan

  $args = @('nuget','push', $packagePath, '--api-key', $ApiKey, '--source', $source, '--skip-duplicate')
  $null = & dotnet @args 2>&1 | ForEach-Object { Write-Host "  $_" }
  $exit = $LASTEXITCODE
  if ($exit -ne 0) {
    Write-Warning "[FAIL] push failed ($exit) for $(Split-Path -Leaf $packagePath)"
    return $false
  }
  Write-Host "[OK] push succeeded for $(Split-Path -Leaf $packagePath)" -ForegroundColor Green
  return $true
}

function Publish-Packages([System.IO.FileInfo[]]$packages) {
  if (-not $ApiKey) { throw 'API key is required for publish.' }
  Write-Host '[PUBLISH] Using nuget.org feed' -ForegroundColor Cyan
  Write-Host ("[PUBLISH] .nupkg to push: {0}" -f $packages.Count)
  foreach ($pkg in $packages) {
    Write-Host "[PUSH] $($pkg.Name)" -ForegroundColor Yellow
    [void](Invoke-PushPackage -packagePath $pkg.FullName)
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
  if ($indexed.Count -eq 0) {
    Write-Warning 'Nenhum pacote .nupkg encontrado para publicar.'
    exit 1
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
  Publish-Packages -packages $toPublish
}

Write-Host '[DONE] Pack process complete.' -ForegroundColor Green
if ($Publish) { Write-Host '[DONE] Publish process complete.' -ForegroundColor Green }
