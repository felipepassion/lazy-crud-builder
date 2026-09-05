## ADDED Requirements

### Requirement: Single-command orchestrator

The platform SHALL provide one entry-point command that runs the complete end-to-end battery without manual interaction and without a developer clicking anything in Visual Studio. It runs on a machine meeting the platform prerequisite — Visual Studio installed with its T4 tooling — and may invoke that tooling.

#### Scenario: One command runs everything

- **WHEN** the orchestrator is invoked with no arguments
- **THEN** it runs every required scenario in order
- **AND** it requires no input during the run

#### Scenario: Runs headless

- **WHEN** the orchestrator is invoked by an agent or a CI job with no interactive console
- **THEN** it completes without prompting
- **AND** it does not require Visual Studio to be open, focused, or driven by a person

#### Scenario: Missing prerequisites fail explicitly

- **WHEN** a required tool such as the Visual Studio T4 toolchain or the .NET SDK is unavailable
- **THEN** the orchestrator fails with a prerequisite-specific exit code and message
- **AND** it does not skip generation and report success

#### Scenario: Selective execution is possible

- **WHEN** the orchestrator is invoked for a single named scenario
- **THEN** only that scenario runs
- **AND** the same reporting and exit-code rules apply

### Requirement: Reliable exit codes from one shared table

The orchestrator SHALL communicate its outcome through one documented exit-code table, shared with the consuming surfaces so a code means the same thing everywhere. The table SHALL distinguish at least: success, assertion failure, prerequisite or environment unavailable, invalid fixture or sandbox, timeout, contract or version mismatch, and ownership conflict.

#### Scenario: Success

- **WHEN** every required scenario passes
- **THEN** the orchestrator exits with the success code

#### Scenario: Assertion failure is distinguishable from a broken environment

- **WHEN** a scenario fails because an assertion did not hold
- **THEN** the exit code is the assertion-failure code
- **AND** it differs from the prerequisite-unavailable code

#### Scenario: Prerequisite unavailable has its own code

- **WHEN** a required tool such as the Visual Studio T4 toolchain or the .NET SDK is unavailable
- **THEN** the orchestrator exits with the prerequisite-unavailable code
- **AND** it does not skip generation and report success

#### Scenario: Invalid fixture or sandbox has its own code

- **WHEN** a fixture cannot be loaded or the sandbox cannot be created
- **THEN** the orchestrator exits with the invalid-fixture-or-sandbox code

#### Scenario: Timeout has its own code

- **WHEN** a scenario or the run exceeds its timeout budget
- **THEN** the orchestrator exits with the timeout code

#### Scenario: Contract mismatch has its own code

- **WHEN** a resolved descriptor's `contractVersion` is outside the supported range
- **THEN** the orchestrator exits with the contract-mismatch code

#### Scenario: Ownership conflict has its own code

- **WHEN** a run fails because files are drifted or unverified
- **THEN** the orchestrator exits with the ownership-conflict code
- **AND** it differs from the generic assertion-failure code

#### Scenario: Codes are documented

- **WHEN** the harness documentation is read
- **THEN** every exit code is listed with its meaning
- **AND** the table matches the one the consuming surfaces use

#### Scenario: Expected failure does not fail the run

- **WHEN** the intentional-failure scenario fails as designed
- **THEN** it is recorded as a pass
- **AND** it does not cause a non-zero overall exit code

### Requirement: Machine-readable and human-readable artifacts

Each run SHALL leave evidence sufficient for an agent or CI to confirm success without re-running anything.

#### Scenario: Run directory is created

- **WHEN** the orchestrator runs
- **THEN** it writes artifacts to a per-run directory identified by timestamp
- **AND** previous runs are not overwritten

#### Scenario: Structured summary is the source of truth

- **WHEN** a run completes
- **THEN** a machine-readable summary records each scenario's id, status, duration and failure reason
- **AND** it records the platform version and package versions exercised

#### Scenario: Human-readable digest is written

- **WHEN** a run completes
- **THEN** a readable digest of the run is written alongside the structured summary

#### Scenario: Single-line verdict

- **WHEN** a run completes
- **THEN** the final output line states the overall verdict and the passed and failed counts
- **AND** the line is greppable by an agent reading truncated output

#### Scenario: Failure evidence is retained

- **WHEN** a scenario fails
- **THEN** the logs, generated output and diagnostics for that scenario are retained in the run directory

### Requirement: Sandbox isolation

Each run SHALL build its own temporary sandbox and SHALL NOT modify the Builder repository or the developer's machine state.

#### Scenario: Sandbox is created per run

