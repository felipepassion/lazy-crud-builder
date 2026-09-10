## 0. External prerequisites (not implementable by the player)

These require access the implementing agent does not have. They are listed without checkboxes so they never block apply progress. The implementable half of each is in the task list below.

- **Rotate the two exposed NuGet publish keys** — the one committed at `HEAD` in `pack-all.ps1:39` and the different one in the working tree — and provision the replacement as a CI secret. Owner: the nuget.org account holder. Tasks 1.1 to 1.3 do not wait on this.
- **Confirm Nutri.Veda's dependency shape.** No `Lazy.Crud` package reference exists in the local checkout, unlike Tio-Sam. Owner: whoever has the current source. Task 11.15 records the outcome; until then the compatibility matrix covers only verified consumers.

## 1. Repository security (implementable)

- [x] 1.1 Remove the API key default from `pack-all.ps1`; read the key from an environment variable and fail with a clear message when it is absent.
- [x] 1.2 Update `publish-all.ps1` the same way; fail when the credential is absent rather than using a default.
- [x] 1.3 Add a credential scan (script + CI step) that fails on an API key literal anywhere in the repository; verify it fails on a planted fixture and passes on the clean tree.
- [x] 1.4 Document the exposure and the rotation requirement in the changelog, pointing at the external prerequisite.

## 2. Golden-output baseline (safety net for every later change)

- [x] 2.1 Pin a reference aggregate (`Products`) and record the exact command that regenerates it today.
- [x] 2.2 Capture the current generated output for every layer as golden files under a test fixture directory.
- [x] 2.3 Add a golden-output regression test that regenerates the reference aggregate and diffs against the golden files.
- [x] 2.4 Verify the golden test passes on the unmodified tree; commit it before any generator change.
- [x] 2.5 Document how to re-bless golden files and require an explicit reason in the commit message.

## 3. Testable seam for generation logic

- [x] 3.1 Create a new library project for output, ownership, marker, hash, manifest and diagnostic logic, with a unit test project.
- [x] 3.2 Define the diagnostic type: stable code, severity, affected path, message, plus a collector interface.
- [x] 3.3 Implement the file writer in the library: create directory, write content, return a result instead of throwing away failures.
- [x] 3.4 Unit-test the writer for success, locked file, unwritable path and missing directory.
- [x] 3.5 Delegate `SaveOutputToSubFolder` in `src/ProjectFiles/project.tt` to the library, keeping its existing signature and default parameter values.
- [x] 3.6 Delegate `SaveOutput` in `project.tt` to the library, keeping its existing signature.
- [x] 3.7 Run the golden-output test; confirm output is unchanged after the delegation.

## 4. Frozen hash semantics

- [x] 4.1 Implement the frozen hash: SHA-256 over the file body, line endings normalized to LF, excluding the Lazy CRUD marker block.
- [x] 4.2 Define the marker block delimiters so exclusion is deterministic for every supported comment syntax.
- [x] 4.3 Define the `hashScope` identifier values: marker-excluded body, and whole body for unmarkable file types.
- [x] 4.4 Unit-test that CRLF and LF inputs with the same body hash identically.
- [x] 4.5 Unit-test that two files differing only in marker content hash identically.
- [x] 4.6 Unit-test that a one-character body change alters the hash.
- [x] 4.7 Unit-test the unmarked-file path where the whole normalized body is hashed.
- [x] 4.8 Expose the hash function as the single implementation used by generation, drift detection, manifest writing and the harness.

## 5. Platform descriptor

- [x] 5.1 Define the `lazycrud.platform.json` schema with `schemaVersion`, `contractVersion`, `platformVersion`, `layers[]`, `templates`, `attributes[]`, `capabilities[]` and `compatibility`.
- [x] 5.2 Publish the JSON Schema for the descriptor.
- [x] 5.3 Populate `layers[]` with `id`, `projectSuffix`, `projectPattern`, `packageId`, `templatePackageId`, `templateContentPath` and `outputPathPattern` from the shipped packages.
- [x] 5.4 Assign a stable `templateId` to every shipped template and record the id assignment so it never changes silently.
- [x] 5.5 Build the `templates.registry[]` with `id`, `layer`, `sourceFile`, `outputPattern`, `ownership`, `templateVersion`, `packageId` and `inPackagePath`.
- [x] 5.6 Declare `templates.packageId`, `aliasPackageIds[]` (including `Lazy.Crud.Templates`), `version` and `contentRoot`.
- [x] 5.7 Build the `attributes[]` catalog with `name`, `fullName`, `namespace`, `packageId`, `category`, `applicability`, `overloads[]` and `documentationId`.
- [x] 5.8 Declare `capabilities[]` and `compatibility`, including the supported `contractVersion` range, the minimum upgradable version and the minimum manifest `schemaVersion`.
- [x] 5.9 Generate the descriptor during build from MSBuild properties and the registry; embed it in every packable project at a fixed path.
- [x] 5.10 Add a validation step asserting every declared layer's `packageId` and `templatePackageId` matches a produced package, and every produced runtime package is declared.
- [x] 5.11 Add a validation step asserting every registry entry resolves to a real file at its `packageId` + `inPackagePath`, and every shipped template has a registry entry.
- [x] 5.12 Add a validation step asserting the attribute catalog and the shipped assemblies agree in both directions.
- [x] 5.13 Implement the descriptor reader with the mixed-version precedence rule (highest `platformVersion` wins) emitting a diagnostic on disagreement.
- [x] 5.14 Unit-test the reader for single-package, multi-package agreeing and multi-package conflicting cases.
- [x] 5.15 Document what increments `contractVersion` and verify an additive field does not break a reader written against the previous version.

