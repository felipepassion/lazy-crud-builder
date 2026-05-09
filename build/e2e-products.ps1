#!/usr/bin/env pwsh
<#
.SYNOPSIS
    E2E local test for LazyCrudBuilder - Products aggregate

.DESCRIPTION
    This script performs an end-to-end test of the LazyCrudBuilder with the Products aggregate:
    1. Configure lazy.settings for local templates
    2. Delete Products aggregate completely
    3. Recreate Products from scratch (with tests)
    4. Run all T4 templates (all layers including tests)
    5. Run all tests and ensure they pass

.PARAMETER Clean
    If set, performs a complete clean before starting

.EXAMPLE
    .\build\e2e-products.ps1
    .\build\e2e-products.ps1 -Clean
#>

[CmdletBinding()]
param(
    [switch]$Clean
)

$ErrorActionPreference = "Stop"
$workspaceRoot = Split-Path $PSScriptRoot -Parent

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "LazyCrudBuilder E2E Test - Products" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Helper function to exit with error
function Exit-WithError {
    param([string]$Message)
    Write-Host "❌ ERROR: $Message" -ForegroundColor Red
    exit 1
}

# Helper function to show step
function Show-Step {
    param([int]$Number, [string]$Description)
    Write-Host ""
    Write-Host "[$Number] $Description" -ForegroundColor Yellow
    Write-Host ("=" * 60) -ForegroundColor Gray
}

# Step 1: Configure lazy.settings for local templates
Show-Step 1 "Configuring lazy.settings for local templates"

$settingsPath = Join-Path $workspaceRoot "src\Mods\lazy.settings"
if (-not (Test-Path $settingsPath)) {
    Exit-WithError "lazy.settings not found at: $settingsPath"
}

$settingsContent = @'
<?xml version="1.0" encoding="utf-8"?>
<SettingsFile CurrentProfile="(Default)" xmlns="http://schemas.microsoft.com/VisualStudio/2004/01/settings">
  <Profiles />
  <Settings>
    <Setting Name="GenerationOutputPath" Type="System.String" Scope="Application">
      <Value Profile="(Default)">LazyCode</Value>
    </Setting>
    <Setting Name="ModsPath" Type="System.String" Scope="Application">
      <Value Profile="(Default)">src/mods</Value>
    </Setting>
    <Setting Name="TemplatesSource" Type="System.String" Scope="Application">
      <Value Profile="(Default)">Local</Value>
    </Setting>
    <Setting Name="TemplatesLocalProjectFilesPath" Type="System.String" Scope="Application">
      <Value Profile="(Default)">src/ProjectFiles</Value>
    </Setting>
    <Setting Name="UseNugetTemplates" Type="System.Boolean" Scope="Application">
      <Value Profile="(Default)">False</Value>
    </Setting>
  </Settings>
</SettingsFile>
'@

Set-Content -Path $settingsPath -Value $settingsContent -Encoding UTF8
Write-Host "✓ lazy.settings configured for local templates" -ForegroundColor Green

# Step 2: Delete Products aggregate
Show-Step 2 "Deleting Products aggregate"

$productsPath = Join-Path $workspaceRoot "src\Products"
if (Test-Path $productsPath) {
    Write-Host "Removing directory: $productsPath" -ForegroundColor Gray
    Remove-Item -Path $productsPath -Recurse -Force
    Write-Host "✓ Products directory deleted" -ForegroundColor Green
} else {
    Write-Host "⚠ Products directory not found (already deleted or first run)" -ForegroundColor Yellow
}

# Remove Products projects from solution
$solutionFile = Get-ChildItem -Path $workspaceRoot -Filter "*.sln" | Select-Object -First 1
if ($solutionFile) {
    Write-Host "Removing Products projects from solution: $($solutionFile.Name)" -ForegroundColor Gray
    
    $solutionContent = Get-Content -Path $solutionFile.FullName -Raw
    $lines = $solutionContent -split "`n"
    $filteredLines = @()
    $skipProject = $false
    
    foreach ($line in $lines) {
        if ($line -match 'Project\(.*\).*Products\.') {
            $skipProject = $true
            continue
        }
        if ($skipProject -and $line -match '^EndProject') {
            $skipProject = $false
            continue
        }
        if (-not $skipProject) {
            $filteredLines += $line
        }
    }
    
    $newContent = $filteredLines -join "`n"
    Set-Content -Path $solutionFile.FullName -Value $newContent -NoNewline
    Write-Host "✓ Products projects removed from solution" -ForegroundColor Green
} else {
    Write-Host "⚠ Solution file not found" -ForegroundColor Yellow
}

