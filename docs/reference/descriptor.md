# Platform descriptor (lazycrud.platform.json)

The platform descriptor is the canonical machine-readable contract of a Lazy CRUD release. It is generated from the build (never edited by hand), embedded in every package at the fixed path `lazycrud/lazycrud.platform.json`, and it is the only source of platform vocabulary a consumer (the Visual Studio extension, the CLI, an AI agent, the harness) may rely on. Consumers must ignore unknown additive fields.

- Fixture: `contracts/fixtures/v1/lazycrud.platform.json`
- Schema: `contracts/schemas/v1/lazycrud.platform.schema.json`
- Also shipped inside `Lazy.Crud.Builder.Templates` under `contracts/`, and available after a restore as `$(LazyCrudDescriptorFile)`
- When prose and fixture disagree, the fixture wins.

## Versioning

| Field | Value at 1.2.0 | Meaning |
| --- | --- | --- |
| `schemaVersion` | 1 | The descriptor document schema |
| `contractVersion` | 1 | The version of the shared cross-repository contract this document expresses; consumers negotiate against it |
| `platformVersion` | `1.2.0` | The Lazy CRUD platform version; equal by construction to the NuGet version of every package of the release (single version source) |

The supported range is declared in `compatibility`: `supportedContractVersionMin` 1, `supportedContractVersionMax` 1, `minimumUpgradableContractVersion` 1. A consumer negotiates as follows: a descriptor whose `contractVersion` is above the consumer's maximum, or below the descriptor's own `minimumUpgradableContractVersion` or the consumer's minimum, is rejected with `LC5001` (exit code 6, `ContractMismatch`).

Additions in 1.2.0 that did **not** increment it, because a consumer written against contract 1 still parses the document and no existing code changed meaning: the optional registry field `emptyOutputAllowed`, the diagnostic code `LC2012` and the exit code `9` (`GenerationDefect`).

What an increment implies: `contractVersion` increases only when a field is removed or its meaning changes in a way that breaks a conforming consumer (or a hash, ownership, marker, diagnostic or exit-code semantic changes). The change is recorded in the changelog as contract-breaking, new fixtures and schemas are published under `contracts/fixtures/v<N>/` and `contracts/schemas/v<N>/` while the previous ones remain retrievable, and consumers must be updated before they accept the new documents. Adding an optional field (for example `compatibility.hostRequirements`) does not increment it; a consumer written against the previous contract still parses the document.

## Document structure

### Top level

| Field | Type | Meaning |
| --- | --- | --- |
| `schemaVersion` | integer, `1` | Document schema |
| `contractVersion` | integer | Shared contract version |
| `platformVersion` | string | Platform (and package) version |
| `generatedAtUtc` | string, optional | Generation timestamp; not emitted by the build so the document is deterministic |
| `layers[]` | array | The supported aggregate layers, in pipeline order |
| `testLayers[]` | array | The generated test projects (additive section) |
| `templates` | object | The canonical templates package and the template registry |
| `attributeCategories[]` | strings | The controlled category vocabulary |
| `attributes[]` | array | The attribute catalog (identity and structure) |
| `capabilities[]` | strings | Named feature flags of this build |
| `nonLayerPackages[]` | strings | Packages that are produced but are not a layer runtime package |
| `compatibility` | object | Contract range, manifest schema and host requirements |

### `layers[]`

| Field | Meaning | Example |
| --- | --- | --- |
| `id` | Canonical layer id | `Application.DTO` |
| `projectSuffix` | Suffix appended to the aggregate name | `.Application.DTO` |
| `projectPattern` | Project name pattern | `{Aggregate}.Application.DTO` |
| `packageId` | Runtime package of the layer | `Lazy.Crud.Builder.Application.DTO` |
| `templatePackageId` | Package carrying the layer's templates, or null when the layer has none | `Lazy.Crud.Builder.Application.DTO`; null for `Enumerations` |
| `templateContentPath` | Path of the layer templates inside that package, or null | `contentFiles/any/any/LazyCrudTemplates/Application.DTO/` |
| `outputPathPattern` | Where generated output is written | `src/{Aggregate}/{Aggregate}.Application.DTO/{OutputPath}` |

Layer ids at 1.2.0: `Domain`, `Application`, `Application.DTO`, `Infra.Data`, `Infra.IoC`, `Api.Queries`, `Enumerations`, `Api`.

### `testLayers[]`

