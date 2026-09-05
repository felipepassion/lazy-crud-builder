using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lazy.Crud.Builder.Platform.Hashing;
using Lazy.Crud.Builder.Platform.Json;

namespace Lazy.Crud.Builder.Platform.Manifests
{
    /// <summary>One file recorded in a generation manifest v2.</summary>
    public sealed class ManifestEntry
    {
        /// <summary>Solution-relative path with forward slashes; null for unresolved migrated entries.</summary>
        public string RelativePath { get; set; }
        public string Layer { get; set; }
        public string TemplateId { get; set; }
        public string TemplateVersion { get; set; }
        /// <summary>GeneratorOwned, GeneratedOnce, Manual or Unknown.</summary>
        public string Ownership { get; set; }
        /// <summary>Null or empty when unknown.</summary>
        public string ContentHash { get; set; }
        public string HashAlgorithm { get; set; }
        public string HashScope { get; set; }

        /// <summary>True for a migrated v1 entry whose absolute path could not be expressed under the solution root.</summary>
        public bool Unresolved { get; set; }
        /// <summary>File name of an unresolved entry (never a path).</summary>
        public string FileName { get; set; }
        /// <summary>SHA-256 of the original absolute path of an unresolved entry, so it can be recognized without storing the path.</summary>
        public string OriginalPathSha256 { get; set; }
        /// <summary>The v1 raw-file hash carried over by migration; never a baseline under the frozen rule.</summary>
        public string LegacyHash { get; set; }

        public bool HashKnown { get { return !string.IsNullOrEmpty(ContentHash); } }

        /// <summary>A hash is a trustworthy baseline only when it was computed under the frozen rule.</summary>
        public bool HasTrustworthyBaseline
        {
            get
            {
                return HashKnown
                    && string.Equals(HashAlgorithm, ContentHasher.AlgorithmName, StringComparison.OrdinalIgnoreCase)
                    && (HashScope == Hashing.HashScope.BodyLfMarkerExcluded || HashScope == Hashing.HashScope.BodyLf);
            }
        }

        public Ownership? OwnershipValue
        {
            get { Ownership o; return OwnershipNames.TryParse(Ownership, out o) ? o : (Ownership?)null; }
        }

        public ManifestEntry Clone()
        {
            return (ManifestEntry)MemberwiseClone();
        }
    }

    /// <summary>The generation manifest v2 document (contract F2).</summary>
    public sealed class GenerationManifest
    {
        public const int CurrentSchemaVersion = 2;
        public const string ManifestDirectory = ".lazycrud";
        public const string ManifestsSubDirectory = "manifests";

        public int SchemaVersion { get; set; } = CurrentSchemaVersion;
        public string AggregateName { get; set; }
        public DateTime GeneratedAtUtc { get; set; } = DateTime.UtcNow;
        public string GeneratorVersion { get; set; }
        public int ContractVersion { get; set; }
        /// <summary>How relative paths are anchored: the solution file name the root was derived from.</summary>
        public string SolutionRootMarker { get; set; }
        public List<ManifestEntry> Files { get; set; } = new List<ManifestEntry>();
        /// <summary>True when this document was loaded from a v1 manifest and migrated in memory.</summary>
        public bool MigratedFromV1 { get; set; }

        public static string GetManifestPath(string solutionRoot, string aggregateName)
        {
            return System.IO.Path.Combine(solutionRoot, ManifestDirectory, ManifestsSubDirectory, aggregateName + ".manifest.json");
        }

        public static string NormalizeRelativePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return relativePath;
            var p = relativePath.Replace('\\', '/');
            while (p.StartsWith("./", StringComparison.Ordinal)) p = p.Substring(2);
            return p;
        }

        public ManifestEntry Find(string relativePath)
        {
            var key = NormalizeRelativePath(relativePath);
            return Files.FirstOrDefault(f => !f.Unresolved && string.Equals(NormalizeRelativePath(f.RelativePath), key, StringComparison.OrdinalIgnoreCase));
        }

