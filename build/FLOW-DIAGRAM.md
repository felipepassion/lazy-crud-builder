# E2E Flow Diagram

## Visual Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                    E2E PRODUCTS FLOW                            │
│                   .\build\e2e-products.ps1                      │
└─────────────────────────────────────────────────────────────────┘

  ┌─────────────────────────────────────────────────────────┐
  │ STEP 1: Configure lazy.settings                         │
  │ ────────────────────────────────────────────────────    │
  │ • TemplatesSource = Local                               │
  │ • UseNugetTemplates = False                             │
  │ • TemplatesLocalProjectFilesPath = src/ProjectFiles     │
  └────────────────────┬────────────────────────────────────┘
                       │
                       ▼
  ┌─────────────────────────────────────────────────────────┐
  │ STEP 2: Delete Products Aggregate                       │
  │ ────────────────────────────────────────────────────    │
  │ • Remove src/Products/ directory                        │
  │ • Remove projects from .sln                             │
  │ • Clean slate for fresh start                           │
  └────────────────────┬────────────────────────────────────┘
                       │
                       ▼
  ┌─────────────────────────────────────────────────────────┐
  │ STEP 3: Create Directory Structure                      │
  │ ────────────────────────────────────────────────────    │
  │ src/Products/                                           │
  │   ├── Products.Domain/                                  │
  │   ├── Products.Enumerations/                            │
  │   ├── Products.Application.DTO/                         │
  │   ├── Products.Application/                             │
  │   ├── Products.Infra.Data/                              │
  │   ├── Products.Infra.IoC/                               │
  │   ├── Products.Api/                                     │
  │   ├── Products.Api.Queries/                             │
  │   └── Tests/                                            │
  │       ├── Lazy.Crud.Products.Tests.Domain/              │
  │       ├── Lazy.Crud.Products.Tests.Enumerations/        │
  │       ├── Lazy.Crud.Products.Tests.DTO/                 │
  │       ├── Lazy.Crud.Products.Tests.Application/         │
  │       ├── Lazy.Crud.Products.Tests.InfraData/           │
  │       ├── Lazy.Crud.Products.Tests.InfraIoC/            │
  │       ├── Lazy.Crud.Products.Tests.Api/                 │
  │       └── Lazy.Crud.Products.Tests.ApiQueries/          │
  └────────────────────┬────────────────────────────────────┘
                       │
                       ▼
  ┌─────────────────────────────────────────────────────────┐
  │ STEP 3.5: Create .csproj Files                          │
  │ ────────────────────────────────────────────────────    │
  │ Calls: .\build\create-csproj-files.ps1                  │
  │                                                          │
  │ • Generates .csproj for each layer                      │
  │ • Adds PackageReferences (MediatR, EF Core, etc.)       │
  │ • Sets up ProjectReferences (layer dependencies)        │
  │ • Configures T4 transformation                          │
  └────────────────────┬────────────────────────────────────┘
                       │
                       ▼
  ┌─────────────────────────────────────────────────────────┐
  │ STEP 3.6: Add Projects to Solution                      │
  │ ────────────────────────────────────────────────────    │
  │ • dotnet sln add src/Products/**/*.csproj               │
  │ • All 16 projects added to .sln                         │
  └────────────────────┬────────────────────────────────────┘
                       │
                       ▼
  ┌─────────────────────────────────────────────────────────┐
  │ STEP 4: Create T4 Wrapper Files                         │
  │ ────────────────────────────────────────────────────    │
  │ Example: Products.Domain/DomainTest.tt                  │
  │ Content: <#@ include file="../../ProjectFiles/..." #>  │
  │                                                          │
  │ Points to local templates in src/ProjectFiles/          │
  └────────────────────┬────────────────────────────────────┘
                       │
                       ▼
  ┌─────────────────────────────────────────────────────────┐
  │ STEP 5: Restore NuGet Packages                          │
  │ ────────────────────────────────────────────────────    │
  │ • dotnet restore                                        │
  └────────────────────┬────────────────────────────────────┘
                       │
                       ▼
  ┌─────────────────────────────────────────────────────────┐
  │ STEP 6: Run T4 Templates                                │
  │ ────────────────────────────────────────────────────    │
  │ Calls: .\build\run-t4-templates.ps1                     │
  │                                                          │
  │ Execution order (respects dependencies):                │
  │ 1. Enumerations                                         │
  │ 2. Domain                                               │
  │ 3. Application.DTO                                      │
  │ 4. Application                                          │
  │ 5. Infra.Data                                           │
  │ 6. Infra.IoC                                            │
  │ 7. Api.Queries                                          │
  │ 8. Api                                                  │
  │ 9. Tests.* (all test layers)                            │
  │                                                          │
  │ Output: *.generated.cs files                            │
  └────────────────────┬────────────────────────────────────┘
                       │
                       ▼
  ┌─────────────────────────────────────────────────────────┐
  │ STEP 7: Build                                           │
  │ ────────────────────────────────────────────────────    │
  │ • dotnet build --no-restore                             │
  │ • Validates generated code compiles                     │
  └────────────────────┬────────────────────────────────────┘
                       │
                       ▼
  ┌─────────────────────────────────────────────────────────┐
  │ STEP 8: Test                                            │
  │ ────────────────────────────────────────────────────    │
  │ • dotnet test --no-build                                │
  │ • Runs all generated tests                              │
  └────────────────────┬────────────────────────────────────┘
                       │
                       ▼
  ┌─────────────────────────────────────────────────────────┐
  │                   ✅ SUCCESS                             │
  │                                                          │
  │ Products aggregate fully recreated and validated!       │
  └─────────────────────────────────────────────────────────┘
