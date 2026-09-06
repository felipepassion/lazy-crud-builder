using Lazy.Crud.Builder.Platform.Descriptor;
using Lazy.Crud.Builder.Platform.Diagnostics;
using Lazy.Crud.Builder.Platform.Manifests;
using Lazy.Crud.Builder.Platform.Markers;
using Lazy.Crud.Builder.Platform.Writer;
using Xunit;

namespace Lazy.Crud.Builder.Platform.Tests;

/// <summary>
/// The collapse guard (LC2012). A transformation that loses its model emits the template's static
/// preamble and stops, producing a file that is well formed and says nothing. Drift protection cannot
/// see it -- a verified file is deliberately overwritable -- so the writer refuses the write itself.
/// The shape came from a real battery run (artifacts/e2e/20260905-022527, F5), where three
/// Application.DTO templates emitted a body with no declaration in it.
/// </summary>
public class OutputCollapseTests
{
    private const string GoodBody = "namespace Lazy.Crud.Products.Application.DTO.Aggregates.ProductsAgg.Requests;\r\npublic partial class ProductDTO { }\r\n";

    /// <summary>What the F5 run actually emitted: the template's using block, and nothing after it.</summary>
    private const string CollapsedBody = "using Lazy.Crud.Builder.Application.DTO.Aggregates.CommonAgg.Models;\r\nusing System.ComponentModel;\r\nusing FluentValidation;\r\n\r\n";

    private static WriteRequest Request(TempSolution root, string content, string templateId = "application-dto.listinings", string fileName = "ProductsAgg.ListiningDTO.cs")
        => new WriteRequest
        {
            SolutionRoot = root.Path,
            AggregateName = "Products",
            Layer = "Application.DTO",
            TemplateId = templateId,
            OutputDirectory = Path.Combine(root.Path, "src", "Products", "Products.Application.DTO", "LazyCode"),
            OutputFileName = fileName,
            Content = content,
            SourceTemplate = "Application.DTO/ListiningsDTO.tt",
        };

    private static Diagnostic Collapse(DiagnosticCollector diags)
        => Assert.Single(diags.Items, d => d.Code == DiagnosticCodes.OutputCollapsed);

    [Fact]
    public void VerifiedFileIsNotOverwrittenByACollapsedBody()
    {
        using var root = TempSolution.Create();
        var first = GenerationWriter.Write(Request(root, GoodBody), null);
        Assert.Equal(WriteOutcome.Written, first.Outcome);
        var before = File.ReadAllText(first.AbsolutePath!);

        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(Request(root, CollapsedBody), diags);

        // The state is verified: without this guard the write would have gone through.
        Assert.Equal(WriteOutcome.RefusedCollapsed, result.Outcome);
        Assert.False(result.Succeeded);
        Assert.Equal(before, File.ReadAllText(first.AbsolutePath!));
        var d = Collapse(diags);
        Assert.Equal(DiagnosticSeverity.Error, d.Severity);
        Assert.Equal("src/Products/Products.Application.DTO/LazyCode/ProductsAgg.ListiningDTO.cs", d.Path);
        Assert.Equal("application-dto.listinings", d.Data["templateId"]);
        Assert.Equal("Application.DTO", d.Data["layer"]);
        Assert.Equal("Products", d.Data["aggregate"]);
        Assert.Equal("no-significant-content", d.Data["reason"]);
        Assert.Equal(BodySignificance.RuleId, d.Data["rule"]);
        Assert.Equal("2", d.Data["currentSignificantLines"]);
        Assert.Equal("0", d.Data["regeneratedSignificantLines"]);
        Assert.NotEqual(d.Data["currentHash"], d.Data["regeneratedHash"]);
        Assert.Equal("none", d.Data["resolutions"]);

        // Nothing was recorded as generated over the good file.
        var manifest = ManifestStore.Load(root.Path, "Products", new DiagnosticCollector()).Manifest!;
        Assert.Equal(Hashing.ContentHasher.Compute(before).Value, manifest.Find(d.Path)!.ContentHash);
    }

