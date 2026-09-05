using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lazy.Crud.Builder.Platform.Json;

namespace Lazy.Crud.Builder.Platform.Diagnostics
{
    /// <summary>
    /// Machine-readable diagnostics for headless runs:
    /// {solutionRoot}/.lazycrud/diagnostics/{Aggregate}.diagnostics.json (a JSON array).
    /// The writer appends from inside the T4 host; the driver clears it before a run and reads it after.
    /// </summary>
    public sealed class DiagnosticsFile : IDiagnosticSink
    {
        public const string Directory = "diagnostics";

        public string Path { get; }

        public DiagnosticsFile(string path) { Path = path; }

        public static string GetPath(string solutionRoot, string aggregateName)
        {
            return System.IO.Path.Combine(solutionRoot, Manifests.GenerationManifest.ManifestDirectory, Directory, aggregateName + ".diagnostics.json");
        }

        public static DiagnosticsFile For(string solutionRoot, string aggregateName)
        {
            return new DiagnosticsFile(GetPath(solutionRoot, aggregateName));
        }

        /// <summary>Set when the file could not be written; the caller reports it through its other sinks.</summary>
        public Exception LastWriteError { get; private set; }

        public void Report(Diagnostic diagnostic)
        {
            try
            {
                var list = ReadRaw();
                list.Add(ToJson(diagnostic));
                var dir = System.IO.Path.GetDirectoryName(Path);
                if (!System.IO.Directory.Exists(dir)) System.IO.Directory.CreateDirectory(dir);
                File.WriteAllText(Path, JsonLite.Write(list, true), new UTF8Encoding(false));
            }
            catch (Exception ex)
            {
                // The diagnostics file is one of several sinks; a failure to persist it is surfaced by
                // the writer as its own diagnostic (LastWriteError) and never masks the original one.
                LastWriteError = ex;
            }
        }

        public void Clear()
        {
            if (File.Exists(Path)) File.Delete(Path);
        }

        public static JsonObject ToJson(Diagnostic d)
        {
            var o = new JsonObject();
            o["code"] = d.Code;
            o["severity"] = d.Severity.ToString().ToLowerInvariant();
            o["path"] = d.Path;
            o["message"] = d.Message;
            o["timestampUtc"] = d.TimestampUtc;
            var data = new JsonObject();
            foreach (var kv in d.Data) data[kv.Key] = kv.Value;
            o["data"] = data;
            return o;
        }

        public static Diagnostic FromJson(JsonObject o)
        {
            DiagnosticSeverity sev;
            if (!Enum.TryParse(o.GetString("severity") ?? "information", true, out sev)) sev = DiagnosticSeverity.Information;
            var d = new Diagnostic(o.GetString("code") ?? "LC0000", sev, o.GetString("path") ?? string.Empty, o.GetString("message") ?? "(no message)");
            var data = o.GetObject("data");
            if (data != null) foreach (var kv in data) d.With(kv.Key, kv.Value as string ?? Convert.ToString(kv.Value));
            return d;
        }

        public IReadOnlyList<Diagnostic> Read()
        {
            return ReadRaw().OfType<JsonObject>().Select(FromJson).ToList();
        }

        private List<object> ReadRaw()
        {
            if (!File.Exists(Path)) return new List<object>();
            try
            {
                var parsed = JsonLite.Parse(File.ReadAllText(Path)) as List<object>;
                return parsed ?? new List<object>();
            }
            catch
            {
                // A corrupt diagnostics file is replaced; the diagnostic being written is not lost.
                return new List<object>();
            }
        }
    }
}
