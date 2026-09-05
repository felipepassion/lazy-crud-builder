using System;
using System.IO;
using System.Text;
using Lazy.Crud.Builder.Platform.Json;

namespace Lazy.Crud.Builder.Platform.Writer
{
    /// <summary>How drifted and unverified files are resolved for a run.</summary>
    public enum ResolutionMode
    {
        /// <summary>Default: protect the file, report it, fail the run.</summary>
        Protect = 0,
        /// <summary>Overwrite deliberately and report each overwrite.</summary>
        Force = 1,
        /// <summary>Leave the affected files untouched, generate the rest, report success listing the skips.</summary>
        SkipModified = 2,
    }

    /// <summary>
    /// Per-aggregate run context written by the driver before transformation and read by the shared
    /// writer inside the T4 host: {solutionRoot}/.lazycrud/runs/{Aggregate}.run.json. When absent
    /// (an interactive "Run Custom Tool" in the IDE) the defaults apply: protect, markers on.
    /// </summary>
    public sealed class RunContext
    {
        public const string RunsDirectory = "runs";

        public string RunId { get; set; }
        public ResolutionMode Resolution { get; set; } = ResolutionMode.Protect;
        public bool MarkersEnabled { get; set; } = true;
        public string TemplateSourceKind { get; set; }
        public string TemplateSourceLocation { get; set; }
        public string TemplateSourceVersion { get; set; }
        public bool Present { get; set; }

        public static string GetPath(string solutionRoot, string aggregateName)
        {
            return Path.Combine(solutionRoot, Manifests.GenerationManifest.ManifestDirectory, RunsDirectory, aggregateName + ".run.json");
        }

        public static RunContext Load(string solutionRoot, string aggregateName)
        {
            var path = GetPath(solutionRoot, aggregateName);
            var ctx = new RunContext();
            if (!File.Exists(path)) return ctx;
            try
            {
                var o = JsonLite.ParseObject(File.ReadAllText(path));
                ctx.Present = true;
                ctx.RunId = o.GetString("runId");
                ResolutionMode mode;
                if (Enum.TryParse(o.GetString("resolution") ?? "Protect", true, out mode)) ctx.Resolution = mode;
                ctx.MarkersEnabled = o.GetBoolean("markersEnabled") ?? true;
                ctx.TemplateSourceKind = o.GetString("templateSourceKind");
                ctx.TemplateSourceLocation = o.GetString("templateSourceLocation");
                ctx.TemplateSourceVersion = o.GetString("templateSourceVersion");
            }
            catch
            {
                // A corrupt run context must never weaken protection: defaults (Protect) apply.
                ctx.Present = false;
            }
            return ctx;
        }

        public void Save(string solutionRoot, string aggregateName)
        {
            var path = GetPath(solutionRoot, aggregateName);
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            var o = new JsonObject();
            o["runId"] = RunId ?? Guid.NewGuid().ToString("N");
            o["resolution"] = Resolution.ToString();
            o["markersEnabled"] = MarkersEnabled;
            o["templateSourceKind"] = TemplateSourceKind;
            o["templateSourceLocation"] = TemplateSourceLocation;
            o["templateSourceVersion"] = TemplateSourceVersion;
            o["createdAtUtc"] = DateTime.UtcNow;
            File.WriteAllText(path, JsonLite.Write(o, true), new UTF8Encoding(false));
        }

        public static void Delete(string solutionRoot, string aggregateName)
        {
            var path = GetPath(solutionRoot, aggregateName);
            if (File.Exists(path)) File.Delete(path);
        }
    }
}
