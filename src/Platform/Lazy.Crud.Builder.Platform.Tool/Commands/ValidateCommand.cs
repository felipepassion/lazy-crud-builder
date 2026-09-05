using System.IO.Compression;
using System.Text.RegularExpressions;
using Lazy.Crud.Builder.Platform.Descriptor;
using Lazy.Crud.Builder.Platform.Tool.Descriptor;
using Lazy.Crud.Builder.Platform.Tool.Fixtures;
using Lazy.Crud.Builder.Platform.Tool.Validation;

namespace Lazy.Crud.Builder.Platform.Tool.Commands;

/// <summary>Repository, package and fixture validation checks (CI gates).</summary>
public static class ValidateCommand
{
    public static int Run(CommandLine o)
    {
        var sub = o.Positional.FirstOrDefault() ?? "";
        var repo = Path.GetFullPath(o.Get("repo") ?? Directory.GetCurrentDirectory());
        var report = new ValidationReport(sub);
        switch (sub)
        {
            case "tree": TreeChecks.Run(repo, o, report); break;
            case "packages": PackageChecks.Run(repo, Path.GetFullPath(o.Require("packages")), report); break;
            case "fixtures": FixtureChecks.Run(repo, o.Get("attributes-assembly"), report); break;
            case "manifest": ManifestChecks.Run(repo, Path.GetFullPath(o.Require("file")), report); break;
            default: throw new CommandLine.UsageException("validate <tree|packages|fixtures|manifest>");
        }
        Console.WriteLine(Json.Serialize(report));
        Console.Error.WriteLine(report.Summary());
        // The CI gates write into artifacts/validation/, which does not exist in a fresh clone; create the
        // directory rather than letting a passing validation die writing its own evidence.
        if (o.Get("report") is { } path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
            File.WriteAllText(path, Json.Serialize(report));
        }
        return report.Failed ? ExitCodes.AssertionFailure : ExitCodes.Success;
    }
}

public sealed class ValidationReport
{
    public ValidationReport(string scope) { Scope = scope; }
    public string Scope { get; }
    public DateTime GeneratedUtc { get; } = DateTime.UtcNow;
    public string PlatformVersion { get; } = PlatformInfo.Version;
    public List<Check> Checks { get; } = new();
    public bool Failed => Checks.Any(c => !c.Passed);

    public sealed class Check
    {
        public string Id { get; set; } = "";
        public bool Passed { get; set; }
        public List<string> Problems { get; set; } = new();
        public string? Detail { get; set; }
    }

    public void Add(string id, IEnumerable<string> problems, string? detail = null)
    {
        var list = problems.ToList();
        Checks.Add(new Check { Id = id, Passed = list.Count == 0, Problems = list, Detail = detail });
        Console.Error.WriteLine((list.Count == 0 ? "  PASS " : "  FAIL ") + id + (detail != null ? " (" + detail + ")" : ""));
        foreach (var p in list) Console.Error.WriteLine("       - " + p);
    }

    public string Summary() => $"validate {Scope}: {Checks.Count(c => c.Passed)} passed, {Checks.Count(c => !c.Passed)} failed";
}

