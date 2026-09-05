# Golden output of the reference aggregate (`Products`)

The golden files under `tests/golden/Products/` are the exact files the generator produces for the
reference aggregate (`tests/fixtures/reference-aggregate`), captured through the real Visual
Studio T4 toolchain. They are the safety net for every generator change: the golden-output
regression test regenerates the aggregate in a temporary sandbox and diffs the result against
these files.

## The command that regenerates the reference aggregate

```powershell
pwsh scripts/golden.ps1 -Capture          # regenerate and re-bless the golden files
pwsh scripts/golden.ps1                   # regenerate and compare (the regression check)
```

`scripts/golden.ps1` does, through `Lazy.Crud.Builder.Platform.Tool`:

1. `sandbox create` — materializes `src/Products/*` layer and test projects in a temporary
   directory from the fixture, in **local template development mode** pointing at this
   repository's `src/ProjectFiles` (so the working-tree templates are what gets exercised), with
   the Lazy CRUD runtime packages restored from the local pack output (`artifacts/nuget/<version>`).
2. `generate --solution <sandbox> --aggregate Products` — the unattended pipeline: restore,
   Domain phase 1 (`DefaultEntities.tt`), entity verification, Domain phase 2
   (`DefaultCommands.tt`), the other layers under the configured throttle, the test layers, output
   wait, post-restore. Transformation runs inside an embedded Visual Studio automation session
   resolved through `vswhere`/the Setup Configuration API.
3. Collects every generated file (everything the manifest records plus every `*.cs`, `*.bat` and
   `*.g.cs` the templates wrote) into `tests/golden/Products/`, normalizing the one
   non-deterministic value the templates emit (the migration timestamp in
   `ProductsAgg.cs` written by `Api/MiniMigrations.tt`).

## How to re-bless

Run `pwsh scripts/golden.ps1 -Capture`, review the diff, and commit the golden files **with an
explicit reason in the commit message** (for example `golden: re-bless after marker v2 header`).
A re-bless without a stated reason is not accepted; the golden diff is the evidence that a
generator change did what it claims and nothing else.

## Comparison rules

- Files are compared after CRLF/LF normalization.
- The leading header region of each file (contiguous comment and blank lines before the first code
  line) is compared separately from the body, so the marker-header change introduced by the
  platform relaunch is reported as a header-only difference; any body difference fails the test.
- The migration timestamp emitted by `MiniMigrations.tt` is masked before comparison.
