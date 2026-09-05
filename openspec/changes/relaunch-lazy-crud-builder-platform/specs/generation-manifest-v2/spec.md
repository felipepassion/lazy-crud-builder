## ADDED Requirements

### Requirement: Manifest schema v2

The generation manifest SHALL be written at `{solutionRoot}/.lazycrud/manifests/{AggregateName}.manifest.json` with `schemaVersion` `2`. It is a frozen shared contract: consuming surfaces read and write it, so field names and semantics SHALL NOT vary between the Builder and its consumers.

The manifest SHALL record, at document level:

- `schemaVersion` — `2`.
- `aggregateName`.
- `generatedAtUtc`.
- `generatorVersion` — the real platform version from the descriptor, never a hardcoded literal.
- `contractVersion` — the `contractVersion` of the platform descriptor used for the run.

And, per file in `files[]`:

- `relativePath` — relative to the solution root, using forward slashes.
- `layer`.
- `templateId` — the stable template id from the descriptor's template registry.
- `templateVersion`.
- `ownership` — `GeneratorOwned`, `GeneratedOnce` or `Manual`.
- `contentHash`.
- `hashAlgorithm`.
- `hashScope`.

`solutionRootMarker`, recording how relative paths are anchored, MAY additionally be present.

#### Scenario: Manifest records relative paths with forward slashes

- **WHEN** a manifest is written
- **THEN** every `relativePath` is relative to the solution root and uses forward slashes
- **AND** no absolute path, drive letter or backslash-separated path appears anywhere in the manifest

#### Scenario: Manifest is portable across machines

- **WHEN** a manifest generated on one machine is read on another machine with the solution at a different absolute path
- **THEN** every recorded file resolves correctly
- **AND** no entry is reported as missing solely because of the path difference

#### Scenario: Every entry carries template identity and ownership

- **WHEN** a manifest is written
- **THEN** every entry has a non-empty `templateId`, `layer`, `ownership` and `contentHash`
- **AND** `templateId` matches an entry in the descriptor's template registry

#### Scenario: Manifest records the contract version used

- **WHEN** an aggregate is generated against a resolved descriptor
- **THEN** the manifest records that descriptor's `contractVersion`

#### Scenario: Generator version is real

- **WHEN** a manifest is written
- **THEN** `generatorVersion` matches the `platformVersion` from the descriptor
- **AND** it is not a hardcoded placeholder

### Requirement: Frozen content hash semantics

`contentHash` SHALL be computed as **SHA-256 over the file body with line endings normalized to LF, excluding the Lazy CRUD marker/header block**. The manifest SHALL record `hashAlgorithm` as `SHA-256` and `hashScope` as the identifier for this rule. Generation, drift detection, the E2E harness and every consuming tool SHALL use this identical computation. The marker is validated separately and never contributes to the hash.

#### Scenario: Hash excludes the marker block

- **WHEN** two files have identical bodies but marker headers differing in generator version or timestamp
- **THEN** both produce the same `contentHash`

#### Scenario: Marker format change does not register as drift

- **WHEN** only the marker header format changes between two platform versions
- **THEN** no file is reported as drifted on the next generation

#### Scenario: CRLF and LF checkouts hash identically

- **WHEN** the same generated body is written once with CRLF and once with LF line endings
- **THEN** both produce the same `contentHash`

#### Scenario: Real content change alters the hash

- **WHEN** a single non-whitespace character in a generated file's body is changed
- **THEN** the recomputed hash differs from the manifest hash

#### Scenario: Hash rule is self-describing

- **WHEN** a manifest entry is read
- **THEN** `hashAlgorithm` and `hashScope` state exactly how the hash was computed
- **AND** a consumer can reproduce it without consulting the Builder source

#### Scenario: Files without a marker hash their whole body

- **WHEN** a file type carries no marker because no safe comment syntax exists for it
- **THEN** the hash covers the entire LF-normalized content
- **AND** `hashScope` records that no marker was excluded

### Requirement: Version 1 manifests migrate without inventing data

