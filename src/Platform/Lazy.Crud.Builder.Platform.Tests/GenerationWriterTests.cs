using System.Text;
using Lazy.Crud.Builder.Platform.Diagnostics;
using Lazy.Crud.Builder.Platform.Hashing;
using Lazy.Crud.Builder.Platform.Manifests;
using Lazy.Crud.Builder.Platform.Markers;
using Lazy.Crud.Builder.Platform.Writer;
using Xunit;

namespace Lazy.Crud.Builder.Platform.Tests;

public class GenerationWriterTests
{
    private const string Body = "namespace Lazy.Crud.Products.Domain;\r\npublic partial class ProductsFilters { }\r\n";

    private static WriteRequest Request(TempSolution root, string content = Body, string templateId = "domain.filters", Ownership? ownership = null, bool recreate = true, string fileName = "ProductsAgg.Filters.cs")
    {
        return new WriteRequest
        {
            SolutionRoot = root.Path,
            AggregateName = "Products",
            Layer = "Domain",
            TemplateId = templateId,
            Ownership = ownership,
            RecreateFileIfExists = recreate,
            OutputDirectory = Path.Combine(root.Path, "src", "Products", "Products.Domain", "LazyCode"),
            OutputFileName = fileName,
            Content = content,
            SourceTemplate = "Domain/Filters.tt",
        };
    }

    private static string Rel(string fileName = "ProductsAgg.Filters.cs") => "src/Products/Products.Domain/LazyCode/" + fileName;

    [Fact]
    public void NewFileIsWrittenWithMarkerAndRecordedInManifest()
    {
        using var root = TempSolution.Create();
        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(Request(root), diags);

        Assert.Equal(WriteOutcome.Written, result.Outcome);
        Assert.Equal(FileState.New, result.State);
        var text = File.ReadAllText(result.AbsolutePath!);
        Assert.True(GeneratedCodeMarker.TryParse(text, out var marker));
        Assert.True(marker!.IsV2);
        Assert.Equal(Ownership.GeneratorOwned, marker.Ownership);
        Assert.Equal("domain.filters", marker.Get("TemplateId"));
        Assert.Equal(PlatformInfo.Version, marker.Get("GeneratorVersion"));
        Assert.EndsWith(Body, text);

        var manifest = ManifestStore.Load(root.Path, "Products", diags).Manifest!;
        var entry = Assert.Single(manifest.Files);
        Assert.Equal(Rel(), entry.RelativePath);
        Assert.Equal("GeneratorOwned", entry.Ownership);
        Assert.Equal("domain.filters", entry.TemplateId);
        Assert.Equal(ContentHasher.Compute(text).Value, entry.ContentHash);
        Assert.Equal(HashScope.BodyLfMarkerExcluded, entry.HashScope);
        Assert.Equal("SHA-256", entry.HashAlgorithm);
        Assert.Equal(PlatformInfo.Version, manifest.GeneratorVersion);
        Assert.Equal(PlatformInfo.ContractVersion, manifest.ContractVersion);
        Assert.DoesNotContain(diags.Items, d => d.Severity == DiagnosticSeverity.Error);
    }

    [Fact]
    public void VerifiedFileIsOverwrittenWithoutDrift()
    {
        using var root = TempSolution.Create();
        GenerationWriter.Write(Request(root), null);
        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(Request(root, Body + "// v2\r\n"), diags);
        Assert.Equal(WriteOutcome.Written, result.Outcome);
        Assert.Equal(FileState.Verified, result.State);
        Assert.DoesNotContain(diags.Items, d => d.Code == DiagnosticCodes.Drifted);
        Assert.EndsWith("// v2\r\n", File.ReadAllText(result.AbsolutePath!));
    }