## 6. Ownership model

- [x] 6.1 Define the ownership enum (`GeneratorOwned`, `GeneratedOnce`, `Manual`) in the library with XML documentation, using exactly these names.
- [x] 6.2 Map the legacy boolean: `recreateFileIfExists: true` to `GeneratorOwned`, `false` to `GeneratedOnce`; unit-test both mappings.
- [x] 6.3 Add an optional explicit ownership parameter to the writer that overrides the boolean; unit-test the override.
- [x] 6.4 Implement `GeneratedOnce` semantics (write only when absent) and unit-test create-when-absent and leave-when-present.
- [x] 6.5 Confirm `Manual` paths are never written under any flag, including force; unit-test it.
- [x] 6.6 Treat an existing file with no manifest ownership and no marker as user-owned; unit-test that it is not overwritten.
- [x] 6.7 Run the golden-output test after the ownership refactor; output must be unchanged.
- [x] 6.8 Add explicit ownership to the 43 call sites passing `recreateFileIfExists: true` in `src/ProjectFiles/**/*.tt`.
- [x] 6.9 Add explicit ownership to the 5 call sites passing `recreateFileIfExists: false`.
- [x] 6.10 Add explicit ownership to the 12 call sites that omit the argument and rely on the default; confirm the total reaches all 60 writer call sites.
- [x] 6.11 Add an ownership coverage check that fails when any writer call site lacks explicit ownership; wire it into CI.
- [x] 6.12 Add a check that each call site's ownership agrees with the descriptor registry entry for its template; fail naming any disagreement.
- [x] 6.13 Emit a diagnostic and treat output as user-owned when a template produces a file but has no registry entry.

## 7. Marker v2

- [x] 7.1 Define the marker v2 format carrying `<auto-generated>`, generator identity, `Aggregate`, `Layer`, `TemplateId`, `Ownership`, `GeneratorVersion` and `TemplateVersion`.
- [x] 7.2 Write the distinct `GeneratorOwned` directive stating edits are overwritten and naming where to customize instead.
- [x] 7.3 Write the distinct `GeneratedOnce` directive stating the file is now user-owned and will not be regenerated.
- [x] 7.4 Implement marker emission in the shared writer, keyed off the ownership value; do not mark `Manual` outputs.
- [x] 7.5 Implement comment-syntax selection per file type; omit the marker where no safe syntax exists but still record the file in the manifest.
- [x] 7.6 Implement a marker parser that extracts every frozen field from a file's opening lines.
- [x] 7.7 Unit-test emission and round-trip parsing for both marked ownerships and each supported file type.
- [x] 7.8 Implement v1 marker recognition for detection, and unit-test that a v1 marker alone does not authorize overwrite.
- [x] 7.9 Unit-test that a missing or malformed marker on a hash-matching file is reported as a marker problem, not as drift.
- [x] 7.10 Replace the bespoke header in `src/ProjectFiles/Tests/Tests.Common.ttinclude` with the shared marker.
- [x] 7.11 Re-bless the golden files and assert the only diff is inside the marker block.
- [x] 7.12 Add a marker coverage check over the reference aggregate output; fail naming any file without a valid marker.
- [x] 7.13 Put marker emission behind a descriptor-declared capability flag so it can be disabled without reverting code.

## 8. Manifest v2

