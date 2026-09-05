namespace Lazy.Crud.Builder.Platform.Tool.Pipeline;

/// <summary>A wrapper template seeded into an aggregate project; it includes a shipped template.</summary>
public sealed record WrapperSpec(string RelativeFile, string SourceTemplate);

/// <summary>One aggregate layer as consumers create it (mirrors the IDE extension's aggregate creator).</summary>
public sealed class LayerSpec
{
    public required string Id { get; init; }
    public string ProjectSuffix => "." + Id;
    public string ProjectPattern => "{Aggregate}." + Id;
    public required string RuntimePackageId { get; init; }
    public string Sdk { get; init; } = "Microsoft.NET.Sdk";
    /// <summary>Layers of the same aggregate referenced as project references.</summary>
    public IReadOnlyList<string> LocalReferences { get; init; } = Array.Empty<string>();
    /// <summary>Additional Lazy CRUD packages referenced by the layer project.</summary>
    public IReadOnlyList<string> ExtraPlatformPackages { get; init; } = Array.Empty<string>();
    /// <summary>Third-party packages referenced by the layer project.</summary>
    public IReadOnlyList<string> ThirdPartyPackages { get; init; } = Array.Empty<string>();
    public IReadOnlyList<WrapperSpec> Wrappers { get; init; } = Array.Empty<WrapperSpec>();
    /// <summary>Template folder inside the package content root.</summary>
    public string TemplateContentPath => TemplateRegistryContentRoot + Id + "/";
    public const string TemplateRegistryContentRoot = "contentFiles/any/any/LazyCrudTemplates/";
}

/// <summary>A generated test project for one layer.</summary>
public sealed class TestLayerSpec
{
    public required string Id { get; init; }
    public required string TargetLayer { get; init; }
    public required WrapperSpec Wrapper { get; init; }
    public string ProjectPattern => "Tests/Lazy.Crud.{Aggregate}." + Id;
}

/// <summary>
/// The canonical consumer layout: the layer set, their order, the wrapper templates each layer
/// carries, the project graph and package lists. This is the shape the IDE extension creates for a
/// new aggregate and the shape the descriptor publishes.
/// </summary>
public static class AggregateLayout
{
    /// <summary>Domain-first pipeline order (Domain is always first).</summary>
    public static readonly IReadOnlyList<LayerSpec> Layers = new[]
    {
        new LayerSpec
        {
            Id = "Domain", RuntimePackageId = "Lazy.Crud.Builder.Domain",
            LocalReferences = new[] { "Application.DTO" },
            ThirdPartyPackages = new[] { "Microsoft.EntityFrameworkCore.Relational", "Serilog" },
            Wrappers = new[] { new WrapperSpec("DefaultEntities.tt", "Domain/DefaultEntities.tt"), new WrapperSpec("DefaultCommands.tt", "Domain/DefaultCommands.tt") },
        },
        new LayerSpec
        {
            Id = "Application", RuntimePackageId = "Lazy.Crud.Builder.Application",
            LocalReferences = new[] { "Infra.Data" },
            ThirdPartyPackages = new[] { "Microsoft.EntityFrameworkCore.Relational", "Npgsql.EntityFrameworkCore.PostgreSQL", "Serilog" },
            Wrappers = new[] { new WrapperSpec("DefaultCommands.tt", "Application/DefaultCommands.tt") },
        },
        new LayerSpec
        {
            Id = "Application.DTO", RuntimePackageId = "Lazy.Crud.Builder.Application.DTO",
            LocalReferences = new[] { "Api.Queries" },
            ExtraPlatformPackages = new[] { "Lazy.Crud.CrossCutting.Infra.Utils" },
            Wrappers = new[] { new WrapperSpec("DefaultCommands.tt", "Application.DTO/DefaultCommands.tt") },
        },
        new LayerSpec
        {
            Id = "Infra.Data", RuntimePackageId = "Lazy.Crud.Builder.Infra.Data",
            LocalReferences = new[] { "Domain" },
            ThirdPartyPackages = new[] { "Microsoft.EntityFrameworkCore.Relational", "Npgsql.EntityFrameworkCore.PostgreSQL", "Serilog" },
            Wrappers = new[] { new WrapperSpec("DefaultCommands.tt", "Infra.Data/DefaultCommands.tt") },
        },
        new LayerSpec
        {
            Id = "Infra.IoC", RuntimePackageId = "Lazy.Crud.Builder.Infra.IoC",
            LocalReferences = new[] { "Application", "Infra.Data" },
            ThirdPartyPackages = new[] { "Microsoft.AspNetCore.Mvc.NewtonsoftJson", "Microsoft.Extensions.DependencyInjection.Abstractions", "Microsoft.EntityFrameworkCore.Relational", "Npgsql.EntityFrameworkCore.PostgreSQL", "Serilog" },
            Wrappers = new[] { new WrapperSpec("DefaultCommands.tt", "Infra.IoC/DefaultCommands.tt") },
        },
        new LayerSpec
        {
            Id = "Api.Queries", RuntimePackageId = "Lazy.Crud.Builder.Api.Queries",
            LocalReferences = new[] { "Enumerations" },
            Wrappers = new[] { new WrapperSpec("DefaultCommands.tt", "Api.Queries/DefaultCommands.tt") },
        },
        new LayerSpec
        {
            Id = "Enumerations", RuntimePackageId = "Lazy.Crud.Builder.Enumeration",
        },
        new LayerSpec
        {
            Id = "Api", RuntimePackageId = "Lazy.Crud.Builder.Api", Sdk = "Microsoft.NET.Sdk.Web",
            LocalReferences = new[] { "Infra.IoC" },
            ThirdPartyPackages = new[] { "Microsoft.AspNetCore.Mvc.NewtonsoftJson", "Swashbuckle.AspNetCore", "Microsoft.EntityFrameworkCore.Design", "Microsoft.EntityFrameworkCore.Relational", "Npgsql.EntityFrameworkCore.PostgreSQL", "Serilog" },
            Wrappers = new[]
            {
                new WrapperSpec("ServerIoCFactory.tt", "Api/MiniIoCFactory.tt"),
                new WrapperSpec("Migrations.tt", "Api/MiniMigrations.tt"),
                new WrapperSpec("Controllers/Controllers.tt", "Api/MiniControllers.tt"),
            },
        },
    };

