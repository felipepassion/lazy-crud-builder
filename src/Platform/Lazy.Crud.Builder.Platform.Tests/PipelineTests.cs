using System.Collections.Concurrent;
using Lazy.Crud.Builder.Platform.Diagnostics;
using Lazy.Crud.Builder.Platform.Tool.Pipeline;
using Xunit;

namespace Lazy.Crud.Builder.Platform.Tests;

/// <summary>A transformer that records calls, simulates work and writes the residue output the IDE would write.</summary>
public sealed class FakeTransformer : ITemplateTransformer
{
    public ConcurrentQueue<(string template, DateTime start, DateTime end)> Calls { get; } = new();
    public List<string> Opened { get; } = new();
    public int Closed { get; private set; }
    public TimeSpan Work { get; set; } = TimeSpan.FromMilliseconds(120);
    public Func<string, TransformOutcome?>? Behavior { get; set; }
    public int Concurrency;
    public int Peak;
    public bool WriteResidue { get; set; } = true;
    public int FailFirstAttemptsFor(string fileName, int times) { _failures[fileName] = times; return times; }
    private readonly ConcurrentDictionary<string, int> _failures = new(StringComparer.OrdinalIgnoreCase);

    public void OpenSolution(string solutionPath) => Opened.Add(solutionPath);

    public TransformOutcome Transform(string templatePath)
    {
        var now = Interlocked.Increment(ref Concurrency);
        lock (this) { if (now > Peak) Peak = now; }
        var start = DateTime.UtcNow;
        try
        {
            Thread.Sleep(Work);
            var name = Path.GetFileName(templatePath);
            if (_failures.TryGetValue(name, out var left) && left > 0)
            {
                _failures[name] = left - 1;
                throw new InvalidOperationException("simulated transient failure for " + name);
            }
            var custom = Behavior?.Invoke(templatePath);
            if (custom != null) return custom;
            if (WriteResidue)
                File.WriteAllText(Path.Combine(Path.GetDirectoryName(templatePath)!, Path.GetFileNameWithoutExtension(templatePath) + ".generated.cs"), "// residue " + DateTime.UtcNow.Ticks);
            if (name.Equals("DefaultEntities.tt", StringComparison.OrdinalIgnoreCase))
            {
                var entities = Path.Combine(Path.GetDirectoryName(templatePath)!, "Aggregates", "ProductsAgg", "Entities");
                Directory.CreateDirectory(entities);
                File.WriteAllText(Path.Combine(entities, "Products.cs"), "public partial class Products {}");
            }
            return new TransformOutcome { Found = true };
        }
        finally
        {
            Interlocked.Decrement(ref Concurrency);
            Calls.Enqueue((templatePath, start, DateTime.UtcNow));
        }
    }

    public void CloseSolution() => Closed++;
    public void Dispose() { }
}

public sealed class FakeProcessRunner : IProcessRunner
{
    public List<string> Commands { get; } = new();
    public Queue<int> ExitCodes { get; } = new();
    public Task<int> RunAsync(string fileName, string arguments, string workingDirectory, Action<string> log, CancellationToken ct)
    {
        Commands.Add(fileName + " " + arguments);
        return Task.FromResult(ExitCodes.Count > 0 ? ExitCodes.Dequeue() : 0);
    }
}

public class PipelineTests
{
    internal static (TempSolution root, PipelineOptions options) Arrange(int maxParallel = 1, bool includeTests = true, string aggregate = "Products", bool cleanResidue = false)
    {
        var root = TempSolution.Create();
        var source = new TemplateSource { Kind = TemplateSourceKind.Local, Location = Path.Combine(root.Path, "ProjectFiles") };
        var agg = Path.Combine(root.Path, "src", aggregate);
        foreach (var layer in AggregateLayout.Layers)
        {
            var dir = Path.Combine(agg, aggregate + layer.ProjectSuffix);
            Directory.CreateDirectory(dir);
            File.WriteAllText(Path.Combine(dir, aggregate + layer.ProjectSuffix + ".csproj"), "<Project />");
        }
        if (includeTests)
        {
            foreach (var t in AggregateLayout.TestLayers)
            {
                var dir = Path.Combine(agg, "Tests", "Lazy.Crud." + aggregate + "." + t.Id);
                Directory.CreateDirectory(dir);
                File.WriteAllText(Path.Combine(dir, "Lazy.Crud." + aggregate + "." + t.Id + ".csproj"), "<Project />");
            }
        }
        var options = new PipelineOptions
        {
            SolutionRoot = root.Path, SolutionFile = Path.Combine(root.Path, "Sandbox.sln"), AggregateName = aggregate,
            TemplateSource = source, MaxParallelT4 = maxParallel, IncludeTests = includeTests, WaitTimeoutMs = 2000, PollIntervalMs = 20,
            RetryDelay = TimeSpan.FromMilliseconds(10), CleanResidue = cleanResidue,
        };
        return (root, options);
    }

