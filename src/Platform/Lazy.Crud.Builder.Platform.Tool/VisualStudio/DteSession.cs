using System.Diagnostics;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;

namespace Lazy.Crud.Builder.Platform.Tool.VisualStudio;

/// <summary>
/// An unattended Visual Studio automation session: launches the resolved devenv.exe in embedding
/// mode (no window, no prompts, no person), binds to its DTE through the Running Object Table by
/// process id (deterministic even with side-by-side installations), opens a solution and runs the
/// T4 custom tool (TextTemplatingFileGenerator) exactly as the IDE does.
/// </summary>
public sealed class DteSession : IDisposable
{
    private readonly StaWorker _sta = new();
    private readonly Action<string> _log;
    private System.Diagnostics.Process? _process;
    private DTE2? _dte;
    private bool _disposed;

    public VsInstance Instance { get; }
    public int ProcessId => _process?.Id ?? 0;
    public string ProgId { get; }

    private DteSession(VsInstance instance, Action<string> log)
    {
        Instance = instance;
        _log = log;
        ProgId = $"VisualStudio.DTE.{instance.MajorVersion}.0";
    }

    /// <summary>
    /// Shows the automated IDE instead of hiding it. Unattended still means unattended — nothing prompts and
    /// nobody clicks — this only makes the window visible so a run can be filmed or watched. Evidence only;
    /// every other path keeps the window hidden.
    /// </summary>
    public bool ShowIde { get; private set; }

    public static DteSession Launch(VsInstance instance, Action<string> log, TimeSpan? startupTimeout = null, bool showIde = false)
    {
        var session = new DteSession(instance, log) { ShowIde = showIde };
        try
        {
            session.Start(startupTimeout ?? TimeSpan.FromMinutes(3));
        }
        catch
        {
            session.Dispose();
            throw;
        }
        return session;
    }

    private void Start(TimeSpan timeout)
    {
        var psi = new ProcessStartInfo(Instance.DevEnvPath, "-Embedding")
        {
            UseShellExecute = false,
            CreateNoWindow = !ShowIde,
            WorkingDirectory = Path.GetDirectoryName(Instance.DevEnvPath) ?? Environment.CurrentDirectory,
        };
        _process = System.Diagnostics.Process.Start(psi) ?? throw new InvalidOperationException("devenv.exe could not be started");
        _log($"dte: started devenv.exe pid={_process.Id} ({Instance.DevEnvPath} -Embedding)");

        var moniker = $"!{ProgId}:{_process.Id}";
        var sw = Stopwatch.StartNew();
        Exception? last = null;
        while (sw.Elapsed < timeout)
        {
            if (_process.HasExited)
                throw new InvalidOperationException($"devenv.exe exited early with code {_process.ExitCode}");
            try
            {
                var obj = _sta.Invoke(() => RunningObjectTable.GetObject(moniker), TimeSpan.FromSeconds(30));
                if (obj != null)
                {
                    _dte = _sta.Invoke(() => (DTE2)obj, TimeSpan.FromSeconds(30));
                    break;
                }
            }
            catch (Exception ex) { last = ex; }
            System.Threading.Thread.Sleep(500);
        }
        if (_dte == null)
        {
            var names = string.Join(", ", RunningObjectTable.ListDisplayNames().Where(n => n.Contains("VisualStudio", StringComparison.OrdinalIgnoreCase)));
            throw new TimeoutException($"DTE {moniker} did not appear in the Running Object Table within {timeout.TotalSeconds:F0}s. ROT entries: [{names}]. Last error: {last?.Message}");
        }
        _log($"dte: bound to {moniker} after {sw.Elapsed.TotalSeconds:F1}s");

        _sta.Invoke(() =>
        {
            // SuppressUI and UserControl stay as they are in both modes: no dialog may ever wait for a person,
            // and the session must close when the run ends whether or not anyone is watching it.
            try { _dte!.SuppressUI = true; } catch (Exception ex) { _log("dte: SuppressUI failed -> " + ex.Message); }
            try { _dte!.UserControl = false; } catch (Exception ex) { _log("dte: UserControl failed -> " + ex.Message); }
            try { _dte!.MainWindow.Visible = ShowIde; } catch (Exception ex) { _log("dte: MainWindow.Visible failed -> " + ex.Message); }
            if (ShowIde)
            {
                try { _dte!.MainWindow.WindowState = vsWindowState.vsWindowStateMaximize; } catch (Exception ex) { _log("dte: MainWindow.WindowState failed -> " + ex.Message); }
                try { _dte!.MainWindow.Activate(); } catch (Exception ex) { _log("dte: MainWindow.Activate failed -> " + ex.Message); }
                _log("dte: the IDE window is visible for this run (evidence mode); it is still driven with no human interaction");
            }
        });
    }