public static class TreeChecks
{
    public static void Run(string repo, CommandLine o, ValidationReport report)
    {
        var projectFiles = Path.Combine(repo, "src", "ProjectFiles");

        // Registry <-> shipped templates (both directions).
        var shipped = Directory.GetFiles(projectFiles, "*.tt", SearchOption.AllDirectories).Concat(Directory.GetFiles(projectFiles, "*.ttinclude", SearchOption.AllDirectories))
            .Select(f => f.Substring(projectFiles.Length + 1).Replace('\\', '/')).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var registered = TemplateRegistry.SourceFiles.ToHashSet(StringComparer.OrdinalIgnoreCase);
        report.Add("registry.shipped-templates-registered", shipped.Except(registered).Select(f => "shipped but unregistered: " + f), shipped.Count + " templates shipped");
        report.Add("registry.registered-templates-shipped", registered.Except(shipped).Select(f => "registered but not shipped: " + f), registered.Count + " registered");
        report.Add("registry.ownership-declared", TemplateRegistry.Entries.Where(e => !Enum.IsDefined(typeof(Ownership), e.Ownership)).Select(e => e.Id));
        // Two templates may share an output pattern only when they are documented alternatives that
        // never run for the same aggregate: the Blazor client pages ("full") and their API-only
        // "mini" counterparts, and the legacy CSV/listing pair. Any other collision is a registry bug.
        var documentedCollisions = new HashSet<string>(StringComparer.Ordinal)
        {
            "application-dto.csv-mapper|application-dto.listinings",
            "webapi.basic-address-component|webapi.form-templates-switcher.css|webapi.mini-form-templates-switcher.css",
            "webapi.client-register-pages.css|webapi.mini-client-register-pages.css",
            "webapi.client-register-pages.page|webapi.mini-client-register-pages.page",
            "webapi.form-templates-switcher.page|webapi.mini-form-templates-switcher.page",
        };
        var collisionGroups = TemplateRegistry.Entries.Where(e => e.OutputPattern != null)
            .GroupBy(e => e.Layer + "|" + e.OutputPattern).Where(g => g.Count() > 1)
            .Select(g => string.Join("|", g.Select(e => e.Id).OrderBy(id => id, StringComparer.Ordinal)))
            .ToList();
        report.Add("registry.output-collisions-known", collisionGroups.Where(g => !documentedCollisions.Contains(g)).Select(g => "undocumented output collision: " + g),
            collisionGroups.Count + " collision group(s), every one a documented alternative");

        // Writer call sites: every SaveOutputToSubFolder/SaveOutput call carries explicit ownership and templateId that agree with the registry.
        var callSites = CallSiteScanner.Scan(projectFiles);
        report.Add("ownership.call-sites-explicit", callSites.Where(c => !c.HasOwnership || !c.HasTemplateId).Select(c => c.Location + ": " + (c.HasOwnership ? "" : "missing ownership ") + (c.HasTemplateId ? "" : "missing templateId")),
            callSites.Count + " call sites, " + callSites.Count(c => c.HasOwnership && c.HasTemplateId) + " with explicit ownership and templateId");
        report.Add("ownership.call-sites-agree-with-registry", callSites.Where(c => c.HasTemplateId).Select(c =>
        {
            var e = TemplateRegistry.Find(c.TemplateId!);
            if (e == null) return c.Location + ": templateId '" + c.TemplateId + "' is not in the registry";
            if (c.HasOwnership && c.Ownership != OwnershipNames.ToName(e.Ownership)) return c.Location + ": ownership " + c.Ownership + " disagrees with registry " + OwnershipNames.ToName(e.Ownership) + " for " + e.Id;
            if (!string.Equals(e.SourceFile, c.SourceFile, StringComparison.OrdinalIgnoreCase)) return c.Location + ": templateId '" + c.TemplateId + "' belongs to " + e.SourceFile;
            return null;
        }).Where(p => p != null)!);

        // No silent catch in the shared write path of project.tt.
        var projectTt = File.ReadAllText(Path.Combine(projectFiles, "project.tt"));
        var writerRegion = ExtractRegion(projectTt, "private void SaveOutput(", "public void CreateDirectory(");
        var emptyCatch = Regex.Matches(writerRegion, @"catch\s*(\([^)]*\))?\s*\{\s*(//[^\n]*\s*)*\}");
        report.Add("diagnostics.no-silent-catch-in-writer", emptyCatch.Cast<Match>().Select(m => "empty catch in writer path: " + m.Value.Replace("\r", "").Replace("\n", " ")));
        var allEmptyCatches = Regex.Matches(projectTt, @"catch\s*(\([^)]*\))?\s*\{\s*\}").Count;
        report.Add("diagnostics.no-empty-catch-in-project-tt", allEmptyCatches == 0 ? Array.Empty<string>() : new[] { allEmptyCatches + " empty catch block(s) remain in project.tt" });

        // Version: no project declares its own version.
        var csprojs = Directory.GetFiles(Path.Combine(repo, "src"), "*.csproj", SearchOption.AllDirectories).Where(p => !p.Contains(@"\bin\") && !p.Contains(@"\obj\"));
        report.Add("version.single-source", csprojs.Where(p => Regex.IsMatch(File.ReadAllText(p), @"<(Version|PackageVersion)>\s*[0-9]")).Select(p => "project declares its own version: " + p.Substring(repo.Length + 1)));
        report.Add("version.central-defined", File.ReadAllText(Path.Combine(repo, "Directory.Build.props")).Contains("<LazyCrudVersion") ? Array.Empty<string>() : new[] { "LazyCrudVersion missing from Directory.Build.props" });
        report.Add("packaging.dead-t4-glob-removed", File.ReadAllText(Path.Combine(repo, "Directory.Build.props")).Contains("contentFiles/any/any/T4/") ? new[] { "Directory.Build.props still packs **/*.tt into contentFiles/any/any/T4/" } : Array.Empty<string>());

        // Shared templates ship only from the templates package.
        var layerCsprojs = csprojs.Where(p => !p.Contains("Templates")).ToList();
        report.Add("packaging.shared-templates-single-package", layerCsprojs.Where(p => Regex.IsMatch(File.ReadAllText(p), @"ProjectFiles\\project\.tt|Tests\.Common\.ttinclude")).Select(p => "layer project packs a shared template: " + p.Substring(repo.Length + 1)));

        // Attribute metadata (from the built assembly when available).
        var asm = o.Get("attributes-assembly") ?? Path.Combine(repo, "src", "Builder", "Builder.Application.DTO", "bin", o.Get("configuration") ?? "Release", "net10.0", "Lazy.Crud.Builder.Application.DTO.dll");
        if (File.Exists(asm))
        {
            var inspections = AttributeCatalogBuilder.Inspect(asm, m => { });
            report.Add("attributes.metadata-contract", inspections.SelectMany(i => i.Problems.Select(p => i.Entry.FullName + ": " + p)), inspections.Count + " public attribute types");
        }
        else report.Add("attributes.metadata-contract", new[] { "attributes assembly not built: " + asm });

        // Credential scan (same patterns as scripts/credential-scan.ps1).
        report.Add("security.no-credential-literal", CredentialScan(repo));

        // Documentation set.
        var docs = new[] { "README.md", "docs/quickstart.md", "docs/architecture.md", "docs/model.md", "docs/generated-code-policy.md", "docs/customization.md", "docs/migrations.md", "docs/testing.md", "docs/compatibility.md", "docs/troubleshooting.md", "CHANGELOG.md", "docs/release.md", "docs/ai-agents.md", "docs/reference/descriptor.md", "docs/reference/manifest-v2.md", "docs/reference/marker-v2.md", "docs/reference/ownership.md", "docs/reference/hash.md", "docs/reference/diagnostics.md", "docs/toolchain.md", "docs/harness.md", "docs/clean-room.md" };
        report.Add("docs.required-pages", docs.Where(d => !File.Exists(Path.Combine(repo, d)) || new FileInfo(Path.Combine(repo, d)).Length < 200).Select(d => "missing or empty: " + d));
        DocsDrift(repo, report);

        // Fixtures equal the current build (when the descriptor exists).
        var descriptorPath = Path.Combine(repo, "artifacts", "platform", "lazycrud.platform.json");
        if (File.Exists(descriptorPath)) FixtureChecks.Compare(repo, PlatformDescriptor.FromJson(File.ReadAllText(descriptorPath), descriptorPath), report);
        else report.Add("fixtures.current", new[] { "descriptor not generated yet: " + descriptorPath });
    }

    private static string ExtractRegion(string text, string start, string end)
    {
        var s = text.IndexOf(start, StringComparison.Ordinal);
        var e = text.IndexOf(end, StringComparison.Ordinal);
        if (s < 0 || e < 0 || e < s) return text;
        return text.Substring(s, e - s);
    }

    private static readonly Regex[] CredentialPatterns =
    {
        new(@"oy2[a-z0-9]{43}"), new(@"gh[pousr]_[A-Za-z0-9]{36,}"), new(@"AKIA[0-9A-Z]{16}"),
        new(@"(?i)(apikey|api_key|api-key|nugetapikey|nuget_api_key)\s*[=:]\s*['""][A-Za-z0-9+/=_\-]{32,}['""]"),
        new(@"(?i)-ApiKey\s+['""]?[a-z0-9]{40,}['""]?"),
    };

    private static IEnumerable<string> CredentialScan(string repo)
    {
        var skip = new[] { @"\.git\", @"\bin\", @"\obj\", @"\artifacts\", @"\node_modules\", @"\.vs\" };
        foreach (var f in Directory.EnumerateFiles(repo, "*", SearchOption.AllDirectories))
        {
            if (skip.Any(s => f.Contains(s, StringComparison.OrdinalIgnoreCase))) continue;
            var ext = Path.GetExtension(f).ToLowerInvariant();
            if (ext is ".dll" or ".exe" or ".png" or ".jpg" or ".nupkg" or ".zip" or ".pdb" or ".ico") continue;
            string text;
            try { text = File.ReadAllText(f); } catch { continue; }
            int line = 0;
            foreach (var l in text.Split('\n'))
            {
                line++;
                foreach (var p in CredentialPatterns) if (p.IsMatch(l)) yield return f.Substring(repo.Length + 1) + ":" + line;
            }
        }
    }

    /// <summary>Documented version, layer and package facts must match the descriptor.</summary>
    private static void DocsDrift(string repo, ValidationReport report)
    {
        var problems = new List<string>();
        var descriptorPath = Path.Combine(repo, "artifacts", "platform", "lazycrud.platform.json");
        if (!File.Exists(descriptorPath)) { report.Add("docs.drift", new[] { "descriptor not generated yet" }); return; }
        var d = PlatformDescriptor.FromJson(File.ReadAllText(descriptorPath), descriptorPath);
        var compat = Path.Combine(repo, "docs", "compatibility.md");
        if (File.Exists(compat))
        {
            var text = File.ReadAllText(compat);
            if (!text.Contains(d.PlatformVersion)) problems.Add("docs/compatibility.md does not mention platform version " + d.PlatformVersion);
            if (!text.Contains("contractVersion " + d.ContractVersion) && !text.Contains("contractVersion: " + d.ContractVersion) && !text.Contains("`" + d.ContractVersion + "`")) problems.Add("docs/compatibility.md does not state contractVersion " + d.ContractVersion);
            if (!text.Contains(d.Compatibility.HostRequirements?.T4Component ?? "Microsoft.VisualStudio.Component.TextTemplating")) problems.Add("docs/compatibility.md does not name the T4 component");
            foreach (var l in d.Layers) if (!text.Contains(l.PackageId)) problems.Add("docs/compatibility.md does not list package " + l.PackageId);
        }
        var arch = Path.Combine(repo, "docs", "architecture.md");
        if (File.Exists(arch))
        {
            var text = File.ReadAllText(arch);
            foreach (var l in d.Layers) if (!text.Contains(l.Id)) problems.Add("docs/architecture.md does not describe layer " + l.Id);
        }
        var readme = Path.Combine(repo, "README.md");
        if (File.Exists(readme))
        {
            var text = File.ReadAllText(readme);
            if (Regex.IsMatch(text, @"\.NET Core 8\.0")) problems.Add("README.md still claims .NET Core 8.0");
            if (!text.Contains("net10.0")) problems.Add("README.md does not state the net10.0 target framework");
            if (!text.Contains(TemplateRegistry.CanonicalTemplatesPackageId)) problems.Add("README.md does not name the canonical templates package");
        }
        foreach (var f in Directory.GetFiles(Path.Combine(repo, "docs"), "*.md", SearchOption.AllDirectories).Concat(new[] { readme }))
        {
            if (!File.Exists(f)) continue;
            var text = File.ReadAllText(f);
            foreach (Match m in Regex.Matches(text, @"`(Lazy\.Crud\.[A-Za-z.]+)`"))
            {
                var id = m.Groups[1].Value;
                var known = d.Layers.Any(l => l.PackageId == id) || d.NonLayerPackages.Contains(id) || d.Templates.PackageId == id || d.Templates.AliasPackageIds.Contains(id)
                            || id.StartsWith("Lazy.Crud.Builder.Platform", StringComparison.Ordinal) || id.EndsWith(".Tool", StringComparison.Ordinal);
                if (!known && Regex.IsMatch(id, @"^Lazy\.Crud\.(Builder|CrossCutting|Templates)")) problems.Add(f.Substring(repo.Length + 1) + " names unknown package " + id);
            }
        }
        report.Add("docs.drift", problems.Distinct());
    }
}

/// <summary>Scans writer call sites in the shipped templates.</summary>
public static class CallSiteScanner
{
    public sealed class CallSite
    {
        public string SourceFile { get; init; } = "";
        public int Line { get; init; }
        public string? Ownership { get; init; }
        public string? TemplateId { get; init; }
        public bool HasOwnership => Ownership != null;
        public bool HasTemplateId => TemplateId != null;
        public string Location => SourceFile + ":" + Line;
    }

    private static readonly Regex CallRegex = new(@"Save(OutputToSubFolder|HostOutput|Output)\s*\((?<args>[^;]*?)\)\s*;", RegexOptions.Singleline);

    public static List<CallSite> Scan(string projectFiles)
    {
        var sites = new List<CallSite>();
        foreach (var file in Directory.GetFiles(projectFiles, "*.tt", SearchOption.AllDirectories).Concat(Directory.GetFiles(projectFiles, "*.ttinclude", SearchOption.AllDirectories)))
        {
            var rel = file.Substring(projectFiles.Length + 1).Replace('\\', '/');
            if (rel == "project.tt") continue; // the writer definitions themselves
            var text = File.ReadAllText(file);
            foreach (Match m in CallRegex.Matches(text))
            {
                var args = m.Groups["args"].Value;
                var line = text.Substring(0, m.Index).Count(c => c == '\n') + 1;
                var ownership = Regex.Match(args, @"ownership\s*:\s*Ownership\.(\w+)");
                var templateId = Regex.Match(args, @"templateId\s*:\s*""([^""]+)""");
                sites.Add(new CallSite { SourceFile = rel, Line = line, Ownership = ownership.Success ? ownership.Groups[1].Value : null, TemplateId = templateId.Success ? templateId.Groups[1].Value : null });
            }
        }
        return sites;
    }
}

public static class PackageChecks
{
    public static void Run(string repo, string packagesDir, ValidationReport report)
    {
        var descriptorPath = Path.Combine(repo, "artifacts", "platform", "lazycrud.platform.json");
        if (!File.Exists(descriptorPath)) { report.Add("packages.descriptor-available", new[] { "descriptor not found: " + descriptorPath }); return; }
        var d = PlatformDescriptor.FromJson(File.ReadAllText(descriptorPath), descriptorPath);
        var nupkgs = Directory.GetFiles(packagesDir, "*.nupkg").Where(p => !p.EndsWith(".symbols.nupkg")).ToList();
        var byId = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var entries = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        var versions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var p in nupkgs)
        {
            using var zip = ZipFile.OpenRead(p);
            var nuspec = zip.Entries.First(e => e.FullName.EndsWith(".nuspec", StringComparison.OrdinalIgnoreCase));
            using var reader = new StreamReader(nuspec.Open());
            var xml = System.Xml.Linq.XDocument.Parse(reader.ReadToEnd());
            var ns = xml.Root!.Name.Namespace;
            var meta = xml.Root.Element(ns + "metadata")!;
            var id = meta.Element(ns + "id")!.Value;
            byId[id] = p;
            versions[id] = meta.Element(ns + "version")!.Value;
            entries[id] = zip.Entries.Select(e => e.FullName.Replace('\\', '/')).ToHashSet(StringComparer.OrdinalIgnoreCase);
        }

        // Every declared package is produced; every produced package is declared.
        var declared = d.Layers.Select(l => l.PackageId).Concat(d.Layers.Where(l => l.TemplatePackageId != null).Select(l => l.TemplatePackageId!)).Concat(d.NonLayerPackages).Concat(new[] { d.Templates.PackageId }).Concat(d.Templates.AliasPackageIds).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        report.Add("packages.declared-produced", declared.Where(id => !byId.ContainsKey(id)).Select(id => "declared but not produced: " + id), byId.Count + " packages produced");
        report.Add("packages.produced-declared", byId.Keys.Where(id => !declared.Contains(id, StringComparer.OrdinalIgnoreCase)).Select(id => "produced but not declared in layers[] or nonLayerPackages[]: " + id));
        report.Add("packages.single-version", versions.Where(kv => kv.Value != d.PlatformVersion).Select(kv => kv.Key + " has version " + kv.Value + " but the descriptor declares " + d.PlatformVersion));

        // Descriptor embedded in every package at the documented path, with the same platform version.
        var embedProblems = new List<string>();
        foreach (var (id, path) in byId)
        {
            if (!entries[id].Contains(PlatformDescriptor.InPackagePath)) { embedProblems.Add(id + " lacks " + PlatformDescriptor.InPackagePath); continue; }
            using var zip = ZipFile.OpenRead(path);
            using var reader = new StreamReader(zip.GetEntry(PlatformDescriptor.InPackagePath)!.Open());
            var embedded = PlatformDescriptor.FromJson(reader.ReadToEnd(), id);
            if (embedded.PlatformVersion != d.PlatformVersion) embedProblems.Add(id + " embeds platformVersion " + embedded.PlatformVersion);
            if (embedded.ContractVersion != d.ContractVersion) embedProblems.Add(id + " embeds contractVersion " + embedded.ContractVersion);
        }
        report.Add("packages.descriptor-embedded", embedProblems);

        // Registry entries resolve to a real file in the named package; layer content paths exist.
        report.Add("packages.registry-resolves", d.Templates.Registry.Where(r => !entries.TryGetValue(r.PackageId, out var set) || !set.Contains(r.InPackagePath)).Select(r => r.Id + " -> " + r.PackageId + "/" + r.InPackagePath + " not found"), d.Templates.Registry.Count + " registry entries");
        report.Add("packages.layer-content-paths", d.Layers.Where(l => l.TemplatePackageId != null).Where(l => !entries.TryGetValue(l.TemplatePackageId!, out var set) || !set.Any(e => e.StartsWith(l.TemplateContentPath!, StringComparison.OrdinalIgnoreCase))).Select(l => l.Id + ": " + l.TemplatePackageId + "/" + l.TemplateContentPath + " has no files"));
        report.Add("packages.shared-templates-in-canonical", d.Templates.SharedFiles.Where(f => !entries.TryGetValue(d.Templates.PackageId, out var set) || !set.Contains(d.Templates.ContentRoot + f)).Select(f => f + " missing from " + d.Templates.PackageId));

        // No duplicate contentFiles paths across packages; no template content outside declared paths.
        var contentOwners = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var (id, set) in entries)
            foreach (var e in set.Where(e => e.StartsWith("contentFiles/", StringComparison.OrdinalIgnoreCase)))
            {
                if (!contentOwners.TryGetValue(e, out var owners)) contentOwners[e] = owners = new List<string>();
                owners.Add(id);
            }
        report.Add("packages.no-contentfiles-collision", contentOwners.Where(kv => kv.Value.Count > 1).Select(kv => kv.Key + " shipped by " + string.Join(", ", kv.Value)));
        var declaredPaths = d.Templates.Registry.Select(r => r.InPackagePath).ToHashSet(StringComparer.OrdinalIgnoreCase);
        report.Add("packages.no-undeclared-template-content", contentOwners.Keys.Where(e => (e.EndsWith(".tt") || e.EndsWith(".ttinclude")) && !declaredPaths.Contains(e)).Select(e => "template content not declared by the registry: " + e));

        // Templates package: MSBuild files named after the package id, tools present. Alias: no content, depends on canonical.
        if (entries.TryGetValue(d.Templates.PackageId, out var tset))
        {
            var t = new List<string>();
            if (!tset.Contains("build/" + d.Templates.PackageId + ".props")) t.Add("build/" + d.Templates.PackageId + ".props missing");
            if (!tset.Contains("build/" + d.Templates.PackageId + ".targets")) t.Add("build/" + d.Templates.PackageId + ".targets missing");
            if (!tset.Contains("tools/net10.0/Lazy.Crud.Builder.Platform.dll")) t.Add("tools/net10.0/Lazy.Crud.Builder.Platform.dll missing");
            if (!tset.Contains("tools/net10.0/Lazy.Crud.Builder.Platform.Tool.dll")) t.Add("tools/net10.0/Lazy.Crud.Builder.Platform.Tool.dll missing");
            if (!tset.Any(e => e.StartsWith("contracts/fixtures/v" + d.ContractVersion + "/", StringComparison.OrdinalIgnoreCase))) t.Add("contract fixtures missing from the templates package");
            report.Add("packages.templates-package-layout", t);
        }
        foreach (var alias in d.Templates.AliasPackageIds)
        {
            if (!entries.TryGetValue(alias, out var aset)) continue;
            var problems = aset.Where(e => e.StartsWith("contentFiles/", StringComparison.OrdinalIgnoreCase) || e.StartsWith("lib/", StringComparison.OrdinalIgnoreCase)).Select(e => alias + " carries content: " + e).ToList();
            using var zip = ZipFile.OpenRead(byId[alias]);
            using var reader = new StreamReader(zip.Entries.First(e => e.FullName.EndsWith(".nuspec")).Open());
            if (!reader.ReadToEnd().Contains("id=\"" + d.Templates.PackageId + "\"")) problems.Add(alias + " does not depend on " + d.Templates.PackageId);
            report.Add("packages.alias-is-dependency-only", problems);
        }

        // XML documentation ships with the attributes package.
        report.Add("packages.attributes-xml-documentation", entries.TryGetValue(AttributeCatalogBuilder.AttributesPackageId, out var aset2) && aset2.Contains("lib/net10.0/Lazy.Crud.Builder.Application.DTO.xml") ? Array.Empty<string>() : new[] { "lib/net10.0/Lazy.Crud.Builder.Application.DTO.xml missing" });

        // Layer packages depend on the templates package without excluding build/contentfiles assets.
        var depProblems = new List<string>();
        foreach (var l in d.Layers.Where(l => l.TemplatePackageId != null))
        {
            using var zip = ZipFile.OpenRead(byId[l.PackageId]);
            using var reader = new StreamReader(zip.Entries.First(e => e.FullName.EndsWith(".nuspec")).Open());
            var nuspec = reader.ReadToEnd();
            var m = Regex.Match(nuspec, "<dependency id=\"" + Regex.Escape(d.Templates.PackageId) + "\"[^>]*>");
            if (!m.Success) depProblems.Add(l.PackageId + " does not depend on " + d.Templates.PackageId);
            else if (m.Value.Contains("exclude=") && (m.Value.Contains("Build") || m.Value.Contains("ContentFiles"))) depProblems.Add(l.PackageId + " excludes build/contentfiles assets of " + d.Templates.PackageId + ": " + m.Value);
        }
        report.Add("packages.layers-depend-on-templates", depProblems);
    }
}

public static class FixtureChecks
{
    public static void Run(string repo, string? attributesAssembly, ValidationReport report)
    {
        var descriptorPath = Path.Combine(repo, "artifacts", "platform", "lazycrud.platform.json");
        PlatformDescriptor d;
        if (attributesAssembly != null && File.Exists(attributesAssembly))
        {
            var inspections = AttributeCatalogBuilder.Inspect(attributesAssembly, m => { });
            d = DescriptorBuilder.Build(PlatformInfo.Version, inspections.Where(i => i.DerivesFromAttribute).Select(i => i.Entry).ToList());
        }
        else if (File.Exists(descriptorPath)) d = PlatformDescriptor.FromJson(File.ReadAllText(descriptorPath), descriptorPath);
        else { report.Add("fixtures.descriptor-available", new[] { "neither --attributes-assembly nor " + descriptorPath + " is available" }); return; }
        Compare(repo, d, report);
    }

