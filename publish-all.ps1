<#
.SYNOPSIS
  Publica todos os pacotes .nupkg da versão mais recente encontrada na pasta de artefatos.

.DESCRIPTION
  Varre a pasta de artefatos (artifacts/nuget por padrão), detecta a subpasta de versão
  mais recente (por ordenação semântica), e faz push de todos os .nupkg no nuget.org.

.PARAMETER OutputRoot
  Pasta raiz de artefatos. Padrão: artifacts/nuget

.PARAMETER ApiKey
  Chave de API NuGet. Obrigatória.

.PARAMETER Source
  Feed NuGet de destino. Padrão: https://api.nuget.org/v3/index.json

.PARAMETER SkipDuplicate
  Passa --skip-duplicate ao dotnet nuget push (padrão: ativado).

.EXAMPLE
  pwsh publish-all.ps1 -ApiKey SUA_CHAVE
  Detecta a versão mais recente e publica todos os .nupkg.

.EXAMPLE
  pwsh publish-all.ps1 -ApiKey SUA_CHAVE -OutputRoot artifacts/nuget
#>
[CmdletBinding()] param(
  [string]$OutputRoot = 'artifacts/nuget',
  [Parameter(Mandatory)][string]$ApiKey,
  [string]$Source = 'https://api.nuget.org/v3/index.json',
  [switch]$NoSkipDuplicate
)

$ErrorActionPreference = 'Stop'

# --- Detectar versão mais recente ---------------------------------------------------
if (!(Test-Path $OutputRoot)) {
  throw "Pasta de artefatos não encontrada: $OutputRoot"
}

$versionDirs = Get-ChildItem -Path $OutputRoot -Directory |
  Where-Object { $_.Name -match '^\d+\.\d+' } |
  Sort-Object {
    try { [version]$_.Name } catch { [version]'0.0.0' }
  } -Descending

if ($versionDirs.Count -eq 0) {
  throw "Nenhuma subpasta de versão encontrada em: $OutputRoot"
}

$latestDir = $versionDirs[0]
Write-Host "[INFO] Versão mais recente detectada: $($latestDir.Name)" -ForegroundColor Green
Write-Host "[INFO] Pasta: $($latestDir.FullName)" -ForegroundColor Cyan

# --- Listar pacotes ------------------------------------------------------------------
$packages = Get-ChildItem -Path $latestDir.FullName -Filter *.nupkg -File |
  Where-Object { $_.Name -notmatch '\.symbols\.' }

if ($packages.Count -eq 0) {
  throw "Nenhum pacote .nupkg encontrado em: $($latestDir.FullName)"
}

Write-Host "[INFO] Pacotes encontrados: $($packages.Count)" -ForegroundColor Cyan
$packages | ForEach-Object { Write-Host "  - $($_.Name)" }

# --- Push ----------------------------------------------------------------------------
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
    Write-Warning "[FAIL] push falhou ($LASTEXITCODE) para $($pkg.Name)"
    $failCount++
    $failed += $pkg.Name
  } else {
    Write-Host "[OK] $($pkg.Name)" -ForegroundColor Green
    $successCount++
  }
}

# --- Resumo --------------------------------------------------------------------------
Write-Host ''
Write-Host '========== RESUMO ==========' -ForegroundColor Cyan
Write-Host "  Versão:     $($latestDir.Name)"
Write-Host "  Sucesso:    $successCount"
Write-Host "  Falha:      $failCount"
if ($failed.Count -gt 0) {
  Write-Host "  Falharam:" -ForegroundColor Red
  $failed | ForEach-Object { Write-Host "    - $_" -ForegroundColor Red }
}
Write-Host '=============================' -ForegroundColor Cyan

if ($failCount -gt 0) { exit 1 }
Write-Host '[DONE] Publish completo.' -ForegroundColor Green