        public void Upsert(ManifestEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));
            entry.RelativePath = NormalizeRelativePath(entry.RelativePath);
            var existing = Find(entry.RelativePath);
            if (existing != null) Files[Files.IndexOf(existing)] = entry;
            else Files.Add(entry);
        }

        public bool Remove(string relativePath)
        {
            var existing = Find(relativePath);
            if (existing == null) return false;
            Files.Remove(existing);
            return true;
        }

        // ------------------------------------------------------------------ JSON ---------------

        /// <summary>Serializes as v2. Refuses absolute paths, drive letters and backslashes.</summary>
        public string ToJson()
        {
            var o = new JsonObject();
            o["schemaVersion"] = (long)CurrentSchemaVersion;
            o["aggregateName"] = AggregateName ?? string.Empty;
            o["generatedAtUtc"] = GeneratedAtUtc.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture);
            o["generatorVersion"] = GeneratorVersion ?? PlatformInfo.Version;
            o["contractVersion"] = (long)(ContractVersion > 0 ? ContractVersion : PlatformInfo.ContractVersion);
            if (!string.IsNullOrEmpty(SolutionRootMarker)) o["solutionRootMarker"] = SolutionRootMarker;
            var files = new List<object>();
            foreach (var f in Files)
            {
                var j = new JsonObject();
                if (f.Unresolved)
                {
                    j["relativePath"] = null;
                    j["unresolved"] = true;
                    j["fileName"] = f.FileName ?? string.Empty;
                    j["originalPathSha256"] = f.OriginalPathSha256 ?? string.Empty;
                }
                else
                {
                    var rel = NormalizeRelativePath(f.RelativePath);
                    AssertRelative(rel);
                    j["relativePath"] = rel;
                }
                j["layer"] = f.Layer ?? string.Empty;
                j["templateId"] = f.TemplateId;
                j["templateVersion"] = f.TemplateVersion;
                j["ownership"] = string.IsNullOrEmpty(f.Ownership) ? OwnershipNames.Unknown : f.Ownership;
                j["contentHash"] = string.IsNullOrEmpty(f.ContentHash) ? null : f.ContentHash;
                j["hashAlgorithm"] = f.HashAlgorithm;
                j["hashScope"] = f.HashScope;
                if (!string.IsNullOrEmpty(f.LegacyHash)) j["legacyHash"] = f.LegacyHash;
                files.Add(j);
            }
            o["files"] = files;
            return JsonLite.Write(o, true);
        }

        internal static void AssertRelative(string rel)
        {
            if (string.IsNullOrEmpty(rel)) throw new InvalidOperationException("A manifest entry has no relativePath.");
            if (rel.IndexOf('\\') >= 0) throw new InvalidOperationException("Manifest paths must use forward slashes: " + rel);
            if (rel.Length >= 2 && rel[1] == ':') throw new InvalidOperationException("Manifest paths must not contain a drive letter: " + rel);
            if (rel.StartsWith("/", StringComparison.Ordinal) || rel.StartsWith("//", StringComparison.Ordinal)) throw new InvalidOperationException("Manifest paths must be relative: " + rel);
        }

        /// <summary>Parses a v2 document. Throws FormatException for malformed input; v1 input must go through <see cref="ManifestMigration"/>.</summary>
        public static GenerationManifest FromJsonV2(string json)
        {
            var o = JsonLite.ParseObject(json);
            var schema = o.GetInteger("schemaVersion");
            if (schema != CurrentSchemaVersion) throw new FormatException("Not a manifest v2 document (schemaVersion=" + (schema.HasValue ? schema.Value.ToString(CultureInfo.InvariantCulture) : "absent") + ")");
            var m = new GenerationManifest
            {
                SchemaVersion = CurrentSchemaVersion,
                AggregateName = o.GetString("aggregateName"),
                GeneratorVersion = o.GetString("generatorVersion"),
                ContractVersion = (int)(o.GetInteger("contractVersion") ?? 0),
                SolutionRootMarker = o.GetString("solutionRootMarker"),
            };
            DateTime at;
            if (DateTime.TryParse(o.GetString("generatedAtUtc"), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out at)) m.GeneratedAtUtc = at;
            foreach (JsonObject f in (o.GetArray("files") ?? new List<object>()).OfType<JsonObject>())
            {
                var e = new ManifestEntry
                {
                    RelativePath = f.GetString("relativePath"),
                    Unresolved = f.GetBoolean("unresolved") ?? false,
                    FileName = f.GetString("fileName"),
                    OriginalPathSha256 = f.GetString("originalPathSha256"),
                    Layer = f.GetString("layer"),
                    TemplateId = f.GetString("templateId"),
                    TemplateVersion = f.GetString("templateVersion"),
                    Ownership = f.GetString("ownership") ?? OwnershipNames.Unknown,
                    ContentHash = f.GetString("contentHash"),
                    HashAlgorithm = f.GetString("hashAlgorithm"),
                    HashScope = f.GetString("hashScope"),
                    LegacyHash = f.GetString("legacyHash"),
                };
                if (!e.Unresolved && !string.IsNullOrEmpty(e.RelativePath)) e.RelativePath = NormalizeRelativePath(e.RelativePath);
                m.Files.Add(e);
            }
            return m;
        }

        /// <summary>Returns the schemaVersion of a manifest document, 1 when absent.</summary>
        public static int DetectSchemaVersion(string json)
        {
            var o = JsonLite.ParseObject(json);
            var v = o.GetInteger("schemaVersion");
            return v.HasValue ? (int)v.Value : 1;
        }
    }
}
