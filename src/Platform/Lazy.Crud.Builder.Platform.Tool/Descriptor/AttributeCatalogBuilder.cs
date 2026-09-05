using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Xml.Linq;
using Lazy.Crud.Builder.Platform.Descriptor;

namespace Lazy.Crud.Builder.Platform.Tool.Descriptor;

/// <summary>Prose read from the XML documentation file, keyed by documentation member id.</summary>
public sealed class XmlDocumentation
{
    public Dictionary<string, XElement> Members { get; } = new(StringComparer.Ordinal);
    public string Path { get; init; } = "";

    public static XmlDocumentation Load(string path)
    {
        var doc = new XmlDocumentation { Path = path };
        if (!File.Exists(path)) return doc;
        var x = XDocument.Load(path);
        foreach (var m in x.Descendants("member"))
        {
            var name = m.Attribute("name")?.Value;
            if (!string.IsNullOrEmpty(name)) doc.Members[name!] = m;
        }
        return doc;
    }

    public string? Element(string memberId, string element)
    {
        return Members.TryGetValue(memberId, out var m) ? m.Element(element)?.Value.Trim() : null;
    }

    public IReadOnlyList<string> ParamNames(string memberId)
    {
        return Members.TryGetValue(memberId, out var m) ? m.Elements("param").Select(p => p.Attribute("name")?.Value ?? "").ToList() : new List<string>();
    }
}

/// <summary>A catalog entry plus the facts the metadata check needs.</summary>
public sealed class AttributeInspection
{
    public required AttributeDescriptor Entry { get; init; }
    public bool DerivesFromAttribute { get; init; }
    public string? Summary { get; init; }
    public string? Remarks { get; init; }
    public List<string> Problems { get; } = new();
    public List<(string ctorDocId, IReadOnlyList<string> parameters, IReadOnlyList<string> documented)> Constructors { get; } = new();
}

/// <summary>
/// Builds the attribute catalog (identity and structure) by reflecting over the shipped
/// Application.DTO assembly with a MetadataLoadContext, joining prose ids to the XML documentation.
/// </summary>
public static class AttributeCatalogBuilder
{
    /// <summary>The controlled category vocabulary (task 13.1), derived from the categories in use.</summary>
    public static readonly IReadOnlyList<string> CategoryVocabulary = new[]
    {
        "Code Generation (T4)",
        "Modeling",
        "Serialization/CSV",
        "UI/Display",
        "UI/Forms",
        "UI/Input",
        "UI/Layout",
        "UI/List",
        "UI/Menu",
        "UI/Navigation",
        "UI/Selectors",
        "UI/Titles",
        "Validation",
    };

    public const string AttributesPackageId = "Lazy.Crud.Builder.Application.DTO";

