using System;
using System.Collections.Generic;

namespace Lazy.Crud.Builder.Platform.Diagnostics
{
    /// <summary>Severity of a diagnostic. A run fails if and only if at least one error was emitted.</summary>
    public enum DiagnosticSeverity
    {
        Information = 0,
        Warning = 1,
        Error = 2,
    }

    /// <summary>
    /// A structured, actionable diagnostic: stable code, severity, affected path or item, a message
    /// stating what to do next, and optional structured data (hashes, template ids, flags).
    /// </summary>
    public sealed class Diagnostic
    {
        public Diagnostic(string code, DiagnosticSeverity severity, string path, string message)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("code is required", nameof(code));
            if (string.IsNullOrWhiteSpace(message)) throw new ArgumentException("message is required", nameof(message));
            Code = code;
            Severity = severity;
            Path = path ?? string.Empty;
            Message = message;
            TimestampUtc = DateTime.UtcNow;
        }

        /// <summary>Stable, documented code such as <c>LC2001</c>. Tests assert on it, never on text.</summary>
        public string Code { get; }
        public DiagnosticSeverity Severity { get; }
        /// <summary>Affected file (solution-relative with forward slashes when known) or item name.</summary>
        public string Path { get; }
        public string Message { get; }
        public DateTime TimestampUtc { get; }
        public Dictionary<string, string> Data { get; } = new Dictionary<string, string>(StringComparer.Ordinal);

        public Diagnostic With(string key, string value)
        {
            Data[key] = value ?? string.Empty;
            return this;
        }

        public override string ToString()
        {
            var sev = Severity == DiagnosticSeverity.Error ? "error" : Severity == DiagnosticSeverity.Warning ? "warning" : "info";
            return string.IsNullOrEmpty(Path)
                ? $"{sev} {Code}: {Message}"
                : $"{sev} {Code}: {Path}: {Message}";
        }
    }

    /// <summary>Receives diagnostics as they are emitted.</summary>
    public interface IDiagnosticSink
    {
        void Report(Diagnostic diagnostic);
    }

    /// <summary>Collects diagnostics for programmatic inspection (tests, headless runs).</summary>
    public sealed class DiagnosticCollector : IDiagnosticSink
    {
        private readonly List<Diagnostic> _items = new List<Diagnostic>();
        private readonly object _gate = new object();

        public void Report(Diagnostic diagnostic)
        {
            if (diagnostic == null) throw new ArgumentNullException(nameof(diagnostic));
            lock (_gate) { _items.Add(diagnostic); }
        }

        public IReadOnlyList<Diagnostic> Items
        {
            get { lock (_gate) { return _items.ToArray(); } }
        }

        public bool HasErrors
        {
            get { lock (_gate) { foreach (var d in _items) if (d.Severity == DiagnosticSeverity.Error) return true; return false; } }
        }

        public int Count { get { lock (_gate) { return _items.Count; } } }

        public IEnumerable<Diagnostic> WithCode(string code)
        {
            foreach (var d in Items) if (string.Equals(d.Code, code, StringComparison.Ordinal)) yield return d;
        }

        public void Clear() { lock (_gate) { _items.Clear(); } }
    }

    /// <summary>A sink that forwards to several sinks (for example collector + T4 host + file).</summary>
    public sealed class CompositeDiagnosticSink : IDiagnosticSink
    {
        private readonly IDiagnosticSink[] _sinks;
        public CompositeDiagnosticSink(params IDiagnosticSink[] sinks) { _sinks = sinks ?? new IDiagnosticSink[0]; }
        public void Report(Diagnostic diagnostic)
        {
            foreach (var s in _sinks) if (s != null) s.Report(diagnostic);
        }
    }
}
