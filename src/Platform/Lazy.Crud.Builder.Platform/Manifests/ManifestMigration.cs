using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Lazy.Crud.Builder.Platform.Descriptor;
using Lazy.Crud.Builder.Platform.Diagnostics;
using Lazy.Crud.Builder.Platform.Json;

namespace Lazy.Crud.Builder.Platform.Manifests
{
    /// <summary>
    /// Migrates a v1 manifest (no schemaVersion, PascalCase fields, absolute FullPath) to v2 in memory.
    /// Migration preserves every entry it can resolve and never fabricates values: unresolvable
    /// paths are retained and marked, missing hashes stay unknown, and ownership stays unknown
    /// unless the registry can supply it unambiguously.
    /// </summary>
    public static class ManifestMigration
    {
        public const string LegacyHashScope = "legacy-v1-raw";

        public static GenerationManifest MigrateV1(string json, string solutionRoot, string aggregateNameHint, IDiagnosticSink diagnostics)
        {
            var o = JsonLite.ParseObject(json);
            var m = new GenerationManifest
            {
                MigratedFromV1 = true,
                AggregateName = o.GetString("AggregateName") ?? o.GetString("aggregateName") ?? aggregateNameHint,
                GeneratorVersion = PlatformInfo.Version,
                ContractVersion = PlatformInfo.ContractVersion,
                SolutionRootMarker = SolutionRoot.MarkerFor(solutionRoot),
            };
            DateTime at;
            var atText = o.GetString("GeneratedAtUtc") ?? o.GetString("generatedAtUtc");
            if (DateTime.TryParse(atText, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out at)) m.GeneratedAtUtc = at;

            var normalizedRoot = SolutionRoot.NormalizeFullPath(solutionRoot);
            var files = o.GetArray("GeneratedFiles") ?? o.GetArray("generatedFiles") ?? o.GetArray("files") ?? new List<object>();
            int unresolved = 0, unknownHash = 0, unknownOwnership = 0;

            foreach (JsonObject f in files.OfType<JsonObject>())
            {
                var fullPath = f.GetString("FullPath") ?? f.GetString("fullPath");
                var templateName = f.GetString("TemplateName") ?? f.GetString("templateName");
                var layer = f.GetString("Layer") ?? f.GetString("layer") ?? string.Empty;
                var v1Hash = f.GetString("ContentHash") ?? f.GetString("contentHash");
                var entry = new ManifestEntry { Layer = layer };

                string relative = null;
                if (!string.IsNullOrWhiteSpace(fullPath))
                {
                    var relCandidate = SolutionRoot.TryMakeRelative(normalizedRoot, fullPath);
                    if (relCandidate != null) relative = relCandidate;
                }
                if (relative == null)
                {
                    entry.Unresolved = true;
                    entry.FileName = string.IsNullOrWhiteSpace(fullPath) ? string.Empty : Path.GetFileName(fullPath.Replace('\\', '/'));
                    entry.OriginalPathSha256 = string.IsNullOrWhiteSpace(fullPath) ? string.Empty : Sha256Hex(fullPath);
                    unresolved++;
                    diagnostics?.Report(new Diagnostic(DiagnosticCodes.MigrationUnresolvedPath, DiagnosticSeverity.Warning, fullPath ?? "<no path>",
                        "This v1 manifest entry does not fall under the current solution root (" + solutionRoot + "); it was retained as unresolved and no relative path was guessed. Regenerate the aggregate to re-record it, or delete the stale entry.")
                        .With("fileName", entry.FileName));
                }
                else
                {
                    entry.RelativePath = relative;
                }

                // Template identity: only from an unambiguous registry match, never guessed.
                TemplateRegistryEntry registry = null;
                if (!entry.Unresolved) registry = TemplateRegistry.TryMatchOutput(relative, m.AggregateName);
                if (registry != null)
                {
                    entry.TemplateId = registry.Id;
                    entry.Ownership = OwnershipNames.ToName(registry.Ownership);
                    if (string.IsNullOrEmpty(entry.Layer)) entry.Layer = registry.Layer;
                }
                else
                {
                    entry.TemplateId = string.IsNullOrEmpty(templateName) || string.Equals(templateName, "Unknown", StringComparison.OrdinalIgnoreCase) ? null : "legacy:" + templateName;
                    entry.Ownership = OwnershipNames.Unknown;
                    unknownOwnership++;
                    diagnostics?.Report(new Diagnostic(DiagnosticCodes.MigrationUnknownOwnership, DiagnosticSeverity.Warning, relative ?? entry.FileName,
                        "This v1 manifest entry carries no ownership and no registered template produces it; ownership is recorded as Unknown and the file is treated as user-owned until a generation run re-records it."));
                }

                // Hash: v1 hashed raw bytes, which is not the frozen rule; it is carried over as legacyHash but never a baseline.
                if (string.IsNullOrWhiteSpace(v1Hash))
                {
                    unknownHash++;
                    diagnostics?.Report(new Diagnostic(DiagnosticCodes.MigrationUnknownHash, DiagnosticSeverity.Warning, relative ?? entry.FileName,
                        "This v1 manifest entry has no content hash; it is recorded as unknown and is never treated as a match. The next generation resolves it as unverified and adopts it when the content is identical."));
                }
                else
                {
                    entry.LegacyHash = v1Hash;
                }
                entry.ContentHash = null;
                entry.HashAlgorithm = null;
                entry.HashScope = null;

                m.Files.Add(entry);
            }

            diagnostics?.Report(new Diagnostic(DiagnosticCodes.ManifestMigrated, DiagnosticSeverity.Information, GenerationManifest.GetManifestPath(solutionRoot, m.AggregateName ?? aggregateNameHint ?? string.Empty),
                "A v1 manifest was migrated in memory: " + m.Files.Count + " entries, " + unresolved + " unresolved, " + unknownHash + " without hash, " + unknownOwnership + " with unknown ownership. It is rewritten as v2 after the next successful generation."));
            return m;
        }

        private static string Sha256Hex(string text)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(new UTF8Encoding(false).GetBytes(text));
                var sb = new StringBuilder();
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
