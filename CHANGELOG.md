# Changelog

All notable changes to the Lazy CRUD platform are recorded here. The platform follows a single
version for every package (`LazyCrudVersion` in `Directory.Build.props`). Contract-breaking
changes are recorded against the `contractVersion` they change.

## [1.2.0] - 2026-11-25

1.2.0 moves the platform to .NET 11. The contracts do not change (`contractVersion` 1, manifest schema 2,
descriptor schema 1); the target framework and the database stack do, so a consumer still on `net10.0` stays
on 1.1.1. See [Migrations](docs/migrations.md#111-to-120).

### Changed (breaking for net10.0 consumers)

- **Every package targets `net11.0`** (was `net10.0`), and the runner ships under `tools/net11.0/` of
  `Lazy.Crud.Builder.Templates`. The descriptor's `compatibility.hostRequirements.dotnetSdk` is `11.0`, the
  harness sandbox and the generated project files target `net11.0`, and CI installs the .NET 11 SDK.
- **EF Core 11 family.** The runtime packages are compiled against `Microsoft.EntityFrameworkCore.*` 11.0.0,
  `Npgsql` and `Npgsql.EntityFrameworkCore.PostgreSQL` 11.0.0, `EFCore.NamingConventions` 11.0.0 and the
  `Microsoft.AspNetCore.*` / `Microsoft.Extensions.*` 11.0.0 packages. 1.1.1 depended on
  `EFCore.NamingConventions` 10.0.1, whose EF Core range `[10,11)` made every 1.1.1 consumer unable to move to
  EF Core 11 without an `NU1608` and a runtime failure. The project files the generator writes for a consumer
  name the same 11.0.0 versions.

### Fixed

- **The harness sandbox solution builds on the .NET 11 SDK.** `dotnet sln add` follows project references by
  default, so a layer's references were added before their own explicit turn in the list, and the .NET 11 SDK
  rejects that second add ("Solution folder 'src' already contains a project with the filename ..."). The
  sandbox lists every project explicitly anyway, so it now passes `--include-references false`.
- **The v1 manifest migration identifies a template by the layer the entry recorded.** The migration
  matched a v1 entry to its registry template by output file name alone and gave up on any collision.
  `{OutputPath}/{Agg}.cs` is written by a template in each of several layers, so every one of those
  entries was recorded as `Unknown` ownership (LC1008); the shared writer then treated the
  generator's own files as the developer's, every layer reported LC4004, and the first generation
  after upgrading a real solution's v1 manifest failed. A name collision is now narrowed by the
  entry's recorded `Layer` (case-insensitive), and an output whose pattern contains `{Entity}` or
  `{Step}` is matched with those placeholders standing for one identifier each, since a v1 manifest
  names concrete files. A template is still returned only when exactly one candidate remains: two
  writers in the same layer that produce the same name (`application-dto.csv-mapper` and
  `application-dto.listinings`, both `{Agg}.ListiningDTO.cs`) stay `Unknown`, and are never guessed.
  Found by the LazyCrudExtension headless tier (scenario 20.22), whose own migration received the same
  rule in its commit `3773419`.
- **The mixed-version matrix no longer claims a combination NuGet cannot restore.** The matrix in
  `docs/compatibility.md` and the descriptor's `compatibility.mixedVersionRanges` said
  `Lazy.Crud.Builder.Application.DTO.Http.Models`, `Lazy.Crud.Builder.Application.Validators` and
  `Lazy.Crud.CrossCutting.Domain` may stay at any version from 1.0.29 alongside runtime packages at 1.1.1,
  verified by harness scenario R3. The packages that depend on them require at least their own version
  (`Application` requires `Application.Validators`; `Application.DTO` requires `Application.DTO.Http.Models` and
  `Application.Validators`; `Application.DTO.Http.Models` requires `CrossCutting.Domain`; all minimum versions), so
  a lower pin is a downgrade NuGet rejects (NU1605). R3 never exercised that row: it pinned
  `Lazy.Crud.Builder.Api` and `Lazy.Crud.Builder.WebApi` one patch version behind every other package, and since
  the sandbox has no WebApi project, only the Api pin took effect. The matrix, the descriptor, the migration guide,
  the harness description and the LC5002 remedy now state what R3 verifies, `Lazy.Crud.Builder.Api` one patch version behind every
  other package, and the templates rule names that one exception and why no other package that ships templates
  may lag. R3 drops the WebApi pin. `MixedVersionMatrixTests` keeps the three in agreement: R3 pins exactly the
  packages the descriptor names, the sandbox references each of them, and nothing depends on any of them.
  Lowering the three packages' dependency lower bounds to 1.0.29 instead was rejected: NuGet resolves a
  dependency to the lowest version its range allows, so every consumer that does not pin them would have
  received 1.0.29. Found by the LazyCrudExtension headless tier, whose scenario 20.25 could restore only R3's
  combination.

### Changed (contractVersion 1, not breaking)

- The v1 contract fixtures gain four migration cases: `{Agg}.cs` in the Domain and in the Application
  layer, each resolved to its own layer's template; `{Agg}.ListiningDTO.cs` in Application.DTO,
  ambiguous within its layer and still `Unknown`; and an entity-named request DTO, resolved through
  `{Entity}`. The cases already in the set migrate exactly as before. A consumer that pins the fixture
  set by hash re-pins when a version carrying it is published.
- The descriptor's `compatibility.mixedVersionRanges` carries three statements instead of four:
  `Lazy.Crud.Builder.Api` one patch version behind every other package; `Lazy.Crud.Builder.Templates` equal to
  every other package that ships templates; every other package at one version, since pinning a package below
  the version another package requires of it is a downgrade NuGet rejects (NU1605). The field keeps its type
  and meaning, statements for people rather than a machine-readable range, so `contractVersion` stays 1; the v1
  descriptor fixture changes in exactly those lines. A consumer that mirrors the statements, or pins the fixture
  set by hash, updates when a version carrying them is published.

## [1.1.1] - Unreleased

The relaunch ships as 1.1.1. 1.1.0 was prepared and gated but never pushed to nuget.org, so it
exists nowhere a consumer can reach; the version was incremented before the first publication and
every reference in this repository names 1.1.1. Nothing that follows changed between the two.


### Security

- **Exposure notice.** `pack-all.ps1` carried a live nuget.org publish key as a default parameter
  value, committed at `d8bb986` in a public repository, and the working tree carried a second,
  different key. Both defaults are removed: `pack-all.ps1` and `publish-all.ps1` now read the key
  from `-ApiKey` or the `NUGET_API_KEY` environment variable and fail with a clear message when it
  is absent. `scripts/credential-scan.ps1` fails the build on any credential literal.
- **Resolved 2026-09-07.** A new publish credential was issued and 1.1.1 was published with it; the
  credential lives in the user's secret store and never in this repository. The leaked key was purged
  from the local history before the first push, so it never reached the remote -- but it existed in
  plain text on a developer machine and **must still be revoked on nuget.org**, which only the account
  holder can do.

### Added (contractVersion 1)

- `lazycrud.platform.json`, the canonical machine-readable platform descriptor, generated from the
  build and embedded in every package at `lazycrud/lazycrud.platform.json`.
- Generated-code ownership (`GeneratorOwned`, `GeneratedOnce`, `Manual`), marker header v2,
  manifest v2 with the frozen SHA-256 LF-normalized body-excluding-marker content hash, and drift
  protection (`verified`, `drifted`, `unverified`, `missing`) with `--force` / `--skip-modified`.
- `Lazy.Crud.Builder.Platform`: the shared writer, hash, marker, manifest and diagnostics library
  used by the templates and the tooling.
- `Lazy.Crud.Builder.Platform.Tool`: deterministic Visual Studio T4 toolchain discovery, the
  unattended generation runner, descriptor/fixture generation and validation checks.
- Contract fixtures and JSON Schemas under `contracts/` for cross-repository consumers.
- The single-command E2E harness (`scripts/e2e.ps1`).
- **Collapse guard.** A `GeneratorOwned` file that carries content is never replaced by a regenerated
  body that carries none. The check runs ahead of the drift states and applies to all of them,
  including `verified`, which drift protection deliberately allows to be overwritten; a transformation
  that loses its model emits the template's static preamble and stops, and the well-formed contentless
  file it offers is indistinguishable from a legitimate change by hash alone. A body is contentless
  when no line of it is content: blank lines, whole-line comments and import or preprocessor
  directives do not count, and anything the rule cannot classify does. Refused with the new
  diagnostic `LC2012` and the new exit code `9` (`GenerationDefect`), naming the file, template,
  layer, aggregate, both hashes, the significant-line count on each side and the reason. No
  resolution flag overrides it: `--force` resolves an ownership conflict and this is not one. A
  template whose correct output is directives only -- the seven `*.global-usings` templates -- declares
  the new optional registry field `emptyOutputAllowed` and is exempt.
  Found by the closeout battery on 2026-09-05 (`artifacts/e2e/20260905-022527`, scenario F5), where
  three Application.DTO templates emitted a body with no declaration in it. The `unverified`
  protection caught that run; a `verified` file would have been overwritten.

  A collapse is **retried before it is believed**: the pipeline re-transforms the template that
  produced it, because a transformation that loses its model is the same class of transient host
  failure it already retries for engine errors and missing output. A retry that produces content
  records `LC2013` (warning), downgrades the superseded `LC2012` to information and leaves the run
  green with the recovery on the record; only a collapse that survives every attempt stays an error
  and exits 9. Without this, the guard turned an intermittent defect into a failing run: the battery
  of 2026-09-07 hit the real degraded Application.DTO transformation during an ordinary `--force`
  regeneration -- same file, same hash pair as the 2026-09-05 incident -- and the refusal, correct in
  itself, failed a scenario that had nothing to do with it.

  These four additions -- `LC2012`, `LC2013`, exit code `9` and `emptyOutputAllowed` -- are **additive under
  contractVersion 1**: no existing field, diagnostic or exit code changed meaning, and a consumer
  written against contract 1 still parses the descriptor. Per the rule in
  [docs/release.md](docs/release.md), that is not an increment. The `contracts/fixtures/v1/` and
  `contracts/schemas/v1/` artifacts were regenerated before 1.1.1 was ever published, so no published
  fixture changed.

### Changed

- Every package now shares one version source. `Lazy.Crud.Builder.Templates` is packed and
  published with the platform, its MSBuild file is named after the package id so NuGet imports it,
  and shared templates (`project.tt`, `Tests/Tests.Common.ttinclude`) ship only from it.
- The first regeneration after upgrading rewrites every generated file with the marker v2 header.
  The body is unchanged; the golden-output test asserts that only the header differs.
- `publish-all.ps1` pushed "the highest version folder under `artifacts/nuget`". The E2E harness
  packs a second, higher version to exercise mixed-version restore, so after a battery run that
  script's target was the test artifact -- and a push to nuget.org cannot be taken back. It now
  publishes the central version (`LazyCrudVersion`), refuses a folder holding any other version, and
  takes `-Version` for a deliberate override.
- Both publish scripts documented exit code 2 when no credential is present but exited 1:
  `Write-Error` throws under `ErrorActionPreference = Stop`, so the `exit 2` never ran. The message
  is written to the error stream directly and the declared code is what CI sees.
- `Lazy.Crud.Builder.Templates` and `Lazy.Crud.Templates` packed two files named `README.md` at the
  package root -- the repository README that `Directory.Build.props` adds to every package, and their
  own -- which left the reader to decide which one `PackageReadmeFile` resolves to. Each now removes
  the inherited item and ships only its own.

### Deprecated

- `Lazy.Crud.Templates` is published as a dependency-only alias of `Lazy.Crud.Builder.Templates`.
  It carries no content and is scheduled for removal two minor releases after 1.1.1.

### Removed

- **BREAKING (dead configuration):** the `contentFiles/any/any/T4/` glob in `Directory.Build.props`.
  No shipped package ever contained that folder.

### Fixed

- The repository `.gitignore` carried a pasted GitHub page footer (`Docs`, `API`, `Blog`, `About`
  and so on) whose lines acted as ignore patterns. They hid `docs/`, `src/ProjectFiles/Api/`,
  `src/ProjectFiles/Tests/Api/` and every `Api/` folder under `tests/` from Git, so the Api
  templates shipped from the maintainer's disk and were never in a commit. The footer is removed.
- `Api/MiniMigrations.tt` carried a writer call that referenced an undefined `AggName`, so the
  template never compiled and the Api layer produced no migration script. The template now writes
  its host output through the shared writer, and the registry records its real output file
  (`Migrations.generated.bat`) instead of a pattern it never produced.
- The generated tests now compile against the code the generator actually produces: controller
  tests resolve `<Aggregate>.Api.Controllers`, application and IoC tests resolve the
  `...Aggregates.<Aggregate>Agg.AppServices` interfaces, and enumeration tests read the aggregate's
  Enumerations project instead of every enum of every referenced assembly. A stray block after the
  final control block of `Tests/Enumerations/Tests.Enumerations.tt` is removed.
- Diagnostics reported by the T4 engine at warning level (Visual Studio 18 reports
  `Compiling transformation: CSxxxx` as a medium-level entry) now fail the transformation instead
  of being reported as success, and a wrapper's `ErrorGeneratingOutput` placeholder is treated as a
  failure and cleaned up rather than left to break the next build.
- `pack-infra.ps1` is deleted. It packed `src/Core/Core.Infra.IoC`, a path that has never existed in
  this repository, with a hand-passed version that bypassed the central version source. Every
  package, `Lazy.Crud.Builder.Infra.IoC` included, is produced by `pack-all.ps1`.
- A layer that reads the Visual Studio code model of the Domain project could observe a partial set of
  entities while the IDE was still re-parsing the files the Domain phase had just written, so a
  generated file such as the IoC factory could omit registrations. The pipeline now waits for the
  host's view of the Domain project to stop changing before any later layer is transformed. It records the
  settled type count and requires the model to report at least that many types again before each
  later layer, so a re-parse in the middle of the run cannot hand a template a partial entity set.
- A template that produces no content is now a failure. An empty body means the code model resolved to
  nothing (a project that failed to load, or a model still being re-parsed), and writing it would have
  replaced working generated code with an empty file. The writer refuses it, reports `LC3005`, and the
  pipeline retries the transformation; engine errors are retried the same way instead of failing the
  step on the first attempt.
- Generation now refuses to run from a partially parsed code model. Before resolving entities from a
  project, the templates compare the number of types the Visual Studio code model reports with the
  number of entity files on disk; when the model is behind, the transformation fails with `LC3005`
  and the runner retries it. Without the check a template could generate an IoC factory, a controller
  set or a mapping file that silently omitted entities Visual Studio had not finished parsing.
- The default message of `[Unique]` produced invalid C#. It was emitted between single quotes, so the
  generated validator carried a character literal instead of a string (`CS1011`, `CS1003`, `CS1012`),
  and the text was Portuguese inside otherwise English generated code. The message is now a proper
  string literal reading `"<display name> already exists."`. Reported by the extension repository from
  a marketplace model where every aggregate root has a unique business key.
- `sandbox create` discards an extracted package copy that is older than the `.nupkg` in the local feed
  it was told to restore from. NuGet caches by id and version, so a package rebuilt at the same version
  never reached a machine that had already restored it: a run then exercised the previous build while
  reporting success. The E2E harness clears the same cache entries before every run.
- Specifications no longer lowercase an identifier. `string` and `System.Guid` shared a branch of the
  specification template, so `<Entity>Specifications.<Property>Equal` emitted `value.ToLower()` for a
  `Guid` as well. `Guid` has no `ToLower`, the call bound to `MemoryExtensions.ToLower`, and the Domain
  project of any aggregate with an identifier property failed to compile with `CS7036`. Identifiers are
  now compared by value. Found by running the generator against the marketplace fixture, where every
  aggregate references another one by id.
- A protected file now reports where it differs. `LC2001` and `LC2002` carry a `firstDifference` field
  naming the first line that disagrees and quoting both versions of it, instead of leaving a person with
  two hashes and no way to tell whether the edit is worth keeping.
