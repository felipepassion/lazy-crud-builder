# Aggregates, entities and attributes

The declarative model is the input of the compiler. This page is the reference for how it is laid out and for every attribute the platform publishes.

## Aggregates

An aggregate is a business area with its own set of entities and its own set of layer projects. Its name (`{Aggregate}`, for example `Products`) drives every convention:

| Convention | Value |
| --- | --- |
| Aggregate folder | `src/{Aggregate}/` under the solution root |
| Layer project | `src/{Aggregate}/{Aggregate}.<Layer>/` (`Domain`, `Application`, `Application.DTO`, `Infra.Data`, `Infra.IoC`, `Api.Queries`, `Enumerations`, `Api`) |
| Aggregate namespace segment | `{Aggregate}Agg` (the `{Agg}` token of the descriptor) |
| Entity folder | `src/{Aggregate}/{Aggregate}.Domain/Aggregates/{Aggregate}Agg/Entities/` |
| Generated output folder | `{OutputPath}` inside each layer project, `GenerationOutputPath` in `lazy.settings`, default `LazyCode` |
| Test project | `src/{Aggregate}/Tests/Lazy.Crud.{Aggregate}.<TestLayer>/` |
| Manifest | `.lazycrud/manifests/{Aggregate}.manifest.json` |

The layer project name patterns can be changed per layer in `lazy.settings` (`DomainLayerPattern`, `ApplicationLayerPattern`, `ApplicationDTOLayerPattern`, `InfraDataLayerPattern`, `InfraIoCLayerPattern`, `ApiLayerPattern`, `ApiQueriesLayerPattern`, `EnumerationsLayerPattern`; default `{Aggregate}.<Layer>`).

## Entities

An entity is a public class in the entity folder of the Domain project that derives from the runtime base type `Entity` (namespace Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.Entities, shipped in `Lazy.Crud.Builder.Domain`). Properties are ordinary auto-properties. Enumerations may be declared next to the entity or in the `Enumerations` project.

The templates collect the model through the Visual Studio code model:

- `DefaultEntities.tt` (Domain phase 1) collects the entities of the Domain project that carry `EndpointsT4`; it seeds the aggregate settings entity (`Aggregates/{Agg}/Entities/{Aggregate}.cs`, created once) and the default folders.
- `DefaultCommands.tt` (Domain phase 2) collects every non-abstract Domain entity plus the aggregate's `EndpointsT4` entities and produces command and event handlers, command and event models, filters, repository interfaces, mapping profiles and specifications.
- The other layers' `DefaultCommands.tt` wrappers derive their output from the same model through the project references of the aggregate.

An entity without `EndpointsT4` is part of the model (it can be referenced, mapped and persisted) but gets no endpoints, commands or pages of its own.

Standard .NET attributes are honored where the templates read them: `System.ComponentModel.DisplayName` supplies the display name used by generated listings and forms (`GetDisplayName` falls back to the property name), and `System.ComponentModel.DataAnnotations` attributes flow into the generated DTOs.

Example (the reference aggregate used by the golden test and the harness, `tests/fixtures/reference-aggregate`):

```csharp
using System.ComponentModel;
using Lazy.Crud.Builder.Application.DTO.Attributes;
using Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.Entities;
using Lazy.Crud.Builder.Domain.Attributes.T4;

namespace Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Entities
{
    [AggregateSettingsT4, EndpointsT4(EndpointTypes.HttpAll)]
    public class Product : Entity
    {
        [RequiredT4, DisplayOnList(1), DisplayName("Name")]
        public string Name { get; set; }

        [DisplayOnList(2), DisplayName("Price")]
        public decimal Price { get; set; }

        [DisplayName("Available from")]
        public DateTime? AvailableFrom { get; set; }

        [DisplayOnList(3), DisplayName("Status")]
        public ProductStatus Status { get; set; }
    }

    [EndpointsT4(EndpointTypes.HttpAll)]
    public class ProductCategory : Entity
    {
        [RequiredT4, DisplayOnList(1), DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Sort order")]
        public int SortOrder { get; set; }
    }
}
```

## The attribute catalog is a contract

All 43 attributes ship in the `Lazy.Crud.Builder.Application.DTO` package (assembly of the same name) across three namespaces:

- Lazy.Crud.Builder.Application.DTO.Attributes (26 attributes)
- Lazy.Crud.Builder.Domain.Attributes.T4 (16 attributes)
- Lazy.Crud.Builder.Domain.Attributes.Auth (1 attribute)

The platform publishes the catalog in two halves that consumers join by `documentationId`:

