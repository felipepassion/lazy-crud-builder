using Lazy.Crud.Builder.Platform.Diagnostics;
using Lazy.Crud.Builder.Platform.Tool.Pipeline;
using Xunit;

namespace Lazy.Crud.Builder.Platform.Tests;

/// <summary>
/// Visual Studio's SDK-style project system throws from RunCustomTool after a wrapper deleted itself
/// during the transformation (observed with VS 18: "'content' must contain at least one element",
/// "The given key was not present in the dictionary"). Output evidence decides, never the exception alone.
/// </summary>
public class TransformerToleranceTests
{
    [Fact]
    public async Task ProjectSystemExceptionAfterOutputIsRecordedNotFailed()
    {
        var (root, options) = PipelineTests.Arrange();
        using var _ = root;
        var transformer = new FakeTransformer
        {
            Behavior = path =>
            {
                if (!Path.GetFileName(path).Equals("DefaultCommands.tt", StringComparison.OrdinalIgnoreCase)) return null;
                // The template ran: output written, wrapper self-deleted, then the project system failed.
                File.WriteAllText(Path.Combine(Path.GetDirectoryName(path)!, "DefaultCommands.generated.cs"), "// residue " + DateTime.UtcNow.Ticks);
                File.Delete(path);
                return new TransformOutcome { Found = true, HostException = "COMException: The given key was not present in the dictionary." };
            },
        };
        var result = await new AggregatePipeline(new FakeProcessRunner(), m => { }).RunAsync(options, transformer);

        Assert.True(result.Succeeded, string.Join("; ", result.Diagnostics.Items.Select(d => d.ToString())));
        var steps = result.Steps.Where(s => s.Name == "DefaultCommands.tt").ToList();
        Assert.NotEmpty(steps);
        Assert.All(steps, s => Assert.True(s.Succeeded));
        Assert.All(steps, s => Assert.Contains("project system raised", s.Detail ?? string.Empty, StringComparison.Ordinal));
        Assert.DoesNotContain(result.Diagnostics.Items, d => d.Severity == DiagnosticSeverity.Error);
    }

    [Fact]
    public async Task ProjectSystemExceptionWithoutOutputFailsWithTheExceptionText()
    {
        var (root, options) = PipelineTests.Arrange();
        using var _ = root;
        var transformer = new FakeTransformer
        {
            WriteResidue = false,
            Behavior = path => Path.GetFileName(path).Equals("DefaultEntities.tt", StringComparison.OrdinalIgnoreCase)
                ? new TransformOutcome { Found = true, HostException = "COMException: 'content' must contain at least one element." }
                : null,
        };
        var result = await new AggregatePipeline(new FakeProcessRunner(), m => { }).RunAsync(options, transformer);

        Assert.False(result.Succeeded);
        Assert.Contains(result.Diagnostics.Items, d => d.Code == DiagnosticCodes.TransformationProducedNothing && d.ToString().Contains("'content' must contain at least one element", StringComparison.Ordinal));
    }
}