- **WHEN** the orchestrator starts
- **THEN** it creates a temporary sandbox directory for the run
- **AND** all generated projects and output live inside it

#### Scenario: Repository is not modified

- **WHEN** a run completes, whether passing or failing
- **THEN** the Builder repository working tree is unchanged apart from the run's artifacts directory

#### Scenario: Sandbox uses public-equivalent artifacts only

- **WHEN** the sandbox restores packages
- **THEN** it consumes only the published packages or the local pack output standing in for them
- **AND** it does not reference Builder project sources

#### Scenario: Run is repeatable

- **WHEN** the orchestrator is run twice in a row
- **THEN** both runs produce the same scenario outcomes
- **AND** the second run is not affected by the first run's state

### Requirement: Positive scenarios

The battery SHALL prove the full happy path from a sample aggregate to compiling, tested code with valid metadata.

#### Scenario: Aggregate is created and layers are generated

- **WHEN** the harness creates a sample aggregate in the sandbox and runs generation
- **THEN** files are produced for every layer the descriptor declares

#### Scenario: Markers are valid on every generated file

- **WHEN** generation completes
- **THEN** every generated file carries a valid marker header
- **AND** `GeneratedOnce` files carry the distinct user-owned marker

#### Scenario: Manifest is valid

- **WHEN** generation completes
- **THEN** the manifest is schema version 2, contains no absolute paths, and records ownership and a non-empty content hash for every entry

#### Scenario: Recorded hashes match the files on disk

- **WHEN** the harness recomputes hashes for the generated files
- **THEN** each matches the value recorded in the manifest

#### Scenario: Generated code compiles and tests pass

- **WHEN** the sandbox solution is built and its tests are run
- **THEN** the build succeeds
- **AND** the generated tests pass

#### Scenario: Regeneration without changes is stable

- **WHEN** generation is run a second time with no model change
- **THEN** it succeeds with no drift reported
- **AND** generated file contents are unchanged

### Requirement: Compatibility regression scenarios

The battery SHALL prove that existing consumers are not broken by the relaunch.

#### Scenario: Version 1 manifest is accepted

- **WHEN** the sandbox is seeded with the published v1 manifest fixture, including absolute paths and empty hashes, and its generated files are unmodified, and generation runs
- **THEN** the run succeeds without a resolution flag
- **AND** the manifest is upgraded to v2 matching the published expected output
- **AND** no drift and no unverified failure is reported

#### Scenario: Deprecated templates package id resolves

- **WHEN** the sandbox is configured with the historical templates package id
- **THEN** templates resolve successfully
- **AND** a deprecation notice is reported

#### Scenario: Mixed package versions are supported

- **WHEN** the sandbox pins a supported mixed-version combination across Lazy CRUD packages
- **THEN** restore, build and generation succeed

#### Scenario: Existing boolean call sites behave unchanged

- **WHEN** generation runs for the reference aggregate
- **THEN** output is byte-identical to the recorded golden output apart from marker headers

#### Scenario: Multi-layer reference has no content collision

- **WHEN** the sandbox project references two or more layer packages
- **THEN** restore and build succeed with no duplicate template content

### Requirement: Intentional failure scenario proves protection

The battery SHALL include at least one scenario that must fail, proving drift protection and diagnostics actually work.

#### Scenario: External modification is detected and blocked

- **WHEN** the harness modifies a `GeneratorOwned` generated file outside the generator and runs generation again with no resolution flag
- **THEN** the run fails
- **AND** the modification is still present in the file afterwards

#### Scenario: The specific drift diagnostic is asserted

- **WHEN** the drift scenario runs
- **THEN** the harness asserts the drift diagnostic code and the affected file path
- **AND** the scenario fails if the run failed for any other reason

#### Scenario: Force flag resolves the drift deliberately

- **WHEN** generation is re-run with the force flag after the drift is detected
- **THEN** the file is regenerated
- **AND** the overwrite is reported in the run output

#### Scenario: Skip flag preserves the modification

- **WHEN** generation is re-run with the skip-modified flag
- **THEN** the modified file is unchanged
- **AND** the remaining files are generated
- **AND** the run reports success listing the skipped file

### Requirement: Headless generation is mandatory and uses the Visual Studio T4 toolchain

The public harness SHALL perform real template transformation, unattended, using the T4 tooling supplied by the resolved Visual Studio installation. The harness runs on a machine meeting the platform prerequisite. Declaring headless generation unsupported, skipping it, or substituting a step that does not actually transform templates SHALL NOT be permitted.

#### Scenario: Transformation actually happens

