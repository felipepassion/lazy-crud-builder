using Lazy.Crud.Builder.Platform.Descriptor;
using Lazy.Crud.Builder.Platform.Tool;
using Lazy.Crud.Builder.Platform.Tool.Commands;
using Lazy.Crud.Builder.Platform.Tool.Descriptor;
using Lazy.Crud.Builder.Platform.Tool.Fixtures;
using Lazy.Crud.Builder.Platform.Tool.Pipeline;
using Lazy.Crud.Builder.Platform.Tool.Validation;
using Xunit;

namespace Lazy.Crud.Builder.Platform.Tests;

public class JsonSchemaLiteTests
{
    private const string Schema = @"{ ""type"": ""object"", ""required"": [""id"", ""kind""], ""additionalProperties"": false,
        ""properties"": { ""id"": { ""type"": ""string"", ""pattern"": ""^[a-z]+$"" }, ""kind"": { ""enum"": [""a"", ""b""] }, ""n"": { ""type"": ""integer"", ""minimum"": 1 },
                          ""items"": { ""type"": ""array"", ""items"": { ""$ref"": ""#/$defs/item"" } }, ""maybe"": { ""anyOf"": [ { ""type"": ""string"" }, { ""type"": ""null"" } ] } },
        ""$defs"": { ""item"": { ""type"": ""object"", ""required"": [""x""], ""properties"": { ""x"": { ""const"": 1 } } } } }";

    [Fact]
    public void ValidDocumentPasses()
    {
        var v = new JsonSchemaLite(Schema);
        Assert.Empty(v.Validate(@"{ ""id"": ""abc"", ""kind"": ""a"", ""n"": 2, ""items"": [ { ""x"": 1 } ], ""maybe"": null }"));
    }

    [Fact]
    public void ViolationsAreNamed()
    {
        var v = new JsonSchemaLite(Schema);
        var errors = v.Validate(@"{ ""id"": ""ABC"", ""kind"": ""c"", ""n"": 0, ""items"": [ { ""x"": 2 } ], ""extra"": true }");
        Assert.Contains(errors, e => e.Contains("$.id") && e.Contains("pattern"));
        Assert.Contains(errors, e => e.Contains("$.kind"));
        Assert.Contains(errors, e => e.Contains("$.n") && e.Contains("minimum"));
        Assert.Contains(errors, e => e.Contains("$.items[0].x"));
        Assert.Contains(errors, e => e.Contains("unexpected property 'extra'"));
        Assert.Contains(new JsonSchemaLite(Schema).Validate(@"{ ""id"": ""a"" }"), e => e.Contains("missing required property 'kind'"));
    }
}

public class FixtureAndSchemaTests
{
    private static string Repo => FindRepo();

    private static string FindRepo()
    {
        var dir = AppContext.BaseDirectory;
        while (dir != null && !File.Exists(Path.Combine(dir, "LazyCrudBuilder.sln"))) dir = Path.GetDirectoryName(dir);
        return dir ?? throw new InvalidOperationException("repository root not found");
    }

    private static PlatformDescriptor BuildDescriptor()
    {
        var attributes = new[]
        {
            new AttributeDescriptor { Name = "RequiredT4", FullName = "Lazy.Crud.Builder.Domain.Attributes.T4.RequiredT4", Namespace = "Lazy.Crud.Builder.Domain.Attributes.T4", PackageId = AttributeCatalogBuilder.AttributesPackageId, Category = "Validation", Applicability = new List<string> { "All" }, DocumentationId = "T:Lazy.Crud.Builder.Domain.Attributes.T4.RequiredT4", Overloads = { new AttributeOverloadDescriptor { Parameters = { new AttributeParameterDescriptor { Name = "errorMessage", Type = "string", Required = true } } } } },
        };
        return DescriptorBuilder.Build("1.1.0", attributes);
    }

    [Fact]
    public void DescriptorValidatesAgainstPublishedSchemaAndRoundTrips()
    {
        var d = BuildDescriptor();
        var json = d.ToJson();
        var schema = new JsonSchemaLite(File.ReadAllText(Path.Combine(Repo, "contracts", "schemas", "v1", "lazycrud.platform.schema.json")));
        Assert.Empty(schema.Validate(json));
        var back = PlatformDescriptor.FromJson(json, "test");
        Assert.Equal(d.Templates.Registry.Count, back.Templates.Registry.Count);
        Assert.Equal(TemplateRegistry.Entries.Count, back.Templates.Registry.Count);
        Assert.Equal("Lazy.Crud.Builder.Templates", back.Templates.PackageId);
        Assert.Contains("Lazy.Crud.Templates", back.Templates.AliasPackageIds);
        Assert.Equal(8, back.Layers.Count);
        Assert.Equal("Domain", back.Layers[0].Id);
        Assert.Null(back.Layers.Single(l => l.Id == "Enumerations").TemplatePackageId);
        Assert.Equal(8, back.TestLayers.Count);
        Assert.Equal("Microsoft.VisualStudio.Component.TextTemplating", back.Compatibility.HostRequirements!.T4Component);
        Assert.Contains("insiders", back.Compatibility.HostRequirements.VisualStudioChannels);
        Assert.Contains("generated-code-markers", back.Capabilities);
        Assert.All(back.Templates.Registry, r => Assert.True(OwnershipNames.TryParse(r.Ownership, out _), r.Id));
    }

    [Fact]
    public void FixturesAreDeterministicAndValidate()
    {
        var d = BuildDescriptor();
        var a = FixtureSet.Produce(d);
        var b = FixtureSet.Produce(d);
        Assert.Equal(a.Keys, b.Keys);
        foreach (var k in a.Keys) Assert.Equal(a[k], b[k]);
        var manifestSchema = new JsonSchemaLite(File.ReadAllText(Path.Combine(Repo, "contracts", "schemas", "v1", "manifest-v2.schema.json")));
        Assert.Empty(manifestSchema.Validate(a["manifest-v2.json"]));
        Assert.Empty(manifestSchema.Validate(a["manifest-v1-migrated.json"]));
        Assert.DoesNotContain("C:\\\\", a["manifest-v1-migrated.json"]);
        Assert.DoesNotContain("E:\\\\", a["manifest-v1-migrated.json"]);
        Assert.Contains("\"unresolved\": true", a["manifest-v1-migrated.json"]);
        Assert.True(a.Keys.Count(k => k.StartsWith("markers/")) == 16);
        var vectors = Platform.Json.JsonLite.ParseObject(a["hash-vectors.json"]).GetArray("vectors")!.Cast<Platform.Json.JsonObject>().ToList();
        Assert.Equal(8, vectors.Count);
        foreach (var v in vectors) Assert.Equal(v.GetString("expectedHash"), Platform.Hashing.ContentHasher.Compute(v.GetString("input")!).Value);
        Assert.Equal(vectors.Single(v => v.GetString("id") == "lf-body").GetString("expectedHash"), vectors.Single(v => v.GetString("id") == "crlf-body").GetString("expectedHash"));
        Assert.Equal(vectors.Single(v => v.GetString("id") == "lf-body").GetString("expectedHash"), vectors.Single(v => v.GetString("id") == "marker-excluded").GetString("expectedHash"));
        Assert.NotEqual(vectors.Single(v => v.GetString("id") == "lf-body").GetString("expectedHash"), vectors.Single(v => v.GetString("id") == "changed-body").GetString("expectedHash"));
        Assert.Equal("body-lf", vectors.Single(v => v.GetString("id") == "unmarked-json").GetString("hashScope"));
    }

    [Fact]
    public void ManifestV2SchemaRejectsAbsolutePaths()
    {
        var schema = new JsonSchemaLite(File.ReadAllText(Path.Combine(Repo, "contracts", "schemas", "v1", "manifest-v2.schema.json")));
        var bad = @"{ ""schemaVersion"": 2, ""aggregateName"": ""P"", ""generatedAtUtc"": ""2026-01-01T00:00:00Z"", ""generatorVersion"": ""1.1.0"", ""contractVersion"": 1,
                      ""files"": [ { ""relativePath"": ""C:/x/y.cs"", ""layer"": ""Domain"", ""templateId"": ""t"", ""templateVersion"": ""1"", ""ownership"": ""GeneratorOwned"", ""contentHash"": ""ab"", ""hashAlgorithm"": ""SHA-256"", ""hashScope"": ""body-lf"" } ] }";
        var errors = schema.Validate(bad);
        Assert.NotEmpty(errors);
    }
}

public class CallSiteScannerTests
{
    [Fact]
    public void ScansMultiLineAndPositionalCallSites()
    {
        var dir = Path.Combine(Path.GetTempPath(), "lazycrud-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(dir, "Domain"));
        File.WriteAllText(Path.Combine(dir, "project.tt"), "<#+ private void SaveOutputToSubFolder(string a, string b, bool c = true) { } #>");
        File.WriteAllText(Path.Combine(dir, "Domain", "A.tt"),
            "<# SaveOutputToSubFolder(\r\n    folderName: GetGenerationOutputPath(),\r\n    outputFileName: @$\"{AggName}.Filters.cs\",\r\n    recreateFileIfExists: true,\r\n    ownership: Ownership.GeneratorOwned,\r\n    templateId: \"domain.filters\"); #>\r\n" +
            "<# SaveOutputToSubFolder($\"{aggName}Agg/{entity.Name}\", @$\"Index.razor\", true); #>\r\n");
        var sites = CallSiteScanner.Scan(dir);
        Assert.Equal(2, sites.Count);
        Assert.Equal("GeneratorOwned", sites[0].Ownership);
        Assert.Equal("domain.filters", sites[0].TemplateId);
        Assert.Equal("Domain/A.tt", sites[0].SourceFile);
        Assert.Equal(1, sites[0].Line);
        Assert.False(sites[1].HasOwnership);
        Assert.False(sites[1].HasTemplateId);
        Assert.Equal(7, sites[1].Line);
    }
}

public class SandboxLayoutTests
{
    [Fact]
    public void ScaffoldWritesConsumerConventionProjects()
    {
        var root = Path.Combine(Path.GetTempPath(), "lazycrud-tests", Guid.NewGuid().ToString("N"));
        var sln = SandboxBuilder.Build(new SandboxOptions
        {
            SandboxRoot = root, AggregateName = "Shop", FixtureDirectory = null, PlatformVersion = "1.1.0",
            TemplateSource = new TemplateSource { Kind = TemplateSourceKind.NuGet, Location = "nuget", Version = "1.1.0" },
            LocalFeeds = new[] { Path.GetTempPath() },
        }, _ => { });
        Assert.True(File.Exists(sln));
        Assert.True(File.Exists(Path.Combine(root, "src", "Shop", "Shop.Domain", "Lazy.Crud.Shop.Domain.csproj")));
        Assert.True(File.Exists(Path.Combine(root, "src", "Shop", "Shop.Domain", "Aggregates", "ShopAgg", "Entities", "Shop.cs")));
        var wrapper = File.ReadAllText(Path.Combine(root, "src", "Shop", "Shop.Domain", "DefaultEntities.tt"));
        Assert.Contains("$(PkgLazy_Crud_Builder_Domain)\\contentFiles\\any\\any\\LazyCrudTemplates\\Domain\\DefaultEntities.tt", wrapper);
        var api = File.ReadAllText(Path.Combine(root, "src", "Shop", "Shop.Api", "Lazy.Crud.Shop.Api.csproj"));
        Assert.Contains("Microsoft.NET.Sdk.Web", api);
        Assert.Contains("Lazy.Crud.Shop.Infra.IoC.csproj", api);
        Assert.Contains("Controllers\\Controllers.tt", api);
        var settings = File.ReadAllText(Path.Combine(root, "src", "Mods", "lazy.settings"));
        Assert.Contains("TemplatesNuGetPackageId", settings);
        Assert.Contains(">NuGet<", settings);
        Assert.True(File.Exists(Path.Combine(root, "src", "Shop", "Tests", "Lazy.Crud.Shop.Tests.Domain", "DomainTest.tt")));
        try { Directory.Delete(root, true); } catch { }
    }
}

/// <summary>
/// The CI gates write their evidence into artifacts/validation/, a directory that does not exist in a
/// fresh clone. A passing validation used to die writing its own report (DirectoryNotFoundException,
/// exit 8), which reads as a broken repository rather than as a missing folder.
/// </summary>
public class ValidationReportOutputTests
{
    private static string FindRepo()
    {
        var dir = AppContext.BaseDirectory;
        while (dir != null && !File.Exists(Path.Combine(dir, "LazyCrudBuilder.sln"))) dir = Path.GetDirectoryName(dir);
        return dir ?? throw new InvalidOperationException("repository root not found");
    }

    [Fact]
    public void ReportIsWrittenIntoADirectoryThatDoesNotExistYet()
    {
        var reportDirectory = Path.Combine(Path.GetTempPath(), "lazycrud-tests", Guid.NewGuid().ToString("N"), "validation");
        var reportPath = Path.Combine(reportDirectory, "fixtures.json");
        Assert.False(Directory.Exists(reportDirectory));

        var exit = ValidateCommand.Run(CommandLine.Parse(new[] { "fixtures", "--repo", FindRepo(), "--report", reportPath }));

        try
        {
            Assert.Equal(ExitCodes.Success, exit);
            Assert.True(File.Exists(reportPath), "the validation report was not written");
            Assert.Contains("\"Scope\": \"fixtures\"", File.ReadAllText(reportPath), StringComparison.Ordinal);
        }
        finally
        {
            try { Directory.Delete(Path.GetDirectoryName(reportDirectory)!, true); } catch { /* best effort */ }
        }
    }
}
