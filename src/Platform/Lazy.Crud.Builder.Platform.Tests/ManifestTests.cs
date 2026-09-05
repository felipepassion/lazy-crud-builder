using Lazy.Crud.Builder.Platform.Diagnostics;
using Lazy.Crud.Builder.Platform.Hashing;
using Lazy.Crud.Builder.Platform.Manifests;
using Xunit;

namespace Lazy.Crud.Builder.Platform.Tests;

public class ManifestTests
{
    [Fact]
    public void V2RoundTripsAndNeverContainsAbsolutePaths()
    {
        var m = new GenerationManifest { AggregateName = "Products", GeneratorVersion = "1.1.0", ContractVersion = 1, SolutionRootMarker = "Sandbox.sln" };
        m.Upsert(new ManifestEntry
        {
            RelativePath = "src\\Products\\Products.Domain\\LazyCode\\ProductsAgg.Filters.cs", Layer = "Domain", TemplateId = "domain.filters",
            TemplateVersion = "1.1.0", Ownership = "GeneratorOwned", ContentHash = new string('a', 64), HashAlgorithm = "SHA-256", HashScope = HashScope.BodyLfMarkerExcluded,
        });
        var json = m.ToJson();
        Assert.Contains("\"schemaVersion\": 2", json);
        Assert.Contains("src/Products/Products.Domain/LazyCode/ProductsAgg.Filters.cs", json);
        Assert.DoesNotContain("\\\\", json);
        Assert.DoesNotContain("FullPath", json);
        var back = GenerationManifest.FromJsonV2(json);
        Assert.Equal("Products", back.AggregateName);
        Assert.Equal(1, back.ContractVersion);
        var e = Assert.Single(back.Files);
        Assert.Equal("domain.filters", e.TemplateId);
        Assert.True(e.HasTrustworthyBaseline);
        Assert.Equal(Ownership.GeneratorOwned, e.OwnershipValue);
    }

    [Fact]
    public void WriterRefusesAbsoluteOrBackslashPaths()
    {
        var m = new GenerationManifest { AggregateName = "P" };
        m.Files.Add(new ManifestEntry { RelativePath = "C:/x/y.cs", Ownership = "GeneratorOwned" });
        Assert.Throws<InvalidOperationException>(() => m.ToJson());
    }

    [Fact]
    public void SchemaVersionDetection()
    {
        Assert.Equal(1, GenerationManifest.DetectSchemaVersion("{ \"AggregateName\": \"Products\", \"GeneratedFiles\": [] }"));
        Assert.Equal(2, GenerationManifest.DetectSchemaVersion("{ \"schemaVersion\": 2 }"));
        Assert.Throws<FormatException>(() => GenerationManifest.FromJsonV2("{ \"AggregateName\": \"Products\" }"));
    }

