# Compatibility matrix

Every row on this page is either verified by an automated check (the harness, the unit tests or `validate packages`) or marked as unverified. The machine-readable source of the platform and host rows is `compatibility` in the platform descriptor (`lazycrud/lazycrud.platform.json` in every package).

## Platform

| Platform version | contractVersion | Descriptor schemaVersion | Manifest schemaVersion written | Manifest schema versions read | Target framework | .NET SDK |
| --- | --- | --- | --- | --- | --- | --- |
| 1.2.0 | contractVersion 1 (supported range 1 to 1, minimum upgradable 1) | 1 | 2 | 1 (migrated in memory, rewritten as 2) and 2 | `net11.0` | 11.0 |
| 1.1.1 | contractVersion 1 (supported range 1 to 1, minimum upgradable 1) | 1 | 2 | 1 (migrated in memory, rewritten as 2) and 2 | `net10.0` | 10.0 |

## Packages at 1.2.0

All packages share the single version source `LazyCrudVersion` and are packed together by `pack-all.ps1`.

| Package | Kind | Layer |
| --- | --- | --- |
| `Lazy.Crud.Builder.Domain` | layer runtime package, ships `Domain` and `Tests/Domain` templates | `Domain` |
| `Lazy.Crud.Builder.Application` | layer runtime package, ships `Application` and `Tests/Application` templates | `Application` |
| `Lazy.Crud.Builder.Application.DTO` | layer runtime package, ships `Application.DTO` and `Tests/Application.DTO` templates and the attribute catalog | `Application.DTO` |
| `Lazy.Crud.Builder.Infra.Data` | layer runtime package, ships `Infra.Data` and `Tests/Infra.Data` templates | `Infra.Data` |
| `Lazy.Crud.Builder.Infra.IoC` | layer runtime package, ships `Infra.IoC` and `Tests/Infra.IoC` templates | `Infra.IoC` |
| `Lazy.Crud.Builder.Api.Queries` | layer runtime package, ships `Api.Queries` and `Tests/Api.Queries` templates | `Api.Queries` |
| `Lazy.Crud.Builder.Enumeration` | layer runtime package, ships `Tests/Enumerations` templates only | `Enumerations` |
| `Lazy.Crud.Builder.Api` | layer runtime package, ships `Api` and `Tests/Api` templates | `Api` |
| `Lazy.Crud.Builder.Application.DTO.Http.Models` | non-layer runtime package | |
| `Lazy.Crud.Builder.Application.Validators` | non-layer runtime package | |
| `Lazy.Crud.Builder.WebApi` | non-layer runtime package, ships the `WebApi` templates | |
| `Lazy.Crud.CrossCutting.Domain` | cross-cutting runtime package | |
| `Lazy.Crud.CrossCutting.Infra.Log` | cross-cutting runtime package | |
| `Lazy.Crud.CrossCutting.Infra.Utils` | cross-cutting runtime package | |
| `Lazy.Crud.CrossCutting.Application.Mail` | cross-cutting runtime package | |
| `Lazy.Crud.Builder.Platform` | contracts library (also shipped under `tools/net11.0/` of the templates package) | |
| `Lazy.Crud.Builder.Templates` | canonical templates package: `project.tt`, `Tests/Tests.Common.ttinclude`, runner, props and targets, contracts, descriptor | |
| `Lazy.Crud.Templates` | deprecated dependency-only alias of `Lazy.Crud.Builder.Templates`; removal two minor releases after 1.1.1 | |

## Mixed package versions

| Combination | Supported | Verified by |
| --- | --- | --- |
| `Lazy.Crud.Builder.Api` one patch version behind every other package, the templates package included (for example 1.2.0 alongside 1.2.1) | Yes. Nothing depends on it, so NuGet restores the lower pin. Its templates run from its own package with the shared templates and the writer of the newer templates package; its older embedded descriptor is reported as `LC5002`, and the highest `platformVersion` stays canonical. | Harness scenario R3 (restore, generation and build with `Lazy.Crud.Builder.Api` at the current version and every other package at the next patch version); `MixedVersionMatrixTests` |
| `Lazy.Crud.Builder.Templates` at a version different from any other package that ships templates | No. `Lazy.Crud.Builder.Api` is the only package that may lag the templates package, and only as above. `Lazy.Crud.Builder.Domain`, `Lazy.Crud.Builder.Application`, `Lazy.Crud.Builder.Application.DTO`, `Lazy.Crud.Builder.Api.Queries` and `Lazy.Crud.Builder.Enumeration` are dependencies of other packages, and every package that depends on them requires at least its own version of them, so a lower pin is a downgrade NuGet rejects (`NU1605`). `Lazy.Crud.Builder.Infra.Data`, `Lazy.Crud.Builder.Infra.IoC` and `Lazy.Crud.Builder.WebApi` have no dependents, but nothing verifies their older templates against a newer templates package. | `validate packages`; descriptor `mixedVersionRanges`; `MixedVersionMatrixTests` |
| Any other package below the rest, including `Lazy.Crud.Builder.Application.DTO.Http.Models`, `Lazy.Crud.Builder.Application.Validators` and `Lazy.Crud.CrossCutting.Domain` at 1.0.x alongside 1.2.0 (the Tio-Sam pins) | No. The platform's dependencies are minimum versions: `Lazy.Crud.Builder.Application` requires `Lazy.Crud.Builder.Application.Validators`, `Lazy.Crud.Builder.Application.DTO` requires `Lazy.Crud.Builder.Application.DTO.Http.Models` and `Lazy.Crud.Builder.Application.Validators`, and `Lazy.Crud.Builder.Application.DTO.Http.Models` requires `Lazy.Crud.CrossCutting.Domain`, each at least at its own version, so a lower pin is a downgrade NuGet rejects (`NU1605`). | The dependencies the published 1.2.0 packages declare |
| Any other layer packages at different versions from each other | No | Descriptor conflict diagnostic `LC5002` names every disagreeing version; the highest `platformVersion` is canonical |

