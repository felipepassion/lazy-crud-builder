## ADDED Requirements

### Requirement: Verify before overwriting a GeneratorOwned file

Before overwriting a `GeneratorOwned` file, the generator SHALL establish whether the file on disk still matches what the generator last produced, using the frozen hash semantics — SHA-256 over the LF-normalized body excluding the marker block. A file SHALL be resolved into exactly one state:

- `verified` — the file's current hash equals the recorded hash. Safe to overwrite.
- `drifted` — the file's current hash differs from the recorded hash. It was modified outside the generator.
- `unverified` — the file exists but no trustworthy baseline hash is recorded, for example after v1 migration or when the marker is v1-only. Its state cannot be proven.
- `missing` — recorded but absent from disk.

The drift check SHALL be performed for the whole aggregate before the first file is written.

#### Scenario: Verified file is regenerated normally

- **WHEN** a `GeneratorOwned` file's current hash equals its recorded hash
- **THEN** the file is overwritten with the newly generated content
- **AND** no drift diagnostic is emitted

#### Scenario: Externally modified file is detected

- **WHEN** a `GeneratorOwned` file's current hash differs from its recorded hash
- **THEN** the file is classified `drifted`
- **AND** it is not overwritten by default

#### Scenario: Missing file is regenerated without drift

- **WHEN** a `GeneratorOwned` file recorded in the manifest no longer exists on disk
- **THEN** it is classified `missing` and regenerated
- **AND** it is not reported as drift

#### Scenario: New file is written without drift

- **WHEN** a generated path has no manifest entry and no file on disk
- **THEN** the file is written
- **AND** its hash is recorded for future runs

#### Scenario: Check precedes writes for the whole aggregate

- **WHEN** a regeneration would write several `GeneratorOwned` files and one of them is drifted
- **THEN** the states of the whole set are resolved before the first file is written

### Requirement: Absent baseline never authorizes overwrite

A file with no trustworthy recorded hash SHALL NOT be overwritten merely because its state is unknown. The generator SHALL first attempt to prove the file is unchanged by comparing the newly generated body against the on-disk body using the frozen hash semantics.

#### Scenario: Unverified file whose body already matches is adopted safely

- **WHEN** a `GeneratorOwned` file is `unverified` and the newly generated body hashes identically to the on-disk body
- **THEN** the content is already correct and no destructive write is required
- **AND** the hash is recorded as the baseline
- **AND** the run continues without requiring user intervention

#### Scenario: Unverified file whose body differs is protected

- **WHEN** a `GeneratorOwned` file is `unverified` and the newly generated body differs from the on-disk body
- **THEN** the file is not overwritten by default
- **AND** a diagnostic reports it as unverified, stating that no baseline exists and the difference cannot be attributed
- **AND** the run reports failure unless a resolution flag is supplied

#### Scenario: Migrated v1 entry is not silently overwritten

- **WHEN** an aggregate migrated from a v1 manifest is regenerated and an entry has an unknown hash and differing content
- **THEN** the file is protected exactly as any other `unverified` file
- **AND** the migration warning and the unverified diagnostic both name the file

#### Scenario: Unknown ownership is treated as user-owned

- **WHEN** a manifest entry has unknown ownership and the file exists on disk
- **THEN** it is treated as user-owned and not overwritten
- **AND** a diagnostic names the file and the missing ownership

### Requirement: Actionable diagnostics with stable codes

Every non-`verified` outcome SHALL produce a diagnostic that names the file and tells the caller how to proceed. Codes SHALL be stable and documented so tests and consuming surfaces can assert on them rather than on message text.

#### Scenario: Drift diagnostic identifies the file and resolutions

- **WHEN** drift is reported for a file
- **THEN** the diagnostic names the relative path, the expected hash, the actual hash and the `templateId`
- **AND** it names the flags that force overwrite or skip the file

#### Scenario: Unverified diagnostic is distinct from drift

- **WHEN** a file is reported as unverified
- **THEN** its diagnostic code differs from the drift code
- **AND** the message explains that no baseline exists rather than asserting the file was edited

#### Scenario: Diagnostics carry stable codes

- **WHEN** any drift or unverified diagnostic is emitted
- **THEN** it carries a stable, documented code
- **AND** tooling and tests assert on the code

#### Scenario: All affected files are reported

