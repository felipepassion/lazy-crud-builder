# Troubleshooting

Every problem the generator can report has a stable code (`LCnnnn`), a severity and a message that says what to do next. This page explains where to find the diagnostics and what the common codes mean. The complete table is in [Diagnostics](reference/diagnostics.md); exit codes are listed at the end.

## Where to look

1. **The run report.** `generate --report <file>` writes a JSON document (also printed on stdout) with `ExitCode`, `ExitCodeName`, `Succeeded`, `TemplateSource` (kind, location, version, requested package id), `Toolchain` (the resolved instance, version, channel, `textTransformPath`, candidates, rejections, discovery log), `PreCheck` (the state of every `GeneratorOwned` file before the first write), `Pipeline` (every step with phase, name, attempts and duration), `TransformedTemplates`, `Manifest` (path, schema version, generator version, contract version, file count) and `Diagnostics`.
2. **The diagnostics file.** `.lazycrud/diagnostics/{Aggregate}.diagnostics.json` is a JSON array written by the shared writer from inside the T4 host and cleared by the runner before each run. Each entry has `code`, `severity` (`information`, `warning`, `error`), `path` (solution-relative, forward slashes), `message`, `timestampUtc` and `data` (structured values such as `templateId`, `expectedHash`, `actualHash`, `regeneratedHash`, `resolutions`). A run fails if and only if at least one `error` was emitted.
3. **stderr.** The runner logs discovery, session and pipeline steps as it goes and ends with `generate: <ExitCodeName> (<code>) in <seconds>s`.
4. **The Visual Studio Error List** is read by the runner after each transformation; T4 engine errors surface as `LC4005` with the engine message.

To read the diagnostics of the last run in PowerShell:

```powershell
Get-Content .lazycrud\diagnostics\Products.diagnostics.json | ConvertFrom-Json | Where-Object severity -eq error | Format-List code, path, message
```

## LC4001: prerequisite unavailable (exit 3)

**Meaning.** No Visual Studio installation with the T4 tooling could be resolved, so nothing was transformed and nothing was written. The message states the requirement (Visual Studio 2022 17.x or 2026 18.x with `Microsoft.VisualStudio.Component.TextTemplating`), what was searched (the discovery log: the override, the `vswhere` query at `%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe`, the Setup Configuration API), what was found (every candidate with its rejection reason, for example `component Microsoft.VisualStudio.Component.TextTemplating not installed` or `Common7/IDE/TextTransform.exe missing`) and the remedy. The `data` of the diagnostic carries `requirement`, `searched`, `found` and `component`.

**Remedy.**

- Install Visual Studio 2022 (17.x) or Visual Studio 2026 (18.x), on any channel including preview and Insiders, and add the **Text Template Transformation** individual component (`Microsoft.VisualStudio.Component.TextTemplating`). Repair the installation if `devenv.exe` or `Common7/IDE/TextTransform.exe` is reported missing.
- If the installation exists but discovery does not see it (for example a non-standard installer location), set `LAZYCRUD_VS_INSTANCE` to the installation path (the folder that contains `Common7\IDE\devenv.exe`) or pass `--vs-instance <path>`. The override is validated and reported as `LC4003`.
- If `LAZYCRUD_VS_SIMULATE_UNAVAILABLE` is set in the environment, discovery is bypassed on purpose (the harness uses it for scenario T2); unset it.
- Do not replace the transformation with `dotnet build`: a plain build never runs the templates. Run `toolchain` to see the resolution report:

```powershell
dotnet Lazy.Crud.Builder.Platform.Tool.dll toolchain
```

## LC2001: drift (exit 7)

**Meaning.** A `GeneratorOwned` file was modified outside the generator: its current content hash differs from the hash recorded in the manifest. The file was not overwritten. The diagnostic names the relative path, the `templateId`, the expected and actual hashes (and, from the writer, the regenerated hash) and the resolutions. When the whole-aggregate pre-check finds drift, the run stops before the first write, so every drifted file of the aggregate is listed.

**Remedy.** Decide per file:

- Move the change to its proper place (a partial class, the seed, the model or the template, see [Safe customization](customization.md)), then regenerate with `--resolution force` to overwrite the drifted file (`LC2006` reports each overwrite).
- Keep the file for now with `--resolution skip-modified` (`LC2007`); the rest of the aggregate is generated and the run succeeds.

The default never destroys the edit; the diff between the file and the regenerated content is what you need to review.

## LC2002: unverified (exit 7)

**Meaning.** A `GeneratorOwned` file exists but has no trustworthy baseline hash (typically after a v1 manifest migration, or a file that predates the manifest), and its content differs from the regenerated body. The difference cannot be attributed to you or to a template change, so the file was protected. The code is distinct from drift on purpose. When the content is identical the file is adopted silently instead (`LC2008`).

**Remedy.** Review the diff, then `--resolution force` to take the regenerated content or `--resolution skip-modified` to keep the file. Once resolved, the manifest records a real baseline and the file behaves as verified or drifted from then on.

## LC2012: the regenerated output collapsed (exit 9)

**Meaning.** The transformation produced a body with nothing in it -- empty, whitespace only, or nothing
but comments and import directives -- while the file it would have replaced carries real content. The
write was refused and **nothing was lost**. This is not a conflict with an edit of yours: it is the
signature of a template that lost the model it resolves and emitted only its static preamble. The
diagnostic names the file, `templateId`, `layer`, `aggregate`, the reason, both hashes and how many
significant lines each side has.

