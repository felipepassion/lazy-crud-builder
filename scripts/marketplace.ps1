#!/usr/bin/env pwsh
<#
.SYNOPSIS
  Runs the whole orchestration against the multi-aggregate marketplace fixture: one solution with four
  aggregates, generated one after another through the real Visual Studio T4 toolchain, then built and
  tested.

.DESCRIPTION
  The reference aggregate proves a single aggregate end to end. This proves the shape a real product
  has: several aggregates in one solution, entities that reference each other across aggregate
  boundaries by id, unique business keys, enums delivered by each aggregate's Enumerations project.

  Every step is asserted, and the report of each generation is kept, so a failure names the aggregate,
  the template and the diagnostic rather than a bare exit code.

.PARAMETER Output
  Directory that receives the solution (default: %LOCALAPPDATA%\lazycrud\marketplace\<stamp>, or pass a
  short path outside AppData when running from a process with MSIX package identity).

.PARAMETER PackageVersion
  Lazy CRUD package version to consume (default: LazyCrudVersion from Directory.Build.props).

.PARAMETER Feed
  Local folder feed holding those packages (default: artifacts/nuget/<PackageVersion>).

.PARAMETER Configuration
  Build configuration of the platform tool to drive the run (default: Release).

.PARAMETER SkipTests
  Generate and build, but do not run the generated tests.

.PARAMETER StepTimeoutMinutes
  Budget for a single command: creating, generating, building or testing (default 20). A step that
  exceeds it is killed with its process tree, so a Visual Studio session that stops responding cannot
  hold the run open indefinitely.

.PARAMETER GlobalTimeoutMinutes
  Budget for the whole run (default 120). Checked before each step.

.EXAMPLE
  pwsh scripts/marketplace.ps1
  pwsh scripts/marketplace.ps1 -Output C:\work\marketplace -PackageVersion 1.1.0
#>
[CmdletBinding()]
param(
  [string]$Output,
  [string]$PackageVersion,
  [string]$Feed,
  [string]$Configuration = 'Release',
  [switch]$SkipTests,
  [int]$StepTimeoutMinutes = 20,
  [int]$GlobalTimeoutMinutes = 120
)

$ErrorActionPreference = 'Stop'
$repo = Split-Path $PSScriptRoot -Parent
Set-Location $repo

$aggregates = @('Catalog', 'Customers', 'Orders', 'Sellers')
$stamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$deadline = (Get-Date).AddMinutes($GlobalTimeoutMinutes)

# Every child runs under a budget and is killed with its process tree when it expires. Without this a
# Visual Studio session that stops responding keeps the whole run alive with nothing to show for it.
function Invoke-Step([string]$file, [string[]]$arguments, [string]$logPath, [string]$label) {
  if ((Get-Date) -gt $deadline) { throw "$label not started: the run exceeded its ${GlobalTimeoutMinutes}-minute budget" }
  $psi = [System.Diagnostics.ProcessStartInfo]::new($file)
  foreach ($a in $arguments) { $psi.ArgumentList.Add($a) }
  $psi.WorkingDirectory = $repo
  $psi.RedirectStandardOutput = $true
  $psi.RedirectStandardError = $true
  $psi.UseShellExecute = $false
  $psi.CreateNoWindow = $true
  $p = [System.Diagnostics.Process]::Start($psi)
  $stdout = $p.StandardOutput.ReadToEndAsync()
  $stderr = $p.StandardError.ReadToEndAsync()
  $budget = [int][Math]::Min($StepTimeoutMinutes * 60, [Math]::Max(60, ($deadline - (Get-Date)).TotalSeconds))
  if (-not $p.WaitForExit($budget * 1000)) {
    try { $p.Kill($true) } catch { }
    Set-Content -Path $logPath -Value ("TIMEOUT after $budget s`n" + $stdout.Result + "`n" + $stderr.Result)
    throw "$label exceeded $budget s and was killed with its process tree (log: $logPath)"
  }
  $p.WaitForExit()
  Set-Content -Path $logPath -Value ("$ $file $($arguments -join ' ')`nexit=$($p.ExitCode)`n" + $stdout.Result + "`n--- stderr ---`n" + $stderr.Result)
  return $p.ExitCode
}

function Get-CentralVersion {
  $xml = [xml](Get-Content -Raw (Join-Path $repo 'Directory.Build.props'))
  $v = $xml.Project.PropertyGroup | ForEach-Object { $_.LazyCrudVersion } | Where-Object { $_ } | ForEach-Object { if ($_ -is [System.Xml.XmlElement]) { $_.InnerText } else { [string]$_ } } | Select-Object -First 1
  if (-not $v) { throw 'LazyCrudVersion is not defined in Directory.Build.props' }
  return ([string]$v).Trim()
}

if (-not $PackageVersion) { $PackageVersion = Get-CentralVersion }
if (-not $Feed) { $Feed = Join-Path $repo "artifacts/nuget/$PackageVersion" }
if (-not (Test-Path $Feed)) { throw "package feed not found: $Feed (run pack-all.ps1 first, or pass -Feed)" }
if (-not $Output) { $Output = Join-Path ([Environment]::GetFolderPath('LocalApplicationData')) "lazycrud/marketplace/$stamp" }

$tool = Join-Path $repo "src/Platform/Lazy.Crud.Builder.Platform.Tool/bin/$Configuration/net10.0/Lazy.Crud.Builder.Platform.Tool.dll"
if (-not (Test-Path $tool)) { throw "platform tool not built: $tool (dotnet build LazyCrudBuilder.sln -c $Configuration)" }

$fixtures = Join-Path $repo 'tests/fixtures/marketplace'
$reportDir = Join-Path $repo "artifacts/marketplace/$stamp"
New-Item -ItemType Directory -Force -Path $reportDir | Out-Null

