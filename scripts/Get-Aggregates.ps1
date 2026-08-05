#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Reads aggregates.manifest.json and returns only real aggregates (filtering sub-folders like Tests).

.DESCRIPTION
    Aggregates manifest can contain entries that are sub-folders of other aggregates
    (e.g., "Tests" is a sub-folder of "Products", not a standalone aggregate).
    This function filters them out, returning only top-level aggregates.

.PARAMETER ManifestPath
    Path to the aggregates.manifest.json file.

.PARAMETER Raw
    If set, returns all entries without filtering.

.EXAMPLE
    . .\scripts\Get-Aggregates.ps1
    $aggregates = Get-FilteredAggregates -ManifestPath "src/aggregates.manifest.json"
    $aggregates | Select-Object Name
#>

# Sub-folder names that should never appear as top-level aggregates.
# Convention: Tests live inside aggregate folders at src/{Aggregate}/Tests/
$script:SubFolderNames = @("Tests")

function Get-FilteredAggregates {
    param(
        [Parameter(Mandatory=$true)]
        [string]$ManifestPath,

        [switch]$Raw
    )

    if (-not (Test-Path $ManifestPath)) {
        Write-Warning "Manifest not found: $ManifestPath"
        return @()
    }

    $content = [System.IO.File]::ReadAllText((Resolve-Path $ManifestPath).Path, [System.Text.Encoding]::UTF8)
    $manifest = $content | ConvertFrom-Json

    if ($null -eq $manifest.Aggregates) {
        return @()
    }

    if ($Raw) {
        return $manifest.Aggregates
    }

    # Filter out sub-folder aggregates
    $filtered = $manifest.Aggregates | Where-Object {
        $_.Name -notin $script:SubFolderNames
    }

    return $filtered
}

Export-ModuleMember -Function Get-FilteredAggregates -Variable SubFolderNames 2>$null
