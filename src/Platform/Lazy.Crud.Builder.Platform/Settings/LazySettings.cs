using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Lazy.Crud.Builder.Platform.Settings
{
    /// <summary>
    /// Reads lazy.settings (the Visual Studio settings XML used by the VSIX, the CLI and the templates).
    /// Only the keys the platform needs are modelled; unknown keys are preserved in <see cref="Raw"/>.
    /// </summary>
    public sealed class LazySettings
    {
        public const string FileName = "lazy.settings";

        public string Path { get; private set; }
        public Dictionary<string, string> Raw { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public string GenerationOutputPath { get { return Get("GenerationOutputPath", "LazyCode"); } }
        public string ModsPath { get { return Get("ModsPath", "src/mods"); } }
        public string TemplatesSource { get { return Get("TemplatesSource", "Local"); } }
        public bool UseNugetTemplates { get { return GetBool("UseNugetTemplates", false); } }
        public string TemplatesLocalProjectFilesPath { get { return Get("TemplatesLocalProjectFilesPath", "src/ProjectFiles"); } }
        public string TemplatesNuGetPackageId { get { return Get("TemplatesNuGetPackageId", Descriptor.TemplateRegistry.CanonicalTemplatesPackageId); } }
        public string TemplatesNuGetVersion { get { return Get("TemplatesNuGetVersion", string.Empty); } }
        public bool GenerateTests { get { return GetBool("GenerateTests", true); } }
        public string TestsPathPattern { get { return Get("TestsPathPattern", "Tests\\Lazy.Crud.{Aggregate}.Tests.{Layer}"); } }
        public int MaxParallelT4 { get { return GetInt("MaxParallelT4", 1); } }
        public int WaitTimeoutMs { get { return GetInt("WaitTimeoutMs", 5000); } }
        public int PollIntervalMs { get { return GetInt("PollIntervalMs", 150); } }
        public bool EnableT4SelfCleanup { get { return GetBool("EnableT4SelfCleanup", false); } }
        /// <summary>Kill switch for the generated-code-markers capability (default on).</summary>
        public bool GeneratedCodeMarkers { get { return GetBool("GeneratedCodeMarkers", true); } }
        /// <summary>When true, package resolution never degrades to a local source tree.</summary>
        public bool DisableLocalTemplateFallback { get { return GetBool("DisableLocalTemplateFallback", false); } }

        public string LayerPattern(string layerId)
        {
            var key = layerId.Replace(".", "") + "LayerPattern";
            return Get(key, "{Aggregate}." + layerId);
        }

        public string Get(string key, string defaultValue)
        {
            string v;
            return Raw.TryGetValue(key, out v) && v != null ? v : defaultValue;
        }

        public bool GetBool(string key, bool defaultValue)
        {
            string v; bool b;
            return Raw.TryGetValue(key, out v) && bool.TryParse(v, out b) ? b : defaultValue;
        }

        public int GetInt(string key, int defaultValue)
        {
            string v; int i;
            return Raw.TryGetValue(key, out v) && int.TryParse(v, NumberStyles.Integer, CultureInfo.InvariantCulture, out i) ? i : defaultValue;
        }

        /// <summary>Candidate locations for lazy.settings under a solution root, in probe order.</summary>
        public static IEnumerable<string> CandidatePaths(string solutionRoot)
        {
            yield return System.IO.Path.Combine(solutionRoot, "src", "Mods", FileName);
            yield return System.IO.Path.Combine(solutionRoot, "Mods", FileName);
        }

        public static string Locate(string solutionRoot)
        {
            if (string.IsNullOrEmpty(solutionRoot)) return null;
            foreach (var c in CandidatePaths(solutionRoot)) if (File.Exists(c)) return c;
            return null;
        }

        public static LazySettings Load(string path)
        {
            var s = new LazySettings { Path = path };
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return s;
            var doc = new XmlDocument { XmlResolver = null };
            doc.Load(path);
            foreach (XmlNode setting in doc.GetElementsByTagName("Setting"))
            {
                var name = setting.Attributes != null && setting.Attributes["Name"] != null ? setting.Attributes["Name"].Value : null;
                if (string.IsNullOrEmpty(name)) continue;
                string value = string.Empty;
                foreach (XmlNode child in setting.ChildNodes)
                {
                    if (child.NodeType == XmlNodeType.Element && child.LocalName == "Value") { value = child.InnerText.Trim(); break; }
                }
                s.Raw[name] = value;
            }
            return s;
        }

        /// <summary>Loads the settings for a solution root; returns defaults (Path null) when absent.</summary>
        public static LazySettings ForSolution(string solutionRoot)
        {
            return Load(Locate(solutionRoot));
        }
    }
}
