using System.Diagnostics;
using System.Text.Json;

namespace Lazy.Crud.Builder.Platform.Tool.VisualStudio;

/// <summary>
/// Deterministic discovery of the Visual Studio T4 toolchain.
///
/// Mechanisms, in order:
///  1. An explicit override (LAZYCRUD_VS_INSTANCE environment variable or --vs-instance) naming an
///     installation path. It is validated, never trusted blindly, and reported as an override.
///  2. The Visual Studio Installer query tool, vswhere.exe, at its documented fixed location
///     (%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe), queried with
///     -all -prerelease -products * so side-by-side, preview and Insiders instances are included.
///  3. The Visual Studio Setup Configuration COM API (the same data vswhere reads).
///
/// No mechanism scans program-files directories or hardcodes an installation path.
///
/// Selection rule (documented in docs/toolchain.md): among instances that satisfy the
/// prerequisite, release-channel instances are preferred over prerelease instances; within a
/// channel the highest installation version wins; ties are broken by ordinal installation path.
/// </summary>
public static class VsInstanceLocator
{
    public const string OverrideEnvironmentVariable = "LAZYCRUD_VS_INSTANCE";

    /// <summary>
    /// When set to a non-empty value, discovery is skipped and reported as unresolvable. Used by
    /// the harness negative scenario to prove early failure without uninstalling anything.
    /// </summary>
    public const string SimulateUnavailableEnvironmentVariable = "LAZYCRUD_VS_SIMULATE_UNAVAILABLE";

    public static VsToolchainResolution Resolve(string? overridePath = null, Action<string>? log = null)
    {
        var discoveryLog = new List<string>();
        void Log(string m) { discoveryLog.Add(m); log?.Invoke(m); }

        var simulate = Environment.GetEnvironmentVariable(SimulateUnavailableEnvironmentVariable);
        if (!string.IsNullOrWhiteSpace(simulate))
        {
            Log($"{SimulateUnavailableEnvironmentVariable} is set: discovery is treated as finding no usable instance.");
            return new VsToolchainResolution
            {
                DiscoveryLog = discoveryLog,
                FailureReason = "No Visual Studio instance providing the T4 tooling is resolvable " +
                                $"({SimulateUnavailableEnvironmentVariable} is set; discovery mechanisms were bypassed).",
            };
        }

        overridePath ??= Environment.GetEnvironmentVariable(OverrideEnvironmentVariable);
        if (!string.IsNullOrWhiteSpace(overridePath))
        {
            Log($"override: {OverrideEnvironmentVariable}/--vs-instance = {overridePath}");
            var instance = ProbeInstallationPath(overridePath!, "override", "override", Log);
            if (instance == null)
            {
                return new VsToolchainResolution
                {
                    OverrideApplied = true,
                    DiscoveryLog = discoveryLog,
                    FailureReason = $"The override path {overridePath} is not a Visual Studio installation " +
                                    "(Common7/IDE/devenv.exe not found).",
                };
            }
            if (!instance.SatisfiesPrerequisite)
            {
                return new VsToolchainResolution
                {
                    OverrideApplied = true,
                    Candidates = new[] { instance },
                    Rejections = new[] { Reject(instance) },
                    DiscoveryLog = discoveryLog,
                    FailureReason = $"The override instance does not provide the T4 tooling: {Reject(instance)}",
                };
            }
            return new VsToolchainResolution { Selected = instance, Candidates = new[] { instance }, OverrideApplied = true, DiscoveryLog = discoveryLog };
        }

        if (!OperatingSystem.IsWindows())
        {
            return new VsToolchainResolution
            {
                DiscoveryLog = discoveryLog,
                FailureReason = "Visual Studio T4 tooling requires Windows; discovery was not attempted on this platform.",
            };
        }

        var candidates = new List<VsInstance>();

        var vswhere = VsWherePath();
        if (vswhere != null && File.Exists(vswhere))
        {
            Log($"vswhere: {vswhere}");
            try
            {
                candidates.AddRange(QueryVsWhere(vswhere, Log));
            }
            catch (Exception ex)
            {
                Log($"vswhere: query failed -> {ex.GetType().Name}: {ex.Message}");
            }
        }
        else
        {
            Log($"vswhere: not present at {vswhere ?? "<unknown>"}");
        }

        if (candidates.Count == 0)
        {
            Log("setup-configuration-api: querying ISetupConfiguration2.EnumAllInstances");
            try
            {
                candidates.AddRange(SetupConfigurationApi.EnumerateInstances(Log));
            }
            catch (Exception ex)
            {
                Log($"setup-configuration-api: failed -> {ex.GetType().Name}: {ex.Message}");
            }
        }

        var satisfying = candidates.Where(c => c.SatisfiesPrerequisite).ToList();
        var rejections = candidates.Where(c => !c.SatisfiesPrerequisite).Select(Reject).ToList();

        if (satisfying.Count == 0)
        {
            var reason = candidates.Count == 0
                ? "No Visual Studio instance was found by vswhere or the Setup Configuration API."
                : $"{candidates.Count} Visual Studio instance(s) found, none provides the T4 tooling.";
            return new VsToolchainResolution
            {
                Candidates = candidates,
                Rejections = rejections,
                DiscoveryLog = discoveryLog,
                FailureReason = reason,
            };
        }

        var selected = SelectDeterministically(satisfying);

        Log($"selected: {selected.Describe()}");
        return new VsToolchainResolution
        {
            Selected = selected,
            Candidates = candidates,
            Rejections = rejections,
            DiscoveryLog = discoveryLog,
        };
    }