    [Fact]
    public async Task DomainRunsFirstInTwoPhasesThenOtherLayersThenTests()
    {
        var (root, options) = Arrange(maxParallel: 3);
        using var _ = root;
        var transformer = new FakeTransformer();
        var runner = new FakeProcessRunner();
        var result = await new AggregatePipeline(runner, m => { }).RunAsync(options, transformer);

        Assert.True(result.Succeeded, string.Join("; ", result.Diagnostics.Items.Select(d => d.ToString())));
        Assert.True(result.DomainGatePassed);
        var order = transformer.Calls.OrderBy(c => c.start).Select(c => c.template).ToList();
        Assert.EndsWith("DefaultEntities.tt", order[0]);
        Assert.EndsWith("DefaultCommands.tt", order[1]);
        Assert.Contains(Path.Combine(root.Path, "src", "Products", "Products.Domain"), order[1]);
        // Every non-Domain layer starts after Domain phase 2 finished.
        var domainEnd = transformer.Calls.Where(c => c.template.Contains("Products.Domain" + Path.DirectorySeparatorChar)).Max(c => c.end);
        foreach (var c in transformer.Calls.Where(c => !c.template.Contains("Products.Domain" + Path.DirectorySeparatorChar)))
            Assert.True(c.start >= domainEnd, c.template + " started before the Domain gate passed");
        // Tests start after every runtime layer finished.
        var runtimeEnd = transformer.Calls.Where(c => !c.template.Contains(Path.DirectorySeparatorChar + "Tests" + Path.DirectorySeparatorChar)).Max(c => c.end);
        foreach (var c in transformer.Calls.Where(c => c.template.Contains(Path.DirectorySeparatorChar + "Tests" + Path.DirectorySeparatorChar)))
            Assert.True(c.start >= runtimeEnd, c.template + " started before runtime layers finished");
        // Wrapper count: Domain 2 + Application 1 + DTO 1 + Infra.Data 1 + Infra.IoC 1 + Api.Queries 1 + Api 3 + 8 tests = 18.
        Assert.Equal(18, transformer.Calls.Count);
        // Restore before and after transformation.
        Assert.Equal(2, runner.Commands.Count(c => c.Contains("restore")));
        Assert.Equal(1, transformer.Closed);
        // Wrappers were (re)seeded.
        Assert.True(File.Exists(Path.Combine(root.Path, "src", "Products", "Products.Domain", "DefaultEntities.tt")));
        Assert.Contains("ProjectFiles", File.ReadAllText(Path.Combine(root.Path, "src", "Products", "Products.Domain", "DefaultEntities.tt")));
    }

