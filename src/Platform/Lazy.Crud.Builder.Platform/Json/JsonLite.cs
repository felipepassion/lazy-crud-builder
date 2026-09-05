using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Lazy.Crud.Builder.Platform.Json
{
    /// <summary>
    /// A dependency-free JSON reader/writer (netstandard2.0, loadable inside the Visual Studio T4
    /// host). Objects are <see cref="Dictionary{TKey,TValue}"/> (insertion-ordered via
    /// <see cref="JsonObject"/>), arrays are <see cref="List{T}"/>, numbers are <c>double</c> or
    /// <c>long</c>, and strings, booleans and null map to their CLR equivalents.
    /// </summary>
    public static class JsonLite
    {
        public static object Parse(string json)
        {
            if (json == null) throw new ArgumentNullException(nameof(json));
            var p = new Parser(json);
            p.SkipWhitespace();
            var value = p.ReadValue();
            p.SkipWhitespace();
            if (!p.AtEnd) throw p.Error("unexpected trailing content");
            return value;
        }

        public static JsonObject ParseObject(string json)
        {
            var o = Parse(json) as JsonObject;
            if (o == null) throw new FormatException("JSON document is not an object");
            return o;
        }

        public static string Write(object value, bool indented = true)
        {
            var sb = new StringBuilder();
            WriteValue(sb, value, indented, 0);
            if (indented) sb.Append('\n');
            return sb.ToString();
        }

        private static void WriteValue(StringBuilder sb, object value, bool indented, int depth)
        {
            if (value == null) { sb.Append("null"); return; }
            if (value is string) { WriteString(sb, (string)value); return; }
            if (value is bool) { sb.Append((bool)value ? "true" : "false"); return; }
            if (value is int || value is long || value is short || value is byte)
            { sb.Append(Convert.ToInt64(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture)); return; }
            if (value is double || value is float || value is decimal)
            { sb.Append(Convert.ToDouble(value, CultureInfo.InvariantCulture).ToString("R", CultureInfo.InvariantCulture)); return; }
            if (value is DateTime) { WriteString(sb, ((DateTime)value).ToUniversalTime().ToString("o", CultureInfo.InvariantCulture)); return; }
            if (value is Enum) { WriteString(sb, value.ToString()); return; }
            var obj = value as JsonObject;
            if (obj != null) { WriteObject(sb, obj, indented, depth); return; }
            var dict = value as IDictionary<string, object>;
            if (dict != null) { WriteObject(sb, new JsonObject(dict), indented, depth); return; }
            var sdict = value as IDictionary<string, string>;
            if (sdict != null)
            {
                var o = new JsonObject();
                foreach (var kv in sdict) o[kv.Key] = kv.Value;
                WriteObject(sb, o, indented, depth);
                return;
            }
            var seq = value as System.Collections.IEnumerable;
            if (seq != null)
            {
                sb.Append('[');
                bool first = true;
                foreach (var item in seq)
                {
                    if (!first) sb.Append(',');
                    first = false;
                    if (indented) { sb.Append('\n'); Indent(sb, depth + 1); }
                    WriteValue(sb, item, indented, depth + 1);
                }
                if (!first && indented) { sb.Append('\n'); Indent(sb, depth); }
                sb.Append(']');
                return;
            }
            WriteString(sb, Convert.ToString(value, CultureInfo.InvariantCulture));
        }

        private static void WriteObject(StringBuilder sb, JsonObject obj, bool indented, int depth)
        {
            sb.Append('{');
            bool first = true;
            foreach (var key in obj.Keys)
            {
                if (!first) sb.Append(',');
                first = false;
                if (indented) { sb.Append('\n'); Indent(sb, depth + 1); }
                WriteString(sb, key);
                sb.Append(indented ? ": " : ":");
                WriteValue(sb, obj[key], indented, depth + 1);
            }
            if (!first && indented) { sb.Append('\n'); Indent(sb, depth); }
            sb.Append('}');
        }

        private static void Indent(StringBuilder sb, int depth)
        {
            for (int i = 0; i < depth; i++) sb.Append("  ");
        }

        private static void WriteString(StringBuilder sb, string s)
        {
            sb.Append('"');
            foreach (var c in s ?? string.Empty)
            {
                switch (c)
                {
                    case '"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    case '\b': sb.Append("\\b"); break;
                    case '\f': sb.Append("\\f"); break;
                    default:
                        if (c < 0x20) sb.Append("\\u").Append(((int)c).ToString("x4"));
                        else sb.Append(c);
                        break;
                }
            }
            sb.Append('"');
        }

        private sealed class Parser
        {
            private readonly string _s;
            private int _i;
            public Parser(string s) { _s = s; }
            public bool AtEnd { get { return _i >= _s.Length; } }

            public FormatException Error(string message)
            {
                return new FormatException("Invalid JSON at position " + _i + ": " + message);
            }

            public void SkipWhitespace()
            {
                while (_i < _s.Length && char.IsWhiteSpace(_s[_i])) _i++;
                if (_i == 0 && _s.Length > 0 && _s[0] == '﻿') { _i++; SkipWhitespace(); }
            }

            public object ReadValue()
            {
                SkipWhitespace();
                if (AtEnd) throw Error("unexpected end");
                var c = _s[_i];
                if (c == '{') return ReadObject();
                if (c == '[') return ReadArray();
                if (c == '"') return ReadString();
                if (c == 't') { Expect("true"); return true; }
                if (c == 'f') { Expect("false"); return false; }
                if (c == 'n') { Expect("null"); return null; }
                return ReadNumber();
            }

            private void Expect(string word)
            {
                if (string.CompareOrdinal(_s, _i, word, 0, word.Length) != 0) throw Error("expected " + word);
                _i += word.Length;
            }

            private JsonObject ReadObject()
            {
                var obj = new JsonObject();
                _i++;
                SkipWhitespace();
                if (_i < _s.Length && _s[_i] == '}') { _i++; return obj; }
                while (true)
                {
                    SkipWhitespace();
                    if (AtEnd || _s[_i] != '"') throw Error("expected property name");
                    var key = ReadString();
                    SkipWhitespace();
                    if (AtEnd || _s[_i] != ':') throw Error("expected ':'");
                    _i++;
                    obj[key] = ReadValue();
                    SkipWhitespace();
                    if (AtEnd) throw Error("unterminated object");
                    if (_s[_i] == ',') { _i++; continue; }
                    if (_s[_i] == '}') { _i++; return obj; }
                    throw Error("expected ',' or '}'");
                }
            }

            private List<object> ReadArray()
            {
                var list = new List<object>();
                _i++;
                SkipWhitespace();
                if (_i < _s.Length && _s[_i] == ']') { _i++; return list; }
                while (true)
                {
                    list.Add(ReadValue());
                    SkipWhitespace();
                    if (AtEnd) throw Error("unterminated array");
                    if (_s[_i] == ',') { _i++; continue; }
                    if (_s[_i] == ']') { _i++; return list; }
                    throw Error("expected ',' or ']'");
                }
            }

            private string ReadString()
            {
                _i++;
                var sb = new StringBuilder();
                while (true)
                {
                    if (AtEnd) throw Error("unterminated string");
                    var c = _s[_i++];
                    if (c == '"') return sb.ToString();
                    if (c != '\\') { sb.Append(c); continue; }
                    if (AtEnd) throw Error("unterminated escape");
                    var e = _s[_i++];
                    switch (e)
                    {
                        case '"': sb.Append('"'); break;
                        case '\\': sb.Append('\\'); break;
                        case '/': sb.Append('/'); break;
                        case 'b': sb.Append('\b'); break;
                        case 'f': sb.Append('\f'); break;
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case 't': sb.Append('\t'); break;
                        case 'u':
                            if (_i + 4 > _s.Length) throw Error("invalid unicode escape");
                            sb.Append((char)int.Parse(_s.Substring(_i, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture));
                            _i += 4;
                            break;
                        default: throw Error("invalid escape");
                    }
                }
            }

            private object ReadNumber()
            {
                int start = _i;
                while (_i < _s.Length && ("+-0123456789.eE".IndexOf(_s[_i]) >= 0)) _i++;
                var text = _s.Substring(start, _i - start);
                if (text.Length == 0) throw Error("unexpected character");
                long l;
                if (text.IndexOf('.') < 0 && text.IndexOf('e') < 0 && text.IndexOf('E') < 0 && long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out l)) return l;
                double d;
                if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out d)) return d;
                throw Error("invalid number");
            }
        }
    }

    /// <summary>An insertion-ordered JSON object.</summary>
    public sealed class JsonObject : IDictionary<string, object>
    {
        private readonly List<string> _keys = new List<string>();
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>(StringComparer.Ordinal);

        public JsonObject() { }
        public JsonObject(IDictionary<string, object> source)
        {
            foreach (var kv in source) this[kv.Key] = kv.Value;
        }

        public object this[string key]
        {
            get { return _values[key]; }
            set { if (!_values.ContainsKey(key)) _keys.Add(key); _values[key] = value; }
        }

        public ICollection<string> Keys { get { return _keys.AsReadOnly(); } }
        public ICollection<object> Values { get { var l = new List<object>(); foreach (var k in _keys) l.Add(_values[k]); return l; } }
        public int Count { get { return _keys.Count; } }
        public bool IsReadOnly { get { return false; } }

        public void Add(string key, object value) { this[key] = value; }
        public void Add(KeyValuePair<string, object> item) { this[item.Key] = item.Value; }
        public void Clear() { _keys.Clear(); _values.Clear(); }
        public bool Contains(KeyValuePair<string, object> item) { object v; return _values.TryGetValue(item.Key, out v) && Equals(v, item.Value); }
        public bool ContainsKey(string key) { return _values.ContainsKey(key); }
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) { foreach (var kv in this) array[arrayIndex++] = kv; }
        public bool Remove(string key) { if (!_values.Remove(key)) return false; _keys.Remove(key); return true; }
        public bool Remove(KeyValuePair<string, object> item) { return Remove(item.Key); }
        public bool TryGetValue(string key, out object value) { return _values.TryGetValue(key, out value); }
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() { foreach (var k in _keys) yield return new KeyValuePair<string, object>(k, _values[k]); }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return GetEnumerator(); }

        // Typed helpers -----------------------------------------------------------------------

        public string GetString(string key)
        {
            object v;
            if (!_values.TryGetValue(key, out v) || v == null) return null;
            return v as string ?? Convert.ToString(v, CultureInfo.InvariantCulture);
        }

        public long? GetInteger(string key)
        {
            object v;
            if (!_values.TryGetValue(key, out v) || v == null) return null;
            if (v is long) return (long)v;
            if (v is double) return (long)(double)v;
            long l;
            if (v is string && long.TryParse((string)v, NumberStyles.Integer, CultureInfo.InvariantCulture, out l)) return l;
            return null;
        }

        public bool? GetBoolean(string key)
        {
            object v;
            if (!_values.TryGetValue(key, out v) || v == null) return null;
            if (v is bool) return (bool)v;
            bool b;
            if (v is string && bool.TryParse((string)v, out b)) return b;
            return null;
        }

        public JsonObject GetObject(string key)
        {
            object v;
            return _values.TryGetValue(key, out v) ? v as JsonObject : null;
        }

        public List<object> GetArray(string key)
        {
            object v;
            return _values.TryGetValue(key, out v) ? v as List<object> : null;
        }

        public List<string> GetStringArray(string key)
        {
            var arr = GetArray(key);
            if (arr == null) return null;
            var result = new List<string>();
            foreach (var item in arr) result.Add(item as string ?? Convert.ToString(item, CultureInfo.InvariantCulture));
            return result;
        }
    }
}
