#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Creates .csproj files for all Products layers

.DESCRIPTION
    Generates proper .csproj files for all Products aggregate layers
    with correct PackageReferences and ProjectReferences
#>

param(
    [Parameter(Mandatory=$false)]
    [string]$AggregateName = "Products",
    
    [Parameter(Mandatory=$false)]
    [string]$TargetFramework = "net10.0"
)

$ErrorActionPreference = "Stop"
$workspaceRoot = Split-Path $PSScriptRoot -Parent
$aggregatePath = Join-Path $workspaceRoot "src\$AggregateName"

function New-CsprojFile {
    param(
        [string]$ProjectPath,
        [string]$ProjectName,
        [string]$Layer,
        [bool]$IsTest = $false
    )
    
    $csprojPath = Join-Path $ProjectPath "$ProjectName.csproj"
    
    if (Test-Path $csprojPath) {
        Write-Host "  ⚠ $ProjectName.csproj already exists, skipping" -ForegroundColor Yellow
        return
    }
    
    $content = @"
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>$TargetFramework</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
"@

    if ($IsTest) {
        $content += @"

    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
"@
    }
    
    $content += @"

  </PropertyGroup>

"@

    # Add common packages
    $content += "  <ItemGroup>`n"
    
    switch ($Layer) {
        "Domain" {
            $content += @"
    <PackageReference Include="MediatR" Version="12.4.0" />
    <PackageReference Include="FluentValidation" Version="11.9.0" />
"@
        }
        "Enumerations" {
            # Usually minimal dependencies
        }
        "Application.DTO" {
            $content += @"
    <PackageReference Include="AutoMapper" Version="13.0.1" />
    <PackageReference Include="FluentValidation" Version="11.9.0" />
"@
        }
        "Application" {
            $content += @"
    <PackageReference Include="MediatR" Version="12.4.0" />
    <PackageReference Include="AutoMapper" Version="13.0.1" />
    <PackageReference Include="FluentValidation" Version="11.9.0" />
"@
        }
        "Infra.Data" {
            $content += @"
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
"@
        }
        "Infra.IoC" {
            $content += @"
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="9.0.0" />
"@
        }
        "Api" {
            $content += @"
    <PackageReference Include="Microsoft.AspNetCore.Mvc.Core" Version="2.2.5" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
"@
        }
        "Api.Queries" {
            # Usually minimal dependencies
        }
    }
    
    # Test packages
    if ($IsTest) {
        $content += @"
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.11.0" />
    <PackageReference Include="xunit" Version="2.9.0" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.8.2">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="coverlet.collector" Version="6.0.2">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
"@
    }
    
    $content += "  </ItemGroup>`n`n"
    
    # Add project references
    if ($Layer -ne "Domain" -and $Layer -ne "Enumerations") {
        $content += "  <ItemGroup>`n"
        
        # Layer dependencies
        $dependencies = @()
        switch ($Layer) {
            "Application.DTO" {
                $dependencies = @("Domain", "Enumerations")
            }
            "Application" {
                $dependencies = @("Domain", "Application.DTO", "Enumerations")
            }
            "Infra.Data" {
                $dependencies = @("Domain", "Enumerations")
            }
            "Infra.IoC" {
                $dependencies = @("Domain", "Application", "Application.DTO", "Infra.Data", "Enumerations")
            }
            "Api" {
                $dependencies = @("Domain", "Application", "Application.DTO", "Enumerations")
            }
            "Api.Queries" {
                $dependencies = @("Domain", "Application.DTO", "Enumerations")
            }
        }
        
        # Test dependencies
        if ($IsTest) {
            $testLayer = $Layer -replace "Tests\.", ""
            $dependencies = @($testLayer)
        }
        
        foreach ($dep in $dependencies) {
            $depProject = "Lazy.Crud.$AggregateName.$dep"
            $depPath = "..\$AggregateName.$dep\$depProject.csproj"
            $content += "    <ProjectReference Include=`"$depPath`" />`n"
        }
        
        $content += "  </ItemGroup>`n`n"
    }
    
    # Add T4 transform on build
    $content += @"
  <ItemGroup>
    <None Update="**\*.tt">
      <Generator>TextTemplatingFileGenerator</Generator>
      <LastGenOutput>%(Filename).cs</LastGenOutput>
    </None>
    <Compile Update="**\*.generated.cs">
      <DependentUpon>%(Filename).tt</DependentUpon>
      <AutoGen>True</AutoGen>
      <DesignTime>True</DesignTime>
    </Compile>
  </ItemGroup>

</Project>
"@

    Set-Content -Path $csprojPath -Value $content -Encoding UTF8
    Write-Host "  ✓ Created: $ProjectName.csproj" -ForegroundColor Green
}

