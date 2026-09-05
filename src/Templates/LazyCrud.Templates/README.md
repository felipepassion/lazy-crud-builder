# Lazy.Crud.Builder.Templates

The canonical Lazy CRUD templates package. It ships:

- `contentFiles/any/any/LazyCrudTemplates/project.tt` and `Tests/Tests.Common.ttinclude`, the
  templates shared by every layer (layer templates ship in their layer's `Lazy.Crud.Builder.*`
  package, which depends on this one).
- `tools/net10.0/`: `Lazy.Crud.Builder.Platform.dll`, the shared generation writer the templates
  reference, and `Lazy.Crud.Builder.Platform.Tool.dll`, the unattended generation runner.
- `build/Lazy.Crud.Builder.Templates.props|targets`, auto-imported by NuGet: they define
  `$(LazyCrudTemplatesRoot)`, `$(LazyCrudPlatformAssembly)` and `$(LazyCrudToolDll)` and expose
  `dotnet build -p:LazyCrudTransformOnBuild=true`.
- `lazycrud/lazycrud.platform.json`, the platform descriptor, and `contracts/`, the contract
  fixtures and JSON Schemas.

## Prerequisite

Generation requires an installed Visual Studio (2022 17.x or 2026 18.x, release, preview or
Insiders channel) with the **Text Template Transformation** component
(`Microsoft.VisualStudio.Component.TextTemplating`). Headless means unattended, not Visual
Studio-free: the runner drives an embedded Visual Studio session through its automation model.

```
dotnet <path-to>/tools/net10.0/Lazy.Crud.Builder.Platform.Tool.dll toolchain
dotnet <path-to>/tools/net10.0/Lazy.Crud.Builder.Platform.Tool.dll generate --solution <dir> --aggregate Products
```

Documentation: https://github.com/felipepassion/lazy-crud-builder/tree/main/docs
