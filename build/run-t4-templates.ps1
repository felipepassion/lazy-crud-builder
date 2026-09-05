#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Transforms the T4 templates of an aggregate through the Visual Studio T4 toolchain, unattended.

.DESCRIPTION
    Superseded wrapper kept for discoverability. The previous version located TextTransform.exe by
    recursively scanning Program Files and offered MSBuild/dotnet-t4 "methods" that do not perform a
    real Lazy CRUD transformation. This version delegates to the platform tool, which resolves the
    installed Visual Studio deterministically (vswhere / Setup Configuration API, prerelease and
    Insiders included), fails early with exit code 3 when the prerequisite is missing, and runs the
    Domain-first pipeline inside an embedded Visual Studio automation session.

.PARAMETER AggregateName
    Name of the aggregate (for example Products).

.PARAMETER Solution
    Solution root or .sln (default: this repository).

.PARAMETER Resolution
    protect (default), force or skip-modified for drifted/unverified files.

.EXAMPLE
    .\build\run-t4-templates.ps1 -AggregateName Products -Solution C:\work\MySolution
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)] [string]$AggregateName,
    [string]$Solution,
    [ValidateSet('protect', 'force', 'skip-modified')] [string]$Resolution = 'protect',
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'
$repo = Split-Path $PSScriptRoot -Parent
if (-not $Solution) { $Solution = $repo }
$tool = Join-Path $repo "src/Platform/Lazy.Crud.Builder.Platform.Tool/bin/$Configuration/net10.0/Lazy.Crud.Builder.Platform.Tool.dll"
if (-not (Test-Path $tool)) {
    Write-Host "Building the platform tool ($Configuration)..." -ForegroundColor Cyan
    & dotnet build (Join-Path $repo 'src/Platform/Lazy.Crud.Builder.Platform.Tool/Lazy.Crud.Builder.Platform.Tool.csproj') -c $Configuration --nologo -v q
    if ($LASTEXITCODE -ne 0) { exit 8 }
}
& dotnet $tool generate --solution $Solution --aggregate $AggregateName --resolution $Resolution
exit $LASTEXITCODE
