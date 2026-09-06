# Release guidance

A release of Lazy CRUD is one version for every package, one set of contract fixtures, a changelog entry and a publish from the environment. This page is the procedure and the rules behind it.

## Single version source

`LazyCrudVersion` in `Directory.Build.props` is the only place the version is written. Every package, the platform descriptor (`platformVersion`), the marker headers (`GeneratorVersion`, `TemplateVersion`) and the manifests (`generatorVersion`) derive from it. A project that declares its own `Version` or `PackageVersion` fails the build (`LazyCrudValidateVersion` in `Directory.Build.targets`) and `validate tree` checks `version.single-source` and `version.central-defined`. For a local experiment override it on the command line: `dotnet pack -p:LazyCrudVersion=1.2.3`.

## Procedure

1. **Set the version.** Edit `LazyCrudVersion` in `Directory.Build.props`.
2. **Write the changelog.** Add the release section to `CHANGELOG.md` (rules below).
3. **Scan for credentials.**

```powershell
pwsh scripts/credential-scan.ps1
```

4. **Build, validate and pack.** `pack-all.ps1` restores, builds the whole solution in Release (which regenerates the descriptor and the fixtures through the `LazyCrudGenerateDescriptor` target), runs `validate tree`, packs every project listed in it into `artifacts/nuget/<version>/`, then runs `validate packages` on the produced set. Any non-zero exit code fails the run.

```powershell
pwsh pack-all.ps1
```

5. **Run the unit tests.**

```powershell
dotnet test src\Platform\Lazy.Crud.Builder.Platform.Tests
```

6. **Run the gates that transform templates** (a machine meeting the host prerequisite is required): the golden-output test (`tests/golden/README.md`) and the E2E harness, which includes the clean room as scenario C1.

```powershell
pwsh scripts/e2e.ps1
```

The release is blocked unless the harness ends with `RESULT: OK (passed=N failed=0)` and exit code 0.

7. **Publish.** The key is read only from `-ApiKey` or the `NUGET_API_KEY` environment variable; both scripts fail with a clear message (exit 2) when neither is present. Nothing in the repository holds a credential.

```powershell
$env:NUGET_API_KEY = '<key from your secret store>'
pwsh pack-all.ps1 -Publish
```

Then prove the published artifact from the internet, not from the pack output:

```powershell
pwsh scripts/public-clean-room.ps1
```

or, to push the latest version folder already produced:

```powershell
pwsh publish-all.ps1
```

`publish-all.ps1` picks the highest version folder under `artifacts/nuget`, pushes every `.nupkg` with `--skip-duplicate` (disable with `-NoSkipDuplicate`) to `https://api.nuget.org/v3/index.json` (override with `-Source`), and prints a summary.

8. **Write the release record.** One machine-readable file per version, `contracts/releases/<version>.json`, is what a consuming repository reads instead of a changelog: the version, `platformVersion`, `contractVersion`, the schema versions, the templates package and its aliases, every produced `.nupkg` with its SHA-256, the commit that produced them, and -- queried against the public source, never assumed -- whether each package id actually answers for that version there.

```powershell
pwsh scripts/release-record.ps1
```

`status` is `published` only when every declared package is on the source; otherwise it is `prepared`. `dotnet nuget push --skip-duplicate` exiting zero is not evidence of publication, and the record never treats it as such. Run it again after publishing to flip the status, and commit the result.

9. **Tag and record.** Tag the commit with the version and move the changelog section from Unreleased to released.

## Validation gates

| Gate | Command | What it proves |
| --- | --- | --- |
| Tree | `validate tree --repo <repo>` (run by `pack-all.ps1`) | Registry and shipped templates agree both ways; every writer call site declares ownership and `templateId` matching the registry; no silent catch in the writer; single version source; dead glob removed; shared templates only in the templates package; attribute metadata contract; no credential literal; required documentation pages present and consistent with the descriptor; committed fixtures equal the build |
| Packages | `validate packages --repo <repo> --packages artifacts/nuget/<version>` (run by `pack-all.ps1`) | Every package the descriptor declares was produced, template paths exist inside the packages named by the registry, no content-file collision, the alias depends on the canonical package |
| Fixtures | `validate fixtures --repo <repo>` | Committed fixtures equal what the implementation produces and validate against their schemas |
| Unit tests | `dotnet test` | Contracts library and runner behavior |
| Golden | `tests/golden/README.md` | Byte-identical reference output apart from the header region |
| Harness | `pwsh scripts/e2e.ps1` | Positive, compatibility, failure, toolchain and clean-room scenarios end to end |
| Clean room | scenario C1 of the harness | The public artifact works exactly as an external user receives it |
| Public clean room | `pwsh scripts/public-clean-room.ps1` (after publishing) | The packages restored from nuget.org, with no local source and an isolated cache, generate through the real toolchain and are byte-identical to what was built |