**Remedy.** Rerun the generation. In the run this rule was written for, the very next run reproduced
the full body byte for byte. If it repeats for the same template, the fault is in that template or in
the model it resolves -- open the aggregate in Visual Studio and check that the Domain entities the
template iterates are present and compile. `--force` does **not** override this refusal, by design: it
resolves an ownership conflict, and a defective transformation is not one. If a template you own is
*meant* to emit only directives (a global-usings file), declare `emptyOutputAllowed` for it in the
platform template registry rather than working around the guard.

## LC3001: template resolution failed

**Meaning.** The configured template source could not be resolved. The message names the configured source, the package id requested (and the canonical id it maps to), the version, and every location probed: the `globalPackagesFolder` of the solution's `nuget.config`, `NUGET_PACKAGES`, then `%USERPROFILE%\.nuget\packages`, each under `<packageid>/<version>/contentFiles/any/any/LazyCrudTemplates/project.tt`. Package resolution never falls back to a local source tree. The same code is used when local mode is configured but no tree containing `project.tt` was found, and when local mode is requested while `DisableLocalTemplateFallback` is `True` (clean room).

**Remedy.** Restore the solution against a feed that contains `Lazy.Crud.Builder.Templates` at the version in `TemplatesNuGetVersion` (or leave the version empty to take the highest restored one); check `TemplatesNuGetPackageId`; in local mode check `TemplatesLocalProjectFilesPath` or pass `--templates local:<dir>`.

## LC1003 and LC1004: manifest unreadable or not saved

**LC1003.** A manifest file exists at `.lazycrud/manifests/{Aggregate}.manifest.json` but cannot be read or parsed. The run stops (exit 1) and no generated file is touched. Repair the JSON (it is either a v2 document with `schemaVersion` 2 or a v1 document) or delete the manifest to start over; after deletion every existing generated file is `unverified` on the next run and is adopted when identical.

**LC1004.** The manifest could not be written (typically `.lazycrud` is read-only or locked). The run is not reported as successful. Make the directory writable, close programs locking the file, rerun.

## LC3003: deprecated templates package id

**Meaning.** `TemplatesNuGetPackageId` names `Lazy.Crud.Templates`, which is a deprecated dependency-only alias. Resolution continues against the canonical `Lazy.Crud.Builder.Templates`.

**Remedy.** Change the setting to `Lazy.Crud.Builder.Templates`. The alias is removed two minor releases after 1.1.0.

## LC5002: descriptor conflict

**Meaning.** The installed packages embed platform descriptors with different `platformVersion` values (mixed package versions). The highest platform version is canonical; the diagnostic names every version and the package it came from.

**Remedy.** Align the package versions. The only supported mixed pins are the three packages listed in the [compatibility matrix](compatibility.md).

## Other codes you may meet

| Code | Meaning | Remedy |
| --- | --- | --- |
| `LC1005` | Manifests for the aggregate exist under both the solution root and `src/`; the solution root was chosen | Delete the stale manifest under `src/.lazycrud/manifests/` |
| `LC1006`, `LC1007`, `LC1008`, `LC1009` | v1 manifest migration: unresolved path, unknown hash, unknown ownership, migration performed | Regenerate; unresolved entries are re-recorded or can be deleted |
| `LC2003` | A manifest entry has unknown ownership; the file is treated as user-owned | Regenerate so the registered template re-records it |
| `LC2004` | A template without a registry entry produced output; treated as user-owned | Register the template in the descriptor registry (forks) |
| `LC2009` | The body matches but the marker is missing or malformed; regenerated with a valid header | Nothing; this is not drift |
| `LC3002`, `LC3006` | Local development mode in use; the resolved template source | Informational |
| `LC4004` | A transformation produced none of its expected outputs within `WaitTimeoutMs` (default 5000 ms, 2 attempts) | Check the Error List output in the report, raise `WaitTimeoutMs` on slow machines, make sure the wrapper is a project item with the `TextTemplatingFileGenerator` custom tool |
| `LC4005` | The T4 engine reported an error, the solution could not be opened, or the aggregate has no Domain project | Fix the template or model error quoted in the message; the Domain layer is required first |
| `LC5001` | The descriptor `contractVersion` is outside the supported range | Update the tool or upgrade the packages, as the message says |
| `LC5003` | No platform descriptor could be resolved from the installed packages | Install a `Lazy.Crud.Builder.*` package of version 1.1.0 or later |
| `LC5004` | A capability was disabled by configuration (`GeneratedCodeMarkers=False`) | Informational |
| `LC1001`, `LC1002` | A generated file or its directory could not be written | Close the program locking the file, fix permissions, rerun |

## Exit codes

| Code | Name | Meaning |
| --- | --- | --- |
| 0 | `Success` | The operation or every required scenario succeeded |
| 1 | `AssertionFailure` | An assertion did not hold or the operation failed (any error diagnostic other than an ownership conflict) |
| 2 | `UsageError` | Invalid command line or missing argument |
| 3 | `PrerequisiteUnavailable` | The Visual Studio T4 toolchain, the .NET SDK or the environment is unavailable |
| 4 | `InvalidFixtureOrSandbox` | A fixture could not be loaded or a sandbox could not be created (also: no `.sln` under the solution root) |
| 5 | `Timeout` | A scenario or the run exceeded its timeout budget |
| 6 | `ContractMismatch` | A descriptor `contractVersion` is outside the supported range, or versions disagree |
| 7 | `OwnershipConflict` | Drifted, unverified or unknown-ownership files require an explicit resolution |
| 8 | `HarnessFailure` | The tool or the harness itself failed (internal error) |
| 9 | `GenerationDefect` | Generation produced an output that carries no content where the existing file has some (`LC2012`); nothing was overwritten |

The table is shared with the extension repository's CLI and harness: a code means the same thing everywhere.