- **Identity and structure** in the descriptor's `attributes[]`: `name`, `fullName`, `namespace`, `packageId`, `category`, `applicability`, `overloads[]` (each with `parameters[]` of `name`, `type`, `required`) and `documentationId` (the XML documentation member id in `T:` form).
- **Prose** (`summary`, `param`, `remarks`) in the XML documentation file shipped in the package at `lib/net10.0/Lazy.Crud.Builder.Application.DTO.xml`.

Every attribute has a `<summary>` and a `<category>` from the controlled vocabulary below, every constructor parameter has a `<param>`, every catalog entry derives from `System.Attribute`, and the build fails (`descriptor generate`, `validate tree`) on any gap. Applicability comes from the declared `AttributeUsage`: `MenuSection` and `SubMenuSection` apply to classes only (single use, inherited); every other attribute declares no usage restriction and is published as `All`.

Category vocabulary (`attributeCategories` in the descriptor): `Code Generation (T4)`, `Modeling`, `Serialization/CSV`, `UI/Display`, `UI/Forms`, `UI/Input`, `UI/Layout`, `UI/List`, `UI/Menu`, `UI/Navigation`, `UI/Selectors`, `UI/Titles`, `Validation`.

## Attribute reference by category

"Default constructor" means the attribute takes no arguments. Namespaces are abbreviated: `DTO.Attributes` is Lazy.Crud.Builder.Application.DTO.Attributes, `T4` is Lazy.Crud.Builder.Domain.Attributes.T4, `Auth` is Lazy.Crud.Builder.Domain.Attributes.Auth.

### Code Generation (T4)

| Attribute | Namespace | Constructors | Effect |
| --- | --- | --- | --- |
| `AggregateSettingsT4` | T4 | `AggregateSettingsT4()` | Marks the aggregate or member that carries aggregate-level settings for the templates. Marker only. |
| `AttatchOnUpdate` | DTO.Attributes | default constructor | The generated mapping attaches the entity to the context on update. |
| `AutoGenerateDefaultDataFiles` | DTO.Attributes | default constructor | Default data files (seeds, initial data) are generated for the aggregate. |
| `AutoGenerateDefaultFiles` | DTO.Attributes | default constructor | The default files for the feature or entity are generated. |
| `DefaultValueT4` | DTO.Attributes | `DefaultValueT4(string value)` | A default value the templates apply to the property. |
| `DettachNavigationProperty` | T4 | `DettachNavigationProperty()` | The navigation property is excluded from generation, mapping and serialization (avoids cycles and large related graphs). |
| `DisableControllers` | T4 | `DisableControllers()` | No controllers or endpoints are generated for the decorated type. |
| `DoNotReplaceAfterGenerated` | DTO.Attributes | default constructor | The generated front-end for the class is not overwritten after the first generation. |
| `DoNotReplaceBaseCadastroPageAfterGenerated` | DTO.Attributes | default constructor | The base registration page of the front-end is not overwritten after the first generation. |
| `EndpointsT4` | T4 | `EndpointsT4(params EndpointTypes[] attr)`, `EndpointsT4(int order, params EndpointTypes[] attr)` | Which HTTP endpoints are generated for the type, with an optional processing order. An entity takes part in endpoint and command generation only when it carries this attribute. |
| `GenerateAppService` | T4 | `GenerateAppService()` | An application service with the common CRUD operations is generated for the aggregate. |
| `IgnorePropertyOnFrontT4` | T4 | default constructor | The property is left out of generated front-end DTOs and UI models. |
| `IgnorePropertyOnListingT4` | T4 | `IgnorePropertyOnListingT4()` | The property is left out of generated listing views. |
| `IgnorePropertyT4` | T4 | `IgnorePropertyT4()` | The property is ignored by every template. |
| `IgnorePropertyT4OnRequest` | T4 | `IgnorePropertyT4OnRequest()` | The property is left out of generated request models. |
| `MigrationOrder` | DTO.Attributes | `MigrationOrder(int order)` | Execution order of the migrations generated from aggregates or seed files. |
| `MustImplementT4` | T4 | `MustImplementT4()` | The class must implement the interface or contract the templates expect. Marker for tooling; not enforced by the attribute itself. |
| `NotRequiredOnFrontT4` | T4 | `NotRequiredOnFrontT4()` | The property is not required on front-end models even when it is required in the domain. |

`EndpointTypes` values: `HttpPost`, `HttpGet`, `HttpDelete`, `HttpPut`, `HttpAll`, `HttpListining`, `Count`.

### Modeling

| Attribute | Namespace | Constructors | Effect |
| --- | --- | --- | --- |
| `NullableProperty` | DTO.Attributes | default constructor | The property is nullable in the DTO and UI model. |
| `OneToOne` (source file `OneToOneT4.cs`) | T4 | `OneToOne()` | A one-to-one relationship between entities for generation and mapping. |
| `ParameterType` | DTO.Attributes | default constructor | A typed parameter used by operations or components. |

