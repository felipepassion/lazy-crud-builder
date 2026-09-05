namespace Lazy.Crud.Builder.Platform.Tool.VisualStudio;

/// <summary>
/// One discovered Visual Studio installation together with the T4 tooling it provides.
/// </summary>
public sealed class VsInstance
{
    public string InstanceId { get; init; } = "";
    public string InstallationPath { get; init; } = "";
    public string InstallationVersion { get; init; } = "";
    public string DisplayName { get; init; } = "";
    public string ProductId { get; init; } = "";
    public string ChannelId { get; init; } = "";
    public bool IsPrerelease { get; init; }
    public bool IsComplete { get; init; }
    public bool IsLaunchable { get; init; }
    public string DevEnvPath { get; init; } = "";
    public string TextTransformPath { get; init; } = "";
    public bool HasTextTemplatingComponent { get; init; }
    /// <summary>"vswhere", "setup-configuration-api" or "override".</summary>
    public string DiscoverySource { get; init; } = "";

    public Version ParsedVersion
    {
        get
        {
            return Version.TryParse(InstallationVersion, out var v) ? v : new Version(0, 0);
        }
    }

    public int MajorVersion => ParsedVersion.Major;

    /// <summary>Channel label used in reports: "release" or "prerelease".</summary>
    public string Channel => IsPrerelease ? "prerelease" : "release";

    public bool DevEnvExists => !string.IsNullOrEmpty(DevEnvPath) && File.Exists(DevEnvPath);
    public bool TextTransformExists => !string.IsNullOrEmpty(TextTransformPath) && File.Exists(TextTransformPath);

    /// <summary>
    /// True when this instance satisfies the platform prerequisite: complete, launchable,
    /// devenv.exe present and the T4 text-templating tooling present.
    /// </summary>
    public bool SatisfiesPrerequisite => IsComplete && IsLaunchable && DevEnvExists && TextTransformExists && HasTextTemplatingComponent;

    public string Describe()
    {
        return $"{DisplayName} [{InstallationVersion}, {Channel}, {ProductId}] at '{InstallationPath}' (id {InstanceId}, via {DiscoverySource})";
    }
}
