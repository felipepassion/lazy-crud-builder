# Toolchain: the Visual Studio prerequisite

An installed Visual Studio with the Text Template Transformation component is an official, verifiable prerequisite for Lazy CRUD generation in every mode: the IDE, the command-line runner, build-driven transformation, CI and the E2E harness. This page defines the requirement, how the toolchain is found and selected, what is reported, and how the unattended session works.

## The requirement

| Requirement | Value |
| --- | --- |
| Operating system | Windows |
| Visual Studio | 2022 (17.x) or 2026 (18.x) |
| Channels | release, preview or Insiders; all are supported and all are discovered |
| Component | `Microsoft.VisualStudio.Component.TextTemplating`, displayed as "Text Template Transformation" in the Visual Studio Installer |
| Files that must exist in the instance | `Common7\IDE\devenv.exe` and `Common7\IDE\TextTransform.exe` |
| Instance state | complete and launchable |
| .NET SDK | 10.0 |

The requirement is declared in the descriptor under `compatibility.hostRequirements` (`platform`, `visualStudioVersions`, `visualStudioChannels`, `t4Component`, `t4ComponentDisplayName`, `dotnetSdk`, `headlessDefinition`), so a tool can check it before starting work instead of discovering it through a failure.

Why: the templates resolve the entity model through the Visual Studio code model (EnvDTE), and the generator association written into every aggregate project is the Visual Studio custom tool `TextTemplatingFileGenerator`. Transformation therefore always runs the T4 tooling of the resolved installation. There is no alternative engine, and a plain `dotnet build` transforms nothing.

## What headless means

Headless means unattended: no prompt, no window a person has to focus, no clicks, no interactive IDE session anyone drives. It does not mean Visual Studio is absent. Generation always runs through the T4 tooling of the resolved Visual Studio installation inside an embedded `devenv.exe` automation session; a plain build never substitutes for transformation. This definition is published verbatim in the descriptor as `compatibility.hostRequirements.headlessDefinition`.

## Discovery

The runner locates the toolchain deterministically. Mechanisms, in order:

1. **Explicit override.** The `LAZYCRUD_VS_INSTANCE` environment variable or the `--vs-instance <path>` option names an installation path. It is validated (the path must contain `Common7\IDE\devenv.exe`; the T4 tooling is validated by the presence of `Common7\IDE\TextTransform.exe`; a path containing `Preview` or `Insiders` is treated as prerelease), never trusted blindly, and reported as an override (`LC4003`). An invalid override fails resolution; it does not fall through to discovery.
2. **vswhere.** The Visual Studio Installer query tool at its documented fixed location, `%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe`, queried with `-all -prerelease -products *` so that side-by-side, preview and Insiders instances are included, plus a second query with `-requires Microsoft.VisualStudio.Component.TextTemplating` to learn which instances carry the component.
3. **Setup Configuration API.** When vswhere is absent or returns no instance, the COM Setup Configuration API (`ISetupConfiguration2.EnumAllInstances`, the same data vswhere reads) is queried.

No mechanism scans program-files directories recursively and none hardcodes an installation path. On a non-Windows host discovery is not attempted and resolution fails.

An instance satisfies the prerequisite when it is complete, launchable, has `devenv.exe` and `TextTransform.exe`, and has the component installed. Every candidate that does not is listed with its rejection reasons (`installation incomplete`, `not launchable`, `devenv.exe missing`, `Common7/IDE/TextTransform.exe missing`, `component Microsoft.VisualStudio.Component.TextTemplating not installed`).

## Selection rule

Among the satisfying instances the runner selects deterministically:

1. release-channel instances before prerelease instances;
2. within a channel, the highest installation version;
3. ties broken by the ordinal order of the installation path.

The same machine resolves the same instance on every run. A machine whose only instance is a preview or Insiders build resolves it; the design treats this as a valid host, which is why discovery includes prerelease channels.

## Simulating an unavailable toolchain

When `LAZYCRUD_VS_SIMULATE_UNAVAILABLE` is set to a non-empty value, discovery is bypassed and the toolchain is reported as unresolvable. The harness uses it for scenario T2 to prove early failure without uninstalling anything. Unset it on a machine where generation is expected to work.

## What is reported

`toolchain` prints the resolution as JSON and exits 0, or prints `LC4001 prerequisite unavailable`, the discovery log and the remedy and exits 3:

```powershell
dotnet Lazy.Crud.Builder.Platform.Tool.dll toolchain
dotnet Lazy.Crud.Builder.Platform.Tool.dll toolchain --vs-instance "C:\Program Files\Microsoft Visual Studio\18\Insiders"
```

