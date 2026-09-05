using Lazy.Crud.Builder.Platform.Diagnostics;
using Lazy.Crud.Builder.Platform.Writer;
using Lazy.Crud.Builder.Platform.Tool.Pipeline;
using Xunit;

namespace Lazy.Crud.Builder.Platform.Tests;

/// <summary>
/// Silent-failure paths observed with the real toolchain: the T4 host reporting compile errors at a
/// non-error level, "ErrorGeneratingOutput" written into a wrapper's output, and residue cleanup
/// deleting real host outputs.
/// </summary>
public class HardeningTests
{
    [Theory]
    [InlineData("vsBuildErrorLevelHigh|C:\\s\\Products.Domain\\DefaultEntities.tt|1|Running transformation: System.NullReferenceException", true)]
    [InlineData("vsBuildErrorLevelMedium|C:\\repo\\src\\ProjectFiles\\Api\\MiniMigrations.tt|13|Compiling transformation: CS0103: The name 'AggName' does not exist in the current context", true)]
    [InlineData("vsBuildErrorLevelMedium|C:\\s\\Products.Domain\\Program.cs|3|Compiling transformation: CS0168: The variable 'x' is declared but never used", true)]
    [InlineData("vsBuildErrorLevelMedium|C:\\s\\Products.Domain\\Program.cs|3|CS0168: The variable 'x' is declared but never used", false)]
    [InlineData("vsBuildErrorLevelLow|C:\\repo\\src\\ProjectFiles\\project.tt|1|Some informational message", false)]
    [InlineData("errorlist-unavailable|||COMException: RPC_E_CALL_REJECTED", false)]
    public void ErrorListEntriesAttributedToTemplatesFailAtAnyLevel(string entry, bool expected)
    {
        Assert.Equal(expected, DteTemplateTransformer.IsError(entry));
    }

    [Fact]
    public async Task ErrorGeneratingOutputWrittenByTheHostFailsTheStep()
    {
        var (root, options) = PipelineTests.Arrange();
        using var _ = root;
        var transformer = new FakeTransformer
        {
            Behavior = path =>
            {
                if (!Path.GetFileName(path).Equals("DefaultEntities.tt", StringComparison.OrdinalIgnoreCase)) return null;
                var residue = Path.Combine(Path.GetDirectoryName(path)!, "DefaultEntities.generated.cs");
                File.WriteAllText(residue, "ErrorGeneratingOutput");
                return new TransformOutcome { Found = true };
            },
        };
        var result = await new AggregatePipeline(new FakeProcessRunner(), m => { }).RunAsync(options, transformer);

        Assert.False(result.Succeeded);
        Assert.False(result.DomainGatePassed);
        Assert.Contains(result.Diagnostics.Items, d => d.Code == DiagnosticCodes.TransformationFailed && d.ToString().Contains("ErrorGeneratingOutput", StringComparison.Ordinal));
        Assert.DoesNotContain(result.TransformedTemplates, t => t.EndsWith("DefaultEntities.tt", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task ResidueCleanupKeepsHostOutputsWithContentAndRemovesEmptyOnes()
    {
        var (root, options) = PipelineTests.Arrange(cleanResidue: true);
        using var _ = root;
        var api = Path.Combine(root.Path, "src", "Products", "Products.Api");
        var domain = Path.Combine(root.Path, "src", "Products", "Products.Domain");
        var bat = Path.Combine(api, "Migrations.generated.bat");
        var emptyTxt = Path.Combine(api, "Migrations.generated.txt");
        var emptyDefault = Path.Combine(domain, "DefaultCommands.cs");
        File.WriteAllText(bat, "dotnet ef migrations add 2026_09_03_10_00_00 -c ProductsAggcontext -p ..\\Products.Infra.Data\r\n");
        File.WriteAllText(emptyTxt, "\uFEFF\r\n");
        File.WriteAllText(emptyDefault, "");

        var result = await new AggregatePipeline(new FakeProcessRunner(), m => { }).RunAsync(options, new FakeTransformer());

        Assert.True(result.Succeeded, string.Join("; ", result.Diagnostics.Items.Select(d => d.ToString())));
        Assert.True(File.Exists(bat), "a host output with content is a generated file and must be kept");
        Assert.False(File.Exists(emptyTxt), "an empty host output next to a wrapper is residue");
        Assert.False(File.Exists(emptyDefault), "an empty Default*.cs residue of a self-deleted wrapper is removed");
    }
}

/// <summary>
/// Reported from the extension repository against 1.1.0: the first generation into a brand-new test
/// project failed with a missing-directory error because nothing created the output folder.
/// </summary>
public class OutputDirectoryTests
{
    [Fact]
    public void MissingOutputDirectoryIsCreatedIncludingItsParents()
    {
        using var root = TempSolution.Create();
        var outputDirectory = Path.Combine(root.Path, "src", "Products", "Tests", "Lazy.Crud.Products.Tests.DTO", "Generated");
        Assert.False(Directory.Exists(outputDirectory));

        var request = new WriteRequest
        {
            SolutionRoot = root.Path,
            AggregateName = "Products",
            Layer = "Tests.DTO",
            TemplateId = "tests.application-dto",
            OutputDirectory = outputDirectory,
            OutputFileName = "ProductsAgg.ProductDtoSerializationTests.g.cs",
            Content = "public class ProductDtoSerializationTests { }\n",
            SourceTemplate = "DtoTest.tt",
        };

        var diagnostics = new DiagnosticCollector();
        var result = GenerationWriter.Write(request, diagnostics);

        Assert.Equal(WriteOutcome.Written, result.Outcome);
        Assert.True(File.Exists(result.AbsolutePath));
        Assert.DoesNotContain(diagnostics.Items, d => d.Severity == DiagnosticSeverity.Error);
    }
}

/// <summary>
/// A protected file reports where it differs, not only that two hashes disagree: that is what a person
/// needs in order to choose between keeping the edit and regenerating.
/// </summary>
public class DifferenceReportingTests
{
    [Fact]
    public void ProtectedFileNamesTheFirstDifferingLine()
    {
        using var root = TempSolution.Create();
        var request = new WriteRequest
        {
            SolutionRoot = root.Path,
            AggregateName = "Products",
            Layer = "Domain",
            TemplateId = "domain.filters",
            OutputDirectory = Path.Combine(root.Path, "src", "Products", "Products.Domain", "LazyCode"),
            OutputFileName = "ProductsAgg.Filters.cs",
            Content = "namespace X;\npublic class A { }\npublic class B { }\n",
            SourceTemplate = "DefaultCommands.tt",
        };
        Assert.Equal(WriteOutcome.Written, GenerationWriter.Write(request, new DiagnosticCollector()).Outcome);

        // The file is edited outside the generator, then regenerated with the original content.
        var path = Path.Combine(request.OutputDirectory, request.OutputFileName);
        File.WriteAllText(path, File.ReadAllText(path).Replace("public class B { }", "public class B { int edited; }"));

        var diagnostics = new DiagnosticCollector();
        var result = GenerationWriter.Write(request, diagnostics);

        Assert.Equal(WriteOutcome.Protected, result.Outcome);
        var drift = Assert.Single(diagnostics.Items, d => d.Code == DiagnosticCodes.Drifted);
        Assert.True(drift.Data.TryGetValue("firstDifference", out var where), "the diagnostic does not say where the file differs");
        Assert.Contains("int edited", where, StringComparison.Ordinal);
        Assert.Contains("line ", where, StringComparison.Ordinal);
    }
}
