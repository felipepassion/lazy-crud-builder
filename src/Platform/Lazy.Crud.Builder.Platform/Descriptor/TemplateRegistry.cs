using System;
using System.Collections.Generic;
using System.Linq;

namespace Lazy.Crud.Builder.Platform.Descriptor
{
    /// <summary>What a registered template does.</summary>
    public enum TemplateKind
    {
        /// <summary>Produces a file through the shared writer.</summary>
        Writer = 0,
        /// <summary>An entry/orchestration template: includes other templates and writes nothing itself (its own IDE output is residue).</summary>
        Entry = 1,
        /// <summary>A shared include (helpers, partial generators) with no output of its own.</summary>
        Include = 2,
    }

    /// <summary>One registered template output. Ids are stable; they never change silently.</summary>
    public sealed class TemplateRegistryEntry
    {
        public TemplateRegistryEntry(string id, string layer, string sourceFile, string outputPattern, Ownership ownership, TemplateKind kind)
        {
            Id = id; Layer = layer; SourceFile = sourceFile; OutputPattern = outputPattern; Ownership = ownership; Kind = kind;
        }

        /// <summary>Stable template id, for example <c>domain.command-handlers</c>.</summary>
        public string Id { get; }
        /// <summary>Layer id (<c>Domain</c>, <c>Tests.Domain</c>, <c>WebApi</c>, <c>Shared</c>).</summary>
        public string Layer { get; }
        /// <summary>Path of the template source under <c>src/ProjectFiles</c>, forward slashes.</summary>
        public string SourceFile { get; }
        /// <summary>Output location pattern relative to the layer project, or null for entry/include templates.</summary>
        public string OutputPattern { get; }
        public Ownership Ownership { get; }
        public TemplateKind Kind { get; }

        /// <summary>The package that carries this template.</summary>
        public string PackageId { get { return TemplateRegistry.PackageForSource(SourceFile); } }
        /// <summary>Path of the template inside its package.</summary>
        public string InPackagePath { get { return TemplateRegistry.InPackagePathForSource(SourceFile); } }
        public string TemplateVersion { get { return PlatformInfo.Version; } }
        public string FileName { get { return SourceFile.Substring(SourceFile.LastIndexOf('/') + 1); } }
    }

    /// <summary>
    /// The canonical template registry: the single source from which the descriptor's
    /// <c>templates.registry[]</c> is generated and against which call sites and packages are validated.
    /// Output pattern tokens: {Aggregate} (aggregate name), {Agg} ({Aggregate}Agg), {Entity},
    /// {Step}, {OutputPath} (GenerationOutputPath from lazy.settings, default LazyCode),
    /// {TestOutputPath} (default Generated). Patterns are relative to the layer project directory.
    /// </summary>
    public static class TemplateRegistry
    {
        public const string CanonicalTemplatesPackageId = "Lazy.Crud.Builder.Templates";
        public const string DeprecatedTemplatesPackageId = "Lazy.Crud.Templates";
        public const string ContentRoot = "contentFiles/any/any/LazyCrudTemplates/";

        public static readonly string[] OutputPatternTokens = { "{Aggregate}", "{Agg}", "{Entity}", "{Step}", "{OutputPath}", "{TestOutputPath}" };

        private static TemplateRegistryEntry W(string id, string layer, string source, string output, Ownership ownership)
            => new TemplateRegistryEntry(id, layer, source, output, ownership, TemplateKind.Writer);
        private static TemplateRegistryEntry E(string id, string layer, string source)
            => new TemplateRegistryEntry(id, layer, source, null, Ownership.GeneratorOwned, TemplateKind.Entry);
        private static TemplateRegistryEntry I(string id, string layer, string source)
            => new TemplateRegistryEntry(id, layer, source, null, Ownership.GeneratorOwned, TemplateKind.Include);

