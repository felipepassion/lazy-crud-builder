## ADDED Requirements

### Requirement: Validation runs against the public artifact only

The clean-room validation SHALL consume the produced packages exactly as an external user would, with no access to this repository's sources, and SHALL fail if the public artifact is incomplete.

#### Scenario: Isolated package cache

- **WHEN** clean-room validation runs
- **THEN** it uses a package cache directory created for that run
- **AND** no previously restored Lazy CRUD package can satisfy a restore

#### Scenario: Only the public feed is configured

- **WHEN** clean-room validation runs
- **THEN** its NuGet configuration clears all inherited sources and adds only the feed under test
- **AND** no private or developer-machine source is reachable

#### Scenario: No local source is reachable

- **WHEN** clean-room validation runs
- **THEN** the sandbox contains no copy of the Builder's template sources
- **AND** no path outside the sandbox is used to resolve templates

#### Scenario: Missing package fails loudly

- **WHEN** a package the descriptor declares is absent from the feed under test
- **THEN** validation fails naming the missing package
- **AND** it does not substitute a local or cached copy

### Requirement: Fallback behavior is disabled during validation

Template resolution fallbacks that mask a broken distribution SHALL be disabled for the validation run.

#### Scenario: NuGet resolution does not silently degrade

- **WHEN** template resolution is configured for NuGet and the package cannot be resolved
- **THEN** resolution fails with a diagnostic
- **AND** it does not fall back to a local directory

#### Scenario: Fallback remains available outside validation

- **WHEN** a normal, non-validation run cannot resolve templates from NuGet
- **THEN** the existing fallback behavior is unchanged
- **AND** the fallback is announced by a warning diagnostic

### Requirement: The consumed artifact is inspected, not assumed

Validation SHALL assert the contents of the consumed packages against the descriptor.

#### Scenario: Template layout matches the descriptor

- **WHEN** the templates package is restored in the clean room
- **THEN** the templates are found at the path the descriptor declares
- **AND** validation fails if they are at a different path

#### Scenario: Descriptor is present in restored packages

- **WHEN** a runtime package is restored in the clean room
- **THEN** the embedded platform descriptor is readable from it
- **AND** its declared platform version matches the version under test

#### Scenario: Documentation file is present

- **WHEN** a package containing attributes is restored
- **THEN** its XML documentation file is present and contains the attribute metadata

### Requirement: A clean-room consumer builds and generates

The validation SHALL prove that an external consumer can go from an empty project to generated, compiling code using only the published artifacts.

#### Scenario: External consumer restores and builds

- **WHEN** a new project in the sandbox references the published packages
- **THEN** restore and build succeed
- **AND** no reference to the Builder repository is required

#### Scenario: External consumer generates code

- **WHEN** the sandbox consumer runs generation for a sample aggregate using only restored templates
- **THEN** the expected files are produced
- **AND** the generated code compiles

### Requirement: The full shared contract resolves from the distribution alone

Clean-room validation SHALL prove that every frozen shared contract a consumer depends on is obtainable from the published artifacts, with no platform source checkout and no local fallback.

#### Scenario: Descriptor resolves from the distribution

- **WHEN** the sandbox restores the published packages
- **THEN** the platform descriptor is readable from them
- **AND** it declares `schemaVersion`, `contractVersion` and `platformVersion`

#### Scenario: Template registry resolves from the distribution

- **WHEN** templates are resolved in the sandbox
- **THEN** every template the descriptor registry declares is located using only the registry's `packageId` and `inPackagePath`
- **AND** validation fails naming any registry entry that does not resolve

#### Scenario: Attribute catalog resolves from the distribution

- **WHEN** the attribute catalog is assembled in the sandbox
- **THEN** identity and structure come from the restored descriptor and prose from the restored XML documentation file
- **AND** no platform source file is required

#### Scenario: Local development mode is unavailable

- **WHEN** clean-room validation runs
- **THEN** local template resolution is disabled
- **AND** any attempt to select it fails the validation rather than degrading to it

#### Scenario: Headless generation works from the distribution

- **WHEN** the sandbox generates an aggregate
- **THEN** transformation runs unattended through the Visual Studio T4 tooling, using only restored artifacts for templates and packages
- **AND** it requires no platform source checkout and no human interaction with Visual Studio

### Requirement: Clean room means no private source, not a bare machine

A clean room SHALL be defined by the absence of private sources, local template fallbacks and non-public feeds — not by the absence of the platform's declared prerequisites. The machine running clean-room validation SHALL satisfy the official prerequisites, including an installed Visual Studio with T4 tooling, because that is part of what a real external user is required to have.

#### Scenario: Prerequisites are present and reported

- **WHEN** clean-room validation runs
- **THEN** the Visual Studio T4 toolchain resolves and the resolved instance is recorded in the run output
- **AND** its presence is treated as a satisfied prerequisite, not as a fallback

#### Scenario: Isolation applies to sources and feeds only

- **WHEN** clean-room isolation is applied
- **THEN** it removes platform source checkouts, local template fallbacks and non-public feeds
- **AND** it does not remove or hide the declared platform prerequisites

#### Scenario: An external user could reproduce the run

- **WHEN** the clean-room configuration is documented
- **THEN** it states exactly what an external user must have installed and what must be absent
- **AND** the stated prerequisites match the published compatibility matrix
