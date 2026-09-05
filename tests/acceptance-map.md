# Acceptance map

Every requirement of every capability spec in
`openspec/changes/relaunch-lazy-crud-builder-platform/specs/` is listed here with the evidence that
covers it: a unit test in `Lazy.Crud.Builder.Platform.Tests`, a check reported by
`Lazy.Crud.Builder.Platform.Tool validate`, or a scenario of the E2E harness (`scripts/e2e.ps1`).

The three kinds of evidence are complementary, not interchangeable. A unit test pins behaviour that
leaves no observable trace in a generated solution (hash rules, migration edge cases, protection
decisions). A validation check pins facts about the repository and the produced packages. A harness
scenario proves the behaviour survives the real Visual Studio T4 toolchain end to end.

Naming:

- `test:<Class>.<Method>` in `src/Platform/Lazy.Crud.Builder.Platform.Tests/`.
- `check:<id>` as reported by `validate tree` or `validate packages`.
- `harness:<scenario>` as reported in `artifacts/e2e/<stamp>/summary.json`.
- `golden` for `tests/golden/Products`, compared by `scripts/golden.ps1` (harness R4).

`scripts/acceptance-map-check.ps1` fails when a name used here no longer exists.

## attribute-metadata-contract

| Requirement | Evidence |
| --- | --- |
| The Builder publishes the complete attribute metadata consumers need | `check:attributes.metadata-contract`, `check:fixtures.descriptor-validates`, `harness:C1` |
| Attribute types are consistent with their documentation | `check:attributes.metadata-contract`, `check:packages.attributes-xml-documentation`, `harness:C1` |
| Attribute contract changes are versioned | `check:fixtures.current`, `test:DescriptorResolverTests.ContractNegotiation`, `test:DescriptorResolverTests.AdditiveFieldsDoNotBreakTheReader` |

## builder-documentation

| Requirement | Evidence |
| --- | --- |
| Public documentation set | `check:docs.required-pages` |
| Documentation states verifiable facts | `check:docs.drift` |
| Quickstart is executable end to end | `harness:P7` runs the fast path of `docs/quickstart.md` verbatim |
| Guidance for AI coding agents | `check:docs.required-pages`, `check:docs.drift` |
| The frozen shared contracts are documented for consumers | `check:docs.required-pages`, `check:fixtures.present`, `check:fixtures.schemas-present` |
| The Visual Studio prerequisite is documented prominently | `check:docs.drift` |

## clean-room-validation

| Requirement | Evidence |
| --- | --- |
| Validation runs against the public artifact only | `harness:C1` |
| Fallback behavior is disabled during validation | `harness:C1` |
| The consumed artifact is inspected, not assumed | `harness:C1`, `check:packages.templates-package-layout` |
| A clean-room consumer builds and generates | `harness:C1` |
| The full shared contract resolves from the distribution alone | `harness:C1`, `check:packages.descriptor-embedded` |
| Clean room means no private source, not a bare machine | `harness:C1`, `harness:T1` |

## contract-fixtures

| Requirement | Evidence |
| --- | --- |
| Canonical contract fixtures are published | `check:fixtures.present`, `check:fixtures.current` |
| Hash vectors make the frozen hash rule independently reproducible | `check:fixtures.hash-vectors-reproducible`, `check:fixtures.committed-hash-vectors-reproducible` |
| Fixtures are consumable by another repository | `check:packages.descriptor-embedded`, `harness:C1` |
| The E2E harness asserts against the same fixtures | `harness:P4`, `harness:R1` |

## drift-protection

| Requirement | Evidence |
| --- | --- |
| Verify before overwriting a GeneratorOwned file | `test:GenerationWriterTests.VerifiedFileIsOverwrittenWithoutDrift`, `test:GenerationWriterTests.DriftedFileIsProtectedByDefaultAndTheEditSurvives`, `harness:F1` |
| Absent baseline never authorizes overwrite | `test:GenerationWriterTests.UnverifiedDifferentContentIsProtectedWithDistinctCode`, `test:GenerationWriterTests.MigratedV1EntryWithUnknownHashIsProtectedWhenContentDiffers`, `harness:F4` |
| Actionable diagnostics with stable codes | `test:ExitCodeTests.EveryDiagnosticCodeIsDescribed`, `harness:F1`, `harness:F4` |
| Explicit resolution flags | `test:GenerationWriterTests.ForceOverwritesDriftAndReportsIt`, `test:GenerationWriterTests.SkipModifiedPreservesTheFileAndSucceeds`, `harness:F2`, `harness:F3`, `harness:F5` |

## e2e-verification-harness

