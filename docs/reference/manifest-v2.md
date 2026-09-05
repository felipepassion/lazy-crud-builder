# Generation manifest v2

The generation manifest records every file the generator wrote for one aggregate, with its ownership and content hash. It is a frozen shared contract (contractVersion 1): the runner, the shared writer, the Visual Studio extension, the CLI and the harness read and write the same document.

- Location: `{solutionRoot}/.lazycrud/manifests/{Aggregate}.manifest.json` (the solution root is the directory of the `.sln` or `.slnx` file)
- Fixture: `contracts/fixtures/v1/manifest-v2.json`
- Migration pair: `contracts/fixtures/v1/manifest-v1-input.json` and `contracts/fixtures/v1/manifest-v1-migrated.json`
- Schema: `contracts/schemas/v1/manifest-v2.schema.json`
- When prose and fixture disagree, the fixture wins.

## Document fields

| Field | Type | Meaning |
| --- | --- | --- |
| `schemaVersion` | integer, `2` | Document schema. A v1 document has no `schemaVersion` (or `1`). |
| `aggregateName` | string | The aggregate |
| `generatedAtUtc` | string, ISO 8601 UTC | Time of the last save |
| `generatorVersion` | string | The platform version that wrote the document (`1.1.0`), from the single version source, never a placeholder |
| `contractVersion` | integer | The `contractVersion` of the platform descriptor used for the run (`1`) |
| `solutionRootMarker` | string or null, optional | How relative paths are anchored: the file name of the solution the root was derived from, for example `Sandbox.sln` |
| `files[]` | array | One entry per recorded file |

## Entry fields

| Field | Type | Meaning |
| --- | --- | --- |
| `relativePath` | string or null | Path relative to the solution root with forward slashes, for example `src/Products/Products.Domain/LazyCode/ProductsAgg.Filters.cs`. Null only for unresolved migrated entries. Never an absolute path, a drive letter or a backslash. |
| `layer` | string | Layer id, for example `Domain`, `Tests.Domain`; may be empty for legacy entries |
| `templateId` | string or null | Stable template id from the descriptor registry, for example `domain.filters`; `legacy:<TemplateName>` or null for migrated entries with no registry match |
| `templateVersion` | string or null | Template version (`1.1.0`); null for migrated entries not yet regenerated |
| `ownership` | string | `GeneratorOwned`, `GeneratedOnce`, `Manual` or `Unknown` (migration only) |
| `contentHash` | string or null | Lowercase hexadecimal SHA-256 computed under the frozen rule; null when unknown |
| `hashAlgorithm` | string or null | `SHA-256` |
| `hashScope` | string or null | `body-lf-marker-excluded` (marker block removed before hashing) or `body-lf` (whole normalized body, for files without a marker) |
| `legacyHash` | string, optional | The v1 raw-file hash carried over by migration. Informational only; never a baseline. |
| `unresolved` | boolean, optional | `true` for a migrated v1 entry whose absolute path could not be expressed under the current solution root |
| `fileName` | string, optional | File name of an unresolved entry (never a path) |
| `originalPathSha256` | string, optional | SHA-256 of the original absolute path of an unresolved entry, so it can be recognized without storing the path |

A hash is a trustworthy baseline only when `contentHash` is present, `hashAlgorithm` is `SHA-256` and `hashScope` is one of the two frozen scopes. Everything else (a missing hash, a `legacyHash` only) makes the file `unverified` on the next run.

## Example

The fixture, produced by the implementation:

```json
{
  "schemaVersion": 2,
  "aggregateName": "Products",
  "generatedAtUtc": "2026-01-01T00:00:00.0000000Z",
  "generatorVersion": "1.1.0",
  "contractVersion": 1,
  "solutionRootMarker": "Sandbox.sln",
  "files": [
    {
      "relativePath": "src/Products/Products.Domain/LazyCode/ProductsAgg.Filters.cs",
      "layer": "Domain",
      "templateId": "domain.filters",
      "templateVersion": "1.1.0",
      "ownership": "GeneratorOwned",
      "contentHash": "5728dc3dfc6273dddf7b145bf2ff69cbc8ab5cd03982ad87245f6e3fca83117d",
      "hashAlgorithm": "SHA-256",
      "hashScope": "body-lf-marker-excluded"
    },
    {
      "relativePath": "src/Products/Products.Domain/Aggregates/ProductsAgg/Entities/Products.cs",
      "layer": "Domain",
      "templateId": "domain.aggregate-settings-entity",
      "templateVersion": "1.1.0",
      "ownership": "GeneratedOnce",
      "contentHash": "e6e5c36e2a7dbd8b3d97d4228100b8c25163f0181069bf324fae0e0ce8066e46",
      "hashAlgorithm": "SHA-256",
      "hashScope": "body-lf-marker-excluded"
    },
    {
      "relativePath": "src/Products/Products.Api/Migrations.generated.bat",
      "layer": "Api",
      "templateId": "api.mini-migrations",
      "templateVersion": "1.1.0",
      "ownership": "GeneratorOwned",
      "contentHash": "54b654a8ad7f3d0ab59a31cdbc2f0edc289de3b393aa3c7754f5ba1ad6d3c7ed",
      "hashAlgorithm": "SHA-256",
      "hashScope": "body-lf-marker-excluded"
    }
  ]
}
```