### Serialization/CSV

| Attribute | Namespace | Constructors | Effect |
| --- | --- | --- | --- |
| `IgnoreCsvMap` | DTO.Attributes | default constructor | The property is ignored in CSV mapping. |

### UI/Display

| Attribute | Namespace | Constructors | Effect |
| --- | --- | --- | --- |
| `Title` | DTO.Attributes | default constructor | A custom display title for UI rendering and documentation. |

### UI/Forms

| Attribute | Namespace | Constructors | Effect |
| --- | --- | --- | --- |
| `Step` | T4 | `Step(int position)` | Position of the property or entity step in multi-step forms or processes. |
| `Steppable` (source file `StepEntity.cs`) | T4 | `Steppable(int quantity)`, `Steppable()` | Marks an entity as steppable with the number of steps. |

### UI/Input

| Attribute | Namespace | Constructors | Effect |
| --- | --- | --- | --- |
| `RadioButton` | DTO.Attributes | default constructor | Renders a radio button option. |

### UI/Layout

| Attribute | Namespace | Constructors | Effect |
| --- | --- | --- | --- |
| `EnableRightContainer` | DTO.Attributes | default constructor | Enables a right-side container or panel in the layout. |
| `HideTitleOnHeader` | DTO.Attributes | default constructor | Hides the title from the page header. |

### UI/List

| Attribute | Namespace | Constructors | Effect |
| --- | --- | --- | --- |
| `DisplayOnList` | DTO.Attributes | `DisplayOnList()`, `DisplayOnList(int order, DisplayOnListTemplates template = 0)` | How the property is rendered in list views: sorting order and an optional template (`DisplayOnListTemplates.Status`). |

### UI/Menu

| Attribute | Namespace | Constructors | Effect |
| --- | --- | --- | --- |
| `MenuSection` | DTO.Attributes | `MenuSection(string name)` | Places the feature generated for the entity under a main menu section. Class targets only. |
| `SubMenuSection` | DTO.Attributes | `SubMenuSection(string parentName, string name)` | Places it under a sub-section of the named main section. Class targets only. |

### UI/Navigation

| Attribute | Namespace | Constructors | Effect |
| --- | --- | --- | --- |
| `ParameterTypePageSwitcher` | DTO.Attributes | default constructor | Page switching driven by parameter type. |
| `RegisterOrder` | DTO.Attributes | `RegisterOrder(int position)` | Zero-based registration or display order for menus and navigation flows. |

### UI/Selectors

| Attribute | Namespace | Constructors | Effect |
| --- | --- | --- | --- |
| `ForeignDataSelector` | DTO.Attributes | default constructor | The property uses a foreign data selector component. |
| `ForeignDataSelectorDropDownWithAutoComplete` | DTO.Attributes | default constructor | A dropdown with auto-complete backed by foreign data. |
| `ListingPicker` | DTO.Attributes | `ListingPicker()`, `ListingPicker(string title)` | The navigation property is rendered as a searchable listing picker, with an optional title. |

### UI/Titles

| Attribute | Namespace | Constructors | Effect |
| --- | --- | --- | --- |
| `H1` | DTO.Attributes | `H1(string title)`, `H1(string title, string subTitle)` | Primary heading on the generated pages and components. |
| `H2` | DTO.Attributes | `H2(string title)`, `H2(string title, string subTitle)` | Secondary heading. |
| `Subtitle` | DTO.Attributes | default constructor | A subtitle shown in pages and components. |

### Validation

| Attribute | Namespace | Constructors | Effect |
| --- | --- | --- | --- |
| `EmailOrPhone` | Auth | default constructor (derives from `ValidationAttribute`) | Accepts either a valid email address or a phone number. |
| `IgnoreValidation` | DTO.Attributes | default constructor | Automatic validations are skipped for the property. |
| `RequiredT4` | T4 | `RequiredT4()`, `RequiredT4(string errorMessage)` | The property is required, with an optional error message. |
| `Unique` (source file `UniqueT4.cs`) | T4 | `Unique()`, `Unique(string message)` | The property or set of properties is unique, with an optional message. |

## Changing the model

The model is yours: entity files are hand-written and never overwritten by the generator (the aggregate settings entity and the user entity are seeded once and then user-owned). Changing an attribute or a property and regenerating is the supported way to change generated behavior; see [Safe customization](customization.md). Renaming or removing a published attribute is a contract change recorded in the changelog and reflected in the fixtures; see [Release guidance](release.md).