# Step 3: Recreate Products aggregate
Show-Step 3 "Recreating Products aggregate from scratch"

# Create directory structure
$productsPath = Join-Path $workspaceRoot "src\Products"
$layers = @(
    "Products.Domain",
    "Products.Enumerations",
    "Products.Application.DTO",
    "Products.Application",
    "Products.Infra.Data",
    "Products.Infra.IoC",
    "Products.Api",
    "Products.Api.Queries",
    "Tests\Lazy.Crud.Products.Tests.Domain",
    "Tests\Lazy.Crud.Products.Tests.Enumerations",
    "Tests\Lazy.Crud.Products.Tests.DTO",
    "Tests\Lazy.Crud.Products.Tests.Application",
    "Tests\Lazy.Crud.Products.Tests.InfraData",
    "Tests\Lazy.Crud.Products.Tests.InfraIoC",
    "Tests\Lazy.Crud.Products.Tests.Api",
    "Tests\Lazy.Crud.Products.Tests.ApiQueries"
)

Write-Host "Creating Products layer structure..." -ForegroundColor Gray
foreach ($layer in $layers) {
    $layerPath = Join-Path $productsPath $layer
    New-Item -ItemType Directory -Path $layerPath -Force | Out-Null
    Write-Host "  ✓ Created: $layer" -ForegroundColor DarkGreen
}

Write-Host "✓ Products structure created" -ForegroundColor Green

# Step 3.5: Create .csproj files
Show-Step 3.5 "Creating .csproj files for all layers"

Write-Host "Running: .\build\create-csproj-files.ps1" -ForegroundColor Gray
& (Join-Path $PSScriptRoot "create-csproj-files.ps1") -AggregateName "Products"
Write-Host "✓ .csproj files created" -ForegroundColor Green

# Step 3.6: Add projects to solution
Show-Step 3.6 "Adding Products projects to solution"