| Requirement | Evidence |
| --- | --- |
| Single-command orchestrator | the run directory `artifacts/e2e/<stamp>/` produced by `pwsh scripts/e2e.ps1` |
| Reliable exit codes from one shared table | `test:ExitCodeTests.TableCoversEveryRequiredOutcome`, the `exitCode` and `exitCodeName` fields of `summary.json` |
| Machine-readable and human-readable artifacts | `summary.json` and `digest.txt` of every run |
| Sandbox isolation | `harness:TREE` |
| Positive scenarios | `harness:P1`, `harness:P2`, `harness:P3`, `harness:P4`, `harness:P5`, `harness:P6`, `harness:P7` |
| Compatibility regression scenarios | `harness:R1`, `harness:R2`, `harness:R3`, `harness:R4`, `harness:R5` |
| Intentional failure scenario proves protection | `harness:F1`, `harness:F2`, `harness:F3` |
| Headless generation is mandatory and uses the Visual Studio T4 toolchain | `harness:T1`, and every positive scenario transforms through the embedded Visual Studio session |
| The harness uses the frozen shared semantics | `harness:P4` computes hashes with the shipped `ContentHasher` |
| Unverified-baseline scenarios | `harness:F4`, `harness:F5` |
| Toolchain prerequisite scenarios | `harness:T1`, `harness:T2` |

## generated-code-markers

| Requirement | Evidence |
| --- | --- |
| Frozen marker header v2 | `test:MarkerTests.GeneratorOwnedHeaderStatesEditsAreOverwrittenAndWhereToCustomize`, `test:MarkerTests.GeneratedOnceHeaderStatesUserOwnedAndLacksOverwrittenWording`, `check:fixtures.markers-parse`, `harness:P2` |
| The marker is excluded from the content hash | `test:ContentHasherTests.MarkerIsExcludedFromTheHash`, `test:ContentHasherTests.MarkerInBlockSyntaxIsExcludedToo`, `harness:P4` |
| Marker v1 remains recognized for detection | `test:MarkerTests.RecognizesHistoricalV1TestHeader`, `test:MarkerTests.ForeignAutoGeneratedBlockIsNotALazyCrudMarker` |
| Markers respect file syntax | `test:MarkerTests.RoundTripsThroughEverySyntax`, `test:MarkerTests.UnmarkableFileTypesHaveNoSyntax`, `harness:P2` |
| Existing test-template header is unified | `golden`: the generated test files carry the marker and no second header |

## generated-code-ownership

| Requirement | Evidence |
| --- | --- |
| Frozen three-value ownership model | `test:OwnershipAndRegistryTests.CanonicalNamesRoundTrip`, `test:OwnershipAndRegistryTests.SynonymsAreRejected` |
| Ownership is declared by the platform descriptor | `check:registry.ownership-declared`, `test:OwnershipAndRegistryTests.EveryWriterHasAnOutputPatternAndEveryEntryHasOwnership` |
| Backward-compatible ownership mapping | `test:OwnershipAndRegistryTests.LegacyBooleanMapsExactly`, `test:GenerationWriterTests.LegacyBooleanFalseMeansGeneratedOnceWhenNoOwnershipOrRegistryEntry`, `harness:R1` |
| Ownership is declared for every template output | `check:ownership.call-sites-explicit`, `check:ownership.call-sites-agree-with-registry`, `test:CallSiteScannerTests.ScansMultiLineAndPositionalCallSites` |

## generation-diagnostics

| Requirement | Evidence |
| --- | --- |
| No silent failures in the generation path | `check:diagnostics.no-silent-catch-in-writer`, `check:diagnostics.no-empty-catch-in-project-tt`, `test:HardeningTests.ErrorGeneratingOutputWrittenByTheHostFailsTheStep`, `test:HardeningTests.ErrorListEntriesAttributedToTemplatesFailAtAnyLevel` |
| Diagnostics are structured and actionable | `test:GenerationWriterTests.UnregisteredTemplateIsUserOwnedAndReported`, `test:GenerationWriterTests.UnwritableOutputDirectoryIsReported`, `test:GenerationWriterTests.LockedFileFailsLoudly`, `harness:F1`, `harness:F4` |
| Diagnostics are testable | the `GenerationWriterTests`, `PipelineTests` and `TransformerToleranceTests` suites run without Visual Studio |
| Template resolution failures are explicit | `test:DescriptorResolverTests.MissingAndMalformedAreErrors`, `harness:R2`, `harness:C1` |

## generation-manifest-v2

| Requirement | Evidence |
| --- | --- |
| Manifest schema v2 | `test:ManifestTests.V2RoundTripsAndNeverContainsAbsolutePaths`, `check:fixtures.manifest-v2-validates`, `harness:P3` |
| Frozen content hash semantics | `test:ContentHasherTests.CrlfAndLfBodiesHashIdentically`, `test:ContentHasherTests.OneCharacterBodyChangeAltersTheHash`, `test:ContentHasherTests.ByteOrderMarkDoesNotChangeTheHash`, `test:ContentHasherTests.UnmarkedFileHashesWholeBodyWithBodyLfScope`, `check:fixtures.hash-vectors-reproducible`, `harness:P4` |
| Version 1 manifests migrate without inventing data | `test:ManifestTests.V1MigrationPreservesEntriesAndNeverInventsData`, `test:ManifestTests.StoreLoadsV1AndPersistsV2OnSave`, `check:fixtures.manifest-v1-migrated-validates`, `harness:R1` |
| Manifest operations never fail silently | `test:ManifestTests.StoreDistinguishesAbsentFromUnreadable`, `test:ManifestTests.SaveFailureIsReportedNotSwallowed`, `test:GenerationWriterTests.ManifestSaveFailureFailsTheWrite` |
| Single manifest root per solution | `test:ManifestTests.ConflictingManifestRootsAreReported`, `test:ManifestTests.RelativePathsResolveAfterTheSolutionMoves` |
| No absolute paths are ever written | `test:ManifestTests.WriterRefusesAbsoluteOrBackslashPaths`, `test:FixtureAndSchemaTests.ManifestV2SchemaRejectsAbsolutePaths`, `check:manifest.no-absolute-paths`, `harness:P3`, `harness:R1` |

