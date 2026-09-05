## ADDED Requirements

### Requirement: The Builder publishes the complete attribute metadata consumers need

Attribute metadata is a frozen shared contract. The Builder SHALL publish everything a consuming surface needs to build an attribute experience, so no consumer maintains its own attribute taxonomy. The contract has exactly two halves:

- **Identity and structure** — in the platform descriptor's `attributes[]`: `name`, `fullName`, `namespace`, `packageId`, `category`, `applicability`, `overloads[]` with `parameters[]` (`name`, `type`, `required`), and `documentationId`.
- **Prose** — in the XML documentation file shipped inside the package: `summary`, `param` per constructor parameter, and `remarks`.

Consumers join the two by `documentationId`. The Builder SHALL NOT publish the prose twice, and SHALL NOT require a consumer to infer category, applicability or parameter shape from source or naming conventions.

#### Scenario: Documentation ships to consumers

- **WHEN** a package containing attributes is produced
- **THEN** the XML documentation file is included in the package
- **AND** it contains the `summary`, `param` and `remarks` content for its attributes

#### Scenario: Identity and prose are joinable

- **WHEN** a consumer holds the descriptor catalog and the package's XML documentation
- **THEN** every catalog entry's `documentationId` resolves to that attribute's documentation member

#### Scenario: Consumers need no taxonomy of their own

- **WHEN** a consumer needs an attribute's name, namespace, category, applicability, overloads or parameter names
- **THEN** every one of those values is available from the published contract
- **AND** no value has to be hardcoded by the consumer

#### Scenario: Every public attribute has a summary and category

- **WHEN** the attribute metadata check runs
- **THEN** every public attribute type has a non-empty `<summary>` and a `<category>`
- **AND** the check fails naming any attribute missing either

#### Scenario: Every constructor parameter is documented

- **WHEN** an attribute declares a constructor with parameters
- **THEN** each parameter has a corresponding `<param>` entry
- **AND** the check fails naming any undocumented parameter

#### Scenario: Categories come from a controlled vocabulary

- **WHEN** an attribute declares a `<category>`
- **THEN** the value is one of the documented category names published in the contract
- **AND** the check fails on an unrecognized category

#### Scenario: Catalog and assembly agree

- **WHEN** the descriptor catalog is validated against the attribute types in the produced packages
- **THEN** every catalog entry corresponds to a type present in the named package, and every public attribute type has a catalog entry
- **AND** validation fails naming any disagreement, so no consumer has to hide or invent entries at runtime

### Requirement: Attribute types are consistent with their documentation

A type documented as a Lazy CRUD attribute SHALL actually be usable as an attribute.

#### Scenario: Documented attributes derive from Attribute

- **WHEN** the attribute metadata check runs over the attributes namespaces
- **THEN** every type published in the catalog derives from `System.Attribute`
- **AND** the check fails naming any type that does not

#### Scenario: Non-attribute metadata types are classified correctly

- **WHEN** a type in the attributes area is not an attribute, such as a settings or descriptor type
- **THEN** it is either converted to a real attribute or excluded from the catalog
- **AND** consumers never offer it in an attribute picker

#### Scenario: Applicability reflects real usage targets

- **WHEN** a catalog entry declares `applicability`
- **THEN** it matches the targets the type actually permits
- **AND** the check fails on a disagreement

### Requirement: Attribute contract changes are versioned

Changes to attribute names, namespaces, categories, applicability or parameters SHALL be treated as public contract changes.

#### Scenario: Removing or renaming an attribute is breaking

- **WHEN** a public attribute is renamed or removed
- **THEN** the change is recorded in the changelog as breaking
- **AND** the `contractVersion` and the compatibility matrix record the platform version in which it changed

#### Scenario: Namespace placement is published

- **WHEN** attributes ship across more than one namespace
- **THEN** each catalog entry declares its `namespace` and `packageId`
- **AND** the documentation matches the shipped assemblies

#### Scenario: Catalog changes reach consumers as fixtures

- **WHEN** the attribute catalog changes
- **THEN** the published contract fixtures are regenerated
- **AND** consuming repositories can detect the change in their contract tests
