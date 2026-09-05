# Architecture

Lazy CRUD is a compiler whose source language is a domain model. Keeping the four roles apart is what makes generated code safe to regenerate.

| Role | Artifact | Where it lives |
| --- | --- | --- |
| Input | Entity classes and attributes | `src/{Aggregate}/{Aggregate}.Domain/Aggregates/{Aggregate}Agg/Entities/*.cs` in the consumer solution |
| Compiler | T4 templates | `src/ProjectFiles/**/*.tt` in this repository; shipped as `contentFiles/any/any/LazyCrudTemplates/<Layer>/` in each layer package, shared files in `Lazy.Crud.Builder.Templates` |
| Runtime | `Lazy.Crud.Builder.*` and `Lazy.Crud.CrossCutting.*` packages, version 1.1.0, target `net10.0` | NuGet |
| Artifact | Generated code, the manifest and the diagnostics file | `{OutputPath}` (default `LazyCode/`) in each layer project, `.lazycrud/` under the solution root |

The templates and the tools never read a hand-written table of layers, package ids or template ids. The platform descriptor `lazycrud.platform.json`, generated from the build and embedded in every package, is the single vocabulary every surface reads. See [Platform descriptor](reference/descriptor.md).

## The model is the input

An aggregate is a folder `src/{Aggregate}/` holding one project per layer. The domain model is the set of entity classes in the Domain project under `Aggregates/{Aggregate}Agg/Entities/`, deriving from the runtime `Entity` base type and decorated with the attributes shipped in `Lazy.Crud.Builder.Application.DTO`. Only entities that carry `EndpointsT4` take part in endpoint and command generation. The attribute catalog is a published contract: identity and structure in the descriptor's `attributes[]`, prose in the package's XML documentation file, joined by `documentationId`. See [Aggregates, entities and attributes](model.md).

## The templates are the compiler

Every layer project carries one or more wrapper templates (`DefaultEntities.tt`, `DefaultCommands.tt`, `ServerIoCFactory.tt`, `Migrations.tt`, `Controllers/Controllers.tt`, `<Layer>Test.tt`) that include the shipped templates through the MSBuild package path property, for example `$(PkgLazy_Crud_Builder_Domain)\contentFiles\any\any\LazyCrudTemplates\Domain\DefaultEntities.tt`. The shipped templates include the shared `project.tt`, which resolves the entity model through the Visual Studio code model (DTE) and writes files through the shared writer in `Lazy.Crud.Builder.Platform`. This is why generation requires an installed Visual Studio with the Text Template Transformation component in every mode, and why "headless" means unattended rather than Visual Studio-free. See [Toolchain](toolchain.md).

The template registry in the descriptor lists 107 templates, each with a stable `id`, its `layer`, `sourceFile`, `outputPattern`, `ownership`, `templateVersion`, the carrying `packageId`, `inPackagePath` and `kind` (`writer`, `entry` or `include`). Output patterns use the tokens `{Aggregate}`, `{Agg}` (`{Aggregate}Agg`), `{Entity}`, `{Step}`, `{OutputPath}` (`GenerationOutputPath` from `lazy.settings`, default `LazyCode`) and `{TestOutputPath}` (default `Generated`), relative to the layer project.

## The packages are the runtime

Generated code compiles against the layer runtime packages and the cross-cutting packages. The distribution strategy is declared in the descriptor: layer-specific templates ship in their layer's runtime package (`layers[].templatePackageId` and `templateContentPath`), the shared templates (`project.tt`, `Tests/Tests.Common.ttinclude`) ship only in `Lazy.Crud.Builder.Templates` (`templates.packageId`, `templates.contentRoot`), and every layer package depends on the templates package. `Lazy.Crud.Templates` is a deprecated dependency-only alias listed in `templates.aliasPackageIds`.

## Layers

The eight layers in pipeline order (Domain is always first). Output paths come from the descriptor: `outputPathPattern` is `src/{Aggregate}/{Aggregate}.<Layer>/{OutputPath}`.

