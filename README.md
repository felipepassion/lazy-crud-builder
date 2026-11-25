# Lazy CRUD Builder

Lazy CRUD is a domain-to-architecture compiler for .NET. You write the domain model: plain entity classes decorated with attributes. The platform compiles that model into a complete, layered aggregate: domain command and event handlers, application services, DTOs and validators, EF Core mappings and repositories, dependency injection, query models, controllers, migrations and a test project per layer. The generated code targets `net11.0` and runs on the `Lazy.Crud.Builder.*` and `Lazy.Crud.CrossCutting.*` runtime packages, version 1.2.0.

| Role | What it is in Lazy CRUD |
| --- | --- |
| Input | Entity classes and attributes (the declarative model) in the Domain project of an aggregate |
| Compiler | The T4 templates shipped in the layer packages and in `Lazy.Crud.Builder.Templates` |
| Runtime | The `Lazy.Crud.Builder.*` and `Lazy.Crud.CrossCutting.*` NuGet packages |
| Artifact | The generated code: regenerable at any time, and never the place to make a change |

Generated code is treated as an artifact with a contract. Every generated file starts with a marker header that states who owns it, every aggregate keeps a manifest with the hash of every file the generator wrote, and the generator refuses to overwrite a file that was edited outside of it unless you explicitly resolve the conflict. See [Generated code policy](docs/generated-code-policy.md).

## Prerequisites

Read this before you generate anything. Generation requires the following in every mode, including the unattended command-line runner and CI:

- Windows.
- An installed **Visual Studio 2022 (17.x) or Visual Studio 2026 (18.x)** on any channel (release, preview or Insiders) with the **Text Template Transformation** component, id `Microsoft.VisualStudio.Component.TextTemplating`. The templates resolve your entity model through the Visual Studio code model (DTE), so every transformation runs the T4 tooling of the resolved installation inside an embedded, unattended Visual Studio automation session.
- The **.NET SDK 11.0**.

"Headless" means unattended: no prompt, no window a person has to focus, no clicks. It does not mean Visual Studio is absent. A plain `dotnet build` never substitutes for transformation. When the toolchain cannot be resolved, the runner fails before transforming anything with diagnostic `LC4001` and exit code 3 (`PrerequisiteUnavailable`). Discovery rules, the selection rule and the override variable are in [Toolchain](docs/toolchain.md).

Check a machine before starting (the runner ships inside `Lazy.Crud.Builder.Templates` under `tools/net11.0/`):

```powershell
dotnet "$env:USERPROFILE\.nuget\packages\lazy.crud.builder.templates\1.2.0\tools\net11.0\Lazy.Crud.Builder.Platform.Tool.dll" toolchain
```

## Quickstart

The [Quickstart](docs/quickstart.md) takes an empty solution to a generated, compiling aggregate using only the published packages. In short:

1. Create the layer projects of an aggregate and reference the layer packages with `GeneratePathProperty="true"`.
2. Write an entity class with attributes under `Aggregates/<Aggregate>Agg/Entities/` in the Domain project.
3. Add `src/Mods/lazy.settings` and the wrapper templates (`DefaultEntities.tt`, `DefaultCommands.tt`).
4. Run the generation runner, then build and test:

```powershell
dotnet "$env:USERPROFILE\.nuget\packages\lazy.crud.builder.templates\1.2.0\tools\net11.0\Lazy.Crud.Builder.Platform.Tool.dll" generate --solution . --aggregate Products --report artifacts\generate.json
dotnet build
dotnet test
```

After a restore the runner path is also available to MSBuild as `$(LazyCrudToolDll)`, and `dotnet build -p:LazyCrudTransformOnBuild=true` invokes it for you. The E2E harness executes this exact path as scenario P7.

## What gets generated

One aggregate produces eight layer projects, transformed Domain first, plus one generated test project per layer:

| Layer id | Project | Runtime package |
| --- | --- | --- |
| `Domain` | `{Aggregate}.Domain` | `Lazy.Crud.Builder.Domain` |
| `Application` | `{Aggregate}.Application` | `Lazy.Crud.Builder.Application` |
| `Application.DTO` | `{Aggregate}.Application.DTO` | `Lazy.Crud.Builder.Application.DTO` |
| `Infra.Data` | `{Aggregate}.Infra.Data` | `Lazy.Crud.Builder.Infra.Data` |
| `Infra.IoC` | `{Aggregate}.Infra.IoC` | `Lazy.Crud.Builder.Infra.IoC` |
| `Api.Queries` | `{Aggregate}.Api.Queries` | `Lazy.Crud.Builder.Api.Queries` |
| `Enumerations` | `{Aggregate}.Enumerations` | `Lazy.Crud.Builder.Enumeration` |
| `Api` | `{Aggregate}.Api` | `Lazy.Crud.Builder.Api` |

[Architecture](docs/architecture.md) explains the model, compiler, runtime and artifact roles, what every layer emits, the pipeline order and its invariants.

## Packages

All packages share one version, 1.2.0, defined once as `LazyCrudVersion` in `Directory.Build.props`, and target `net11.0`. Every package embeds the platform descriptor at `lazycrud/lazycrud.platform.json`.