- **WHEN** a harness scenario generates an aggregate
- **THEN** the templates are transformed by the resolved Visual Studio T4 tooling
- **AND** the produced files are asserted to exist with valid markers and manifest entries

#### Scenario: Transformation is proven, not inferred from a build

- **WHEN** a generation scenario completes
- **THEN** the harness asserts the transformed outputs exist, carry marker v2, appear in the manifest with matching hashes, and that the solution builds and its generated tests pass
- **AND** a successful project build alone does not satisfy the scenario

#### Scenario: A no-op transformation is a failure

- **WHEN** a transformation step completes without producing the outputs the scenario declared
- **THEN** the scenario fails
- **AND** the run does not report success on the basis of a zero exit code alone

#### Scenario: The run is unattended

- **WHEN** the harness runs
- **THEN** no person opens, focuses or clicks anything in Visual Studio
- **AND** the run completes without an interactive IDE session

#### Scenario: Resolved toolchain is recorded as evidence

- **WHEN** a run completes
- **THEN** the structured summary records the resolved Visual Studio instance, its version and channel, and the T4 tooling path used

#### Scenario: MSBuild wiring is exercised

- **WHEN** a sandbox project consumes the published templates package
- **THEN** the package's MSBuild integration makes transformation reachable from a build-driven invocation on a machine meeting the prerequisite
- **AND** a scenario asserts that the outputs were produced

### Requirement: The harness uses the frozen shared semantics

The harness SHALL compute hashes, read ownership, parse markers and validate documents using exactly the frozen contracts and the published fixtures, so it cannot certify semantics that differ from what consumers receive.

#### Scenario: Hashing matches the published vectors

- **WHEN** the harness verifies content hashes
- **THEN** it uses the published hash vectors and the frozen SHA-256 LF-normalized body-excluding-marker rule
- **AND** it defines no expected values of its own

#### Scenario: Documents validate against published schemas

- **WHEN** the harness validates a produced manifest or descriptor
- **THEN** it validates against the published JSON Schema fixtures

#### Scenario: Ownership vocabulary is the frozen one

- **WHEN** the harness asserts ownership
- **THEN** it uses `GeneratorOwned`, `GeneratedOnce` and `Manual` exactly
- **AND** an unexpected ownership value fails the scenario

### Requirement: Unverified-baseline scenarios

The battery SHALL prove that a file with no trustworthy baseline is never silently overwritten, and that the safe-adoption path works.

#### Scenario: Unverified but identical content is adopted without intervention

- **WHEN** a v1-migrated aggregate whose generated files are unmodified is regenerated
- **THEN** the run succeeds without requiring a resolution flag
- **AND** the manifest afterwards records real hashes for those files

#### Scenario: Unverified and differing content is protected

- **WHEN** a v1-migrated aggregate has a `GeneratorOwned` file whose content differs from what the generator now produces and whose hash is unknown
- **THEN** the run fails with the unverified diagnostic naming that file
- **AND** the file is unchanged on disk afterwards

#### Scenario: Unverified diagnostic is distinct from drift

- **WHEN** the unverified scenario runs
- **THEN** the harness asserts the unverified diagnostic code
- **AND** the scenario fails if the drift code is reported instead

### Requirement: Toolchain prerequisite scenarios

The battery SHALL prove both that generation works when the Visual Studio T4 toolchain is present, and that its absence fails early with the correct diagnostic rather than silently degrading.

#### Scenario: Positive — toolchain detected and generation completes

- **WHEN** the harness runs on a machine where the Visual Studio T4 tooling resolves
- **THEN** the resolved instance, version, channel and tooling path are reported
- **AND** the aggregate generates, markers and manifest validate, hashes match, the solution builds and the generated tests pass

#### Scenario: Negative — unresolvable toolchain fails early

- **WHEN** the harness runs a scenario in which the Visual Studio T4 toolchain is made unresolvable
- **THEN** the run fails before transforming any template
- **AND** the exit code is the prerequisite-unavailable code
- **AND** the diagnostic names the requirement, what was searched and what must be installed

#### Scenario: Negative scenario does not fall back to a plain build

- **WHEN** the unresolvable-toolchain scenario runs
- **THEN** the harness asserts that no generated output was produced
- **AND** it asserts that the run did not report success

#### Scenario: Negative scenario is recorded as a pass

- **WHEN** the unresolvable-toolchain scenario fails as designed
- **THEN** it is recorded as a passing scenario
- **AND** it does not make the overall exit code non-zero

#### Scenario: Prerequisite failure is distinguishable from an assertion failure

- **WHEN** the unresolvable-toolchain scenario reports its exit code
- **THEN** the code differs from the assertion-failure code
