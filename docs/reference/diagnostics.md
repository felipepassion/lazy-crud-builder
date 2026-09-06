# Diagnostics

Every failure, protection decision and notable event in the generation path is a structured diagnostic with a stable code. Codes never change meaning; new codes are appended. Tests, the harness, the CLI and the Visual Studio extension assert on codes, never on message text.

- Fixture: `diagnosticCodes` in `contracts/fixtures/v1/fixture-index.json` (every code with its description)
- When prose and fixture disagree, the fixture wins.

## Structure

| Field | Meaning |
| --- | --- |
| `code` | Stable code, `LCnnnn` |
| `severity` | `information`, `warning` or `error`. A run fails if and only if at least one error was emitted. |
| `path` | The affected file (solution-relative, forward slashes, when known) or item |
| `message` | What happened and what to do next |
| `timestampUtc` | When it was emitted |
| `data` | Structured values: for example `templateId`, `expectedHash`, `actualHash`, `regeneratedHash`, `previousHash`, `resolutions`, `requirement`, `searched`, `found`, `component`, `packageId`, `version`, `probed` |

Diagnostics reach three sinks at once: the in-memory collector of the run (available as structured data to tests and to the report), the host (console or IDE), and the machine-readable file `{solutionRoot}/.lazycrud/diagnostics/{Aggregate}.diagnostics.json`, a JSON array the shared writer appends to from inside the T4 host and the runner clears before each run. A failure to write that file is itself reported (`LC1010`) and never masks the original diagnostic. The `generate` report (`--report <file>`) includes every diagnostic of the run under `Diagnostics` with `Code`, `Severity`, `Path`, `Message` and `Data`.

Exit codes derive from diagnostics: an error `LC2012` exits 9 (`GenerationDefect`) and is checked first, because a collapsed output is a defective transformation and not a conflict with the developer; an error `LC2001` or `LC2002` exits 7 (`OwnershipConflict`); `LC4001` exits 3 (`PrerequisiteUnavailable`); `LC5001` corresponds to 6 (`ContractMismatch`); any other error exits 1 (`AssertionFailure`).

## Codes

### 1xxx: I/O, manifest and migration

| Code | Severity | Meaning | Remedy |
| --- | --- | --- | --- |
| `LC1001` | error | Writing a generated file failed (locked, unwritable, invalid path). The file is not reported as generated. | Close the program locking the file or fix the permissions, then rerun. |
| `LC1002` | error | Creating the output directory failed. | Check the path and permissions. |
| `LC1003` | error | A manifest exists but cannot be read or parsed. Distinguishable from "no manifest present". No generated file is touched. | Repair the JSON or delete the manifest to start over. |
| `LC1004` | error | Saving (or deleting) the manifest failed; the run must not report success. | Make `.lazycrud` writable and rerun. |
| `LC1005` | warning | Manifests for one aggregate exist under more than one root (the solution root and `src/`); the solution root was chosen. | Delete the stale manifest under the other root. |
| `LC1006` | warning | A v1 manifest entry could not be expressed relative to the solution root; retained as unresolved, no path guessed. | Regenerate to re-record the file, or delete the stale entry. |
| `LC1007` | warning | A v1 manifest entry had no content hash; recorded as unknown, never treated as a match. | Nothing; the next generation resolves it as unverified and adopts it when identical. |
| `LC1008` | warning | A v1 manifest entry had no ownership and no registered template produces it; ownership recorded as `Unknown`, file treated as user-owned. | Regenerate so a registered template re-records it. |
| `LC1009` | information | A v1 manifest was migrated in memory (counts of entries, unresolved, without hash, unknown ownership). | Nothing; it is rewritten as v2 after the next successful generation. |
| `LC1010` | warning | The machine-readable diagnostics file could not be written; diagnostics were still reported to the host. | Make `.lazycrud/diagnostics` writable. |

### 2xxx: ownership, drift and protection

| Code | Severity | Meaning | Remedy |
| --- | --- | --- | --- |
| `LC2001` | error | A `GeneratorOwned` file was modified outside the generator (hash mismatch) and was not overwritten. Data: `templateId`, `expectedHash`, `actualHash`, `regeneratedHash`, `resolutions`. | Move the edit to its proper place and rerun with `--resolution force`, or keep it with `--resolution skip-modified`. |
| `LC2002` | error | A `GeneratorOwned` file has no trustworthy baseline and its content differs from the regenerated body; the difference cannot be attributed, so the file was not overwritten. Distinct from drift on purpose. | Review the diff; `--resolution force` or `--resolution skip-modified`. |
| `LC2003` | warning | A manifest entry has unknown ownership; the file is treated as user-owned and protected. | Regenerate so a registered template re-records it. |
| `LC2004` | warning | A template with no registry entry produced output; treated as user-owned (created only when absent). | Register the template in the descriptor registry. |
| `LC2005` | information | A `GeneratedOnce` file already exists and was left unchanged. | Nothing; delete the file to scaffold it again. |
| `LC2006` | warning | A drifted or unverified file was overwritten because the force flag was supplied; the previous content is lost. Data: `previousHash`. | Confirm this was intended. |
| `LC2007` | warning | A drifted or unverified file was left untouched because the skip-modified flag was supplied; the regenerated content was not written. | Move the edit later and force once. |
| `LC2008` | information | An unverified file whose regenerated body matched the on-disk body adopted its baseline without a destructive write (only the header is rewritten when missing or v1). | Nothing. |
| `LC2009` | warning | The marker is missing or malformed but the body matches the recorded hash; the file is regenerated with a valid marker. Not content drift. | Nothing. |
| `LC2010` | information | A path classified `Manual` was requested for writing and refused. | Nothing; `Manual` paths are never written. |
| `LC2012` | error | A regenerated body carries no content (empty, whitespace only, or nothing but comments and import directives) while the file it would replace carries some; the write was refused and nothing was lost. This is the signature of a transformation that lost its model and emitted only the template's static preamble. `data`: `templateId`, `layer`, `aggregate`, `sourceTemplate`, `reason` (`empty`, `whitespace-only`, `no-significant-content`), `rule`, `currentHash`, `regeneratedHash`, `currentSignificantLines`, `regeneratedSignificantLines`, `regeneratedBodyLength`, `resolutions` (`none`). | Rerun the generation; the same run usually produces the full body. If it repeats, the template or the model it resolves is at fault. No resolution flag overrides the refusal -- `--force` resolves an ownership conflict, and this is not one. A template whose correct output is directives only declares `emptyOutputAllowed` in the registry instead. |
| `LC2011` | information | A `GeneratorOwned` file has no trustworthy baseline (for example it carries only a v1 marker); recognized as generated but not authorized for overwrite, resolved by content comparison when regenerated. | Nothing until `LC2002` or `LC2008` follows. |