    public string Version => _sta.Invoke(() => _dte!.Version);

    public void OpenSolution(string solutionPath, TimeSpan? timeout = null, int expectedProjectCount = -1)
    {
        var t = timeout ?? TimeSpan.FromMinutes(5);
        _log($"dte: opening solution {solutionPath}");
        _sta.Invoke(() => { _dte!.Solution.Open(solutionPath); }, t);
        var sw = Stopwatch.StartNew();
        int stableCount = -1; int stableTicks = 0;
        var slowPolls = 0;
        while (sw.Elapsed < t)
        {
            // A poll that does not answer in time is "not settled yet", not a failure: while Visual Studio loads
            // projects on a busy machine, one enumeration of Solution.Projects can take longer than the per-call
            // budget. Letting that TimeoutException out ended a whole scenario in a minute even though the real
            // deadline below had minutes left. The deadline is what decides; a slow poll is reported and retried.
            int count;
            try
            {
                count = _sta.Invoke(() =>
                {
                    if (!_dte!.Solution.IsOpen) return -1;
                    int n = 0;
                    foreach (Project p in _dte.Solution.Projects) { if (ProjectLoaded(p)) n++; }
                    return n;
                }, TimeSpan.FromSeconds(60));
            }
            catch (COMException) { count = -1; }
            catch (TimeoutException)
            {
                slowPolls++;
                _log($"dte: the project system did not answer within 60s while loading ({slowPolls} slow poll(s), {(t - sw.Elapsed).TotalSeconds:F0}s of the load budget left)");
                count = -1;
            }
            if (count >= 0 && (expectedProjectCount < 0 || count >= expectedProjectCount))
            {
                if (count == stableCount) stableTicks++; else { stableCount = count; stableTicks = 0; }
                if (stableTicks >= 3) { _log($"dte: solution open, {count} project(s) loaded after {sw.Elapsed.TotalSeconds:F1}s"); return; }
            }
            System.Threading.Thread.Sleep(500);
        }
        throw new TimeoutException($"Solution {solutionPath} did not finish loading within {t.TotalSeconds:F0}s");
    }

    private static bool ProjectLoaded(Project project)
    {
        try
        {
            return !string.IsNullOrEmpty(project.FullName);
        }
        catch { return false; }
    }

    /// <summary>
    /// Runs the custom tool (TextTemplatingFileGenerator) for a .tt project item, the same
    /// operation as "Run Custom Tool" in the IDE. Returns false when the item is not part of
    /// the loaded solution.
    /// </summary>
    public bool RunCustomTool(string templatePath, TimeSpan? timeout = null)
    {
        return _sta.Invoke(() =>
        {
            var item = FindProjectItemWithRetry(templatePath, TimeSpan.FromSeconds(15));
            if (item == null)
            {
                // The wrapper was re-seeded on disk after the project loaded (wrappers delete themselves
                // after running) and the project system has not picked it up yet. Re-include it the way
                // the IDE extension does (add the file to its project, set the custom tool) and give the
                // project system a moment to publish the item.
                item = ReincludeTemplate(templatePath) ?? FindProjectItemWithRetry(templatePath, TimeSpan.FromSeconds(10));
                if (item == null) return false;
            }
            EnsureCustomTool(item);
            if (item.Object is VSLangProj.VSProjectItem vsItem)
            {
                vsItem.RunCustomTool();
                return true;
            }
            // Fallback used by the IDE extension: activate the document and execute the command.
            try { item.Open(EnvDTE.Constants.vsViewKindTextView).Activate(); } catch { /* command below still targets the item */ }
            _dte!.ExecuteCommand("TextTemplating.RunCustomTool");
            return true;
        }, timeout ?? TimeSpan.FromMinutes(5));
    }

    /// <summary>
    /// SDK-style (CPS) projects discover files created on disk asynchronously; a wrapper re-seeded
    /// after the solution loaded can take a few seconds to become a project item.
    /// </summary>
    private ProjectItem? FindProjectItemWithRetry(string templatePath, TimeSpan timeout)
    {
        var sw = Stopwatch.StartNew();
        while (true)
        {
            ProjectItem? item = null;
            try { item = _dte!.Solution.FindProjectItem(templatePath); } catch (COMException) { }
            if (item != null) return item;
            if (sw.Elapsed >= timeout) return null;
            System.Threading.Thread.Sleep(500);
        }
    }