| Layer id | Project | Runtime package | Templates in package | Project references |
| --- | --- | --- | --- | --- |
| `Domain` | `{Aggregate}.Domain` | `Lazy.Crud.Builder.Domain` | `LazyCrudTemplates/Domain/` | Application.DTO |
| `Application` | `{Aggregate}.Application` | `Lazy.Crud.Builder.Application` | `LazyCrudTemplates/Application/` | Infra.Data |
| `Application.DTO` | `{Aggregate}.Application.DTO` | `Lazy.Crud.Builder.Application.DTO` | `LazyCrudTemplates/Application.DTO/` | Api.Queries |
| `Infra.Data` | `{Aggregate}.Infra.Data` | `Lazy.Crud.Builder.Infra.Data` | `LazyCrudTemplates/Infra.Data/` | Domain |
| `Infra.IoC` | `{Aggregate}.Infra.IoC` | `Lazy.Crud.Builder.Infra.IoC` | `LazyCrudTemplates/Infra.IoC/` | Application, Infra.Data |
| `Api.Queries` | `{Aggregate}.Api.Queries` | `Lazy.Crud.Builder.Api.Queries` | `LazyCrudTemplates/Api.Queries/` | Enumerations |
| `Enumerations` | `{Aggregate}.Enumerations` | `Lazy.Crud.Builder.Enumeration` | none (`templatePackageId` is null) | none |
| `Api` | `{Aggregate}.Api` | `Lazy.Crud.Builder.Api` | `LazyCrudTemplates/Api/` | Infra.IoC |

What each layer emits (registry writer entries; `GeneratorOwned` unless stated, paths relative to the layer project):

- **Domain**: `{OutputPath}/{Agg}.cs` (global usings), `{Agg}.DomainCommandHandlers.cs`, `{Agg}.DomainCommandModels.cs`, `{Agg}.DomainEventHandlers.cs`, `{Agg}.DomainEventModels.cs`, `{Agg}.Filters.cs`, `{Agg}.IRespositories.cs`, `{Agg}.ProfilesMapping.cs`, `{Agg}.ProfilesListiningMapping.cs`, `{Agg}.Specifications.cs`, `{Agg}.CSVMappingProfiles.cs`. Seeds (`GeneratedOnce`): `Aggregates/{Agg}/Entities/{Aggregate}.cs`, `Aggregates/UsersAgg/Entities/User.cs`, `Aggregates/{Agg}/CommandHandlers/{Entity}CommandHandler.cs`.
- **Application**: `{Agg}.cs`, `{Agg}.AppServices.cs`, `{Agg}.IAppServices.cs`.
- **Application.DTO**: `{Agg}.cs`, `{Agg}.ListiningDTO.cs`, `{Agg}.RequestsDTO.cs`, `{Agg}.SteppableRequestsDTO.cs`, `{Agg}.SteppableRequestsValidators.cs`. Seeds: `Aggregates/{Agg}/Requests/{Entity}DTO.cs`, `Aggregates/{Agg}/Validators/{Entity}Validator.cs`.
- **Infra.Data**: `{Agg}.cs`, `{Agg}.IdentityContext.cs`, `{Agg}.Mappings.cs`, `{Agg}.Respositories.cs`.
- **Infra.IoC**: `{Agg}.cs`, `{Agg}.IoCFactory.cs`.
- **Api.Queries**: `T4/{Agg}.cs` (global usings), `{OutputPath}/{Agg}.QueryModels.cs`.
- **Api**: `{Agg}.Api.GlobalUsings.cs`, `{Agg}.Controllers.cs`, `{Agg}.cs` (IoC factory and migrations).
- **Enumerations**: holds the aggregate enumerations you write; it has no layer templates.

### Test layers

Each layer has a generated test project (`testLayers[]` in the descriptor), transformed after every runtime layer. Project pattern: `src/{Aggregate}/Tests/Lazy.Crud.{Aggregate}.<TestLayer>`; output under `{TestOutputPath}` inside it.

| Test layer id | Target layer | Wrapper | Output |
| --- | --- | --- | --- |
| `Tests.Domain` | Domain | `DomainTest.tt` | `{Agg}.{Entity}EntityTests.g.cs` |
| `Tests.Application` | Application | `ApplicationTest.tt` | `{Agg}.{Entity}AppServiceTests.g.cs` |
| `Tests.DTO` | Application.DTO | `DtoTest.tt` | `{Agg}.{Entity}DtoSerializationTests.g.cs` |
| `Tests.InfraData` | Infra.Data | `InfraDataTest.tt` | `{Agg}.{Entity}MappingTests.g.cs` |
| `Tests.InfraIoC` | Infra.IoC | `InfraIoCTest.tt` | `{Agg}.{Entity}IoCTests.g.cs` |
| `Tests.Api` | Api | `ApiTest.tt` | `{Agg}.{Entity}ControllerTests.g.cs` |
| `Tests.ApiQueries` | Api.Queries | `ApiQueriesTest.tt` | `{Agg}.{Entity}QueryModelTests.g.cs` |
| `Tests.Enumerations` | Enumerations | `EnumerationsTest.tt` | `{Agg}.{Agg}EnumerationsTests.g.cs`, or `NoEnumerations.g.cs` when the aggregate declares none |

