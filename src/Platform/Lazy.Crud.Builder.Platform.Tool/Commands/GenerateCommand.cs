using System.Diagnostics;
using Lazy.Crud.Builder.Platform.Descriptor;
using Lazy.Crud.Builder.Platform.Diagnostics;
using Lazy.Crud.Builder.Platform.Manifests;
using Lazy.Crud.Builder.Platform.Settings;
using Lazy.Crud.Builder.Platform.Tool.Pipeline;
using Lazy.Crud.Builder.Platform.Tool.VisualStudio;
using Lazy.Crud.Builder.Platform.Writer;

namespace Lazy.Crud.Builder.Platform.Tool.Commands;

/// <summary>
/// Unattended generation of one aggregate: prerequisite check, whole-aggregate drift pre-check,
/// pipeline execution through the resolved Visual Studio toolchain, and a machine-readable report.
/// </summary>
public static class GenerateCommand
{
    public static int Run(CommandLine o)
    {
        var log = new Action<string>(m => Console.Error.WriteLine(m));
        var sw = Stopwatch.StartNew();
        var root = ManifestCommand.SolutionRootOf(o.Require("solution"));
        var slnFile = Directory.GetFiles(root, "*.sln").Concat(Directory.GetFiles(root, "*.slnx")).OrderBy(p => p, StringComparer.Ordinal).FirstOrDefault();
        var aggregate = o.Require("aggregate");
        var reportPath = o.Get("report");
        var resolution = ParseResolution(o.Get("resolution") ?? "protect");
        var diagnostics = new DiagnosticCollector();
        var report = new RunReport { Command = "generate", SolutionRoot = root, Aggregate = aggregate, Resolution = resolution.ToString(), StartedUtc = DateTime.UtcNow };

        int Finish(int code)
        {
            report.ExitCode = code;
            report.ExitCodeName = ExitCodes.Name(code);
            report.Succeeded = code == ExitCodes.Success;
            report.DurationMs = sw.Elapsed.TotalMilliseconds;
            report.Diagnostics = diagnostics.Items.Select(RunReport.DiagnosticRecord.From).ToList();
            var json = Json.Serialize(report);
            Console.WriteLine(json);
            if (reportPath != null) { Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(reportPath))!); File.WriteAllText(reportPath, json); }
            log($"generate: {ExitCodes.Name(code)} ({code}) in {sw.Elapsed.TotalSeconds:F1}s");
            return code;
        }

        if (slnFile == null)
        {
            diagnostics.Report(new Diagnostic(DiagnosticCodes.TemplateResolutionFailed, DiagnosticSeverity.Error, root, "No .sln/.slnx found under the solution root."));
            return Finish(ExitCodes.InvalidFixtureOrSandbox);
        }
        report.SolutionFile = slnFile;

        // 1. Settings and template source (reported on every run).
        var settings = LazySettings.ForSolution(root);
        report.SettingsPath = settings.Path;
        var templateSource = ResolveTemplateSource(o, settings, root, diagnostics, log);
        if (templateSource == null) return Finish(ExitCodes.AssertionFailure);
        report.TemplateSource = new RunReport.TemplateSourceRecord { Kind = templateSource.Kind.ToString(), Location = templateSource.Location, Version = templateSource.Version, RequestedPackageId = templateSource.RequestedPackageId };
        diagnostics.Report(new Diagnostic(DiagnosticCodes.TemplateSourceResolved, DiagnosticSeverity.Information, templateSource.Location, "Template source: " + templateSource.Describe()));
        if (templateSource.Kind == TemplateSourceKind.Local)
            diagnostics.Report(new Diagnostic(DiagnosticCodes.LocalTemplateMode, DiagnosticSeverity.Information, templateSource.Location, "Local template development mode is in use (explicitly selected); templates resolve from the source tree, not from packages."));

        // 2. Prerequisite: the Visual Studio T4 toolchain, before anything is transformed or written.
        var toolchain = VsInstanceLocator.Resolve(o.Get("vs-instance"), m => log("  toolchain: " + m));
        report.Toolchain = toolchain.ToReport();
        if (!toolchain.Succeeded)
        {
            var d = new Diagnostic(DiagnosticCodes.PrerequisiteUnavailable, DiagnosticSeverity.Error, string.Empty,
                "Visual Studio with the T4 text-templating tooling is required for generation in every mode (headless included) and could not be resolved. " +
                toolchain.FailureReason + " Discovery: " + string.Join(" | ", toolchain.DiscoveryLog) + ". Remedy: " + toolchain.Remedy)
                .With("requirement", "Visual Studio 2022/2026 with " + VsToolchainResolution.RequiredComponentId)
                .With("searched", string.Join(" | ", toolchain.DiscoveryLog))
                .With("found", string.Join(" | ", toolchain.Candidates.Select(c => c.Describe())))
                .With("component", VsToolchainResolution.RequiredComponentId);
            diagnostics.Report(d);
            return Finish(ExitCodes.PrerequisiteUnavailable);
        }
        // A process with MSIX package identity sees AppData through file-system virtualization, which
        // devenv.exe does not share; say so before generating into such a path.
        var virtualized = PackageIdentity.DescribeVirtualizedPath(root);
        if (virtualized != null) log("  warning: " + virtualized);
        var vs = toolchain.Selected!;
        diagnostics.Report(new Diagnostic(toolchain.OverrideApplied ? DiagnosticCodes.ToolchainOverrideApplied : DiagnosticCodes.ToolchainResolved, DiagnosticSeverity.Information, vs.InstallationPath,
            (toolchain.OverrideApplied ? "Toolchain override applied: " : "Toolchain resolved: ") + vs.Describe() + "; T4 tooling " + vs.TextTransformPath));

        // 2b. Platform descriptor from the restored packages: highest platformVersion wins, conflicts are reported,
        //     and the contract version is negotiated before anything is written.
        var candidates = DescriptorCandidates(root, log).ToList();
        if (candidates.Count > 0)
        {
            var descriptor = DescriptorResolver.Resolve(candidates, diagnostics);
            if (descriptor != null)
            {
                report.Descriptor = new RunReport.DescriptorRecord { Source = descriptor.Source, PlatformVersion = descriptor.PlatformVersion, ContractVersion = descriptor.ContractVersion, Candidates = candidates.Select(c => c.Source).ToList() };
                if (!DescriptorResolver.IsContractSupported(descriptor, PlatformInfo.MinimumUpgradableContractVersion, PlatformInfo.ContractVersion, diagnostics))
                    return Finish(ExitCodes.ContractMismatch);
            }
        }
        else if (templateSource.Kind == TemplateSourceKind.NuGet)
        {
            diagnostics.Report(new Diagnostic(DiagnosticCodes.DescriptorMissing, DiagnosticSeverity.Warning, root,
                "No lazycrud.platform.json was found in the restored Lazy.Crud packages; packages older than 1.1.0 embed no descriptor (legacy compatibility mode)."));
        }

        // 3. Whole-aggregate drift pre-check before the first write.
        var manifestRoot = SolutionRoot.ResolveManifestRoot(root, aggregate, diagnostics);
        var load = ManifestStore.Load(manifestRoot, aggregate, diagnostics);
        if (load.Failed) return Finish(ExitCodes.AssertionFailure);
        var states = DriftInspector.InspectAggregate(root, load.Manifest);
        report.PreCheck = states.Select(s => new RunReport.FileStateRecord { RelativePath = s.RelativePath, State = s.State.ToString(), Ownership = s.Entry?.Ownership, UnknownOwnership = s.UnknownOwnership }).ToList();
        var drifted = states.Where(s => s.State == FileState.Drifted).ToList();
        foreach (var s in states.Where(s => s.State == FileState.Unverified))
            diagnostics.Report(new Diagnostic(DiagnosticCodes.LegacyMarkerDetected, DiagnosticSeverity.Information, s.RelativePath, "No trustworthy baseline is recorded for this GeneratorOwned file; it is resolved by content comparison when regenerated."));
        foreach (var s in states.Where(s => s.UnknownOwnership))
            diagnostics.Report(new Diagnostic(DiagnosticCodes.UnknownOwnership, DiagnosticSeverity.Warning, s.RelativePath, "The manifest records unknown ownership for this file; it is treated as user-owned until a registered template re-records it."));
        if (drifted.Count > 0 && resolution == ResolutionMode.Protect)
        {
            foreach (var s in drifted)
                diagnostics.Report(new Diagnostic(DiagnosticCodes.Drifted, DiagnosticSeverity.Error, s.RelativePath,
                    "The file was modified outside the generator (recorded hash " + s.Entry!.ContentHash!.Substring(0, 12) + ", current hash " + s.CurrentHash!.Value.Substring(0, 12) + "); nothing was written. Resolve with --resolution force or --resolution skip-modified.")
                    .With("templateId", s.Entry.TemplateId ?? string.Empty).With("expectedHash", s.Entry.ContentHash!).With("actualHash", s.CurrentHash.Value).With("resolutions", "force,skip-modified"));
            return Finish(ExitCodes.OwnershipConflict);
        }

        // 4. Run context for the writer inside the T4 host; diagnostics file cleared for this run.
        var runId = Guid.NewGuid().ToString("N");
        report.RunId = runId;
        new RunContext { RunId = runId, Resolution = resolution, MarkersEnabled = settings.GeneratedCodeMarkers, TemplateSourceKind = templateSource.Kind.ToString(), TemplateSourceLocation = templateSource.Location, TemplateSourceVersion = templateSource.Version }.Save(root, aggregate);
        var diagFile = DiagnosticsFile.For(root, aggregate);
        diagFile.Clear();

        // 5. Pipeline.
        var options = new PipelineOptions
        {
            SolutionRoot = root, SolutionFile = slnFile, AggregateName = aggregate, TemplateSource = templateSource,
            MaxParallelT4 = settings.MaxParallelT4, WaitTimeoutMs = settings.WaitTimeoutMs, PollIntervalMs = settings.PollIntervalMs,
            IncludeTests = !o.Flag("no-tests") && settings.GenerateTests, SkipRestore = o.Flag("skip-restore"),
        };
        PipelineResult result;
        try
        {
            using var transformer = new DteTemplateTransformer(vs, log);
            result = new AggregatePipeline(new ProcessRunner(), log).RunAsync(options, transformer).GetAwaiter().GetResult();
        }
        catch (TimeoutException ex)
        {
            diagnostics.Report(new Diagnostic(DiagnosticCodes.TransformationFailed, DiagnosticSeverity.Error, slnFile, "Timed out: " + ex.Message));
            RunContext.Delete(root, aggregate);
            return Finish(ExitCodes.Timeout);
        }
        catch (Exception ex)
        {
            // A run that dies inside the pipeline still owes the caller its report: the harness, the CLI and
            // the VSIX read the report, not the console, and a missing report turns a diagnosable failure into
            // "the file does not exist". The stack goes to the diagnostic so the cause survives in the evidence.
            diagnostics.Report(new Diagnostic(DiagnosticCodes.TransformationFailed, DiagnosticSeverity.Error, slnFile,
                "The generation pipeline failed unexpectedly: " + ex.Message)
                .With("exception", ex.GetType().FullName ?? ex.GetType().Name)
                .With("stack", ex.ToString()));
            RunContext.Delete(root, aggregate);
            return Finish(ExitCodes.HarnessFailure);
        }
        finally
        {
            RunContext.Delete(root, aggregate);
        }
        report.Pipeline = result.Steps.Select(s => new RunReport.StepRecord { Phase = s.Phase, Name = s.Name, Succeeded = s.Succeeded, Attempts = s.Attempts, DurationMs = s.DurationMs, Detail = s.Detail }).ToList();
        report.TransformedTemplates = result.TransformedTemplates.Select(t => SolutionRoot.TryMakeRelative(SolutionRoot.NormalizeFullPath(root), t) ?? t).ToList();
        report.DomainGatePassed = result.DomainGatePassed;
        foreach (var d in result.Diagnostics.Items) diagnostics.Report(d);

        // 6. Diagnostics emitted by the writer inside the T4 host.
        var writerDiagnostics = diagFile.Read();
        foreach (var d in writerDiagnostics) diagnostics.Report(d);

        // 7. Manifest summary.
        var final = ManifestStore.Load(manifestRoot, aggregate, diagnostics);
        if (final.Loaded)
        {
            report.Manifest = new RunReport.ManifestRecord
            {
                Path = SolutionRoot.TryMakeRelative(SolutionRoot.NormalizeFullPath(root), final.Path) ?? final.Path,
                SchemaVersion = final.Manifest!.SchemaVersion,
                GeneratorVersion = final.Manifest.GeneratorVersion,
                ContractVersion = final.Manifest.ContractVersion,
                Files = final.Manifest.Files.Count,
            };
        }

        var ownershipCodes = new[] { DiagnosticCodes.Drifted, DiagnosticCodes.Unverified };
        if (diagnostics.Items.Any(d => d.Severity == DiagnosticSeverity.Error && ownershipCodes.Contains(d.Code))) return Finish(ExitCodes.OwnershipConflict);
        if (diagnostics.HasErrors || !result.Succeeded) return Finish(ExitCodes.AssertionFailure);
        return Finish(ExitCodes.Success);
    }

    private static ResolutionMode ParseResolution(string text)
    {
        switch (text.Trim().ToLowerInvariant())
        {
            case "protect": case "none": return ResolutionMode.Protect;
            case "force": return ResolutionMode.Force;
            case "skip-modified": case "skip": return ResolutionMode.SkipModified;
            default: throw new CommandLine.UsageException("--resolution must be protect, force or skip-modified");
        }
    }

    /// <summary>
    /// Template source: an explicit --templates value wins; otherwise lazy.settings decides. Package
    /// mode never degrades to a local tree: when the templates package is not restored the run fails
    /// with LC3001 naming the package id, the version and every location probed.
    /// </summary>
    private static TemplateSource? ResolveTemplateSource(CommandLine o, LazySettings settings, string root, DiagnosticCollector diagnostics, Action<string> log)
    {
        var spec = o.Get("templates");
        if (spec != null)
        {
            var explicitSource = SandboxCommand.ParseTemplateSource(spec, settings.TemplatesNuGetVersion, settings.TemplatesNuGetPackageId);
            if (explicitSource.Kind == TemplateSourceKind.Local && settings.DisableLocalTemplateFallback)
            {
                diagnostics.Report(new Diagnostic(DiagnosticCodes.TemplateResolutionFailed, DiagnosticSeverity.Error, explicitSource.Location,
                    "Local template resolution is disabled for this solution (DisableLocalTemplateFallback=True, clean-room or strict validation); package resolution is required."));
                return null;
            }
            return explicitSource;
        }

        var useLocal = !settings.UseNugetTemplates && string.Equals(settings.TemplatesSource, "Local", StringComparison.OrdinalIgnoreCase);
        if (useLocal)
        {
            if (settings.DisableLocalTemplateFallback)
            {
                diagnostics.Report(new Diagnostic(DiagnosticCodes.TemplateResolutionFailed, DiagnosticSeverity.Error, root,
                    "lazy.settings selects local templates but local resolution is disabled (DisableLocalTemplateFallback=True)."));
                return null;
            }
            var configured = settings.TemplatesLocalProjectFilesPath;
            var candidates = new List<string>();
            if (!string.IsNullOrWhiteSpace(configured)) candidates.Add(Path.IsPathRooted(configured) ? configured : Path.Combine(root, configured));
            candidates.Add(Path.Combine(root, "src", "ProjectFiles"));
            candidates.Add(Path.Combine(root, "ProjectFiles"));
            var found = candidates.FirstOrDefault(c => Directory.Exists(c) && File.Exists(Path.Combine(c, "project.tt")));
            if (found == null)
            {
                diagnostics.Report(new Diagnostic(DiagnosticCodes.TemplateResolutionFailed, DiagnosticSeverity.Error, root,
                    "Local templates were configured but no template tree containing project.tt was found. Probed: " + string.Join("; ", candidates)));
                return null;
            }
            return new TemplateSource { Kind = TemplateSourceKind.Local, Location = Path.GetFullPath(found), Version = "source-tree" };
        }

        // NuGet mode: the templates package must be restored (global packages folder or the solution's nuget.config globalPackagesFolder).
        var requested = settings.TemplatesNuGetPackageId;
        var canonical = TemplateRegistry.CanonicalTemplatesPackageId;
        var isAlias = string.Equals(requested, TemplateRegistry.DeprecatedTemplatesPackageId, StringComparison.OrdinalIgnoreCase);
        if (isAlias)
            diagnostics.Report(new Diagnostic(DiagnosticCodes.DeprecatedTemplatesPackageId, DiagnosticSeverity.Warning, requested,
                "The templates package id '" + requested + "' is deprecated; it resolves to the canonical '" + canonical + "'. Update TemplatesNuGetPackageId in lazy.settings."));
        var packageId = isAlias || string.IsNullOrWhiteSpace(requested) ? canonical : requested;
        var probed = new List<string>();
        foreach (var folder in PackageFolders(root))
        {
            var dir = Path.Combine(folder, packageId.ToLowerInvariant());
            probed.Add(dir);
            if (!Directory.Exists(dir)) continue;
            var versions = Directory.GetDirectories(dir).Select(Path.GetFileName).Where(v => v != null).Select(v => v!).ToList();
            string? version = null;
            if (!string.IsNullOrWhiteSpace(settings.TemplatesNuGetVersion) && versions.Contains(settings.TemplatesNuGetVersion, StringComparer.OrdinalIgnoreCase)) version = settings.TemplatesNuGetVersion;
            else if (versions.Count > 0) version = versions.OrderByDescending(v => PlatformInfo.ParseVersion(v)).ThenByDescending(v => v, StringComparer.Ordinal).First();
            if (version == null) continue;
            var content = Path.Combine(dir, version, "contentFiles", "any", "any", "LazyCrudTemplates");
            probed.Add(content);
            if (File.Exists(Path.Combine(content, "project.tt")))
                return new TemplateSource { Kind = TemplateSourceKind.NuGet, Location = content, Version = version, RequestedPackageId = requested };
        }
        diagnostics.Report(new Diagnostic(DiagnosticCodes.TemplateResolutionFailed, DiagnosticSeverity.Error, packageId,
            "The templates package '" + packageId + "'" + (string.IsNullOrWhiteSpace(settings.TemplatesNuGetVersion) ? "" : " version " + settings.TemplatesNuGetVersion) +
            " is not restored (configured source: NuGet, requested id '" + requested + "'). Probed: " + string.Join("; ", probed) +
            ". Restore the solution with a feed that contains the package. Package resolution never falls back to a local source tree.")
            .With("packageId", packageId).With("requestedPackageId", requested).With("version", settings.TemplatesNuGetVersion ?? "").With("probed", string.Join(";", probed)));
        return null;
    }

    /// <summary>
    /// Embedded descriptors of every Lazy.Crud.* package restored for the solution's projects, read
    /// from the package folders on disk (obj/project.assets.json), without invoking MSBuild.
    /// </summary>
    private static IEnumerable<DescriptorCandidate> DescriptorCandidates(string root, Action<string> log)
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var folders = PackageFolders(root).Where(Directory.Exists).ToList();
        foreach (var assets in Directory.EnumerateFiles(root, "project.assets.json", SearchOption.AllDirectories))
        {
            System.Text.Json.JsonDocument doc;
            try { doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(assets)); } catch { continue; }
            using (doc)
            {
                if (!doc.RootElement.TryGetProperty("libraries", out var libs)) continue;
                foreach (var lib in libs.EnumerateObject())
                {
                    var slash = lib.Name.IndexOf('/');
                    if (slash < 0) continue;
                    var id = lib.Name.Substring(0, slash);
                    var version = lib.Name.Substring(slash + 1);
                    if (!id.StartsWith("Lazy.Crud.", StringComparison.OrdinalIgnoreCase)) continue;
                    var key = id.ToLowerInvariant() + "/" + version;
                    if (!seen.Add(key)) continue;
                    foreach (var folder in folders)
                    {
                        var path = Path.Combine(folder, id.ToLowerInvariant(), version, "lazycrud", "lazycrud.platform.json");
                        if (File.Exists(path))
                        {
                            yield return new DescriptorCandidate(id + " " + version, File.ReadAllText(path));
                            break;
                        }
                    }
                }
            }
        }
    }

    private static IEnumerable<string> PackageFolders(string root)
    {
        // 1. globalPackagesFolder from the solution's nuget.config (clean-room sandboxes isolate their cache).
        var configured = ConfiguredGlobalPackagesFolder(root);
        if (configured != null) yield return configured;
        var env = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
        if (!string.IsNullOrEmpty(env)) yield return env;
        yield return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages");
    }

    private static string? ConfiguredGlobalPackagesFolder(string root)
    {
        var cfg = Path.Combine(root, "nuget.config");
        if (!File.Exists(cfg)) return null;
        try
        {
            var x = System.Xml.Linq.XDocument.Load(cfg);
            var v = x.Descendants("config").SelectMany(c => c.Elements("add"))
                .FirstOrDefault(a => string.Equals((string?)a.Attribute("key"), "globalPackagesFolder", StringComparison.OrdinalIgnoreCase))?.Attribute("value")?.Value;
            if (string.IsNullOrEmpty(v)) return null;
            return Path.IsPathRooted(v) ? v : Path.Combine(root, v);
        }
        catch
        {
            return null;
        }
    }
}

