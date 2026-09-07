using System.Collections.Concurrent;
using System.Diagnostics;
using Lazy.Crud.Builder.Platform.Diagnostics;

namespace Lazy.Crud.Builder.Platform.Tool.Pipeline;

public sealed class PipelineOptions
{
    public required string SolutionRoot { get; init; }
    public required string SolutionFile { get; init; }
    public required string AggregateName { get; init; }
    public required TemplateSource TemplateSource { get; init; }
    /// <summary>Aggregate directory (default {SolutionRoot}/src/{Aggregate}).</summary>
    public string? AggregateDirectory { get; init; }
    public int MaxParallelT4 { get; init; } = 1;
    public int WaitTimeoutMs { get; init; } = 5000;
    public int PollIntervalMs { get; init; } = 150;
    public int RestoreAttempts { get; init; } = 2;
    public int TransformAttempts { get; init; } = 2;
    public TimeSpan RetryDelay { get; init; } = TimeSpan.FromMilliseconds(300);
    public bool IncludeTests { get; init; } = true;
    public bool SkipRestore { get; init; }
    public bool CleanResidue { get; init; } = true;
}

public sealed class PipelineStep
{
    public required string Phase { get; init; }
    public required string Name { get; init; }
    public DateTime StartedUtc { get; init; }
    public DateTime FinishedUtc { get; set; }
    public bool Succeeded { get; set; }
    public int Attempts { get; set; }
    public string? Detail { get; set; }
    public double DurationMs => (FinishedUtc - StartedUtc).TotalMilliseconds;
}

public sealed class PipelineResult
{
    public bool Succeeded { get; set; }
    public bool DomainGatePassed { get; set; }
    public List<PipelineStep> Steps { get; } = new();
    public List<string> TransformedTemplates { get; } = new();
    public List<string> DiscoveredProjects { get; } = new();
    public DiagnosticCollector Diagnostics { get; } = new();
    public TimeSpan Elapsed { get; set; }
    /// <summary>Peak number of concurrently running non-Domain transformations (for regression tests).</summary>
    public int PeakConcurrency { get; set; }
    public string? FailureReason { get; set; }
}

/// <summary>
/// The deterministic, Domain-first generation pipeline for one aggregate. It preserves the accumulated
/// invariants of the IDE extension's AggregateT4Pipeline:
///  - per-aggregate serialization (one semaphore per aggregate name; different aggregates may run concurrently);
///  - package restore before transformation, with a bounded retry, and a post-restore after generation;
///  - Domain first: DefaultEntities.tt, then verification that entities exist, then DefaultCommands.tt (two-phase);
///  - gate: no other layer is transformed when the Domain phase fails;
///  - non-Domain layers fan out in parallel under the configured MaxParallelT4 throttle;
///  - bounded retry (2 attempts, fixed delay) around each transformation;
///  - wait for produced output with polling and a timeout; a transformation that produces nothing fails.
/// </summary>
public sealed class AggregatePipeline
{
    private static readonly ConcurrentDictionary<string, SemaphoreSlim> AggregateLocks = new(StringComparer.OrdinalIgnoreCase);

    private readonly IProcessRunner _processes;
    private readonly Action<string> _log;

    public AggregatePipeline(IProcessRunner processes, Action<string> log)
    {
        _processes = processes;
        _log = log;
    }

    public async Task<PipelineResult> RunAsync(PipelineOptions options, ITemplateTransformer transformer, CancellationToken ct = default)
    {
        var sem = AggregateLocks.GetOrAdd(options.AggregateName, _ => new SemaphoreSlim(1, 1));
        await Task.Yield();
        await sem.WaitAsync(ct).ConfigureAwait(false);
        try
        {
            return await RunCoreAsync(options, transformer, ct).ConfigureAwait(false);
        }
        finally
        {
            sem.Release();
        }
    }