    public static readonly IReadOnlyList<TestLayerSpec> TestLayers = new[]
    {
        new TestLayerSpec { Id = "Tests.Domain", TargetLayer = "Domain", Wrapper = new WrapperSpec("DomainTest.tt", "Tests/Domain/Tests.Domain.tt") },
        new TestLayerSpec { Id = "Tests.Application", TargetLayer = "Application", Wrapper = new WrapperSpec("ApplicationTest.tt", "Tests/Application/Tests.Application.tt") },
        new TestLayerSpec { Id = "Tests.DTO", TargetLayer = "Application.DTO", Wrapper = new WrapperSpec("DtoTest.tt", "Tests/Application.DTO/Tests.DTO.tt") },
        new TestLayerSpec { Id = "Tests.InfraData", TargetLayer = "Infra.Data", Wrapper = new WrapperSpec("InfraDataTest.tt", "Tests/Infra.Data/Tests.InfraData.tt") },
        new TestLayerSpec { Id = "Tests.InfraIoC", TargetLayer = "Infra.IoC", Wrapper = new WrapperSpec("InfraIoCTest.tt", "Tests/Infra.IoC/Tests.InfraIoC.tt") },
        new TestLayerSpec { Id = "Tests.Api", TargetLayer = "Api", Wrapper = new WrapperSpec("ApiTest.tt", "Tests/Api/Tests.Api.tt") },
        new TestLayerSpec { Id = "Tests.ApiQueries", TargetLayer = "Api.Queries", Wrapper = new WrapperSpec("ApiQueriesTest.tt", "Tests/Api.Queries/Tests.ApiQueries.tt") },
        new TestLayerSpec { Id = "Tests.Enumerations", TargetLayer = "Enumerations", Wrapper = new WrapperSpec("EnumerationsTest.tt", "Tests/Enumerations/Tests.Enumerations.tt") },
    };

    /// <summary>Third-party package versions pinned for sandbox consumers (aligned with the platform's central versions).</summary>
    public static readonly IReadOnlyDictionary<string, string> ThirdPartyVersions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["Microsoft.EntityFrameworkCore.Relational"] = "10.0.10",
        ["Microsoft.EntityFrameworkCore.Design"] = "10.0.10",
        ["Npgsql.EntityFrameworkCore.PostgreSQL"] = "10.0.3",
        ["Serilog"] = "4.4.0",
        ["Microsoft.AspNetCore.Mvc.NewtonsoftJson"] = "10.0.10",
        ["Microsoft.Extensions.DependencyInjection.Abstractions"] = "10.0.10",
        ["Swashbuckle.AspNetCore"] = "7.3.1",
        ["Microsoft.NET.Test.Sdk"] = "17.13.0",
        ["xunit"] = "2.9.3",
        ["xunit.runner.visualstudio"] = "3.0.2",
    };

    public static LayerSpec? FindLayer(string id) => Layers.FirstOrDefault(l => string.Equals(l.Id, id, StringComparison.OrdinalIgnoreCase));

    public static int LayerIndex(string id)
    {
        for (int i = 0; i < Layers.Count; i++) if (string.Equals(Layers[i].Id, id, StringComparison.OrdinalIgnoreCase)) return i;
        return Layers.Count;
    }

    /// <summary>The MSBuild property NuGet generates for a package reference with GeneratePathProperty.</summary>
    public static string PackagePathProperty(string packageId) => "$(Pkg" + packageId.Replace('.', '_') + ")";

    /// <summary>Package carrying a template source path (shared templates come from the canonical package).</summary>
    public static string PackageForTemplate(string sourceTemplate)
    {
        return global::Lazy.Crud.Builder.Platform.Descriptor.TemplateRegistry.PackageForSource(sourceTemplate.Replace('\\', '/'));
    }
}
