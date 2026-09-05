using System.Diagnostics;
using System.Text;

namespace Lazy.Crud.Builder.Platform.Tool.Pipeline;

public sealed class SandboxOptions
{
    public required string SandboxRoot { get; init; }
    public required string AggregateName { get; init; }
    /// <summary>Directory holding the reference aggregate model (tests/fixtures/reference-aggregate); null scaffolds a minimal sample model.</summary>
    public string? FixtureDirectory { get; init; }
    public required TemplateSource TemplateSource { get; init; }
    /// <summary>Version pinned for every Lazy.Crud.* package (overridable per package).</summary>
    public required string PlatformVersion { get; init; }
    public IReadOnlyDictionary<string, string> PackageVersionOverrides { get; init; } = new Dictionary<string, string>();
    /// <summary>Local folder feed(s) added to nuget.config (the pack output standing in for the public feed).</summary>
    public IReadOnlyList<string> LocalFeeds { get; init; } = Array.Empty<string>();
    /// <summary>Clean room: clear inherited sources, isolated package cache, no local template fallback.</summary>
    public bool CleanRoom { get; init; }
    public string? IsolatedPackagesFolder { get; init; }
    /// <summary>Local mode only: absolute path of the Platform assembly for $(LazyCrudPlatformAssembly).</summary>
    public string? PlatformAssemblyPath { get; init; }
    public string TemplatesPackageId { get; init; } = global::Lazy.Crud.Builder.Platform.Descriptor.TemplateRegistry.CanonicalTemplatesPackageId;
    public bool IncludeTests { get; init; } = true;
    public string GenerationOutputPath { get; init; } = "LazyCode";
    public int MaxParallelT4 { get; init; } = 1;
    /// <summary>False when the consumer must not reference the templates package directly (legacy package sets without one).</summary>
    public bool IncludeTemplatesPackageReference { get; init; } = true;
}

/// <summary>
/// Materializes a consumer solution for the reference aggregate exactly as an external user would
/// have it: layer projects with package references, wrappers, lazy.settings, nuget.config, and a
/// .sln. Nothing in the sandbox references Builder project sources; templates come either from
/// installed packages (public-equivalent artifacts) or, in explicitly selected local development
/// mode, from a template source tree.
/// </summary>
public static class SandboxBuilder
{
    private static readonly UTF8Encoding Utf8Bom = new(true);