| Field | Meaning | Example |
| --- | --- | --- |
| `id` | Test layer id (`Tests.` prefix) | `Tests.DTO` |
| `targetLayer` | The layer it tests | `Application.DTO` |
| `projectPattern` | Project path pattern | `src/{Aggregate}/Tests/Lazy.Crud.{Aggregate}.Tests.DTO` |
| `templatePackageId` | Package carrying the test templates (the target layer's package) | `Lazy.Crud.Builder.Application.DTO` |
| `templateContentPath` | Path inside that package | `contentFiles/any/any/LazyCrudTemplates/Tests/Application.DTO/` |
| `outputPathPattern` | Output location | `src/{Aggregate}/Tests/Lazy.Crud.{Aggregate}.Tests.DTO/{TestOutputPath}` |

### `templates`

| Field | Meaning | Value at 1.2.0 |
| --- | --- | --- |
| `packageId` | The canonical templates package | `Lazy.Crud.Builder.Templates` |
| `aliasPackageIds[]` | Deprecated ids that resolve to the canonical one | `Lazy.Crud.Templates` |
| `version` | Templates version | `1.2.0` |
| `contentRoot` | Root of the templates inside packages | `contentFiles/any/any/LazyCrudTemplates/` |
| `sharedFiles[]` | Files shipped only by the templates package | `project.tt`, `Tests/Tests.Common.ttinclude` |
| `outputPatternTokens[]` | Tokens used by `outputPattern` | `{Aggregate}`, `{Agg}`, `{Entity}`, `{Step}`, `{OutputPath}`, `{TestOutputPath}` |
| `registry[]` | One entry per template (107 at 1.2.0) | see below |

Registry entry:

| Field | Meaning | Example |
| --- | --- | --- |
| `id` | Stable template id (`^[a-z0-9-]+(\.[a-z0-9-]+)+$`); never changes silently | `domain.command-handlers` |
| `layer` | Layer id, test layer id, `WebApi` or `Shared` | `Domain` |
| `sourceFile` | Template source path under the content root, forward slashes | `Domain/DomainCommandHandlers.tt` |
| `outputPattern` | Output location relative to the layer project, or null for entry and include templates | `{OutputPath}/{Agg}.DomainCommandHandlers.cs` |
| `ownership` | `GeneratorOwned`, `GeneratedOnce` or `Manual` | `GeneratorOwned` |
| `templateVersion` | Version of the template | `1.2.0` |
| `packageId` | Package carrying the template | `Lazy.Crud.Builder.Domain` |
| `inPackagePath` | Full path inside that package | `contentFiles/any/any/LazyCrudTemplates/Domain/DomainCommandHandlers.tt` |
| `kind` | `writer` (produces a file through the shared writer), `entry` (orchestration template that includes others and writes nothing itself) or `include` (shared helper with no output) | `writer` |
| `emptyOutputAllowed` | True when this template's correct output carries no significant content (a global-usings file is entirely import directives), so a contentless body is not evidence of a collapsed transformation and `LC2012` does not apply. Absent means false. | `false`; `true` for the seven `*.global-usings` templates |

One source file can have several writer entries when it writes several outputs (for example `domain.command-handlers` and `domain.command-handlers.seed`, the second being the `GeneratedOnce` seed).

### `attributes[]`

| Field | Meaning |
| --- | --- |
| `name` | Type name, for example `RequiredT4` |
| `fullName` | Namespace-qualified type name |
| `namespace` | Namespace |
| `packageId` | `Lazy.Crud.Builder.Application.DTO` for every attribute |
| `category` | One of `attributeCategories[]` |
| `applicability[]` | Targets from the declared `AttributeUsage`, or `All` |
| `overloads[]` | One entry per public constructor, each with `parameters[]` of `name`, `type` (`string`, `int`, `bool`, `double`, `decimal`, `object`, `long`, arrays with `[]`, or the full type name) and `required` (false for parameters with a default value or `params` arrays) |
| `documentationId` | The XML documentation member id in `T:` form, joining the entry to `summary`, `param` and `remarks` in the package's XML documentation file |

The prose is not duplicated in the descriptor. See [Aggregates, entities and attributes](../model.md).

### `capabilities[]`

At 1.2.0: `platform-descriptor`, `generated-code-ownership`, `generated-code-markers`, `generation-manifest-v2`, `drift-protection`, `generation-diagnostics`, `attribute-metadata-contract`, `contract-fixtures`, `unattended-t4-generation`, `templates-alias-resolution`.

### `nonLayerPackages[]`

At 1.2.0: `Lazy.Crud.Builder.Application.DTO.Http.Models`, `Lazy.Crud.Builder.Application.Validators`, `Lazy.Crud.Builder.WebApi`, `Lazy.Crud.CrossCutting.Domain`, `Lazy.Crud.CrossCutting.Infra.Log`, `Lazy.Crud.CrossCutting.Infra.Utils`, `Lazy.Crud.CrossCutting.Application.Mail`, `Lazy.Crud.Builder.Platform`, `Lazy.Crud.Builder.Templates`, `Lazy.Crud.Templates`.

### `compatibility`

| Field | Value at 1.2.0 | Meaning |
| --- | --- | --- |
| `supportedContractVersionMin` | 1 | Lowest contract version accepted |
| `supportedContractVersionMax` | 1 | Highest contract version understood |
| `minimumUpgradableContractVersion` | 1 | Lowest contract version this build upgrades from |
| `minimumManifestSchemaVersion` | 1 | Lowest manifest schema read (v1 is migrated) |
| `manifestSchemaVersion` | 2 | Manifest schema written |
| `mixedVersionRanges[]` | three statements | The supported mixed pins, as the [compatibility matrix](../compatibility.md#mixed-package-versions) states them: `Lazy.Crud.Builder.Api` may be one patch version behind every other package; the templates package must equal the version of every other package that ships templates; every other package is at one version, because pinning a package below the version another package requires of it is a downgrade NuGet rejects (`NU1605`) |
| `hostRequirements` | object | `platform` (`windows`), `visualStudioVersions` (`17.x (Visual Studio 2022)`, `18.x (Visual Studio 2026)`), `visualStudioChannels` (`release`, `preview`, `insiders`), `t4Component` (`Microsoft.VisualStudio.Component.TextTemplating`), `t4ComponentDisplayName` (`Text Template Transformation`), `dotnetSdk` (`10.0`), `headlessDefinition` (the sentence defining headless as unattended, not Visual Studio-free). Additive and optional; it does not by itself require a contract version increment. |

## Example (excerpt)

```json
{
  "schemaVersion": 1,
  "contractVersion": 1,
  "platformVersion": "1.2.0",
  "layers": [
    {
      "id": "Domain",
      "projectSuffix": ".Domain",
      "projectPattern": "{Aggregate}.Domain",
      "packageId": "Lazy.Crud.Builder.Domain",
      "templatePackageId": "Lazy.Crud.Builder.Domain",
      "templateContentPath": "contentFiles/any/any/LazyCrudTemplates/Domain/",
      "outputPathPattern": "src/{Aggregate}/{Aggregate}.Domain/{OutputPath}"
    }
  ],
  "templates": {
    "packageId": "Lazy.Crud.Builder.Templates",
    "aliasPackageIds": ["Lazy.Crud.Templates"],
    "version": "1.2.0",
    "contentRoot": "contentFiles/any/any/LazyCrudTemplates/",
    "registry": [
      {
        "id": "domain.filters",
        "layer": "Domain",
        "sourceFile": "Domain/Filters.tt",
        "outputPattern": "{OutputPath}/{Agg}.Filters.cs",
        "ownership": "GeneratorOwned",
        "templateVersion": "1.2.0",
        "packageId": "Lazy.Crud.Builder.Domain",
        "inPackagePath": "contentFiles/any/any/LazyCrudTemplates/Domain/Filters.tt",
        "kind": "writer"
      }
    ]
  },
  "compatibility": {
    "supportedContractVersionMin": 1,
    "supportedContractVersionMax": 1,
    "minimumUpgradableContractVersion": 1,
    "minimumManifestSchemaVersion": 1,
    "manifestSchemaVersion": 2,
    "hostRequirements": {
      "platform": "windows",
      "visualStudioVersions": ["17.x (Visual Studio 2022)", "18.x (Visual Studio 2026)"],
      "visualStudioChannels": ["release", "preview", "insiders"],
      "t4Component": "Microsoft.VisualStudio.Component.TextTemplating",
      "dotnetSdk": "11.0"
    }
  }
}
```

## Resolution among several copies

A solution with several installed packages holds several embedded copies. A consumer collects them and resolves: the copy with the highest `platformVersion` is canonical (ties broken by source path); when the versions disagree, `LC5002` (warning) names every version and the package it came from. A copy that cannot be parsed, or no copy at all, is `LC5003` (error). Reading the descriptor requires neither MSBuild nor every package to be installed: any single `Lazy.Crud.Builder.*` package lists all layers, templates and attributes.

## How it is produced and validated

The `LazyCrudGenerateDescriptor` target runs after the runner project builds: `descriptor generate --repo <repo> --version <LazyCrudVersion> --attributes-assembly <Application.DTO dll> --output artifacts/platform/lazycrud.platform.json --fixtures contracts`. Generation fails on any attribute metadata violation. `validate tree` checks the registry against the shipped templates in both directions, `validate packages` checks the produced packages against the descriptor (declared packages, template paths, collisions, the alias dependency), and `validate fixtures` checks that the committed fixture equals the build and validates against the schema.