    /// <summary>
    /// The documented selection rule: release before prerelease, then highest version, then
    /// ordinal installation path. Pure, so it can be unit-tested with synthetic instances.
    /// </summary>
    public static VsInstance SelectDeterministically(IEnumerable<VsInstance> satisfying)
    {
        return satisfying
            .OrderBy(c => c.IsPrerelease ? 1 : 0)
            .ThenByDescending(c => c.ParsedVersion)
            .ThenBy(c => c.InstallationPath, StringComparer.Ordinal)
            .First();
    }

    public static string Reject(VsInstance i)
    {
        var reasons = new List<string>();
        if (!i.IsComplete) reasons.Add("installation incomplete");
        if (!i.IsLaunchable) reasons.Add("not launchable");
        if (!i.DevEnvExists) reasons.Add("devenv.exe missing");
        if (!i.TextTransformExists) reasons.Add("Common7/IDE/TextTransform.exe missing");
        if (!i.HasTextTemplatingComponent) reasons.Add($"component {VsToolchainResolution.RequiredComponentId} not installed");
        return $"{i.Describe()}: {string.Join(", ", reasons)}";
    }

    /// <summary>
    /// Documented fixed location of the Visual Studio Installer. This is the installer, not a
    /// Visual Studio instance; instances are discovered through it.
    /// </summary>
    public static string? VsWherePath()
    {
        var pf86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
        if (string.IsNullOrEmpty(pf86)) pf86 = Environment.GetEnvironmentVariable("ProgramFiles");
        if (string.IsNullOrEmpty(pf86)) return null;
        return Path.Combine(pf86, "Microsoft Visual Studio", "Installer", "vswhere.exe");
    }

    private static IEnumerable<VsInstance> QueryVsWhere(string vswhere, Action<string> log)
    {
        var json = RunVsWhere(vswhere, "-all -prerelease -products * -format json -utf8");
        var withT4 = new HashSet<string>(
            RunVsWhere(vswhere, $"-all -prerelease -products * -requires {VsToolchainResolution.RequiredComponentId} -property instanceId")
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()),
            StringComparer.OrdinalIgnoreCase);