- [x] 8.1 Define the manifest v2 schema with `schemaVersion`, `aggregateName`, `generatedAtUtc`, `generatorVersion`, `contractVersion` and the per-file fields.
- [x] 8.2 Publish the JSON Schema for manifest v2.
- [x] 8.3 Implement the v2 writer producing solution-relative forward-slash paths; unit-test that no absolute path, drive letter or backslash path is ever emitted.
- [x] 8.4 Populate `generatorVersion` and `contractVersion` from the descriptor instead of the hardcoded `"1.0"`.
- [x] 8.5 Record `templateId`, `templateVersion`, `layer`, `ownership`, `contentHash`, `hashAlgorithm` and `hashScope` for every entry; unit-test that none is empty.
- [x] 8.6 Assert every written `templateId` exists in the descriptor registry.
- [x] 8.7 Confirm the writer never emits `FullPath` or any other absolute path, including during migration.
- [x] 8.8 Implement the v1 reader: treat missing `schemaVersion` as v1 and load without error.
- [x] 8.9 Convert resolvable v1 absolute paths to relative paths; retain and mark unresolvable entries with a warning instead of dropping or guessing them.
- [x] 8.10 Record missing v1 hashes as unknown rather than as a match; unit-test that unknown is distinguishable from equal.
- [x] 8.11 Record ownership as unknown when neither the v1 entry nor the registry supplies one, and treat such files as user-owned.
- [x] 8.12 Persist the upgrade to v2 on the next successful generation, keeping still-unresolved entries marked; unit-test with the real v1 `Products.manifest.json` fixture.
- [x] 8.13 Replace the silent catches in manifest load/save/delete with diagnostics; unit-test corrupt-manifest and unwritable-path cases.
- [x] 8.14 Return an explicit not-present result for an absent manifest, distinguishable from a read failure.
- [x] 8.15 Implement single-root resolution and emit a diagnostic when manifests for one aggregate exist under multiple roots; reproduce using the existing `.lazycrud` and `src/.lazycrud` duplication.
- [x] 8.16 Verify a manifest written on one path resolves correctly when the solution is moved to a different absolute path.

## 9. Drift and unverified-baseline protection

- [x] 9.1 Implement state resolution for `GeneratorOwned` files into `verified`, `drifted`, `unverified` or `missing` using the frozen hash.
- [x] 9.2 Resolve the states for the whole aggregate before the first file is written; unit-test the ordering.
- [x] 9.3 Implement the drift diagnostic with a stable code carrying relative path, expected hash, actual hash, `templateId` and available resolutions.
- [x] 9.4 Implement the unverified diagnostic with a stable code distinct from drift, stating that no baseline exists.
- [x] 9.5 Implement safe adoption: when an unverified file's regenerated body hashes identically to the on-disk body, record the baseline and continue without user intervention.
- [x] 9.6 Implement protection: when an unverified file's regenerated body differs, do not overwrite and fail without an explicit flag.
- [x] 9.7 Implement default behavior for drifted files: do not overwrite, report every affected file, fail the run.
- [x] 9.8 Implement the force flag (overwrite and report each overwrite), and confirm it still never touches `Manual` files.
- [x] 9.9 Implement the skip-modified flag (leave affected files, generate the rest, report success listing skips).
- [x] 9.10 Ensure a missing file and a genuinely new file are written without being reported as drift; unit-test both.
- [x] 9.11 Unit-test that after a default-mode run the external modification is still present on disk.
- [x] 9.12 Unit-test that a v1-migrated unknown-hash file with differing content is protected, not overwritten.
- [x] 9.13 Document the flags, both diagnostic codes and the state model.

## 10. Diagnostics cleanup

- [x] 10.1 Remove the empty `catch (Exception ex) { }` around the write in `project.tt:1652`, routing failures to diagnostics.
- [x] 10.2 Replace `catch { /* ignore malformed project entries */ }` (`project.tt:535`) with a recorded diagnostic naming the unresolved entry.
- [x] 10.3 Replace the swallowed exception in `FindType` (`project.tt:759`) with a recorded diagnostic.
- [x] 10.4 Review the remaining catches in `project.tt` and either justify each in a comment or convert it to a diagnostic.
- [x] 10.5 Classify each diagnostic as error, warning or information; make the run fail if and only if an error was emitted.
- [x] 10.6 Write diagnostics to a machine-readable file for headless runs.
- [x] 10.7 Add a repository check asserting the shared write path contains no exception handler that discards its exception; wire it into CI.
- [x] 10.8 Emit an explicit diagnostic when template resolution fails, naming the configured source, the package id and every location probed.
- [x] 10.9 Make package-based resolution fail rather than degrade to local; report the resolved template source kind, location and version on every run.
- [x] 10.10 Make `GetGenerationOutputPath` report a diagnostic instead of silently returning an empty string when `lazy.settings` cannot be found from a package-installed template location.

## 11. Packaging, versioning and distribution

