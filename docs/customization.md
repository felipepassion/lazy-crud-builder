# Safe customization

The rule is simple: change the input of the compiler, an extension point the compiler leaves for you, or the compiler itself. Never the artifact. A change made inside a `GeneratorOwned` file is protected on the next run (`LC2001`) and lost the moment you force it.

## Pick the right edit target

| You want to | Edit | Not |
| --- | --- | --- |
| Add, rename or remove a field, a relationship, an endpoint, a validation rule or a display option | The entity class and its attributes under `Aggregates/{Aggregate}Agg/Entities/` ([model reference](model.md)) | The generated DTO, mapping, controller or page |
| Add behavior to a generated type (a method, an override, an extra constructor) | A partial class file of your own next to the generated one | The generated file |
| Change what a command handler, a request DTO or a validator does | The user-owned seed under `Aggregates/{Agg}/CommandHandlers/`, `Aggregates/{Agg}/Requests/` or `Aggregates/{Agg}/Validators/` (`GeneratedOnce`) | The `GeneratorOwned` companion under `LazyCode/` |
| Change how every aggregate is generated | The template, in a local template fork (see below) | A generated file in one aggregate |
| Keep the generator away from a hand-written file | Nothing: files the generator did not write are never touched | Editing a generated file to "own" it |

## Model-driven customization

Most customization needs no code outside the model. The attributes decide what is generated: `EndpointsT4` selects the endpoints, `DisableControllers` removes controllers for a type, `GenerateAppService` adds an application service, `IgnorePropertyT4` and its `OnRequest`, `OnFrontT4` and `OnListingT4` variants remove a property from specific outputs, `NotRequiredOnFrontT4`, `RequiredT4`, `Unique`, `IgnoreValidation` shape validation, `DisplayOnList`, `H1`, `H2`, `Subtitle`, `Title`, `MenuSection`, `SubMenuSection`, `RegisterOrder`, `Step`, `Steppable`, `ListingPicker` and the selector attributes shape the generated UI, `MigrationOrder` orders migrations. Regenerate after every model change; the manifest and the markers keep the round trip safe.

Two front-end attributes exist specifically to protect customized pages: `DoNotReplaceAfterGenerated` keeps the generated front-end of a class from being overwritten after the first generation, and `DoNotReplaceBaseCadastroPageAfterGenerated` does the same for the base registration page.

## Partial classes

The templates declare the generated types as `partial` in the Domain command handlers, command and event models, mapping profiles, specifications and CSV mapping profiles, the application services, the request, response, listing, update and steppable DTOs and their validators, the identity context, mappings and repositories, the IoC factory, the controllers and the query models. Add a file of your own in the same project and namespace with the same type name and the `partial` modifier; the compiler merges it with the generated part on every build, and the generator never sees your file because it is not a path it writes.

```csharp
// src/Products/Products.Application/ProductsAgg/ProductAppService.Custom.cs (your file, never generated)
namespace Lazy.Crud.Products.Application.Aggregates.ProductsAgg.AppServices
{
    public partial class ProductAppService
    {
        public Task<int> CountPublishedAsync() => /* your code */ Task.FromResult(0);
    }
}
```

Use the namespace and type name you find in the generated file. If a generated type you need is not partial, the change belongs in the template.

## Per layer