        using var doc = JsonDocument.Parse(json);
        foreach (var el in doc.RootElement.EnumerateArray())
        {
            var path = el.GetProperty("installationPath").GetString() ?? "";
            var id = Str(el, "instanceId");
            var productPath = Str(el, "productPath");
            var instance = new VsInstance
            {
                InstanceId = id,
                InstallationPath = path,
                InstallationVersion = Str(el, "installationVersion"),
                DisplayName = Str(el, "displayName"),
                ProductId = Str(el, "productId"),
                ChannelId = Str(el, "channelId"),
                IsPrerelease = Bool(el, "isPrerelease"),
                IsComplete = Bool(el, "isComplete"),
                IsLaunchable = Bool(el, "isLaunchable"),
                DevEnvPath = productPath.Length > 0 ? productPath : Path.Combine(path, "Common7", "IDE", "devenv.exe"),
                TextTransformPath = Path.Combine(path, "Common7", "IDE", "TextTransform.exe"),
                HasTextTemplatingComponent = withT4.Contains(id),
                DiscoverySource = "vswhere",
            };
            log($"vswhere: found {instance.Describe()} t4Component={instance.HasTextTemplatingComponent}");
            yield return instance;
        }
    }

    private static string Str(JsonElement el, string name) =>
        el.TryGetProperty(name, out var p) && p.ValueKind == JsonValueKind.String ? p.GetString() ?? "" : "";

    private static bool Bool(JsonElement el, string name) =>
        el.TryGetProperty(name, out var p) && (p.ValueKind == JsonValueKind.True || (p.ValueKind == JsonValueKind.String && string.Equals(p.GetString(), "true", StringComparison.OrdinalIgnoreCase)));

    private static string RunVsWhere(string vswhere, string args)
    {
        var psi = new ProcessStartInfo(vswhere, args)
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            StandardOutputEncoding = System.Text.Encoding.UTF8,
        };
        using var p = Process.Start(psi) ?? throw new InvalidOperationException("vswhere could not be started");
        var stdout = p.StandardOutput.ReadToEnd();
        var stderr = p.StandardError.ReadToEnd();
        p.WaitForExit(60_000);
        if (p.ExitCode != 0)
            throw new InvalidOperationException($"vswhere exited with {p.ExitCode}: {stderr}");
        return stdout;
    }

    /// <summary>
    /// Validates a candidate installation path (used for the override). Returns null when the path
    /// does not contain Common7/IDE/devenv.exe.
    /// </summary>
    public static VsInstance? ProbeInstallationPath(string installationPath, string instanceId, string source, Action<string> log)
    {
        if (string.IsNullOrWhiteSpace(installationPath) || !Directory.Exists(installationPath)) return null;
        var devenv = Path.Combine(installationPath, "Common7", "IDE", "devenv.exe");
        if (!File.Exists(devenv)) return null;
        var version = "0.0";
        try { version = FileVersionInfo.GetVersionInfo(devenv).ProductVersion?.Split('+', ' ')[0] ?? "0.0"; } catch { /* version stays unknown; reported below */ }
        var tt = Path.Combine(installationPath, "Common7", "IDE", "TextTransform.exe");
        log($"{source}: probing {installationPath} devenv={File.Exists(devenv)} textTransform={File.Exists(tt)} version={version}");
        return new VsInstance
        {
            InstanceId = instanceId,
            InstallationPath = installationPath,
            InstallationVersion = version,
            DisplayName = "Visual Studio (" + source + ")",
            ProductId = "",
            ChannelId = "",
            IsPrerelease = installationPath.IndexOf("Preview", StringComparison.OrdinalIgnoreCase) >= 0
                           || installationPath.IndexOf("Insiders", StringComparison.OrdinalIgnoreCase) >= 0,
            IsComplete = true,
            IsLaunchable = true,
            DevEnvPath = devenv,
            TextTransformPath = tt,
            // The override is validated by the presence of the tooling itself.
            HasTextTemplatingComponent = File.Exists(tt),
            DiscoverySource = source,
        };
    }
}
