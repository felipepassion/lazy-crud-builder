## ADDED Requirements

### Requirement: Single version source for all packages

The platform version SHALL be defined in one place and flow to every package. Individual project files SHALL NOT each declare their own literal version.

#### Scenario: Version is defined once

- **WHEN** the platform version is changed
- **THEN** exactly one file is edited
- **AND** every produced package carries the new version

#### Scenario: Version drift fails the build

- **WHEN** a project declares a version that disagrees with the central version
- **THEN** the build or CI validation fails naming the project

### Requirement: Every runtime package is packed and published together

The pack step SHALL produce every package the platform declares, including the templates package.

#### Scenario: Pack list matches the descriptor

- **WHEN** the pack step runs
- **THEN** a package is produced for every entry the descriptor declares
- **AND** the step fails if any declared package is missing from the output

#### Scenario: Templates package is packed

- **WHEN** the pack step runs
- **THEN** the canonical templates package is among the produced packages

#### Scenario: Pack failures fail the run

- **WHEN** packing any project returns a non-zero exit code
- **THEN** the pack step reports failure and exits non-zero
- **AND** it does not report success with a partial package set

### Requirement: Templates package identity is resolvable by existing clients

The canonical templates package SHALL be `Lazy.Crud.Builder.Templates`, and clients that request the historical id `Lazy.Crud.Templates` SHALL still resolve.

#### Scenario: Canonical package carries the templates

- **WHEN** the canonical templates package is installed
- **THEN** it contains the templates at the path the descriptor declares

#### Scenario: Deprecated alias resolves

- **WHEN** a client configured with `TemplatesNuGetPackageId` set to `Lazy.Crud.Templates` restores packages
- **THEN** the templates are resolvable
- **AND** a deprecation notice directs the user to the canonical id

#### Scenario: MSBuild props are auto-imported

- **WHEN** the templates package is installed into a project
- **THEN** its MSBuild targets file is imported automatically by NuGet
- **AND** the file name matches the package id so the import is not skipped

### Requirement: Shared templates ship from exactly one package

Files shared across layers SHALL be delivered by a single package so that installing multiple layer packages cannot produce colliding content files.

#### Scenario: No duplicate content paths across packages

- **WHEN** the produced packages are inspected together
- **THEN** no two packages declare the same `contentFiles` path
- **AND** the validation fails naming any collision

#### Scenario: Multi-layer consumer restores cleanly

- **WHEN** a consumer project references two or more layer packages
- **THEN** restore and build succeed with no duplicate-item or overwrite warnings for template content
- **AND** exactly one copy of each shared template is present

#### Scenario: Dead packaging configuration is removed

- **WHEN** the produced packages are inspected
- **THEN** no package contains template content at a path the descriptor does not declare

### Requirement: Compatibility matrix is published and tested

The platform SHALL publish a compatibility matrix stating which platform versions work with which client surface versions and which manifest schema versions, including support for mixed package versions within one solution.

#### Scenario: Matrix covers mixed version pins

- **WHEN** a solution pins different versions across Lazy CRUD packages within the supported range
- **THEN** the matrix states whether that combination is supported
- **AND** a test exercises a representative mixed-version combination

#### Scenario: Matrix is verified, not asserted

- **WHEN** the compatibility validation runs
- **THEN** each supported combination in the matrix is exercised by an automated check
- **AND** the validation fails if a documented combination does not work

### Requirement: Publication requires no embedded credentials

Publishing SHALL obtain credentials from the environment or a secret store. No credential SHALL appear in repository files.

#### Scenario: No credential in the repository

- **WHEN** the repository is scanned for publish credentials
- **THEN** no API key literal is present in any script or configuration file
- **AND** the scan runs in CI and fails the build on a match

#### Scenario: Publish reads credentials from the environment

- **WHEN** the publish step runs
- **THEN** it reads the API key from an environment variable or CI secret
- **AND** it fails with a clear message when the credential is absent, rather than using a default

### Requirement: The descriptor declares the canonical template distribution strategy

The Builder SHALL resolve the historical ambiguity between an aggregator templates package and per-layer embedded templates by declaring one canonical strategy in the platform descriptor. Consumers SHALL follow that declaration instead of probing candidate layouts.

The declared strategy is: **layer-specific templates ship in their layer's runtime package**, declared per layer as `templatePackageId` and `templateContentPath`; **templates shared across layers ship in exactly one canonical templates package**, declared as `templates.packageId` with `templates.contentRoot`. Every layer package depends on the canonical templates package.

#### Scenario: Strategy is machine-readable

- **WHEN** a consumer resolves templates
- **THEN** it learns from the descriptor which package carries each template and at which in-package path
- **AND** it does not probe alternative candidate layouts

#### Scenario: Shared templates resolve from one package

- **WHEN** a consumer resolves a template shared across layers
- **THEN** it resolves from the canonical templates package
- **AND** no layer package supplies a second copy

#### Scenario: Registry and packages agree

- **WHEN** the distribution validation runs
- **THEN** every template registry entry's `packageId` and `inPackagePath` locates a file in the produced package
- **AND** validation fails naming any entry that does not resolve

#### Scenario: Aggregator alias remains resolvable

- **WHEN** a consumer requests templates using a deprecated aggregator id
- **THEN** the alias resolves to the canonical templates package
- **AND** a deprecation diagnostic names the canonical id

### Requirement: Local template fallback is a diagnosed development mode

Resolving templates from a local source tree SHALL be available only as an explicitly selected development or compatibility mode. It SHALL NEVER be reached silently from a failed package resolution.

#### Scenario: Package resolution failure does not fall back silently

- **WHEN** template resolution is configured for packages and the package cannot be resolved
- **THEN** the operation fails with a diagnostic naming the package id, the expected version and each location searched
- **AND** it does not switch to a local source tree

#### Scenario: Local mode is explicit and reported

- **WHEN** local template resolution is selected explicitly
- **THEN** the run reports that local development mode is in use and names the resolved directory
- **AND** the report distinguishes it from package-based resolution

#### Scenario: Resolved source is always reported

- **WHEN** any generation run completes
- **THEN** the report names the template source kind, its location and the version resolved

#### Scenario: Public validation cannot use local mode

- **WHEN** clean-room or public-distribution validation runs
- **THEN** local template resolution is unavailable
- **AND** any attempt to use it fails the validation

### Requirement: The compatibility matrix declares host prerequisites

The published compatibility matrix SHALL state the host prerequisites for each supported platform version, including the required Visual Studio versions and channels and the component that supplies T4 tooling.

#### Scenario: Matrix names the supported Visual Studio versions

- **WHEN** the compatibility matrix is read
- **THEN** it names the Visual Studio versions and channels supported for generation
- **AND** it names the component that must be installed for T4 tooling

#### Scenario: Prerequisite is verified, not only asserted

- **WHEN** compatibility validation runs
- **THEN** the declared toolchain resolves on the validating machine through the supported discovery mechanism
- **AND** validation fails if a declared prerequisite cannot be resolved

#### Scenario: Prerelease channels are stated explicitly

- **WHEN** the matrix declares channel support
- **THEN** it states whether prerelease and Insiders instances are supported for generation