    private ProjectItem? ReincludeTemplate(string templatePath)
    {
        var project = FindContainingProject(templatePath);
        if (project == null)
        {
            _log("dte: no loaded project contains " + templatePath);
            return null;
        }
        try
        {
            var added = project.ProjectItems.AddFromFile(templatePath);
            _log("dte: re-included " + Path.GetFileName(templatePath) + " into " + project.Name);
            return added;
        }
        catch (Exception ex)
        {
            _log("dte: AddFromFile failed for " + templatePath + " -> " + ex.Message);
            // The glob may have picked it up meanwhile.
            return _dte!.Solution.FindProjectItem(templatePath);
        }
    }

    private Project? FindContainingProject(string filePath)
    {
        var dir = Path.GetFullPath(Path.GetDirectoryName(filePath)!);
        Project? best = null;
        int bestLength = -1;
        foreach (Project p in _dte!.Solution.Projects)
        {
            foreach (var candidate in Flatten(p))
            {
                string full;
                try { full = candidate.FullName; } catch { continue; }
                if (string.IsNullOrEmpty(full)) continue;
                var projDir = Path.GetFullPath(Path.GetDirectoryName(full)!);
                if (dir.StartsWith(projDir, StringComparison.OrdinalIgnoreCase) && projDir.Length > bestLength)
                {
                    best = candidate;
                    bestLength = projDir.Length;
                }
            }
        }
        return best;
    }

    /// <summary>
    /// Walks a solution folder down to the real projects it contains. Every read here is defensive: the
    /// project system reloads while templates are transformed into it, and mid-reload a solution folder
    /// reports a null ProjectItems collection or an item whose SubProject throws. That is a folder with
    /// nothing to contribute at this instant, not a failure — a run once died on the NullReferenceException
    /// this raised, after the transformation itself had succeeded.
    /// </summary>
    private static IEnumerable<Project> Flatten(Project? project)
    {
        if (project == null) yield break;
        string kind;
        try { kind = project.Kind; } catch { yield break; }
        if (kind != EnvDTE80.ProjectKinds.vsProjectKindSolutionFolder)
        {
            yield return project;
            yield break;
        }

        // Materialized before yielding: an iterator cannot yield from inside a try/catch.
        var children = new List<Project>();
        try
        {
            var items = project.ProjectItems;
            if (items != null)
            {
                foreach (ProjectItem pi in items)
                {
                    Project? sub;
                    try { sub = pi?.SubProject; } catch { continue; }
                    if (sub != null) children.Add(sub);
                }
            }
        }
        catch { yield break; }

        foreach (var child in children)
            foreach (var nested in Flatten(child)) yield return nested;
    }

    private void EnsureCustomTool(ProjectItem item)
    {
        try
        {
            var prop = item.Properties?.Item("CustomTool");
            if (prop != null && !string.Equals(Convert.ToString(prop.Value), "TextTemplatingFileGenerator", StringComparison.OrdinalIgnoreCase))
                prop.Value = "TextTemplatingFileGenerator";
        }
        catch (Exception ex)
        {
            _log("dte: CustomTool property not settable on " + item.Name + " -> " + ex.Message);
        }
    }

    /// <summary>
    /// Counts the types the code model currently reports for the project that owns
    /// <paramref name="projectDirectory"/>, walking only the project's own files. Writing generated
    /// files into a project makes Visual Studio re-parse it, and a read taken during that re-parse
    /// can report a partial set; comparing consecutive counts is how the pipeline waits it out.
    /// Returns -1 when the project cannot be inspected.
    /// </summary>
    public int CountDeclaredTypes(string projectDirectory)
    {
        return _sta.Invoke(() =>
        {
            // "Cannot be inspected" covers the whole read: locating the project walks the solution tree,
            // which the project system rebuilds under us. Any failure there means "no reading available
            // right now" and the caller polls again; it must never end the run.
            try
            {
                var project = FindProjectByDirectory(projectDirectory);
                if (project == null) return -1;
                var count = 0;
                CountInItems(project.ProjectItems, ref count);
                return count;
            }
            catch (Exception ex) when (ex is COMException || ex is NullReferenceException || ex is InvalidCastException)
            {
                return -1;
            }
        }, TimeSpan.FromMinutes(1));
    }

    private static void CountInItems(ProjectItems? items, ref int count)
    {
        if (items == null) return;
        foreach (ProjectItem item in items)
        {
            try
            {
                var model = item.FileCodeModel;
                if (model != null) CountInElements(model.CodeElements, ref count);
            }
            catch { /* an item without a code model contributes nothing */ }
            try { CountInItems(item.ProjectItems, ref count); }
            catch { /* a folder that cannot be walked contributes nothing */ }
        }
    }

