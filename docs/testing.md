# Testing

There are two things to test: the code the platform generates for your aggregate, and the platform itself (the templates, the writer and the runner). Both are covered without opening an IDE.

## Testing generated code

### Generated test projects

For every layer the platform generates an xunit test project, transformed after all runtime layers (`GenerateTests` in `lazy.settings`, default `True`; `--no-tests` on the runner skips them). Projects live at `src/{Aggregate}/Tests/Lazy.Crud.{Aggregate}.<TestLayer>/`, reference their target layer project and the layer package, and carry one wrapper template.

| Test layer | Template | Generated file | What it covers |
| --- | --- | --- | --- |
| `Tests.Domain` | `Tests/Domain/Tests.Domain.tt` | `{Agg}.{Entity}EntityTests.g.cs` | Entity instantiation, default property values, `ToString`, property setters |
| `Tests.Application` | `Tests/Application/Tests.Application.tt` | `{Agg}.{Entity}AppServiceTests.g.cs` | Application service interface existence and method discovery |
| `Tests.DTO` | `Tests/Application.DTO/Tests.DTO.tt` | `{Agg}.{Entity}DtoSerializationTests.g.cs` | DTO instantiation, JSON serialization and round trips |
| `Tests.InfraData` | `Tests/Infra.Data/Tests.InfraData.tt` | `{Agg}.{Entity}MappingTests.g.cs` | Repository interfaces and entity mapping classes |
| `Tests.InfraIoC` | `Tests/Infra.IoC/Tests.InfraIoC.tt` | `{Agg}.{Entity}IoCTests.g.cs` | IoC factory, registration methods, repository and service resolution |
| `Tests.Api` | `Tests/Api/Tests.Api.tt` | `{Agg}.{Entity}ControllerTests.g.cs` | Controller existence, base class, route and HTTP method attributes per `EndpointsT4` |
| `Tests.ApiQueries` | `Tests/Api.Queries/Tests.ApiQueries.tt` | `{Agg}.{Entity}QueryModelTests.g.cs` | Query model instantiation, pagination, serialization, filters and ordering |
| `Tests.Enumerations` | `Tests/Enumerations/Tests.Enumerations.tt` | `{Agg}.{Agg}EnumerationsTests.g.cs` or `NoEnumerations.g.cs` | Enum members, parsing and `ToString` |

Generated test files are `GeneratorOwned` (they carry the same marker v2 header as production code) and land under `{TestOutputPath}` (default `Generated`). Put your own tests in separate files of the same project; the generator never touches files it did not write. Run everything with:

```powershell
dotnet test
```

### The manifest and the diagnostics are test evidence

After a generation run, `.lazycrud/manifests/{Aggregate}.manifest.json` lists every generated file with its content hash, and `.lazycrud/diagnostics/{Aggregate}.diagnostics.json` lists every diagnostic. A CI job can assert that the manifest validates against the published schema and that the diagnostics contain no `error` entry:

```powershell
dotnet Lazy.Crud.Builder.Platform.Tool.dll validate manifest --file .lazycrud\manifests\Products.manifest.json --repo <lazy-crud-builder checkout>
dotnet Lazy.Crud.Builder.Platform.Tool.dll hash --file src\Products\Products.Domain\LazyCode\ProductsAgg.Filters.cs
```

`hash` prints the frozen content hash of a file so it can be compared with the manifest entry.

## Testing the platform

### Unit tests: Lazy.Crud.Builder.Platform.Tests

`src/Platform/Lazy.Crud.Builder.Platform.Tests` (xunit, `net10.0`) tests the contracts library and the runner without Visual Studio:

| Test class | Covers |
| --- | --- |
| `ContentHasherTests` | CRLF and LF equivalence, byte-order mark, marker exclusion in line and block syntaxes, one-character change, whole-body scope for unmarked files |
| `MarkerTests` | Policies per ownership, `Manual` never rendered, unmarkable types, round trip through every comment syntax, opening-lines-only parsing, v1 and foreign header recognition, malformed v2 problems |
| `OwnershipAndRegistryTests` | Legacy boolean mapping, canonical names and rejected synonyms, unique and stable registry ids, output patterns and ownership on every entry, package mapping per distribution strategy, unambiguous output matching |
| `ManifestTests` | v2 round trip with no absolute paths, refusal of absolute or backslash paths, schema detection, v1 migration without invented data, absent versus unreadable, v1 load and v2 save, save failure reporting, conflicting roots, relative paths after the solution moves |
| `GenerationWriterTests` | New, verified, drifted (edit survives), force, skip-modified, missing, `GeneratedOnce`, legacy boolean, explicit ownership, unregistered template, `Manual` under force, unverified identical adoption, unverified differing protection with the distinct code, migrated v1 entries, locked file, unwritable directory, manifest save failure, markers switch, unmarkable types, whole-aggregate pre-check, marker problem on a matching body |
| `PipelineTests` | Domain first in two phases then layers then tests, Domain gate, throttle, per-aggregate serialization, retry then report, transformation that produces nothing fails, output wait polling, restore retry |
| `SupportTests` | JSON round trips, `lazy.settings` keys and defaults, descriptor resolution (single, agreeing, highest version with diagnostic, missing or malformed, additive fields, contract negotiation), toolchain selection rule, rejection reasons, simulated unavailability, override validation, exit-code table, diagnostic descriptions |