### 3xxx: templates, settings and model resolution

| Code | Severity | Meaning | Remedy |
| --- | --- | --- | --- |
| `LC3001` | error | The configured template source could not be resolved: the package is not restored (every probed location is listed), no local tree containing `project.tt` was found, or local mode was requested while `DisableLocalTemplateFallback` is `True`. Package resolution never falls back to a local tree. | Restore with a feed containing `Lazy.Crud.Builder.Templates` at the configured version; check `TemplatesNuGetPackageId`, `TemplatesNuGetVersion`, `TemplatesLocalProjectFilesPath`. |
| `LC3002` | information | Local template development mode is in use (explicitly selected); templates resolve from a source tree. | Nothing; expected outside package mode. |
| `LC3003` | warning | A deprecated templates package id (`Lazy.Crud.Templates`) was resolved to the canonical `Lazy.Crud.Builder.Templates`. | Update `TemplatesNuGetPackageId`. |
| `LC3004` | reserved | `lazy.settings` could not be located from the template location. Defined in the code table for template hosts; not emitted by the 1.1.0 runner or writer (the runner falls back to defaults when the file is absent). | Place `lazy.settings` at `src/Mods/lazy.settings` or `Mods/lazy.settings` under the solution root. |
| `LC3005` | warning | A project or type in the model could not be resolved (tolerated): for example no entity file found after Domain phase 1, so Domain phase 2 generates nothing. | Check the entity folder `Aggregates/{Aggregate}Agg/Entities` and the `EndpointsT4` attributes. |
| `LC3006` | information | The resolved template source (kind, location, version) for the run. | Nothing. |

### 4xxx: toolchain and transformation

| Code | Severity | Meaning | Remedy |
| --- | --- | --- | --- |
| `LC4001` | error | The Visual Studio T4 toolchain (or the .NET SDK) could not be resolved; nothing was transformed. Data: `requirement`, `searched`, `found`, `component`. Exit code 3. | Install Visual Studio 2022 (17.x) or 2026 (18.x) on any channel with `Microsoft.VisualStudio.Component.TextTemplating`, or set `LAZYCRUD_VS_INSTANCE` / `--vs-instance`. Never substitute a plain build. |
| `LC4002` | information | The resolved toolchain: instance, version, channel and T4 tooling path. | Nothing. |
| `LC4003` | information | An explicit toolchain override was applied (validated, not trusted blindly). | Nothing. |
| `LC4004` | error | A transformation step produced none of its expected outputs (after the configured attempts and `WaitTimeoutMs`); the run does not report output it did not produce. | Inspect the Error List output in the report; raise `WaitTimeoutMs` on slow machines; confirm the wrapper is a project item with the `TextTemplatingFileGenerator` custom tool. |
| `LC4005` | error (warning for a failed pre-restore) | The T4 engine reported an error while transforming a template, the solution could not be opened, the aggregate directory or Domain project is missing, a wrapper does not exist, or `dotnet restore` failed. | Fix the template or model error quoted in the message; the Domain layer is required first. |

### 5xxx: contracts and descriptor

| Code | Severity | Meaning | Remedy |
| --- | --- | --- | --- |
| `LC5001` | error | The descriptor `contractVersion` is outside the supported range (above the consumer's maximum, or below the minimum upgradable). Exit code 6. | Update the tool, or upgrade the platform packages, as the message states. |
| `LC5002` | warning | Installed packages embed descriptors that disagree; the highest `platformVersion` is canonical. Data: each source with its version. | Align the package versions. |
| `LC5003` | error | No platform descriptor could be resolved, or a copy could not be parsed. | Install a `Lazy.Crud.Builder.*` package of version 1.1.0 or later; reinstall the package. |
| `LC5004` | information | A capability was disabled by configuration (`GeneratedCodeMarkers=False`): the file is written without a marker and hashed as a whole body. | Nothing; re-enable the setting to get markers back. |
| `LC5005` | reserved | A manifest entry references a `templateId` absent from the registry. Defined in the code table; not emitted by the 1.1.0 runner or writer. | Regenerate so the entry is re-recorded by a registered template. |

## Reading the diagnostics of a run

```powershell
Get-Content .lazycrud\diagnostics\Products.diagnostics.json | ConvertFrom-Json | Sort-Object severity -Descending | Format-Table code, severity, path
```

See [Troubleshooting](../troubleshooting.md) for the codes most often met and the reasoning behind their remedies.