A reader SHALL accept a v1 manifest — one with no `schemaVersion`, or `schemaVersion` `1` — and migrate it. Migration SHALL preserve every entry it can resolve and SHALL NOT fabricate values it cannot derive. Absent metadata SHALL never be treated as permission to overwrite.

#### Scenario: Reading a v1 manifest succeeds

- **WHEN** a manifest without `schemaVersion` is loaded
- **THEN** it is interpreted as v1 and loading succeeds without error

#### Scenario: Resolvable absolute paths are migrated

- **WHEN** a v1 entry's absolute path falls under the current solution root
- **THEN** it is converted to a forward-slash relative path and retained

#### Scenario: Unresolvable entries are retained and reported, not invented

- **WHEN** a v1 entry's absolute path cannot be expressed relative to the current solution root
- **THEN** the entry is retained in the migrated manifest marked unresolved
- **AND** a migration warning names the path
- **AND** no relative path is guessed for it

#### Scenario: Missing hash is recorded as unknown, not as a match

- **WHEN** a v1 entry has an empty or absent `ContentHash`
- **THEN** the migrated entry records the hash as unknown
- **AND** it is never treated as though the file matched its last generated content

#### Scenario: Missing ownership is recorded as unknown

- **WHEN** a v1 entry has no ownership information and the descriptor's registry cannot supply one for its template
- **THEN** the migrated entry records ownership as unknown
- **AND** the file is treated as user-owned until an ownership value is established

#### Scenario: Migration never authorizes destructive overwrite

- **WHEN** a migrated entry has unknown hash or unknown ownership and the file exists on disk
- **THEN** the file is not overwritten on the basis of the missing metadata alone
- **AND** it is resolved as an `unverified` file rather than as a verified one
- **AND** the run reports it for explicit resolution

#### Scenario: Upgrade is persisted on the next successful generation

- **WHEN** an aggregate with a v1 manifest is regenerated successfully
- **THEN** the manifest is rewritten as v2 with populated hashes, ownership and `contractVersion` for every entry that was resolved
- **AND** entries still unresolved remain marked as such

### Requirement: Manifest operations never fail silently

Loading, saving, migrating and deleting a manifest SHALL surface failures instead of swallowing them.

#### Scenario: Unreadable manifest is reported

- **WHEN** a manifest file exists but cannot be parsed
- **THEN** a diagnostic identifies the file and the parse error
- **AND** the result is distinguishable from "no manifest present"

#### Scenario: Absent manifest is not an error

- **WHEN** no manifest exists for an aggregate
- **THEN** the load returns an explicit not-present result
- **AND** it is distinguishable from a read failure

#### Scenario: Failed save fails the run

- **WHEN** writing the manifest fails, for example because the path is not writable
- **THEN** a diagnostic identifies the path and the underlying error
- **AND** the generation run does not report success

### Requirement: Single manifest root per solution

The manifest location SHALL be derived from one defined solution root, and manifests found under more than one root SHALL be detected.

#### Scenario: Conflicting manifest roots are reported

- **WHEN** manifests for the same aggregate exist under more than one candidate root
- **THEN** a diagnostic names each location found and the root that was chosen
- **AND** the run does not silently pick one

### Requirement: No absolute paths are ever written

The v2 writer SHALL NOT emit the v1 `FullPath` field or any other absolute path, even for reader compatibility. `schemaVersion` is the compatibility mechanism: a v1-only reader detects `schemaVersion` `2` and migrates or refuses, rather than being fed a stale absolute path.

#### Scenario: v1 FullPath is not carried forward

- **WHEN** a v1 manifest containing `FullPath` is migrated and rewritten as v2
- **THEN** the written document contains no `FullPath` field
- **AND** the written document contains no absolute path

#### Scenario: Schema version signals the change to old readers

- **WHEN** a reader written against v1 opens a v2 manifest
- **THEN** `schemaVersion` `2` identifies the format
- **AND** the reader can detect the mismatch instead of misreading a path