The test templates ship in the target layer's package under `LazyCrudTemplates/Tests/<Layer>/`; the shared `Tests/Tests.Common.ttinclude` ships in the templates package.

## The generated code is an artifact

Every file the generator writes is classified `GeneratorOwned`, `GeneratedOnce` or `Manual` ([Ownership](reference/ownership.md)), starts with a marker v2 header when its file type allows a comment ([Marker v2](reference/marker-v2.md)), and is recorded in the aggregate manifest with a SHA-256 content hash computed over the LF-normalized body excluding the marker ([Manifest v2](reference/manifest-v2.md), [Content hash](reference/hash.md)). Before overwriting a `GeneratorOwned` file the writer resolves its state (`verified`, `drifted`, `unverified`, `missing`) and never destroys an edit by default. Everything the generator has to say is a structured diagnostic with a stable `LCnnnn` code ([Diagnostics](reference/diagnostics.md)).

State kept under the consumer solution root:

| Path | Content |
| --- | --- |
| `.lazycrud/manifests/{Aggregate}.manifest.json` | Manifest v2 |
| `.lazycrud/runs/{Aggregate}.run.json` | Run context written by the runner before transformation and deleted afterwards: resolution mode, markers switch, template source. When it is absent (an interactive Run Custom Tool in the IDE) the defaults apply: protect, markers on. |
| `.lazycrud/diagnostics/{Aggregate}.diagnostics.json` | Machine-readable diagnostics of the last run (a JSON array) |

## The pipeline

The runner (`Lazy.Crud.Builder.Platform.Tool.dll generate`) executes one aggregate deterministically:

1. Resolve the template source (`--templates` or `lazy.settings`) and report it (`LC3006`; `LC3002` in local development mode). Package resolution never degrades to a local tree (`LC3001`).
2. Resolve the Visual Studio T4 toolchain (`LC4002`, or `LC4001` and exit 3 before anything is written).
3. Load the manifest (migrating v1 in memory, `LC1009`) and run the drift pre-check for the whole aggregate. With `--resolution protect` (the default) any drifted file stops the run (`LC2001`, exit 7) before the first write.
4. Write the run context and clear the diagnostics file.
5. Ensure the wrappers, `dotnet restore` (2 attempts), open the solution in the embedded Visual Studio session.
6. Domain phase 1 (`DefaultEntities.tt`), verification that `Aggregates/{Aggregate}Agg/Entities` holds entity files, Domain phase 2 (`DefaultCommands.tt`). If either phase fails, no other layer is transformed (the Domain gate).
7. The remaining layers in parallel under the `MaxParallelT4` throttle (default 1), then the test layers.
8. Wait for produced outputs to stabilize, clean wrapper residue, close the solution, post-restore.
9. Merge the writer diagnostics, summarize the manifest, write the report.

Invariants preserved from the IDE extension's pipeline: Domain-first ordering; the Domain layer's two-phase execution; the Domain gate; parallel non-Domain layers under the configured throttle; per-aggregate serialization (one lock per aggregate name, so different aggregates may run concurrently); restore before transformation with bounded retry and a post-restore; bounded retry (2 attempts) around each transformation; polling for produced output with `WaitTimeoutMs` and `PollIntervalMs`; a transformation that produces nothing fails (`LC4004`) regardless of the tool's exit code.

## Surfaces

The Builder repository owns the framework, the packages, the templates, the contracts and the runner. The Visual Studio extension and CLI (a separate repository) consume the same descriptor, manifest, marker, hash, diagnostics and exit-code contracts, and test against the fixtures under `contracts/`. AI coding agents read the same artifacts; see [Using Lazy CRUD with AI coding agents](ai-agents.md).
