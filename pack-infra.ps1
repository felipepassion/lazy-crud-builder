param(
 [Parameter(Mandatory=$true)][string]$Version,
 [string]$Configuration = 'Release'
)

$proj = "src/Core/Core.Infra.IoC/Core.Infra.IoC.csproj"
$artifacts = Join-Path (Get-Location) "artifacts/nupkg"
New-Item -ItemType Directory -Force -Path $artifacts | Out-Null

Write-Host "Packing aggregator: $proj -> version $Version" -ForegroundColor Cyan
& dotnet pack $proj -c $Configuration -o $artifacts /p:PackageVersion=$Version /p:ContinuousIntegrationBuild=true --nologo

Write-Host "Done. Files:" -ForegroundColor Green
Get-ChildItem $artifacts -Filter "*Infra.IoC*${Version}*.nupkg" | Select-Object FullName, Length | Format-Table -AutoSize