    [Fact]
    public void DriftedFileIsProtectedByDefaultAndTheEditSurvives()
    {
        using var root = TempSolution.Create();
        var first = GenerationWriter.Write(Request(root), null);
        var edited = File.ReadAllText(first.AbsolutePath!) + "// human edit\r\n";
        File.WriteAllText(first.AbsolutePath!, edited);

        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(Request(root, Body + "// regenerated\r\n"), diags);

        Assert.Equal(WriteOutcome.Protected, result.Outcome);
        Assert.Equal(FileState.Drifted, result.State);
        Assert.Equal(edited, File.ReadAllText(first.AbsolutePath!));
        var d = Assert.Single(diags.Items, x => x.Code == DiagnosticCodes.Drifted);
        Assert.Equal(DiagnosticSeverity.Error, d.Severity);
        Assert.Equal(Rel(), d.Path);
        Assert.Equal("domain.filters", d.Data["templateId"]);
        Assert.Equal(64, d.Data["expectedHash"].Length);
        Assert.Equal(64, d.Data["actualHash"].Length);
        Assert.Contains("force", d.Message);
        Assert.Contains("skip-modified", d.Message);
        Assert.True(diags.HasErrors);
        // The diagnostics file for headless runs carries the same record.
        var fileDiags = DiagnosticsFile.For(root.Path, "Products").Read();
        Assert.Contains(fileDiags, x => x.Code == DiagnosticCodes.Drifted);
    }