    [Fact]
    public void UnverifiedFileIsNotOverwrittenByACollapsedBody()
    {
        using var root = TempSolution.Create();
        var written = GenerationWriter.Write(Request(root, GoodBody), null);
        // No baseline: the manifest is removed, which is the state a v1 migration leaves behind.
        File.Delete(Path.Combine(root.Path, ".lazycrud", "manifests", "Products.manifest.json"));

        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(Request(root, CollapsedBody), diags);

        Assert.Equal(WriteOutcome.RefusedCollapsed, result.Outcome);
        Assert.Contains("public partial class ProductDTO", File.ReadAllText(written.AbsolutePath!));
        // Collapse is reported as itself, not as an ownership conflict.
        Assert.DoesNotContain(diags.Items, d => d.Code == DiagnosticCodes.Unverified);
        Collapse(diags);
    }

    [Fact]
    public void ForceDoesNotOverrideTheCollapseRefusal()
    {
        using var root = TempSolution.Create();
        var written = GenerationWriter.Write(Request(root, GoodBody), null);
        var before = File.ReadAllText(written.AbsolutePath!);
        new RunContext { Resolution = ResolutionMode.Force }.Save(root.Path, "Products");

        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(Request(root, CollapsedBody), diags);

        Assert.Equal(WriteOutcome.RefusedCollapsed, result.Outcome);
        Assert.Equal(before, File.ReadAllText(written.AbsolutePath!));
        Assert.DoesNotContain(diags.Items, d => d.Code == DiagnosticCodes.ForcedOverwrite);
    }

    [Fact]
    public void AnEmptyBodyIsRefusedAndReportedAsEmpty()
    {
        using var root = TempSolution.Create();
        var written = GenerationWriter.Write(Request(root, GoodBody), null);
        var diags = new DiagnosticCollector();

        var result = GenerationWriter.Write(Request(root, string.Empty), diags);

        Assert.Equal(WriteOutcome.RefusedCollapsed, result.Outcome);
        Assert.Equal("empty", Collapse(diags).Data["reason"]);
        Assert.Contains("public partial class ProductDTO", File.ReadAllText(written.AbsolutePath!));
    }

    [Fact]
    public void AWhitespaceOnlyBodyIsRefusedAndTheRealFileSurvives()
    {
        // The observable symptom of the defect: with the guard removed, the file that reaches disk is
        // its marker and nothing else. The guard is what keeps it from replacing the real one.
        using var root = TempSolution.Create();
        GenerationWriter.Write(Request(root, GoodBody), null);
        var diags = new DiagnosticCollector();

        var result = GenerationWriter.Write(Request(root, "\r\n   \r\n"), diags);

        Assert.Equal(WriteOutcome.RefusedCollapsed, result.Outcome);
        Assert.Equal("whitespace-only", Collapse(diags).Data["reason"]);
        var text = File.ReadAllText(result.AbsolutePath!);
        Assert.True(GeneratedCodeMarker.TryParse(text, out _));
        Assert.Contains("public partial class ProductDTO", text);
    }

    [Fact]
    public void ATemplateThatDeclaresAContentlessOutputIsNotAFalsePositive()
    {
        // A global-usings file is entirely import directives; its correct output has no significant
        // line, and it must keep regenerating.
        using var root = TempSolution.Create();
        var entry = TemplateRegistry.Find("application-dto.global-usings");
        Assert.True(entry!.EmptyOutputAllowed);

        var first = "global using System.ComponentModel;\r\nglobal using FluentValidation;\r\n";
        var second = "global using System.ComponentModel;\r\n";
        GenerationWriter.Write(Request(root, first, "application-dto.global-usings", "ProductsAgg.cs"), null);
        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(Request(root, second, "application-dto.global-usings", "ProductsAgg.cs"), diags);

        Assert.Equal(WriteOutcome.Written, result.Outcome);
        Assert.DoesNotContain(diags.Items, d => d.Code == DiagnosticCodes.OutputCollapsed);
        Assert.EndsWith(second, File.ReadAllText(result.AbsolutePath!));
    }

    [Fact]
    public void ANewFileIsNeverACollapseBecauseNothingWouldBeLost()
    {
        using var root = TempSolution.Create();
        var diags = new DiagnosticCollector();
        var result = GenerationWriter.Write(Request(root, CollapsedBody), diags);

        Assert.Equal(WriteOutcome.Written, result.Outcome);
        Assert.DoesNotContain(diags.Items, d => d.Code == DiagnosticCodes.OutputCollapsed);
    }