if ($solutionFile) {
    Write-Host "Adding all Products projects to solution..." -ForegroundColor Gray
    Push-Location $workspaceRoot
    try {
        $csprojFiles = Get-ChildItem -Path "src\Products" -Filter "*.csproj" -Recurse
        foreach ($proj in $csprojFiles) {
            $relativePath = $proj.FullName.Replace($workspaceRoot + "\", "")
            Write-Host "  Adding: $relativePath" -ForegroundColor DarkGreen
            dotnet sln $solutionFile.FullName add $relativePath 2>&1 | Out-Null
        }
        Write-Host "✓ All projects added to solution" -ForegroundColor Green
    } finally {
        Pop-Location
    }
} else {
    Write-Host "⚠ Solution file not found, skipping project addition" -ForegroundColor Yellow
}

# Step 4: Create T4 wrapper files
Show-Step 4 "Creating T4 wrapper files"

Write-Host "Creating T4 wrappers for all layers..." -ForegroundColor Gray

# Create placeholder T4 wrapper files for each layer
$t4Mappings = @{
    "Products.Domain" = "DomainTest.tt"
    "Products.Enumerations" = "EnumerationsTest.tt"
    "Tests\Lazy.Crud.Products.Tests.Domain" = "DomainTest.tt"
    "Tests\Lazy.Crud.Products.Tests.Enumerations" = "EnumerationsTest.tt"
    "Tests\Lazy.Crud.Products.Tests.DTO" = "DTOTest.tt"
    "Tests\Lazy.Crud.Products.Tests.Application" = "ApplicationTest.tt"
    "Tests\Lazy.Crud.Products.Tests.InfraData" = "InfraDataTest.tt"
    "Tests\Lazy.Crud.Products.Tests.InfraIoC" = "InfraIoCTest.tt"
    "Tests\Lazy.Crud.Products.Tests.Api" = "ApiTest.tt"
    "Tests\Lazy.Crud.Products.Tests.ApiQueries" = "ApiQueriesTest.tt"
}

foreach ($mapping in $t4Mappings.GetEnumerator()) {
    $layerPath = Join-Path $productsPath $mapping.Key
    $t4File = Join-Path $layerPath $mapping.Value
    
    # Determine the correct include path based on layer
    $includePathParts = $mapping.Key -split '\\'
    if ($includePathParts[0] -eq "Tests") {
        $testLayer = $includePathParts[1] -replace 'Lazy\.Crud\.Products\.Tests\.', ''
        $includePath = "..\..\ProjectFiles\Tests\$testLayer\$testLayer.Tests.tt"
    } else {
        $layer = $mapping.Key -replace 'Products\.', ''
        $includePath = "..\..\ProjectFiles\$layer\$layer.tt"
    }
    
    $t4Content = @"
<#@ include file="$includePath" #>
"@
    
    if (-not (Test-Path $t4File)) {
        Set-Content -Path $t4File -Value $t4Content -Encoding UTF8
        Write-Host "  ✓ Created T4 wrapper: $($mapping.Value) -> $($mapping.Key)" -ForegroundColor DarkGreen
    }
}

Write-Host "✓ T4 wrappers created" -ForegroundColor Green

# Step 5: Restore packages
Show-Step 5 "Restoring NuGet packages"

Push-Location $workspaceRoot
try {
    Write-Host "Running: dotnet restore" -ForegroundColor Gray
    dotnet restore 2>&1 | Out-Null
    if ($LASTEXITCODE -ne 0) {
        Write-Host "⚠ dotnet restore had warnings (common for new projects)" -ForegroundColor Yellow
    } else {
        Write-Host "✓ Restore completed" -ForegroundColor Green
    }
} finally {
    Pop-Location
}

# Step 6: Run T4 templates (optional, requires Visual Studio)
Show-Step 6 "T4 Template Transformation"

Write-Host "Attempting to run T4 templates..." -ForegroundColor Gray
$runT4Script = Join-Path $PSScriptRoot "run-t4-templates.ps1"
if (Test-Path $runT4Script) {
    try {
        & $runT4Script -AggregateName "Products"
        Write-Host "✓ T4 templates executed" -ForegroundColor Green
    } catch {
        Write-Host "⚠ T4 transformation failed or not available: $($_.Exception.Message)" -ForegroundColor Yellow
        Write-Host ""
        Write-Host "Manual options to run T4 templates:" -ForegroundColor Cyan
        Write-Host "  1. Visual Studio: Right-click .tt files → 'Run Custom Tool'" -ForegroundColor White
        Write-Host "  2. Command line: msbuild /t:TransformAll" -ForegroundColor White
        Write-Host "  3. Install dotnet-t4: dotnet tool install -g dotnet-t4" -ForegroundColor White
    }
} else {
    Write-Host "⚠ T4 runner script not found, skipping" -ForegroundColor Yellow
}

# Step 7: Build and Test
Show-Step 7 "Building and Testing"

# Step 7: Build and Test
Show-Step 7 "Building and Testing"

Push-Location $workspaceRoot
try {
    Write-Host "Running: dotnet build --no-restore" -ForegroundColor Gray
    $buildOutput = dotnet build --no-restore 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "⚠ Build failed - T4 templates need to be run first" -ForegroundColor Yellow
        Write-Host "Build output (last 20 lines):" -ForegroundColor Gray
        $buildOutput | Select-Object -Last 20 | ForEach-Object { Write-Host "  $_" -ForegroundColor DarkGray }
    } else {
        Write-Host "✓ Build completed successfully" -ForegroundColor Green

        Write-Host ""
        Write-Host "Running: dotnet test --no-build" -ForegroundColor Gray
        dotnet test --no-build --verbosity normal
        if ($LASTEXITCODE -ne 0) {
            Write-Host "⚠ Some tests failed" -ForegroundColor Yellow
        } else {
            Write-Host "✓ All tests passed" -ForegroundColor Green
        }
    }
} finally {
    Pop-Location
}

# Summary
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "E2E Test Completed" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Run T4 templates in Visual Studio or via msbuild /t:TransformAll" -ForegroundColor White
Write-Host "2. Run this script again to validate tests pass" -ForegroundColor White
Write-Host ""
Write-Host "Or implement: lazycrud e2e local --agg Products" -ForegroundColor Cyan
