using System;
using System.Collections.Generic;
using System.IO;
using Lazy.Crud.Builder.Platform.Hashing;
using Lazy.Crud.Builder.Platform.Manifests;
using Lazy.Crud.Builder.Platform.Markers;

namespace Lazy.Crud.Builder.Platform.Writer
{
    /// <summary>The resolved state of a GeneratorOwned file before it is written.</summary>
    public enum FileState
    {
        /// <summary>No manifest entry and no file on disk: a new file.</summary>
        New = 0,
        /// <summary>The current hash equals the recorded hash. Safe to overwrite.</summary>
        Verified = 1,
        /// <summary>The current hash differs from the recorded hash. Modified outside the generator.</summary>
        Drifted = 2,
        /// <summary>The file exists but no trustworthy baseline hash is recorded.</summary>
        Unverified = 3,
        /// <summary>Recorded in the manifest but absent from disk.</summary>
        Missing = 4,
    }

    public sealed class FileStateResult
    {
        public string RelativePath { get; set; }
        public string AbsolutePath { get; set; }
        public FileState State { get; set; }
        public ManifestEntry Entry { get; set; }
        public ContentHash CurrentHash { get; set; }
        public MarkerInfo Marker { get; set; }
        public bool MarkerProblem { get; set; }
        public bool UnknownOwnership { get; set; }
    }

    /// <summary>
    /// Resolves verified/drifted/unverified/missing for GeneratorOwned files using the frozen hash.
    /// Used by the driver for the whole aggregate before the first write, and by the writer per file.
    /// </summary>
    public static class DriftInspector
    {
        public static FileStateResult Inspect(string solutionRoot, string relativePath, ManifestEntry entry)
        {
            var abs = Path.Combine(solutionRoot, relativePath.Replace('/', Path.DirectorySeparatorChar));
            var result = new FileStateResult { RelativePath = relativePath, AbsolutePath = abs, Entry = entry };
            var exists = File.Exists(abs);
            if (!exists)
            {
                result.State = entry == null ? FileState.New : FileState.Missing;
                return result;
            }
            string content = File.ReadAllText(abs);
            result.CurrentHash = ContentHasher.Compute(content);
            MarkerInfo marker;
            if (GeneratedCodeMarker.TryParse(content, out marker)) result.Marker = marker;
            if (entry == null)
            {
                // Exists on disk, not in the manifest: no baseline.
                result.State = FileState.Unverified;
                return result;
            }
            result.UnknownOwnership = entry.OwnershipValue == null;
            if (!entry.HasTrustworthyBaseline)
            {
                result.State = FileState.Unverified;
                return result;
            }
            if (ContentHasher.HashEquals(entry.ContentHash, result.CurrentHash.Value))
            {
                result.State = FileState.Verified;
                var syntax = CommentSyntax.ForFileName(abs);
                result.MarkerProblem = syntax != null && (result.Marker == null || !result.Marker.IsV2 || result.Marker.Problems.Count > 0);
                return result;
            }
            result.State = FileState.Drifted;
            return result;
        }

        /// <summary>Resolves the state of every resolved GeneratorOwned entry recorded in a manifest.</summary>
        public static List<FileStateResult> InspectAggregate(string solutionRoot, GenerationManifest manifest)
        {
            var results = new List<FileStateResult>();
            if (manifest == null) return results;
            foreach (var entry in manifest.Files)
            {
                if (entry.Unresolved || string.IsNullOrEmpty(entry.RelativePath)) continue;
                var ownership = entry.OwnershipValue;
                if (ownership == Ownership.GeneratedOnce || ownership == Ownership.Manual) continue;
                results.Add(Inspect(solutionRoot, entry.RelativePath, entry));
            }
            return results;
        }
    }
}
