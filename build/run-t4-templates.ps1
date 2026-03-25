#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Runs T4 templates for a specific aggregate

.DESCRIPTION
    Executes T4 text templates in the correct order for all layers of an aggregate.
    Supports multiple execution methods: MSBuild, Visual Studio API, or dotnet-t4

.PARAMETER AggregateName
    Name of the aggregate (e.g., "Products")

.PARAMETER Method
    Execution method: MSBuild, VSTextTransform, or DotnetT4

.PARAMETER LayersOnly
    If set, runs only production layers (not tests)

.EXAMPLE
    .\build\run-t4-templates.ps1 -AggregateName Products
    .\build\run-t4-templates.ps1 -AggregateName Products -Method MSBuild
    .\build\run-t4-templates.ps1 -AggregateName Products -LayersOnly
#>

[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [string]$AggregateName,
    
    [Parameter(Mandatory=$false)]
    [ValidateSet("MSBuild", "VSTextTransform", "DotnetT4", "Auto")]
    [string]$Method = "Auto",
    
    [switch]$LayersOnly
)

$ErrorActionPreference = "Stop"
$workspaceRoot = Split-Path $PSScriptRoot -Parent
$aggregatePath = Join-Path $workspaceRoot "src\$AggregateName"

function Test-CommandExists {
    param([string]$Command)
    $null -ne (Get-Command $Command -ErrorAction SilentlyContinue)
}

function Get-T4TransformMethod {
    if ($Method -ne "Auto") {
        return $Method
    }
    
    # Auto-detect available method
    if (Test-CommandExists "msbuild") {
        Write-Host "Auto-detected: MSBuild" -ForegroundColor Green
        return "MSBuild"
    }
    
    if (Test-CommandExists "dotnet-t4") {
        Write-Host "Auto-detected: dotnet-t4" -ForegroundColor Green
        return "DotnetT4"
    }
    
    # Check for TextTransform.exe (comes with Visual Studio)
    $vsPath = "${env:ProgramFiles}\Microsoft Visual Studio"
    if (Test-Path $vsPath) {
        $textTransform = Get-ChildItem -Path $vsPath -Filter "TextTransform.exe" -Recurse -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($textTransform) {
            Write-Host "Auto-detected: Visual Studio TextTransform" -ForegroundColor Green
            return "VSTextTransform"
        }
    }
    
    Write-Host "⚠ No T4 transform tool found!" -ForegroundColor Yellow
    Write-Host "Install one of:" -ForegroundColor Yellow
    Write-Host "  - Visual Studio with .NET workload (includes MSBuild)" -ForegroundColor White
    Write-Host "  - dotnet tool install -g dotnet-t4" -ForegroundColor White
    return $null
}

function Invoke-T4Transform {
    param(
        [string]$FilePath,
        [string]$TransformMethod
    )
    
    $fileName = Split-Path $FilePath -Leaf
    Write-Host "  Transforming: $fileName" -ForegroundColor Gray
    
    switch ($TransformMethod) {
        "MSBuild" {
            # MSBuild approach: build the project which triggers T4
            $projectFile = Get-ChildItem -Path (Split-Path $FilePath -Parent) -Filter "*.csproj" | Select-Object -First 1
            if ($projectFile) {
                msbuild $projectFile.FullName /t:TransformAll /p:TransformOnBuild=true /nologo /verbosity:quiet
            }
        }
        "VSTextTransform" {
            $textTransformPath = Get-ChildItem -Path "${env:ProgramFiles}\Microsoft Visual Studio" -Filter "TextTransform.exe" -Recurse -ErrorAction SilentlyContinue | Select-Object -First 1
            if ($textTransformPath) {
                & $textTransformPath.FullName -out "$FilePath.cs" $FilePath
            }
        }
        "DotnetT4" {
            Push-Location (Split-Path $FilePath -Parent)
            try {
                dotnet t4 $fileName -o "$fileName.cs"
            } finally {
                Pop-Location
            }
        }
    }
}

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "T4 Template Transformation - $AggregateName" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

if (-not (Test-Path $aggregatePath)) {
    Write-Host "❌ Aggregate path not found: $aggregatePath" -ForegroundColor Red
    exit 1
}