- [x] 11.1 Move the version to a single central property; remove the literal `<Version>` from the 16 project files.
- [x] 11.2 Add a validation step failing the build when a project declares a version that disagrees with the central version.
- [x] 11.3 Add the templates project to the pack list in `pack-all.ps1`.
- [x] 11.4 Fix pack failure detection in `pack-all.ps1`: check the native exit code so a failed pack fails the run.
- [x] 11.5 Rename the templates package MSBuild file to match its `PackageId` so NuGet auto-imports it; verify the import in a consuming project.
- [x] 11.6 Add the MSBuild integration to the templates package so a build-driven invocation reaches the Visual Studio T4 tooling instead of silently transforming nothing.
- [x] 11.7 Verify in a sandbox project that consuming the templates package makes transformation reachable unattended on a machine meeting the prerequisite, and that it produces the expected outputs.
- [x] 11.8 Move `project.tt` and `Tests/Tests.Common.ttinclude` into the canonical templates package and remove them from the per-layer packages.
- [x] 11.9 Make each layer package depend on the templates package so multi-layer consumers still receive shared templates.
- [x] 11.10 Keep layer-specific templates in their layer packages and confirm they match the descriptor's per-layer declarations.
- [x] 11.11 Remove the dead `**\*.tt` glob targeting `contentFiles/any/any/T4/` from `Directory.Build.props`.
- [x] 11.12 Add a validation step failing when two produced packages declare the same `contentFiles` path.
- [x] 11.13 Publish `Lazy.Crud.Templates` as a deprecated dependency-only metapackage depending on `Lazy.Crud.Builder.Templates`; confirm it carries no template content.
- [x] 11.14 Fix or delete `pack-infra.ps1`, which packs the non-existent `src/Core/Core.Infra.IoC`.
- [x] 11.15 Author the compatibility matrix covering platform versions, contract versions, manifest schema versions and supported mixed-version combinations, and record the Nutri.Veda verification outcome.
- [x] 11.16 Add an automated check exercising each documented combination, including the Tio-Sam mixed pin (`1.0.32` with `1.0.29`).
  - Corrected: R3 never exercised the Tio-Sam pin, and no check can. The 1.1.1 packages that depend on `Application.DTO.Http.Models`, `Application.Validators` and `CrossCutting.Domain` require them at 1.1.1, so pinning them at 1.0.29 next to 1.1.1 is a downgrade NuGet rejects (NU1605). The matrix now documents that pin as unsupported, and the one mixed combination that restores, `Lazy.Crud.Builder.Api` one patch version behind every other package, is the one R3 exercises; `MixedVersionMatrixTests` keeps the matrix, the descriptor and R3 in agreement (CHANGELOG [Unreleased]).

## 12. Visual Studio T4 toolchain prerequisite

- [x] 12.1 Declare the supported Visual Studio versions, channels and the component that supplies T4 tooling as the platform's host prerequisite.
- [x] 12.2 Record that declaration in the descriptor under `compatibility.hostRequirements` and confirm it is an additive optional field that does not break a reader written against a descriptor without it.
- [x] 12.3 Implement toolchain discovery through the Visual Studio Installer query tool (`vswhere`) or the Setup Configuration API, resolving both the instance and the T4 tooling path.
- [x] 12.4 Include prerelease and Insiders channels in discovery; verify against a machine whose only instance is prerelease, where a query without the prerelease flag returns nothing.
- [x] 12.5 Define and document the deterministic selection rule when several instances are installed; verify repeated runs resolve the same instance.
- [x] 12.6 Support an explicit instance or tooling-path override, and report when one was applied.
- [x] 12.7 Remove the recursive `${env:ProgramFiles}\Microsoft Visual Studio` scan from `build/run-t4-templates.ps1` in favour of the supported discovery mechanism.
- [x] 12.8 Validate the resolved tooling exists and is executable before any template is transformed.
- [x] 12.9 Implement the prerequisite-unavailable diagnostic with a stable code naming the requirement, the discovery performed, what was found and the component to install.
- [x] 12.10 Fail with the prerequisite-unavailable exit code when the instance or its T4 tooling cannot be resolved, before transforming anything.
- [x] 12.11 Remove any path that treats a plain project build as a substitute for transformation, and fail a transformation step that produces none of its expected outputs.
- [x] 12.12 Report the resolved instance, version, channel and tooling path in every generation run's machine-readable output.
- [x] 12.13 Preserve Domain-first ordering, the Domain two-phase execution and the Domain success gate; add regression tests covering them.
- [x] 12.14 Preserve the configured concurrency throttle and per-aggregate serialization; add regression tests covering both.
- [x] 12.15 Preserve restore ordering and the existing output wait and retry behavior; add regression tests rather than simplifying them away.
- [x] 12.16 Confirm via the golden-output test that the chosen transformation path reproduces the previous Visual Studio result, unchanged apart from the marker block.
- [x] 12.17 Unit-test discovery, validation and the prerequisite diagnostic with the toolchain present, absent and unresolvable.

## 13. Attribute metadata contract