The resolution report (also embedded as `Toolchain` in the `generate` report) contains: `resolved`, `overrideApplied`, `instanceId`, `displayName`, `installationPath`, `installationVersion`, `channel` (`release` or `prerelease`), `channelId`, `productId`, `devenvPath`, `textTransformPath`, `discoverySource` (`vswhere`, `setup-configuration-api` or `override`), `requiredComponent`, `candidates`, `rejections`, `discoveryLog` and `failureReason`. Every `generate` run records `LC4002` (or `LC4003` for an override) with the resolved instance and the T4 tooling path.

## Failure before work begins

When no instance satisfies the prerequisite, `generate` emits `LC4001` (error) before transforming or writing anything and exits 3 (`PrerequisiteUnavailable`). The message names the requirement, the discovery performed, every instance found with the reason it was rejected, the component that must be installed and the remedy: install Visual Studio 2022 (17.x) or 2026 (18.x, release, preview or Insiders channel) with the Text Template Transformation component, or point `LAZYCRUD_VS_INSTANCE` / `--vs-instance` at a valid installation path. The run never falls back to building the project as though that were a transformation, and never reports output it did not produce.

## The unattended session

Transformation runs inside an embedded Visual Studio automation session managed by the runner:

1. The resolved `devenv.exe` is started with `-Embedding` (no window, no prompts). `SuppressUI` is set, `UserControl` is cleared and the main window is hidden.
2. The runner binds to the DTE object of that exact process through the Running Object Table moniker `!VisualStudio.DTE.<major>.0:<pid>`, so side-by-side installations cannot be confused.
3. The solution is opened and the runner waits until the project count is stable.
4. For each wrapper template the runner runs the custom tool (`RunCustomTool` on the project item, the same operation as Run Custom Tool in the IDE, with `TextTemplating.RunCustomTool` as the fallback command), then reads the Error List for T4 engine errors.
5. Each transformation must produce its output: the runner snapshots the wrapper's residue file (`<wrapper>.generated.*`) before the call and polls until it changes, up to `WaitTimeoutMs` (default 5000 ms, `PollIntervalMs` default 150 ms). A transformation that produces nothing fails with `LC4004`, with 2 attempts per template.
6. The solution is closed and Visual Studio is asked to quit; a process that does not exit within 30 seconds is terminated.

Startup, solution opening and each custom tool call have their own timeouts; exceeding them ends the run with exit code 5 (`Timeout`).

## Pipeline invariants

Adopting the declared toolchain preserved the accumulated behavior of the IDE extension's pipeline:

- Domain-first ordering: `Domain`, then `Application`, `Application.DTO`, `Infra.Data`, `Infra.IoC`, `Api.Queries`, `Enumerations`, `Api`, then the test layers.
- The Domain layer's two-phase execution: `DefaultEntities.tt`, verification that `Aggregates/{Aggregate}Agg/Entities` contains entity files, `DefaultCommands.tt`.
- The Domain gate: when either Domain phase fails no other layer is transformed and the failure is reported.
- Non-Domain layers fan out in parallel under the `MaxParallelT4` throttle from `lazy.settings` (default 1).
- Per-aggregate serialization: one lock per aggregate name; operations on different aggregates proceed concurrently.
- `dotnet restore` before transformation (2 attempts) so the `$(Pkg...)` include paths exist, and a post-restore after generation.
- Bounded retry (2 attempts, fixed delay) around each transformation, wait for produced output with polling and a timeout.

These invariants are covered by `PipelineTests` in the unit test project.

## Build-driven transformation

The templates package imports `build/Lazy.Crud.Builder.Templates.targets` into every project that references it. A plain build only prints a low-importance hint that the project contains templates a build does not transform. Opting in runs the runner before compilation:

```powershell
dotnet build -p:LazyCrudTransformOnBuild=true
dotnet build -p:LazyCrudTransformOnBuild=true -p:LazyCrudAggregate=Products -p:LazyCrudResolution=skip-modified
```

`LazyCrudAggregate` defaults to the first segment of the project name (after removing a `Lazy.Crud.` prefix); `LazyCrudResolution` defaults to `protect`. When the toolchain is unavailable the runner fails with exit code 3 and the build fails too; the compile step is never substituted for a transformation.

## Local template development

The runner, the harness and the golden test can transform the templates of this repository directly (`--templates local:<dir>`, or `TemplatesSource` `Local` in `lazy.settings`). The shared includes and the writer assembly then resolve through `LazyCrudTemplatesRoot` and `LazyCrudPlatformAssembly` in `Directory.Build.props`. Local mode is explicit and reported (`LC3002`); it is never reached from a failed package resolution.
