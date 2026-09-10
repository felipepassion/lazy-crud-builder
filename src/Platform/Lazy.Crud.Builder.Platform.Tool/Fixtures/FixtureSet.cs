using System.Text;
using Lazy.Crud.Builder.Platform.Descriptor;
using Lazy.Crud.Builder.Platform.Diagnostics;
using Lazy.Crud.Builder.Platform.Hashing;
using Lazy.Crud.Builder.Platform.Json;
using Lazy.Crud.Builder.Platform.Manifests;
using Lazy.Crud.Builder.Platform.Markers;

namespace Lazy.Crud.Builder.Platform.Tool.Fixtures;

/// <summary>
/// The canonical contract fixture set, produced by the same code paths that write the real
/// artifacts. Every file is deterministic for a given platform version, so a committed fixture that
/// differs from the current build fails the fixture check.
/// </summary>
public static class FixtureSet
{
    private static readonly UTF8Encoding Utf8 = new(false);

    /// <summary>The fixed solution root the v1 migration pair is anchored on (never a real machine path).</summary>
    public const string FixtureSolutionRoot = @"C:\lazycrud-fixture\solution";

    public static string VersionDirectory(string contractsRoot, int contractVersion) => Path.Combine(contractsRoot, "fixtures", "v" + contractVersion);

    /// <summary>Produces every fixture file (relative path -> content) for a descriptor.</summary>
    public static SortedDictionary<string, string> Produce(PlatformDescriptor descriptor)
    {
        var files = new SortedDictionary<string, string>(StringComparer.Ordinal);
        files["lazycrud.platform.json"] = descriptor.ToJson();
        files["manifest-v2.json"] = ManifestV2Fixture(descriptor.PlatformVersion);
        files["manifest-v1-input.json"] = ManifestV1Input;
        files["manifest-v1-migrated.json"] = ManifestV1Migrated();
        foreach (var (name, content) in MarkerFixtures(descriptor.PlatformVersion)) files["markers/" + name] = content;
        files["hash-vectors.json"] = HashVectors(descriptor.PlatformVersion);
        files["fixture-index.json"] = Index(descriptor, files.Keys);
        return files;
    }

    private static string Index(PlatformDescriptor d, IEnumerable<string> names)
    {
        var o = new JsonObject();
        o["contractVersion"] = (long)d.ContractVersion;
        o["platformVersion"] = d.PlatformVersion;
        o["descriptorSchema"] = "../../schemas/v" + d.ContractVersion + "/lazycrud.platform.schema.json";
        o["manifestSchema"] = "../../schemas/v" + d.ContractVersion + "/manifest-v2.schema.json";
        o["hashAlgorithm"] = ContentHasher.AlgorithmName;
        o["hashScopes"] = new List<object> { HashScope.BodyLfMarkerExcluded, HashScope.BodyLf };
        o["ownership"] = new List<object> { OwnershipNames.GeneratorOwned, OwnershipNames.GeneratedOnce, OwnershipNames.Manual };
        o["exitCodes"] = ExitCodes.Table.ToDictionary(kv => kv.Value, kv => (object)(long)kv.Key);
        o["diagnosticCodes"] = DiagnosticCodes.All.ToDictionary(kv => kv.Key, kv => (object)kv.Value);
        o["files"] = names.Where(n => n != "fixture-index.json").Cast<object>().ToList();
        o["rule"] = "When prose and fixture disagree, the fixture wins.";
        return JsonLite.Write(o, true);
    }

