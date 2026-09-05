## ADDED Requirements

### Requirement: Visual Studio is an official platform prerequisite

Lazy CRUD for the Windows/Visual Studio platform SHALL declare an installed Visual Studio as an **official, verifiable prerequisite** for T4 generation in every mode, including headless and CLI. The generator SHALL rely on the T4 tooling provided by that installation, exactly as the product has done for years.

An installed Visual Studio SHALL NOT be treated as an accidental fallback, an optional accelerator, or a development-only convenience. It is a declared requirement of the platform, in the same way a toolchain installed on the system is a requirement for other IDE-adjacent tooling.

#### Scenario: The prerequisite is declared, not implied

- **WHEN** the platform's requirements are published
- **THEN** an installed Visual Studio with T4 text-templating tooling is stated as a prerequisite for generation
- **AND** it is stated for the headless and CLI paths, not only for the IDE path

#### Scenario: Generation relies on the installed tooling

- **WHEN** T4 transformation runs in any mode
- **THEN** it is performed by the T4 tooling supplied by the resolved Visual Studio installation

#### Scenario: The prerequisite is verifiable before work begins

- **WHEN** a generation or harness run starts
- **THEN** the prerequisite is checked before any template is transformed
- **AND** the run does not begin transforming with an unresolved toolchain

### Requirement: Headless means no human interaction, not the absence of Visual Studio

The platform SHALL define "headless" as running without human interaction and without requiring a visible window or an IDE session a user must drive. Headless SHALL NOT be defined as running without Visual Studio installed.

#### Scenario: Headless runs without human interaction

- **WHEN** a headless generation or harness run executes
- **THEN** it completes without prompting, without a user clicking anything in Visual Studio, and without requiring an interactive IDE session
- **AND** it may invoke tooling provided by the Visual Studio installation

#### Scenario: Headless is not advertised as Visual Studio independence

- **WHEN** the headless mode is documented
- **THEN** it states that Visual Studio must be installed
- **AND** it states that no human interaction with Visual Studio is required

#### Scenario: An agent can run generation unattended

- **WHEN** an AI agent or a CI job invokes generation on a machine meeting the prerequisite
- **THEN** generation completes unattended
- **AND** no window has to be opened or focused by a person

### Requirement: Deterministic discovery of the Visual Studio T4 toolchain

The toolchain SHALL be located through a supported, deterministic discovery mechanism — the Visual Studio Installer's query tool (`vswhere`) or the equivalent Setup Configuration API — and SHALL NOT be located by fragile hardcoded paths or by recursively scanning program-files directories.

Discovery SHALL account for side-by-side installations, and SHALL include prerelease and Insiders channels, because an otherwise valid machine may have only a prerelease instance installed.

#### Scenario: Toolchain is resolved through the supported mechanism

- **WHEN** the toolchain is resolved
- **THEN** the resolution uses the Visual Studio Installer query tool or the Setup Configuration API
- **AND** it does not depend on a hardcoded installation path

#### Scenario: Prerelease and Insiders instances are discoverable

- **WHEN** the only installed instance is a prerelease or Insiders channel instance
- **THEN** discovery still resolves it
- **AND** the run proceeds

#### Scenario: Side-by-side installations resolve deterministically

- **WHEN** more than one Visual Studio instance is installed
- **THEN** the selection rule is documented and applied deterministically
- **AND** the same machine resolves the same instance on repeated runs

#### Scenario: No recursive filesystem scanning

- **WHEN** discovery runs
- **THEN** it does not enumerate program-files directories recursively to find the tooling

#### Scenario: The resolved toolchain is reported

- **WHEN** any generation or harness run completes
- **THEN** the report names the resolved instance, its version and channel, and the resolved T4 tooling path
- **AND** the value is recorded in the run's machine-readable output

#### Scenario: An explicit override is honored and reported

- **WHEN** an operator supplies an explicit instance or tooling path
- **THEN** that value is used instead of discovery
- **AND** the report states that an override was applied

### Requirement: Unresolvable toolchain fails early with an actionable diagnostic

When the required Visual Studio installation or its T4 tooling cannot be resolved, the CLI and the harness SHALL fail before attempting generation, with a stable diagnostic code, an actionable message, and a non-zero exit code. They SHALL NOT substitute a mechanism that does not transform templates, and SHALL NOT report success.

#### Scenario: Missing installation fails before generation

- **WHEN** no Visual Studio instance satisfying the prerequisite can be resolved
- **THEN** the run fails before transforming any template
- **AND** the exit code is the prerequisite-unavailable code

#### Scenario: Missing T4 tooling fails before generation

- **WHEN** an instance is resolved but its T4 text-templating tooling cannot be located or executed
- **THEN** the run fails with a diagnostic naming the instance, the expected tooling and the discovery performed
- **AND** the exit code is the prerequisite-unavailable code

#### Scenario: The diagnostic tells the operator what to do

- **WHEN** the prerequisite diagnostic is emitted
- **THEN** it names the requirement, what was searched, what was found, and the workload or component that must be installed

#### Scenario: A plain build is never a substitute

- **WHEN** the toolchain cannot be resolved
- **THEN** the run does not fall back to building the project as though that were transformation
- **AND** it does not report generated output it did not produce

#### Scenario: Transformation that produces nothing is a failure

- **WHEN** a transformation step completes without producing the outputs expected for the templates it was given
- **THEN** the run fails with a diagnostic naming the templates and the missing outputs
- **AND** a zero exit code from the underlying tool does not make the step succeed

### Requirement: The generation pipeline's existing gates are preserved

Adopting the declared toolchain SHALL preserve the accumulated behavior of the current pipeline, which exists because of real race conditions, and SHALL use the transformation path that most faithfully reproduces a real Visual Studio transformation.

The preserved behavior includes: Domain-first ordering with the documented layer sequence; the Domain layer's internal two-phase execution; gating subsequent layers on Domain success; parallel execution of non-Domain layers under the configured concurrency throttle; per-aggregate serialization; package restore ordering; and the existing wait and retry behavior for produced output.

#### Scenario: Domain-first ordering is preserved

- **WHEN** an aggregate is generated
- **THEN** the Domain layer is transformed first and subsequent layers follow the documented order

#### Scenario: Domain gate is preserved

- **WHEN** Domain transformation fails
- **THEN** dependent layers are not transformed
- **AND** the failure is reported

#### Scenario: Concurrency throttle is preserved

- **WHEN** non-Domain layers run in parallel
- **THEN** concurrency respects the configured maximum
- **AND** the configured value continues to be honored from settings

#### Scenario: Per-aggregate serialization is preserved

- **WHEN** two operations target the same aggregate concurrently
- **THEN** they are serialized
- **AND** operations on different aggregates are still allowed to proceed concurrently

#### Scenario: Restore and output-wait behavior is preserved

- **WHEN** generation requires package restore or waits for produced output
- **THEN** the existing ordering, wait and retry behavior is retained
- **AND** regression tests cover it rather than simplifying it away

#### Scenario: Transformation fidelity is preserved

- **WHEN** the transformation path is chosen
- **THEN** it is the one that most faithfully reproduces a real Visual Studio transformation for the shipped templates
- **AND** the golden-output regression test confirms the produced files are unchanged apart from the marker block