    [Fact]
    public void V1MigrationPreservesEntriesAndNeverInventsData()
    {
        using var root = TempSolution.Create();
        var resolvable = Path.Combine(root.Path, "src", "Products", "Products.Domain", "LazyCode", "ProductsAgg.DomainCommandHandlers.cs");
        var residue = Path.Combine(root.Path, "src", "Products", "Products.Domain", "DefaultEntities.generated.cs");
        var v1 = "{\n" +
                 "  \"AggregateName\": \"Products\",\n" +
                 "  \"GeneratedAtUtc\": \"2026-05-09T09:44:52.3962907Z\",\n" +
                 "  \"GeneratorVersion\": \"1.0\",\n" +
                 "  \"GeneratedFiles\": [\n" +
                 "    { \"FullPath\": \"" + residue.Replace("\\", "\\\\") + "\", \"TemplateName\": \"Unknown\", \"Layer\": \"Domain\", \"ContentHash\": \"7eb70257593da06f682a3ddda54a9d260d4fc514f645237f5ca74b08f8da61a6\" },\n" +
                 "    { \"FullPath\": \"E:\\\\repos\\\\felipepassion\\\\lazy-crud-builder\\\\LazyCode\\\\Products\\\\DefaultEntities.generated.cs\", \"TemplateName\": \"OutputRedirection\", \"Layer\": \"Redirected\", \"ContentHash\": \"\" },\n" +
                 "    { \"FullPath\": \"" + resolvable.Replace("\\", "\\\\") + "\", \"TemplateName\": \"Unknown\", \"Layer\": \"Domain\", \"ContentHash\": \"\" }\n" +
                 "  ]\n}";
        var diags = new DiagnosticCollector();
        var m = ManifestMigration.MigrateV1(v1, root.Path, "Products", diags);

        Assert.True(m.MigratedFromV1);
        Assert.Equal(3, m.Files.Count);

        var first = m.Files[0];
        Assert.Equal("src/Products/Products.Domain/DefaultEntities.generated.cs", first.RelativePath);
        Assert.False(first.Unresolved);
        Assert.Null(first.ContentHash);                     // v1 hash is not the frozen rule: unknown baseline
        Assert.Equal("7eb70257593da06f682a3ddda54a9d260d4fc514f645237f5ca74b08f8da61a6", first.LegacyHash);
        Assert.False(first.HasTrustworthyBaseline);
        Assert.Equal(OwnershipNames.Unknown, first.Ownership);

        var second = m.Files[1];
        Assert.True(second.Unresolved);
        Assert.Null(second.RelativePath);
        Assert.Equal("DefaultEntities.generated.cs", second.FileName);
        Assert.Equal(64, second.OriginalPathSha256!.Length);
        Assert.Null(second.ContentHash);

        var third = m.Files[2];
        Assert.Equal("src/Products/Products.Domain/LazyCode/ProductsAgg.DomainCommandHandlers.cs", third.RelativePath);
        Assert.Equal("domain.command-handlers", third.TemplateId);   // unambiguous registry match
        Assert.Equal("GeneratorOwned", third.Ownership);
        Assert.Null(third.ContentHash);

        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.MigrationUnresolvedPath && d.Path.Contains("E:\\repos"));
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.MigrationUnknownHash);
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.MigrationUnknownOwnership);
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.ManifestMigrated);

        var json = m.ToJson();
        Assert.DoesNotContain("E:\\\\", json);
        Assert.DoesNotContain("FullPath", json);
        Assert.DoesNotContain(":\\\\", json);
        Assert.Contains("\"unresolved\": true", json);
        var reloaded = GenerationManifest.FromJsonV2(json);
        Assert.True(reloaded.Files[1].Unresolved);
    }

    [Fact]
    public void StoreDistinguishesAbsentFromUnreadable()
    {
        using var root = TempSolution.Create();
        var diags = new DiagnosticCollector();
        var absent = ManifestStore.Load(root.Path, "Nope", diags);
        Assert.True(absent.NotPresent);
        Assert.False(absent.Failed);
        Assert.Empty(diags.Items);

        var path = GenerationManifest.GetManifestPath(root.Path, "Broken");
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, "{ not json");
        var broken = ManifestStore.Load(root.Path, "Broken", diags);
        Assert.True(broken.Failed);
        Assert.False(broken.NotPresent);
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.ManifestUnreadable && d.Severity == DiagnosticSeverity.Error);
    }

    [Fact]
    public void StoreLoadsV1AndPersistsV2OnSave()
    {
        using var root = TempSolution.Create();
        var path = GenerationManifest.GetManifestPath(root.Path, "Products");
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, "{ \"AggregateName\": \"Products\", \"GeneratedFiles\": [ { \"FullPath\": \"Q:\\\\elsewhere\\\\x.cs\", \"TemplateName\": \"Unknown\", \"Layer\": \"Domain\", \"ContentHash\": \"\" } ] }");
        var diags = new DiagnosticCollector();
        var load = ManifestStore.Load(root.Path, "Products", diags);
        Assert.True(load.Loaded);
        Assert.True(load.Manifest!.MigratedFromV1);
        Assert.True(ManifestStore.Save(load.Manifest, root.Path, diags));
        var json = File.ReadAllText(path);
        Assert.Contains("\"schemaVersion\": 2", json);
        Assert.Contains("\"unresolved\": true", json);
        Assert.DoesNotContain("Q:\\\\", json);
        Assert.Equal("Sandbox.sln", GenerationManifest.FromJsonV2(json).SolutionRootMarker);
    }

    [Fact]
    public void SaveFailureIsReportedNotSwallowed()
    {
        using var root = TempSolution.Create();
        // Make ".lazycrud" a file so the manifests directory cannot be created.
        File.WriteAllText(Path.Combine(root.Path, GenerationManifest.ManifestDirectory), "blocker");
        var diags = new DiagnosticCollector();
        var ok = ManifestStore.Save(new GenerationManifest { AggregateName = "Products" }, root.Path, diags);
        Assert.False(ok);
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.ManifestSaveFailed && d.Severity == DiagnosticSeverity.Error);
    }

    [Fact]
    public void ConflictingManifestRootsAreReported()
    {
        using var root = TempSolution.Create();
        foreach (var r in new[] { root.Path, Path.Combine(root.Path, "src") })
        {
            var p = GenerationManifest.GetManifestPath(r, "Products");
            Directory.CreateDirectory(Path.GetDirectoryName(p)!);
            File.WriteAllText(p, "{}");
        }
        var diags = new DiagnosticCollector();
        var chosen = SolutionRoot.ResolveManifestRoot(root.Path, "Products", diags);
        Assert.Equal(root.Path, chosen);
        var d = Assert.Single(diags.Items);
        Assert.Equal(DiagnosticCodes.ManifestRootConflict, d.Code);
        Assert.Contains("src", d.Data["candidate2"]);
    }

    [Fact]
    public void RelativePathsResolveAfterTheSolutionMoves()
    {
        using var a = TempSolution.Create();
        using var b = TempSolution.Create();
        var m = new GenerationManifest { AggregateName = "Products" };
        m.Upsert(new ManifestEntry { RelativePath = "src/Products/Products.Domain/LazyCode/ProductsAgg.Filters.cs", Ownership = "GeneratorOwned", ContentHash = new string('b', 64), HashAlgorithm = "SHA-256", HashScope = HashScope.BodyLfMarkerExcluded });
        var json = m.ToJson();
        var reloaded = GenerationManifest.FromJsonV2(json);
        var underB = Path.Combine(b.Path, reloaded.Files[0].RelativePath!.Replace('/', Path.DirectorySeparatorChar));
        Assert.StartsWith(b.Path, underB);
        Assert.DoesNotContain(a.Path, json);
    }
}

/// <summary>A temporary solution root with a Sandbox.sln marker, deleted on dispose.</summary>
public sealed class TempSolution : IDisposable
{
    public string Path { get; }
    private TempSolution(string path) { Path = path; }

    public static TempSolution Create()
    {
        var dir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "lazycrud-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        File.WriteAllText(System.IO.Path.Combine(dir, "Sandbox.sln"), "");
        return new TempSolution(dir);
    }

    public void Dispose()
    {
        try { Directory.Delete(Path, true); } catch { /* best effort */ }
    }
}