The third entry is written by the T4 host rather than by the template (`Api/MiniMigrations.tt` declares an output extension). The writer still resolves ownership, renders the marker in the file type's comment syntax and records the entry, so a host-written file is protected like any other. `body-lf` replaces `body-lf-marker-excluded` when the file type has no safe comment syntax (JSON, XML, plain text) or when `GeneratedCodeMarkers` is disabled in `lazy.settings`. A `GeneratedOnce` entry records the hash of the seed as written; that hash is informational, because the file is never overwritten.

## Rules

- **Never an absolute path.** The writer refuses to serialize a path that starts with `/`, contains a drive letter or contains a backslash. The v1 `FullPath` field is never carried forward. `schemaVersion` is the compatibility mechanism for old readers.
- **Portable.** Because paths are relative to the solution root, a manifest generated on one machine resolves on another with the solution at a different absolute path.
- **One root per solution.** The manifest root is the solution root. When a manifest for the same aggregate also exists under `src/` (a historical second root), `LC1005` names both and the solution root is chosen.
- **No silent failure.** Loading distinguishes not present (not an error) from unreadable (`LC1003`, the run stops and no generated file is touched). A failed save (`LC1004`) fails the run.
- **Serialized writes.** The writer holds a system-wide named mutex keyed by the manifest path while it reads, updates and saves, so parallel layer transformations of one aggregate never interleave.
- **Saved atomically.** The document is written to a temporary file and moved into place.

## v1 migration

A document with no `schemaVersion` is v1 (PascalCase fields `AggregateName`, `GeneratedAtUtc`, `GeneratorVersion`, `GeneratedFiles[]` with `FullPath`, `TemplateName`, `Layer`, `ContentHash`). It is migrated in memory when loaded (`LC1009`) and rewritten as v2 after the next successful generation, or in place with:

```powershell
dotnet Lazy.Crud.Builder.Platform.Tool.dll manifest migrate --solution <dir> --aggregate <Name>
```

Migration preserves every entry and never fabricates values:

| v1 situation | v2 result | Diagnostic |
| --- | --- | --- |
| `FullPath` under the current solution root | `relativePath` with forward slashes | |
| `FullPath` outside the current root (another machine) | `relativePath` null, `unresolved` true, `fileName`, `originalPathSha256` | `LC1006` (warning) |
| Output file name matches exactly one registry writer pattern | `templateId` and `ownership` from the registry (`layer` filled when empty) | |
| No unambiguous registry match | `templateId` is `legacy:<TemplateName>` (or null when the name was `Unknown`), `ownership` is `Unknown` | `LC1008` (warning) |
| `ContentHash` present | copied to `legacyHash`; `contentHash`, `hashAlgorithm`, `hashScope` null | |
| `ContentHash` empty | nothing recorded | `LC1007` (warning) |

The fixture pair shows all three cases: `manifest-v1-input.json` holds one resolvable entry with a hash, one resolvable entry without a hash, and one entry from another machine; `manifest-v1-migrated.json` is the exact expected output (with `solutionRootMarker` `Sandbox.sln` and the fixed root `C:\lazycrud-fixture\solution` used for the pair). Harness scenario R1 seeds the input and asserts the output.

Absent metadata is never permission to overwrite: after migration every existing `GeneratorOwned` file is `unverified` and is adopted only when its content is identical to the regenerated body (`LC2008`); otherwise it is protected (`LC2002`). Entries with `Unknown` ownership are treated as user-owned (`LC2003`).

## Validating a manifest

```powershell
dotnet Lazy.Crud.Builder.Platform.Tool.dll validate manifest --file .lazycrud\manifests\Products.manifest.json --repo <lazy-crud-builder checkout>
```

The check validates the document against `contracts/schemas/v1/manifest-v2.schema.json`. Consumers can validate with any JSON Schema 2020-12 validator.
