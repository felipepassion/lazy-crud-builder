#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Fails when a publish credential literal (NuGet API key, GitHub token, AWS key, generic api-key
  assignment) appears anywhere in the repository working tree.

.DESCRIPTION
  Scans every text file under the repository root except .git, bin, obj, artifacts and
  node_modules. Exit code 0 means clean; 1 means at least one match (printed as path:line).
  Use -SelfTest to plant a fake key in a temporary directory and prove the scan detects it.

.PARAMETER Root
  Directory to scan. Defaults to the repository root (parent of this script's folder).

.PARAMETER SelfTest
  Plants a synthetic key in a temporary directory, scans it, and fails unless it is detected.
#>
[CmdletBinding()]
param(
  [string]$Root,
  [switch]$SelfTest
)

$ErrorActionPreference = 'Stop'
if (-not $Root) { $Root = Split-Path $PSScriptRoot -Parent }

$patterns = @(
  @{ Name = 'nuget.org API key';            Regex = 'oy2[a-z0-9]{43}' },
  @{ Name = 'GitHub token';                 Regex = 'gh[pousr]_[A-Za-z0-9]{36,}' },
  @{ Name = 'AWS access key id';            Regex = 'AKIA[0-9A-Z]{16}' },
  @{ Name = 'Azure DevOps / generic PAT';   Regex = '(?i)(apikey|api_key|api-key|nugetapikey|nuget_api_key)\s*[=:]\s*[''"][A-Za-z0-9+/=_\-]{32,}[''"]' },
  @{ Name = 'Inline -ApiKey literal';       Regex = '(?i)-ApiKey\s+[''"]?[a-z0-9]{40,}[''"]?' }
)

$excludeDirs = @('.git', 'bin', 'obj', 'artifacts', 'node_modules', '.vs', 'TestResults')
$textExtensions = @('.ps1', '.psm1', '.psd1', '.cs', '.tt', '.ttinclude', '.csproj', '.props', '.targets', '.json', '.xml', '.yml', '.yaml', '.md', '.txt', '.config', '.settings', '.sln', '.nuspec', '.sh', '.cmd', '.bat', '.editorconfig', '.gitattributes', '.gitignore', '.razor', '.css', '.js', '.ts', '.html', '.env')

function Invoke-Scan([string]$dir) {
  $hits = @()
  $files = Get-ChildItem -Path $dir -Recurse -File -Force | Where-Object {
    $rel = $_.FullName.Substring($dir.Length).TrimStart('\', '/')
    $parts = $rel -split '[\\/]'
    -not ($parts | Where-Object { $excludeDirs -contains $_ }) -and ($textExtensions -contains $_.Extension.ToLowerInvariant() -or $_.Extension -eq '')
  }
  foreach ($f in $files) {
    $lineNo = 0
    foreach ($line in [System.IO.File]::ReadLines($f.FullName)) {
      $lineNo++
      foreach ($p in $patterns) {
        if ($line -match $p.Regex) {
          $hits += [PSCustomObject]@{ File = $f.FullName; Line = $lineNo; Pattern = $p.Name }
        }
      }
    }
  }
  return $hits
}

if ($SelfTest) {
  $tmp = Join-Path ([System.IO.Path]::GetTempPath()) ("lazycrud-credscan-" + [Guid]::NewGuid().ToString('N'))
  New-Item -ItemType Directory -Path $tmp | Out-Null
  try {
    $alphabet = 'abcdefghijklmnopqrstuvwxyz0123456789'
    $rnd = [System.Random]::new()
    $synthetic = 'oy2' + (-join (1..43 | ForEach-Object { $alphabet[$rnd.Next($alphabet.Length)] }))
    Set-Content -Path (Join-Path $tmp 'planted.ps1') -Value ("[string]`$ApiKey = '" + $synthetic + "'")
    $hits = Invoke-Scan $tmp
    if ($hits.Count -eq 0) { Write-Error 'credential-scan self-test FAILED: the planted key was not detected'; exit 1 }
    Write-Host "credential-scan self-test OK: planted key detected ($($hits[0].Pattern))" -ForegroundColor Green
  } finally {
    Remove-Item -Recurse -Force $tmp -ErrorAction SilentlyContinue
  }
}

$hits = Invoke-Scan $Root
if ($hits.Count -gt 0) {
  Write-Host "credential-scan: $($hits.Count) credential literal(s) found:" -ForegroundColor Red
  $hits | ForEach-Object { Write-Host ("  {0}:{1}  [{2}]" -f $_.File, $_.Line, $_.Pattern) -ForegroundColor Red }
  Write-Host 'Remove the literal and read credentials from the environment (NUGET_API_KEY). Rotate any key that was committed.' -ForegroundColor Yellow
  exit 1
}
Write-Host "credential-scan: clean ($Root)" -ForegroundColor Green
exit 0