- **WHEN** more than one file is drifted or unverified in a run
- **THEN** every affected file is reported
- **AND** the run does not stop at the first one

### Requirement: Explicit resolution flags

The caller SHALL be able to resolve drift and unverified states explicitly, and the default SHALL never destroy the content.

#### Scenario: Default blocks the overwrite

- **WHEN** drifted or unverified files are detected and no resolution flag is supplied
- **THEN** those files are left untouched on disk
- **AND** the generation run reports failure

#### Scenario: Force overwrites deliberately

- **WHEN** the force flag is supplied
- **THEN** the affected files are overwritten with generated content
- **AND** each overwrite is reported in the run output

#### Scenario: Skip preserves and continues

- **WHEN** the skip-modified flag is supplied
- **THEN** the affected files are left untouched
- **AND** all other files are still generated
- **AND** the run reports success with the skipped files listed

#### Scenario: Human or AI edit is never lost silently

- **WHEN** a generated file is edited externally and generation runs again with no flags
- **THEN** the external edit is still present on disk afterwards
- **AND** the run output states that the file was protected

#### Scenario: Flags never reach Manual files

- **WHEN** the force flag is supplied
- **THEN** files classified `Manual` are still not written or deleted

### Requirement: A contentless output never replaces a file that has content

Drift protection answers whether a human changed a file, and a `verified` file is deliberately overwritable. It therefore cannot see a transformation that resolved no model and emitted only its template's static preamble: the body is new, well formed and says nothing. Ahead of the state resolution, and for every state including `verified`, the generator SHALL refuse to replace a file whose body carries content with a regenerated body that carries none.

A body carries content when at least one of its lines is significant. A line SHALL be significant unless it is blank, a whole-line comment in the file's comment syntax, or an import or preprocessor directive. The rule SHALL be conservative: a line it cannot classify counts as significant. The rule SHALL NOT be a threshold — a body that shrinks but still declares something is written normally.

The refusal SHALL be reported with a diagnostic code distinct from drift and from unverified, SHALL identify the file, template, layer and aggregate, both hashes and the significant-line count on each side, and SHALL map to its own exit code, because the run produced a defective artifact rather than meeting a conflict with the developer. No resolution flag SHALL override it. A template whose correct output carries no significant content SHALL declare that in the template registry and SHALL be exempt.

#### Scenario: Verified file survives a contentless regeneration

- **WHEN** a `GeneratorOwned` file is `verified` and the regenerated body carries no significant line
- **THEN** the file is not overwritten
- **AND** the diagnostic names the file, the `templateId`, the layer, the aggregate, both hashes and the significant-line count of each side
- **AND** the run reports the exit code reserved for a defective generation, not the ownership-conflict code

#### Scenario: Unverified file survives a contentless regeneration

- **WHEN** a `GeneratorOwned` file is `unverified` and the regenerated body carries no significant line
- **THEN** the file is not overwritten
- **AND** the collapse is reported rather than the unverified difference

#### Scenario: Force does not override the refusal

- **WHEN** the force flag is supplied and the regenerated body carries no significant line
- **THEN** the file is still not overwritten
- **AND** no forced-overwrite diagnostic is emitted for it

#### Scenario: A declared contentless output regenerates normally

- **WHEN** a template declares that its correct output carries no significant content and it regenerates such a body
- **THEN** the file is written
- **AND** no collapse diagnostic is emitted

#### Scenario: A new file is never a collapse

- **WHEN** no file exists at the output path and the regenerated body carries no significant line
- **THEN** the file is written, because no content is lost

#### Scenario: A collapse is retried before it is believed

- **WHEN** a transformation produces a contentless body and the write is refused
- **THEN** the generator re-transforms that template within its retry budget
- **AND** a retry that produces content records a diagnostic distinct from the refusal, leaves the file correct and does not fail the run
- **AND** a collapse that survives every attempt keeps the refusal diagnostic and its exit code

#### Scenario: A collapse in one layer does not retry another

- **WHEN** one layer's transformation collapses while another layer is transforming a template of the same file name
- **THEN** only the layer whose output collapsed is retried

#### Scenario: A smaller body that still declares something is written

- **WHEN** the regenerated body has fewer significant lines than the file on disk but at least one
- **THEN** the file is overwritten as an ordinary regeneration
