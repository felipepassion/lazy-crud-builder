# Changelog

All notable changes to the Lazy CRUD platform are recorded here. The platform follows a single
version for every package (`LazyCrudVersion` in `Directory.Build.props`). Contract-breaking
changes are recorded against the `contractVersion` they change.

## [1.1.0] - Unreleased

### Security

- **Exposure notice.** `pack-all.ps1` carried a live nuget.org publish key as a default parameter
  value, committed at `d8bb986` in a public repository, and the working tree carried a second,
  different key. Both defaults are removed: `pack-all.ps1` and `publish-all.ps1` now read the key
  from `-ApiKey` or the `NUGET_API_KEY` environment variable and fail with a clear message when it
  is absent. `scripts/credential-scan.ps1` fails the build on any credential literal.
- **External prerequisite (owner action, not done by this change):** both exposed keys must be
  rotated on nuget.org and the replacement stored as the `NUGET_API_KEY` CI secret. Until rotation
  happens, the keys in git history remain usable by anyone who reads them.

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

  These three additions -- `LC2012`, exit code `9` and `emptyOutputAllowed` -- are **additive under
  contractVersion 1**: no existing field, diagnostic or exit code changed meaning, and a consumer
  written against contract 1 still parses the descriptor. Per the rule in
  [docs/release.md](docs/release.md), that is not an increment. The `contracts/fixtures/v1/` and
  `contracts/schemas/v1/` artifacts were regenerated before 1.1.0 was ever published, so no published
  fixture changed.

### Changed

- Every package now shares one version source. `Lazy.Crud.Builder.Templates` is packed and
  published with the platform, its MSBuild file is named after the package id so NuGet imports it,
  and shared templates (`project.tt`, `Tests/Tests.Common.ttinclude`) ship only from it.
- The first regeneration after upgrading rewrites every generated file with the marker v2 header.
  The body is unchanged; the golden-output test asserts that only the header differs.
- `Lazy.Crud.Builder.Templates` and `Lazy.Crud.Templates` packed two files named `README.md` at the
  package root -- the repository README that `Directory.Build.props` adds to every package, and their
  own -- which left the reader to decide which one `PackageReadmeFile` resolves to. Each now removes
  the inherited item and ships only its own.

### Deprecated

- `Lazy.Crud.Templates` is published as a dependency-only alias of `Lazy.Crud.Builder.Templates`.
  It carries no content and is scheduled for removal two minor releases after 1.1.0.

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
