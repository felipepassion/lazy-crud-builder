using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lazy.Crud.Builder.Platform.Diagnostics;
using Lazy.Crud.Builder.Platform.Manifests;

namespace Lazy.Crud.Builder.Platform
{
    /// <summary>Solution-root discovery and path helpers shared by the writer, the runner and the harness.</summary>
    public static class SolutionRoot
    {
        /// <summary>Walks up from a file or directory until a .sln/.slnx file is found. Returns null when none exists.</summary>
        public static string FindFromPath(string startPath)
        {
            if (string.IsNullOrWhiteSpace(startPath)) return null;
            var dir = Directory.Exists(startPath) ? startPath : Path.GetDirectoryName(startPath);
            while (!string.IsNullOrEmpty(dir))
            {
                try
                {
                    if (Directory.GetFiles(dir, "*.sln").Length > 0 || Directory.GetFiles(dir, "*.slnx").Length > 0) return dir;
                }
                catch { /* unreadable directory: keep walking up */ }
                var parent = Path.GetDirectoryName(dir);
                if (string.Equals(parent, dir, StringComparison.OrdinalIgnoreCase)) break;
                dir = parent;
            }
            return null;
        }

        /// <summary>The marker recorded in manifests: the solution file name the root was anchored on.</summary>
        public static string MarkerFor(string solutionRoot)
        {
            try
            {
                if (string.IsNullOrEmpty(solutionRoot) || !Directory.Exists(solutionRoot)) return null;
                var sln = Directory.GetFiles(solutionRoot, "*.sln").Concat(Directory.GetFiles(solutionRoot, "*.slnx")).OrderBy(p => p, StringComparer.Ordinal).FirstOrDefault();
                return sln == null ? null : Path.GetFileName(sln);
            }
            catch { return null; }
        }

        public static string NormalizeFullPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            var full = Path.GetFullPath(path).Replace('/', '\\').TrimEnd('\\');
            return full;
        }

        /// <summary>
        /// Expresses an absolute path relative to the root with forward slashes, or returns null when
        /// the path is not under the root. Comparison is case-insensitive (Windows semantics).
        /// </summary>
        public static string TryMakeRelative(string normalizedRoot, string absolutePath)
        {
            if (string.IsNullOrEmpty(normalizedRoot) || string.IsNullOrEmpty(absolutePath)) return null;
            string full;
            try
            {
                if (!Path.IsPathRooted(absolutePath)) return null;
                full = Path.GetFullPath(absolutePath).Replace('/', '\\');
            }
            catch { return null; }
            var root = normalizedRoot.TrimEnd('\\') + "\\";
            if (!full.StartsWith(root, StringComparison.OrdinalIgnoreCase)) return null;
            var rel = full.Substring(root.Length).Replace('\\', '/');
            return rel.Length == 0 ? null : rel;
        }

        /// <summary>Makes a path under the root relative (forward slashes); throws when outside the root.</summary>
        public static string MakeRelative(string solutionRoot, string absolutePath)
        {
            var rel = TryMakeRelative(NormalizeFullPath(solutionRoot), absolutePath);
            if (rel == null) throw new InvalidOperationException("The path '" + absolutePath + "' is not under the solution root '" + solutionRoot + "'.");
            return rel;
        }

        /// <summary>
        /// Candidate manifest roots for an aggregate: the solution root and its src sub-directory
        /// (the historical second root). Reports a conflict when more than one holds a manifest.
        /// </summary>
        public static string ResolveManifestRoot(string solutionRoot, string aggregateName, IDiagnosticSink diagnostics)
        {
            var candidates = new List<string> { solutionRoot, Path.Combine(solutionRoot, "src") };
            var found = candidates.Where(c => File.Exists(GenerationManifest.GetManifestPath(c, aggregateName))).ToList();
            if (found.Count > 1)
            {
                var d = new Diagnostic(DiagnosticCodes.ManifestRootConflict, DiagnosticSeverity.Warning, GenerationManifest.GetManifestPath(solutionRoot, aggregateName),
                    "Manifests for aggregate '" + aggregateName + "' exist under more than one root: " +
                    string.Join("; ", found.Select(f => GenerationManifest.GetManifestPath(f, aggregateName))) +
                    ". The solution root '" + solutionRoot + "' was chosen. Delete the stale manifest under the other root.");
                for (int i = 0; i < found.Count; i++) d.With("candidate" + (i + 1), GenerationManifest.GetManifestPath(found[i], aggregateName));
                d.With("chosen", solutionRoot);
                diagnostics?.Report(d);
            }
            return solutionRoot;
        }
    }
}