        private static readonly TemplateRegistryEntry[] _entries =
        {
            // Shared (canonical templates package)
            I("shared.project", "Shared", "project.tt"),
            I("shared.tests-common", "Shared", "Tests/Tests.Common.ttinclude"),

            // Domain
            E("domain.default-entities", "Domain", "Domain/DefaultEntities.tt"),
            E("domain.default-commands", "Domain", "Domain/DefaultCommands.tt"),
            E("domain.domain-commands", "Domain", "Domain/DomainCommands.tt"),
            E("domain.reset-this-aggregate", "Domain", "Domain/ResetThisAggregate.tt"),
            W("domain.aggregate-settings-entity", "Domain", "Domain/AggregateSettingsEntity.tt", "Aggregates/{Agg}/Entities/{Aggregate}.cs", Ownership.GeneratedOnce),
            I("domain.default-folders", "Domain", "Domain/DefaultFolders.tt"),
            W("domain.user-entity", "Domain", "Domain/UserEntity.tt", "Aggregates/UsersAgg/Entities/User.cs", Ownership.GeneratedOnce),
            W("domain.legacy-commands.entities", "Domain", "Domain/Commands.tt", "../../../{Aggregate}/{Aggregate}.Domain/DefaultEntities.tt", Ownership.GeneratorOwned),
            W("domain.legacy-commands.commands", "Domain", "Domain/Commands.tt", "../../../{Aggregate}/{Aggregate}.{Layer}/DefaultCommands.tt", Ownership.GeneratorOwned),
            W("domain.global-usings", "Domain", "Domain/DomainGlobalUsings.tt", "{OutputPath}/{Agg}.cs", Ownership.GeneratorOwned),
            W("domain.command-handlers", "Domain", "Domain/DomainCommandHandlers.tt", "{OutputPath}/{Agg}.DomainCommandHandlers.cs", Ownership.GeneratorOwned),
            W("domain.command-handlers.seed", "Domain", "Domain/DomainCommandHandlers.tt", "Aggregates/{Agg}/CommandHandlers/{Entity}CommandHandler.cs", Ownership.GeneratedOnce),
            W("domain.command-models", "Domain", "Domain/DomainCommandModels.tt", "{OutputPath}/{Agg}.DomainCommandModels.cs", Ownership.GeneratorOwned),
            W("domain.event-handlers", "Domain", "Domain/DomainEventHandlers.tt", "{OutputPath}/{Agg}.DomainEventHandlers.cs", Ownership.GeneratorOwned),
            W("domain.event-models", "Domain", "Domain/DomainEventModels.tt", "{OutputPath}/{Agg}.DomainEventModels.cs", Ownership.GeneratorOwned),
            W("domain.filters", "Domain", "Domain/Filters.tt", "{OutputPath}/{Agg}.Filters.cs", Ownership.GeneratorOwned),
            W("domain.irepositories", "Domain", "Domain/IRespositories.tt", "{OutputPath}/{Agg}.IRespositories.cs", Ownership.GeneratorOwned),
            W("domain.profiles-listining-mapping", "Domain", "Domain/ProfilesListiningMapping.tt", "{OutputPath}/{Agg}.ProfilesListiningMapping.cs", Ownership.GeneratorOwned),
            W("domain.profiles-mapping", "Domain", "Domain/ProfilesMapping.tt", "{OutputPath}/{Agg}.ProfilesMapping.cs", Ownership.GeneratorOwned),
            W("domain.specifications", "Domain", "Domain/Specifications.tt", "{OutputPath}/{Agg}.Specifications.cs", Ownership.GeneratorOwned),
            I("domain.specification-item", "Domain", "Domain/SpecificationItem.tt"),
            I("domain.specification-sub-item", "Domain", "Domain/SpecificationSubItem.tt"),
            W("domain.csv-mapping-profiles", "Domain", "Domain/CSVMappingProfiles.tt", "{OutputPath}/{Agg}.CSVMappingProfiles.cs", Ownership.GeneratorOwned),

            // Application
            E("application.default-commands", "Application", "Application/DefaultCommands.tt"),
            W("application.global-usings", "Application", "Application/ApplicationGlobalUsings.tt", "{OutputPath}/{Agg}.cs", Ownership.GeneratorOwned),
            W("application.app-services", "Application", "Application/AppServices.tt", "{OutputPath}/{Agg}.AppServices.cs", Ownership.GeneratorOwned),
            W("application.iapp-services", "Application", "Application/IAppServices.tt", "{OutputPath}/{Agg}.IAppServices.cs", Ownership.GeneratorOwned),

            // Application.DTO
            E("application-dto.default-commands", "Application.DTO", "Application.DTO/DefaultCommands.tt"),
            W("application-dto.global-usings", "Application.DTO", "Application.DTO/ApplicationDtoGlobalUsings.tt", "{OutputPath}/{Agg}.cs", Ownership.GeneratorOwned),
            W("application-dto.csv-mapper", "Application.DTO", "Application.DTO/CsvMapper.tt", "{OutputPath}/{Agg}.ListiningDTO.cs", Ownership.GeneratorOwned),
            W("application-dto.listinings", "Application.DTO", "Application.DTO/ListiningsDTO.tt", "{OutputPath}/{Agg}.ListiningDTO.cs", Ownership.GeneratorOwned),
            W("application-dto.requests", "Application.DTO", "Application.DTO/RequestsDTO.tt", "{OutputPath}/{Agg}.RequestsDTO.cs", Ownership.GeneratorOwned),
            W("application-dto.requests.seed", "Application.DTO", "Application.DTO/RequestsDTO.tt", "Aggregates/{Agg}/Requests/{Entity}DTO.cs", Ownership.GeneratedOnce),
            E("application-dto.responses", "Application.DTO", "Application.DTO/ResponsesDTO.tt"),
            W("application-dto.steppable-requests", "Application.DTO", "Application.DTO/SteppableRequestsDTO.tt", "{OutputPath}/{Agg}.SteppableRequestsDTO.cs", Ownership.GeneratorOwned),
            W("application-dto.steppable-requests-validators", "Application.DTO", "Application.DTO/SteppableRequestsValidators.tt", "{OutputPath}/{Agg}.SteppableRequestsValidators.cs", Ownership.GeneratorOwned),
            W("application-dto.steppable-requests-validators.seed", "Application.DTO", "Application.DTO/SteppableRequestsValidators.tt", "Aggregates/{Agg}/Validators/{Entity}Validator.cs", Ownership.GeneratedOnce),
            E("application-dto.updates", "Application.DTO", "Application.DTO/UpdatesDTO.tt"),

            // Infra.Data
            E("infra-data.default-commands", "Infra.Data", "Infra.Data/DefaultCommands.tt"),
            W("infra-data.identity-context", "Infra.Data", "Infra.Data/IdentityContext.tt", "{OutputPath}/{Agg}.IdentityContext.cs", Ownership.GeneratorOwned),
            W("infra-data.global-usings", "Infra.Data", "Infra.Data/InfraDataGlobalUsings.tt", "{OutputPath}/{Agg}.cs", Ownership.GeneratorOwned),
            W("infra-data.mappings", "Infra.Data", "Infra.Data/Mappings.tt", "{OutputPath}/{Agg}.Mappings.cs", Ownership.GeneratorOwned),
            W("infra-data.repositories", "Infra.Data", "Infra.Data/Respositories.tt", "{OutputPath}/{Agg}.Respositories.cs", Ownership.GeneratorOwned),

            // Infra.IoC
            E("infra-ioc.default-commands", "Infra.IoC", "Infra.IoC/DefaultCommands.tt"),
            W("infra-ioc.global-usings", "Infra.IoC", "Infra.IoC/InfraIoCGlobalUsings.tt", "{OutputPath}/{Agg}.cs", Ownership.GeneratorOwned),
            W("infra-ioc.ioc-factory", "Infra.IoC", "Infra.IoC/IoCFactory.tt", "{OutputPath}/{Agg}.IoCFactory.cs", Ownership.GeneratorOwned),

            // Api.Queries
            E("api-queries.default-commands", "Api.Queries", "Api.Queries/DefaultCommands.tt"),
            W("api-queries.global-usings", "Api.Queries", "Api.Queries/ApiQueriesGlobalUsings.tt", "T4/{Agg}.cs", Ownership.GeneratorOwned),
            W("api-queries.query-models", "Api.Queries", "Api.Queries/QueryModels.tt", "{OutputPath}/{Agg}.QueryModels.cs", Ownership.GeneratorOwned),

            // Api
            W("api.global-usings", "Api", "Api/ApiGlobalUsings.tt", "{OutputPath}/{Agg}.Api.GlobalUsings.cs", Ownership.GeneratorOwned),
            W("api.mini-controllers", "Api", "Api/MiniControllers.tt", "{OutputPath}/{Agg}.Controllers.cs", Ownership.GeneratorOwned),
            W("api.mini-ioc-factory", "Api", "Api/MiniIoCFactory.tt", "{OutputPath}/{Agg}.cs", Ownership.GeneratorOwned),
            // The migration script embeds the moment it was generated, so its content is never reproducible;
            // it is seeded once and then belongs to the developer who runs it.
            W("api.mini-migrations", "Api", "Api/MiniMigrations.tt", "Migrations.generated.bat", Ownership.GeneratedOnce),

            // Tests (carried by the layer package under Tests/<Layer>/)
            W("tests.domain", "Tests.Domain", "Tests/Domain/Tests.Domain.tt", "{TestOutputPath}/{Agg}.{Entity}EntityTests.g.cs", Ownership.GeneratorOwned),
            W("tests.application", "Tests.Application", "Tests/Application/Tests.Application.tt", "{TestOutputPath}/{Agg}.{Entity}AppServiceTests.g.cs", Ownership.GeneratorOwned),
            W("tests.application-dto", "Tests.DTO", "Tests/Application.DTO/Tests.DTO.tt", "{TestOutputPath}/{Agg}.{Entity}DtoSerializationTests.g.cs", Ownership.GeneratorOwned),
            W("tests.infra-data", "Tests.InfraData", "Tests/Infra.Data/Tests.InfraData.tt", "{TestOutputPath}/{Agg}.{Entity}MappingTests.g.cs", Ownership.GeneratorOwned),
            W("tests.infra-ioc", "Tests.InfraIoC", "Tests/Infra.IoC/Tests.InfraIoC.tt", "{TestOutputPath}/{Agg}.{Entity}IoCTests.g.cs", Ownership.GeneratorOwned),
            W("tests.api", "Tests.Api", "Tests/Api/Tests.Api.tt", "{TestOutputPath}/{Agg}.{Entity}ControllerTests.g.cs", Ownership.GeneratorOwned),
            W("tests.api-queries", "Tests.ApiQueries", "Tests/Api.Queries/Tests.ApiQueries.tt", "{TestOutputPath}/{Agg}.{Entity}QueryModelTests.g.cs", Ownership.GeneratorOwned),
            W("tests.enumerations", "Tests.Enumerations", "Tests/Enumerations/Tests.Enumerations.tt", "{TestOutputPath}/{Agg}.{Agg}EnumerationsTests.g.cs", Ownership.GeneratorOwned),
            W("tests.enumerations.none", "Tests.Enumerations", "Tests/Enumerations/Tests.Enumerations.tt", "{TestOutputPath}/NoEnumerations.g.cs", Ownership.GeneratorOwned),

            // WebApi (Blazor client/server generators)
            W("webapi.basic-address-component", "WebApi", "WebApi/BasicAddressComponent.tt", "{Agg}/{Entity}/Cadastro/{Entity}{Step}.razor.css", Ownership.GeneratorOwned),
            I("webapi.basic-configuracao-component", "WebApi", "WebApi/BasicConfiguracaoComponent.tt"),
            I("webapi.basic-contacts-component", "WebApi", "WebApi/BasicContactsComponent.tt"),
            I("webapi.basic-date-time-input", "WebApi", "WebApi/BasicDateTimeInput.tt"),
            I("webapi.basic-enum-input", "WebApi", "WebApi/BasicEnumInput.tt"),
            I("webapi.basic-foreign-data-drop-down-with-auto-complete", "WebApi", "WebApi/BasicForeignDataDropDownWithAutoComplete.tt"),
            I("webapi.basic-foreign-data-selector", "WebApi", "WebApi/BasicForeignDataSelector.tt"),
            I("webapi.basic-form-css", "WebApi", "WebApi/BasicForm.css.tt"),
            I("webapi.basic-form", "WebApi", "WebApi/BasicForm.tt"),
            I("webapi.basic-form-input-chooser", "WebApi", "WebApi/BasicFormInputChooser.tt"),
            I("webapi.basic-form-input-general-chooser", "WebApi", "WebApi/BasicFormInputGeneralChooser.tt"),
            I("webapi.basic-form-list-chooser", "WebApi", "WebApi/BasicFormListChooser.tt"),
            I("webapi.basic-image-input", "WebApi", "WebApi/BasicImageInput.tt"),
            I("webapi.basic-listbox-component", "WebApi", "WebApi/BasicListboxComponent.tt"),
            I("webapi.basic-mini-listing-picker-component", "WebApi", "WebApi/BasicMiniListingPickerComponent.tt"),
            I("webapi.basic-switch-toggler-input", "WebApi", "WebApi/BasicSwitchTogglerInput.tt"),
            I("webapi.basic-tab-bar-content-component", "WebApi", "WebApi/BasicTabBarContentComponent.tt"),
            I("webapi.basic-text-input", "WebApi", "WebApi/BasicTextInput.tt"),
            I("webapi.basic-text-input-title", "WebApi", "WebApi/BasicTextInputTitle.tt"),
            E("webapi.client-ioc-factory", "WebApi", "WebApi/ClientIoCFactory.tt"),
            W("webapi.client-listining-pages.index", "WebApi", "WebApi/ClientListiningPages.tt", "{Agg}/{Entity}/Index.razor", Ownership.GeneratorOwned),
            W("webapi.client-listining-pages.index-css", "WebApi", "WebApi/ClientListiningPages.tt", "{Agg}/{Entity}/Index.razor.css", Ownership.GeneratedOnce),
            E("webapi.client-pages-generator", "WebApi", "WebApi/ClientPagesGenerator.tt"),
            W("webapi.client-register-pages.page", "WebApi", "WebApi/ClientRegisterPages.tt", "{Agg}/{Entity}/Cadastro/Cadastro.razor", Ownership.GeneratorOwned),
            W("webapi.client-register-pages.css", "WebApi", "WebApi/ClientRegisterPages.tt", "{Agg}/{Entity}/Cadastro/Cadastro.razor.css", Ownership.GeneratorOwned),
            E("webapi.controllers", "WebApi", "WebApi/Controllers.tt"),
            E("webapi.dashboard-controllers", "WebApi", "WebApi/DashboardControllers.tt"),
            W("webapi.form-templates-switcher.css", "WebApi", "WebApi/FormTemplatesSwitcher.tt", "{Agg}/{Entity}/Cadastro/{Entity}{Step}.razor.css", Ownership.GeneratorOwned),
            W("webapi.form-templates-switcher.page", "WebApi", "WebApi/FormTemplatesSwitcher.tt", "{Agg}/{Entity}/Cadastro/{Entity}{Step}.razor", Ownership.GeneratorOwned),
            E("webapi.ioc-factory", "WebApi", "WebApi/IoCFactory.tt"),
            E("webapi.microservice-controller", "WebApi", "WebApi/MicroserviceController.tt"),
            E("webapi.migrations", "WebApi", "WebApi/Migrations.tt"),
            I("webapi.mini-basic-form-css", "WebApi", "WebApi/MiniBasicForm.css.tt"),
            I("webapi.mini-basic-form", "WebApi", "WebApi/MiniBasicForm.tt"),
            E("webapi.mini-client-ioc-factory", "WebApi", "WebApi/MiniClientIoCFactory.tt"),
            E("webapi.mini-client-pages-generator", "WebApi", "WebApi/MiniClientPagesGenerator.tt"),
            W("webapi.mini-client-register-pages.page", "WebApi", "WebApi/MiniClientRegisterPages.tt", "{Agg}/{Entity}/Cadastro/Cadastro.razor", Ownership.GeneratorOwned),
            W("webapi.mini-client-register-pages.css", "WebApi", "WebApi/MiniClientRegisterPages.tt", "{Agg}/{Entity}/Cadastro/Cadastro.razor.css", Ownership.GeneratorOwned),
            W("webapi.mini-controllers", "WebApi", "WebApi/MiniControllers.tt", "Controllers/Controllers.generated.cs", Ownership.GeneratorOwned),
            W("webapi.mini-form-templates-switcher.css", "WebApi", "WebApi/MiniFormTemplatesSwitcher.tt", "{Agg}/{Entity}/Cadastro/{Entity}{Step}.razor.css", Ownership.GeneratorOwned),
            W("webapi.mini-form-templates-switcher.page", "WebApi", "WebApi/MiniFormTemplatesSwitcher.tt", "{Agg}/{Entity}/Cadastro/{Entity}{Step}.razor", Ownership.GeneratorOwned),
            E("webapi.mini-ioc-factory", "WebApi", "WebApi/MiniIoCFactory.tt"),
            E("webapi.mini-migrations", "WebApi", "WebApi/MiniMigrations.tt"),
        };