| Layer | Safe extension points | Generated and replaceable |
| --- | --- | --- |
| `Domain` | Entity classes; `Aggregates/{Agg}/Entities/{Aggregate}.cs` and `Aggregates/UsersAgg/Entities/User.cs` (seeded once); `Aggregates/{Agg}/CommandHandlers/{Entity}CommandHandler.cs` (seeded once); partial classes for command handlers, command and event models, profiles, specifications | `LazyCode/{Agg}.*.cs` |
| `Application` | Partial classes for `AppServices` and `IAppServices` | `LazyCode/{Agg}.cs`, `{Agg}.AppServices.cs`, `{Agg}.IAppServices.cs` |
| `Application.DTO` | `Aggregates/{Agg}/Requests/{Entity}DTO.cs` and `Aggregates/{Agg}/Validators/{Entity}Validator.cs` (seeded once); partial classes for the DTOs | `LazyCode/{Agg}.*.cs` |
| `Infra.Data` | Partial classes for the identity context, mappings and repositories; your own migrations | `LazyCode/{Agg}.*.cs` |
| `Infra.IoC` | A partial `IoCFactory`; additional registrations in your own extension methods | `LazyCode/{Agg}.cs`, `{Agg}.IoCFactory.cs` |
| `Api.Queries` | Partial query models | `T4/{Agg}.cs`, `LazyCode/{Agg}.QueryModels.cs` |
| `Enumerations` | The enumeration files are yours | nothing generated |
| `Api` | `Program.cs`, `appsettings*.json`, your own controllers and middleware; partial generated controllers | `LazyCode/{Agg}.Api.GlobalUsings.cs`, `{Agg}.Controllers.cs`, `{Agg}.cs` |
| Test projects | Your own test files next to the generated `*.g.cs` files | `{TestOutputPath}/*.g.cs` |

## Customizing the templates

When a change must apply to every aggregate, change the template. Templates are the compiler, so this is a platform-level customization with a platform-level obligation: keep the fork aligned with the package version you consume.

1. Copy the template tree you want to change (the layout of `src/ProjectFiles`: `project.tt`, `Tests/Tests.Common.ttinclude`, one folder per layer).
2. Select local template development mode explicitly. Either pass `--templates local:<dir>` to the runner, or set `TemplatesSource` to `Local`, `UseNugetTemplates` to `False` and `TemplatesLocalProjectFilesPath` to the fork directory in `lazy.settings` (the runner also probes `src/ProjectFiles` and `ProjectFiles` under the solution root for a tree containing `project.tt`).
3. Point the MSBuild properties at the fork in the solution's `Directory.Build.props`, before the package props are imported, so shared includes and the writer assembly resolve from your tree:

```xml
<Project>
  <PropertyGroup>
    <LazyCrudTemplatesRoot>$(MSBuildThisFileDirectory)templates\</LazyCrudTemplatesRoot>
    <LazyCrudPlatformAssembly>$(MSBuildThisFileDirectory)templates\Lazy.Crud.Builder.Platform.dll</LazyCrudPlatformAssembly>
  </PropertyGroup>
</Project>
```

The run reports local mode explicitly (`LC3002`, template source kind `Local`, location and version `source-tree`). Local mode is a development mode: it is never reached silently from a failed package resolution (`LC3001` instead), and it is refused when `DisableLocalTemplateFallback` is `True`, which is what clean-room validation sets.

Rules for a fork:

- Keep the writer call sites. Every `SaveOutput` and `SaveOutputToSubFolder` call carries an explicit `ownership` and `templateId` that must agree with the descriptor registry; a template whose id is not registered has its output treated as user-owned and reported with `LC2004`.
- Do not hand-write marker headers; the shared writer emits them.
- Version the fork with the packages. The templates package must equal the version of every layer package that ships templates; a fork made for 1.0.x keeps working in local mode but does not gain the 1.1.0 markers, manifest v2 or drift protection until it is rebased on the 1.1.0 templates. See [Migrations](migrations.md).

## What never to do

- Do not edit a `GeneratorOwned` file and force the next regeneration to "keep" it. Force discards your edit (`LC2006`); `skip-modified` keeps the file but freezes it against every future template improvement.
- Do not delete the manifest to silence a drift report. Without a baseline every file becomes `unverified`, and a file that differs from the regenerated body is protected anyway (`LC2002`).
- Do not put hand-written files under `{OutputPath}` with names that match a registry output pattern; they will be seen as unverified generated files.

## Recovering

- To get a fresh seed: delete the `GeneratedOnce` file and regenerate.
- To discard an accidental edit in a generated file: regenerate with `--resolution force` (the diagnostic lists exactly which files are overwritten).
- To keep an edit temporarily: `--resolution skip-modified`, then move the change to a partial class or the template and force once.
