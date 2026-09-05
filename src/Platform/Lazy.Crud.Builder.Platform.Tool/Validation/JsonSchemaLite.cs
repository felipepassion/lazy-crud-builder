using System.Text.Json;
using System.Text.RegularExpressions;

namespace Lazy.Crud.Builder.Platform.Tool.Validation;

/// <summary>
/// A compact JSON Schema (draft 2020-12 subset) validator sufficient for the published contract
/// schemas: type, properties, required, additionalProperties, items, enum, const, minimum,
/// minLength, pattern, anyOf, oneOf, allOf, $ref to #/$defs.
/// </summary>
public sealed class JsonSchemaLite
{
    private readonly JsonElement _root;

    public JsonSchemaLite(string schemaJson)
    {
        _root = JsonDocument.Parse(schemaJson).RootElement.Clone();
    }

    public List<string> Validate(string documentJson)
    {
        var doc = JsonDocument.Parse(documentJson).RootElement.Clone();
        var errors = new List<string>();
        Validate(_root, doc, "$", errors);
        return errors;
    }

    private void Validate(JsonElement schema, JsonElement value, string path, List<string> errors)
    {
        if (schema.ValueKind == JsonValueKind.True) return;
        if (schema.ValueKind == JsonValueKind.False) { errors.Add(path + ": schema forbids any value"); return; }
        if (schema.TryGetProperty("$ref", out var refEl))
        {
            var target = Resolve(refEl.GetString()!);
            Validate(target, value, path, errors);
            return;
        }
        if (schema.TryGetProperty("type", out var typeEl))
        {
            var allowed = typeEl.ValueKind == JsonValueKind.Array ? typeEl.EnumerateArray().Select(t => t.GetString()!).ToList() : new List<string> { typeEl.GetString()! };
            if (!allowed.Any(t => Matches(t, value))) { errors.Add(path + ": expected type " + string.Join("|", allowed) + " but found " + Kind(value)); return; }
        }
        if (schema.TryGetProperty("enum", out var enumEl))
        {
            if (!enumEl.EnumerateArray().Any(e => JsonEquals(e, value))) errors.Add(path + ": value " + value.GetRawText() + " is not one of " + enumEl.GetRawText());
        }
        if (schema.TryGetProperty("const", out var constEl) && !JsonEquals(constEl, value)) errors.Add(path + ": expected const " + constEl.GetRawText());
        if (value.ValueKind == JsonValueKind.Number)
        {
            if (schema.TryGetProperty("minimum", out var min) && value.GetDouble() < min.GetDouble()) errors.Add(path + ": below minimum " + min.GetRawText());
            if (schema.TryGetProperty("maximum", out var max) && value.GetDouble() > max.GetDouble()) errors.Add(path + ": above maximum " + max.GetRawText());
        }
        if (value.ValueKind == JsonValueKind.String)
        {
            var s = value.GetString() ?? "";
            if (schema.TryGetProperty("minLength", out var minLen) && s.Length < minLen.GetInt32()) errors.Add(path + ": shorter than minLength " + minLen.GetInt32());
            if (schema.TryGetProperty("pattern", out var pattern) && !Regex.IsMatch(s, pattern.GetString()!)) errors.Add(path + ": value '" + s + "' does not match pattern " + pattern.GetString());
        }
        if (value.ValueKind == JsonValueKind.Object)
        {
            var props = schema.TryGetProperty("properties", out var p) ? p : default;
            if (schema.TryGetProperty("required", out var req))
                foreach (var r in req.EnumerateArray())
                    if (!value.TryGetProperty(r.GetString()!, out _)) errors.Add(path + ": missing required property '" + r.GetString() + "'");
            var additional = schema.TryGetProperty("additionalProperties", out var ap) ? ap : default;
            foreach (var member in value.EnumerateObject())
            {
                if (props.ValueKind == JsonValueKind.Object && props.TryGetProperty(member.Name, out var propSchema))
                    Validate(propSchema, member.Value, path + "." + member.Name, errors);
                else if (additional.ValueKind == JsonValueKind.False)
                    errors.Add(path + ": unexpected property '" + member.Name + "'");
                else if (additional.ValueKind == JsonValueKind.Object)
                    Validate(additional, member.Value, path + "." + member.Name, errors);
            }
        }
        if (value.ValueKind == JsonValueKind.Array && schema.TryGetProperty("items", out var items))
        {
            int i = 0;
            foreach (var item in value.EnumerateArray()) Validate(items, item, path + "[" + i++ + "]", errors);
            if (schema.TryGetProperty("minItems", out var minItems) && i < minItems.GetInt32()) errors.Add(path + ": fewer than minItems " + minItems.GetInt32());
        }
        if (schema.TryGetProperty("allOf", out var allOf))
            foreach (var sub in allOf.EnumerateArray()) Validate(sub, value, path, errors);
        if (schema.TryGetProperty("anyOf", out var anyOf))
        {
            var ok = anyOf.EnumerateArray().Any(sub => { var e = new List<string>(); Validate(sub, value, path, e); return e.Count == 0; });
            if (!ok) errors.Add(path + ": matches none of anyOf");
        }
        if (schema.TryGetProperty("oneOf", out var oneOf))
        {
            var count = oneOf.EnumerateArray().Count(sub => { var e = new List<string>(); Validate(sub, value, path, e); return e.Count == 0; });
            if (count != 1) errors.Add(path + ": matches " + count + " of oneOf (exactly one required)");
        }
    }

    private JsonElement Resolve(string reference)
    {
        if (!reference.StartsWith("#/", StringComparison.Ordinal)) throw new NotSupportedException("only local $ref is supported: " + reference);
        var el = _root;
        foreach (var segment in reference.Substring(2).Split('/'))
            el = el.GetProperty(segment.Replace("~1", "/").Replace("~0", "~"));
        return el;
    }

    private static bool Matches(string type, JsonElement v)
    {
        switch (type)
        {
            case "object": return v.ValueKind == JsonValueKind.Object;
            case "array": return v.ValueKind == JsonValueKind.Array;
            case "string": return v.ValueKind == JsonValueKind.String;
            case "number": return v.ValueKind == JsonValueKind.Number;
            case "integer": return v.ValueKind == JsonValueKind.Number && v.TryGetInt64(out _);
            case "boolean": return v.ValueKind == JsonValueKind.True || v.ValueKind == JsonValueKind.False;
            case "null": return v.ValueKind == JsonValueKind.Null;
            default: return false;
        }
    }

    private static string Kind(JsonElement v) => v.ValueKind switch
    {
        JsonValueKind.True or JsonValueKind.False => "boolean",
        JsonValueKind.Number => "number",
        JsonValueKind.String => "string",
        JsonValueKind.Object => "object",
        JsonValueKind.Array => "array",
        JsonValueKind.Null => "null",
        _ => v.ValueKind.ToString(),
    };

    private static bool JsonEquals(JsonElement a, JsonElement b)
    {
        if (a.ValueKind != b.ValueKind)
        {
            if (a.ValueKind is JsonValueKind.True or JsonValueKind.False && b.ValueKind is JsonValueKind.True or JsonValueKind.False) return a.ValueKind == b.ValueKind;
            return false;
        }
        return a.GetRawText() == b.GetRawText() || (a.ValueKind == JsonValueKind.String && a.GetString() == b.GetString());
    }
}