- [x] 13.1 Define the controlled `<category>` vocabulary from the categories currently in use across the 43 attribute files.
- [x] 13.2 Add the missing `<summary>` and `<category>` to `SubMenuSection.cs`.
- [x] 13.3 Decide and apply the fix for `MenuSection` and `SubMenuSection`, which are documented as attributes but do not derive from `Attribute`.
- [x] 13.4 Review every public attribute's `<summary>` for user-facing phrasing suitable for a picker.
- [x] 13.5 Add a `<param>` entry for every attribute constructor parameter that lacks one.
- [x] 13.6 Add `<remarks>` where behavior is not obvious from the summary.
- [x] 13.7 Record each attribute's namespace and delivering package in the catalog, covering the three current namespaces.
- [x] 13.8 Derive `applicability` and constructor `overloads[]` for every attribute and verify them against the assemblies.
- [x] 13.9 Emit `documentationId` for every catalog entry and verify it resolves in the shipped XML documentation file.
- [x] 13.10 Implement the attribute metadata check: summary present, category present and in vocabulary, params documented, catalog derives from `Attribute`, catalog and assembly agree both ways.
- [x] 13.11 Wire the metadata check into CI and confirm it fails on a planted violation.
- [x] 13.12 Verify the produced package's XML documentation file still contains the category entries after the changes.

## 14. Contract fixtures for cross-repository consumption

- [x] 14.1 Generate the descriptor fixture from the same code path that writes the real descriptor.
- [x] 14.2 Generate the manifest v2 fixture from the real manifest writer.
- [x] 14.3 Publish the v1 input fixture and its expected migrated v2 output, including an unresolvable entry and an unknown-hash entry.
- [x] 14.4 Publish marker examples for `GeneratorOwned` and `GeneratedOnce` in each supported comment syntax.
- [x] 14.5 Publish hash vectors covering marker exclusion, CRLF/LF equivalence and an unmarked file, with expected values.
- [x] 14.6 Stamp every fixture with the `contractVersion` it represents and keep prior versions retrievable.
- [x] 14.7 Add a check that fails when a fixture differs from what the implementation currently produces.
- [x] 14.8 Validate every document fixture against its published JSON Schema in CI.
- [x] 14.9 Add a test that recomputes every hash vector with the production implementation and asserts the recorded values.
- [x] 14.10 Publish the fixtures in a retrievable artifact at a documented, stable location obtainable without a Builder source checkout.
- [x] 14.11 Document the fixture set, its location and the rule that the fixture wins when prose and artifact disagree.

## 15. Continuous integration

- [x] 15.1 Add a CI workflow that restores, builds and runs unit tests on push and pull request.
- [x] 15.2 Add the pack step to CI and fail on any packing error.
- [x] 15.3 Wire in the validation checks: descriptor, template registry, attribute catalog, ownership coverage, ownership/registry agreement, marker coverage, no-silent-catch, content-path collisions, credential scan, version drift, fixture drift.
- [x] 15.4 Publish CI artifacts: produced packages, generated descriptor, contract fixtures and validation reports.
- [x] 15.5 Gate publication on all validation checks, the clean-room phase and the E2E harness passing.

## 16. Documentation

- [x] 16.1 Rewrite `README.md` as a product introduction; remove the `.NET Core 8.0` claim and the unrelated deployment/technology content.
- [x] 16.2 Write the quickstart taking a reader from an empty project to generated, compiling code using published packages only.
- [x] 16.3 Write the architecture page: model as input, templates as compiler, packages as runtime, generated code as artifact.
- [x] 16.4 Write the aggregates, entities and attributes reference, generated from or verified against the attribute catalog.
- [x] 16.5 Write the generated-code policy page: ownership values, markers, what is safe to edit.
- [x] 16.6 Write the safe-customization guide covering the correct extension points per layer.
- [x] 16.7 Write the migrations guide, including the marker-header diff expected on first regeneration after upgrade.
- [x] 16.8 Write the testing guide for generated code and for templates.
- [x] 16.9 Publish the compatibility matrix page from task 11.15.
- [x] 16.10 Write the troubleshooting guide, including drift and unverified diagnostics, template resolution failures and the prerequisite-unavailable diagnostic with its remedy.
- [x] 16.11 Write changelog and release guidance, including the deprecation timeline for the alias package.
- [x] 16.12 Write "Using Lazy CRUD with AI coding agents": ownership, marker reading, correct edit target, drift and unverified diagnostics, the Visual Studio prerequisite and why a failed prerequisite must be reported rather than worked around, and the descriptor and manifest as authoritative machine-readable sources.
- [x] 16.13 Write reference pages for each frozen contract: descriptor, manifest v2, marker v2, ownership model and hash semantics, each linking to its fixture and schema.
- [x] 16.14 Document the current `contractVersion`, the supported range and what an increment implies for consumers.
- [x] 16.15 Reconcile or retire the existing root documents (`CHECKLIST.md`, `IMPLEMENTATION_SUMMARY.md`, `INSTRUCTIONS.md`, `QUICKSTART-E2E.md`, `TEST_OUTPUT_EXAMPLES.md`, `build/*.md`) so no stale document contradicts the new set.
- [x] 16.16 Implement the documentation drift check comparing documented version, layer and package facts against the descriptor; wire it into CI.