    public static string ManifestV2Fixture(string platformVersion)
    {
        var m = new GenerationManifest
        {
            AggregateName = "Products",
            GeneratedAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            GeneratorVersion = platformVersion,
            ContractVersion = PlatformInfo.ContractVersion,
            SolutionRootMarker = "Sandbox.sln",
        };
        var body = "namespace Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Filters;\r\n\r\npublic partial class ProductFilter { }\r\n";
        var hash = ContentHasher.ComputeBody(body, true);
        m.Upsert(new ManifestEntry
        {
            RelativePath = "src/Products/Products.Domain/LazyCode/ProductsAgg.Filters.cs", Layer = "Domain", TemplateId = "domain.filters",
            TemplateVersion = platformVersion, Ownership = OwnershipNames.GeneratorOwned, ContentHash = hash.Value, HashAlgorithm = hash.Algorithm, HashScope = hash.Scope,
        });
        var seed = "namespace Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Entities;\r\n\r\npublic partial class Products { }\r\n";
        var seedHash = ContentHasher.ComputeBody(seed, true);
        m.Upsert(new ManifestEntry
        {
            RelativePath = "src/Products/Products.Domain/Aggregates/ProductsAgg/Entities/Products.cs", Layer = "Domain", TemplateId = "domain.aggregate-settings-entity",
            TemplateVersion = platformVersion, Ownership = OwnershipNames.GeneratedOnce, ContentHash = seedHash.Value, HashAlgorithm = seedHash.Algorithm, HashScope = seedHash.Scope,
        });
        var bat = "dotnet ef migrations add Initial -c ProductsAggContext -p ..\\Products.Infra.Data\r\n";
        // Written by the T4 host after the writer decided ownership and rendered the REM marker.
        var batHash = ContentHasher.ComputeBody(bat, true);
        m.Upsert(new ManifestEntry
        {
            RelativePath = "src/Products/Products.Api/Migrations.generated.bat", Layer = "Api", TemplateId = "api.mini-migrations",
            TemplateVersion = platformVersion, Ownership = OwnershipNames.GeneratedOnce, ContentHash = batHash.Value, HashAlgorithm = batHash.Algorithm, HashScope = batHash.Scope,
        });
        // ToJson refreshes nothing: the timestamp stays fixed, so the fixture is deterministic.
        return m.ToJson();
    }

    public const string ManifestV1Input =
        "{\n" +
        "  \"AggregateName\": \"Products\",\n" +
        "  \"GeneratedAtUtc\": \"2026-05-09T09:44:52.3962907Z\",\n" +
        "  \"GeneratorVersion\": \"1.0\",\n" +
        "  \"GeneratedFiles\": [\n" +
        "    {\n" +
        "      \"FullPath\": \"C:\\\\lazycrud-fixture\\\\solution\\\\src\\\\Products\\\\Products.Domain\\\\LazyCode\\\\ProductsAgg.DomainCommandHandlers.cs\",\n" +
        "      \"TemplateName\": \"Unknown\",\n" +
        "      \"Layer\": \"Domain\",\n" +
        "      \"ContentHash\": \"7eb70257593da06f682a3ddda54a9d260d4fc514f645237f5ca74b08f8da61a6\"\n" +
        "    },\n" +
        "    {\n" +
        "      \"FullPath\": \"C:\\\\lazycrud-fixture\\\\solution\\\\src\\\\Products\\\\Products.Domain\\\\DefaultEntities.generated.cs\",\n" +
        "      \"TemplateName\": \"Unknown\",\n" +
        "      \"Layer\": \"Domain\",\n" +
        "      \"ContentHash\": \"\"\n" +
        "    },\n" +
        "    {\n" +
        "      \"FullPath\": \"E:\\\\repos\\\\felipepassion\\\\lazy-crud-builder\\\\LazyCode\\\\Products\\\\DefaultEntities.generated.cs\",\n" +
        "      \"TemplateName\": \"OutputRedirection\",\n" +
        "      \"Layer\": \"Redirected\",\n" +
        "      \"ContentHash\": \"\"\n" +
        "    },\n" +
        "    {\n" +
        "      \"FullPath\": \"C:\\\\lazycrud-fixture\\\\solution\\\\src\\\\Products\\\\Products.Domain\\\\LazyCode\\\\ProductsAgg.cs\",\n" +
        "      \"TemplateName\": \"Unknown\",\n" +
        "      \"Layer\": \"Domain\",\n" +
        "      \"ContentHash\": \"\"\n" +
        "    },\n" +
        "    {\n" +
        "      \"FullPath\": \"C:\\\\lazycrud-fixture\\\\solution\\\\src\\\\Products\\\\Products.Application\\\\LazyCode\\\\ProductsAgg.cs\",\n" +
        "      \"TemplateName\": \"Unknown\",\n" +
        "      \"Layer\": \"Application\",\n" +
        "      \"ContentHash\": \"\"\n" +
        "    },\n" +
        "    {\n" +
        "      \"FullPath\": \"C:\\\\lazycrud-fixture\\\\solution\\\\src\\\\Products\\\\Products.Application.DTO\\\\LazyCode\\\\ProductsAgg.ListiningDTO.cs\",\n" +
        "      \"TemplateName\": \"Unknown\",\n" +
        "      \"Layer\": \"Application.DTO\",\n" +
        "      \"ContentHash\": \"\"\n" +
        "    },\n" +
        "    {\n" +
        "      \"FullPath\": \"C:\\\\lazycrud-fixture\\\\solution\\\\src\\\\Products\\\\Products.Application.DTO\\\\Aggregates\\\\ProductsAgg\\\\Requests\\\\ProductDTO.cs\",\n" +
        "      \"TemplateName\": \"Unknown\",\n" +
        "      \"Layer\": \"Application.DTO\",\n" +
        "      \"ContentHash\": \"\"\n" +
        "    }\n" +
        "  ]\n" +
        "}\n";