| Package | Role |
| --- | --- |
| `Lazy.Crud.Builder.Domain`, `Lazy.Crud.Builder.Application`, `Lazy.Crud.Builder.Application.DTO`, `Lazy.Crud.Builder.Infra.Data`, `Lazy.Crud.Builder.Infra.IoC`, `Lazy.Crud.Builder.Api`, `Lazy.Crud.Builder.Api.Queries`, `Lazy.Crud.Builder.Enumeration` | Layer runtime packages. Each carries its layer's templates under `contentFiles/any/any/LazyCrudTemplates/<Layer>/` and depends on the templates package. |
| `Lazy.Crud.Builder.Application.DTO.Http.Models`, `Lazy.Crud.Builder.Application.Validators`, `Lazy.Crud.Builder.WebApi` | Non-layer runtime packages. |
| `Lazy.Crud.CrossCutting.Domain`, `Lazy.Crud.CrossCutting.Infra.Log`, `Lazy.Crud.CrossCutting.Infra.Utils`, `Lazy.Crud.CrossCutting.Application.Mail` | Cross-cutting runtime packages. |
| `Lazy.Crud.Builder.Platform` | The contracts library: ownership, marker v2, the frozen content hash, manifest v2, diagnostics and the shared generation writer. |
| `Lazy.Crud.Builder.Templates` | The canonical templates package: `project.tt`, `Tests/Tests.Common.ttinclude`, the runner under `tools/net11.0/`, the MSBuild props and targets, the contract fixtures and schemas under `contracts/`, and the descriptor. |
| `Lazy.Crud.Templates` | Deprecated dependency-only alias of `Lazy.Crud.Builder.Templates`. It carries no content and is scheduled for removal two minor releases after 1.1.1. |

## Documentation

Start here:

- [Quickstart](docs/quickstart.md): empty project to generated, compiling aggregate.
- [Architecture](docs/architecture.md): model, compiler, runtime, artifact; layers; pipeline invariants.
- [Aggregates, entities and attributes](docs/model.md): the declarative model reference.
- [Generated code policy](docs/generated-code-policy.md): ownership, markers, what is safe to edit.
- [Safe customization](docs/customization.md): change generated behavior without losing it on regeneration.
- [Migrations](docs/migrations.md): moving between platform versions, including 1.0.x to 1.1.1 and 1.1.1 to 1.2.0.
- [Testing](docs/testing.md): generated test projects, unit tests, the golden test and the harness.
- [Compatibility matrix](docs/compatibility.md): versions, consumers, mixed pins and host prerequisites.
- [Troubleshooting](docs/troubleshooting.md): diagnostics by code and how to read `.lazycrud/diagnostics`.
- [Release guidance](docs/release.md) and the [Changelog](CHANGELOG.md).
- [Using Lazy CRUD with AI coding agents](docs/ai-agents.md).

Platform and operations:

- [Toolchain](docs/toolchain.md): the Visual Studio prerequisite, discovery, selection and the unattended session.
- [E2E harness](docs/harness.md): running the battery and reading its artifacts.
- [Clean room](docs/clean-room.md): validating the public distribution as an external user receives it.

Frozen contract reference (the fixtures win when prose and fixture disagree):

- [Platform descriptor](docs/reference/descriptor.md)
- [Manifest v2](docs/reference/manifest-v2.md)
- [Marker v2](docs/reference/marker-v2.md)
- [Ownership](docs/reference/ownership.md)
- [Content hash](docs/reference/hash.md)
- [Diagnostics](docs/reference/diagnostics.md)

Documents that predate the 1.1.1 relaunch are kept under [docs/legacy](docs/legacy/) for history only.

## Contracts and fixtures

The shared contracts consumed by the Visual Studio extension, the CLI and AI agents are frozen under `contractVersion` 1 and published as implementation-generated fixtures:

- In the repository: `contracts/fixtures/v1/` (descriptor, `manifest-v2.json`, `manifest-v1-input.json` and `manifest-v1-migrated.json`, `markers/*.txt`, `hash-vectors.json`, `fixture-index.json`) and the JSON Schemas under `contracts/schemas/v1/`.
- Inside the `Lazy.Crud.Builder.Templates` package under `contracts/`.
- From GitHub as raw files, for example `https://raw.githubusercontent.com/felipepassion/lazy-crud-builder/main/contracts/fixtures/v1/fixture-index.json`, and as the CI artifact `lazycrud-contract-fixtures`.

## Repository layout

| Path | Content |
| --- | --- |
| `src/Builder`, `src/CrossCutting` | The runtime packages |
| `src/ProjectFiles` | The T4 templates (the compiler): one folder per layer, `project.tt` and `Tests/` |
| `src/Platform` | `Lazy.Crud.Builder.Platform` (contracts library), the `Lazy.Crud.Builder.Platform.Tool` runner and the unit tests |
| `src/Templates` | The `Lazy.Crud.Builder.Templates` package project and the deprecated `Lazy.Crud.Templates` alias |
| `contracts/` | Fixtures and JSON Schemas per `contractVersion` |
| `tests/fixtures/reference-aggregate`, `tests/golden` | The reference aggregate and its golden output |
| `scripts/` | `e2e.ps1` (the harness) and `credential-scan.ps1` |
| `pack-all.ps1`, `publish-all.ps1` | Pack every package into `artifacts/nuget/<version>/` and publish with `NUGET_API_KEY` |

## License

See [LICENSE](LICENSE).