        private static readonly Dictionary<string, TemplateRegistryEntry> _byId = _entries.ToDictionary(e => e.Id, StringComparer.Ordinal);

        public static IReadOnlyList<TemplateRegistryEntry> Entries { get { return _entries; } }

        public static TemplateRegistryEntry Find(string templateId)
        {
            if (string.IsNullOrEmpty(templateId)) return null;
            TemplateRegistryEntry e;
            return _byId.TryGetValue(templateId, out e) ? e : null;
        }

        public static bool TryGetOwnership(string templateId, out Ownership ownership)
        {
            var e = Find(templateId);
            ownership = e != null ? e.Ownership : Ownership.GeneratorOwned;
            return e != null;
        }

        /// <summary>Distinct template source files, in registry order.</summary>
        public static IEnumerable<string> SourceFiles
        {
            get { return _entries.Select(e => e.SourceFile).Distinct(StringComparer.OrdinalIgnoreCase); }
        }

        public static IEnumerable<TemplateRegistryEntry> ForSource(string sourceFile)
        {
            return _entries.Where(e => string.Equals(e.SourceFile, sourceFile, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>Maps a template source path (under src/ProjectFiles) to the package that carries it.</summary>
        public static string PackageForSource(string sourceFile)
        {
            var folder = TopFolder(sourceFile);
            if (folder == null) return CanonicalTemplatesPackageId;
            if (folder == "Tests")
            {
                var sub = SecondFolder(sourceFile);
                if (sub == null) return CanonicalTemplatesPackageId;
                return PackageForLayerFolder(sub);
            }
            return PackageForLayerFolder(folder);
        }

        public static string PackageForLayerFolder(string folder)
        {
            switch (folder)
            {
                case "Domain": return "Lazy.Crud.Builder.Domain";
                case "Application": return "Lazy.Crud.Builder.Application";
                case "Application.DTO": return "Lazy.Crud.Builder.Application.DTO";
                case "Infra.Data": return "Lazy.Crud.Builder.Infra.Data";
                case "Infra.IoC": return "Lazy.Crud.Builder.Infra.IoC";
                case "Api": return "Lazy.Crud.Builder.Api";
                case "Api.Queries": return "Lazy.Crud.Builder.Api.Queries";
                case "Enumerations": return "Lazy.Crud.Builder.Enumeration";
                case "WebApi": return "Lazy.Crud.Builder.WebApi";
                default: return CanonicalTemplatesPackageId;
            }
        }

        public static string InPackagePathForSource(string sourceFile)
        {
            return ContentRoot + sourceFile;
        }

        private static string TopFolder(string sourceFile)
        {
            var idx = sourceFile.IndexOf('/');
            return idx < 0 ? null : sourceFile.Substring(0, idx);
        }

        private static string SecondFolder(string sourceFile)
        {
            var idx = sourceFile.IndexOf('/');
            if (idx < 0) return null;
            var rest = sourceFile.Substring(idx + 1);
            var idx2 = rest.IndexOf('/');
            return idx2 < 0 ? null : rest.Substring(0, idx2);
        }

        /// <summary>
        /// Best-effort match of an existing output path (for example a v1 manifest entry) to the
        /// writer template that produces it. Returns null when no unambiguous match exists;
        /// migration never invents a template id.
        /// </summary>
        public static TemplateRegistryEntry TryMatchOutput(string relativePath, string aggregateName)
        {
            if (string.IsNullOrEmpty(relativePath)) return null;
            var fileName = relativePath.Replace('\\', '/');
            fileName = fileName.Substring(fileName.LastIndexOf('/') + 1);
            var agg = (aggregateName ?? string.Empty) + "Agg";
            TemplateRegistryEntry match = null;
            foreach (var e in _entries)
            {
                if (e.Kind != TemplateKind.Writer || e.OutputPattern == null) continue;
                if (e.OutputPattern.IndexOf("{Entity}", StringComparison.Ordinal) >= 0 || e.OutputPattern.IndexOf("{Step}", StringComparison.Ordinal) >= 0) continue;
                var patternFile = e.OutputPattern.Substring(e.OutputPattern.LastIndexOf('/') + 1)
                    .Replace("{Agg}", agg).Replace("{Aggregate}", aggregateName ?? string.Empty);
                if (string.Equals(patternFile, fileName, StringComparison.OrdinalIgnoreCase))
                {
                    if (match != null) return null; // ambiguous
                    match = e;
                }
            }
            return match;
        }
    }
}