```powershell
dotnet test src\Platform\Lazy.Crud.Builder.Platform.Tests
```

### Golden output of the reference aggregate

`tests/fixtures/reference-aggregate` is the declarative model of the `Products` reference aggregate; `tests/golden/Products/` holds the exact files the generator produces for it through the real Visual Studio T4 toolchain. The regression test regenerates the aggregate in a temporary sandbox (local template mode pointing at the working tree, runtime packages restored from `artifacts/nuget/<version>`) and diffs the result against the golden files after CRLF/LF normalization, comparing the header region separately from the body so that a marker change shows as a header-only difference and any body difference fails. The command and the re-bless rules (an explicit reason in the commit message is mandatory) are in `tests/golden/README.md`.

### Repository validations

`validate tree` is the CI gate over the source tree: registry versus shipped templates in both directions, explicit ownership and `templateId` on every writer call site and agreement with the registry, no silent catch in the shared write path, single version source, no dead packaging glob, shared templates only in the templates package, the attribute metadata contract, the credential scan, the required documentation set and the documentation drift check against the descriptor, and the committed fixtures against the current build. `validate packages` inspects a produced `.nupkg` set against the descriptor; `validate fixtures` compares committed fixtures with freshly produced ones and validates them against their schemas.

```powershell
dotnet src\Platform\Lazy.Crud.Builder.Platform.Tool\bin\Release\net10.0\Lazy.Crud.Builder.Platform.Tool.dll validate tree --repo .
dotnet src\Platform\Lazy.Crud.Builder.Platform.Tool\bin\Release\net10.0\Lazy.Crud.Builder.Platform.Tool.dll validate fixtures --repo .
```

### The E2E harness

`pwsh scripts/e2e.ps1` runs the whole battery unattended on a machine that meets the prerequisite: sandbox creation from the reference aggregate, generation of every layer through the Visual Studio T4 toolchain, marker, manifest and hash validation, build and generated tests, regeneration stability, the quickstart path, compatibility regressions (v1 manifest, deprecated package id, mixed versions, golden output, multi-layer reference), the intentional-failure scenarios (drift blocked, force, skip-modified, unverified differing and identical), the toolchain scenarios and the clean room. See [E2E harness](harness.md).

## The marketplace run

The reference aggregate proves one aggregate end to end. `pwsh scripts/marketplace.ps1` proves the
shape a real product has: one solution with four aggregates (Catalog, Customers, Orders, Sellers),
entities that reference each other across aggregate boundaries by identifier, unique business keys,
and enums delivered by each aggregate's Enumerations project. It creates the solution, generates every
aggregate through the Visual Studio T4 toolchain, builds the whole solution and runs the generated
tests, writing a summary and the per-aggregate generation reports to `artifacts/marketplace/<stamp>/`.

The model lives in `tests/fixtures/marketplace/`. It is the fixture to extend when a defect only
shows up with several aggregates or with an attribute the single reference aggregate does not use.

## Which test covers which requirement

[tests/acceptance-map.md](../tests/acceptance-map.md) lists every requirement of every capability
specification with the unit test, validation check or harness scenario that covers it.
`pwsh scripts/acceptance-map-check.ps1` fails when a name in that table no longer resolves or when a
requirement has no row, so the map cannot quietly rot as tests are renamed. CI runs it beside the
other validation steps.

## Prerequisite for anything that transforms

Unit tests and validations run anywhere the .NET SDK 10.0 is installed. The golden test and the harness transform templates, so they require Windows with Visual Studio 2022 (17.x) or 2026 (18.x) on any channel and the `Microsoft.VisualStudio.Component.TextTemplating` component; without it they fail early with exit code 3 rather than skipping. See [Toolchain](toolchain.md).