    [Fact]
    public void AnOrdinaryRegenerationIsUnchangedByTheGuard()
    {
        using var root = TempSolution.Create();
        GenerationWriter.Write(Request(root, GoodBody), null);
        var diags = new DiagnosticCollector();

        var next = GoodBody + "public partial class ProductCategoryDTO { }\r\n";
        var result = GenerationWriter.Write(Request(root, next), diags);

        Assert.Equal(WriteOutcome.Written, result.Outcome);
        Assert.Equal(FileState.Verified, result.State);
        Assert.DoesNotContain(diags.Items, d => d.Severity == DiagnosticSeverity.Error);
        Assert.EndsWith(next, File.ReadAllText(result.AbsolutePath!));
    }

    [Fact]
    public void AShrinkingBodyThatStillDeclaresSomethingIsWritten()
    {
        // The guard must not stand in for a threshold: losing most of a file is a legitimate change
        // (an entity was removed); losing all of its content is not.
        using var root = TempSolution.Create();
        var many = GoodBody + "public partial class A { }\r\npublic partial class B { }\r\npublic partial class C { }\r\n";
        GenerationWriter.Write(Request(root, many), null);
        var diags = new DiagnosticCollector();

        var result = GenerationWriter.Write(Request(root, "namespace N;\r\n"), diags);

        Assert.Equal(WriteOutcome.Written, result.Outcome);
        Assert.DoesNotContain(diags.Items, d => d.Code == DiagnosticCodes.OutputCollapsed);
    }

    [Fact]
    public void TheHostWrittenPathEmitsTheExistingFileWhenItRefuses()
    {
        using var root = TempSolution.Create();
        var written = GenerationWriter.Write(Request(root, GoodBody), null);
        var before = File.ReadAllText(written.AbsolutePath!);
        var diags = new DiagnosticCollector();

        var result = GenerationWriter.PrepareHostOutput(Request(root, CollapsedBody), diags);

        Assert.Equal(WriteOutcome.RefusedCollapsed, result.Outcome);
        Assert.Equal(before, result.EmittedText);
        Collapse(diags);
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData("   \r\n\t\r\n", 0)]
    [InlineData("// only a comment\r\n", 0)]
    [InlineData("using System;\r\nglobal using X;\r\nextern alias Y;\r\n#nullable enable\r\n", 0)]
    [InlineData("using System;\r\npublic class C { }\r\n", 1)]
    [InlineData("namespace N { }\r\n", 1)]
    public void SignificantLinesFollowTheFrozenRule(string body, int expected)
        => Assert.Equal(expected, BodySignificance.CountSignificantLines(body, CommentSyntax.CSharpLine));

    [Fact]
    public void TheCssUniversalSelectorIsContentNotAComment()
    {
        // The block-comment interior prefix is "*", which is also a CSS selector: an ambiguous
        // one-character prefix never classifies a line as a comment.
        Assert.Equal(1, BodySignificance.CountSignificantLines("* { margin: 0; }\r\n", CommentSyntax.CBlock));
        Assert.Equal(0, BodySignificance.CountSignificantLines("/* nothing here */\r\n", CommentSyntax.CBlock));
    }

    [Fact]
    public void EveryContentlessTemplateOutputInTheReferenceCorpusIsDeclared()
    {
        // The declaration is not a guess: it is the set of templates whose golden output has no
        // significant line. A new template that legitimately emits only directives fails here until
        // it is declared, which is the point.
        var goldenRoot = FindGolden();
        var undeclared = new List<string>();
        foreach (var file in Directory.GetFiles(goldenRoot, "*.*", SearchOption.AllDirectories))
        {
            if (file.EndsWith(".md", StringComparison.OrdinalIgnoreCase)) continue;
            var text = File.ReadAllText(file);
            if (!GeneratedCodeMarker.TryParse(text, out var marker) || marker is null) continue;
            var templateId = marker.Get("TemplateId");
            var syntax = CommentSyntax.ForFileName(file);
            if (!BodySignificance.IsContentless(BodySignificance.BodyOf(text), syntax)) continue;
            var entry = TemplateRegistry.Find(templateId);
            if (entry is null || !entry.EmptyOutputAllowed) undeclared.Add(templateId + " (" + Path.GetFileName(file) + ")");
        }
        Assert.Empty(undeclared);
    }

    private static string FindGolden()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && !Directory.Exists(Path.Combine(dir.FullName, "tests", "golden"))) dir = dir.Parent;
        Assert.NotNull(dir);
        return Path.Combine(dir!.FullName, "tests", "golden");
    }
}