```

## Layer Dependency Graph

```
┌───────────────────────────────────────────────────────────────┐
│                    LAYER DEPENDENCIES                         │
└───────────────────────────────────────────────────────────────┘

                    ┌─────────────────┐
                    │  Enumerations   │ ◄─── No dependencies
                    └────────┬────────┘
                             │
                             │ referenced by
                             ▼
                    ┌─────────────────┐
                    │     Domain      │ ◄─── Depends on: Enumerations
                    └────────┬────────┘
                             │
                    ┌────────┴────────┐
                    │                 │
                    ▼                 ▼
          ┌──────────────┐   ┌───────────────┐
          │ Application  │   │  Infra.Data   │
          │     DTO      │   │               │
          └──────┬───────┘   └───────┬───────┘
                 │                   │
                 │ referenced by     │
                 ▼                   │
          ┌──────────────┐           │
          │ Application  │◄──────────┘
          └──────┬───────┘
                 │
        ┌────────┴────────┐
        │                 │
        ▼                 ▼
  ┌───────────┐    ┌──────────────┐
  │ Infra.IoC │    │     API      │
  └───────────┘    │              │
                   ├─ Api         │
                   └─ Api.Queries │
                   └──────────────┘

Tests mirror the production structure:
  Tests.Domain → Domain
  Tests.Application → Application
  Tests.DTO → Application.DTO
  etc.
