# Clean room

Clean-room validation proves that the published packages work exactly as an external user receives them: no private source, no local template fallback, no non-public feed, no pre-populated package cache. It is scenario C1 of the E2E harness and a release gate.

A clean room is defined by the absence of private sources, not by a bare machine. The host must satisfy the platform's declared prerequisites, including the installed Visual Studio toolchain, because that is what a real external user is required to have. The values below match the [compatibility matrix](compatibility.md).

## What the host must have installed

| Requirement | Value |
| --- | --- |
| Operating system | Windows |
| Visual Studio | 2022 (17.x) or 2026 (18.x), on any channel: release, preview or Insiders |
| Component | `Microsoft.VisualStudio.Component.TextTemplating` (Text Template Transformation) |
| .NET SDK | 10.0 |
| PowerShell | 7 (`pwsh`), to run the harness |
| Network | Access to `https://api.nuget.org/v3/index.json` for the third-party packages the generated projects use (EF Core, Npgsql, Serilog, Swashbuckle, xunit), unless a mirror stands in for it |
| The feed under test | Either the public feed with the Lazy CRUD packages published, or a local folder feed containing the exact pack output (`artifacts/nuget/<version>/`) standing in for it |

The toolchain is resolved through the supported discovery (vswhere or the Setup Configuration API) and the resolved instance is recorded in the run output. Its presence is a satisfied prerequisite, not a fallback.

## What must be absent

| Must be absent or disabled | How the clean room enforces it |
| --- | --- |
| Any copy of the Builder's template sources (`src/ProjectFiles`) reachable from the sandbox | The sandbox contains none; `TemplatesSource` is `NuGet` and `UseNugetTemplates` is `True` |
| Local template fallback | `DisableLocalTemplateFallback` is `True` in the sandbox `lazy.settings`; `--templates local:<dir>` is refused with `LC3001`, and package resolution never degrades to a local tree |
| Private or developer-machine NuGet sources | The sandbox `nuget.config` starts with `<clear />`, then adds only the feed under test and nuget.org |
| Previously restored Lazy CRUD packages | An isolated package cache: `globalPackagesFolder` in the sandbox `nuget.config` (and `NUGET_PACKAGES` for the run) points at a folder created for that run |
| Lazy CRUD packages coming from anywhere but the feed under test | Package source mapping: the pattern `Lazy.Crud.*` maps to the feed under test, `*` maps to nuget.org |
| A Builder source checkout | Nothing in the sandbox references Builder project sources; every template resolves by the registry's `packageId` and `inPackagePath` from restored packages |

## What is inspected

The clean room does not assume the artifact is complete; it asserts it against the descriptor:

- every package the descriptor declares (`layers[].packageId`, `layers[].templatePackageId`, `templates.packageId`, `templates.aliasPackageIds[]`, `nonLayerPackages[]`) is present on the feed under test, and a missing package fails the validation naming it (no local or cached substitute);
- the platform descriptor is readable from a restored runtime package at `lazycrud/lazycrud.platform.json`, declares `schemaVersion`, `contractVersion` and `platformVersion`, and its platform version equals the version under test;
- every registry template is located using only `packageId` and `inPackagePath` (`contentFiles/any/any/LazyCrudTemplates/...`), and the templates package carries the shared templates at `templates.contentRoot`;
- the attribute catalog assembles from the restored descriptor plus the restored XML documentation file (`lib/net10.0/Lazy.Crud.Builder.Application.DTO.xml`) with no platform source file;
- an aggregate generates headlessly from restored artifacts, the produced files carry marker v2 and manifest entries with matching hashes, and the sandbox builds and its generated tests pass.

## The public clean room

Scenario C1 runs against the pack output standing in for the public feed. That proves the package set is self-sufficient, but nothing was downloaded, so it cannot prove that the bytes an external user receives are the ones that work. `scripts/public-clean-room.ps1` closes that gap after a release is pushed: the same sandbox with `--clean-room` and **no** `--feed`, which leaves `https://api.nuget.org/v3/index.json` as the only source in the generated `nuget.config`, with its own empty package cache.

```powershell
pwsh scripts/public-clean-room.ps1
```

It polls the source until every declared package answers for the version (publication is asynchronous, and a push that exited zero is not yet restorable), creates the sandbox, asserts the room is clean, generates through the resolved Visual Studio toolchain, reads the descriptor out of the *downloaded* package and resolves every registry entry inside it, verifies markers, manifest and hashes -- computed with the downloaded `Lazy.Crud.Builder.Platform` assembly, not a local build -- builds and tests the generated solution, and finally hashes the downloaded `.nupkg` files against `contracts/releases/<version>.json` so the run states whether the internet served what this repository built. Exit 0 proves it, exit 5 means the version is not on the source yet, exit 1 means something failed; evidence lands in `artifacts/public-clean-room/<stamp>/`.

## Reproducing it by hand

The sandbox the harness builds can be created with the runner. From a repository checkout after `pwsh pack-all.ps1`:

```powershell
$tool = "src\Platform\Lazy.Crud.Builder.Platform.Tool\bin\Release\net10.0\Lazy.Crud.Builder.Platform.Tool.dll"
$sandbox = "$env:TEMP\lazycrud-clean-room"
dotnet $tool sandbox create --output $sandbox --aggregate Products --fixture tests\fixtures\reference-aggregate --version 1.1.0 --feed artifacts\nuget\1.1.0 --templates nuget --clean-room --packages-folder "$sandbox\packages"
$env:NUGET_PACKAGES = "$sandbox\packages"
dotnet $tool generate --solution $sandbox --aggregate Products --templates nuget --report "$sandbox\generate.json"
dotnet build "$sandbox\Sandbox.sln"
dotnet test "$sandbox\Sandbox.sln"
```

`sandbox create` materializes the layer projects with package references at the requested version (`--override <PackageId>=<version>` pins individual packages for mixed-version scenarios), the wrappers, `src/Mods/lazy.settings`, `Directory.Build.props`, the `nuget.config` described above and `Sandbox.sln`, and writes `sandbox.json` describing what it did. `--templates-package Lazy.Crud.Templates` reproduces the deprecated-alias scenario; `--no-tests` skips the test projects; `--max-parallel <n>` sets `MaxParallelT4`.

An external user reproduces the same conditions by installing the prerequisites above, creating a new solution as in the [Quickstart](quickstart.md), and restoring from the public feed with a `nuget.config` that clears other sources.
