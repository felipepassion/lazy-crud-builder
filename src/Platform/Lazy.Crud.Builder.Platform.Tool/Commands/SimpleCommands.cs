using System.Text.Json;
using Lazy.Crud.Builder.Platform.Diagnostics;
using Lazy.Crud.Builder.Platform.Hashing;
using Lazy.Crud.Builder.Platform.Manifests;
using Lazy.Crud.Builder.Platform.Tool.VisualStudio;

namespace Lazy.Crud.Builder.Platform.Tool.Commands;

public static class Json
{
    public static readonly JsonSerializerOptions Options = new() { WriteIndented = true };
    public static string Serialize(object value) => JsonSerializer.Serialize(value, Options);
}

public static class ToolchainCommand
{
    public static int Run(CommandLine o)
    {
        var resolution = VsInstanceLocator.Resolve(o.Get("vs-instance"), m => Console.Error.WriteLine("  " + m));
        Console.WriteLine(Json.Serialize(resolution.ToReport()));
        if (!resolution.Succeeded)
        {
            Console.Error.WriteLine("LC4001 prerequisite unavailable: " + resolution.FailureReason);
            Console.Error.WriteLine("  searched: " + string.Join(" | ", resolution.DiscoveryLog));
            Console.Error.WriteLine("  remedy: " + resolution.Remedy);
            return ExitCodes.PrerequisiteUnavailable;
        }
        return ExitCodes.Success;
    }
}

public static class HashCommand
{
    public static int Run(CommandLine o)
    {
        var file = o.Require("file");
        if (!File.Exists(file)) { Console.Error.WriteLine("file not found: " + file); return ExitCodes.InvalidFixtureOrSandbox; }
        var h = ContentHasher.Compute(File.ReadAllText(file));
        Console.WriteLine(Json.Serialize(new { file, contentHash = h.Value, hashAlgorithm = h.Algorithm, hashScope = h.Scope }));
        return ExitCodes.Success;
    }
}

public static class ManifestCommand
{
    public static int Run(CommandLine o)
    {
        var sub = o.Positional.FirstOrDefault() ?? "";
        if (sub != "migrate") throw new CommandLine.UsageException("manifest <migrate> --solution <dir> --aggregate <name>");
        var root = SolutionRootOf(o.Require("solution"));
        var agg = o.Require("aggregate");
        var diags = new DiagnosticCollector();
        var load = ManifestStore.Load(root, agg, diags);
        foreach (var d in diags.Items) Console.Error.WriteLine("  " + d);
        if (load.NotPresent) { Console.Error.WriteLine("no manifest for " + agg); return ExitCodes.InvalidFixtureOrSandbox; }
        if (load.Failed) return ExitCodes.AssertionFailure;
        if (!ManifestStore.Save(load.Manifest!, root, diags)) return ExitCodes.AssertionFailure;
        Console.WriteLine(File.ReadAllText(load.Path));
        return ExitCodes.Success;
    }

    public static string SolutionRootOf(string pathOrSln)
    {
        var full = Path.GetFullPath(pathOrSln);
        if (File.Exists(full)) return Path.GetDirectoryName(full)!;
        return full;
    }
}

public static class ProbeCommand
{
    public static int Run(CommandLine o)
    {
        if (o.Positional.Count < 2) throw new CommandLine.UsageException("probe <sln> <tt>...");
        var resolution = VsInstanceLocator.Resolve(o.Get("vs-instance"), m => Console.Error.WriteLine("  " + m));
        if (!resolution.Succeeded) { Console.Error.WriteLine("prerequisite unavailable: " + resolution.FailureReason); return ExitCodes.PrerequisiteUnavailable; }
        using var session = DteSession.Launch(resolution.Selected!, m => Console.Error.WriteLine(m));
        Console.Error.WriteLine("DTE version: " + session.Version);
        session.OpenSolution(o.Positional[0]);
        var allOk = true;
        foreach (var tt in o.Positional.Skip(1))
        {
            var ok = session.RunCustomTool(tt);
            Console.Error.WriteLine($"RunCustomTool({Path.GetFileName(tt)}) -> {ok}");
            allOk &= ok;
            foreach (var e in session.ReadErrorList()) Console.Error.WriteLine("  errorlist: " + e);
        }
        session.CloseSolution();
        return allOk ? ExitCodes.Success : ExitCodes.AssertionFailure;
    }
}
