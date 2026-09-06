using System;
using System.Collections.Generic;
using System.Linq;
using Lazy.Crud.Builder.Platform.Diagnostics;
using Lazy.Crud.Builder.Platform.Json;

namespace Lazy.Crud.Builder.Platform.Descriptor
{
    /// <summary>One supported aggregate layer.</summary>
    public sealed class LayerDescriptor
    {
        public string Id { get; set; }
        public string ProjectSuffix { get; set; }
        public string ProjectPattern { get; set; }
        public string PackageId { get; set; }
        public string TemplatePackageId { get; set; }
        public string TemplateContentPath { get; set; }
        public string OutputPathPattern { get; set; }
    }

    /// <summary>One test project layer (additive section).</summary>
    public sealed class TestLayerDescriptor
    {
        public string Id { get; set; }
        public string TargetLayer { get; set; }
        public string ProjectPattern { get; set; }
        public string TemplatePackageId { get; set; }
        public string TemplateContentPath { get; set; }
        public string OutputPathPattern { get; set; }
    }

    /// <summary>A registry entry as published in the descriptor.</summary>
    public sealed class RegistryEntryDescriptor
    {
        public string Id { get; set; }
        public string Layer { get; set; }
        public string SourceFile { get; set; }
        public string OutputPattern { get; set; }
        public string Ownership { get; set; }
        public string TemplateVersion { get; set; }
        public string PackageId { get; set; }
        public string InPackagePath { get; set; }
        public string Kind { get; set; }
        /// <summary>
        /// True when this template's correct output carries no significant content, so a contentless
        /// body is not evidence of a collapsed transformation. Absent means false. See LC2012.
        /// </summary>
        public bool EmptyOutputAllowed { get; set; }
    }

    public sealed class TemplatesDescriptor
    {
        public string PackageId { get; set; }
        public List<string> AliasPackageIds { get; set; } = new List<string>();
        public string Version { get; set; }
        public string ContentRoot { get; set; }
        public List<string> SharedFiles { get; set; } = new List<string>();
        public List<string> OutputPatternTokens { get; set; } = new List<string>();
        public List<RegistryEntryDescriptor> Registry { get; set; } = new List<RegistryEntryDescriptor>();
    }

