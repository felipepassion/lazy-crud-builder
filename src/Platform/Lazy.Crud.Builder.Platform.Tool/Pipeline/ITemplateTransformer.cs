using Lazy.Crud.Builder.Platform.Tool.VisualStudio;

namespace Lazy.Crud.Builder.Platform.Tool.Pipeline;

public sealed class TransformOutcome
{
    /// <summary>False when the template is not a project item of the loaded solution.</summary>
    public bool Found { get; init; }
    /// <summary>Errors reported by the T4 engine (IDE Error List) for this transformation.</summary>
    public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();
    /// <summary>
    /// An exception raised by the project system while running the custom tool. Visual Studio's
    /// SDK-style project system throws after the transformation completed when the wrapper deleted
    /// itself during the run (observed: "'content' must contain at least one element", "The given
    /// key was not present in the dictionary"). The pipeline decides from output evidence.
    /// </summary>
    public string? HostException { get; init; }
}

/// <summary>The transformation engine abstraction: the real one drives Visual Studio; tests use a fake.</summary>
public interface ITemplateTransformer : IDisposable
{
    void OpenSolution(string solutionPath);
    TransformOutcome Transform(string templatePath);
    void CloseSolution();

    /// <summary>
    /// Blocks until the host's view of <paramref name="projectDirectory"/> has settled, so a layer that
    /// reads the model of a project just written to does not observe a partial set of types. When
    /// <paramref name="minimumTypes"/> is positive the wait also requires the model to report at least
    /// that many types again, which is how a re-parse that temporarily drops types is waited out.
    /// Returns the settled count, or -1 when the host has no live model. Engines without one return -1.
    /// </summary>
    int WaitForModelStable(string projectDirectory, TimeSpan timeout, int minimumTypes = 0) => -1;
}

/// <summary>Runs a process (dotnet restore) and returns its exit code.</summary>
public interface IProcessRunner
{
    Task<int> RunAsync(string fileName, string arguments, string workingDirectory, Action<string> log, CancellationToken ct);
}

public sealed class ProcessRunner : IProcessRunner
{
    public async Task<int> RunAsync(string fileName, string arguments, string workingDirectory, Action<string> log, CancellationToken ct)
    {
        var psi = new System.Diagnostics.ProcessStartInfo(fileName, arguments)
        {
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        using var p = System.Diagnostics.Process.Start(psi) ?? throw new InvalidOperationException("could not start " + fileName);
        var stdout = p.StandardOutput.ReadToEndAsync(ct);
        var stderr = p.StandardError.ReadToEndAsync(ct);
        await p.WaitForExitAsync(ct).ConfigureAwait(false);
        foreach (var line in (await stdout).Split('\n')) if (line.Trim().Length > 0) log("  " + line.TrimEnd());
        foreach (var line in (await stderr).Split('\n')) if (line.Trim().Length > 0) log("  " + line.TrimEnd());
        return p.ExitCode;
    }
}

/// <summary>The real transformer: an unattended Visual Studio session (RunCustomTool = TextTemplatingFileGenerator).</summary>
public sealed class DteTemplateTransformer : ITemplateTransformer
{
    private readonly DteSession _session;
    private readonly Action<string> _log;
    private readonly object _gate = new();
    private List<string> _previousErrorList = new();

    public DteTemplateTransformer(VsInstance instance, Action<string> log)
    {
        _log = log;
        _session = DteSession.Launch(instance, log);
        _log("dte: automation session ready (Visual Studio " + _session.Version + ", pid " + _session.ProcessId + ")");
    }

    public void OpenSolution(string solutionPath)
    {
        _session.OpenSolution(solutionPath);
        _previousErrorList = _session.ReadErrorList().ToList();
    }