public sealed class RunReport
{
    public string Command { get; set; } = "";
    public string? RunId { get; set; }
    public string SolutionRoot { get; set; } = "";
    public string? SolutionFile { get; set; }
    public string Aggregate { get; set; } = "";
    public string Resolution { get; set; } = "";
    public string PlatformVersion { get; set; } = PlatformInfo.Version;
    public int ContractVersion { get; set; } = PlatformInfo.ContractVersion;
    public DateTime StartedUtc { get; set; }
    public double DurationMs { get; set; }
    public bool Succeeded { get; set; }
    public int ExitCode { get; set; }
    public string ExitCodeName { get; set; } = "";
    public string? SettingsPath { get; set; }
    public TemplateSourceRecord? TemplateSource { get; set; }
    public Dictionary<string, object?>? Toolchain { get; set; }
    public DescriptorRecord? Descriptor { get; set; }
    public bool DomainGatePassed { get; set; }
    public List<FileStateRecord> PreCheck { get; set; } = new();
    public List<StepRecord> Pipeline { get; set; } = new();
    public List<string> TransformedTemplates { get; set; } = new();
    public ManifestRecord? Manifest { get; set; }
    public List<DiagnosticRecord> Diagnostics { get; set; } = new();

    public sealed class TemplateSourceRecord { public string Kind { get; set; } = ""; public string Location { get; set; } = ""; public string Version { get; set; } = ""; public string RequestedPackageId { get; set; } = ""; }
    public sealed class DescriptorRecord { public string? Source { get; set; } public string? PlatformVersion { get; set; } public int ContractVersion { get; set; } public List<string> Candidates { get; set; } = new(); }
    public sealed class FileStateRecord { public string RelativePath { get; set; } = ""; public string State { get; set; } = ""; public string? Ownership { get; set; } public bool UnknownOwnership { get; set; } }
    public sealed class StepRecord { public string Phase { get; set; } = ""; public string Name { get; set; } = ""; public bool Succeeded { get; set; } public int Attempts { get; set; } public double DurationMs { get; set; } public string? Detail { get; set; } }
    public sealed class ManifestRecord { public string Path { get; set; } = ""; public int SchemaVersion { get; set; } public string? GeneratorVersion { get; set; } public int ContractVersion { get; set; } public int Files { get; set; } }
    public sealed class DiagnosticRecord
    {
        public string Code { get; set; } = ""; public string Severity { get; set; } = ""; public string Path { get; set; } = ""; public string Message { get; set; } = ""; public Dictionary<string, string> Data { get; set; } = new();
        public static DiagnosticRecord From(Diagnostic d) => new() { Code = d.Code, Severity = d.Severity.ToString().ToLowerInvariant(), Path = d.Path, Message = d.Message, Data = new Dictionary<string, string>(d.Data) };
    }
}
