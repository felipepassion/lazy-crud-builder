using System;
using System.Collections.Generic;
using Lazy.Crud.Builder.Platform.Hashing;
using Lazy.Crud.Builder.Platform.Markers;

namespace Lazy.Crud.Builder.Platform.Writer
{
    /// <summary>
    /// The frozen rule that decides whether a generated body carries content at all.
    ///
    /// A transformation that loses its model emits the template's static preamble and stops, so the
    /// file it would write is syntactically well formed and semantically empty: imports, blank lines
    /// and comments, and no declaration. Hashing cannot tell that apart from a legitimate change, so
    /// the writer needs its own notion of "this body says nothing" before it overwrites a file that
    /// said something. That notion is frozen here, in one place, because the diagnostic it produces
    /// (<see cref="Diagnostics.DiagnosticCodes.OutputCollapsed"/>) is a published contract.
    ///
    /// A line is <b>significant</b> unless it is blank, a comment, or an import or preprocessor
    /// directive. The rule is deliberately conservative: anything it cannot classify counts as
    /// significant, so a body is only ever called contentless when every one of its lines is
    /// demonstrably not content.
    /// </summary>
    public static class BodySignificance
    {
        /// <summary>Identifier of this rule, recorded in the diagnostic so a consumer can pin it.</summary>
        public const string RuleId = "significant-lines-v1";

        /// <summary>Prefixes that make a line an import or preprocessor directive rather than content.</summary>
        private static readonly string[] DirectivePrefixes =
        {
            "using ", "using\t", "global using ", "global using\t", "extern alias ", "@using ", "#",
        };

        /// <summary>
        /// Counts the lines of <paramref name="body"/> that carry content, using the comment syntax of
        /// the file the body is written to (null when the file type carries no marker).
        /// </summary>
        public static int CountSignificantLines(string body, CommentSyntax syntax)
        {
            var count = 0;
            foreach (var line in Lines(body))
            {
                if (IsSignificant(line, syntax)) count++;
            }
            return count;
        }

        /// <summary>True when no line of the body carries content.</summary>
        public static bool IsContentless(string body, CommentSyntax syntax)
        {
            return CountSignificantLines(body, syntax) == 0;
        }

        /// <summary>
        /// Why a body is contentless, for the diagnostic: <c>empty</c> (nothing at all),
        /// <c>whitespace-only</c>, or <c>no-significant-content</c> (only comments and directives).
        /// </summary>
        public static string Reason(string body)
        {
            if (string.IsNullOrEmpty(body)) return "empty";
            return string.IsNullOrWhiteSpace(ContentHasher.NormalizeLineEndings(body)) ? "whitespace-only" : "no-significant-content";
        }

        /// <summary>The body of a file on disk: its content with a leading Lazy CRUD marker block removed.</summary>
        public static string BodyOf(string fileContent)
        {
            if (string.IsNullOrEmpty(fileContent)) return string.Empty;
            MarkerBlock block;
            if (GeneratedCodeMarker.TryFindMarkerBlock(fileContent, out block) && block != null)
                return fileContent.Substring(block.BodyStart);
            return fileContent;
        }

        private static IEnumerable<string> Lines(string body)
        {
            var normalized = ContentHasher.NormalizeLineEndings(body ?? string.Empty);
            return normalized.Split('\n');
        }

        private static bool IsSignificant(string line, CommentSyntax syntax)
        {
            var text = (line ?? string.Empty).Trim();
            if (text.Length == 0) return false;
            if (IsComment(text, syntax)) return false;
            foreach (var prefix in DirectivePrefixes)
            {
                if (text.StartsWith(prefix, StringComparison.Ordinal)) return false;
            }
            return true;
        }

        /// <summary>
        /// A whole-line comment. Only the syntaxes the marker itself can emit are recognized, plus the
        /// interior and delimiters of a block comment, so that a body written entirely as a comment
        /// block is not mistaken for content.
        /// </summary>
        private static bool IsComment(string text, CommentSyntax syntax)
        {
            if (text.StartsWith("//", StringComparison.Ordinal)) return true;
            if (syntax == null) return false;
            // A one-character prefix is ambiguous with real content (the block-comment interior "*"
            // is also the CSS universal selector), so only unambiguous prefixes are honoured.
            var prefix = (syntax.LinePrefix ?? string.Empty).Trim();
            if (prefix.Length > 1 && text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) return true;
            if (syntax.IsBlock)
            {
                var open = (syntax.Open ?? string.Empty).Trim();
                var close = (syntax.Close ?? string.Empty).Trim();
                if (open.Length > 0 && text.StartsWith(open, StringComparison.Ordinal)) return true;
                if (close.Length > 0 && text.EndsWith(close, StringComparison.Ordinal)) return true;
            }
            return false;
        }
    }
}
