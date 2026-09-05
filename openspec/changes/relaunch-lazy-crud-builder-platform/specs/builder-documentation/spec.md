## ADDED Requirements

### Requirement: Public documentation set

The Builder SHALL publish documentation that presents Lazy CRUD as a product. The set SHALL cover, at minimum:

- README — what Lazy CRUD is, what it produces, and the first thing to do.
- Quickstart — empty project to generated, compiling aggregate.
- Architecture — the model as input, templates as compiler, packages as runtime, generated code as artifact.
- Aggregates, entities and attributes — the declarative model reference.
- Generated-code policy — ownership values, markers, what is safe to edit.
- Safe customization — how to change generated behavior without losing it on regeneration.
- Migrations — moving between platform versions.
- Testing — how to test generated code and the templates.
- Compatibility matrix.
- Troubleshooting.
- Changelog and release guidance.
- Using Lazy CRUD with AI coding agents.

#### Scenario: Every documented page exists

- **WHEN** the documentation check runs
- **THEN** every page in the required set exists and is non-empty
- **AND** the check fails naming any missing page

#### Scenario: README introduces the product

- **WHEN** a first-time reader opens the README
- **THEN** it states what Lazy CRUD is, what it generates and how to start
- **AND** it links to the quickstart and architecture pages

### Requirement: Documentation states verifiable facts

Documentation SHALL not contradict the shipped artifacts. Version, target framework, package names and paths SHALL match what the platform actually produces.

#### Scenario: Target framework is accurate

- **WHEN** documentation states a target framework
- **THEN** it matches the framework the packages actually target

#### Scenario: Package names are accurate

- **WHEN** documentation names a package
- **THEN** that package id is produced by the pack step or is documented as a deprecated alias

#### Scenario: Documented commands work

- **WHEN** a command shown in the quickstart is executed in a clean environment
- **THEN** it succeeds
- **AND** the documentation check exercises it

#### Scenario: Drift between docs and descriptor is detected

- **WHEN** the documentation check runs
- **THEN** version, layer and package facts in the documentation are compared against the platform descriptor
- **AND** the check fails on a mismatch

### Requirement: Quickstart is executable end to end

The quickstart SHALL take a reader from nothing to generated, compiling code without referencing the Builder repository sources.

#### Scenario: Quickstart uses published artifacts only

- **WHEN** the quickstart is followed
- **THEN** every step uses published packages
- **AND** no step requires cloning or building this repository

#### Scenario: Quickstart is verified automatically

- **WHEN** the E2E harness runs
- **THEN** it executes the quickstart path
- **AND** a failure in the quickstart fails the harness

### Requirement: Guidance for AI coding agents

The documentation SHALL include a section telling an AI coding agent how to work in a Lazy CRUD codebase without destroying generated or user-owned code.

#### Scenario: Agent guidance explains ownership and markers

- **WHEN** an agent reads the guidance
- **THEN** it learns the three ownership values, how to read the marker header, and which files it may edit
- **AND** it learns that editing a `GeneratorOwned` file is lost unless the template is changed

#### Scenario: Agent guidance explains the correct edit target

- **WHEN** a change is needed in generated behavior
- **THEN** the guidance directs the agent to the template, the model or the user-owned extension point rather than the generated file

#### Scenario: Agent guidance explains drift diagnostics

- **WHEN** an agent encounters a drift diagnostic
- **THEN** the guidance explains what it means and which resolution to choose
- **AND** it states that the default behavior preserves the modification

#### Scenario: Agent guidance names the machine-readable sources

- **WHEN** an agent needs platform facts
- **THEN** the guidance points to the platform descriptor and the generation manifest
- **AND** it states that these, not documentation prose, are authoritative

### Requirement: The frozen shared contracts are documented for consumers

The documentation SHALL describe every frozen shared contract precisely enough for an independent implementation, and SHALL point to the published fixtures as the authority.

#### Scenario: Each frozen contract has a reference page

- **WHEN** the documentation check runs
- **THEN** a reference page exists for the platform descriptor, the manifest v2 schema, the marker v2 format, the ownership model and the hash semantics
- **AND** the check fails naming any missing page

#### Scenario: Hash semantics are documented exactly

- **WHEN** the hash reference is read
- **THEN** it states the algorithm, the line-ending normalization, the exclusion of the marker block, and the meaning of `hashAlgorithm` and `hashScope`
- **AND** it links to the published hash vectors

#### Scenario: Documentation points at fixtures, not prose, as the authority

- **WHEN** a consumer implements a frozen contract
- **THEN** the documentation directs it to the published fixtures and schemas
- **AND** states that the fixtures win if prose and fixture disagree

#### Scenario: Contract version and compatibility are documented

- **WHEN** the contract reference is read
- **THEN** it states the current `contractVersion`, the supported range and what a version increment implies for consumers

### Requirement: The Visual Studio prerequisite is documented prominently

The documentation SHALL state that an installed Visual Studio with T4 text-templating tooling is a prerequisite for generation in every mode, including headless and CLI, and SHALL explain what "headless" does and does not mean.

#### Scenario: Prerequisite appears before the first generation step

- **WHEN** a reader follows the README or the quickstart
- **THEN** the Visual Studio prerequisite is stated before the first step that generates code
- **AND** the required workload or component for T4 tooling is named

#### Scenario: Headless is defined precisely

- **WHEN** the headless or CLI documentation is read
- **THEN** it states that headless means no human interaction and no IDE session to drive
- **AND** it states that Visual Studio must still be installed because its T4 tooling performs the transformation

#### Scenario: Troubleshooting covers an unresolved toolchain

- **WHEN** the troubleshooting guide is read
- **THEN** it covers the prerequisite-unavailable diagnostic, what discovery was attempted and how to install or repair the required component

#### Scenario: Agent guidance states the prerequisite

- **WHEN** an AI coding agent reads the agent guidance
- **THEN** it learns that generation requires the Visual Studio toolchain on the machine
- **AND** it learns that a failed prerequisite must be reported rather than worked around with a plain build