## Consumers

| Consumer | Status | Shape | Notes |
| --- | --- | --- | --- |
| CopilotSpeaker | Verified | Same aggregate shape as the reference aggregate | Covered by the positive scenarios P1 to P7. |
| Tio-Sam | Verified | Mixed pins: 1.0.32 for most packages, 1.0.29 for `Lazy.Crud.Builder.Application.DTO.Http.Models`, `Lazy.Crud.Builder.Application.Validators` and `Lazy.Crud.CrossCutting.Domain`; declares `Lazy.Crud.Builder.Templates` 1.0.1; generates in local template mode from its own fork of the templates project | Its legacy setup keeps working unchanged (local mode is supported and reported). Upgrading to 1.2.0 requires aligning the templates package version with the layer packages and rebasing the fork, and moving the three packages it pins at 1.0.29 to 1.2.0 as well, since the 1.2.0 packages that depend on them require 1.2.0; see [Migrations](migrations.md). |
| Nutri.Veda | Unverified | Its dependency shape could not be confirmed from the local checkout (no Lazy CRUD package reference was found there) | Listed as not covered by this matrix until the owner confirms the actual coupling. Nothing on this page should be read as a compatibility claim for Nutri.Veda. |
| Lazy CRUD extension (VSIX and CLI) | Adopts contractVersion 1 | Consumes the descriptor, manifest v2, marker v2, hash, diagnostics and exit-code contracts | Tests against the fixtures under `contracts/fixtures/v1/` and `contracts/schemas/v1/`; the fixtures win when prose and fixture disagree. |

## Host prerequisites for generation

Generation in every mode, including headless and CLI, requires the host below. "Headless" means unattended (no prompt, no window to focus, no person driving the IDE), not Visual Studio-free.

| Requirement | Value | Verified by |
| --- | --- | --- |
| Operating system | Windows | Harness scenario T1 (toolchain resolved and recorded) |
| Visual Studio | 2022 (17.x) or 2026 (18.x) | T1; unit tests of the selection rule |
| Channels | release, preview and Insiders are all supported for generation; prerelease instances are discovered and used when they are the only satisfying instance | Discovery queries `vswhere` with `-all -prerelease -products *` and falls back to the Setup Configuration API |
| Component | `Microsoft.VisualStudio.Component.TextTemplating` (Text Template Transformation) | T1 (component checked per instance); T2 proves early failure with exit code 3 when unresolvable |
| .NET SDK | 11.0 | Every scenario |
| Discovery | Visual Studio Installer query tool (`vswhere`) or the Setup Configuration API; override with `LAZYCRUD_VS_INSTANCE` | T1, T2; `toolchain` command |

## Contract negotiation

| Field (descriptor `compatibility`) | Value at 1.2.0 | Meaning |
| --- | --- | --- |
| `supportedContractVersionMin` | 1 | Lowest contract a consumer of this build may present |
| `supportedContractVersionMax` | 1 | Highest contract this build understands; a descriptor above it fails with `LC5001` (exit 6, "update the tool") |
| `minimumUpgradableContractVersion` | 1 | Lowest contract this build still upgrades from; below it `LC5001` says "upgrade the platform packages" |
| `minimumManifestSchemaVersion` | 1 | Lowest manifest schema read (v1 is migrated) |
| `manifestSchemaVersion` | 2 | Manifest schema written |

A `contractVersion` increment happens only for a breaking contract change (a field removed or a meaning changed); additive optional fields such as `compatibility.hostRequirements` do not increment it. See [Platform descriptor](reference/descriptor.md) and [Release guidance](release.md).

## How the matrix is verified

| Row | Check |
| --- | --- |
| Platform, packages, templates identity | `validate tree` and `validate packages` in `pack-all.ps1`; `OwnershipAndRegistryTests` and `SupportTests` |
| Mixed pins | Harness R3 (`Lazy.Crud.Builder.Api` one patch version behind); `MixedVersionMatrixTests` (the matrix, the descriptor and R3 name the same package, R3's sandbox references it, and nothing depends on it) |
| v1 manifests | Harness R1 (accepted and upgraded), F4 and F5 (unverified handling) |
| Deprecated alias | Harness R2 (`Lazy.Crud.Templates` resolves with `LC3003`) |
| Byte-identical output apart from headers | Harness R4 and the golden-output test |
| Multi-layer references without content collision | Harness R5 |
| Host prerequisites | Harness T1 and T2; clean room C1 records the resolved instance |
| Public distribution | Clean room C1 |
