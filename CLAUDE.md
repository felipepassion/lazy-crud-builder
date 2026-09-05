# Working in this repository

## Long-running commands: never leave one unbounded

Generation drives a real Visual Studio through COM automation. When that session stops responding the
call does not return, so any command that waits on it hangs forever unless something kills it. This has
already cost more than an hour of wall time twice. The rules below are not style preferences.

**Every command that starts a child process must give it a deadline and kill its process tree when the
deadline passes.** `scripts/e2e.ps1` and `scripts/marketplace.ps1` both do this through their own
runner: a per-step budget, a whole-run budget checked before each step, and `Process.Kill($true)` on
expiry. Copy that shape rather than inventing a new one. A new script without a timeout is a defect,
even if it passes on the day it is written.

**Never poll with a sleep loop in a background shell.** A `while ... sleep` waiter is a second process
that outlives what it is waiting for, and a stack of them is indistinguishable from the work itself.
Start the real command in the background and act on its completion notification. When a status check is
genuinely needed, run one short bounded command that reads the log and exits.

**Budgets that apply here.** A single aggregate generates in roughly 2 to 8 minutes, so a per-step
budget of 20 minutes is generous and a run of the four-aggregate marketplace fits in 120. The E2E
battery takes 20 to 30 minutes with a 150-minute global budget. A step that exceeds these is hung, not
slow: kill it and read the log rather than waiting longer.

**Check for orphans after any interrupted run.** Killing a runner does not always take the Visual
Studio it launched. The generator assigns `devenv.exe` to a Windows job object so the operating system
terminates it with the parent, but a session started by an older build has no such protection:

```powershell
Get-Process devenv | Select-Object Id, StartTime
```

Kill anything older than the run that started it. **Never kill the maintainer's own Visual Studio**,
which is the instance that has been running far longer than any automation.

## Generation facts worth knowing before changing anything

- Templates resolve the model through the Visual Studio code model, so transformation only happens
  inside a `devenv.exe` session. `TextTransform.exe` and `dotnet build` cannot substitute for it, and a
  green build is never evidence that generation ran.
- A template that produces no content, or whose output file contains `ErrorGeneratingOutput`, is a
  failure. Both are detected and retried; do not weaken those checks to make a run pass.
- The reference aggregate under `tests/fixtures/reference-aggregate` is the fast regression gate and
  `tests/fixtures/marketplace` is the realistic one. Two defects that shipped for months were invisible
  to the first and obvious to the second, so add a property to both when fixing a template.
- NuGet caches a package by id and version. Rebuilding `1.1.0` locally does not reach anything that
  already restored `1.1.0`; `sandbox create` discards a stale extracted copy and the harness purges the
  versions under test. Any other consumer of the local feed has to do the same.

## Before saying something works

Run the checks, do not reason about them: `dotnet test` on the platform tests, `pwsh pack-all.ps1`
(which runs `validate tree` and `validate packages`), `pwsh scripts/golden.ps1`, and
`pwsh scripts/e2e.ps1` for the acceptance battery. Report what the output said, including the failures.