    [Fact]
    public void ForceOverwritesDriftAndReportsIt()
    {
        using var root = TempSolution.Create();
        var first = GenerationWriter.Write(Request(root), null);
        File.WriteAllText(first.AbsolutePath!, File.ReadAllText(first.AbsolutePath!) + "// human edit\r\n");
        new RunContext { Resolution = ResolutionMode.Force }.Save(root.Path, "Products");

        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(Request(root, Body + "// regenerated\r\n"), diags);
        Assert.Equal(WriteOutcome.Written, result.Outcome);
        Assert.EndsWith("// regenerated\r\n", File.ReadAllText(first.AbsolutePath!));
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.ForcedOverwrite);
        Assert.False(diags.HasErrors);
    }

    [Fact]
    public void SkipModifiedPreservesTheFileAndSucceeds()
    {
        using var root = TempSolution.Create();
        var first = GenerationWriter.Write(Request(root), null);
        var edited = File.ReadAllText(first.AbsolutePath!) + "// human edit\r\n";
        File.WriteAllText(first.AbsolutePath!, edited);
        new RunContext { Resolution = ResolutionMode.SkipModified }.Save(root.Path, "Products");

        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(Request(root, Body + "// regenerated\r\n"), diags);
        Assert.Equal(WriteOutcome.SkippedByFlag, result.Outcome);
        Assert.Equal(edited, File.ReadAllText(first.AbsolutePath!));
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.SkippedModified);
        Assert.False(diags.HasErrors);
    }

    [Fact]
    public void MissingFileIsRegeneratedWithoutDrift()
    {
        using var root = TempSolution.Create();
        var first = GenerationWriter.Write(Request(root), null);
        File.Delete(first.AbsolutePath!);
        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(Request(root), diags);
        Assert.Equal(WriteOutcome.Written, result.Outcome);
        Assert.Equal(FileState.Missing, result.State);
        Assert.DoesNotContain(diags.Items, d => d.Code == DiagnosticCodes.Drifted || d.Code == DiagnosticCodes.Unverified);
    }

    [Fact]
    public void GeneratedOnceIsCreatedWhenAbsentAndLeftWhenPresent()
    {
        using var root = TempSolution.Create();
        var req = Request(root, "public partial class Product { }\r\n", "domain.aggregate-settings-entity", fileName: "Products.cs");
        req.OutputDirectory = Path.Combine(root.Path, "src", "Products", "Products.Domain", "Aggregates", "ProductsAgg", "Entities");
        var created = GenerationWriter.Write(req, null);
        Assert.Equal(WriteOutcome.Written, created.Outcome);
        Assert.Equal(Ownership.GeneratedOnce, created.Ownership);
        var text = File.ReadAllText(created.AbsolutePath!);
        Assert.Contains("Ownership: GeneratedOnce", text);
        Assert.DoesNotContain("overwritten", text);

        File.WriteAllText(created.AbsolutePath!, "// user content\r\n");
        var diags = new DiagnosticCollector();
        var again = GenerationWriter.Write(req, diags);
        Assert.Equal(WriteOutcome.SkippedExisting, again.Outcome);
        Assert.Equal("// user content\r\n", File.ReadAllText(created.AbsolutePath!));
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.GeneratedOnceSkipped);
        Assert.False(diags.HasErrors);
        var manifest = ManifestStore.Load(root.Path, "Products", diags).Manifest!;
        Assert.Equal("GeneratedOnce", manifest.Files.Single().Ownership);
    }

    [Fact]
    public void LegacyBooleanFalseMeansGeneratedOnceWhenNoOwnershipOrRegistryEntry()
    {
        using var root = TempSolution.Create();
        var req = Request(root, "seed", templateId: null, recreate: false, fileName: "Seed.cs");
        var first = GenerationWriter.Write(req, null);
        Assert.Equal(Ownership.GeneratedOnce, first.Ownership);
        File.WriteAllText(first.AbsolutePath!, "edited");
        var second = GenerationWriter.Write(req, null);
        Assert.Equal(WriteOutcome.SkippedExisting, second.Outcome);
        Assert.Equal("edited", File.ReadAllText(first.AbsolutePath!));
    }

    [Fact]
    public void ExplicitOwnershipOverridesTheBoolean()
    {
        using var root = TempSolution.Create();
        var req = Request(root, "x", templateId: "domain.filters", ownership: Ownership.GeneratedOnce, recreate: true);
        var first = GenerationWriter.Write(req, null);
        Assert.Equal(Ownership.GeneratedOnce, first.Ownership);
        var second = GenerationWriter.Write(req, null);
        Assert.Equal(WriteOutcome.SkippedExisting, second.Outcome);
    }

    [Fact]
    public void UnregisteredTemplateIsUserOwnedAndReported()
    {
        using var root = TempSolution.Create();
        var req = Request(root, "x", templateId: "nope.unregistered", recreate: true, fileName: "Unregistered.cs");
        var diags = new DiagnosticCollector();
        var first = GenerationWriter.Write(req, diags);
        Assert.Equal(WriteOutcome.Written, first.Outcome);
        Assert.Equal(Ownership.GeneratedOnce, first.Ownership);
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.UnregisteredTemplate && d.Data["templateId"] == "nope.unregistered");
        File.WriteAllText(first.AbsolutePath!, "edited");
        Assert.Equal(WriteOutcome.SkippedExisting, GenerationWriter.Write(req, null).Outcome);
        Assert.Equal("edited", File.ReadAllText(first.AbsolutePath!));
    }

    [Fact]
    public void ManualIsNeverWrittenEvenWithForce()
    {
        using var root = TempSolution.Create();
        new RunContext { Resolution = ResolutionMode.Force }.Save(root.Path, "Products");
        var req = Request(root, "x", ownership: Ownership.Manual, fileName: "Manual.cs");
        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(req, diags);
        Assert.Equal(WriteOutcome.RefusedManual, result.Outcome);
        Assert.False(File.Exists(result.AbsolutePath!));
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.ManualPathRefused);
    }

    [Fact]
    public void UnverifiedIdenticalContentIsAdoptedAndGetsAMarker()
    {
        using var root = TempSolution.Create();
        var req = Request(root);
        Directory.CreateDirectory(req.OutputDirectory);
        var abs = Path.Combine(req.OutputDirectory, req.OutputFileName);
        // A legacy file with no manifest entry, no marker and identical body.
        File.WriteAllText(abs, Body, new UTF8Encoding(true));

        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(req, diags);
        Assert.Equal(WriteOutcome.Adopted, result.Outcome);
        Assert.Equal(FileState.Unverified, result.State);
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.UnverifiedAdopted);
        Assert.False(diags.HasErrors);
        var text = File.ReadAllText(abs);
        Assert.True(GeneratedCodeMarker.TryParse(text, out var marker) && marker!.IsV2);
        var entry = ManifestStore.Load(root.Path, "Products", null).Manifest!.Files.Single();
        Assert.Equal(ContentHasher.Compute(text).Value, entry.ContentHash);
        Assert.True(entry.HasTrustworthyBaseline);
    }

    [Fact]
    public void UnverifiedDifferentContentIsProtectedWithDistinctCode()
    {
        using var root = TempSolution.Create();
        var req = Request(root);
        Directory.CreateDirectory(req.OutputDirectory);
        var abs = Path.Combine(req.OutputDirectory, req.OutputFileName);
        File.WriteAllText(abs, Body + "// pre-manifest user edit\r\n");

        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(req, diags);
        Assert.Equal(WriteOutcome.Protected, result.Outcome);
        Assert.Equal(FileState.Unverified, result.State);
        Assert.Equal(Body + "// pre-manifest user edit\r\n", File.ReadAllText(abs));
        var d = Assert.Single(diags.Items, x => x.Code == DiagnosticCodes.Unverified);
        Assert.DoesNotContain(diags.Items, x => x.Code == DiagnosticCodes.Drifted);
        Assert.Contains("baseline", d.Message);
        Assert.True(diags.HasErrors);
    }

    [Fact]
    public void MigratedV1EntryWithUnknownHashIsProtectedWhenContentDiffers()
    {
        using var root = TempSolution.Create();
        var req = Request(root);
        Directory.CreateDirectory(req.OutputDirectory);
        var abs = Path.Combine(req.OutputDirectory, req.OutputFileName);
        File.WriteAllText(abs, Body + "// old generated content\r\n");
        var manifestPath = GenerationManifest.GetManifestPath(root.Path, "Products");
        Directory.CreateDirectory(Path.GetDirectoryName(manifestPath)!);
        File.WriteAllText(manifestPath, "{ \"AggregateName\": \"Products\", \"GeneratorVersion\": \"1.0\", \"GeneratedFiles\": [ { \"FullPath\": \"" + abs.Replace("\\", "\\\\") + "\", \"TemplateName\": \"Unknown\", \"Layer\": \"Domain\", \"ContentHash\": \"\" } ] }");

        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(req, diags);
        Assert.Equal(WriteOutcome.Protected, result.Outcome);
        Assert.Equal(Body + "// old generated content\r\n", File.ReadAllText(abs));
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.Unverified && d.Path == Rel());
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.MigrationUnknownHash);
        Assert.DoesNotContain(diags.Items, d => d.Code == DiagnosticCodes.Drifted);
    }

    [Fact]
    public void MigratedV1EntryWithIdenticalContentIsAdoptedAndManifestBecomesV2()
    {
        using var root = TempSolution.Create();
        var req = Request(root);
        Directory.CreateDirectory(req.OutputDirectory);
        var abs = Path.Combine(req.OutputDirectory, req.OutputFileName);
        File.WriteAllText(abs, Body);
        var manifestPath = GenerationManifest.GetManifestPath(root.Path, "Products");
        Directory.CreateDirectory(Path.GetDirectoryName(manifestPath)!);
        File.WriteAllText(manifestPath, "{ \"AggregateName\": \"Products\", \"GeneratorVersion\": \"1.0\", \"GeneratedFiles\": [ { \"FullPath\": \"" + abs.Replace("\\", "\\\\") + "\", \"TemplateName\": \"Unknown\", \"Layer\": \"Domain\", \"ContentHash\": \"\" }, { \"FullPath\": \"E:\\\\gone\\\\x.cs\", \"TemplateName\": \"Unknown\", \"Layer\": \"Domain\", \"ContentHash\": \"\" } ] }");

        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(req, diags);
        Assert.Equal(WriteOutcome.Adopted, result.Outcome);
        Assert.False(diags.HasErrors);
        var json = File.ReadAllText(manifestPath);
        Assert.Contains("\"schemaVersion\": 2", json);
        Assert.Contains("\"unresolved\": true", json);
        var m = GenerationManifest.FromJsonV2(json);
        var entry = m.Files.Single(f => !f.Unresolved);
        Assert.True(entry.HasTrustworthyBaseline);
        Assert.Equal("domain.filters", entry.TemplateId);
    }

    [Fact]
    public void LockedFileFailsLoudly()
    {
        using var root = TempSolution.Create();
        var first = GenerationWriter.Write(Request(root), null);
        using (var lockStream = new FileStream(first.AbsolutePath!, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
            var diags = new DiagnosticCollector();
            var result = GenerationWriter.Write(Request(root), diags);
            Assert.Equal(WriteOutcome.Failed, result.Outcome);
            Assert.Contains(diags.Items, d => d.Severity == DiagnosticSeverity.Error);
            Assert.True(diags.HasErrors);
        }
    }

    [Fact]
    public void UnwritableOutputDirectoryIsReported()
    {
        using var root = TempSolution.Create();
        var req = Request(root);
        // A file where the output directory should be.
        Directory.CreateDirectory(Path.GetDirectoryName(req.OutputDirectory)!);
        File.WriteAllText(req.OutputDirectory, "blocker");
        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(req, diags);
        Assert.Equal(WriteOutcome.Failed, result.Outcome);
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.DirectoryCreateFailed || d.Code == DiagnosticCodes.FileWriteFailed);
    }

    [Fact]
    public void ManifestSaveFailureFailsTheWrite()
    {
        using var root = TempSolution.Create();
        File.WriteAllText(Path.Combine(root.Path, GenerationManifest.ManifestDirectory), "blocker");
        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(Request(root), diags);
        Assert.Equal(WriteOutcome.Failed, result.Outcome);
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.ManifestSaveFailed);
    }

    [Fact]
    public void MarkersCanBeDisabledByTheCapabilitySwitch()
    {
        using var root = TempSolution.Create();
        var mods = Path.Combine(root.Path, "src", "Mods");
        Directory.CreateDirectory(mods);
        File.WriteAllText(Path.Combine(mods, "lazy.settings"),
            "<?xml version=\"1.0\" encoding=\"utf-8\"?><SettingsFile><Settings><Setting Name=\"GeneratedCodeMarkers\" Type=\"System.Boolean\" Scope=\"Application\"><Value Profile=\"(Default)\">False</Value></Setting></Settings></SettingsFile>");
        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(Request(root), diags);
        Assert.Equal(WriteOutcome.Written, result.Outcome);
        Assert.Equal(Body, File.ReadAllText(result.AbsolutePath!));
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.CapabilityDisabled);
        var entry = ManifestStore.Load(root.Path, "Products", null).Manifest!.Files.Single();
        Assert.Equal(HashScope.BodyLf, entry.HashScope);
    }

    [Fact]
    public void UnmarkableFileTypeIsTrackedWithWholeBodyHash()
    {
        using var root = TempSolution.Create();
        var req = Request(root, "{ \"a\": 1 }", fileName: "data.json");
        var result = GenerationWriter.Write(req, null);
        Assert.Equal(WriteOutcome.Written, result.Outcome);
        Assert.Equal("{ \"a\": 1 }", File.ReadAllText(result.AbsolutePath!));
        var entry = ManifestStore.Load(root.Path, "Products", null).Manifest!.Files.Single();
        Assert.Equal(HashScope.BodyLf, entry.HashScope);
        Assert.Equal(ContentHasher.Compute("{ \"a\": 1 }").Value, entry.ContentHash);
    }

    [Fact]
    public void WholeAggregateStatesAreResolvedBeforeAnyWrite()
    {
        using var root = TempSolution.Create();
        var a = GenerationWriter.Write(Request(root, "a", fileName: "A.cs"), null);
        var b = GenerationWriter.Write(Request(root, "b", fileName: "B.cs"), null);
        var c = GenerationWriter.Write(Request(root, "c", fileName: "C.cs"), null);
        File.AppendAllText(b.AbsolutePath!, "// edit");
        File.Delete(c.AbsolutePath!);
        var manifest = ManifestStore.Load(root.Path, "Products", null).Manifest!;
        var states = DriftInspector.InspectAggregate(root.Path, manifest);
        Assert.Equal(3, states.Count);
        Assert.Equal(FileState.Verified, states.Single(s => s.RelativePath.EndsWith("A.cs")).State);
        Assert.Equal(FileState.Drifted, states.Single(s => s.RelativePath.EndsWith("B.cs")).State);
        Assert.Equal(FileState.Missing, states.Single(s => s.RelativePath.EndsWith("C.cs")).State);
        // Nothing was written by inspection.
        Assert.Equal("a", File.ReadAllText(a.AbsolutePath!).Substring(File.ReadAllText(a.AbsolutePath!).Length - 1));
    }

    [Fact]
    public void MarkerProblemOnMatchingBodyIsNotDrift()
    {
        using var root = TempSolution.Create();
        var first = GenerationWriter.Write(Request(root), null);
        // Strip the marker but keep the body.
        var text = File.ReadAllText(first.AbsolutePath!);
        GeneratedCodeMarker.TryFindMarkerBlock(text, out var block);
        File.WriteAllText(first.AbsolutePath!, text.Substring(block!.BodyStart));
        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(Request(root), diags);
        Assert.Equal(WriteOutcome.Written, result.Outcome);
        Assert.Contains(diags.Items, d => d.Code == DiagnosticCodes.MarkerProblem);
        Assert.DoesNotContain(diags.Items, d => d.Code == DiagnosticCodes.Drifted);
    }
}