```

## File Structure After E2E

```
lazy-crud-builder/
│
├── src/
│   │
│   ├── Products/                                  ◄─── Created by E2E
│   │   │
│   │   ├── Products.Domain/
│   │   │   ├── Lazy.Crud.Products.Domain.csproj  ◄─── Generated
│   │   │   ├── DomainTest.tt                     ◄─── T4 wrapper
│   │   │   ├── Generated/                        ◄─── T4 output
│   │   │   │   └── *.generated.cs
│   │   │   └── (other domain files)
│   │   │
│   │   ├── Products.Enumerations/
│   │   │   └── ... (same structure)
│   │   │
│   │   ├── Products.Application.DTO/
│   │   ├── Products.Application/
│   │   ├── Products.Infra.Data/
│   │   ├── Products.Infra.IoC/
│   │   ├── Products.Api/
│   │   ├── Products.Api.Queries/
│   │   │
│   │   └── Tests/
│   │       ├── Lazy.Crud.Products.Tests.Domain/
│   │       │   ├── Lazy.Crud.Products.Tests.Domain.csproj
│   │       │   ├── DomainTest.tt
│   │       │   ├── Generated/
│   │       │   ├── Baseline/
│   │       │   └── Inputs/
│   │       │
│   │       ├── Lazy.Crud.Products.Tests.Enumerations/
│   │       ├── Lazy.Crud.Products.Tests.DTO/
│   │       ├── Lazy.Crud.Products.Tests.Application/
│   │       ├── Lazy.Crud.Products.Tests.InfraData/
│   │       ├── Lazy.Crud.Products.Tests.InfraIoC/
│   │       ├── Lazy.Crud.Products.Tests.Api/
│   │       └── Lazy.Crud.Products.Tests.ApiQueries/
│   │
│   ├── ProjectFiles/                              ◄─── Template source
│   │   ├── Domain/
│   │   │   └── Domain.tt                         ◄─── Actual template
│   │   ├── Application/
│   │   ├── Tests/
│   │   │   └── Domain/
│   │   │       └── Domain.Tests.tt               ◄─── Test template
│   │   └── ...
│   │
│   └── Mods/
│       └── lazy.settings                          ◄─── Updated by E2E
│
├── build/                                         ◄─── E2E Scripts
│   ├── e2e-products.ps1                          ◄─── Main E2E
│   ├── create-csproj-files.ps1
│   ├── run-t4-templates.ps1
│   ├── README.md
│   ├── CLI-SPEC.md
│   ├── IMPLEMENTATION-SUMMARY.md
│   └── VALIDATION-CHECKLIST.md
│
├── QUICKSTART-E2E.md                              ◄─── Quick guide
└── README.md                                      ◄─── Updated
```

## T4 Template Flow

```
┌────────────────────────────────────────────────────────────────┐
│                   T4 TEMPLATE FLOW                             │
└────────────────────────────────────────────────────────────────┘

User runs: .\build\run-t4-templates.ps1 -AggregateName Products

Step 1: Auto-detect T4 tool
  ├─ Check for MSBuild ✓
  ├─ Check for dotnet-t4
  └─ Check for TextTransform.exe

Step 2: Process in dependency order
  │
  ├─ [1] Products.Enumerations/EnumerationsTest.tt
  │   │   <#@ include file="../../ProjectFiles/Enumerations/Enumerations.tt" #>
  │   │
  │   └─► src/ProjectFiles/Enumerations/Enumerations.tt (template logic)
  │       │
  │       └─► Output: Products.Enumerations/Generated/*.generated.cs
  │
  ├─ [2] Products.Domain/DomainTest.tt
  │   │   <#@ include file="../../ProjectFiles/Domain/Domain.tt" #>
  │   │
  │   └─► src/ProjectFiles/Domain/Domain.tt
  │       │
  │       └─► Output: Products.Domain/Generated/*.generated.cs
  │
  ├─ [3-8] Other production layers...
  │
  └─ [9] Tests.Domain/DomainTest.tt
      │   <#@ include file="../../../ProjectFiles/Tests/Domain/Domain.Tests.tt" #>
      │
      └─► src/ProjectFiles/Tests/Domain/Domain.Tests.tt
          │
          └─► Output: Tests.Domain/Generated/*.generated.cs

All layers processed ✓
```

## Success Criteria Flow

```
┌────────────────────────────────────────────────────────────────┐
│                VALIDATION CHECKPOINTS                          │
└────────────────────────────────────────────────────────────────┘

After E2E completes:

✓ lazy.settings configured
  └─ TemplatesSource = Local
  └─ UseNugetTemplates = False

✓ Directory structure exists
  └─ 8 production layers
  └─ 8 test layers

✓ .csproj files created
  └─ 16 total (.csproj files)
  └─ Valid SDK-style projects
  └─ Correct PackageReferences
  └─ Correct ProjectReferences

✓ Projects in solution
  └─ dotnet sln list | grep Products
  └─ All 16 projects listed

✓ T4 wrappers created
  └─ *.tt files exist
  └─ Point to correct ProjectFiles

✓ T4 executed
  └─ *.generated.cs exist
  └─ Contain <auto-generated> marker

✓ Build succeeds
  └─ dotnet build → 0 errors

✓ Tests run
  └─ dotnet test → executes

✓ Idempotent
  └─ Can run E2E again
  └─ Same result
```

---

## Legend

- `◄───` : Reference/dependency
- `▼` : Flow direction
- `✓` : Success checkpoint
- `►` : Transformation/output
- `└─` : Hierarchical relationship

---

**Version:** 1.0  
**Created:** 2024  
**Purpose:** Visual guide for E2E flow understanding
