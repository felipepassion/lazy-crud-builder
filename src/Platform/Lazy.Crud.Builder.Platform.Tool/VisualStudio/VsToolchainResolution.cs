namespace Lazy.Crud.Builder.Platform.Tool.VisualStudio;

/// <summary>
/// Result of resolving the Visual Studio T4 toolchain. Either <see cref="Selected"/> is set or
/// <see cref="FailureReason"/> explains why no instance satisfies the prerequisite.
/// </summary>
public sealed class VsToolchainResolution
{
    public VsInstance? Selected { get; init; }
    public IReadOnlyList<VsInstance> Candidates { get; init; } = Array.Empty<VsInstance>();
    public IReadOnlyList<string> Rejections { get; init; } = Array.Empty<string>();
    public IReadOnlyList<string> DiscoveryLog { get; init; } = Array.Empty<string>();
    public bool OverrideApplied { get; init; }
    public string? FailureReason { get; init; }
    public bool Succeeded => Selected != null;

    /// <summary>The component that must be installed for T4 tooling.</summary>
    public const string RequiredComponentId = "Microsoft.VisualStudio.Component.TextTemplating";
    public const string RequiredComponentDisplayName = "Text Template Transformation";

    public string Remedy =>
        "Install Visual Studio 2022 (17.x) or Visual Studio 2026 (18.x, release, preview or Insiders channel) " +
        "with the '" + RequiredComponentDisplayName + "' component (" + RequiredComponentId + "), " +
        "or point LAZYCRUD_VS_INSTANCE / --vs-instance at a valid installation path.";

    public Dictionary<string, object?> ToReport()
    {
        return new Dictionary<string, object?>
        {
            ["resolved"] = Succeeded,
            ["overrideApplied"] = OverrideApplied,
            ["instanceId"] = Selected?.InstanceId,
            ["displayName"] = Selected?.DisplayName,
            ["installationPath"] = Selected?.InstallationPath,
            ["installationVersion"] = Selected?.InstallationVersion,
            ["channel"] = Selected?.Channel,
            ["channelId"] = Selected?.ChannelId,
            ["productId"] = Selected?.ProductId,
            ["devenvPath"] = Selected?.DevEnvPath,
            ["textTransformPath"] = Selected?.TextTransformPath,
            ["discoverySource"] = Selected?.DiscoverySource,
            ["requiredComponent"] = RequiredComponentId,
            ["candidates"] = Candidates.Select(c => c.Describe()).ToArray(),
            ["rejections"] = Rejections,
            ["discoveryLog"] = DiscoveryLog,
            ["failureReason"] = FailureReason,
        };
    }
}
