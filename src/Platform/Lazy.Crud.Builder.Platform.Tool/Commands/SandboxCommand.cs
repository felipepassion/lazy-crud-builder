using Lazy.Crud.Builder.Platform.Tool.Pipeline;

namespace Lazy.Crud.Builder.Platform.Tool.Commands;

public static class SandboxCommand
{
    public static int Run(CommandLine o)
    {
        var sub = o.Positional.FirstOrDefault() ?? "create";
        if (sub != "create") throw new CommandLine.UsageException("sandbox create ...");
        var output = Path.GetFullPath(o.Require("output"));
        string? fixture = null;
        if (o.Get("fixture") is { } fixtureArg)
        {
            fixture = Path.GetFullPath(fixtureArg);
            if (!Directory.Exists(fixture)) { Console.Error.WriteLine("fixture directory not found: " + fixture); return ExitCodes.InvalidFixtureOrSandbox; }
        }
        var source = ParseTemplateSource(o.Get("templates") ?? "nuget", o.Get("version") ?? PlatformInfo.Version, o.Get("templates-package"));
        var overrides = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var ov in o.GetAll("override"))
        {
            var eq = ov.IndexOf('=');
            if (eq <= 0) throw new CommandLine.UsageException("--override expects PackageId=version");
            overrides[ov.Substring(0, eq)] = ov.Substring(eq + 1);
        }
        var options = new SandboxOptions
        {
            SandboxRoot = output,
            AggregateName = o.Get("aggregate") ?? "Products",
            FixtureDirectory = fixture,
            TemplateSource = source,
            PlatformVersion = o.Get("version") ?? PlatformInfo.Version,
            PackageVersionOverrides = overrides,
            LocalFeeds = o.GetAll("feed").Select(Path.GetFullPath).ToList(),
            CleanRoom = o.Flag("clean-room"),
            IsolatedPackagesFolder = o.Get("packages-folder") is { } pf ? Path.GetFullPath(pf) : null,
            PlatformAssemblyPath = o.Get("platform-assembly"),
            TemplatesPackageId = o.Get("templates-package") ?? global::Lazy.Crud.Builder.Platform.Descriptor.TemplateRegistry.CanonicalTemplatesPackageId,
            IncludeTests = !o.Flag("no-tests"),
            MaxParallelT4 = o.GetInt("max-parallel", 1),
            IncludeTemplatesPackageReference = !o.Flag("no-templates-package"),
        };
        try
        {
            if (Directory.Exists(output) && o.Flag("clean")) Directory.Delete(output, true);
            var sln = SandboxBuilder.Build(options, m => Console.Error.WriteLine(m));

            // A created sandbox is restored, so its packages (the templates package in particular)
            // are present before anything asks for them. --no-restore leaves that to the caller.
            if (!o.Flag("no-restore"))
            {
                DiscardStaleExtractedPackages(options);
                var restore = Restore(sln, Path.GetDirectoryName(sln)!);
                if (restore != 0)
                {
                    Console.Error.WriteLine("sandbox restore failed with exit code " + restore +
                                            ". The sandbox exists at " + output + "; fix the feed or run dotnet restore there.");
                    return ExitCodes.InvalidFixtureOrSandbox;
                }
            }

            Console.WriteLine(sln);
            return ExitCodes.Success;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("sandbox creation failed: " + ex.Message);
            return ExitCodes.InvalidFixtureOrSandbox;
        }
    }

    /// <summary>
    /// NuGet caches a package by id and version, so a package rebuilt at the same version never reaches
    /// a machine that already restored it: the previously extracted copy keeps being served, and the
    /// sandbox silently exercises the old build. When a local folder feed carries a newer .nupkg than
    /// the extracted copy, the extracted copy is discarded so the restore takes the new one.
    /// </summary>
    private static void DiscardStaleExtractedPackages(SandboxOptions options)
    {
        if (options.LocalFeeds == null || options.LocalFeeds.Count == 0) return;
        var globalPackages = options.IsolatedPackagesFolder ?? DefaultGlobalPackagesFolder();
        if (globalPackages == null || !Directory.Exists(globalPackages)) return;

        foreach (var feed in options.LocalFeeds)
        {
            if (!Directory.Exists(feed)) continue;
            foreach (var nupkg in Directory.GetFiles(feed, "*.nupkg"))
            {
                var name = Path.GetFileNameWithoutExtension(nupkg);
                var suffix = "." + options.PlatformVersion;
                if (!name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)) continue;
                var id = name.Substring(0, name.Length - suffix.Length).ToLowerInvariant();
                var extracted = Path.Combine(globalPackages, id, options.PlatformVersion);
                if (!Directory.Exists(extracted)) continue;
                try
                {
                    if (Directory.GetLastWriteTimeUtc(extracted) >= File.GetLastWriteTimeUtc(nupkg)) continue;
                    Directory.Delete(extracted, true);
                    Console.Error.WriteLine("  cache: discarded the extracted copy of " + id + " " + options.PlatformVersion +
                                            " because the feed carries a newer package");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("  cache: the extracted copy of " + id + " " + options.PlatformVersion +
                                            " could not be discarded (" + ex.GetType().Name + ": " + ex.Message +
                                            "); the restore may serve the previous build of that package");
                }
            }
        }
    }

    private static string? DefaultGlobalPackagesFolder()
    {
        var configured = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
        if (!string.IsNullOrEmpty(configured)) return configured;
        var profile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return string.IsNullOrEmpty(profile) ? null : Path.Combine(profile, ".nuget", "packages");
    }

    private static int Restore(string solutionFile, string workingDirectory)

    {
        var psi = new System.Diagnostics.ProcessStartInfo("dotnet", "restore \"" + solutionFile + "\" --nologo -v q")
        {
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };
        using var process = System.Diagnostics.Process.Start(psi);
        if (process == null) { Console.Error.WriteLine("dotnet restore could not be started"); return 1; }
        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();
        foreach (var line in (stdout + stderr).Split('\n')) if (line.Trim().Length > 0) Console.Error.WriteLine("  " + line.TrimEnd());
        return process.ExitCode;
    }

    public static TemplateSource ParseTemplateSource(
string spec, string version, string? templatesPackage)
    {
        if (spec.StartsWith("local:", StringComparison.OrdinalIgnoreCase))
        {
            var dir = Path.GetFullPath(spec.Substring(6));
            if (!Directory.Exists(dir)) throw new CommandLine.UsageException("local template directory not found: " + dir);
            return new TemplateSource { Kind = TemplateSourceKind.Local, Location = dir, Version = "source-tree" };
        }
        if (string.Equals(spec, "nuget", StringComparison.OrdinalIgnoreCase))
        {
            return new TemplateSource { Kind = TemplateSourceKind.NuGet, Location = "nuget", Version = version, RequestedPackageId = templatesPackage ?? global::Lazy.Crud.Builder.Platform.Descriptor.TemplateRegistry.CanonicalTemplatesPackageId };
        }
        throw new CommandLine.UsageException("--templates must be nuget or local:<dir>");
    }
}
