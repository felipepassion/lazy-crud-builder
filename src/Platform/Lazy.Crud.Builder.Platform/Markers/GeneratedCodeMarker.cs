using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lazy.Crud.Builder.Platform.Markers
{
    /// <summary>Comment syntax used to emit a marker for a given file type.</summary>
    public sealed class CommentSyntax
    {
        private CommentSyntax(string id, string open, string linePrefix, string close)
        {
            Id = id; Open = open; LinePrefix = linePrefix; Close = close;
        }

        public string Id { get; }
        /// <summary>Line emitted before the marker lines (empty for line-comment syntaxes).</summary>
        public string Open { get; }
        /// <summary>Prefix of every marker line.</summary>
        public string LinePrefix { get; }
        /// <summary>Line emitted after the marker lines (empty for line-comment syntaxes).</summary>
        public string Close { get; }
        public bool IsBlock { get { return Open.Length > 0; } }

        public static readonly CommentSyntax CSharpLine = new CommentSyntax("line-slash", "", "// ", "");
        public static readonly CommentSyntax CBlock = new CommentSyntax("block-c", "/*", " * ", " */");
        public static readonly CommentSyntax Razor = new CommentSyntax("block-razor", "@*", " ", "*@");
        public static readonly CommentSyntax T4 = new CommentSyntax("block-t4", "<#/*", " ", "*/#>");
        public static readonly CommentSyntax Batch = new CommentSyntax("line-rem", "", "REM ", "");
        public static readonly CommentSyntax Sql = new CommentSyntax("line-dash", "", "-- ", "");
        public static readonly CommentSyntax Hash = new CommentSyntax("line-hash", "", "# ", "");
        public static readonly CommentSyntax Html = new CommentSyntax("block-html", "<!--", " ", "-->");

        public static IEnumerable<CommentSyntax> All
        {
            get { return new[] { CSharpLine, CBlock, Razor, T4, Batch, Sql, Hash, Html }; }
        }

        /// <summary>
        /// Selects the comment syntax for a file name, or null when no safe syntax exists
        /// (for example .json or XML documents whose first bytes are significant).
        /// </summary>
        public static CommentSyntax ForFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return null;
            var lower = fileName.ToLowerInvariant();
            if (lower.EndsWith(".razor.css") || lower.EndsWith(".cshtml.css")) return CBlock;
            var ext = Path.GetExtension(lower);
            switch (ext)
            {
                case ".cs": case ".ts": case ".js": case ".java": case ".kt": case ".go": case ".swift":
                case ".rs": case ".c": case ".cpp": case ".h": case ".hpp": case ".scala": case ".dart":
                    return CSharpLine;
                case ".css": case ".scss": case ".less": case ".sass":
                    return CBlock;
                case ".razor": case ".cshtml":
                    return Razor;
                case ".tt": case ".ttinclude":
                    return T4;
                case ".bat": case ".cmd":
                    return Batch;
                case ".sql":
                    return Sql;
                case ".ps1": case ".psm1": case ".psd1": case ".sh": case ".py": case ".rb": case ".yml": case ".yaml": case ".toml":
                    return Hash;
                case ".md": case ".html": case ".htm":
                    return Html;
                default:
                    return null;
            }
        }
    }

    /// <summary>The frozen field set of marker v2.</summary>
    public sealed class MarkerFields
    {
        public const string GeneratorIdentity = "Lazy.Crud.Builder";
        public const string MarkerVersion = "v2";

        public string Generator { get; set; } = GeneratorIdentity;
        public string Aggregate { get; set; }
        public string Layer { get; set; }
        public string TemplateId { get; set; }
        public Ownership Ownership { get; set; }
        public string GeneratorVersion { get; set; }
        /// <summary>Optional; emitted when the descriptor supplies one.</summary>
        public string TemplateVersion { get; set; }
        /// <summary>Optional; the template source file, named in the customization directive.</summary>
        public string SourceTemplate { get; set; }
    }

    /// <summary>Location of a leading marker block inside file content.</summary>
    public sealed class MarkerBlock
    {
        public int Start { get; set; }
        /// <summary>Index of the first body character (after the newline terminating the block).</summary>
        public int BodyStart { get; set; }
        public string Text { get; set; }
    }

    /// <summary>Result of parsing a file's opening lines.</summary>
    public sealed class MarkerInfo
    {
        /// <summary>"v2", "v1" (historical Lazy CRUD header) or "foreign" (an auto-generated block of another tool).</summary>
        public string Version { get; set; }
        public bool IsV2 { get { return Version == "v2"; } }
        public bool IsV1 { get { return Version == "v1"; } }
        public MarkerBlock Block { get; set; }
        public Dictionary<string, string> Values { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public List<string> Problems { get; } = new List<string>();

        public string Get(string key) { string v; return Values.TryGetValue(key, out v) ? v : null; }
        public Ownership? Ownership
        {
            get { Ownership o; return OwnershipNames.TryParse(Get("Ownership"), out o) ? o : (Ownership?)null; }
        }
    }

    /// <summary>
    /// Emits and parses the frozen marker header v2 (contract F5). The header is written by the
    /// shared writer for GeneratorOwned and GeneratedOnce files, never hand-written by templates,
    /// and is excluded from the content hash.
    /// </summary>
    public static class GeneratedCodeMarker
    {
        public const string OpenTag = "<auto-generated>";
        public const string CloseTag = "</auto-generated>";
        public const string VersionKey = "LazyCrud-Marker";

        public const string GeneratorOwnedPolicy =
            "Regenerated on every run. Do not edit this file: direct edits are overwritten by the next generation.";
        public const string GeneratedOncePolicy =
            "Scaffolded once and now user-owned. This file will not be regenerated; your edits are preserved.";

        /// <summary>Renders the marker for the given fields and syntax. Returns null for unmarkable syntaxes.</summary>
        public static string Render(MarkerFields fields, CommentSyntax syntax)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));
            if (syntax == null) return null;
            if (fields.Ownership == Ownership.Manual) throw new InvalidOperationException("Manual files are never marked.");

            var lines = new List<string>();
            lines.Add(OpenTag);
            lines.Add(VersionKey + ": " + MarkerFields.MarkerVersion);
            lines.Add("Generator: " + (fields.Generator ?? MarkerFields.GeneratorIdentity));
            lines.Add("Aggregate: " + (fields.Aggregate ?? string.Empty));
            lines.Add("Layer: " + (fields.Layer ?? string.Empty));
            lines.Add("TemplateId: " + (fields.TemplateId ?? string.Empty));
            lines.Add("Ownership: " + OwnershipNames.ToName(fields.Ownership));
            lines.Add("GeneratorVersion: " + (fields.GeneratorVersion ?? string.Empty));
            if (!string.IsNullOrEmpty(fields.TemplateVersion)) lines.Add("TemplateVersion: " + fields.TemplateVersion);
            if (!string.IsNullOrEmpty(fields.SourceTemplate)) lines.Add("SourceTemplate: " + fields.SourceTemplate);
            if (fields.Ownership == Ownership.GeneratorOwned)
            {
                lines.Add("Policy: " + GeneratorOwnedPolicy);
                var where = string.IsNullOrEmpty(fields.SourceTemplate) ? "the template" : "the template (" + fields.SourceTemplate + ")";
                lines.Add("Customize: change " + where + ", the entity model, or a user-owned partial/extension file instead of this file.");
            }
            else
            {
                lines.Add("Policy: " + GeneratedOncePolicy);
                lines.Add("Customize: edit this file freely; delete it if you want the generator to scaffold it again.");
            }
            lines.Add(CloseTag);

            var sb = new StringBuilder();
            if (syntax.IsBlock) sb.Append(syntax.Open).Append("\r\n");
            foreach (var l in lines) sb.Append(syntax.LinePrefix).Append(l).Append("\r\n");
            if (syntax.IsBlock) sb.Append(syntax.Close).Append("\r\n");
            return sb.ToString();
        }

        /// <summary>
        /// Locates the leading marker block: an auto-generated block starting on the first line
        /// (optionally preceded by a byte-order mark or a comment-opener line). Deterministic for
        /// every supported comment syntax.
        /// </summary>
        public static bool TryFindMarkerBlock(string content, out MarkerBlock block)
        {
            block = null;
            if (string.IsNullOrEmpty(content)) return false;
            var lines = SplitLines(content);
            if (lines.Count == 0) return false;

            int openIndex = -1;
            for (int i = 0; i < Math.Min(2, lines.Count); i++)
            {
                var stripped = StripDecorations(lines[i].Text);
                if (string.Equals(stripped, OpenTag, StringComparison.OrdinalIgnoreCase)) { openIndex = i; break; }
                if (i == 0 && !IsBareOpener(stripped, lines[i].Text)) return false;
            }
            if (openIndex < 0) return false;

            int startLine = openIndex;
            if (openIndex == 1 && IsBareOpener(StripDecorations(lines[0].Text), lines[0].Text)) startLine = 0;

            int closeIndex = -1;
            for (int i = openIndex + 1; i < Math.Min(lines.Count, openIndex + 80); i++)
            {
                var stripped = StripDecorations(lines[i].Text);
                if (string.Equals(stripped, CloseTag, StringComparison.OrdinalIgnoreCase)) { closeIndex = i; break; }
            }
            if (closeIndex < 0) return false;

            int endLine = closeIndex;
            if (closeIndex + 1 < lines.Count && IsBareCloser(lines[closeIndex + 1].Text)) endLine = closeIndex + 1;

            int start = lines[startLine].Start;
            if (start == 1 && content[0] == '﻿') start = 0;
            int bodyStart = lines[endLine].End;
            block = new MarkerBlock { Start = start, BodyStart = bodyStart, Text = content.Substring(start, bodyStart - start) };
            return true;
        }

        /// <summary>Parses the marker in a file's opening lines without reading the rest of the file.</summary>
        public static bool TryParse(string content, out MarkerInfo info)
        {
            info = null;
            MarkerBlock block;
            if (!TryFindMarkerBlock(content, out block)) return false;
            info = new MarkerInfo { Block = block, Version = "foreign" };
            foreach (var raw in SplitLines(block.Text))
            {
                var line = StripDecorations(raw.Text);
                if (line.Length == 0) continue;
                if (string.Equals(line, OpenTag, StringComparison.OrdinalIgnoreCase) || string.Equals(line, CloseTag, StringComparison.OrdinalIgnoreCase)) continue;
                var idx = line.IndexOf(':');
                if (idx > 0)
                {
                    var key = line.Substring(0, idx).Trim();
                    var value = line.Substring(idx + 1).Trim();
                    if (!info.Values.ContainsKey(key)) info.Values[key] = value;
                }
            }
            string version;
            if (info.Values.TryGetValue(VersionKey, out version) && string.Equals(version, MarkerFields.MarkerVersion, StringComparison.OrdinalIgnoreCase))
            {
                info.Version = "v2";
                foreach (var required in new[] { "Generator", "Aggregate", "Layer", "TemplateId", "Ownership", "GeneratorVersion" })
                {
                    string v;
                    if (!info.Values.TryGetValue(required, out v) || string.IsNullOrEmpty(v))
                        info.Problems.Add("missing field " + required);
                }
                if (info.Ownership == null) info.Problems.Add("invalid Ownership value");
                else if (info.Ownership == Platform.Ownership.Manual) info.Problems.Add("Manual files are never marked");
            }
            else
            {
                var text = block.Text;
                if (text.IndexOf("LazyCrud", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    text.IndexOf("Lazy CRUD", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    text.IndexOf("T4 template", StringComparison.OrdinalIgnoreCase) >= 0)
                    info.Version = "v1";
            }
            return true;
        }

        private static bool IsBareOpener(string stripped, string raw)
        {
            var t = raw.Trim();
            if (t == "/*" || t == "@*" || t == "<#/*" || t == "<!--" || t == "/**") return true;
            return stripped.Length == 0 && t.Length > 0;
        }

        private static bool IsBareCloser(string raw)
        {
            var t = raw.Trim();
            return t == "*/" || t == "*@" || t == "*/#>" || t == "-->" || t == "**/";
        }

        /// <summary>Removes comment decorations from a marker line so keys and tags can be read.</summary>
        public static string StripDecorations(string line)
        {
            if (line == null) return string.Empty;
            var t = line.Trim();
            if (t.Length > 0 && t[0] == '﻿') t = t.Substring(1).Trim();
            foreach (var suffix in new[] { "*/#>", "*/", "*@", "-->" })
                if (t.EndsWith(suffix, StringComparison.Ordinal)) { t = t.Substring(0, t.Length - suffix.Length).Trim(); break; }
            foreach (var prefix in new[] { "<#/*", "///", "//", "/*", "@*", "<!--", "REM ", "rem ", "::", "--", "#", "*", "'" })
                if (t.StartsWith(prefix, StringComparison.Ordinal)) { t = t.Substring(prefix.Length).Trim(); break; }
            return t;
        }

        private struct Line
        {
            public int Start; public int End; public string Text;
        }

        private static List<Line> SplitLines(string content)
        {
            var result = new List<Line>();
            int pos = 0;
            while (pos < content.Length)
            {
                int nl = content.IndexOf('\n', pos);
                int end = nl < 0 ? content.Length : nl + 1;
                int textEnd = nl < 0 ? content.Length : nl;
                if (textEnd > pos && content[textEnd - 1] == '\r') textEnd--;
                result.Add(new Line { Start = pos, End = end, Text = content.Substring(pos, textEnd - pos) });
                pos = end;
                if (result.Count > 200) break;
            }
            return result;
        }
    }
}
