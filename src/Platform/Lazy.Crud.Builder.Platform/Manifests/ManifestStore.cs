using System;
using System.IO;
using System.Text;
using System.Threading;
using Lazy.Crud.Builder.Platform.Diagnostics;

namespace Lazy.Crud.Builder.Platform.Manifests
{
    /// <summary>Outcome of loading a manifest: present, absent, or failed. Never a silent null.</summary>
    public sealed class ManifestLoadResult
    {
        public GenerationManifest Manifest { get; set; }
        public bool NotPresent { get; set; }
        public bool Failed { get; set; }
        public Diagnostic Failure { get; set; }
        public string Path { get; set; }
        public bool Loaded { get { return Manifest != null; } }
    }

    /// <summary>
    /// Loads, migrates and saves manifests under {solutionRoot}/.lazycrud/manifests. Failures are
    /// reported as diagnostics and returned explicitly; nothing is swallowed.
    /// </summary>
    public static class ManifestStore
    {
        public static ManifestLoadResult Load(string solutionRoot, string aggregateName, IDiagnosticSink diagnostics)
        {
            var path = GenerationManifest.GetManifestPath(solutionRoot, aggregateName);
            var result = new ManifestLoadResult { Path = path };
            if (!File.Exists(path))
            {
                result.NotPresent = true;
                return result;
            }
            string json;
            try
            {
                json = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                result.Failed = true;
                result.Failure = new Diagnostic(DiagnosticCodes.ManifestUnreadable, DiagnosticSeverity.Error, path,
                    "The manifest could not be read: " + ex.GetType().Name + ": " + ex.Message + ". Fix the file permissions or delete the manifest to start over.");
                diagnostics?.Report(result.Failure);
                return result;
            }
            try
            {
                var schema = GenerationManifest.DetectSchemaVersion(json);
                if (schema >= GenerationManifest.CurrentSchemaVersion)
                {
                    result.Manifest = GenerationManifest.FromJsonV2(json);
                }
                else
                {
                    result.Manifest = ManifestMigration.MigrateV1(json, solutionRoot, aggregateName, diagnostics);
                }
                if (string.IsNullOrEmpty(result.Manifest.AggregateName)) result.Manifest.AggregateName = aggregateName;
                return result;
            }
            catch (Exception ex)
            {
                result.Failed = true;
                result.Failure = new Diagnostic(DiagnosticCodes.ManifestUnreadable, DiagnosticSeverity.Error, path,
                    "The manifest exists but cannot be parsed: " + ex.Message + ". Repair the JSON or delete the manifest; generated files are not touched.");
                diagnostics?.Report(result.Failure);
                return result;
            }
        }

        /// <summary>Saves as v2. Returns false (and reports) when the write fails.</summary>
        public static bool Save(GenerationManifest manifest, string solutionRoot, IDiagnosticSink diagnostics)
        {
            var path = GenerationManifest.GetManifestPath(solutionRoot, manifest.AggregateName);
            try
            {
                manifest.GeneratedAtUtc = DateTime.UtcNow;
                if (string.IsNullOrEmpty(manifest.GeneratorVersion)) manifest.GeneratorVersion = PlatformInfo.Version;
                if (manifest.ContractVersion <= 0) manifest.ContractVersion = PlatformInfo.ContractVersion;
                if (string.IsNullOrEmpty(manifest.SolutionRootMarker)) manifest.SolutionRootMarker = SolutionRoot.MarkerFor(solutionRoot);
                var json = manifest.ToJson();
                var dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                var tmp = path + ".tmp";
                File.WriteAllText(tmp, json, new UTF8Encoding(false));
                if (File.Exists(path)) File.Delete(path);
                File.Move(tmp, path);
                manifest.MigratedFromV1 = false;
                return true;
            }
            catch (Exception ex)
            {
                diagnostics?.Report(new Diagnostic(DiagnosticCodes.ManifestSaveFailed, DiagnosticSeverity.Error, path,
                    "The manifest could not be saved: " + ex.GetType().Name + ": " + ex.Message + ". The run must not be treated as successful; make the .lazycrud directory writable and rerun."));
                return false;
            }
        }

        public static bool Delete(string solutionRoot, string aggregateName, IDiagnosticSink diagnostics)
        {
            var path = GenerationManifest.GetManifestPath(solutionRoot, aggregateName);
            try
            {
                if (File.Exists(path)) File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                diagnostics?.Report(new Diagnostic(DiagnosticCodes.ManifestSaveFailed, DiagnosticSeverity.Error, path,
                    "The manifest could not be deleted: " + ex.Message));
                return false;
            }
        }

        /// <summary>
        /// Runs an action under a system-wide named mutex keyed by the manifest path, so parallel
        /// layer transformations of one aggregate never interleave manifest read-modify-write cycles.
        /// </summary>
        public static T WithLock<T>(string solutionRoot, string aggregateName, Func<T> action)
        {
            var key = "Global\\LazyCrud.Manifest." + Sha1(GenerationManifest.GetManifestPath(solutionRoot, aggregateName).ToLowerInvariant());
            bool created;
            using (var mutex = new Mutex(false, key, out created))
            {
                bool acquired = false;
                try
                {
                    try { acquired = mutex.WaitOne(TimeSpan.FromMinutes(2)); }
                    catch (AbandonedMutexException) { acquired = true; }
                    if (!acquired) throw new TimeoutException("Could not acquire the manifest lock for aggregate '" + aggregateName + "' within 2 minutes.");
                    return action();
                }
                finally
                {
                    if (acquired) { try { mutex.ReleaseMutex(); } catch { /* released by abandonment */ } }
                }
            }
        }

        private static string Sha1(string text)
        {
            using (var sha = System.Security.Cryptography.SHA1.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));
                var sb = new StringBuilder();
                foreach (var b in bytes) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