$transformMethod = Get-T4TransformMethod
if (-not $transformMethod) {
    exit 1
}

Write-Host "Using method: $transformMethod" -ForegroundColor Cyan
Write-Host ""

# Define layer execution order (respects dependencies)
$layerOrder = @(
    @{Path = "$AggregateName.Enumerations"; Name = "Enumerations"},
    @{Path = "$AggregateName.Domain"; Name = "Domain"},
    @{Path = "$AggregateName.Application.DTO"; Name = "Application.DTO"},
    @{Path = "$AggregateName.Application"; Name = "Application"},
    @{Path = "$AggregateName.Infra.Data"; Name = "Infra.Data"},
    @{Path = "$AggregateName.Infra.IoC"; Name = "Infra.IoC"},
    @{Path = "$AggregateName.Api.Queries"; Name = "Api.Queries"},
    @{Path = "$AggregateName.Api"; Name = "Api"}
)

$testLayerOrder = @(
    @{Path = "Tests\Lazy.Crud.$AggregateName.Tests.Enumerations"; Name = "Tests.Enumerations"},
    @{Path = "Tests\Lazy.Crud.$AggregateName.Tests.Domain"; Name = "Tests.Domain"},
    @{Path = "Tests\Lazy.Crud.$AggregateName.Tests.DTO"; Name = "Tests.DTO"},
    @{Path = "Tests\Lazy.Crud.$AggregateName.Tests.Application"; Name = "Tests.Application"},
    @{Path = "Tests\Lazy.Crud.$AggregateName.Tests.InfraData"; Name = "Tests.InfraData"},
    @{Path = "Tests\Lazy.Crud.$AggregateName.Tests.InfraIoC"; Name = "Tests.InfraIoC"},
    @{Path = "Tests\Lazy.Crud.$AggregateName.Tests.ApiQueries"; Name = "Tests.ApiQueries"},
    @{Path = "Tests\Lazy.Crud.$AggregateName.Tests.Api"; Name = "Tests.Api"}
)

# Process production layers
Write-Host "[1] Processing Production Layers" -ForegroundColor Yellow
Write-Host ("=" * 60) -ForegroundColor Gray

foreach ($layer in $layerOrder) {
    $layerPath = Join-Path $aggregatePath $layer.Path
    if (Test-Path $layerPath) {
        Write-Host "Processing: $($layer.Name)" -ForegroundColor Cyan
        
        $t4Files = Get-ChildItem -Path $layerPath -Filter "*.tt" -ErrorAction SilentlyContinue
        foreach ($t4File in $t4Files) {
            Invoke-T4Transform -FilePath $t4File.FullName -TransformMethod $transformMethod
        }
        
        Write-Host "  ✓ $($layer.Name) completed" -ForegroundColor Green
    } else {
        Write-Host "  ⚠ Layer not found: $($layer.Path)" -ForegroundColor Yellow
    }
}

# Process test layers
if (-not $LayersOnly) {
    Write-Host ""
    Write-Host "[2] Processing Test Layers" -ForegroundColor Yellow
    Write-Host ("=" * 60) -ForegroundColor Gray
    
    foreach ($layer in $testLayerOrder) {
        $layerPath = Join-Path $aggregatePath $layer.Path
        if (Test-Path $layerPath) {
            Write-Host "Processing: $($layer.Name)" -ForegroundColor Cyan
            
            $t4Files = Get-ChildItem -Path $layerPath -Filter "*.tt" -ErrorAction SilentlyContinue
            foreach ($t4File in $t4Files) {
                Invoke-T4Transform -FilePath $t4File.FullName -TransformMethod $transformMethod
            }
            
            Write-Host "  ✓ $($layer.Name) completed" -ForegroundColor Green
        } else {
            Write-Host "  ⚠ Layer not found: $($layer.Path)" -ForegroundColor Yellow
        }
    }
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "T4 Transformation Completed" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "✓ All T4 templates processed" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "  1. Check for generated .cs files in each layer" -ForegroundColor White
Write-Host "  2. Run: dotnet build" -ForegroundColor White
Write-Host "  3. Run: dotnet test" -ForegroundColor White
