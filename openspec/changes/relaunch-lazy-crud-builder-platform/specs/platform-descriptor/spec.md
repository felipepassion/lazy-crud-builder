## ADDED Requirements

### Requirement: Canonical platform descriptor

The Builder SHALL publish a single machine-readable descriptor, `lazycrud.platform.json`, that is the authoritative source of the shared platform vocabulary. It is the only contract consuming surfaces (`LazyCrud.Core`, the CLI, the VSIX, AI agents) are permitted to rely on, and consumers SHALL NOT depend on any field the Builder does not promise here.

The descriptor SHALL contain:

**Document identity**
- `schemaVersion` — the descriptor document schema version.
- `contractVersion` — the version of the shared cross-repository contract this document expresses. Consumers negotiate against this value.
- `platformVersion` — the Lazy CRUD platform version. By construction it equals the NuGet package version of every package in the release, because all packages share one version source.

**`layers[]`** — one entry per supported layer:
- `id` — the canonical layer identifier, for example `Domain`.
- `projectSuffix` — the suffix appended to the aggregate name, for example `.Domain`.
- `projectPattern` — the full project-name pattern, for example `{Aggregate}.Domain`.
- `packageId` — the runtime package for the layer.
- `templatePackageId` — the package that carries this layer's templates.
- `templateContentPath` — the path inside that package where this layer's templates live.
- `outputPathPattern` — where generated output for this layer is written.

**`templates`**
- `packageId` — the canonical templates package id.
- `aliasPackageIds[]` — deprecated ids that resolve to the canonical one.
- `version` — the templates version.
- `contentRoot` — the root path inside the package where templates live.
- `registry[]` — one entry per template: `id` (stable `templateId`), `layer`, `sourceFile`, `outputPattern`, `ownership`, `templateVersion`, `packageId` and `inPackagePath`.

**`attributes[]`** — the attribute catalog identity and structure:
- `name`, `fullName`, `namespace`, `packageId`.
- `category` — from the controlled vocabulary.
- `applicability` — the targets the attribute may be applied to.
- `overloads[]` — each with `parameters[]` of `name`, `type` and `required`.
- `documentationId` — the XML documentation member id (`T:` form) used to join this entry to the prose shipped in the package's `.xml` file.

**`capabilities[]`** — named feature flags this build supports.

**`compatibility`** — the supported `contractVersion` range, the minimum upgradable `contractVersion`, the minimum manifest `schemaVersion` understood, and `hostRequirements` declaring the host prerequisites for generation (the required Visual Studio versions and channels and the component supplying T4 tooling). `hostRequirements` is an additive optional field and does not by itself require a `contractVersion` increment.

#### Scenario: Descriptor is generated during build

- **WHEN** the solution is built or packed
- **THEN** `lazycrud.platform.json` is generated from MSBuild properties and the template registry
- **AND** it is embedded in every packable `Lazy.Crud.Builder.*` and `Lazy.Crud.CrossCutting.*` package at a fixed, documented path

#### Scenario: Descriptor identifies itself

- **WHEN** a descriptor document is loaded
- **THEN** it declares `schemaVersion`, `contractVersion` and `platformVersion`

#### Scenario: Descriptor supplies the complete consumer vocabulary

- **WHEN** a consumer needs a layer id, project suffix, package id, template id, template file name, output pattern, template ownership or attribute category
- **THEN** every one of those values is present in the descriptor
- **AND** the consumer never needs a hardcoded table to obtain it

#### Scenario: Descriptor declares every packed layer

- **WHEN** the descriptor is validated against the produced `.nupkg` set
- **THEN** every layer's `packageId` and `templatePackageId` matches a package actually produced by the pack step
- **AND** every produced runtime package appears in `layers[]` or is explicitly listed as a non-layer package

#### Scenario: Every template is registered with an id and ownership

- **WHEN** the descriptor is validated against the shipped templates
- **THEN** every shipped template has a `registry[]` entry with a non-empty `id` and an `ownership` value
- **AND** validation fails naming any template that is shipped but unregistered, or registered but not shipped

#### Scenario: Template content paths match the packed layout

- **WHEN** the descriptor declares `templates.contentRoot`, `layers[].templateContentPath` or a registry entry's `inPackagePath`
- **THEN** each of those paths exists inside the produced package it names
- **AND** validation fails if the packed layout differs

#### Scenario: Drift between descriptor and build fails CI

- **WHEN** a project declares a `PackageId` or version that disagrees with the descriptor
- **THEN** the build or CI validation step fails with a message naming the project, the declared value and the expected value

### Requirement: Attribute catalog identity is published, prose is not duplicated

