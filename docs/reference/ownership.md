# Ownership

Every file the generator writes is classified with exactly one ownership value. The three names are canonical and frozen (contractVersion 1): the descriptor registry, the marker header, the manifest, the diagnostics, the documentation and every consuming surface use exactly these strings and never a synonym.

- Fixture: `ownership` in `contracts/fixtures/v1/fixture-index.json`; `templates.registry[].ownership` in `contracts/fixtures/v1/lazycrud.platform.json`; `files[].ownership` in `contracts/fixtures/v1/manifest-v2.json`
- Schema enumerations: `contracts/schemas/v1/lazycrud.platform.schema.json` (`GeneratorOwned`, `GeneratedOnce`, `Manual`) and `contracts/schemas/v1/manifest-v2.schema.json` (the same three plus `Unknown`)
- When prose and fixture disagree, the fixture wins.

## Values

| Value | Definition | Created | Overwritten | Deleted |
| --- | --- | --- | --- | --- |
| `GeneratorOwned` | Replaceable. Regenerated on every run, but never overwritten before its current hash is compared against the last generated hash; an external divergence produces an explicit diagnostic and never a silent loss. | When absent | Only when `verified` (hash matches), `missing`, or `unverified` with identical content (adoption); otherwise only with `--resolution force` | Never by the generator |
| `GeneratedOnce` | User-owned after scaffold. Created when the file does not exist; once it exists it is never overwritten automatically. | When absent | Never (`LC2005` reports the skip); delete the file to have it scaffolded again | Never by the generator |
| `Manual` | Outside the generator's ownership. | Never | Never, under any flag including force (`LC2010`) | Never |

`Unknown` is not an ownership value a template can declare. It appears only in a migrated v1 manifest entry whose ownership could not be established; such a file is treated as user-owned and protected (`LC2003`) until a registered template re-records it.

## Where ownership is declared and how it is resolved

1. **Explicit value at the writer call site.** Every `SaveOutput` and `SaveOutputToSubFolder` call in the shipped templates carries `ownership:` and `templateId:` arguments; `validate tree` fails when any call site lacks either or disagrees with the registry.
2. **The descriptor registry.** `templates.registry[].ownership` is the published declaration per `templateId`; when a call site passes only a template id, the registry supplies the ownership. Every registry entry has an explicit value.
3. **The legacy boolean.** For a call site with neither, the historical `recreateFileIfExists` parameter maps exactly: `true` is `GeneratorOwned`, `false` is `GeneratedOnce`. Existing call sites therefore behave as before. An explicit ownership always overrides the boolean.

A template with no registry entry is reported (`LC2004`) and its output is treated as user-owned: a `GeneratorOwned` request from an unregistered template is downgraded to `GeneratedOnce` (created only when absent).

Registry entries with `GeneratedOnce` at 1.1.0: `domain.aggregate-settings-entity` (`Aggregates/{Agg}/Entities/{Aggregate}.cs`), `domain.user-entity` (`Aggregates/UsersAgg/Entities/User.cs`), `domain.command-handlers.seed` (`Aggregates/{Agg}/CommandHandlers/{Entity}CommandHandler.cs`), `application-dto.requests.seed` (`Aggregates/{Agg}/Requests/{Entity}DTO.cs`), `application-dto.steppable-requests-validators.seed` (`Aggregates/{Agg}/Validators/{Entity}Validator.cs`) and `webapi.client-listining-pages.index-css` (`{Agg}/{Entity}/Index.razor.css`). Every other writer entry is `GeneratorOwned`. No shipped template declares `Manual`; the value exists so that a path can be declared off-limits.

## Where ownership is visible

| Surface | Field |
| --- | --- |
| Marker header | `Ownership: GeneratorOwned` or `Ownership: GeneratedOnce`, with a distinct `Policy` line per value; `Manual` files carry no marker |
| Manifest v2 | `files[].ownership`, one of the three values or `Unknown` |
| Descriptor | `templates.registry[].ownership` |
| Diagnostics | `LC2001`, `LC2002` (protected `GeneratorOwned` files), `LC2003` (unknown), `LC2004` (unregistered), `LC2005` (`GeneratedOnce` skipped), `LC2010` (`Manual` refused) |

The manifest is the query interface for tools and agents: it answers the ownership of any recorded path with one of the three values, and it answers unknown only for migrated entries; paths absent from the manifest are user-owned.

## States of a GeneratorOwned file

Before a `GeneratorOwned` file is written it resolves into exactly one state, computed with the frozen hash for the whole aggregate before the first write:

| State | Meaning | Default action |
| --- | --- | --- |
| new | No manifest entry and no file on disk | Written and recorded |
| `verified` | Current hash equals the recorded hash | Overwritten |
| `drifted` | Current hash differs from the recorded hash | Protected (`LC2001`), run fails with exit code 7 |
| `unverified` | The file exists but no trustworthy baseline is recorded (no entry, `Unknown` hash after migration, v1-only marker) | Identical regenerated body: adopted (`LC2008`); different: protected (`LC2002`), exit code 7 |
| `missing` | Recorded but absent from disk | Regenerated, not reported as drift |

Resolutions: `--resolution protect` (default), `--resolution force` (`LC2006` per overwrite), `--resolution skip-modified` (`LC2007` per skip). No resolution reaches `GeneratedOnce` or `Manual` files. See [Generated code policy](../generated-code-policy.md) and [Content hash](hash.md).