## 17. Clean-room public distribution

- [x] 17.1 Implement sandbox creation with an isolated `NUGET_PACKAGES` directory per run.
- [x] 17.2 Generate a `nuget.config` that clears all inherited sources and adds only the feed under test.
- [x] 17.3 Add the resolver option that disables local fallback, defaulting to today's behavior and enabled only by the harness and clean-room runs.
- [x] 17.4 Assert the sandbox contains no copy of the Builder template sources and resolves nothing outside itself.
- [x] 17.5 Assert every package the descriptor declares is present in the feed under test; fail naming any missing package.
- [x] 17.6 Assert the embedded descriptor is readable from a restored package and declares the expected `schemaVersion`, `contractVersion` and `platformVersion`.
- [x] 17.7 Assert every template registry entry resolves from the restored packages using only `packageId` and `inPackagePath`.
- [x] 17.8 Assert the attribute catalog assembles from the restored descriptor and XML documentation with no platform source present.
- [x] 17.9 Assert that selecting local template resolution fails the clean-room run rather than degrading to it.
- [x] 17.10 Build a consumer project in the sandbox from the published packages and confirm restore and build succeed.
- [x] 17.11 Generate a sample aggregate in the sandbox from restored templates only, unattended via the resolved Visual Studio T4 tooling, and confirm the generated code compiles.
- [x] 17.12 Assert the clean-room host satisfies the declared prerequisites and record the resolved toolchain in the run output, treating it as satisfied rather than as a fallback.
- [x] 17.13 Document exactly what a clean-room host must have installed and what must be absent, and confirm it matches the published compatibility matrix.

## 18. End-to-end verification harness

- [x] 18.1 Create the orchestrator entry point with no required arguments and an option to run a single named scenario.
- [x] 18.2 Drive harness transformation through the resolved Visual Studio T4 tooling from task group 12; remove the recursive program-files scan in `build/run-t4-templates.ps1`.
- [x] 18.3 Implement prerequisite checks (.NET SDK, Visual Studio T4 toolchain) that fail with the prerequisite-unavailable code and never skip generation.
- [x] 18.4 Assert that a transformation step producing none of the scenario's declared outputs fails, rather than passing on a zero exit code.
- [x] 18.5 Define and document the shared exit-code table: success, assertion failure, prerequisite unavailable, invalid fixture or sandbox, timeout, contract mismatch, ownership conflict.
- [x] 18.6 Implement per-scenario and global timeout budgets that exit with the timeout code.
- [x] 18.7 Implement the per-run artifacts directory keyed by timestamp, never overwriting previous runs.
- [x] 18.8 Implement the structured run summary recording each scenario's id, status, duration, failure reason, platform version, contract version and package versions.
- [x] 18.9 Implement the human-readable digest alongside the structured summary.
- [x] 18.10 Implement the final single-line verdict with passed and failed counts.
- [x] 18.11 Retain logs, generated output and diagnostics for every failed scenario.
- [x] 18.12 Assert the Builder working tree is unmodified after a run apart from the artifacts directory.
- [x] 18.13 Make the harness compute hashes with the production implementation and the published vectors, defining no expected values of its own.
- [x] 18.14 Make the harness validate produced descriptors and manifests against the published JSON Schemas.
- [x] 18.15 Scenario P1: create the sample aggregate and generate every layer the descriptor declares.
- [x] 18.16 Scenario P2: validate marker v2 on every generated file, including the distinct `GeneratedOnce` directive and the absence of markers on `Manual` outputs.
- [x] 18.17 Scenario P3: validate the manifest is v2 with forward-slash relative paths, no absolute paths, and `templateId`, `ownership`, `contentHash`, `hashAlgorithm`, `hashScope` and `contractVersion` populated.
- [x] 18.18 Scenario P4: recompute hashes with the frozen rule and assert they match the manifest.
- [x] 18.19 Scenario P5: build the sandbox solution and run its generated tests.
- [x] 18.20 Scenario P6: regenerate with no model change and assert no drift and unchanged content.
- [x] 18.21 Scenario P7: run the documented quickstart path and fail the harness if it fails.
- [x] 18.22 Scenario R1: seed the published v1 manifest fixture with unmodified files; assert success without a flag, upgrade to the expected v2 output, and no drift or unverified failure.
- [x] 18.23 Scenario R2: configure the deprecated templates package id; assert templates resolve and a deprecation notice is reported.
- [x] 18.24 Scenario R3: pin a supported mixed-version combination; assert restore, build and generation succeed.
- [x] 18.25 Scenario R4: assert generated output matches the golden files apart from marker headers.
- [x] 18.26 Scenario R5: reference two or more layer packages; assert no duplicate template content and a clean build.
- [x] 18.27 Scenario F1: modify a `GeneratorOwned` file externally, regenerate with no flag; assert the run fails with the ownership-conflict code, assert the specific drift diagnostic code and file path, and assert the modification survives.
- [x] 18.28 Scenario F2: re-run with force; assert the file is regenerated and the overwrite is reported.
- [x] 18.29 Scenario F3: re-run with skip-modified; assert the modification survives, other files generate, and the run reports success listing the skip.
- [x] 18.30 Scenario F4: seed a v1-migrated unknown-hash file whose content differs from current output; assert the run fails with the unverified diagnostic code, not the drift code, and the file is unchanged.
- [x] 18.31 Scenario F5: assert an unverified file whose regenerated body is identical is adopted silently and its hash recorded.
- [x] 18.32 Assert the intentional-failure scenarios are recorded as passes and do not make the overall exit code non-zero.
- [x] 18.33 Run the orchestrator twice consecutively and assert identical scenario outcomes.
- [x] 18.34 Add the harness to CI as the release gate.
- [x] 18.36 Scenario T1 (positive): assert the Visual Studio T4 toolchain resolves, record instance, version, channel and tooling path in the summary, and complete a full unattended generation.
- [x] 18.37 Scenario T2 (negative): make the toolchain unresolvable, assert the run fails before transforming anything with the prerequisite-unavailable code and the correct diagnostic, assert no generated output was produced, and record it as a passing scenario.
- [x] 18.38 Assert across the positive scenarios that transformation actually occurred — outputs exist, carry marker v2, appear in the manifest with matching hashes, and the solution builds with its generated tests passing — so a bare project build cannot satisfy them.
- [x] 18.39 Assert no scenario requires Visual Studio to be open, focused or driven by a person.
- [x] 18.40 Document how to run the harness and how to read its artifacts, for both a human and a headless agent, including the Visual Studio prerequisite.