    private static void CountInElements(CodeElements? elements, ref int count)
    {
        if (elements == null) return;
        foreach (CodeElement element in elements)
        {
            vsCMElement kind;
            try { kind = element.Kind; } catch { continue; }
            try
            {
                if (kind == vsCMElement.vsCMElementClass) { count++; CountInElements(((CodeClass)element).Members, ref count); }
                else if (kind == vsCMElement.vsCMElementInterface) { count++; }
                else if (kind == vsCMElement.vsCMElementEnum) { count++; }
                else if (kind == vsCMElement.vsCMElementStruct) { count++; }
                else if (kind == vsCMElement.vsCMElementNamespace) { CountInElements(((CodeNamespace)element).Members, ref count); }
            }
            catch { /* an element that cannot be read contributes nothing */ }
        }
    }

    private Project? FindProjectByDirectory(string directory)
    {
        var target = Path.GetFullPath(directory).TrimEnd(Path.DirectorySeparatorChar);
        foreach (Project p in _dte!.Solution.Projects)
        {
            foreach (var candidate in Flatten(p))
            {
                string full;
                try { full = candidate.FullName; } catch { continue; }
                if (string.IsNullOrEmpty(full)) continue;
                var dir = Path.GetFullPath(Path.GetDirectoryName(full)!).TrimEnd(Path.DirectorySeparatorChar);
                if (string.Equals(dir, target, StringComparison.OrdinalIgnoreCase)) return candidate;
            }
        }
        return null;
    }

    public string SolutionFullName
 => _sta.Invoke(() => _dte!.Solution.FullName);

    /// <summary>
    /// Reads the IDE Error List (where the T4 engine reports template compilation and runtime
    /// errors). Each entry is "level|file|line|description".
    /// </summary>
    public IReadOnlyList<string> ReadErrorList()
    {
        return _sta.Invoke(() =>
        {
            var result = new List<string>();
            try
            {
                var items = _dte!.ToolWindows.ErrorList.ErrorItems;
                for (int i = 1; i <= items.Count; i++)
                {
                    var item = items.Item(i);
                    string level = "", file = "", line = "", desc = "";
                    try { level = item.ErrorLevel.ToString(); } catch { }
                    try { file = item.FileName ?? ""; } catch { }
                    try { line = item.Line.ToString(); } catch { }
                    try { desc = item.Description ?? ""; } catch { }
                    result.Add($"{level}|{file}|{line}|{desc}");
                }
            }
            catch (Exception ex)
            {
                result.Add($"errorlist-unavailable|||{ex.GetType().Name}: {ex.Message}");
            }
            return (IReadOnlyList<string>)result;
        }, TimeSpan.FromMinutes(1));
    }

    /// <summary>
    /// Evidence mode only: opens some of the files the run produced in the visible IDE, so a recording of the
    /// run shows the generated code rather than an idle window. Every step is best effort and none of it can
    /// change what was generated — this happens after the pipeline, on files already written.
    /// </summary>
    public void RevealGeneratedFiles(IReadOnlyList<string> absolutePaths, int max = 4)
    {
        if (!ShowIde || absolutePaths.Count == 0) return;
        foreach (var path in absolutePaths.Take(max))
        {
            try
            {
                _sta.Invoke(() =>
                {
                    var window = _dte!.ItemOperations.OpenFile(path, Constants.vsViewKindPrimary);
                    window?.Activate();
                }, TimeSpan.FromSeconds(30));
                _log("dte: showing " + Path.GetFileName(path));
                System.Threading.Thread.Sleep(2500);
            }
            catch (Exception ex) { _log("dte: could not show " + Path.GetFileName(path) + " -> " + ex.Message); }
        }
        try { _sta.Invoke(() => { _dte!.ExecuteCommand("View.SolutionExplorer"); }, TimeSpan.FromSeconds(20)); } catch { /* evidence only */ }
        System.Threading.Thread.Sleep(2000);
    }

    public void CloseSolution()
    {
        try { _sta.Invoke(() => { _dte!.Solution.Close(false); }, TimeSpan.FromMinutes(2)); }
        catch (Exception ex) { _log("dte: Solution.Close failed -> " + ex.Message); }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        try
        {
            if (_dte != null)
            {
                try { _sta.Invoke(() => { _dte!.Quit(); }, TimeSpan.FromMinutes(1)); }
                catch (Exception ex) { _log("dte: Quit failed -> " + ex.Message); }
            }
        }
        finally
        {
            if (_process != null)
            {
                if (!_process.WaitForExit(30_000))
                {
                    _log("dte: devenv did not exit after Quit; killing");
                    try { _process.Kill(true); } catch { /* already gone */ }
                }
                else
                {
                    _log($"dte: devenv exited with code {_process.ExitCode}");
                }
            }
            _sta.Dispose();
        }
    }
}
