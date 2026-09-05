namespace Lazy.Crud.Builder.Platform.Tool.Pipeline;

public enum TemplateSourceKind
{
    /// <summary>Templates resolved from installed NuGet packages (public-equivalent artifacts).</summary>
    NuGet = 0,
    /// <summary>Explicitly selected local development mode: templates from a source tree (src/ProjectFiles).</summary>
    Local = 1,
}

/// <summary>Where wrapper templates include the shipped templates from.</summary>
public sealed class TemplateSource
{
    public TemplateSourceKind Kind { get; init; }
    /// <summary>Local mode: the ProjectFiles directory. NuGet mode: informational (package cache root).</summary>
    public string Location { get; init; } = "";
    public string Version { get; init; } = "";
    /// <summary>NuGet mode: the templates package id requested by the consumer (may be a deprecated alias).</summary>
    public string RequestedPackageId { get; init; } = global::Lazy.Crud.Builder.Platform.Descriptor.TemplateRegistry.CanonicalTemplatesPackageId;

    public string Describe() => Kind == TemplateSourceKind.Local
        ? $"local development mode (source tree {Location})"
        : $"NuGet packages (version {Version}, templates package {RequestedPackageId})";

    /// <summary>Builds the include directive for a wrapper pointing at a shipped template.</summary>
    public string IncludeDirectiveFor(string sourceTemplate)
    {
        var relative = sourceTemplate.Replace('/', '\\');
        if (Kind == TemplateSourceKind.Local)
        {
            var path = Path.Combine(Location, relative);
            return "<#@ include file=\"" + path + "\" #>";
        }
        var package = AggregateLayout.PackageForTemplate(sourceTemplate);
        return "<#@ include file=\"" + AggregateLayout.PackagePathProperty(package) + "\\contentFiles\\any\\any\\LazyCrudTemplates\\" + relative + "\" #>";
    }
}