## 19. Player acceptance

- [x] 19.1 Execute the complete E2E macro in a single command from a clean checkout and capture the run directory.
- [x] 19.2 Confirm the exit code is the success code and the final verdict line reports zero failed scenarios.
- [x] 19.3 Review the structured summary and confirm every scenario from 18.15 to 18.31 and the toolchain scenarios in 18.36 to 18.37 are present and passed, with no scenario silently skipped.
- [x] 19.4 Review the generated sandbox output by hand: marker v2 correct per ownership, manifest v2 with relative paths and populated hashes, generated code compiling.
- [x] 19.5 Confirm the drift scenario proved protection by asserting its diagnostic code, and the unverified scenario asserted a distinct code.
- [x] 19.6 Confirm the clean-room phase consumed only published artifacts, with no local source or private feed reachable and local mode refused, on a host meeting the declared prerequisites.
- [x] 19.7 Confirm the toolchain scenarios proved both a real unattended transformation and an early, correctly diagnosed failure when the toolchain is unresolvable.
- [x] 19.8 Confirm the published fixtures match what the implementation produces and validate against their schemas.
- [x] 19.9 Confirm every acceptance criterion in each capability spec is covered by a test, a validation check or a harness scenario; record the mapping.
- [x] 19.10 Confirm no credential literal remains in the repository and the changelog records the rotation requirement.
- [x] 19.11 Verify the documented quickstart works for an external consumer following it verbatim.
- [x] 19.12 Report the status of the external prerequisites in section 0 without blocking on them.
- [x] 19.13 Only after 19.1 through 19.12 pass, mark this OpenSpec change implemented and ready to archive.

## Closeout - 2026-09-05

The change was fully checked but had never been committed or re-verified end to end. This pass ran the
gates and the battery on the real toolchain, fixed what they found, and committed the work.

**Verified**

- Platform unit tests 107 of 107; `validate tree` 24 of 24; `validate fixtures` 8 of 8; acceptance map OK;
  credential scan clean; Release build 0 warnings 0 errors.
- E2E battery `RESULT: OK (passed=21 failed=0)` against Visual Studio Professional 2026
  (18.10.12113.136, prerelease) through its own TextTransform tooling. Evidence:
  `artifacts/e2e/20260905-041226`.
- Transformation is proven from the run report, not from a build: 18 templates transformed, Domain gate
  passed, 43 files in the manifest (schema 2, contract 1), templates resolved from the restored
  `Lazy.Crud.Builder.Templates` package.

**Defects the battery found, and what was done**

- `DteSession.Flatten` dereferenced a solution folder while the project system was reloading; the
  NullReferenceException ended the clean-room scenario after its transformation had already succeeded.
  Walking a solution folder is defensive at every read now.
- `generate` wrote no report when the pipeline failed unexpectedly, so a diagnosable failure reached the
  harness as "the report file does not exist". The report is always written.
- `validate` wrote its report without creating the parent directory, so a passing CI gate died on
  `artifacts/validation/` in a fresh clone (exit 8 after 24 of 24 checks passed).