    public sealed class AttributeParameterDescriptor
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }
    }

    public sealed class AttributeOverloadDescriptor
    {
        public List<AttributeParameterDescriptor> Parameters { get; set; } = new List<AttributeParameterDescriptor>();
    }

    public sealed class AttributeDescriptor
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Namespace { get; set; }
        public string PackageId { get; set; }
        public string Category { get; set; }
        public List<string> Applicability { get; set; } = new List<string>();
        public List<AttributeOverloadDescriptor> Overloads { get; set; } = new List<AttributeOverloadDescriptor>();
        public string DocumentationId { get; set; }
    }

    public sealed class HostRequirementsDescriptor
    {
        public string Platform { get; set; } = "windows";
        public List<string> VisualStudioVersions { get; set; } = new List<string>();
        public List<string> VisualStudioChannels { get; set; } = new List<string>();
        public string T4Component { get; set; }
        public string T4ComponentDisplayName { get; set; }
        public string DotnetSdk { get; set; }
        public string HeadlessDefinition { get; set; }
    }

    public sealed class CompatibilityDescriptor
    {
        public int SupportedContractVersionMin { get; set; }
        public int SupportedContractVersionMax { get; set; }
        public int MinimumUpgradableContractVersion { get; set; }
        public int MinimumManifestSchemaVersion { get; set; }
        public int ManifestSchemaVersion { get; set; }
        public List<string> MixedVersionRanges { get; set; } = new List<string>();
        public HostRequirementsDescriptor HostRequirements { get; set; }
    }

    /// <summary>The canonical platform descriptor, lazycrud.platform.json (contract F1).</summary>
    public sealed class PlatformDescriptor
    {
        public const string FileName = "lazycrud.platform.json";
        /// <summary>Fixed, documented path inside every package.</summary>
        public const string InPackagePath = "lazycrud/lazycrud.platform.json";

        public int SchemaVersion { get; set; }
        public int ContractVersion { get; set; }
        public string PlatformVersion { get; set; }
        public string GeneratedAtUtc { get; set; }
        public List<LayerDescriptor> Layers { get; set; } = new List<LayerDescriptor>();
        public List<TestLayerDescriptor> TestLayers { get; set; } = new List<TestLayerDescriptor>();
        public TemplatesDescriptor Templates { get; set; } = new TemplatesDescriptor();
        public List<AttributeDescriptor> Attributes { get; set; } = new List<AttributeDescriptor>();
        public List<string> AttributeCategories { get; set; } = new List<string>();
        public List<string> Capabilities { get; set; } = new List<string>();
        public List<string> NonLayerPackages { get; set; } = new List<string>();
        public CompatibilityDescriptor Compatibility { get; set; } = new CompatibilityDescriptor();
        /// <summary>Where this document was loaded from (not serialized).</summary>
        public string Source { get; set; }

        public Version ParsedPlatformVersion { get { return PlatformInfo.ParseVersion(PlatformVersion); } }

        public LayerDescriptor FindLayer(string id)
        {
            return Layers.FirstOrDefault(l => string.Equals(l.Id, id, StringComparison.OrdinalIgnoreCase));
        }

        public RegistryEntryDescriptor FindTemplate(string templateId)
        {
            return Templates.Registry.FirstOrDefault(r => string.Equals(r.Id, templateId, StringComparison.Ordinal));
        }

        // ------------------------------------------------------------------ JSON ---------------

        public string ToJson()
        {
            var o = new JsonObject();
            o["schemaVersion"] = (long)SchemaVersion;
            o["contractVersion"] = (long)ContractVersion;
            o["platformVersion"] = PlatformVersion;
            if (!string.IsNullOrEmpty(GeneratedAtUtc)) o["generatedAtUtc"] = GeneratedAtUtc;
            o["layers"] = Layers.Select(l =>
            {
                var j = new JsonObject();
                j["id"] = l.Id; j["projectSuffix"] = l.ProjectSuffix; j["projectPattern"] = l.ProjectPattern;
                j["packageId"] = l.PackageId; j["templatePackageId"] = l.TemplatePackageId;
                j["templateContentPath"] = l.TemplateContentPath; j["outputPathPattern"] = l.OutputPathPattern;
                return (object)j;
            }).ToList();
            o["testLayers"] = TestLayers.Select(l =>
            {
                var j = new JsonObject();
                j["id"] = l.Id; j["targetLayer"] = l.TargetLayer; j["projectPattern"] = l.ProjectPattern;
                j["templatePackageId"] = l.TemplatePackageId; j["templateContentPath"] = l.TemplateContentPath;
                j["outputPathPattern"] = l.OutputPathPattern;
                return (object)j;
            }).ToList();
            var t = new JsonObject();
            t["packageId"] = Templates.PackageId;
            t["aliasPackageIds"] = Templates.AliasPackageIds.Cast<object>().ToList();
            t["version"] = Templates.Version;
            t["contentRoot"] = Templates.ContentRoot;
            t["sharedFiles"] = Templates.SharedFiles.Cast<object>().ToList();
            t["outputPatternTokens"] = Templates.OutputPatternTokens.Cast<object>().ToList();
            t["registry"] = Templates.Registry.Select(r =>
            {
                var j = new JsonObject();
                j["id"] = r.Id; j["layer"] = r.Layer; j["sourceFile"] = r.SourceFile; j["outputPattern"] = r.OutputPattern;
                j["ownership"] = r.Ownership; j["templateVersion"] = r.TemplateVersion; j["packageId"] = r.PackageId;
                j["inPackagePath"] = r.InPackagePath; j["kind"] = r.Kind;
                j["emptyOutputAllowed"] = r.EmptyOutputAllowed;
                return (object)j;
            }).ToList();
            o["templates"] = t;
            o["attributeCategories"] = AttributeCategories.Cast<object>().ToList();
            o["attributes"] = Attributes.Select(a =>
            {
                var j = new JsonObject();
                j["name"] = a.Name; j["fullName"] = a.FullName; j["namespace"] = a.Namespace; j["packageId"] = a.PackageId;
                j["category"] = a.Category; j["applicability"] = a.Applicability.Cast<object>().ToList();
                j["overloads"] = a.Overloads.Select(ov =>
                {
                    var jo = new JsonObject();
                    jo["parameters"] = ov.Parameters.Select(p =>
                    {
                        var jp = new JsonObject();
                        jp["name"] = p.Name; jp["type"] = p.Type; jp["required"] = p.Required;
                        return (object)jp;
                    }).ToList();
                    return (object)jo;
                }).ToList();
                j["documentationId"] = a.DocumentationId;
                return (object)j;
            }).ToList();
            o["capabilities"] = Capabilities.Cast<object>().ToList();
            o["nonLayerPackages"] = NonLayerPackages.Cast<object>().ToList();
            var c = new JsonObject();
            c["supportedContractVersionMin"] = (long)Compatibility.SupportedContractVersionMin;
            c["supportedContractVersionMax"] = (long)Compatibility.SupportedContractVersionMax;
            c["minimumUpgradableContractVersion"] = (long)Compatibility.MinimumUpgradableContractVersion;
            c["minimumManifestSchemaVersion"] = (long)Compatibility.MinimumManifestSchemaVersion;
            c["manifestSchemaVersion"] = (long)Compatibility.ManifestSchemaVersion;
            c["mixedVersionRanges"] = Compatibility.MixedVersionRanges.Cast<object>().ToList();
            if (Compatibility.HostRequirements != null)
            {
                var h = new JsonObject();
                h["platform"] = Compatibility.HostRequirements.Platform;
                h["visualStudioVersions"] = Compatibility.HostRequirements.VisualStudioVersions.Cast<object>().ToList();
                h["visualStudioChannels"] = Compatibility.HostRequirements.VisualStudioChannels.Cast<object>().ToList();
                h["t4Component"] = Compatibility.HostRequirements.T4Component;
                h["t4ComponentDisplayName"] = Compatibility.HostRequirements.T4ComponentDisplayName;
                h["dotnetSdk"] = Compatibility.HostRequirements.DotnetSdk;
                h["headlessDefinition"] = Compatibility.HostRequirements.HeadlessDefinition;
                c["hostRequirements"] = h;
            }
            o["compatibility"] = c;
            return JsonLite.Write(o, true);
        }

        public static PlatformDescriptor FromJson(string json, string source = null)
        {
            var o = JsonLite.ParseObject(json);
            var d = new PlatformDescriptor { Source = source };
            d.SchemaVersion = (int)(o.GetInteger("schemaVersion") ?? 0);
            d.ContractVersion = (int)(o.GetInteger("contractVersion") ?? 0);
            d.PlatformVersion = o.GetString("platformVersion");
            d.GeneratedAtUtc = o.GetString("generatedAtUtc");
            if (d.SchemaVersion <= 0) throw new FormatException("descriptor has no schemaVersion");
            if (d.ContractVersion <= 0) throw new FormatException("descriptor has no contractVersion");
            if (string.IsNullOrEmpty(d.PlatformVersion)) throw new FormatException("descriptor has no platformVersion");

            foreach (JsonObject l in (o.GetArray("layers") ?? new List<object>()).OfType<JsonObject>())
            {
                d.Layers.Add(new LayerDescriptor
                {
                    Id = l.GetString("id"), ProjectSuffix = l.GetString("projectSuffix"), ProjectPattern = l.GetString("projectPattern"),
                    PackageId = l.GetString("packageId"), TemplatePackageId = l.GetString("templatePackageId"),
                    TemplateContentPath = l.GetString("templateContentPath"), OutputPathPattern = l.GetString("outputPathPattern"),
                });
            }
            foreach (JsonObject l in (o.GetArray("testLayers") ?? new List<object>()).OfType<JsonObject>())
            {
                d.TestLayers.Add(new TestLayerDescriptor
                {
                    Id = l.GetString("id"), TargetLayer = l.GetString("targetLayer"), ProjectPattern = l.GetString("projectPattern"),
                    TemplatePackageId = l.GetString("templatePackageId"), TemplateContentPath = l.GetString("templateContentPath"),
                    OutputPathPattern = l.GetString("outputPathPattern"),
                });
            }
            var t = o.GetObject("templates");
            if (t != null)
            {
                d.Templates.PackageId = t.GetString("packageId");
                d.Templates.AliasPackageIds = t.GetStringArray("aliasPackageIds") ?? new List<string>();
                d.Templates.Version = t.GetString("version");
                d.Templates.ContentRoot = t.GetString("contentRoot");
                d.Templates.SharedFiles = t.GetStringArray("sharedFiles") ?? new List<string>();
                d.Templates.OutputPatternTokens = t.GetStringArray("outputPatternTokens") ?? new List<string>();
                foreach (JsonObject r in (t.GetArray("registry") ?? new List<object>()).OfType<JsonObject>())
                {
                    d.Templates.Registry.Add(new RegistryEntryDescriptor
                    {
                        Id = r.GetString("id"), Layer = r.GetString("layer"), SourceFile = r.GetString("sourceFile"),
                        OutputPattern = r.GetString("outputPattern"), Ownership = r.GetString("ownership"),
                        TemplateVersion = r.GetString("templateVersion"), PackageId = r.GetString("packageId"),
                        InPackagePath = r.GetString("inPackagePath"), Kind = r.GetString("kind"),
                        EmptyOutputAllowed = r.GetBoolean("emptyOutputAllowed") ?? false,
                    });
                }
            }
            d.AttributeCategories = o.GetStringArray("attributeCategories") ?? new List<string>();
            foreach (JsonObject a in (o.GetArray("attributes") ?? new List<object>()).OfType<JsonObject>())
            {
                var attr = new AttributeDescriptor
                {
                    Name = a.GetString("name"), FullName = a.GetString("fullName"), Namespace = a.GetString("namespace"),
                    PackageId = a.GetString("packageId"), Category = a.GetString("category"),
                    Applicability = a.GetStringArray("applicability") ?? new List<string>(), DocumentationId = a.GetString("documentationId"),
                };
                foreach (JsonObject ov in (a.GetArray("overloads") ?? new List<object>()).OfType<JsonObject>())
                {
                    var overload = new AttributeOverloadDescriptor();
                    foreach (JsonObject p in (ov.GetArray("parameters") ?? new List<object>()).OfType<JsonObject>())
                        overload.Parameters.Add(new AttributeParameterDescriptor { Name = p.GetString("name"), Type = p.GetString("type"), Required = p.GetBoolean("required") ?? false });
                    attr.Overloads.Add(overload);
                }
                d.Attributes.Add(attr);
            }
            d.Capabilities = o.GetStringArray("capabilities") ?? new List<string>();
            d.NonLayerPackages = o.GetStringArray("nonLayerPackages") ?? new List<string>();
            var c = o.GetObject("compatibility");
            if (c != null)
            {
                d.Compatibility.SupportedContractVersionMin = (int)(c.GetInteger("supportedContractVersionMin") ?? 0);
                d.Compatibility.SupportedContractVersionMax = (int)(c.GetInteger("supportedContractVersionMax") ?? 0);
                d.Compatibility.MinimumUpgradableContractVersion = (int)(c.GetInteger("minimumUpgradableContractVersion") ?? 0);
                d.Compatibility.MinimumManifestSchemaVersion = (int)(c.GetInteger("minimumManifestSchemaVersion") ?? 0);
                d.Compatibility.ManifestSchemaVersion = (int)(c.GetInteger("manifestSchemaVersion") ?? 0);
                d.Compatibility.MixedVersionRanges = c.GetStringArray("mixedVersionRanges") ?? new List<string>();
                var h = c.GetObject("hostRequirements");
                if (h != null)
                {
                    d.Compatibility.HostRequirements = new HostRequirementsDescriptor
                    {
                        Platform = h.GetString("platform"),
                        VisualStudioVersions = h.GetStringArray("visualStudioVersions") ?? new List<string>(),
                        VisualStudioChannels = h.GetStringArray("visualStudioChannels") ?? new List<string>(),
                        T4Component = h.GetString("t4Component"),
                        T4ComponentDisplayName = h.GetString("t4ComponentDisplayName"),
                        DotnetSdk = h.GetString("dotnetSdk"),
                        HeadlessDefinition = h.GetString("headlessDefinition"),
                    };
                }
            }
            return d;
        }
    }

    /// <summary>A descriptor document candidate: where it came from and its content.</summary>
    public sealed class DescriptorCandidate
    {
        public DescriptorCandidate(string source, string json) { Source = source; Json = json; }
        public string Source { get; }
        public string Json { get; }
    }

    /// <summary>
    /// Resolves the canonical descriptor among several embedded copies (mixed package versions):
    /// the highest platformVersion wins and a diagnostic names each conflicting version.
    /// </summary>
    public static class DescriptorResolver
    {
        public static PlatformDescriptor Resolve(IEnumerable<DescriptorCandidate> candidates, IDiagnosticSink diagnostics)
        {
            var parsed = new List<PlatformDescriptor>();
            foreach (var c in candidates ?? Enumerable.Empty<DescriptorCandidate>())
            {
                try { parsed.Add(PlatformDescriptor.FromJson(c.Json, c.Source)); }
                catch (Exception ex)
                {
                    diagnostics?.Report(new Diagnostic(DiagnosticCodes.DescriptorMissing, DiagnosticSeverity.Error, c.Source,
                        "The platform descriptor could not be parsed: " + ex.Message + ". Reinstall the package or regenerate the descriptor."));
                }
            }
            if (parsed.Count == 0)
            {
                diagnostics?.Report(new Diagnostic(DiagnosticCodes.DescriptorMissing, DiagnosticSeverity.Error, string.Empty,
                    "No platform descriptor (lazycrud.platform.json) was found in the installed packages. Install a Lazy.Crud.Builder.* package of version 1.1.0 or later."));
                return null;
            }
            var winner = parsed.OrderByDescending(p => p.ParsedPlatformVersion).ThenBy(p => p.Source, StringComparer.Ordinal).First();
            var distinct = parsed.Select(p => p.PlatformVersion).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            if (distinct.Count > 1)
            {
                var d = new Diagnostic(DiagnosticCodes.DescriptorConflict, DiagnosticSeverity.Warning, winner.Source,
                    "Installed packages embed descriptors with different platform versions (" +
                    string.Join(", ", parsed.Select(p => p.PlatformVersion + " from " + p.Source)) +
                    "); the highest platform version " + winner.PlatformVersion + " is canonical. Align package versions to remove this warning.");
                foreach (var p in parsed) d.With(p.Source, p.PlatformVersion);
                diagnostics?.Report(d);
            }
            return winner;
        }

        /// <summary>Negotiates a consumer's supported contract range against a descriptor.</summary>
        public static bool IsContractSupported(PlatformDescriptor descriptor, int consumerMin, int consumerMax, IDiagnosticSink diagnostics)
        {
            if (descriptor == null) return false;
            if (descriptor.ContractVersion > consumerMax)
            {
                diagnostics?.Report(new Diagnostic(DiagnosticCodes.ContractVersionMismatch, DiagnosticSeverity.Error, descriptor.Source ?? string.Empty,
                    "The descriptor declares contractVersion " + descriptor.ContractVersion + " but this tool supports " + consumerMin + " to " + consumerMax + ". Update the tool."));
                return false;
            }
            if (descriptor.ContractVersion < descriptor.Compatibility.MinimumUpgradableContractVersion || descriptor.ContractVersion < consumerMin)
            {
                diagnostics?.Report(new Diagnostic(DiagnosticCodes.ContractVersionMismatch, DiagnosticSeverity.Error, descriptor.Source ?? string.Empty,
                    "The descriptor declares contractVersion " + descriptor.ContractVersion + " which is below the supported minimum " + consumerMin + ". Upgrade the platform packages."));
                return false;
            }
            return true;
        }
    }
}
