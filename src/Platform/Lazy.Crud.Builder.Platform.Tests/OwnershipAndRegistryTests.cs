using Lazy.Crud.Builder.Platform.Descriptor;
using Xunit;

namespace Lazy.Crud.Builder.Platform.Tests;

public class OwnershipAndRegistryTests
{
    [Fact]
    public void LegacyBooleanMapsExactly()
    {
        Assert.Equal(Ownership.GeneratorOwned, OwnershipNames.FromLegacyRecreateFlag(true));
        Assert.Equal(Ownership.GeneratedOnce, OwnershipNames.FromLegacyRecreateFlag(false));
    }

    [Theory]
    [InlineData("GeneratorOwned", Ownership.GeneratorOwned)]
    [InlineData("GeneratedOnce", Ownership.GeneratedOnce)]
    [InlineData("Manual", Ownership.Manual)]
    public void CanonicalNamesRoundTrip(string name, Ownership expected)
    {
        Assert.True(OwnershipNames.TryParse(name, out var parsed));
        Assert.Equal(expected, parsed);
        Assert.Equal(name, OwnershipNames.ToName(expected));
    }

    [Theory]
    [InlineData("Replaceable")]
    [InlineData("UserOwned")]
    [InlineData("generatorowned")]
    [InlineData("")]
    public void SynonymsAreRejected(string name)
    {
        Assert.False(OwnershipNames.TryParse(name, out _));
    }

    [Fact]
    public void RegistryIdsAreUniqueAndStable()
    {
        var ids = TemplateRegistry.Entries.Select(e => e.Id).ToList();
        Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
        // Frozen id assignment: these ids are published in the descriptor and must never change silently.
        var frozen = new[]
        {
            "shared.project", "shared.tests-common",
            "domain.default-entities", "domain.default-commands", "domain.aggregate-settings-entity", "domain.user-entity",
            "domain.global-usings", "domain.command-handlers", "domain.command-handlers.seed", "domain.command-models",
            "domain.event-handlers", "domain.event-models", "domain.filters", "domain.irepositories",
            "domain.profiles-listining-mapping", "domain.profiles-mapping", "domain.specifications", "domain.csv-mapping-profiles",
            "application.default-commands", "application.global-usings", "application.app-services", "application.iapp-services",
            "application-dto.default-commands", "application-dto.global-usings", "application-dto.listinings",
            "application-dto.requests", "application-dto.requests.seed", "application-dto.steppable-requests",
            "application-dto.steppable-requests-validators", "application-dto.steppable-requests-validators.seed",
            "infra-data.default-commands", "infra-data.identity-context", "infra-data.global-usings", "infra-data.mappings", "infra-data.repositories",
            "infra-ioc.default-commands", "infra-ioc.global-usings", "infra-ioc.ioc-factory",
            "api-queries.default-commands", "api-queries.global-usings", "api-queries.query-models",
            "api.global-usings", "api.mini-controllers", "api.mini-ioc-factory", "api.mini-migrations",
            "tests.domain", "tests.application", "tests.application-dto", "tests.infra-data", "tests.infra-ioc", "tests.api", "tests.api-queries", "tests.enumerations", "tests.enumerations.none",
        };
        foreach (var id in frozen) Assert.Contains(id, ids);
    }

    [Fact]
    public void EveryWriterHasAnOutputPatternAndEveryEntryHasOwnership()
    {
        foreach (var e in TemplateRegistry.Entries)
        {
            Assert.False(string.IsNullOrWhiteSpace(e.Id));
            Assert.False(string.IsNullOrWhiteSpace(e.Layer));
            Assert.False(string.IsNullOrWhiteSpace(e.SourceFile));
            Assert.False(string.IsNullOrWhiteSpace(e.PackageId));
            Assert.StartsWith(TemplateRegistry.ContentRoot, e.InPackagePath);
            Assert.True(Enum.IsDefined(typeof(Ownership), e.Ownership));
            if (e.Kind == TemplateKind.Writer) Assert.False(string.IsNullOrWhiteSpace(e.OutputPattern), e.Id);
            else Assert.Null(e.OutputPattern);
        }
    }

    [Fact]
    public void PackageMappingFollowsTheDistributionStrategy()
    {
        Assert.Equal("Lazy.Crud.Builder.Templates", TemplateRegistry.Find("shared.project")!.PackageId);
        Assert.Equal("Lazy.Crud.Builder.Domain", TemplateRegistry.Find("domain.command-handlers")!.PackageId);
        Assert.Equal("Lazy.Crud.Builder.Domain", TemplateRegistry.Find("tests.domain")!.PackageId);
        Assert.Equal("Lazy.Crud.Builder.Enumeration", TemplateRegistry.Find("tests.enumerations")!.PackageId);
        Assert.Equal("Lazy.Crud.Builder.WebApi", TemplateRegistry.Find("webapi.mini-controllers")!.PackageId);
        Assert.Equal("contentFiles/any/any/LazyCrudTemplates/Domain/DomainCommandHandlers.tt", TemplateRegistry.Find("domain.command-handlers")!.InPackagePath);
    }

    [Fact]
    public void TryMatchOutputIsUnambiguousOrNull()
    {
        Assert.Equal("domain.command-handlers", TemplateRegistry.TryMatchOutput("src/Products/Products.Domain/LazyCode/ProductsAgg.DomainCommandHandlers.cs", "Products")!.Id);
        // {Agg}.cs is produced by several templates: ambiguous, so never guessed.
        Assert.Null(TemplateRegistry.TryMatchOutput("src/Products/Products.Domain/LazyCode/ProductsAgg.cs", "Products"));
        Assert.Null(TemplateRegistry.TryMatchOutput("src/Products/Products.Domain/DefaultEntities.generated.cs", "Products"));
    }
}
