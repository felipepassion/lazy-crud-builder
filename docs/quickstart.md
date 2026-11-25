# Quickstart: from an empty solution to a generated, compiling aggregate

This page creates an aggregate in a new solution using only the published packages. Nothing here requires cloning or building the Lazy CRUD repository.

There are two ways through it. The fast path below scaffolds the solution with one command and is the path the E2E harness runs verbatim as scenario P7 on every run of the battery. The numbered walkthrough after it builds the same layout by hand: it is what the scaffold command emits, and it is the layout the platform descriptor declares, so read it when you want to create the projects yourself or to understand what the scaffold produced.

## 0. Prerequisites (before the first generation step)

Generation requires, in every mode including this command-line walkthrough:

- Windows.
- Visual Studio 2022 (17.x) or Visual Studio 2026 (18.x) on any channel (release, preview or Insiders) with the **Text Template Transformation** component (`Microsoft.VisualStudio.Component.TextTemplating`). Install it through the Visual Studio Installer under Individual components. The templates read your entity model through the Visual Studio code model, so the T4 tooling of that installation performs every transformation inside an embedded, unattended Visual Studio session.
- The .NET SDK 11.0.
- PowerShell 7 (`pwsh`) or Windows PowerShell for the commands shown here.

Headless means unattended (no prompt, no window to focus, no clicks); it does not mean Visual Studio is absent. Without the toolchain the runner stops before writing anything with `LC4001` and exit code 3. See [Toolchain](toolchain.md).

## Fast path: scaffold the solution