    public static List<AttributeInspection> Inspect(string assemblyPath, Action<string> log)
    {
        var xmlPath = Path.ChangeExtension(assemblyPath, ".xml");
        var docs = XmlDocumentation.Load(xmlPath);
        if (docs.Members.Count == 0) log("warning: XML documentation not found or empty at " + xmlPath);

        var paths = ResolverPaths(assemblyPath, log);
        var resolver = new PathAssemblyResolver(paths);
        using var mlc = new MetadataLoadContext(resolver, "System.Private.CoreLib");
        var asm = mlc.LoadFromAssemblyPath(assemblyPath);
        var attributeBase = mlc.CoreAssembly!.GetType("System.Attribute")!;

        var results = new List<AttributeInspection>();
        Type[] types;
        try { types = asm.GetTypes(); }
        catch (ReflectionTypeLoadException ex) { types = ex.Types.Where(t => t != null).ToArray()!; }

        foreach (var t in types.Where(t => t.IsPublic && t.IsClass && !t.IsNested && !t.IsAbstract && IsAttributeArea(t)))
        {
            var derives = DerivesFrom(t, attributeBase);
            var docId = "T:" + t.FullName;
            var category = docs.Element(docId, "category");
            var entry = new AttributeDescriptor
            {
                Name = t.Name,
                FullName = t.FullName!,
                Namespace = t.Namespace ?? "",
                PackageId = AttributesPackageId,
                Category = category,
                Applicability = Applicability(t),
                DocumentationId = docId,
            };
            var inspection = new AttributeInspection
            {
                Entry = entry,
                DerivesFromAttribute = derives,
                Summary = docs.Element(docId, "summary"),
                Remarks = docs.Element(docId, "remarks"),
            };
            foreach (var ctor in t.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
            {
                var overload = new AttributeOverloadDescriptor();
                var names = new List<string>();
                foreach (var p in ctor.GetParameters())
                {
                    var isParams = p.GetCustomAttributesData().Any(a => a.AttributeType.FullName == "System.ParamArrayAttribute");
                    overload.Parameters.Add(new AttributeParameterDescriptor { Name = p.Name ?? "", Type = FriendlyTypeName(p.ParameterType), Required = !p.HasDefaultValue && !isParams });
                    names.Add(p.Name ?? "");
                }
                entry.Overloads.Add(overload);
                var ctorDocId = "M:" + t.FullName + ".#ctor" + (names.Count == 0 ? "" : "(" + string.Join(",", ctor.GetParameters().Select(p => DocTypeName(p.ParameterType))) + ")");
                inspection.Constructors.Add((ctorDocId, names, docs.ParamNames(ctorDocId)));
            }

            // Metadata contract checks (task 13.10).
            if (!derives) inspection.Problems.Add("does not derive from System.Attribute");
            if (string.IsNullOrWhiteSpace(inspection.Summary)) inspection.Problems.Add("missing <summary>");
            if (string.IsNullOrWhiteSpace(category)) inspection.Problems.Add("missing <category>");
            else if (!CategoryVocabulary.Contains(category, StringComparer.Ordinal)) inspection.Problems.Add("category '" + category + "' is not in the controlled vocabulary");
            foreach (var (ctorDocId, parameters, documented) in inspection.Constructors)
            {
                foreach (var p in parameters)
                    if (!documented.Contains(p, StringComparer.Ordinal)) inspection.Problems.Add("constructor parameter '" + p + "' has no <param> (" + ctorDocId + ")");
            }
            results.Add(inspection);
        }
        return results.OrderBy(r => r.Entry.FullName, StringComparer.Ordinal).ToList();
    }

    private static bool IsAttributeArea(Type t)
    {
        var ns = t.Namespace ?? "";
        return ns.Contains(".Attributes", StringComparison.Ordinal);
    }

    private static bool DerivesFrom(Type t, Type baseType)
    {
        for (var cur = t.BaseType; cur != null; cur = cur.BaseType)
            if (cur.FullName == baseType.FullName) return true;
        return false;
    }

    private static List<string> Applicability(Type t)
    {
        var usage = t.GetCustomAttributesData().FirstOrDefault(a => a.AttributeType.FullName == "System.AttributeUsageAttribute");
        if (usage == null || usage.ConstructorArguments.Count == 0) return new List<string> { "All" };
        var value = Convert.ToInt32(usage.ConstructorArguments[0].Value);
        var names = new List<string>();
        foreach (var target in Enum.GetValues<AttributeTargets>())
        {
            if (target == AttributeTargets.All) continue;
            if ((value & (int)target) == (int)target) names.Add(target.ToString());
        }
        if (value == (int)AttributeTargets.All) return new List<string> { "All" };
        return names;
    }

    private static string FriendlyTypeName(Type t)
    {
        if (t.IsArray) return FriendlyTypeName(t.GetElementType()!) + "[]";
        switch (t.FullName)
        {
            case "System.String": return "string";
            case "System.Int32": return "int";
            case "System.Boolean": return "bool";
            case "System.Double": return "double";
            case "System.Decimal": return "decimal";
            case "System.Object": return "object";
            case "System.Int64": return "long";
        }
        return t.FullName ?? t.Name;
    }

    private static string DocTypeName(Type t)
    {
        if (t.IsArray) return DocTypeName(t.GetElementType()!) + "[]";
        return (t.FullName ?? t.Name).Replace('+', '.');
    }

    /// <summary>
    /// Assemblies the MetadataLoadContext may need: the runtime pack, the inspected assembly's
    /// folder, and the compile-time references recorded in the project's obj/project.assets.json.
    /// </summary>
    private static List<string> ResolverPaths(string assemblyPath, Action<string> log)
    {
        var paths = new List<string>();
        var runtimeDir = RuntimeEnvironment.GetRuntimeDirectory();
        paths.AddRange(Directory.GetFiles(runtimeDir, "*.dll"));
        var asmDir = Path.GetDirectoryName(assemblyPath)!;
        paths.AddRange(Directory.GetFiles(asmDir, "*.dll"));

        // bin/<Configuration>/<tfm>/X.dll -> <project>/obj/project.assets.json
        var projectDir = Path.GetFullPath(Path.Combine(asmDir, "..", "..", ".."));
        var assets = Path.Combine(projectDir, "obj", "project.assets.json");
        if (File.Exists(assets))
        {
            try
            {
                var globalPackages = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
                if (string.IsNullOrEmpty(globalPackages)) globalPackages = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages");
                using var doc = JsonDocument.Parse(File.ReadAllText(assets));
                var libraries = doc.RootElement.GetProperty("libraries");
                foreach (var target in doc.RootElement.GetProperty("targets").EnumerateObject())
                {
                    foreach (var lib in target.Value.EnumerateObject())
                    {
                        var idVersion = lib.Name; // "Id/Version"
                        var slash = idVersion.IndexOf('/');
                        if (slash < 0) continue;
                        var id = idVersion.Substring(0, slash);
                        var version = idVersion.Substring(slash + 1);
                        var type = lib.Value.TryGetProperty("type", out var t) ? t.GetString() : "package";
                        if (!lib.Value.TryGetProperty("compile", out var compile)) continue;
                        foreach (var file in compile.EnumerateObject())
                        {
                            if (file.Name.EndsWith("_._", StringComparison.Ordinal)) continue;
                            string candidate;
                            if (type == "project")
                            {
                                if (!libraries.TryGetProperty(idVersion, out var libInfo) || !libInfo.TryGetProperty("path", out var projPath)) continue;
                                var refProject = Path.GetFullPath(Path.Combine(projectDir, projPath.GetString()!));
                                var refDir = Path.GetDirectoryName(refProject)!;
                                var configuration = new DirectoryInfo(Path.GetFullPath(Path.Combine(asmDir, ".."))).Name;
                                var tfm = new DirectoryInfo(asmDir).Name;
                                candidate = Path.Combine(refDir, "bin", configuration, tfm, Path.GetFileName(file.Name));
                            }
                            else
                            {
                                candidate = Path.Combine(globalPackages, id.ToLowerInvariant(), version, file.Name.Replace('/', Path.DirectorySeparatorChar));
                            }
                            if (File.Exists(candidate)) paths.Add(candidate);
                        }
                    }
                    break; // first target is enough
                }
            }
            catch (Exception ex)
            {
                log("warning: could not read " + assets + ": " + ex.Message);
            }
        }
        return paths.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }
}