Write-Host "Creating .csproj files for $AggregateName aggregate" -ForegroundColor Cyan
Write-Host ""

# Production layers
$layers = @(
    @{Name = "Lazy.Crud.$AggregateName.Domain"; Path = "$AggregateName.Domain"; Layer = "Domain"},
    @{Name = "Lazy.Crud.$AggregateName.Enumerations"; Path = "$AggregateName.Enumerations"; Layer = "Enumerations"},
    @{Name = "Lazy.Crud.$AggregateName.Application.DTO"; Path = "$AggregateName.Application.DTO"; Layer = "Application.DTO"},
    @{Name = "Lazy.Crud.$AggregateName.Application"; Path = "$AggregateName.Application"; Layer = "Application"},
    @{Name = "Lazy.Crud.$AggregateName.Infra.Data"; Path = "$AggregateName.Infra.Data"; Layer = "Infra.Data"},
    @{Name = "Lazy.Crud.$AggregateName.Infra.IoC"; Path = "$AggregateName.Infra.IoC"; Layer = "Infra.IoC"},
    @{Name = "Lazy.Crud.$AggregateName.Api"; Path = "$AggregateName.Api"; Layer = "Api"},
    @{Name = "Lazy.Crud.$AggregateName.Api.Queries"; Path = "$AggregateName.Api.Queries"; Layer = "Api.Queries"}
)

foreach ($layer in $layers) {
    $projectPath = Join-Path $aggregatePath $layer.Path
    if (Test-Path $projectPath) {
        New-CsprojFile -ProjectPath $projectPath -ProjectName $layer.Name -Layer $layer.Layer
    } else {
        Write-Host "  ⚠ Directory not found: $projectPath" -ForegroundColor Yellow
    }
}

# Test layers
Write-Host ""
Write-Host "Creating test projects..." -ForegroundColor Cyan

$testLayers = @(
    @{Name = "Lazy.Crud.$AggregateName.Tests.Domain"; Path = "Tests\Lazy.Crud.$AggregateName.Tests.Domain"; Layer = "Domain"},
    @{Name = "Lazy.Crud.$AggregateName.Tests.Enumerations"; Path = "Tests\Lazy.Crud.$AggregateName.Tests.Enumerations"; Layer = "Enumerations"},
    @{Name = "Lazy.Crud.$AggregateName.Tests.DTO"; Path = "Tests\Lazy.Crud.$AggregateName.Tests.DTO"; Layer = "Application.DTO"},
    @{Name = "Lazy.Crud.$AggregateName.Tests.Application"; Path = "Tests\Lazy.Crud.$AggregateName.Tests.Application"; Layer = "Application"},
    @{Name = "Lazy.Crud.$AggregateName.Tests.InfraData"; Path = "Tests\Lazy.Crud.$AggregateName.Tests.InfraData"; Layer = "Infra.Data"},
    @{Name = "Lazy.Crud.$AggregateName.Tests.InfraIoC"; Path = "Tests\Lazy.Crud.$AggregateName.Tests.InfraIoC"; Layer = "Infra.IoC"},
    @{Name = "Lazy.Crud.$AggregateName.Tests.Api"; Path = "Tests\Lazy.Crud.$AggregateName.Tests.Api"; Layer = "Api"},
    @{Name = "Lazy.Crud.$AggregateName.Tests.ApiQueries"; Path = "Tests\Lazy.Crud.$AggregateName.Tests.ApiQueries"; Layer = "Api.Queries"}
)

foreach ($layer in $testLayers) {
    $projectPath = Join-Path $aggregatePath $layer.Path
    if (Test-Path $projectPath) {
        New-CsprojFile -ProjectPath $projectPath -ProjectName $layer.Name -Layer $layer.Layer -IsTest $true
    } else {
        Write-Host "  ⚠ Directory not found: $projectPath" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "✓ All .csproj files created successfully" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Add projects to solution: dotnet sln add src/$AggregateName/**/*.csproj" -ForegroundColor White
Write-Host "2. Restore packages: dotnet restore" -ForegroundColor White
Write-Host "3. Run T4 templates" -ForegroundColor White
Write-Host "4. Build: dotnet build" -ForegroundColor White