    public static string ManifestV1Migrated()
    {
        var diagnostics = new DiagnosticCollector();
        var migrated = ManifestMigration.MigrateV1(ManifestV1Input, FixtureSolutionRoot, "Products", diagnostics);
        migrated.SolutionRootMarker = "Sandbox.sln";
        return migrated.ToJson();
    }

    public static IEnumerable<(string name, string content)> MarkerFixtures(string platformVersion)
    {
        var syntaxes = new (string ext, CommentSyntax syntax)[]
        {
            ("cs", CommentSyntax.CSharpLine), ("css", CommentSyntax.CBlock), ("razor", CommentSyntax.Razor),
            ("tt", CommentSyntax.T4), ("bat", CommentSyntax.Batch), ("sql", CommentSyntax.Sql), ("ps1", CommentSyntax.Hash), ("md", CommentSyntax.Html),
        };
        foreach (var ownership in new[] { Ownership.GeneratorOwned, Ownership.GeneratedOnce })
        {
            foreach (var (ext, syntax) in syntaxes)
            {
                var fields = new MarkerFields
                {
                    Aggregate = "Products", Layer = "Domain",
                    TemplateId = ownership == Ownership.GeneratorOwned ? "domain.command-handlers" : "domain.command-handlers.seed",
                    Ownership = ownership, GeneratorVersion = platformVersion, TemplateVersion = platformVersion,
                    SourceTemplate = "Domain/DomainCommandHandlers.tt",
                };
                yield return (OwnershipNames.ToName(ownership) + "." + ext + ".txt", GeneratedCodeMarker.Render(fields, syntax)!);
            }
        }
    }

