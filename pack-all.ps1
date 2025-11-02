param(
 [string]$Version = $(Get-Date -Format 'yyyy.MM.dd'),
 [string]$Configuration = 'Release'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

Write-Host "Packing all packable projects with version $Version..." -ForegroundColor Cyan

# Ensure README exists
if (-not (Test-Path -Path "README.md")) {
 Write-Error "README.md not found at repo root. Aborting."; exit 1
}

# Restore solution
& dotnet restore --nologo | Out-Host

# Find all csproj files
$projects = Get-ChildItem -Recurse -Filter *.csproj | Select-Object -ExpandProperty FullName

$artifacts = Join-Path (Get-Location) "artifacts\nupkg"
New-Item -ItemType Directory -Force -Path $artifacts | Out-Null

foreach ($proj in $projects) {
 Write-Host "Packing: $proj" -ForegroundColor Yellow
 & dotnet pack "$proj" -c $Configuration -o $artifacts /p:PackageVersion=$Version /p:ContinuousIntegrationBuild=true --nologo
 if ($LASTEXITCODE -ne 0) {
 Write-Warning "Skipping (not packable or failed): $proj"
 $global:LASTEXITCODE = 0
 }
}

Write-Host "Done. Packages in: $artifacts" -ForegroundColor Green