    /// <summary>
    /// Runs the custom tool and returns the T4 engine errors that appeared in the IDE Error List
    /// during this transformation (the list accumulates across transformations, so only new
    /// entries are attributed to this template). Transformation entries fail the step at any level.
    /// </summary>
    public TransformOutcome Transform(string templatePath)
    {
        lock (_gate)
        {
            bool found;
            string? hostException = null;
            try
            {
                found = _session.RunCustomTool(templatePath);
            }
            catch (Exception ex)
            {
                // The custom tool may have run to completion before the project system failed; report
                // the failure and let the pipeline check the output evidence and the Error List.
                found = true;
                hostException = ex.GetType().Name + ": " + Flatten(ex);
                _log("  dte: RunCustomTool raised " + hostException);
            }
            if (!found) return new TransformOutcome { Found = false };
            var current = _session.ReadErrorList().ToList();
            var added = new List<string>(current);
            foreach (var old in _previousErrorList) added.Remove(old);
            _previousErrorList = current;
            var errors = added.Where(IsError).ToList();
            foreach (var e in added) _log("  errorlist: " + e);
            return new TransformOutcome { Found = true, Errors = errors, HostException = hostException };
        }
    }

    private static string Flatten(Exception ex)
    {
        if (ex is AggregateException agg) return string.Join("; ", agg.Flatten().InnerExceptions.Select(Flatten));
        var message = ex.Message;
        if (ex.InnerException != null && !message.Contains(ex.InnerException.Message, StringComparison.Ordinal)) message += " -> " + ex.InnerException.Message;
        return message;
    }

    internal static bool IsError(string entry)
    {
        // Entries are "level|file|line|description". The T4 engine reports template compilation and
        // runtime failures at vsBuildErrorLevelHigh, but Visual Studio 18 was observed reporting
        // "Compiling transformation: CS0103 ..." at vsBuildErrorLevelMedium; any transformation entry
        // attributed to a template file fails the step regardless of its level.
        if (entry.StartsWith("errorlist-unavailable", StringComparison.OrdinalIgnoreCase)) return false;
        if (entry.StartsWith("vsBuildErrorLevelLow", StringComparison.OrdinalIgnoreCase)) return false;
        if (entry.StartsWith("vsBuildErrorLevelHigh", StringComparison.OrdinalIgnoreCase)) return true;
        var parts = entry.Split('|');
        var file = parts.Length > 1 ? parts[1] : "";
        var description = parts.Length > 3 ? parts[3] : "";
        return file.EndsWith(".tt", StringComparison.OrdinalIgnoreCase)
               || file.EndsWith(".ttinclude", StringComparison.OrdinalIgnoreCase)
               || description.StartsWith("Compiling transformation", StringComparison.OrdinalIgnoreCase)
               || description.StartsWith("Running transformation", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Polls the code model until the number of declared types stops changing for three consecutive
    /// reads. Visual Studio re-parses a project after files are written into it, and a template that
    /// reads the model mid-reparse generates from an incomplete set of entities.
    /// </summary>
    public int WaitForModelStable(string projectDirectory, TimeSpan timeout, int minimumTypes = 0)
    {
        // A re-parse temporarily drops types and can report a couple of different counts on the way
        // back up, so the wait ends when three consecutive reads all reach the highest count seen
        // (and the caller's floor), rather than when three reads happen to be identical.
        var deadline = DateTime.UtcNow + timeout;
        var highWater = minimumTypes;
        var atPeak = 0;
        var last = -1;
        while (DateTime.UtcNow < deadline)
        {
            int count;
            lock (_gate) { count = _session.CountDeclaredTypes(projectDirectory); }
            if (count < 0) { _log("  model: " + projectDirectory + " is not a loaded project; not waiting"); return -1; }
            last = count;
            if (count > highWater) { highWater = count; atPeak = 1; }
            else if (count == highWater)
            {
                atPeak++;
                if (atPeak >= 3)
                {
                    _log("  model: " + Path.GetFileName(projectDirectory) + " settled at " + count + " type(s)");
                    return count;
                }
            }
            else { atPeak = 0; }
            System.Threading.Thread.Sleep(300);
        }
        _log("  model: " + Path.GetFileName(projectDirectory) + " did not settle within " + timeout.TotalSeconds.ToString("F0") + "s; continuing with " + last + " type(s), expected at least " + minimumTypes);
        return last;
    }

    public void CloseSolution() => _session.CloseSolution();

    public void Dispose() => _session.Dispose();
}
