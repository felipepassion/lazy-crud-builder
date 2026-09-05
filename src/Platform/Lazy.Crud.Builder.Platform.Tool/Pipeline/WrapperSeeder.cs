using System.Text;

namespace Lazy.Crud.Builder.Platform.Tool.Pipeline;

/// <summary>
/// Seeds ("ensures") the wrapper templates of an aggregate before every transformation. The shared
/// project.tt deletes the DefaultEntities/DefaultCommands wrappers after they run (self-cleanup),
/// so a regeneration must re-create them; this mirrors the IDE extension's EnsureTemplatesForProject.
/// </summary>
public static class WrapperSeeder
{
    private static readonly UTF8Encoding Utf8Bom = new(true);

    /// <summary>Writes every wrapper for the given project directory. Returns the wrapper paths (existing or created).</summary>
    public static IReadOnlyList<string> EnsureLayer(string projectDirectory, LayerSpec layer, TemplateSource source, Action<string>? log = null)
    {
        var result = new List<string>();
        foreach (var w in layer.Wrappers)
        {
            result.Add(Ensure(projectDirectory, w, source, log));
        }
        return result;
    }

    public static string EnsureTestLayer(string projectDirectory, TestLayerSpec layer, TemplateSource source, Action<string>? log = null)
    {
        return Ensure(projectDirectory, layer.Wrapper, source, log);
    }

    private static string Ensure(string projectDirectory, WrapperSpec wrapper, TemplateSource source, Action<string>? log)
    {
        var path = Path.Combine(projectDirectory, wrapper.RelativeFile.Replace('/', Path.DirectorySeparatorChar));
        var content = source.IncludeDirectiveFor(wrapper.SourceTemplate) + "\r\n";
        var dir = Path.GetDirectoryName(path)!;
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        if (!File.Exists(path) || File.ReadAllText(path) != content)
        {
            File.WriteAllText(path, content, Utf8Bom);
            log?.Invoke($"wrapper seeded: {path}");
        }
        return path;
    }
}