    public static string Build(SandboxOptions o, Action<string> log)
    {
        var root = o.SandboxRoot;
        Directory.CreateDirectory(root);
        var src = Path.Combine(root, "src");
        var aggDir = Path.Combine(src, o.AggregateName);
        Directory.CreateDirectory(aggDir);

        // Copy the fixture model into the layer projects (fixture layout: <Layer>/... relative to the layer project).
        foreach (var layer in AggregateLayout.Layers)
        {
            var projDir = Path.Combine(aggDir, o.AggregateName + layer.ProjectSuffix);
            Directory.CreateDirectory(projDir);
            if (o.FixtureDirectory != null)
            {
                var fixtureLayer = Path.Combine(o.FixtureDirectory, layer.Id);
                if (Directory.Exists(fixtureLayer)) CopyTree(fixtureLayer, projDir, o.AggregateName);
            }
            else
            {
                WriteDefaultModel(projDir, layer, o.AggregateName);
            }
            // Consumer convention (what the IDE extension creates): directory {Aggregate}.{Layer}, project Lazy.Crud.{Aggregate}.{Layer}.
            File.WriteAllText(Path.Combine(projDir, "Lazy.Crud." + o.AggregateName + layer.ProjectSuffix + ".csproj"), LayerCsproj(o, layer), Utf8Bom);
            WrapperSeeder.EnsureLayer(projDir, layer, o.TemplateSource, log);
        }

        if (o.IncludeTests)
        {
            foreach (var t in AggregateLayout.TestLayers)
            {
                var projName = "Lazy.Crud." + o.AggregateName + "." + t.Id;
                var projDir = Path.Combine(aggDir, AggregateLayout.TestsFolderName, projName);
                Directory.CreateDirectory(projDir);
                File.WriteAllText(Path.Combine(projDir, projName + ".csproj"), TestCsproj(o, t), Utf8Bom);
                File.WriteAllText(Path.Combine(projDir, "SmokeTest.cs"),
                    "using Xunit;\r\n\r\nnamespace " + projName.Replace('.', '_') + ";\r\n\r\npublic class SmokeTest\r\n{\r\n    [Fact]\r\n    public void ProjectCompiles() => Assert.True(true);\r\n}\r\n", Utf8Bom);
                WrapperSeeder.EnsureTestLayer(projDir, t, o.TemplateSource, log);
            }
        }

        // lazy.settings (src/Mods/lazy.settings), the location every surface probes first.
        var mods = Path.Combine(src, "Mods");
        Directory.CreateDirectory(mods);
        File.WriteAllText(Path.Combine(mods, "lazy.settings"), LazySettingsXml(o), Utf8Bom);

        // Directory.Build.props: consumer-level switches; local mode publishes the template root explicitly.
        File.WriteAllText(Path.Combine(root, "Directory.Build.props"), DirectoryBuildProps(o), Utf8Bom);

        // nuget.config: local feed(s) standing in for the public feed; clean room clears inherited sources.
        File.WriteAllText(Path.Combine(root, "nuget.config"), NuGetConfig(o), Utf8Bom);

        // Solution file.
        var sln = Path.Combine(root, "Sandbox.sln");
        CreateSolution(root, sln, log);

        var report = Path.Combine(root, "sandbox.json");
        File.WriteAllText(report, System.Text.Json.JsonSerializer.Serialize(new
        {
            aggregate = o.AggregateName,
            platformVersion = o.PlatformVersion,
            templateSource = o.TemplateSource.Kind.ToString(),
            templateLocation = o.TemplateSource.Location,
            templatesPackageId = o.TemplatesPackageId,
            cleanRoom = o.CleanRoom,
            isolatedPackagesFolder = o.IsolatedPackagesFolder,
            localFeeds = o.LocalFeeds,
            solution = sln,
            createdUtc = DateTime.UtcNow,
        }, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }), Utf8Bom);
        log("sandbox created: " + root);
        return sln;
    }

    /// <summary>The minimal sample model written by the quickstart scaffold (no fixture): one aggregate root entity.</summary>
    private static void WriteDefaultModel(string projDir, LayerSpec layer, string agg)
    {
        switch (layer.Id)
        {
            case "Domain":
                var entities = Path.Combine(projDir, "Aggregates", agg + "Agg", "Entities");
                Directory.CreateDirectory(entities);
                File.WriteAllText(Path.Combine(entities, agg + ".cs"),
                    "using System.ComponentModel;\r\nusing Lazy.Crud.Builder.Application.DTO.Attributes;\r\nusing Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.Entities;\r\nusing Lazy.Crud.Builder.Domain.Attributes.T4;\r\n\r\n" +
                    "namespace Lazy.Crud." + agg + ".Domain.Aggregates." + agg + "Agg.Entities\r\n{\r\n" +
                    "    /// <summary>Aggregate root of " + agg + ". Edit this model, then regenerate.</summary>\r\n" +
                    "    [AggregateSettingsT4, EndpointsT4(EndpointTypes.HttpAll)]\r\n" +
                    "    public class " + agg + " : Entity\r\n    {\r\n" +
                    "        [RequiredT4, DisplayOnList(1), DisplayName(\"Name\")]\r\n        public string Name { get; set; }\r\n\r\n" +
                    "        [DisplayName(\"Description\")]\r\n        public string Description { get; set; }\r\n    }\r\n}\r\n", Utf8Bom);
                break;
            case "Enumerations":
                File.WriteAllText(Path.Combine(projDir, agg + "Enumerations.cs"),
                    "namespace Lazy.Crud." + agg + ".Enumerations\r\n{\r\n    public enum " + agg + "Enumerations\r\n    {\r\n        None = 0,\r\n    }\r\n}\r\n", Utf8Bom);
                break;
            case "Api":
                File.WriteAllText(Path.Combine(projDir, "Program.cs"),
                    "using Lazy.Crud.Builder.Api.Middlewares;\r\nusing Lazy.Crud." + agg + ".Api;\r\n\r\nvar builder = WebApplication.CreateBuilder(args);\r\n\r\nbuilder.Services.AddEndpointsApiExplorer();\r\nbuilder.Services.AddSwaggerGen();\r\nbuilder.Services.AddAuthorization();\r\n\r\nbuilder.Services.InjectDependencies(builder.Configuration);\r\n\r\nvar app = builder.Build();\r\n\r\napp.UseSwagger();\r\napp.UseSwaggerUI();\r\n\r\napp.UseForwardedHeaders();\r\napp.UseHttpsRedirection();\r\n\r\napp.UseAuthorization();\r\n\r\napp.MapControllers();\r\napp.OnAppInitialized();\r\napp.UseMiddleware<ErrorHandlingMiddleware>();\r\n\r\napp.Run();\r\n", Utf8Bom);
                break;
        }
    }

    private static void CopyTree(string from, string to, string aggregateName)
    {
        foreach (var file in Directory.GetFiles(from, "*", SearchOption.AllDirectories))
        {
            var rel = file.Substring(from.Length).TrimStart('\\', '/').Replace("{Aggregate}", aggregateName);
            var dest = Path.Combine(to, rel);
            Directory.CreateDirectory(Path.GetDirectoryName(dest)!);
            var text = File.ReadAllText(file).Replace("{Aggregate}", aggregateName);
            File.WriteAllText(dest, text, Utf8Bom);
        }
    }

    private static string Version(SandboxOptions o, string packageId)
    {
        return o.PackageVersionOverrides.TryGetValue(packageId, out var v) ? v : o.PlatformVersion;
    }

    private static string LayerCsproj(SandboxOptions o, LayerSpec layer)
    {
        var sb = new StringBuilder();
        sb.Append("<Project Sdk=\"").Append(layer.Sdk).Append("\">\r\n");
        sb.Append("  <PropertyGroup>\r\n    <TargetFramework>net10.0</TargetFramework>\r\n    <ImplicitUsings>enable</ImplicitUsings>\r\n    <Nullable>disable</Nullable>\r\n    <NoWarn>$(NoWarn);1701;1702;8618;1998;8602;8625;8629;CS1591;CS8632;CS0108;CS0114</NoWarn>\r\n    <IsPackable>false</IsPackable>\r\n  </PropertyGroup>\r\n");
        sb.Append("  <ItemGroup>\r\n");
        sb.Append("    <PackageReference Include=\"").Append(layer.RuntimePackageId).Append("\" Version=\"").Append(Version(o, layer.RuntimePackageId)).Append("\" GeneratePathProperty=\"true\" />\r\n");
        foreach (var p in layer.ExtraPlatformPackages)
            sb.Append("    <PackageReference Include=\"").Append(p).Append("\" Version=\"").Append(Version(o, p)).Append("\" GeneratePathProperty=\"true\" />\r\n");
        if (o.IncludeTemplatesPackageReference && (layer.Id == "Domain" || layer.Id == "Api"))
        {
            // The templates package is a transitive dependency of every layer package; a direct reference on one
            // project per aggregate keeps $(PkgLazy_Crud_Builder_Templates) available for tooling and diagnostics.
            sb.Append("    <PackageReference Include=\"").Append(o.TemplatesPackageId).Append("\" Version=\"").Append(Version(o, o.TemplatesPackageId)).Append("\" GeneratePathProperty=\"true\" />\r\n");
        }
        foreach (var p in layer.ThirdPartyPackages)
        {
            var v = AggregateLayout.ThirdPartyVersions[p];
            if (p == "Microsoft.EntityFrameworkCore.Design")
                sb.Append("    <PackageReference Include=\"").Append(p).Append("\" Version=\"").Append(v).Append("\" PrivateAssets=\"all\" />\r\n");
            else
                sb.Append("    <PackageReference Include=\"").Append(p).Append("\" Version=\"").Append(v).Append("\" />\r\n");
        }
        sb.Append("  </ItemGroup>\r\n");
        if (layer.LocalReferences.Count > 0)
        {
            sb.Append("  <ItemGroup>\r\n");
            foreach (var r in layer.LocalReferences)
                sb.Append("    <ProjectReference Include=\"..\\").Append(o.AggregateName).Append('.').Append(r).Append("\\Lazy.Crud.").Append(o.AggregateName).Append('.').Append(r).Append(".csproj\" />\r\n");
            sb.Append("  </ItemGroup>\r\n");
        }
        sb.Append("  <ItemGroup>\r\n    <None Update=\"*.tt\">\r\n      <Generator>TextTemplatingFileGenerator</Generator>\r\n    </None>\r\n");
        if (layer.Id == "Api")
            sb.Append("    <None Update=\"Controllers\\Controllers.tt\">\r\n      <Generator>TextTemplatingFileGenerator</Generator>\r\n    </None>\r\n");
        sb.Append("  </ItemGroup>\r\n</Project>\r\n");
        return sb.ToString();
    }

    private static string TestCsproj(SandboxOptions o, TestLayerSpec t)
    {
        var target = o.AggregateName + "." + t.TargetLayer;
        var sb = new StringBuilder();
        sb.Append("<Project Sdk=\"Microsoft.NET.Sdk\">\r\n  <PropertyGroup>\r\n    <TargetFramework>net10.0</TargetFramework>\r\n    <ImplicitUsings>enable</ImplicitUsings>\r\n    <Nullable>disable</Nullable>\r\n    <IsPackable>false</IsPackable>\r\n    <IsTestProject>true</IsTestProject>\r\n    <NoWarn>$(NoWarn);CS1591;CS8632;xUnit1004</NoWarn>\r\n  </PropertyGroup>\r\n");
        sb.Append("  <ItemGroup>\r\n");
        sb.Append("    <PackageReference Include=\"Microsoft.NET.Test.Sdk\" Version=\"").Append(AggregateLayout.ThirdPartyVersions["Microsoft.NET.Test.Sdk"]).Append("\" />\r\n");
        sb.Append("    <PackageReference Include=\"xunit\" Version=\"").Append(AggregateLayout.ThirdPartyVersions["xunit"]).Append("\" />\r\n");
        sb.Append("    <PackageReference Include=\"xunit.runner.visualstudio\" Version=\"").Append(AggregateLayout.ThirdPartyVersions["xunit.runner.visualstudio"]).Append("\">\r\n      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>\r\n      <PrivateAssets>all</PrivateAssets>\r\n    </PackageReference>\r\n");
        var layer = AggregateLayout.FindLayer(t.TargetLayer)!;
        sb.Append("    <PackageReference Include=\"").Append(layer.RuntimePackageId).Append("\" Version=\"").Append(Version(o, layer.RuntimePackageId)).Append("\" GeneratePathProperty=\"true\" />\r\n");
        sb.Append("  </ItemGroup>\r\n");
        sb.Append("  <ItemGroup>\r\n    <ProjectReference Include=\"..\\..\\").Append(target).Append("\\Lazy.Crud.").Append(target).Append(".csproj\" />\r\n  </ItemGroup>\r\n");
        sb.Append("  <ItemGroup>\r\n    <None Update=\"*.tt\">\r\n      <Generator>TextTemplatingFileGenerator</Generator>\r\n    </None>\r\n  </ItemGroup>\r\n</Project>\r\n");
        return sb.ToString();
    }

    private static string LazySettingsXml(SandboxOptions o)
    {
        var isLocal = o.TemplateSource.Kind == TemplateSourceKind.Local;
        var sb = new StringBuilder();
        sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<SettingsFile CurrentProfile=\"(Default)\" xmlns=\"http://schemas.microsoft.com/VisualStudio/2004/01/settings\">\r\n  <Profiles />\r\n  <Settings>\r\n");
        void Setting(string name, string type, string value) => sb.Append("    <Setting Name=\"").Append(name).Append("\" Type=\"").Append(type).Append("\" Scope=\"Application\">\r\n      <Value Profile=\"(Default)\">").Append(value).Append("</Value>\r\n    </Setting>\r\n");
        Setting("GenerationOutputPath", "System.String", o.GenerationOutputPath);
        Setting("ModsPath", "System.String", "src/mods");
        Setting("TemplatesSource", "System.String", isLocal ? "Local" : "NuGet");
        Setting("UseNugetTemplates", "System.Boolean", isLocal ? "False" : "True");
        Setting("TemplatesLocalProjectFilesPath", "System.String", isLocal ? o.TemplateSource.Location : "");
        Setting("TemplatesNuGetPackageId", "System.String", o.TemplatesPackageId);
        Setting("TemplatesNuGetVersion", "System.String", o.PlatformVersion);
        Setting("DisableLocalTemplateFallback", "System.Boolean", o.CleanRoom ? "True" : "False");
        Setting("MaxParallelT4", "System.Int32", o.MaxParallelT4.ToString());
        Setting("GenerateTests", "System.Boolean", o.IncludeTests ? "True" : "False");
        Setting("TestsPathPattern", "System.String", "Tests\\Lazy.Crud.{Aggregate}.Tests.{Layer}");
        sb.Append("  </Settings>\r\n</SettingsFile>\r\n");
        return sb.ToString();
    }

    private static string DirectoryBuildProps(SandboxOptions o)
    {
        var sb = new StringBuilder();
        sb.Append("<Project>\r\n  <PropertyGroup>\r\n    <ManagePackageVersionsCentrally>false</ManagePackageVersionsCentrally>\r\n    <TreatWarningsAsErrors>false</TreatWarningsAsErrors>\r\n    <LazyCrudSandbox>true</LazyCrudSandbox>\r\n");
        if (o.TemplateSource.Kind == TemplateSourceKind.Local)
        {
            sb.Append("    <!-- Explicit local template development mode: shared includes and the writer assembly resolve from the source tree. -->\r\n");
            sb.Append("    <LazyCrudTemplatesRoot>").Append(o.TemplateSource.Location.TrimEnd('\\', '/')).Append("\\</LazyCrudTemplatesRoot>\r\n");
            if (!string.IsNullOrEmpty(o.PlatformAssemblyPath))
                sb.Append("    <LazyCrudPlatformAssembly>").Append(o.PlatformAssemblyPath).Append("</LazyCrudPlatformAssembly>\r\n");
        }
        sb.Append("  </PropertyGroup>\r\n</Project>\r\n");
        return sb.ToString();
    }

    private static string NuGetConfig(SandboxOptions o)
    {
        var sb = new StringBuilder();
        sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<configuration>\r\n");
        if (o.CleanRoom && !string.IsNullOrEmpty(o.IsolatedPackagesFolder))
        {
            sb.Append("  <config>\r\n    <add key=\"globalPackagesFolder\" value=\"").Append(o.IsolatedPackagesFolder).Append("\" />\r\n  </config>\r\n");
        }
        sb.Append("  <packageSources>\r\n");
        if (o.CleanRoom) sb.Append("    <clear />\r\n");
        int i = 0;
        foreach (var feed in o.LocalFeeds)
            sb.Append("    <add key=\"lazycrud-feed-").Append(++i).Append("\" value=\"").Append(feed).Append("\" />\r\n");
        if (o.CleanRoom) sb.Append("    <add key=\"nuget.org\" value=\"https://api.nuget.org/v3/index.json\" />\r\n");
        sb.Append("  </packageSources>\r\n");
        if (o.CleanRoom && o.LocalFeeds.Count > 0)
        {
            // Package source mapping: Lazy CRUD packages may only come from the feed under test; everything else from nuget.org.
            sb.Append("  <packageSourceMapping>\r\n");
            for (int k = 1; k <= o.LocalFeeds.Count; k++)
                sb.Append("    <packageSource key=\"lazycrud-feed-").Append(k).Append("\">\r\n      <package pattern=\"Lazy.Crud.*\" />\r\n    </packageSource>\r\n");
            sb.Append("    <packageSource key=\"nuget.org\">\r\n      <package pattern=\"*\" />\r\n    </packageSource>\r\n  </packageSourceMapping>\r\n");
        }
        sb.Append("</configuration>\r\n");
        return sb.ToString();
    }

    /// <summary>
    /// Builds the sandbox solution with the same shape a consumer's solution has: one folder for the aggregate
    /// holding its layer projects, and a Tests folder inside it holding its test projects.
    ///
    /// Adding projects without --solution-folder mirrors the directory path instead, and because a layer's
    /// directory (Products.Domain) is not named after its project (Lazy.Crud.Products.Domain), every layer ended
    /// up wrapped in a solution folder of its own: one project per folder, the folder repeating the project's
    /// name. The sandbox exists to reproduce what an external consumer receives, so a layout no consumer gets is
    /// a layout the battery must not validate.
    /// </summary>
    private static void CreateSolution(string root, string sln, Action<string> log)
    {
        if (File.Exists(sln)) File.Delete(sln);
        Run("dotnet", "new sln -n Sandbox --format sln --force", root, log);

        var srcRoot = Path.Combine(root, "src");
        var testsSegment = Path.DirectorySeparatorChar + AggregateLayout.TestsFolderName + Path.DirectorySeparatorChar;
        foreach (var group in Directory.GetFiles(srcRoot, "*.csproj", SearchOption.AllDirectories)
                     .GroupBy(p => SolutionFolderFor(srcRoot, p, testsSegment), StringComparer.OrdinalIgnoreCase)
                     .OrderBy(g => g.Key, StringComparer.Ordinal))
        {
            var args = new StringBuilder("sln \"" + sln + "\" add");
            if (!string.IsNullOrEmpty(group.Key)) args.Append(" --solution-folder \"").Append(group.Key).Append('"');
            foreach (var p in group) args.Append(" \"").Append(p).Append('"');
            Run("dotnet", args.ToString(), root, log);
        }
    }

    /// <summary>
    /// src/&lt;Aggregate&gt; for a layer project, src/&lt;Aggregate&gt;/Tests for a test project. Anything that is
    /// not under an aggregate directory keeps the directory shape it already has.
    /// </summary>
    private static string SolutionFolderFor(string srcRoot, string projectFile, string testsSegment)
    {
        var relative = projectFile.Substring(srcRoot.Length).TrimStart(Path.DirectorySeparatorChar);
        var segments = relative.Split(Path.DirectorySeparatorChar);
        if (segments.Length < 2) return "src";
        var aggregate = segments[0];
        var isTest = projectFile.IndexOf(testsSegment, StringComparison.OrdinalIgnoreCase) >= 0;
        return isTest
            ? "src/" + aggregate + "/" + AggregateLayout.TestsFolderName
            : "src/" + aggregate;
    }

    private static void Run(string file, string args, string cwd, Action<string> log)
    {
        var psi = new ProcessStartInfo(file, args) { WorkingDirectory = cwd, RedirectStandardOutput = true, RedirectStandardError = true, UseShellExecute = false, CreateNoWindow = true };
        using var p = Process.Start(psi)!;
        var output = p.StandardOutput.ReadToEnd() + p.StandardError.ReadToEnd();
        p.WaitForExit();
        if (p.ExitCode != 0) throw new InvalidOperationException($"{file} {args} failed ({p.ExitCode}): {output}");
        foreach (var line in output.Split('\n')) if (line.Trim().Length > 0) log("  " + line.TrimEnd());
    }
}
