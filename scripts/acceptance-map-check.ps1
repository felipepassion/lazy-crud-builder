#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Verifies that every test, validation check and harness scenario named in tests/acceptance-map.md
  still exists.

.DESCRIPTION
  The acceptance map records how each capability requirement of the OpenSpec change is covered. A
  renamed test or check would silently turn a row into a claim about nothing, so this script resolves
  every `test:`, `check:` and `harness:` name against the sources that define them:

    test:<Class>.<Method>  a [Fact] or [Theory] method in src/Platform/Lazy.Crud.Builder.Platform.Tests
    check:<id>             a report.Add("<id>", ...) call in the validation command
    harness:<id>           an Add-Scenario '<id>' in scripts/e2e.ps1 (TREE is added by the runner)

  Exit code 0 when every name resolves, 1 otherwise.
#>
[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$repo = Split-Path $PSScriptRoot -Parent
$map = Join-Path $repo 'tests/acceptance-map.md'
if (-not (Test-Path $map)) { Write-Error "acceptance map not found: $map"; exit 1 }
$text = Get-Content -Raw $map

# ---- known names -------------------------------------------------------------------------------
$tests = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
foreach ($file in Get-ChildItem (Join-Path $repo 'src/Platform/Lazy.Crud.Builder.Platform.Tests') -Filter *.cs -Recurse) {
  $class = $null
  foreach ($line in Get-Content $file.FullName) {
    if ($line -match '^\s*(?:public|internal)\s+(?:sealed\s+)?class\s+([A-Za-z0-9_]+)') { $class = $Matches[1] }
    elseif ($class -and $line -match '^\s*public\s+(?:async\s+)?(?:Task|void)\s+([A-Za-z0-9_]+)\s*\(') { [void]$tests.Add("$class.$($Matches[1])") }
  }
}

$checks = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
$validate = Get-Content -Raw (Join-Path $repo 'src/Platform/Lazy.Crud.Builder.Platform.Tool/Commands/ValidateCommand.cs')
foreach ($m in [regex]::Matches($validate, 'report\.Add\("([a-z0-9.\-]+)"')) { [void]$checks.Add($m.Groups[1].Value) }

$scenarios = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::Ordinal)
[void]$scenarios.Add('TREE')   # added by the runner itself, not by Add-Scenario
$harness = Get-Content -Raw (Join-Path $repo 'scripts/e2e.ps1')
foreach ($m in [regex]::Matches($harness, "Add-Scenario\s+'([A-Za-z0-9]+)'")) { [void]$scenarios.Add($m.Groups[1].Value) }

# ---- resolve every name used in the map ----------------------------------------------------------
$problems = @()
$counts = @{ test = 0; check = 0; harness = 0 }
foreach ($m in [regex]::Matches($text, '`(test|check|harness):([A-Za-z0-9_.\-]+)`')) {
  $kind = $m.Groups[1].Value
  $name = $m.Groups[2].Value
  $counts[$kind]++
  switch ($kind) {
    'test'    { if (-not $tests.Contains($name))     { $problems += "unknown test: $name" } }
    'check'   { if (-not $checks.Contains($name))    { $problems += "unknown validation check: $name" } }
    'harness' { if (-not $scenarios.Contains($name)) { $problems += "unknown harness scenario: $name" } }
  }
}

# ---- every capability spec has a section ---------------------------------------------------------
$specsDir = Join-Path $repo 'openspec/changes/relaunch-lazy-crud-builder-platform/specs'
if (Test-Path $specsDir) {
  foreach ($dir in Get-ChildItem $specsDir -Directory) {
    if ($text -notmatch [regex]::Escape("## $($dir.Name)")) { $problems += "capability without a section: $($dir.Name)" }
    $spec = Join-Path $dir.FullName 'spec.md'
    if (-not (Test-Path $spec)) { continue }
    foreach ($r in [regex]::Matches((Get-Content -Raw $spec), '(?m)^### Requirement:\s*(.+?)\s*$')) {
      $title = $r.Groups[1].Value
      if ($text -notmatch [regex]::Escape("| $title |")) { $problems += "requirement without a row: $($dir.Name) -> $title" }
    }
  }
}

$problems = @($problems | Sort-Object -Unique)
Write-Host ("acceptance map: {0} test reference(s), {1} check reference(s), {2} scenario reference(s)" -f $counts.test, $counts.check, $counts.harness)
if ($problems.Count -gt 0) {
  $problems | ForEach-Object { Write-Host "  $_" -ForegroundColor Red }
  Write-Host "RESULT: FAIL ($($problems.Count) unresolved reference(s))" -ForegroundColor Red
  exit 1
}
Write-Host 'RESULT: OK (every reference resolves and every requirement has a row)' -ForegroundColor Green
exit 0
