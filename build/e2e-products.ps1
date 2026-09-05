#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Superseded: runs the Lazy CRUD end-to-end battery (scripts/e2e.ps1).

.DESCRIPTION
    The previous script mutated this repository (rewrote src/Mods/lazy.settings, deleted and
    recreated src/Products, edited the solution) and depended on local sources. The platform now
    proves itself in temporary sandboxes built from public-equivalent packages and never touches the
    working tree apart from artifacts/. This wrapper forwards to scripts/e2e.ps1; pass -Scenario to
    run one scenario (for example -Scenario P1).
#>
[CmdletBinding()]
param(
    [string]$Scenario,
    [switch]$SkipPack
)
$repo = Split-Path $PSScriptRoot -Parent
$args = @('-NoProfile', '-File', (Join-Path $repo 'scripts/e2e.ps1'))
if ($Scenario) { $args += @('-Scenario', $Scenario) }
if ($SkipPack) { $args += '-SkipPack' }
& pwsh @args
exit $LASTEXITCODE