    private async Task<PipelineResult> RunCoreAsync(PipelineOptions o, ITemplateTransformer transformer, CancellationToken ct)
    {
        var result = new PipelineResult();
        var sw = Stopwatch.StartNew();
        var aggDir = o.AggregateDirectory ?? Path.Combine(o.SolutionRoot, "src", o.AggregateName);
        if (!Directory.Exists(aggDir))
        {
            result.FailureReason = "aggregate directory not found: " + aggDir;
            result.Diagnostics.Report(new Diagnostic(DiagnosticCodes.TransformationFailed, DiagnosticSeverity.Error, aggDir, result.FailureReason));
            return Finish(result, sw);
        }

        // 1. Discover layer projects, Domain first.
        var projects = DiscoverProjects(aggDir, o.AggregateName);
        var testProjects = o.IncludeTests ? DiscoverTestProjects(aggDir, o.AggregateName) : new List<(TestLayerSpec layer, string dir)>();
        foreach (var (layer, dir) in projects) result.DiscoveredProjects.Add(layer.Id + " -> " + dir);
        foreach (var (layer, dir) in testProjects) result.DiscoveredProjects.Add(layer.Id + " -> " + dir);
        if (projects.Count == 0)
        {
            result.FailureReason = "no layer project found under " + aggDir;
            result.Diagnostics.Report(new Diagnostic(DiagnosticCodes.TransformationFailed, DiagnosticSeverity.Error, aggDir, result.FailureReason));
            return Finish(result, sw);
        }

        // 2. Ensure wrappers (they self-delete after running).
        var ensure = Step(result, "ensure", "wrappers");
        foreach (var (layer, dir) in projects) WrapperSeeder.EnsureLayer(dir, layer, o.TemplateSource, _log);
        foreach (var (layer, dir) in testProjects) WrapperSeeder.EnsureTestLayer(dir, layer, o.TemplateSource, _log);
        Done(ensure, true);

        // 3. Pre-restore with bounded retry (NuGet path properties must exist before T4 runs).
        if (!o.SkipRestore)
        {
            var restored = await RestoreWithRetryAsync(o, result, "pre-restore", ct).ConfigureAwait(false);
            if (!restored)
            {
                result.Diagnostics.Report(new Diagnostic(DiagnosticCodes.TransformationFailed, DiagnosticSeverity.Warning, o.SolutionFile,
                    "dotnet restore failed after " + o.RestoreAttempts + " attempts; $(Pkg...) template includes may not resolve."));
            }
        }

        // 4. Open the solution in the transformation engine.
        var open = Step(result, "session", "open-solution");
        try
        {
            transformer.OpenSolution(o.SolutionFile);
            Done(open, true);
        }
        catch (Exception ex)
        {
            Done(open, false, ex.Message);
            result.FailureReason = "could not open the solution in the transformation engine: " + ex.Message;
            result.Diagnostics.Report(new Diagnostic(DiagnosticCodes.TransformationFailed, DiagnosticSeverity.Error, o.SolutionFile, result.FailureReason));
            return Finish(result, sw);
        }

        try
        {
            // 5. PHASE A - Domain gate (sequential, two-phase).
            var domain = projects.FirstOrDefault(p => p.layer.Id == "Domain");
            if (domain.layer == null)
            {
                result.Diagnostics.Report(new Diagnostic(DiagnosticCodes.TransformationFailed, DiagnosticSeverity.Error, aggDir,
                    "No Domain project was found; the Domain layer is required first."));
                result.FailureReason = "no Domain project";
                return Finish(result, sw);
            }

            var domainLayer = domain.layer;
            var domainDir = domain.dir;
            Action reseedDomain = () => WrapperSeeder.EnsureLayer(domainDir, domainLayer, o.TemplateSource, _log);
            var entitiesTt = Path.Combine(domain.dir, "DefaultEntities.tt");
            if (!await TransformWithRetryAsync(o, result, "domain-phase-1", entitiesTt, transformer, ct, reseedDomain).ConfigureAwait(false))
            {
                result.FailureReason = "Domain phase 1 (DefaultEntities.tt) failed";
                return Finish(result, sw);
            }

            var verify = Step(result, "domain-phase-1.5", "verify-entities");
            var entitiesDir = Path.Combine(domain.dir, "Aggregates", o.AggregateName + "Agg", "Entities");
            var entityCount = Directory.Exists(entitiesDir) ? Directory.GetFiles(entitiesDir, "*.cs").Length : 0;
            Done(verify, entityCount > 0, entityCount + " entity file(s)");
            if (entityCount == 0)
            {
                result.Diagnostics.Report(new Diagnostic(DiagnosticCodes.ModelResolutionFailed, DiagnosticSeverity.Warning, entitiesDir,
                    "No entity files were found after DefaultEntities.tt ran; DefaultCommands.tt will generate nothing for this aggregate."));
            }

            var commandsTt = Path.Combine(domain.dir, "DefaultCommands.tt");
            if (!await TransformWithRetryAsync(o, result, "domain-phase-2", commandsTt, transformer, ct, reseedDomain).ConfigureAwait(false))
            {
                result.FailureReason = "Domain phase 2 (DefaultCommands.tt) failed";
                return Finish(result, sw);
            }
            result.DomainGatePassed = true;

            // 6. PHASE B - non-Domain layers in parallel under the throttle.
            // Every later layer reads the Domain code model. The Domain phase just wrote files into
            // that project, so wait until the host's view of it stops changing before reading it.
            var settle = Step(result, "domain-phase-2", "settle-model");
            var modelTimeout = TimeSpan.FromMilliseconds(Math.Max(o.WaitTimeoutMs, 30000));
            var domainTypes = transformer.WaitForModelStable(domainDir, modelTimeout);
            Done(settle, true, domainTypes >= 0 ? domainTypes + " type(s) in the Domain model" : null);

            var others = projects.Where(p => p.layer.Id != "Domain").OrderBy(p => AggregateLayout.LayerIndex(p.layer.Id)).ToList();
            // Every later layer reads the Domain model. Requiring the settled type count again before
            // each one waits out a re-parse that temporarily reports fewer types, which would otherwise
            // generate a file from a partial set of entities.
            var beforeLayer = new Func<string, Task>(_ =>
            {
                transformer.WaitForModelStable(domainDir, modelTimeout, Math.Max(domainTypes, 0));
                return Task.CompletedTask;
            });

            var ok = await FanOutAsync(o, result, "layer", others.Select(p =>
            {
                var layer = p.layer; var dir = p.dir;
                return (layer.Id, dir, (IReadOnlyList<WrapperSpec>)layer.Wrappers, (Action)(() => WrapperSeeder.EnsureLayer(dir, layer, o.TemplateSource, _log)));
            }).ToList(), transformer, ct, beforeLayer).ConfigureAwait(false);

            // 7. PHASE C - test layers, after every runtime layer.
            if (ok && testProjects.Count > 0)
            {
                ok = await FanOutAsync(o, result, "tests", testProjects.Select(p =>
                {
                    var layer = p.layer; var dir = p.dir;
                    return (layer.Id, dir, (IReadOnlyList<WrapperSpec>)new[] { layer.Wrapper }, (Action)(() => WrapperSeeder.EnsureTestLayer(dir, layer, o.TemplateSource, _log)));
                }).ToList(), transformer, ct).ConfigureAwait(false);
            }

            // 8. Wait for produced outputs to stabilize.
            var wait = Step(result, "outputs", "wait-stable");
            var stable = await WaitForOutputsStableAsync(aggDir, o, ct).ConfigureAwait(false);
            Done(wait, stable, stable ? "stable" : "timeout");

            if (o.CleanResidue) CleanResidue(aggDir, projects.Select(p => p.dir).Concat(testProjects.Select(p => p.dir)));

            result.Succeeded = ok && !result.Diagnostics.HasErrors;
        }
        finally
        {
            var close = Step(result, "session", "close-solution");
            try { transformer.CloseSolution(); Done(close, true); }
            catch (Exception ex) { Done(close, false, ex.Message); }
        }

        // 9. Post-restore so generated references resolve for the build.
        if (!o.SkipRestore)
        {
            await RestoreWithRetryAsync(o, result, "post-restore", ct).ConfigureAwait(false);
        }

        return Finish(result, sw);
    }

