# E2E harness

The harness is the single command that proves the whole chain: sandbox creation from the reference aggregate, real template transformation through the Visual Studio T4 toolchain, markers, manifests, hashes, build, generated tests, regeneration stability, the quickstart path, compatibility regressions, the intentional-failure scenarios, the toolchain scenarios and the clean room. It runs unattended and leaves machine-readable evidence.

## Prerequisites

- Windows with Visual Studio 2022 (17.x) or 2026 (18.x) on any channel (release, preview or Insiders) and the `Microsoft.VisualStudio.Component.TextTemplating` component. The harness performs real transformations; it never skips generation or substitutes a plain build. When the toolchain is unavailable it fails explicitly with exit code 3.
- The .NET SDK 10.0.
- PowerShell 7 (`pwsh`).
- The packages under test: the harness consumes only public-equivalent artifacts, that is the local pack output under `artifacts/nuget/<version>/` standing in for the public feed. Produce it with `pwsh pack-all.ps1` first.

## Running

The whole battery, no arguments:

```powershell
pwsh scripts/e2e.ps1
```

One scenario by id:

```powershell
pwsh scripts/e2e.ps1 -Scenario F1
```

Add `-SandboxRoot <directory>` to place the per-run sandbox under a directory of your choice instead of `%LOCALAPPDATA%/lazycrud/e2e`. Pick a short path outside AppData when the harness runs from a process with MSIX package identity (for example a packaged desktop application), because AppData is virtualized for such processes and Visual Studio does not share that view.

The same reporting and exit-code rules apply to a single scenario. The harness never prompts, never requires Visual Studio to be open or focused, never modifies the repository working tree apart from `artifacts/`, and builds a fresh temporary sandbox per run so two consecutive runs produce the same outcomes.

## Reading the result

The final line of the output is the verdict, greppable even from truncated logs:

```text
RESULT: OK (passed=N failed=0)
```

or

```text
RESULT: FAIL (...)
```

Artifacts are written under `artifacts/e2e/<yyyyMMdd-HHmmss>/`; previous runs are never overwritten:

| Artifact | Content |
| --- | --- |
| `summary.json` | The source of truth: each scenario's id, status, duration and failure reason; the platform version and package versions exercised; the resolved Visual Studio instance, its version and channel and the T4 tooling path used |
| `digest.txt` | The human-readable digest of the same run |
| per-scenario logs | The console output, the generation reports, the generated output and the diagnostics of each scenario, retained on failure |

An agent or a CI job confirms a run by reading `summary.json` and the verdict line; nothing needs to be re-run.

## Exit codes

The harness uses the shared table; a code means the same thing in the runner and in the extension repository's CLI.

| Code | Name | When |
| --- | --- | --- |
| 0 | `Success` | Every required scenario passed (including the scenarios that are expected to fail as designed) |
| 1 | `AssertionFailure` | A scenario assertion did not hold |
| 2 | `UsageError` | Invalid arguments (for example an unknown scenario id) |
| 3 | `PrerequisiteUnavailable` | The Visual Studio T4 toolchain or the .NET SDK is unavailable on the host |
| 4 | `InvalidFixtureOrSandbox` | A fixture could not be loaded or the sandbox could not be created |
| 5 | `Timeout` | A scenario or the run exceeded its budget |
| 6 | `ContractMismatch` | A resolved descriptor's `contractVersion` is outside the supported range |
| 7 | `OwnershipConflict` | A run failed because files are drifted or unverified where that was not the expected outcome |
| 8 | `HarnessFailure` | The harness itself failed, as opposed to a scenario failing |
| 9 | `GenerationDefect` | Generation produced an output that carries no content where the existing file has some (`LC2012`); the write was refused and nothing was lost, but the run produced a defective artifact |

## Scenarios

Positive:

| Id | Proves |
| --- | --- |
| P1 | The sample aggregate is created in the sandbox and generation produces files for every layer the descriptor declares |
| P2 | Every generated file carries a valid marker v2; `GeneratedOnce` files carry the distinct user-owned policy |
| P3 | The manifest is schema version 2, contains no absolute path, and records ownership and a non-empty content hash for every entry |
| P4 | Recomputing the frozen hash of every generated file matches the manifest |
| P5 | The sandbox solution builds and the generated tests pass |
| P6 | A second generation without model changes succeeds, reports no drift and leaves every file unchanged |
| P7 | The quickstart path (empty project, published packages only, entity, `lazy.settings`, wrappers, runner, build) succeeds |

Compatibility regression:

| Id | Proves |
| --- | --- |
| R1 | A v1 manifest (the published fixture with absolute paths and empty hashes) is accepted, upgraded to v2 matching the published expected output, with no drift and no unverified failure when the files are unmodified |
| R2 | The deprecated templates package id `Lazy.Crud.Templates` resolves, with a deprecation notice (`LC3003`) |
| R3 | A supported mixed-version combination (the Tio-Sam pins) restores, builds and generates |
| R4 | The reference aggregate's output is byte-identical to the golden output apart from the marker headers |
| R5 | A project referencing several layer packages restores and builds with no duplicate template content |

Intentional failure (recorded as passes when they fail as designed):

| Id | Proves |
| --- | --- |
| F1 | An external edit to a `GeneratorOwned` file is detected: the run fails with `LC2001` naming that file, exit code 7, and the edit survives on disk |
| F2 | `--resolution force` regenerates the drifted file and reports the overwrite |
| F3 | `--resolution skip-modified` preserves the modified file, generates the rest and succeeds listing the skip |
| F4 | An unverified file whose content differs is protected with `LC2002`, not `LC2001`, and is unchanged on disk |
| F5 | An unverified file whose content is identical is adopted without intervention (`LC2008`) and the manifest records a real hash |
| F6 | A template edited to resolve no model emits a body of imports and nothing else on every attempt; the shipped writer refuses the overwrite with `LC2012`, the retry does not rescue it, the run exits 9, `--force` does not get around it, the generated file is byte-identical afterwards, and the declared contentless output (global usings) still regenerates |

Toolchain:

| Id | Proves |
| --- | --- |
| T1 | The Visual Studio T4 toolchain resolves through the supported discovery, and the instance, version, channel and tooling path are recorded in the summary |
| T2 | With the toolchain made unresolvable (`LAZYCRUD_VS_SIMULATE_UNAVAILABLE`), the run fails before transforming anything with exit code 3, the diagnostic names the requirement, what was searched and what must be installed, and no output is produced; recorded as a pass |

Clean room:

| Id | Proves |
| --- | --- |
| C1 | In an isolated `NUGET_PACKAGES` folder, with a `nuget.config` that clears every inherited source and adds only the local feed standing in for the public feed plus nuget.org with package source mapping, local template mode is refused, every package the descriptor declares is present, the descriptor is readable from a restored package, every registry template resolves by `packageId` and `inPackagePath`, the attribute catalog assembles from the descriptor and the XML documentation, and headless generation works from restored artifacts only |

## How the harness stays honest

- It computes hashes, reads ownership, parses markers and validates documents with the frozen contracts and the published fixtures under `contracts/` (hash vectors, JSON Schemas, the v1 migration pair); it defines no expected values of its own.
- A generation scenario passes only when the transformed outputs exist with marker v2, appear in the manifest with matching hashes, and the solution builds and its generated tests pass. A successful build alone never satisfies a scenario, and a transformation that produces nothing fails.
- The intentional-failure scenarios assert the specific diagnostic code and the affected path, so a crash for an unrelated reason cannot pass as "protection works".

See [Testing](testing.md), [Toolchain](toolchain.md) and [Clean room](clean-room.md).
