## ADDED Requirements

### Requirement: Frozen three-value ownership model

Every file the generator writes SHALL be classified with exactly one ownership value. These are the canonical names, used identically in the descriptor, the marker, the manifest, diagnostics, documentation and every consuming surface:

- `GeneratorOwned` — replaceable. Regenerated on every run, but never overwritten before its current hash is compared against the last generated hash; an external divergence produces an explicit diagnostic and never a silent loss.
- `GeneratedOnce` — user-owned after scaffold. Created when the file does not exist; once it exists it is never overwritten automatically.
- `Manual` — outside the generator's ownership. Never created, modified or deleted by the generator.

No surface SHALL introduce a synonym for these values.

#### Scenario: GeneratorOwned file is regenerated after a hash match

- **WHEN** a `GeneratorOwned` file's current hash equals the hash recorded in the manifest
- **THEN** the file is overwritten with the newly generated content
- **AND** the manifest records its ownership as `GeneratorOwned`

#### Scenario: GeneratorOwned file is never overwritten silently on divergence

- **WHEN** a `GeneratorOwned` file's current hash differs from the recorded hash
- **THEN** the file is not overwritten by default
- **AND** an explicit diagnostic reports the divergence

#### Scenario: GeneratedOnce file is not overwritten

- **WHEN** a template declares a file as `GeneratedOnce` and the file already exists on disk
- **THEN** the file content is left unchanged
- **AND** the run reports it as skipped because it is user-owned after scaffold

#### Scenario: GeneratedOnce file is created when absent

- **WHEN** a template declares a file as `GeneratedOnce` and no file exists at that path
- **THEN** the file is created with the generated content
- **AND** the manifest records it as `GeneratedOnce`

#### Scenario: Manual files are never written

- **WHEN** a path is classified `Manual`
- **THEN** the generator does not create, modify or delete it under any flag, including force

#### Scenario: Unclassified existing file is treated as user-owned

- **WHEN** a file exists in a generated output location but has no ownership in the manifest and no Lazy CRUD marker
- **THEN** it is treated as user-owned
- **AND** it is not overwritten or deleted

### Requirement: Ownership is declared by the platform descriptor

The ownership of each template's output SHALL be declared in the descriptor's template registry, so that every surface derives ownership from one published source rather than its own table.

#### Scenario: Registry supplies ownership per template

- **WHEN** a template registered in the descriptor produces a file
- **THEN** the file's ownership is the registry's declared `ownership` for that `templateId`

#### Scenario: Every registered template declares ownership

- **WHEN** the descriptor is validated
- **THEN** every template registry entry has an explicit `ownership` value
- **AND** validation fails naming any entry without one, so no consumer needs to apply a default for a Builder-published template

#### Scenario: Unregistered template output is reported

- **WHEN** generation produces a file from a template that has no registry entry
- **THEN** a diagnostic names the template
- **AND** the output is treated as user-owned rather than silently assumed replaceable

### Requirement: Backward-compatible ownership mapping

The existing boolean parameter `recreateFileIfExists` SHALL continue to work, mapping to the ownership model without changing behavior for any existing call site.

#### Scenario: Existing true call sites keep behaving identically

- **WHEN** an existing template calls the writer with `recreateFileIfExists: true` and no explicit ownership
- **THEN** the file is treated as `GeneratorOwned`
- **AND** the produced file body is byte-identical to the pre-change output

#### Scenario: Existing false call sites keep behaving identically

- **WHEN** an existing template calls the writer with `recreateFileIfExists: false` and no explicit ownership
- **THEN** the file is treated as `GeneratedOnce`
- **AND** an existing file at that path is left unchanged

#### Scenario: Explicit ownership overrides the boolean

- **WHEN** a call site passes an explicit ownership value
- **THEN** that value is used
- **AND** the boolean parameter is ignored for that call

### Requirement: Ownership is declared for every template output

Every writer call site in the shipped templates SHALL carry an explicit ownership declaration once migration is complete, and every declaration SHALL agree with the descriptor registry.

#### Scenario: Ownership coverage is verifiable

- **WHEN** the ownership coverage check runs over `src/ProjectFiles/**/*.tt`
- **THEN** it reports the count of writer call sites and the count carrying explicit ownership
- **AND** the check fails if any call site lacks an explicit ownership declaration

#### Scenario: Call sites agree with the registry

- **WHEN** the coverage check compares each call site's ownership with the descriptor registry entry for its template
- **THEN** they agree
- **AND** the check fails naming any disagreement

#### Scenario: Ownership is queryable per file

- **WHEN** a tool or agent asks for the ownership of a generated file path
- **THEN** the manifest answers with one of `GeneratorOwned`, `GeneratedOnce` or `Manual`
- **AND** it answers unknown only for paths absent from the manifest, which are treated as user-owned