The platform tool ships inside `Lazy.Crud.Builder.Templates` under `tools/net11.0/`. With the package
downloaded (or from a checkout's `artifacts/nuget/<version>/` feed), one command creates the whole
consumer solution, and a second one generates it:

```powershell
dotnet <tool>\Lazy.Crud.Builder.Platform.Tool.dll scaffold --output Shop --aggregate Shop --version 1.2.0
dotnet <tool>\Lazy.Crud.Builder.Platform.Tool.dll generate --solution Shop --aggregate Shop --report Shop\generate.json
dotnet build Shop\Sandbox.sln
```

`scaffold` writes the layer projects, the wrapper templates, `lazy.settings`, a sample entity model and
the solution file, then restores it. `generate` resolves the Visual Studio T4 toolchain, transforms every
template in the required order and writes the report. Add `--feed <dir>` to either command to restore from
a local folder feed instead of nuget.org.

Scenario P7 of the harness runs exactly these three commands and then asserts the solution builds and the
Domain layer produced output, so this path is verified on every battery run.

## 1. Create the solution and the aggregate folder

```powershell
mkdir Shop
cd Shop
dotnet new sln -n Shop
mkdir src\Products
mkdir src\Mods
```

An aggregate lives under `src/{Aggregate}/`, one project per layer named `{Aggregate}.{Layer}`. This is the canonical consumer layout published in the platform descriptor (`layers[].projectPattern` and `outputPathPattern`).

## 2. Create the layer projects

Create one project directory and `.csproj` per layer under `src/Products/`. Every layer package is referenced with `GeneratePathProperty="true"` so that MSBuild exposes `$(PkgLazy_Crud_Builder_<Layer>)`, the property the wrapper templates use to include the shipped templates.

| Layer | `.csproj` | SDK | Package references (version 1.2.0 unless stated) | Project references | Wrapper templates |
| --- | --- | --- | --- | --- | --- |
| Domain | `Products.Domain/Products.Domain.csproj` | Microsoft.NET.Sdk | `Lazy.Crud.Builder.Domain`, `Lazy.Crud.Builder.Templates`, Microsoft.EntityFrameworkCore.Relational 11.0.0, Serilog 4.4.0 | Application.DTO | `DefaultEntities.tt`, `DefaultCommands.tt` |
| Application | `Products.Application/Products.Application.csproj` | Microsoft.NET.Sdk | `Lazy.Crud.Builder.Application`, Microsoft.EntityFrameworkCore.Relational 11.0.0, Npgsql.EntityFrameworkCore.PostgreSQL 11.0.0, Serilog 4.4.0 | Infra.Data | `DefaultCommands.tt` |
| Application.DTO | `Products.Application.DTO/Products.Application.DTO.csproj` | Microsoft.NET.Sdk | `Lazy.Crud.Builder.Application.DTO`, `Lazy.Crud.CrossCutting.Infra.Utils` | Api.Queries | `DefaultCommands.tt` |
| Infra.Data | `Products.Infra.Data/Products.Infra.Data.csproj` | Microsoft.NET.Sdk | `Lazy.Crud.Builder.Infra.Data`, Microsoft.EntityFrameworkCore.Relational 11.0.0, Npgsql.EntityFrameworkCore.PostgreSQL 11.0.0, Serilog 4.4.0 | Domain | `DefaultCommands.tt` |
| Infra.IoC | `Products.Infra.IoC/Products.Infra.IoC.csproj` | Microsoft.NET.Sdk | `Lazy.Crud.Builder.Infra.IoC`, Microsoft.AspNetCore.Mvc.NewtonsoftJson 11.0.0, Microsoft.Extensions.DependencyInjection.Abstractions 11.0.0, Microsoft.EntityFrameworkCore.Relational 11.0.0, Npgsql.EntityFrameworkCore.PostgreSQL 11.0.0, Serilog 4.4.0 | Application, Infra.Data | `DefaultCommands.tt` |
| Api.Queries | `Products.Api.Queries/Products.Api.Queries.csproj` | Microsoft.NET.Sdk | `Lazy.Crud.Builder.Api.Queries` | Enumerations | `DefaultCommands.tt` |
| Enumerations | `Products.Enumerations/Products.Enumerations.csproj` | Microsoft.NET.Sdk | `Lazy.Crud.Builder.Enumeration` | none | none |
| Api | `Products.Api/Products.Api.csproj` | Microsoft.NET.Sdk.Web | `Lazy.Crud.Builder.Api`, `Lazy.Crud.Builder.Templates`, Microsoft.AspNetCore.Mvc.NewtonsoftJson 11.0.0, Swashbuckle.AspNetCore 7.3.1, Microsoft.EntityFrameworkCore.Design 11.0.0 (PrivateAssets=all), Microsoft.EntityFrameworkCore.Relational 11.0.0, Npgsql.EntityFrameworkCore.PostgreSQL 11.0.0, Serilog 4.4.0 | Infra.IoC | `ServerIoCFactory.tt`, `Migrations.tt`, `Controllers/Controllers.tt` |

The templates package is a transitive dependency of every layer package; the direct reference on Domain and Api keeps `$(PkgLazy_Crud_Builder_Templates)` available to tooling. The Domain project looks like this (the other class-library layers differ only in the package references, the project reference and the wrapper names from the table):

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net11.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>disable</Nullable>
    <NoWarn>$(NoWarn);1701;1702;8618;1998;8602;8625;8629;CS1591;CS8632;CS0108;CS0114</NoWarn>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Lazy.Crud.Builder.Domain" Version="1.2.0" GeneratePathProperty="true" />
    <PackageReference Include="Lazy.Crud.Builder.Templates" Version="1.2.0" GeneratePathProperty="true" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Relational" Version="11.0.0" />
    <PackageReference Include="Serilog" Version="4.4.0" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\Products.Application.DTO\Products.Application.DTO.csproj" />
  </ItemGroup>
  <ItemGroup>
    <None Update="*.tt">
      <Generator>TextTemplatingFileGenerator</Generator>
    </None>
  </ItemGroup>
</Project>
```

The Api project uses the web SDK and additionally registers the controller wrapper:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net11.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>disable</Nullable>
    <NoWarn>$(NoWarn);1701;1702;8618;1998;8602;8625;8629;CS1591;CS8632;CS0108;CS0114</NoWarn>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Lazy.Crud.Builder.Api" Version="1.2.0" GeneratePathProperty="true" />
    <PackageReference Include="Lazy.Crud.Builder.Templates" Version="1.2.0" GeneratePathProperty="true" />
    <PackageReference Include="Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="11.0.0" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="7.3.1" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="11.0.0" PrivateAssets="all" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Relational" Version="11.0.0" />
    <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="11.0.0" />
    <PackageReference Include="Serilog" Version="4.4.0" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\Products.Infra.IoC\Products.Infra.IoC.csproj" />
  </ItemGroup>
  <ItemGroup>
    <None Update="*.tt">
      <Generator>TextTemplatingFileGenerator</Generator>
    </None>
    <None Update="Controllers\Controllers.tt">
      <Generator>TextTemplatingFileGenerator</Generator>
    </None>
  </ItemGroup>
</Project>
```

`Products.Api/Program.cs` wires the generated dependency injection and middleware (the `InjectDependencies` and `OnAppInitialized` extension methods are produced by the Api templates):

```csharp
using Lazy.Crud.Builder.Api.Middlewares;
using Lazy.Crud.Products.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

builder.Services.InjectDependencies(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseForwardedHeaders();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.OnAppInitialized();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();
```

`Products.Enumerations/ProductsEnumerations.cs` holds the aggregate-level enumerations (an enum with a single member is fine to start):

```csharp
namespace Lazy.Crud.Products.Enumerations
{
    public enum ProductsEnumerations
    {
        None = 0,
    }
}
```

## 3. Write the entity model

Entities live in the Domain project under `Aggregates/{Aggregate}Agg/Entities/`. The runner verifies that this folder contains entity files after the first Domain phase. Create `src/Products/Products.Domain/Aggregates/ProductsAgg/Entities/Product.cs`:

```csharp
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Lazy.Crud.Builder.Application.DTO.Attributes;
using Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.Entities;
using Lazy.Crud.Builder.Domain.Attributes.T4;

namespace Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Entities
{
    /// <summary>A product sold by the store.</summary>
    [AggregateSettingsT4, EndpointsT4(EndpointTypes.HttpAll)]
    public class Product : Entity
    {
        [RequiredT4, DisplayOnList(1), DisplayName("Name")]
        public string Name { get; set; }

        [DisplayOnList(2), DisplayName("Price")]
        public decimal Price { get; set; }

        [DisplayName("Stock")]
        public int Stock { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Available from")]
        public DateTime? AvailableFrom { get; set; }

        [DisplayOnList(3), DisplayName("Status")]
        public ProductStatus Status { get; set; }
    }

    /// <summary>Lifecycle of a product listing.</summary>
    public enum ProductStatus
    {
        Draft = 0,
        Published = 1,
        Discontinued = 2,
    }
}
```

`EndpointsT4` is what makes an entity participate in generation; `HttpAll` asks for every endpoint. The full attribute reference is in [Aggregates, entities and attributes](model.md).

## 4. Add lazy.settings

Create `src/Mods/lazy.settings`. It selects package-based template resolution. The default of `TemplatesSource` is `Local`, which is the development mode for the templates themselves, so a consumer must select NuGet mode explicitly:

```xml
<?xml version="1.0" encoding="utf-8"?>
<SettingsFile CurrentProfile="(Default)" xmlns="http://schemas.microsoft.com/VisualStudio/2004/01/settings">
  <Profiles />
  <Settings>
    <Setting Name="GenerationOutputPath" Type="System.String" Scope="Application">
      <Value Profile="(Default)">LazyCode</Value>
    </Setting>
    <Setting Name="ModsPath" Type="System.String" Scope="Application">
      <Value Profile="(Default)">src/mods</Value>
    </Setting>
    <Setting Name="TemplatesSource" Type="System.String" Scope="Application">
      <Value Profile="(Default)">NuGet</Value>
    </Setting>
    <Setting Name="UseNugetTemplates" Type="System.Boolean" Scope="Application">
      <Value Profile="(Default)">True</Value>
    </Setting>
    <Setting Name="TemplatesNuGetPackageId" Type="System.String" Scope="Application">
      <Value Profile="(Default)">Lazy.Crud.Builder.Templates</Value>
    </Setting>
    <Setting Name="TemplatesNuGetVersion" Type="System.String" Scope="Application">
      <Value Profile="(Default)">1.2.0</Value>
    </Setting>
    <Setting Name="MaxParallelT4" Type="System.Int32" Scope="Application">
      <Value Profile="(Default)">1</Value>
    </Setting>
    <Setting Name="GenerateTests" Type="System.Boolean" Scope="Application">
      <Value Profile="(Default)">True</Value>
    </Setting>
    <Setting Name="TestsPathPattern" Type="System.String" Scope="Application">
      <Value Profile="(Default)">Tests\Lazy.Crud.{Aggregate}.Tests.{Layer}</Value>
    </Setting>
  </Settings>
</SettingsFile>
```

The runner probes `src/Mods/lazy.settings` first, then `Mods/lazy.settings`, under the solution root. Other keys (`GeneratedCodeMarkers`, `DisableLocalTemplateFallback`, `WaitTimeoutMs`, `PollIntervalMs`, `EnableT4SelfCleanup`, the `<Layer>LayerPattern` keys) keep their defaults.

## 5. Add the wrapper templates

A wrapper is a one-line `.tt` file inside a layer project that includes the shipped template from the restored package. Create `src/Products/Products.Domain/DefaultEntities.tt`:

```text
<#@ include file="$(PkgLazy_Crud_Builder_Domain)\contentFiles\any\any\LazyCrudTemplates\Domain\DefaultEntities.tt" #>
```

and `src/Products/Products.Domain/DefaultCommands.tt`:

```text
<#@ include file="$(PkgLazy_Crud_Builder_Domain)\contentFiles\any\any\LazyCrudTemplates\Domain\DefaultCommands.tt" #>
```

The other layers follow the same pattern with their own package property and template folder, for example `$(PkgLazy_Crud_Builder_Application)\contentFiles\any\any\LazyCrudTemplates\Application\DefaultCommands.tt` in `Products.Application/DefaultCommands.tt`, and for the Api layer `Api\MiniIoCFactory.tt` (in `ServerIoCFactory.tt`), `Api\MiniMigrations.tt` (in `Migrations.tt`) and `Api\MiniControllers.tt` (in `Controllers\Controllers.tt`). The runner re-creates any wrapper that is missing or differs before every transformation, so a wrapper you forget is seeded for you; writing them yourself keeps the solution usable from the IDE as well.

## 6. Add the projects to the solution and restore

```powershell
dotnet sln Shop.sln add (Get-ChildItem src -Recurse -Filter *.csproj).FullName
dotnet restore Shop.sln
```

Restore brings the packages, the templates and the runner into the package cache. NuGet auto-imports `build/Lazy.Crud.Builder.Templates.props` and `.targets` from the templates package, which define `$(LazyCrudTemplatesRoot)`, `$(LazyCrudPlatformAssembly)` and `$(LazyCrudToolDll)`.

## 7. Verify the toolchain and generate

```powershell
$tool = "$env:USERPROFILE\.nuget\packages\lazy.crud.builder.templates\1.2.0\tools\net11.0\Lazy.Crud.Builder.Platform.Tool.dll"
dotnet $tool toolchain
dotnet $tool generate --solution . --aggregate Products --report artifacts\generate.json
```

`toolchain` prints the resolved Visual Studio instance as JSON and exits 3 when none satisfies the prerequisite. `generate` resolves the template source, checks the toolchain, runs the whole-aggregate drift pre-check, opens the solution in an embedded Visual Studio session, transforms Domain first (`DefaultEntities.tt`, verification that entities exist, `DefaultCommands.tt`), then the other layers under `MaxParallelT4`, then the test layers, and writes a report to `artifacts/generate.json`. Its last line on stderr reads `generate: Success (0)` on success.

The build-driven equivalent, using the property the templates package defines, is:

```powershell
dotnet build Shop.sln -p:LazyCrudTransformOnBuild=true -p:LazyCrudAggregate=Products
```

## 8. Build and test

```powershell
dotnet build Shop.sln
dotnet test Shop.sln
```

## What you now have

- Generated code under each layer's `LazyCode/` folder (`GenerationOutputPath`), for example `Products.Domain/LazyCode/ProductsAgg.DomainCommandHandlers.cs`, every file starting with a marker v2 header (`Ownership: GeneratorOwned`).
- User-owned seeds created once, for example `Products.Domain/Aggregates/ProductsAgg/CommandHandlers/ProductCommandHandler.cs` (`Ownership: GeneratedOnce`), which you may edit freely.
- The manifest `.lazycrud/manifests/Products.manifest.json` (schema version 2) recording every generated file with its ownership and content hash, and machine-readable diagnostics at `.lazycrud/diagnostics/Products.diagnostics.json`.

## Optional: generated test projects

With `GenerateTests` set to `True` the runner also transforms one test project per layer when it exists at `src/Products/Tests/Lazy.Crud.Products.<TestLayer>/`, for example `Lazy.Crud.Products.Tests.Domain`. Each is an xunit project (Microsoft.NET.Test.Sdk 17.13.0, xunit 2.9.3, xunit.runner.visualstudio 3.0.2) that references its target layer project and the layer package with `GeneratePathProperty="true"`, and carries one wrapper, for example `DomainTest.tt`:

```text
<#@ include file="$(PkgLazy_Crud_Builder_Domain)\contentFiles\any\any\LazyCrudTemplates\Tests\Domain\Tests.Domain.tt" #>
```

Test layer ids and wrappers: `Tests.Domain` (`DomainTest.tt`), `Tests.Application` (`ApplicationTest.tt`), `Tests.DTO` (`DtoTest.tt`), `Tests.InfraData` (`InfraDataTest.tt`), `Tests.InfraIoC` (`InfraIoCTest.tt`), `Tests.Api` (`ApiTest.tt`), `Tests.ApiQueries` (`ApiQueriesTest.tt`), `Tests.Enumerations` (`EnumerationsTest.tt`). Pass `--no-tests` to `generate` to skip them. See [Testing](testing.md).

## Regenerating

Change the entity, run `generate` again. Files whose recorded hash still matches are overwritten; a file you edited by hand is reported as drifted (`LC2001`) and left untouched, and the run exits 7 until you choose `--resolution force` or `--resolution skip-modified`. See [Generated code policy](generated-code-policy.md) and [Safe customization](customization.md).