    private async Task<bool> FanOutAsync(PipelineOptions o, PipelineResult result, string phase,
        List<(string id, string dir, IReadOnlyList<WrapperSpec> wrappers, Action reseed)> items, ITemplateTransformer transformer, CancellationToken ct,
        Func<string, Task>? beforeItem = null)
    {
        if (items.Count == 0) return true;
        var max = Math.Max(1, o.MaxParallelT4);
        using var throttler = new SemaphoreSlim(max, max);
        int running = 0;
        var allOk = true;
        var tasks = items.Select(item => Task.Run(async () =>
        {
            await throttler.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                var now = Interlocked.Increment(ref running);
                lock (result) { if (now > result.PeakConcurrency) result.PeakConcurrency = now; }
                if (beforeItem != null) await beforeItem(item.id).ConfigureAwait(false);
                foreach (var w in item.wrappers.OrderBy(w => w.RelativeFile, StringComparer.OrdinalIgnoreCase))
                {
                    var tt = Path.Combine(item.dir, w.RelativeFile.Replace('/', Path.DirectorySeparatorChar));
                    var ok = await TransformWithRetryAsync(o, result, phase + ":" + item.id, tt, transformer, ct, item.reseed).ConfigureAwait(false);
                    if (!ok) allOk = false;
                }
            }
            finally
            {
                Interlocked.Decrement(ref running);
                throttler.Release();
            }
        }, ct)).ToList();
        await Task.WhenAll(tasks).ConfigureAwait(false);
        return allOk;
    }

    private async Task<bool> TransformWithRetryAsync(PipelineOptions o, PipelineResult result, string phase, string templatePath, ITemplateTransformer transformer, CancellationToken ct, Action? reseed = null)
    {
        var step = Step(result, phase, Path.GetFileName(templatePath));
        var attempts = Math.Max(1, o.TransformAttempts);
        // A transformation that loses its model emits its template's preamble and stops. The writer
        // refuses to write that over a file that has content (LC2012), so nothing is lost -- and the
        // same sandbox reproduces the full body on the next attempt, which makes it exactly the class
        // of transient host failure this loop already exists for. Collapses are correlated to this
        // wrapper by the source template and layer the writer records, so a collapse in another layer
        // running in parallel never triggers a retry here.
        var wrapper = Path.GetFileName(templatePath);
        var layerId = phase.Contains(':') ? phase.Substring(phase.LastIndexOf(':') + 1) : "Domain";
        // Only what this call causes counts: entries already in the file when it starts belong to
        // someone else, and the file is append-only within a run, so growth beyond the baseline is
        // this attempt's. Occurrences, not distinct paths -- the same file collapsing twice is two.
        var collapseBaseline = CollapsedOutputs(o, wrapper, layerId);
        var observedCollapses = new List<string>();
        for (int attempt = 1; attempt <= attempts; attempt++)
        {
            ct.ThrowIfCancellationRequested();
            step.Attempts = attempt;
            try
            {
                // Wrappers delete themselves after a successful write (project.tt self-cleanup); re-seed before a retry.
                if (!File.Exists(templatePath) && reseed != null) reseed();
                if (!File.Exists(templatePath))
                {
                    Done(step, false, "template not found");
                    result.Diagnostics.Report(new Diagnostic(DiagnosticCodes.TransformationFailed, DiagnosticSeverity.Error, templatePath, "The wrapper template does not exist."));
                    return false;
                }
                var before = SnapshotResidue(templatePath);
                var treeBefore = SnapshotTree(Path.GetDirectoryName(templatePath)!);
                var startedUtc = DateTime.UtcNow;
                var outcome = transformer.Transform(templatePath);
                if (!outcome.Found)
                {
                    throw new InvalidOperationException("the template is not a project item of the loaded solution (was the project loaded and the .tt included?)");
                }
                if (outcome.Errors.Count > 0)
                {
                    // Reported on every attempt, and retried: a transient host or code-model failure
                    // succeeds on the next attempt, a real template error still fails the step.
                    foreach (var e in outcome.Errors)
                        result.Diagnostics.Report(new Diagnostic(DiagnosticCodes.TransformationFailed, DiagnosticSeverity.Error, templatePath, "T4 engine error: " + e));
                    throw new InvalidOperationException(outcome.Errors.Count + " engine error(s): " + outcome.Errors[0]);
                }
                var produced = await WaitForEvidenceAsync(templatePath, before, treeBefore, startedUtc, o, ct).ConfigureAwait(false);
                if (!produced)
                {
                    throw new InvalidOperationException(outcome.HostException != null
                        ? "the project system raised '" + outcome.HostException + "' and no output evidence appeared within " + o.WaitTimeoutMs + "ms"
                        : "the transformation produced no output within " + o.WaitTimeoutMs + "ms");
                }
                var hostErrors = HostOutputErrors(templatePath);
                if (hostErrors.Count > 0)
                {
                    foreach (var e in hostErrors)
                        result.Diagnostics.Report(new Diagnostic(DiagnosticCodes.TransformationFailed, DiagnosticSeverity.Error, templatePath, "T4 host error: " + e));
                    Done(step, false, "the host reported ErrorGeneratingOutput");
                    return false;
                }
                var collapsedNow = CollapsedOutputs(o, wrapper, layerId);
                if (collapsedNow.Count > collapseBaseline.Count)
                {
                    var newlyCollapsed = collapsedNow.Skip(collapseBaseline.Count).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                    foreach (var c in newlyCollapsed)
                        if (!observedCollapses.Contains(c, StringComparer.OrdinalIgnoreCase)) observedCollapses.Add(c);
                    collapseBaseline = collapsedNow;
                    if (attempt < attempts)
                        throw new InvalidOperationException("the transformation produced a body with no content for " + string.Join(", ", newlyCollapsed) + " (LC2012); the file was not overwritten");
                    // Out of attempts: the refusal stands, the files on disk are the good ones, and the
                    // LC2012 the writer recorded is what fails the run.
                    Done(step, false, "the transformation kept producing a body with no content for " + string.Join(", ", newlyCollapsed));
                    return false;
                }
                if (observedCollapses.Count > 0)
                {
                    // An earlier attempt collapsed and this one did not: say so, so the run is green with
                    // the recovery on the record rather than green in silence.
                    foreach (var recovered in observedCollapses)
                    {
                        _log($"{phase}: {wrapper} produced a body with no content for {recovered} on an earlier attempt; the retry produced content");
                        result.Diagnostics.Report(new Diagnostic(DiagnosticCodes.OutputCollapseRetried, DiagnosticSeverity.Warning, recovered,
                            "The transformation produced a body with no content for this file and the write was refused (LC2012); it was retried and the retry produced content, so the file is correct and nothing was overwritten. Attempt " + attempt + " of " + attempts + ".")
                            .With("sourceTemplate", wrapper).With("layer", layerId).With("attempts", attempt.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                    }
                }
                if (outcome.HostException != null)
                {
                    // The output is on disk and the Error List is clean: the project system failed after the
                    // transformation (self-deleting wrapper), which is recorded, not hidden.
                    _log($"{phase}: {Path.GetFileName(templatePath)} produced its output; the project system raised afterwards: {outcome.HostException}");
                    lock (result) result.TransformedTemplates.Add(templatePath);
                    Done(step, true, "output verified; project system raised after the transformation: " + outcome.HostException);
                    return true;
                }
                lock (result) result.TransformedTemplates.Add(templatePath);
                Done(step, true);
                return true;
            }
            catch (Exception ex) when (attempt < attempts)
            {
                _log($"{phase}: attempt {attempt}/{attempts} failed -> {ex.Message}; retrying");
                await Task.Delay(o.RetryDelay, ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Done(step, false, ex.Message);
                result.Diagnostics.Report(new Diagnostic(DiagnosticCodes.TransformationProducedNothing, DiagnosticSeverity.Error, templatePath,
                    "Transformation failed after " + attempts + " attempt(s): " + ex.Message + ". The run does not report generated output it did not produce."));
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// The output paths this wrapper's layer has had refused as contentless so far in this run, read
    /// from the machine-readable diagnostics file the writer appends to from inside the T4 host. The
    /// writer runs in another process, so this file is the only channel it has back to the pipeline.
    /// </summary>
    private static List<string> CollapsedOutputs(PipelineOptions o, string wrapperFileName, string layerId)
    {
        try
        {
            return DiagnosticsFile.For(o.SolutionRoot, o.AggregateName).Read()
                .Where(d => d.Code == DiagnosticCodes.OutputCollapsed
                            && string.Equals(Value(d, "sourceTemplate"), wrapperFileName, StringComparison.OrdinalIgnoreCase)
                            && string.Equals(Value(d, "layer"), layerId, StringComparison.OrdinalIgnoreCase))
                .Select(d => d.Path)
                .ToList();
        }
        catch (Exception)
        {
            // An unreadable diagnostics file must not turn a healthy transformation into a retry loop.
            return new List<string>();
        }
    }

    private static string? Value(Diagnostic d, string key)
    {
        string? v;
        return d.Data.TryGetValue(key, out v) ? v : null;
    }

    // ------------------------------------------------------------------ residue / output wait ------

    /// <summary>The IDE writes the wrapper's own output ("residue") next to it; its change proves the transformation ran.</summary>
    private static Dictionary<string, DateTime> SnapshotResidue(string templatePath)
    {
        var dir = Path.GetDirectoryName(templatePath)!;
        var stem = Path.GetFileNameWithoutExtension(templatePath) + ".generated.";
        var snap = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        if (!Directory.Exists(dir)) return snap;
        foreach (var f in Directory.GetFiles(dir, stem + "*")) snap[f] = File.GetLastWriteTimeUtc(f);
        return snap;
    }

    /// <summary>
    /// Evidence that a transformation actually ran and produced output: the wrapper's residue output
    /// was (re)written, or the wrapper deleted itself (project.tt self-cleanup only happens after a
    /// successful write), or a generated file appeared or changed under the project directory.
    /// A zero exit code from the engine is never taken as evidence by itself.
    /// </summary>
    private static async Task<bool> WaitForEvidenceAsync(string templatePath, Dictionary<string, DateTime> before, (int count, DateTime max) treeBefore, DateTime startedUtc, PipelineOptions o, CancellationToken ct)
    {
        var deadline = DateTime.UtcNow.AddMilliseconds(Math.Max(o.WaitTimeoutMs, 500));
        var projectDir = Path.GetDirectoryName(templatePath)!;
        while (true)
        {
            var now = SnapshotResidue(templatePath);
            foreach (var kv in now)
            {
                DateTime old;
                if (!before.TryGetValue(kv.Key, out old) || kv.Value > old || kv.Value >= startedUtc.AddSeconds(-2)) return true;
            }
            if (!File.Exists(templatePath)) return true;
            var tree = SnapshotTree(projectDir);
            if (tree.count != treeBefore.count || tree.max > treeBefore.max) return true;
            if (DateTime.UtcNow > deadline) return false;
            await Task.Delay(Math.Max(o.PollIntervalMs, 50), ct).ConfigureAwait(false);
        }
    }

    private static (int count, DateTime max) SnapshotTree(string projectDir)
    {
        try
        {
            var files = Directory.GetFiles(projectDir, "*", SearchOption.AllDirectories)
                .Where(f => !f.Contains(@"\obj\") && !f.Contains(@"\bin\") && !f.EndsWith(".tt", StringComparison.OrdinalIgnoreCase)).ToList();
            var max = DateTime.MinValue;
            foreach (var f in files) { var t = File.GetLastWriteTimeUtc(f); if (t > max) max = t; }
            return (files.Count, max);
        }
        catch { return (0, DateTime.MinValue); }
    }

    private static async Task<bool> WaitForOutputsStableAsync(string aggDir, PipelineOptions o, CancellationToken ct)
    {
        var timeout = TimeSpan.FromMilliseconds(Math.Max(3000, o.WaitTimeoutMs));
        var start = DateTime.UtcNow;
        var (count, max) = Snapshot(aggDir);
        var lastChange = DateTime.UtcNow;
        while (DateTime.UtcNow - start < timeout)
        {
            await Task.Delay(Math.Max(o.PollIntervalMs, 50), ct).ConfigureAwait(false);
            var (c, m) = Snapshot(aggDir);
            if (c != count || m > max) { count = c; max = m; lastChange = DateTime.UtcNow; }
            else if ((DateTime.UtcNow - lastChange).TotalMilliseconds >= 500) return true;
        }
        return false;
    }

    private static (int count, DateTime max) Snapshot(string aggDir)
    {
        try
        {
            var files = Directory.GetFiles(aggDir, "*.cs", SearchOption.AllDirectories).Where(f => !f.Contains(@"\obj\") && !f.Contains(@"\bin\")).ToList();
            var max = DateTime.MinValue;
            foreach (var f in files) { var t = File.GetLastWriteTimeUtc(f); if (t > max) max = t; }
            return (files.Count, max);
        }
        catch { return (0, DateTime.MinValue); }
    }

    /// <summary>
    /// The host writes "ErrorGeneratingOutput" into the wrapper's own output when the template failed
    /// to compile or threw. That file is a failure report, never evidence of generation.
    /// </summary>
    private static List<string> HostOutputErrors(string templatePath)
    {
        var errors = new List<string>();
        foreach (var f in ResidueCandidates(templatePath))
        {
            string text;
            try { if (new FileInfo(f).Length > 256 * 1024) continue; text = File.ReadAllText(f); } catch { continue; }
            if (text.Contains("ErrorGeneratingOutput", StringComparison.Ordinal))
                errors.Add("the host wrote ErrorGeneratingOutput to " + Path.GetFileName(f) + " (the template did not run to completion; see the Error List entries in the log)");
        }
        return errors;
    }

    /// <summary>The wrapper's own host outputs: stem.generated.* and stem.cs next to it.</summary>
    private static IEnumerable<string> ResidueCandidates(string templatePath)
    {
        var dir = Path.GetDirectoryName(templatePath)!;
        if (!Directory.Exists(dir)) yield break;
        var stem = Path.GetFileNameWithoutExtension(templatePath);
        foreach (var f in Directory.GetFiles(dir, stem + ".*"))
        {
            var name = Path.GetFileName(f);
            if (name.StartsWith(stem + ".generated.", StringComparison.OrdinalIgnoreCase)
                || string.Equals(name, stem + ".cs", StringComparison.OrdinalIgnoreCase))
                yield return f;
        }
    }

    /// <summary>
    /// Removes the empty host outputs left next to wrappers (project.tt writes the real files itself
    /// and clears the generation environment, so the host output is an empty file). A host output
    /// with content, such as Migrations.generated.bat, is a real generated file and is kept.
    /// </summary>
    private static void CleanResidue(string aggDir, IEnumerable<string> projectDirs)
    {
        foreach (var dir in projectDirs)
        {
            if (!Directory.Exists(dir)) continue;
            var candidates = new List<string>();
            foreach (var tt in Directory.GetFiles(dir, "*.tt", SearchOption.AllDirectories))
                candidates.AddRange(ResidueCandidates(tt));
            // Wrappers self-delete; residue of deleted wrappers is named after them.
            candidates.AddRange(Directory.GetFiles(dir, "Default*.generated.cs", SearchOption.TopDirectoryOnly));
            candidates.AddRange(Directory.GetFiles(dir, "Default*.cs", SearchOption.TopDirectoryOnly));
            foreach (var residue in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (!IsEmptyResidue(residue)) continue;
                try { File.Delete(residue); } catch { /* residue is disposable; a locked residue is not an error */ }
            }
        }
    }

    // BOM, space, tab, CR, LF: the leading bytes a host output may carry before its real content.
    private static readonly char[] BomAndWhitespace = { (char)0xFEFF, (char)32, (char)9, (char)13, (char)10 };

    private static bool IsEmptyResidue(string path)
    {
        try
        {
            var info = new FileInfo(path);
            if (!info.Exists) return false;
            if (info.Length > 4096) return false;
            var text = File.ReadAllText(path).TrimStart(BomAndWhitespace);
            // "ErrorGeneratingOutput" is the host's failure placeholder, not generated code: it is
            // recorded as a diagnostic and removed so it cannot break a later build.
            return text.Length == 0 || text == "ErrorGeneratingOutput";
        }
        catch { return false; }
    }

    // ------------------------------------------------------------------ restore ---------------------

    private async Task<bool> RestoreWithRetryAsync(PipelineOptions o, PipelineResult result, string phase, CancellationToken ct)
    {
        var step = Step(result, phase, "dotnet restore");
        var attempts = Math.Max(1, o.RestoreAttempts);
        for (int attempt = 1; attempt <= attempts; attempt++)
        {
            step.Attempts = attempt;
            var code = await _processes.RunAsync("dotnet", "restore \"" + o.SolutionFile + "\" -v q --nologo", o.SolutionRoot, _log, ct).ConfigureAwait(false);
            if (code == 0) { Done(step, true); return true; }
            _log($"{phase}: attempt {attempt}/{attempts} failed with exit code {code}");
            if (attempt < attempts) await Task.Delay(o.RetryDelay, ct).ConfigureAwait(false);
        }
        Done(step, false, "restore failed");
        return false;
    }

    // ------------------------------------------------------------------ discovery ------------------

    public static List<(LayerSpec layer, string dir)> DiscoverProjects(string aggDir, string aggregateName)
    {
        var list = new List<(LayerSpec, string)>();
        foreach (var layer in AggregateLayout.Layers)
        {
            var dir = Path.Combine(aggDir, aggregateName + layer.ProjectSuffix);
            if (Directory.Exists(dir) && Directory.GetFiles(dir, "*.csproj").Length > 0) list.Add((layer, dir));
        }
        return list;
    }

    public static List<(TestLayerSpec layer, string dir)> DiscoverTestProjects(string aggDir, string aggregateName)
    {
        var list = new List<(TestLayerSpec, string)>();
        foreach (var layer in AggregateLayout.TestLayers)
        {
            var dir = Path.Combine(aggDir, "Tests", "Lazy.Crud." + aggregateName + "." + layer.Id);
            if (Directory.Exists(dir) && Directory.GetFiles(dir, "*.csproj").Length > 0) list.Add((layer, dir));
        }
        return list;
    }

    // ------------------------------------------------------------------ bookkeeping -----------------

    private PipelineStep Step(PipelineResult result, string phase, string name)
    {
        var s = new PipelineStep { Phase = phase, Name = name, StartedUtc = DateTime.UtcNow };
        lock (result) result.Steps.Add(s);
        _log($"[{phase}] {name}");
        return s;
    }

    private void Done(PipelineStep step, bool ok, string? detail = null)
    {
        step.FinishedUtc = DateTime.UtcNow;
        step.Succeeded = ok;
        step.Detail = detail;
        _log($"[{step.Phase}] {step.Name} -> {(ok ? "ok" : "FAILED")}{(detail != null ? " (" + detail + ")" : "")} in {step.DurationMs:F0}ms");
    }

    private static PipelineResult Finish(PipelineResult result, Stopwatch sw)
    {
        result.Elapsed = sw.Elapsed;
        return result;
    }
}