Write-Host "[marketplace] solution: $Output" -ForegroundColor Cyan
Write-Host "[marketplace] packages: $PackageVersion from $Feed" -ForegroundColor Cyan
Write-Host "[marketplace] evidence: $reportDir" -ForegroundColor Cyan

$summary = [ordered]@{
  runId = $stamp; startedUtc = (Get-Date).ToUniversalTime().ToString('o')
  solution = $Output; packageVersion = $PackageVersion; configuration = $Configuration
  aggregates = @(); build = $null; tests = $null; verdict = ''
}

# ---- 1. one solution, four aggregates ------------------------------------------------------------
foreach ($agg in $aggregates) {
  Write-Host "[marketplace] creating $agg" -ForegroundColor Yellow
  $create = @($tool, 'sandbox', 'create', '--output', $Output, '--aggregate', $agg,
              '--fixture', (Join-Path $fixtures $agg), '--version', $PackageVersion, '--feed', $Feed)
  $code = Invoke-Step 'dotnet' $create (Join-Path $reportDir "create-$agg.log") "create-$agg"
  if ($code -ne 0) { throw "creating $agg failed with exit code $code (log: $reportDir/create-$agg.log)" }
}

# ---- 2. generate each aggregate through the real toolchain ---------------------------------------
foreach ($agg in $aggregates) {
  Write-Host "[marketplace] generating $agg" -ForegroundColor Yellow
  $report = Join-Path $reportDir "generate-$agg.json"
  $started = Get-Date
  $exit = Invoke-Step 'dotnet' @($tool, 'generate', '--solution', $Output, '--aggregate', $agg, '--report', $report) `
                      (Join-Path $reportDir "generate-$agg.log") "generate-$agg"
  $elapsed = [math]::Round(((Get-Date) - $started).TotalSeconds, 1)

  $doc = if (Test-Path $report) { Get-Content -Raw $report | ConvertFrom-Json -Depth 24 } else { $null }
  $errors = if ($doc) { @($doc.Diagnostics | Where-Object { $_.Severity -eq 'error' }) } else { @() }
  $summary.aggregates += [ordered]@{
    id = $agg; exitCode = $exit; seconds = $elapsed
    transformed = if ($doc) { @($doc.TransformedTemplates).Count } else { 0 }
    manifestFiles = if ($doc -and $doc.Manifest) { [int]$doc.Manifest.Files } else { 0 }
    errors = @($errors | ForEach-Object { "$($_.Code) $($_.Path): $($_.Message)" })
  }
  if ($exit -ne 0) {
    Write-Host "  FAILED ($exit)" -ForegroundColor Red
    $errors | Select-Object -First 5 | ForEach-Object { Write-Host "    $($_.Code) $($_.Path)" -ForegroundColor Red }
    $summary.verdict = "RESULT: FAIL (generation of $agg exited $exit)"
    $summary | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $reportDir 'summary.json')
    Write-Host $summary.verdict -ForegroundColor Red
    exit $exit
  }
  Write-Host ("  ok in {0}s, {1} template(s), {2} manifest entrie(s)" -f $elapsed, $summary.aggregates[-1].transformed, $summary.aggregates[-1].manifestFiles) -ForegroundColor Green
}

# ---- 3. the whole solution compiles --------------------------------------------------------------
Write-Host '[marketplace] building the solution' -ForegroundColor Yellow
$sln = Join-Path $Output 'Sandbox.sln'
$summary.build = Invoke-Step 'dotnet' @('build', $sln, '-c', 'Debug', '--nologo', '-v', 'q') (Join-Path $reportDir 'build.log') 'build'
if ($summary.build -ne 0) {
  Select-String -Path (Join-Path $reportDir 'build.log') -Pattern ' error ' | Select-Object -First 10 |
    ForEach-Object { Write-Host "  $($_.Line)" -ForegroundColor Red }
  $summary.verdict = 'RESULT: FAIL (the generated solution does not build)'
  $summary | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $reportDir 'summary.json')
  Write-Host $summary.verdict -ForegroundColor Red
  exit 1
}
Write-Host '  build ok' -ForegroundColor Green

# ---- 4. the generated tests pass -----------------------------------------------------------------
if (-not $SkipTests) {
  Write-Host '[marketplace] running the generated tests' -ForegroundColor Yellow
  $summary.tests = Invoke-Step 'dotnet' @('test', $sln, '-c', 'Debug', '--no-build', '--nologo', '-v', 'q') (Join-Path $reportDir 'test.log') 'test'
  if ($summary.tests -ne 0) {
    Select-String -Path (Join-Path $reportDir 'test.log') -Pattern 'Failed|error' | Select-Object -First 10 |
      ForEach-Object { Write-Host "  $($_.Line)" -ForegroundColor Red }
    $summary.verdict = 'RESULT: FAIL (generated tests failed)'
    $summary | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $reportDir 'summary.json')
    Write-Host $summary.verdict -ForegroundColor Red
    exit 1
  }
  $passed = (Select-String -Path (Join-Path $reportDir 'test.log') -Pattern 'Passed:\s+(\d+)' -AllMatches).Matches |
    ForEach-Object { [int]$_.Groups[1].Value } | Measure-Object -Sum
  Write-Host "  $($passed.Sum) generated test(s) passed" -ForegroundColor Green
  $summary.testsPassed = $passed.Sum
}

$summary.finishedUtc = (Get-Date).ToUniversalTime().ToString('o')
$summary.verdict = "RESULT: OK ($($aggregates.Count) aggregates generated, built$(if (-not $SkipTests) { ' and tested' }))"
$summary | ConvertTo-Json -Depth 8 | Set-Content (Join-Path $reportDir 'summary.json')
Write-Host $summary.verdict -ForegroundColor Green
exit 0
