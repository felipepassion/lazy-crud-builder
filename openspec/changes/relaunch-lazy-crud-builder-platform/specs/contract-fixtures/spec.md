## ADDED Requirements

### Requirement: Canonical contract fixtures are published

The Builder SHALL publish versioned, machine-readable fixtures for every frozen shared contract, so consuming repositories test against artifacts produced by the authority rather than reproducing the schema by hand. The fixture set SHALL cover:

- A valid `lazycrud.platform.json` for the current `contractVersion`, including layers, template registry with ownership, and the attribute catalog.
- A JSON Schema for the descriptor.
- A valid manifest v2 document.
- A JSON Schema for manifest v2.
- The v1 manifest input and its expected migrated v2 output, including an unresolvable entry and an unknown-hash entry.
- Marker header examples for `GeneratorOwned` and `GeneratedOnce`, in each supported comment syntax.
- Hash vectors: input bodies with expected `contentHash` values, covering CRLF/LF equivalence, marker exclusion, and an unmarked file.

#### Scenario: Fixtures exist for every frozen contract

- **WHEN** the fixture set is validated
- **THEN** a fixture is present for the descriptor, the manifest v2 document, the v1 migration pair, the markers and the hash vectors
- **AND** validation fails naming any missing fixture

#### Scenario: Fixtures are versioned by contract version

- **WHEN** a fixture is published
- **THEN** it declares the `contractVersion` it represents
- **AND** fixtures for a previous `contractVersion` remain retrievable

#### Scenario: Fixtures are produced by the implementation, not written by hand

- **WHEN** the descriptor and manifest fixtures are generated
- **THEN** they are produced by the same code path that writes the real artifacts
- **AND** a check fails if a fixture differs from what the implementation currently produces

#### Scenario: Fixtures validate against their own schemas

- **WHEN** the fixture check runs
- **THEN** each document fixture validates against its published JSON Schema

### Requirement: Hash vectors make the frozen hash rule independently reproducible

The hash fixtures SHALL let any consumer verify its own implementation of the frozen hash semantics without reading Builder source.

#### Scenario: Vector covers marker exclusion

- **WHEN** a consumer hashes the vector whose input carries a marker header
- **THEN** its computed value equals the expected `contentHash` recorded in the vector

#### Scenario: Vector covers line-ending normalization

- **WHEN** a consumer hashes the CRLF and LF variants of the same body
- **THEN** both equal the single expected value recorded in the vector

#### Scenario: Vector covers an unmarked file

- **WHEN** a consumer hashes the vector for a file type that carries no marker
- **THEN** its computed value equals the expected value for the whole normalized body

#### Scenario: The Builder verifies its own vectors

- **WHEN** the Builder's test suite runs
- **THEN** it recomputes every hash vector with the production implementation and asserts the recorded values

### Requirement: Fixtures are consumable by another repository

The fixtures SHALL be distributed so a separate repository can consume them in contract and parity tests without cloning the Builder.

#### Scenario: Fixtures ship in a retrievable artifact

- **WHEN** a consuming repository needs the fixtures
- **THEN** they are obtainable from a published artifact at a documented, stable location
- **AND** obtaining them does not require a Builder source checkout

#### Scenario: Consuming repositories are told what changed

- **WHEN** a fixture changes because a frozen contract changed
- **THEN** the change is recorded in the changelog against its `contractVersion`
- **AND** the compatibility matrix records which consumer versions accept it

#### Scenario: Fixture drift fails the Builder build

- **WHEN** a frozen contract changes without its fixture and schema being regenerated
- **THEN** the fixture check fails naming the stale fixture

### Requirement: The E2E harness asserts against the same fixtures

The end-to-end battery SHALL use the published fixtures and hash vectors rather than its own copies, so the harness cannot pass against semantics that differ from what consumers receive.

#### Scenario: Harness validates real output against the published schemas

- **WHEN** the harness validates a generated manifest or descriptor
- **THEN** it validates against the published JSON Schema fixture

#### Scenario: Harness uses the published hash vectors

- **WHEN** the harness verifies hash computation
- **THEN** it uses the published vectors
- **AND** it does not define its own expected values

#### Scenario: Harness uses the published v1 migration pair

- **WHEN** the harness runs the v1 migration scenario
- **THEN** it seeds the published v1 fixture and asserts the published expected v2 output
