using System;
using System.Security.Cryptography;
using System.Text;
using Lazy.Crud.Builder.Platform.Markers;

namespace Lazy.Crud.Builder.Platform.Hashing
{
    /// <summary>The identifiers recorded in a manifest entry's <c>hashScope</c>.</summary>
    public static class HashScope
    {
        /// <summary>SHA-256 over the LF-normalized body after the leading Lazy CRUD marker block was removed.</summary>
        public const string BodyLfMarkerExcluded = "body-lf-marker-excluded";

        /// <summary>SHA-256 over the whole LF-normalized content; used for file types that carry no marker.</summary>
        public const string BodyLf = "body-lf";
    }

    /// <summary>The single frozen hash implementation shared by generation, drift detection, manifests and the harness.</summary>
    public sealed class ContentHash
    {
        public ContentHash(string value, string scope)
        {
            Value = value;
            Scope = scope;
        }

        /// <summary>Lowercase hexadecimal SHA-256.</summary>
        public string Value { get; }
        /// <summary>One of the <see cref="HashScope"/> identifiers.</summary>
        public string Scope { get; }
        public string Algorithm { get { return ContentHasher.AlgorithmName; } }
    }

    /// <summary>
    /// Frozen content hash semantics (contract F3): SHA-256 over the file body with line endings
    /// normalized to LF, excluding the leading Lazy CRUD marker block. Files that carry no marker
    /// (either an unmarkable type or no block present) hash their whole normalized body.
    /// </summary>
    public static class ContentHasher
    {
        public const string AlgorithmName = "SHA-256";

        /// <summary>Computes the frozen hash for file content, excluding a leading marker block if present.</summary>
        public static ContentHash Compute(string content)
        {
            if (content == null) content = string.Empty;
            MarkerBlock block;
            string body;
            string scope;
            if (GeneratedCodeMarker.TryFindMarkerBlock(content, out block))
            {
                body = content.Substring(block.BodyStart);
                scope = HashScope.BodyLfMarkerExcluded;
            }
            else
            {
                body = content;
                scope = HashScope.BodyLf;
            }
            return new ContentHash(HashNormalized(body), scope);
        }

        /// <summary>Computes the frozen hash of a body that is known to contain no marker (whole-body scope).</summary>
        public static ContentHash ComputeBody(string body, bool markerExcluded)
        {
            return new ContentHash(HashNormalized(body ?? string.Empty), markerExcluded ? HashScope.BodyLfMarkerExcluded : HashScope.BodyLf);
        }

        /// <summary>Normalizes CRLF and lone CR to LF and strips a leading byte-order mark.</summary>
        public static string NormalizeLineEndings(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            if (text[0] == '﻿') text = text.Substring(1);
            if (text.IndexOf('\r') < 0) return text;
            var sb = new StringBuilder(text.Length);
            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (c == '\r')
                {
                    sb.Append('\n');
                    if (i + 1 < text.Length && text[i + 1] == '\n') i++;
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private static string HashNormalized(string body)
        {
            var normalized = NormalizeLineEndings(body);
            var bytes = new UTF8Encoding(false).GetBytes(normalized);
            using (var sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(bytes);
                var sb = new StringBuilder(hash.Length * 2);
                foreach (var b in hash) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }

        public static bool HashEquals(string a, string b)
        {
            return !string.IsNullOrEmpty(a) && !string.IsNullOrEmpty(b) && string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }
    }
}
