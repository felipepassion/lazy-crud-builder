#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Superseded: scaffolds a consumer solution for an aggregate with the platform tool.

.DESCRIPTION
    The previous script wrote hand-maintained .csproj files with stale package versions. The
    platform tool scaffolds the canonical consumer layout (layer projects, package references with
    GeneratePathProperty, wrapper templates, test projects, lazy.settings, nuget.config, solution)
    from the same layout the descriptor publishes.

.PARAMETER AggregateName
    Aggregate name (default Products).

.PARAMETER Output
    Target directory for the new solution (default: a folder named after the aggregate under the current directory).

.PARAMETER Version
    Lazy CRUD package version to reference (default: LazyCrudVersion from Directory.Build.props).

.PARAMETER Feed
    Optional local folder feed (for example artifacts/nuget/<version>) added to the sandbox nuget.config.
#>
[CmdletBinding()]
param(
    [string]$AggregateName = 'Products',
    [string]$Output,
    [string]$Version,
    [string]$Feed,
    [string]$Configuration = 'Release'
)
$ErrorActionPreference = 'Stop'
$repo = Split-Path $PSScriptRoot -Parent
if (-not $Version) {
    $xml = [xml](Get-Content -Raw (Join-Path $repo 'Directory.Build.props'))
    $Version = ([string]($xml.Project.PropertyGroup | ForEach-Object { $_.LazyCrudVersion } | Where-Object { $_ } | ForEach-Object { if ($_ -is [System.Xml.XmlElement]) { $_.InnerText } else { [string]$_ } } | Select-Object -First 1)).Trim()
}
if (-not $Output) { $Output = Join-Path (Get-Location) $AggregateName }
$tool = Join-Path $repo "src/Platform/Lazy.Crud.Builder.Platform.Tool/bin/$Configuration/net10.0/Lazy.Crud.Builder.Platform.Tool.dll"
if (-not (Test-Path $tool)) { throw "Platform tool not built: $tool (dotnet build LazyCrudBuilder.sln -c $Configuration)" }
$args = @('scaffold', '--output', $Output, '--aggregate', $AggregateName, '--version', $Version)
if ($Feed) { $args += @('--feed', $Feed) }
& dotnet $tool @args
exit $LASTEXITCODE
