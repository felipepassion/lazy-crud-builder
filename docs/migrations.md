# Migrations between platform versions

The platform has one version for every package (`LazyCrudVersion`), one contract version for the shared artifacts (`contractVersion`, currently 1) and one manifest schema version (currently 2). This page tells you what changes when you move between platform versions and what to expect the first time you regenerate after an upgrade.

## General rules

- Upgrade every `Lazy.Crud.Builder.*` and `Lazy.Crud.CrossCutting.*` reference in a solution together. The only supported mixed pins are listed in the [compatibility matrix](compatibility.md).
- The templates package (`Lazy.Crud.Builder.Templates`) must equal the version of every layer package that ships templates. Restore the solution after changing versions so the wrapper includes resolve against the new package paths.
- Manifests are never rejected: an older manifest is read, migrated in memory and rewritten in the current schema after the next successful generation.
- A contract-breaking change increments `contractVersion`; consumers written against an older contract see `LC5001` (`ContractMismatch`, exit 6) instead of misreading a document. Additive fields never increment it.

## 1.0.x to 1.1.1

### 1. Align the package versions

Set every Lazy CRUD package reference to 1.1.1. Three packages may stay at any version from 1.0.29 to 1.1.1 next to runtime packages at 1.1.1: `Lazy.Crud.Builder.Application.DTO.Http.Models`, `Lazy.Crud.Builder.Application.Validators` and `Lazy.Crud.CrossCutting.Domain`. `Lazy.Crud.Builder.Templates` must be 1.1.1 whenever any layer package is 1.1.1; a pin such as the historical `Lazy.Crud.Builder.Templates` 1.0.1 next to 1.1.1 layer packages is not supported.

Every 1.1.1 package embeds a platform descriptor. When packages of different versions are installed and their descriptors disagree, the highest `platformVersion` wins and `LC5002` names every conflicting version; aligning the versions removes the warning.

### 2. Templates package identity

The canonical templates package is `Lazy.Crud.Builder.Templates`. Solutions whose `lazy.settings` names the historical id `Lazy.Crud.Templates` keep resolving: 1.1.1 publishes that id as a deprecated dependency-only alias, and the runner reports `LC3003` while resolving it to the canonical package. Update `TemplatesNuGetPackageId` to `Lazy.Crud.Builder.Templates`; the alias is removed two minor releases after 1.1.1.

If you consumed templates from packages before 1.1.1, note that the shared files (`project.tt`, `Tests/Tests.Common.ttinclude`) now ship only in the templates package. Layer packages no longer carry them, which removes the content-file collision a solution referencing several layer packages used to receive. The undocumented `contentFiles/any/any/T4/` glob was removed; no shipped package ever contained that folder.

### 3. lazy.settings

- For package-based generation set `TemplatesSource` to `NuGet` and `UseNugetTemplates` to `True`; the default (`Local`) is the development mode of the templates themselves.
- New keys with safe defaults: `GeneratedCodeMarkers` (`True`) and `DisableLocalTemplateFallback` (`False`).
- Existing keys keep their meaning: `GenerationOutputPath` (`LazyCode`), `MaxParallelT4`, `WaitTimeoutMs`, `PollIntervalMs`, `GenerateTests`, `TestsPathPattern`, the layer pattern keys.

### 4. The first regeneration rewrites every header

1.1.1 emits the marker v2 header on every `GeneratorOwned` and `GeneratedOnce` file. The first regeneration after upgrading therefore rewrites every generated file: the header is new, the body is unchanged. Expect a large but header-only diff in your repository; the golden-output test in this repository asserts that only the header region differs for the reference aggregate. Commit that diff as the upgrade commit.

The header is excluded from the content hash, so this rewrite does not register as drift on later runs.

### 5. Manifest v1 is migrated

A manifest without `schemaVersion` (1.0.x) is loaded as v1 and migrated in memory (`LC1009`), then rewritten as v2 at `.lazycrud/manifests/{Aggregate}.manifest.json` after the next successful generation. Migration never invents data:

- An entry whose absolute path falls under the current solution root becomes a forward-slash relative path.
- An entry whose absolute path cannot be expressed under the current root (typical for manifests committed from another machine) is retained as `unresolved` with its `fileName` and the SHA-256 of the original path, and `LC1006` names it. Regenerate to re-record the file, or delete the stale entry.
- The v1 raw-file hash is carried over as `legacyHash` but is never a baseline under the frozen rule; `contentHash` is recorded as unknown (`LC1007` when v1 had none).
- Template identity and ownership come only from an unambiguous registry match on the output file name; otherwise `templateId` is `legacy:<TemplateName>` or null and ownership is `Unknown` (`LC1008`), which is treated as user-owned until a registered template re-records the file (`LC2003`).
- The v1 `FullPath` field and every other absolute path disappear from the rewritten document.

To migrate a manifest in place ahead of the first regeneration:

```powershell
dotnet Lazy.Crud.Builder.Platform.Tool.dll manifest migrate --solution . --aggregate Products
```

If manifests exist under both the solution root and `src/` (the historical second root), `LC1005` names both and the solution root wins; delete the stale copy.

### 6. Unverified files on the first run

After migration no generated file has a trustworthy baseline hash, so each `GeneratorOwned` file is `unverified` on the first regeneration:

- If the regenerated body is identical to the file on disk, the baseline is adopted silently (`LC2008`) and only the header is written. This is the common case for an untouched legacy solution, so the first run needs no flag.
- If the body differs, the file is protected with `LC2002` (not `LC2001`: nothing proves the file was edited, it may equally be a template improvement) and the run exits 7. Inspect the diff, then rerun with `--resolution force` to take the regenerated content or `--resolution skip-modified` to keep the file and continue.

Legacy files that carried only the historical test-template header are recognized as generated (`LC2011`) but are handled exactly like any other unverified file.

### 7. Local template forks

A solution that generates from its own copy of the templates project (local template mode with `TemplatesLocalProjectFilesPath`, as Tio-Sam does) keeps working unchanged: local mode is still supported and reported (`LC3002`). To take the 1.1.1 behavior (markers, manifest v2, drift protection, diagnostics), rebase the fork on the 1.1.1 templates, reference the 1.1.1 packages and point `LazyCrudTemplatesRoot` (and `LazyCrudPlatformAssembly` when the fork lives outside the package) at the fork in `Directory.Build.props`. See [Safe customization](customization.md).

### 8. Tooling

Generation from the command line now goes through the unattended runner shipped in `Lazy.Crud.Builder.Templates` (`tools/net10.0/Lazy.Crud.Builder.Platform.Tool.dll`) or through `dotnet build -p:LazyCrudTransformOnBuild=true`. Both require the Visual Studio T4 toolchain on the machine and fail early with `LC4001` (exit 3) when it is missing; a plain `dotnet build` transforms nothing and is not a substitute. See [Toolchain](toolchain.md).

## Checklist

1. Update package references to 1.1.1 (mixed pins only where the matrix allows).
2. Update `TemplatesNuGetPackageId`, `TemplatesSource` and `UseNugetTemplates` in `lazy.settings`.
3. `dotnet restore`.
4. Run `toolchain` to confirm the machine, then `generate` per aggregate with the default resolution.
5. Resolve any `LC2002` explicitly; review `.lazycrud/diagnostics/{Aggregate}.diagnostics.json`.
6. Commit the header-only diff and the v2 manifests.