    public static string HashVectors(string platformVersion)
    {
        var body = "namespace Lazy.Crud.Products.Domain;\n\npublic partial class Product\n{\n    public string Name { get; set; }\n}\n";
        var bodyCrlf = body.Replace("\n", "\r\n");
        var markerA = GeneratedCodeMarker.Render(new MarkerFields { Aggregate = "Products", Layer = "Domain", TemplateId = "domain.filters", Ownership = Ownership.GeneratorOwned, GeneratorVersion = platformVersion, TemplateVersion = platformVersion, SourceTemplate = "Domain/Filters.tt" }, CommentSyntax.CSharpLine)!;
        var markerB = GeneratedCodeMarker.Render(new MarkerFields { Aggregate = "Products", Layer = "Domain", TemplateId = "domain.filters", Ownership = Ownership.GeneratorOwned, GeneratorVersion = "0.9.0", TemplateVersion = "0.9.0", SourceTemplate = "Domain/Filters.tt" }, CommentSyntax.CSharpLine)!;
        var razorMarker = GeneratedCodeMarker.Render(new MarkerFields { Aggregate = "Products", Layer = "WebApi", TemplateId = "webapi.client-listining-pages.index", Ownership = Ownership.GeneratorOwned, GeneratorVersion = platformVersion, TemplateVersion = platformVersion, SourceTemplate = "WebApi/ClientListiningPages.tt" }, CommentSyntax.Razor)!;
        var json = "{\r\n  \"aggregate\": \"Products\"\r\n}\r\n";
        var changed = body.Replace("Name", "Title");

        var vectors = new List<object>();
        void Add(string id, string description, string input, string fileName)
        {
            var h = ContentHasher.Compute(input);
            var o = new JsonObject();
            o["id"] = id; o["description"] = description; o["fileName"] = fileName; o["input"] = input;
            o["expectedHash"] = h.Value; o["hashAlgorithm"] = h.Algorithm; o["hashScope"] = h.Scope;
            vectors.Add(o);
        }
        Add("lf-body", "Plain body with LF line endings and no marker: whole body is hashed.", body, "Product.cs");
        Add("crlf-body", "The same body with CRLF line endings: identical hash to lf-body.", bodyCrlf, "Product.cs");
        Add("marker-excluded", "Marker v2 header followed by the CRLF body: the marker block is excluded, so the hash equals lf-body.", markerA + bodyCrlf, "ProductsAgg.Filters.cs");
        Add("marker-version-changed", "A different marker (other generator version) with the same body: identical hash to marker-excluded.", markerB + bodyCrlf, "ProductsAgg.Filters.cs");
        Add("razor-marker-excluded", "Block-comment marker (Razor) followed by the body: excluded like any other syntax.", razorMarker + bodyCrlf, "Index.razor");
        Add("bom-body", "A UTF-8 byte-order mark before the body does not change the hash.", "\uFEFF" + bodyCrlf, "Product.cs");
        Add("unmarked-json", "A file type without a safe comment syntax carries no marker; the whole normalized body is hashed with scope body-lf.", json, "settings.json");
        Add("changed-body", "One identifier changed in the body: the hash differs from lf-body.", changed, "Product.cs");

        var root = new JsonObject();
        root["contractVersion"] = (long)PlatformInfo.ContractVersion;
        root["platformVersion"] = platformVersion;
        root["hashAlgorithm"] = ContentHasher.AlgorithmName;
        root["rule"] = "SHA-256 over the file body with line endings normalized to LF (CRLF and lone CR become LF), a leading UTF-8 BOM removed, and the leading Lazy CRUD marker block removed; lowercase hex.";
        root["markerBlockRule"] = "The marker block starts at the line containing <auto-generated> (line 1, or line 2 when line 1 is a bare comment opener such as /*, @*, <#/* or <!--) and ends after the line containing </auto-generated> (plus a following bare comment closer such as */, *@, */#> or -->). The body starts after that line's newline.";
        root["vectors"] = vectors;
        return JsonLite.Write(root, true);
    }

    public static void WriteAll(string directory, SortedDictionary<string, string> files)
    {
        Directory.CreateDirectory(directory);
        foreach (var (rel, content) in files)
        {
            var path = Path.Combine(directory, rel.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, content, Utf8);
        }
    }

    /// <summary>Compares committed fixtures with freshly produced ones; returns the list of differing or missing files.</summary>
    public static List<string> Diff(string directory, SortedDictionary<string, string> produced)
    {
        var problems = new List<string>();
        foreach (var (rel, content) in produced)
        {
            var path = Path.Combine(directory, rel.Replace('/', Path.DirectorySeparatorChar));
            if (!File.Exists(path)) { problems.Add("missing: " + rel); continue; }
            var committed = ContentHasher.NormalizeLineEndings(File.ReadAllText(path));
            if (!string.Equals(committed, ContentHasher.NormalizeLineEndings(content), StringComparison.Ordinal)) problems.Add("stale: " + rel);
        }
        return problems;
    }
}