The descriptor SHALL publish attribute identity and structure — name, namespace, package, category, applicability, constructor overloads and parameters, and the documentation member id. Human-readable prose (`summary`, `param`, `remarks`) SHALL remain in the XML documentation file shipped in the package, and consumers SHALL join the two by `documentationId`. The Builder SHALL NOT publish the prose twice, and consumers SHALL NOT maintain their own attribute taxonomy.

#### Scenario: Catalog is complete for the shipped assemblies

- **WHEN** the descriptor is validated against the attribute types in the produced packages
- **THEN** every public attribute type has a catalog entry
- **AND** validation fails naming any attribute present in the assembly but absent from the catalog, or present in the catalog but absent from the assembly

#### Scenario: Documentation id joins catalog to prose

- **WHEN** a consumer holds a catalog entry and the package's XML documentation file
- **THEN** the entry's `documentationId` resolves to that attribute's documentation member
- **AND** `summary`, `param` and `remarks` are readable without inferring the id

#### Scenario: Every catalog entry carries a category

- **WHEN** the catalog is produced
- **THEN** every entry has a `category` from the controlled vocabulary
- **AND** validation fails naming any entry without one, so consumers never need a default-group fallback for a Builder-published attribute

#### Scenario: Overloads and parameters are published

- **WHEN** an attribute declares one or more constructors
- **THEN** the catalog entry lists each overload with its parameter names, types and whether each is required

### Requirement: Contract version negotiation

The descriptor SHALL carry the information consumers need to negotiate compatibility explicitly, and the Builder SHALL define the meaning of each outcome.

#### Scenario: Compatibility range is declared

- **WHEN** a descriptor is published
- **THEN** it declares the supported `contractVersion` range, the minimum upgradable `contractVersion` and the minimum manifest `schemaVersion`

#### Scenario: Contract version increments on a breaking contract change

- **WHEN** a field is removed or its meaning changes in a way that would break a conforming consumer
- **THEN** `contractVersion` is incremented
- **AND** the change is recorded in the changelog as a contract-breaking change

#### Scenario: Additive change does not break consumers

- **WHEN** a new optional field is added to the descriptor
- **THEN** `contractVersion` is not required to change
- **AND** a consumer written against the previous version still parses the document

### Requirement: Surfaces resolve platform facts from the descriptor

Consumers SHALL be able to read the descriptor from an installed package without executing MSBuild and without requiring every package to be installed.

#### Scenario: Reading the descriptor from a single installed package

- **WHEN** a consumer has installed only one `Lazy.Crud.Builder.*` package
- **THEN** the descriptor is readable from that package
- **AND** it lists all supported layers, templates and attributes, not only those of the installed package

#### Scenario: Mixed package versions produce a defined winner

- **WHEN** a solution has packages of differing versions installed and their embedded descriptors disagree
- **THEN** the descriptor with the highest `platformVersion` is canonical
- **AND** a diagnostic is emitted naming each conflicting version and the package it came from

### Requirement: Canonical templates package identity

The descriptor SHALL declare exactly one canonical `templates.packageId` and SHALL record every deprecated alias that resolves to it, so no consumer has to guess which package carries templates.

#### Scenario: Deprecated alias is declared

- **WHEN** a consumer requests templates by a deprecated id such as `Lazy.Crud.Templates`
- **THEN** the descriptor identifies it in `aliasPackageIds[]` as an alias of the canonical `Lazy.Crud.Builder.Templates`
- **AND** the consumer resolves templates without failing

#### Scenario: Per-layer template packages are declared

- **WHEN** a layer's templates ship in that layer's own runtime package
- **THEN** the layer entry declares that `templatePackageId` and its `templateContentPath`
- **AND** a consumer resolving templates per layer never needs a single hardcoded package id

### Requirement: Host prerequisites are declared in the descriptor

The descriptor SHALL declare, under `compatibility.hostRequirements`, the host prerequisites a machine must satisfy to generate with this platform version, so a surface can check them before starting work instead of discovering them through a failure.

#### Scenario: Prerequisites are machine-readable

- **WHEN** a surface prepares to generate
- **THEN** it reads the required Visual Studio versions, channels and T4 tooling component from `compatibility.hostRequirements`
- **AND** it does not hardcode that requirement

#### Scenario: Declaration matches the published matrix

- **WHEN** the descriptor validation runs
- **THEN** `compatibility.hostRequirements` agrees with the published compatibility matrix
- **AND** validation fails naming any disagreement

#### Scenario: Field is additive for existing consumers

- **WHEN** a consumer written against a descriptor without `hostRequirements` reads one that has it
- **THEN** the document still parses
- **AND** no `contractVersion` increment is required solely for this field