    public static void Compare(string repo, PlatformDescriptor d, ValidationReport report)
    {
        var contracts = Path.Combine(repo, "contracts");
        var dir = FixtureSet.VersionDirectory(contracts, d.ContractVersion);
        var produced = FixtureSet.Produce(d);
        report.Add("fixtures.present", produced.Keys.Where(k => !File.Exists(Path.Combine(dir, k.Replace('/', Path.DirectorySeparatorChar)))).Select(k => "missing fixture: " + k), produced.Count + " fixture files expected under " + dir);
        report.Add("fixtures.current", FixtureSet.Diff(dir, produced));

        var schemaDir = Path.Combine(contracts, "schemas", "v" + d.ContractVersion);
        var descriptorSchema = Path.Combine(schemaDir, "lazycrud.platform.schema.json");
        var manifestSchema = Path.Combine(schemaDir, "manifest-v2.schema.json");
        if (!File.Exists(descriptorSchema) || !File.Exists(manifestSchema)) { report.Add("fixtures.schemas-present", new[] { "schemas missing under " + schemaDir }); return; }
        var ds = new JsonSchemaLite(File.ReadAllText(descriptorSchema));
        var ms = new JsonSchemaLite(File.ReadAllText(manifestSchema));
        report.Add("fixtures.descriptor-validates", ds.Validate(produced["lazycrud.platform.json"]));
        report.Add("fixtures.manifest-v2-validates", ms.Validate(produced["manifest-v2.json"]));
        report.Add("fixtures.manifest-v1-migrated-validates", ms.Validate(produced["manifest-v1-migrated.json"]));

        // The Builder verifies its own hash vectors with the production implementation.
        var vectors = Platform.Json.JsonLite.ParseObject(produced["hash-vectors.json"]).GetArray("vectors")!.Cast<Platform.Json.JsonObject>();
        report.Add("fixtures.hash-vectors-reproducible", vectors.Where(v => Platform.Hashing.ContentHasher.Compute(v.GetString("input")!).Value != v.GetString("expectedHash")).Select(v => "vector " + v.GetString("id") + " does not reproduce"));
        var committedVectors = Path.Combine(dir, "hash-vectors.json");
        if (File.Exists(committedVectors))
        {
            var committed = Platform.Json.JsonLite.ParseObject(File.ReadAllText(committedVectors)).GetArray("vectors")!.Cast<Platform.Json.JsonObject>();
            report.Add("fixtures.committed-hash-vectors-reproducible", committed.Where(v => Platform.Hashing.ContentHasher.Compute(v.GetString("input")!).Value != v.GetString("expectedHash")).Select(v => "committed vector " + v.GetString("id") + " does not reproduce"));
        }
        // Marker fixtures round-trip through the production parser.
        report.Add("fixtures.markers-parse", produced.Where(kv => kv.Key.StartsWith("markers/")).Where(kv => !Platform.Markers.GeneratedCodeMarker.TryParse(kv.Value + "body", out var info) || !info!.IsV2 || info.Problems.Count > 0).Select(kv => kv.Key + " does not parse as marker v2"));
    }
}

public static class ManifestChecks
{
    public static void Run(string repo, string file, ValidationReport report)
    {
        var schema = Path.Combine(repo, "contracts", "schemas", "v" + PlatformInfo.ContractVersion, "manifest-v2.schema.json");
        var validator = new JsonSchemaLite(File.ReadAllText(schema));
        var text = File.ReadAllText(file);
        report.Add("manifest.schema", validator.Validate(text), file);
        report.Add("manifest.no-absolute-paths", Regex.IsMatch(text, @"[A-Za-z]:\\\\|\\\\\\\\") ? new[] { "absolute or backslash path found" } : Array.Empty<string>());
    }
}