    [Fact]
    public async Task DomainGateStopsEverythingWhenPhaseOneFails()
    {
        var (root, options) = Arrange(maxParallel: 2);
        using var _ = root;
        var transformer = new FakeTransformer { Behavior = p => p.EndsWith("DefaultEntities.tt") ? new TransformOutcome { Found = true, Errors = new[] { "Compiling transformation: boom" } } : null };
        var result = await new AggregatePipeline(new FakeProcessRunner(), m => { }).RunAsync(options, transformer);
        Assert.False(result.Succeeded);
        Assert.False(result.DomainGatePassed);
        // The engine error is retried, then the gate stops the run: nothing but Domain phase 1 ran.
        Assert.All(transformer.Calls, c => Assert.EndsWith("DefaultEntities.tt", c.template));
        Assert.Equal(options.TransformAttempts, transformer.Calls.Count);
        Assert.Contains(result.Diagnostics.Items, d => d.Code == DiagnosticCodes.TransformationFailed && d.Severity == DiagnosticSeverity.Error);
        Assert.Equal(1, transformer.Closed);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    public async Task ThrottleIsHonoured(int maxParallel)
    {
        var (root, options) = Arrange(maxParallel: maxParallel);
        using var _ = root;
        var transformer = new FakeTransformer { Work = TimeSpan.FromMilliseconds(150) };
        var result = await new AggregatePipeline(new FakeProcessRunner(), m => { }).RunAsync(options, transformer);
        Assert.True(result.Succeeded);
        Assert.True(transformer.Peak <= maxParallel, $"peak {transformer.Peak} > {maxParallel}");
        Assert.True(result.PeakConcurrency <= maxParallel);
        if (maxParallel > 1) Assert.True(transformer.Peak > 1, "parallel fan-out did not happen");
    }

    [Fact]
    public async Task SameAggregateIsSerializedDifferentAggregatesRunConcurrently()
    {
        var (rootA, optionsA) = Arrange(maxParallel: 1, includeTests: false, aggregate: "Orders");
        using var _a = rootA;
        var t1 = new FakeTransformer { Work = TimeSpan.FromMilliseconds(80) };
        var t2 = new FakeTransformer { Work = TimeSpan.FromMilliseconds(80) };
        var pipeline = new AggregatePipeline(new FakeProcessRunner(), m => { });
        var run1 = Task.Run(() => pipeline.RunAsync(optionsA, t1));
        var run2 = Task.Run(() => pipeline.RunAsync(optionsA, t2));
        await Task.WhenAll(run1, run2);
        var all = t1.Calls.Concat(t2.Calls).OrderBy(c => c.start).ToList();
        var end1 = t1.Calls.Max(c => c.end); var start2 = t2.Calls.Min(c => c.start);
        var end2 = t2.Calls.Max(c => c.end); var start1 = t1.Calls.Min(c => c.start);
        Assert.True(start2 >= end1 || start1 >= end2, "runs for the same aggregate overlapped");

        var (rootB, optionsB) = Arrange(maxParallel: 1, includeTests: false, aggregate: "Invoices");
        using var _b = rootB;
        var tB = new FakeTransformer { Work = TimeSpan.FromMilliseconds(80) };
        var tA = new FakeTransformer { Work = TimeSpan.FromMilliseconds(80) };
        var runA = Task.Run(() => pipeline.RunAsync(optionsA, tA));
        var runB = Task.Run(() => pipeline.RunAsync(optionsB, tB));
        await Task.WhenAll(runA, runB);
        var overlap = tA.Calls.Any(a => tB.Calls.Any(b => a.start < b.end && b.start < a.end));
        Assert.True(overlap, "different aggregates were not allowed to run concurrently");
    }

    [Fact]
    public async Task TransientFailureIsRetriedTwiceThenReported()
    {
        var (root, options) = Arrange(includeTests: false);
        using var _ = root;
        var transformer = new FakeTransformer();
        transformer.FailFirstAttemptsFor("DefaultCommands.tt", 1);
        var result = await new AggregatePipeline(new FakeProcessRunner(), m => { }).RunAsync(options, transformer);
        Assert.True(result.Succeeded);
        var step = result.Steps.First(s => s.Phase == "domain-phase-2");
        Assert.Equal(2, step.Attempts);

        var (root2, options2) = Arrange(includeTests: false);
        using var _2 = root2;
        var t2 = new FakeTransformer();
        t2.FailFirstAttemptsFor("DefaultEntities.tt", 2);
        var result2 = await new AggregatePipeline(new FakeProcessRunner(), m => { }).RunAsync(options2, t2);
        Assert.False(result2.Succeeded);
        Assert.Contains(result2.Diagnostics.Items, d => d.Code == DiagnosticCodes.TransformationProducedNothing);
        Assert.Equal(2, result2.Steps.First(s => s.Phase == "domain-phase-1").Attempts);
    }

    [Fact]
    public async Task ACollapsedOutputIsRetriedAndTheRecoveryIsRecorded()
    {
        // The writer runs inside the T4 host and reports a refused contentless body through the
        // machine-readable diagnostics file. This is the only channel the pipeline has to see it, so the
        // fake transformer writes exactly what the real writer would -- once -- and the pipeline must
        // treat it as the transient host failure it is.
        var (root, options) = Arrange(includeTests: false);
        using var _ = root;
        var file = DiagnosticsFile.For(options.SolutionRoot, options.AggregateName);
        var transformer = new FakeTransformer();
        var collapses = 0;
        transformer.Behavior = path =>
        {
            if (Path.GetFileName(path).Equals("DefaultCommands.tt", StringComparison.OrdinalIgnoreCase)
                && Path.GetDirectoryName(path)!.EndsWith("Products.Domain", StringComparison.OrdinalIgnoreCase)
                && collapses++ == 0)
            {
                file.Report(new Diagnostic(DiagnosticCodes.OutputCollapsed, DiagnosticSeverity.Error,
                        "src/Products/Products.Domain/LazyCode/ProductsAgg.Filters.cs", "contentless body refused")
                    .With("sourceTemplate", "DefaultCommands.tt").With("layer", "Domain"));
            }
            return null;
        };

        var result = await new AggregatePipeline(new FakeProcessRunner(), m => { }).RunAsync(options, transformer);

        Assert.True(result.Succeeded);
        Assert.Equal(2, result.Steps.First(s => s.Phase == "domain-phase-2").Attempts);
        var recovery = Assert.Single(result.Diagnostics.Items, d => d.Code == DiagnosticCodes.OutputCollapseRetried);
        Assert.Equal(DiagnosticSeverity.Warning, recovery.Severity);
        Assert.Equal("src/Products/Products.Domain/LazyCode/ProductsAgg.Filters.cs", recovery.Path);
        Assert.Equal("DefaultCommands.tt", recovery.Data["sourceTemplate"]);
        Assert.Equal("Domain", recovery.Data["layer"]);
    }

    [Fact]
    public async Task ACollapseThatSurvivesEveryAttemptFailsTheStep()
    {
        var (root, options) = Arrange(includeTests: false);
        using var _ = root;
        var file = DiagnosticsFile.For(options.SolutionRoot, options.AggregateName);
        var transformer = new FakeTransformer();
        transformer.Behavior = path =>
        {
            // The same file every time, which is what a template that keeps losing its model produces.
            if (Path.GetFileName(path).Equals("DefaultEntities.tt", StringComparison.OrdinalIgnoreCase))
            {
                file.Report(new Diagnostic(DiagnosticCodes.OutputCollapsed, DiagnosticSeverity.Error,
                        "src/Products/Products.Domain/LazyCode/ProductsAgg.Filters.cs", "contentless body refused")
                    .With("sourceTemplate", "DefaultEntities.tt").With("layer", "Domain"));
            }
            return null;
        };

        var result = await new AggregatePipeline(new FakeProcessRunner(), m => { }).RunAsync(options, transformer);

        Assert.False(result.Succeeded);
        var step = result.Steps.First(s => s.Phase == "domain-phase-1");
        Assert.False(step.Succeeded);
        Assert.Contains("no content", step.Detail);
        Assert.DoesNotContain(result.Diagnostics.Items, d => d.Code == DiagnosticCodes.OutputCollapseRetried);
    }

    [Fact]
    public async Task ACollapseInAnotherLayerDoesNotRetryThisOne()
    {
        var (root, options) = Arrange(includeTests: false);
        using var _ = root;
        var file = DiagnosticsFile.For(options.SolutionRoot, options.AggregateName);
        // Several layers seed a wrapper named DefaultCommands.tt, so the source template alone is not a
        // correlation: the layer the writer records is what keeps one layer's defect out of another's loop.
        file.Report(new Diagnostic(DiagnosticCodes.OutputCollapsed, DiagnosticSeverity.Error,
                "src/Products/Products.Application.DTO/LazyCode/ProductsAgg.ListiningDTO.cs", "contentless body refused")
            .With("sourceTemplate", "DefaultCommands.tt").With("layer", "Application.DTO"));

        var result = await new AggregatePipeline(new FakeProcessRunner(), m => { }).RunAsync(options, new FakeTransformer());

        Assert.True(result.Succeeded);
        Assert.Equal(1, result.Steps.First(s => s.Phase == "domain-phase-2").Attempts);
        Assert.DoesNotContain(result.Diagnostics.Items, d => d.Code == DiagnosticCodes.OutputCollapseRetried);
    }

    [Fact]
    public async Task TransformationThatProducesNothingFails()
    {
        var (root, options) = Arrange(includeTests: false);
        using var _ = root;
        var transformer = new FakeTransformer { WriteResidue = false };
        var result = await new AggregatePipeline(new FakeProcessRunner(), m => { }).RunAsync(options, transformer);
        Assert.False(result.Succeeded);
        var d = result.Diagnostics.Items.First(x => x.Code == DiagnosticCodes.TransformationProducedNothing);
        Assert.Contains("produced no output", d.Message);
    }

    [Fact]
    public async Task OutputWaitPollsUntilTheResidueAppears()
    {
        var (root, options) = Arrange(includeTests: false);
        using var _ = root;
        var transformer = new FakeTransformer { WriteResidue = false };
        transformer.Behavior = p =>
        {
            // Write the residue late, on another thread, to exercise the wait/poll path.
            var residue = Path.Combine(Path.GetDirectoryName(p)!, Path.GetFileNameWithoutExtension(p) + ".generated.cs");
            Task.Run(async () => { await Task.Delay(400); File.WriteAllText(residue, "late"); });
            if (p.EndsWith("DefaultEntities.tt"))
            {
                var entities = Path.Combine(Path.GetDirectoryName(p)!, "Aggregates", "ProductsAgg", "Entities");
                Directory.CreateDirectory(entities);
                File.WriteAllText(Path.Combine(entities, "Products.cs"), "class Products {}");
            }
            return new TransformOutcome { Found = true };
        };
        var result = await new AggregatePipeline(new FakeProcessRunner(), m => { }).RunAsync(options, transformer);
        Assert.True(result.Succeeded, string.Join("; ", result.Diagnostics.Items.Select(d => d.ToString())));
    }

    [Fact]
    public async Task SelfDeletingWrapperCountsAsEvidenceAndIsReseededForTheNextRun()
    {
        var (root, options) = Arrange(includeTests: false);
        using var _ = root;
        // project.tt deletes DefaultEntities.tt/DefaultCommands.tt after writing; VS then writes no residue.
        var transformer = new FakeTransformer { WriteResidue = false };
        transformer.Behavior = p =>
        {
            var name = Path.GetFileName(p);
            if (name is "DefaultEntities.tt" or "DefaultCommands.tt")
            {
                if (name == "DefaultEntities.tt")
                {
                    var entities = Path.Combine(Path.GetDirectoryName(p)!, "Aggregates", "ProductsAgg", "Entities");
                    Directory.CreateDirectory(entities);
                    File.WriteAllText(Path.Combine(entities, "Products.cs"), "class Products {}");
                }
                File.Delete(p);
                return new TransformOutcome { Found = true };
            }
            File.WriteAllText(Path.Combine(Path.GetDirectoryName(p)!, Path.GetFileNameWithoutExtension(p) + ".generated.cs"), "// residue");
            return new TransformOutcome { Found = true };
        };
        var pipeline = new AggregatePipeline(new FakeProcessRunner(), m => { });
        var first = await pipeline.RunAsync(options, transformer);
        Assert.True(first.Succeeded, string.Join("; ", first.Diagnostics.Items.Select(d => d.ToString())));
        // Second run: the wrappers were deleted by the first run and must be re-seeded, not reported missing.
        var second = await pipeline.RunAsync(options, transformer);
        Assert.True(second.Succeeded, string.Join("; ", second.Diagnostics.Items.Select(d => d.ToString())));
        Assert.Equal(2, transformer.Calls.Count(c => c.template.EndsWith("DefaultEntities.tt")));
    }

    [Fact]
    public async Task RestoreIsRetriedOnceAndDoesNotAbortTheRun()
    {
        var (root, options) = Arrange(includeTests: false);
        using var _ = root;
        var runner = new FakeProcessRunner();
        runner.ExitCodes.Enqueue(1); runner.ExitCodes.Enqueue(0);
        var result = await new AggregatePipeline(runner, m => { }).RunAsync(options, new FakeTransformer());
        Assert.True(result.Succeeded);
        Assert.Equal(2, result.Steps.First(s => s.Phase == "pre-restore").Attempts);
        Assert.Equal(3, runner.Commands.Count); // 2 pre-restore attempts + 1 post-restore
    }
}