- Waiting for a solution to load treated a slow project-system poll as a failed open, ending a scenario a
  minute in with four minutes of budget left. A slow poll is now reported and retried.
- The harness read the working tree with git attached to the console; after driving devenv that killed a
  completed run with "No process is on the other end of the pipe".
- The sandbox grouped every layer in a solution folder of its own -- ten folders for sixteen projects, each
  repeating its project name. Layers now sit under `src/<Aggregate>` and tests under
  `src/<Aggregate>/Tests`, which is what `LazyCrud.Core` has always created for real consumers.

**Open, with evidence**

- ~~Intermittent degraded Application.DTO output.~~ **Closed 2026-09-07, see the closeout below.**
- Intermittent degraded Application.DTO output. In one battery run (`artifacts/e2e/20260905-022527`, F5)
  three DTO templates emitted a marker-only body; regenerating the same sandbox reproduced the full body
  byte for byte, and two later batteries passed. The unverified protection caught it and refused the write,
  naming file, templateId, both hashes and the first differing line -- no content was lost. The exposure is
  the *verified* path: with a baseline present, that same collapsed body would have been written over good
  generated code without question. Closing it needs a rule that refuses to overwrite a non-empty generated
  body with an empty one, which appends a diagnostic code and therefore a contract revision. Not done here.
- Rotating the two exposed NuGet publish keys remains the account holder's, as recorded in section 0.

## Closeout - 2026-09-07: the collapsed-output hole is closed

**The defect, reproduced exactly.** A `GeneratorOwned` file that carries content is never replaced by a
regenerated body that carries none. The check runs ahead of the drift states and applies to all of them,
including `verified`, which drift protection deliberately allows to be overwritten and which was the
exposure the previous closeout left open. A body is contentless when no line of it is content: blank
lines, whole-line comments and import or preprocessor directives do not count, and anything the rule
cannot classify does, so the refusal is only ever raised on a body demonstrably empty of content. The
rule is not a threshold; a file that shrinks because an entity was removed still declares something.
Refused with `LC2012` and exit code 9 (`GenerationDefect`), naming file, template, layer, aggregate,
both hashes, the significant-line count on each side and the reason. No resolution flag overrides it.
The seven `*.global-usings` templates, whose correct output is directives only, declare
`emptyOutputAllowed` in the registry; a test asserts every contentless output in the golden corpus is
covered by such a declaration.

**A collapse is retried before it is believed.** A transformation that loses its model is the same class
of transient host failure the pipeline already retries. It re-transforms the template whose output was
refused; a retry that produces content records `LC2013` (warning) and downgrades the superseded `LC2012`
to information, so the run is green with the recovery on the record. Only a collapse that survives every
attempt keeps the error and exits 9.

**Evidence.**

- The defect caught in the wild, unprompted: battery `artifacts/e2e/20260907-013918`, scenario F2, an
  ordinary `--resolution force` regeneration of an unmodified template tree. `LC2012` on
  `ProductsAgg.ListiningDTO.cs`, on-disk hash `6f37411b...`, regenerated `d709d3bc...` -- the same pair
  recorded in the 2026-09-05 F5 incident. Nine lines of good generated code survived a force that would
  have destroyed them before this change. That run also proved the guard alone was not enough: the
  refusal, correct in itself, failed a scenario about drift resolution. Hence the retry.
- Harness scenario F6 proves it in a real Visual Studio transformation rather than a stub: a copy of the
  template tree with one edit (`ListiningsDTO.tt` takes its "no entities" branch), 18 templates
  transformed, `LC2012` with the full diagnostic, exit 9, `--force` also exit 9, the generated file
  byte-identical afterwards, and the declared contentless output still regenerating.
- Unit tests: `OutputCollapseTests` (12 cases) for the rule and the writer, `PipelineTests` (3 cases) for
  the retry, the persistent case and the cross-layer correlation. The third found two defects in the
  first draft of the retry: an entry already in the diagnostics file counted as a recovery, and repeated
  collapses of the same path were deduplicated into a silent one.
- Release gate green on a clean tree: `artifacts/e2e/20260907-021034`, `RESULT: OK (passed=22 failed=0)`,
  Visual Studio Professional 2026 18.10.12120.281, plus 128 of 128 unit tests, `validate tree` 24 of 24,
  `validate fixtures` 8 of 8, acceptance map OK, credential scan clean.

`LC2012`, `LC2013`, exit code 9 and `emptyOutputAllowed` are additive under `contractVersion` 1 -- no
existing field, diagnostic or exit code changed meaning -- which is what `docs/release.md` says an
increment is for. The `v1` fixtures were regenerated from the build before 1.1.0 was ever published, so
no published fixture changed.

**Still open:** publishing 1.1.0. Everything up to the push is proved (see
`contracts/releases/1.1.0.json`); the push itself has no credential in this environment, and rotating
the two exposed keys remains the account holder's action.