## Security

`pack-all.ps1` used to carry a live nuget.org publish key as a default parameter value. The defaults are gone: keys are read only from `-ApiKey` or `NUGET_API_KEY`, `scripts/credential-scan.ps1` (and the `security.no-credential-literal` check of `validate tree`) fail on any credential literal, and `-SelfTest` proves the scan detects a planted key. Rotating the exposed keys on nuget.org and storing the replacement as the `NUGET_API_KEY` CI secret is an external owner action, recorded in `CHANGELOG.md`.

## The alias deprecation timeline

`Lazy.Crud.Templates` is published from 1.1.0 as a dependency-only alias of `Lazy.Crud.Builder.Templates` so that configurations naming the historical id keep resolving (`LC3003` reports the deprecation). It is scheduled for removal two minor releases after 1.1.0. Until then it is packed and published with every release, listed in the descriptor under `templates.aliasPackageIds` and `nonLayerPackages`, and verified by harness scenario R2. The release that removes it drops it from `pack-all.ps1`, from `DescriptorBuilder.NonLayerPackages` and from the descriptor's alias list, and records the removal as breaking in the changelog.

## contractVersion rules

The shared contracts (descriptor, manifest v2, marker v2, ownership, hash, diagnostics codes, exit codes) are frozen under `contractVersion` 1 (`PlatformInfo.ContractVersion`).

- **Increment** `contractVersion` when a field is removed or its meaning changes in a way that breaks a conforming consumer, when a hash rule or scope changes, when an ownership or marker value changes, or when a diagnostic or exit code changes meaning. Record the change in the changelog under a contract-breaking heading naming the new `contractVersion`, publish the new fixtures under `contracts/fixtures/v<N>/` and `contracts/schemas/v<N>/` while keeping the previous directories retrievable, update `supportedContractVersionMin`, `supportedContractVersionMax` and `minimumUpgradableContractVersion` in `PlatformInfo`, and update the [compatibility matrix](compatibility.md) with the consumer versions that accept the new contract.
- **Do not increment** for additive changes: a new optional field (for example `compatibility.hostRequirements`), a new diagnostic code appended to the table, a new template registered with a new id, a new attribute. Consumers must ignore unknown fields.
- A published fixture is never changed silently. If a frozen contract must change, it is superseded by a new `contractVersion`.
- Renaming or removing a public attribute is a breaking change of the attribute contract: record it in the changelog, and the fixtures (descriptor and attribute catalog) are regenerated.

## Fixture regeneration

The fixtures are produced by the implementation, never edited by hand. Building the solution runs `descriptor generate --repo . --version <LazyCrudVersion> --attributes-assembly <Application.DTO dll> --output artifacts/platform/lazycrud.platform.json --fixtures contracts`, which writes the descriptor and the fixture set under `contracts/fixtures/v<contractVersion>/`. `validate tree` (`fixtures.current`) and `validate fixtures` fail when a committed fixture differs from what the build produces, so after any change to the registry, the attributes, the marker, the hash or the manifest:

1. Build the solution (Release).
2. Review the diff under `contracts/`.
3. Commit the regenerated fixtures together with the change, with the reason in the commit message.

The fixtures ship inside `Lazy.Crud.Builder.Templates` under `contracts/`, are retrievable raw from GitHub, and are published as the CI artifact `lazycrud-contract-fixtures`.

## Changelog rules

`CHANGELOG.md` records every notable change against the platform version, under the headings Security, Added, Changed, Deprecated, Removed and Fixed as needed. Contract-breaking changes are recorded against the `contractVersion` they change and are labeled breaking. Dead-configuration removals that cannot affect any consumer are still recorded (as 1.1.0 records the removal of the `contentFiles/any/any/T4/` glob). External prerequisites that a release depends on but that the implementer cannot perform (key rotation, a consumer confirming its dependency shape) are recorded as such, with the owner.