## package-distribution

| Requirement | Evidence |
| --- | --- |
| Single version source for all packages | `check:version.single-source`, `check:version.central-defined`, `check:packages.single-version` |
| Every runtime package is packed and published together | `check:packages.declared-produced`, `check:packages.produced-declared` |
| Templates package identity is resolvable by existing clients | `check:packages.alias-is-dependency-only`, `harness:R2` |
| Shared templates ship from exactly one package | `check:packages.shared-templates-in-canonical`, `check:packaging.shared-templates-single-package`, `check:packages.no-contentfiles-collision`, `harness:R5` |
| Compatibility matrix is published and tested | `check:docs.drift`, `harness:R3` |
| Publication requires no embedded credentials | `check:security.no-credential-literal`, `scripts/credential-scan.ps1 -SelfTest` |
| The descriptor declares the canonical template distribution strategy | `test:OwnershipAndRegistryTests.PackageMappingFollowsTheDistributionStrategy`, `check:packages.registry-resolves`, `check:packages.layer-content-paths` |
| Local template fallback is a diagnosed development mode | `harness:C1` refuses it; `golden` runs in it and reports `LC3002` |
| The compatibility matrix declares host prerequisites | `check:docs.drift`, `check:fixtures.descriptor-validates` |

## platform-descriptor

| Requirement | Evidence |
| --- | --- |
| Canonical platform descriptor | `test:FixtureAndSchemaTests.DescriptorValidatesAgainstPublishedSchemaAndRoundTrips`, `check:fixtures.descriptor-validates`, `check:fixtures.current` |
| Attribute catalog identity is published, prose is not duplicated | `check:attributes.metadata-contract`, `check:docs.drift` |
| Contract version negotiation | `test:DescriptorResolverTests.ContractNegotiation`, `test:DescriptorResolverTests.HighestPlatformVersionWinsWithDiagnostic`, `test:DescriptorResolverTests.AgreeingPackagesProduceNoWarning`, `harness:R3` |
| Surfaces resolve platform facts from the descriptor | `test:DescriptorResolverTests.SinglePackageResolves`, `harness:R3`, `harness:C1` |
| Canonical templates package identity | `check:registry.shipped-templates-registered`, `check:registry.registered-templates-shipped`, `test:OwnershipAndRegistryTests.RegistryIdsAreUniqueAndStable` |
| Host prerequisites are declared in the descriptor | `check:fixtures.descriptor-validates`, `harness:T1` |

## t4-toolchain-prerequisite

| Requirement | Evidence |
| --- | --- |
| Visual Studio is an official platform prerequisite | `harness:T1`, `check:docs.drift` |
| Headless means no human interaction, not the absence of Visual Studio | `harness:T1` and every positive scenario: an embedded `devenv.exe -Embedding` session with no window and no input |
| Deterministic discovery of the Visual Studio T4 toolchain | `test:VsInstanceLocatorTests.SelectionPrefersReleaseThenHighestVersionThenPath`, `test:VsInstanceLocatorTests.OverrideIsValidatedAndReported`, `harness:T1` |
| Unresolvable toolchain fails early with an actionable diagnostic | `test:VsInstanceLocatorTests.SimulatedUnavailabilityFailsWithoutDiscovery`, `test:VsInstanceLocatorTests.RejectionNamesTheMissingComponent`, `harness:T2` |
| The generation pipeline's existing gates are preserved | `test:PipelineTests.DomainRunsFirstInTwoPhasesThenOtherLayersThenTests`, `test:PipelineTests.DomainGateStopsEverythingWhenPhaseOneFails`, `test:PipelineTests.SameAggregateIsSerializedDifferentAggregatesRunConcurrently`, `test:PipelineTests.ThrottleIsHonoured`, `test:PipelineTests.RestoreIsRetriedOnceAndDoesNotAbortTheRun`, `test:PipelineTests.OutputWaitPollsUntilTheResidueAppears`, `test:PipelineTests.TransformationThatProducesNothingFails`, `harness:P1` |

## Left to a person

Nothing in the specs depends on manual judgement. Two items are external to this repository and are
recorded as prerequisites in section 0 of
`openspec/changes/relaunch-lazy-crud-builder-platform/tasks.md`:

- Rotating the exposed nuget.org publish keys. `check:security.no-credential-literal` proves the
  working tree is clean; it cannot prove the keys already in Git history were revoked.
- Nutri.Veda's dependency shape. `docs/compatibility.md` records it as unverified and no scenario
  claims otherwise.
