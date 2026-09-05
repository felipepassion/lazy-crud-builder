using System.Runtime.InteropServices;

namespace Lazy.Crud.Builder.Platform.Tool.VisualStudio;

/// <summary>
/// Minimal binding to the Visual Studio Setup Configuration COM API
/// (Microsoft.VisualStudio.Setup.Configuration). Used when vswhere.exe is unavailable.
/// Interface layouts follow Setup.Configuration.h.
/// </summary>
public static class SetupConfigurationApi
{
    private const string SetupConfigurationClsid = "177F0C4A-1CD3-4DE7-A32C-71DBBB9FA36D";

    [ComImport, Guid("42843719-DB4C-46C2-8E7C-64F1816EFD5B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface ISetupConfiguration
    {
        IEnumSetupInstances EnumInstances();
        ISetupInstance GetInstanceForCurrentProcess();
        ISetupInstance GetInstanceForPath([MarshalAs(UnmanagedType.LPWStr)] string path);
    }

    [ComImport, Guid("26AAB78C-4A60-49D6-AF3B-3C35BC93365D"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface ISetupConfiguration2 : ISetupConfiguration
    {
        new IEnumSetupInstances EnumInstances();
        new ISetupInstance GetInstanceForCurrentProcess();
        new ISetupInstance GetInstanceForPath([MarshalAs(UnmanagedType.LPWStr)] string path);
        IEnumSetupInstances EnumAllInstances();
    }

    [ComImport, Guid("6380BCFF-41D3-4B2E-8B2E-BF8A6810C848"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IEnumSetupInstances
    {
        void Next(int celt, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Interface, SizeParamIndex = 0), Out] ISetupInstance[] rgelt, out int pceltFetched);
        void Skip(int celt);
        void Reset();
        IEnumSetupInstances Clone();
    }

    [ComImport, Guid("B41463C3-8866-43B5-BC33-2B0676F7F42E"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface ISetupInstance
    {
        [return: MarshalAs(UnmanagedType.BStr)] string GetInstanceId();
        System.Runtime.InteropServices.ComTypes.FILETIME GetInstallDate();
        [return: MarshalAs(UnmanagedType.BStr)] string GetInstallationName();
        [return: MarshalAs(UnmanagedType.BStr)] string GetInstallationPath();
        [return: MarshalAs(UnmanagedType.BStr)] string GetInstallationVersion();
        [return: MarshalAs(UnmanagedType.BStr)] string GetDisplayName(int lcid);
        [return: MarshalAs(UnmanagedType.BStr)] string GetDescription(int lcid);
        [return: MarshalAs(UnmanagedType.BStr)] string ResolvePath([MarshalAs(UnmanagedType.LPWStr)] string relativePath);
    }

    [ComImport, Guid("89143C9A-05AF-49B0-B717-72E218A2185C"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface ISetupInstance2 : ISetupInstance
    {
        [return: MarshalAs(UnmanagedType.BStr)] new string GetInstanceId();
        new System.Runtime.InteropServices.ComTypes.FILETIME GetInstallDate();
        [return: MarshalAs(UnmanagedType.BStr)] new string GetInstallationName();
        [return: MarshalAs(UnmanagedType.BStr)] new string GetInstallationPath();
        [return: MarshalAs(UnmanagedType.BStr)] new string GetInstallationVersion();
        [return: MarshalAs(UnmanagedType.BStr)] new string GetDisplayName(int lcid);
        [return: MarshalAs(UnmanagedType.BStr)] new string GetDescription(int lcid);
        [return: MarshalAs(UnmanagedType.BStr)] new string ResolvePath([MarshalAs(UnmanagedType.LPWStr)] string relativePath);
        uint GetState();
        [return: MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_UNKNOWN)] ISetupPackageReference[] GetPackages();
        ISetupPackageReference GetProduct();
        [return: MarshalAs(UnmanagedType.BStr)] string GetProductPath();
        [return: MarshalAs(UnmanagedType.Interface)] object GetErrors();
        [return: MarshalAs(UnmanagedType.VariantBool)] bool IsLaunchable();
        [return: MarshalAs(UnmanagedType.VariantBool)] bool IsComplete();
        [return: MarshalAs(UnmanagedType.Interface)] object GetProperties();
        [return: MarshalAs(UnmanagedType.BStr)] string GetEnginePath();
    }

    [ComImport, Guid("9AD8E40F-39A2-40F1-BF64-0A6C50DD9EEB"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface ISetupInstanceCatalog
    {
        [return: MarshalAs(UnmanagedType.Interface)] object GetCatalogInfo();
        [return: MarshalAs(UnmanagedType.VariantBool)] bool IsPrerelease();
    }

    [ComImport, Guid("DA8D8A16-B2B6-4487-A2F1-594CCCCD6BF5"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface ISetupPackageReference
    {
        [return: MarshalAs(UnmanagedType.BStr)] string GetId();
        [return: MarshalAs(UnmanagedType.BStr)] string GetVersion();
        [return: MarshalAs(UnmanagedType.BStr)] string GetChipset();
        [return: MarshalAs(UnmanagedType.BStr)] string GetLanguage();
        [return: MarshalAs(UnmanagedType.BStr)] string GetBranch();
        [return: MarshalAs(UnmanagedType.BStr)] string GetType();
        [return: MarshalAs(UnmanagedType.BStr)] string GetUniqueId();
        [return: MarshalAs(UnmanagedType.VariantBool)] bool GetIsExtension();
    }

    public static IEnumerable<VsInstance> EnumerateInstances(Action<string> log)
    {
        if (!OperatingSystem.IsWindows()) yield break;
        var type = Type.GetTypeFromCLSID(new Guid(SetupConfigurationClsid));
        if (type == null) { log("setup-configuration-api: CLSID not registered"); yield break; }
        var config = (ISetupConfiguration2)Activator.CreateInstance(type)!;
        var e = config.EnumAllInstances();
        var buffer = new ISetupInstance[1];
        while (true)
        {
            e.Next(1, buffer, out var fetched);
            if (fetched == 0) break;
            var inst = buffer[0];
            VsInstance? result = null;
            try
            {
                var inst2 = (ISetupInstance2)inst;
                var path = inst2.GetInstallationPath();
                var packages = SafePackages(inst2);
                var hasT4 = packages.Any(p => string.Equals(p, VsToolchainResolution.RequiredComponentId, StringComparison.OrdinalIgnoreCase));
                var isPrerelease = false;
                try { isPrerelease = ((ISetupInstanceCatalog)inst).IsPrerelease(); } catch { /* older engine without catalog info: treated as release */ }
                string productPath;
                try { productPath = Path.Combine(path, inst2.GetProductPath()); } catch { productPath = Path.Combine(path, "Common7", "IDE", "devenv.exe"); }
                result = new VsInstance
                {
                    InstanceId = inst2.GetInstanceId(),
                    InstallationPath = path,
                    InstallationVersion = inst2.GetInstallationVersion(),
                    DisplayName = SafeCall(() => inst2.GetDisplayName(0x409)) ?? "",
                    ProductId = SafeCall(() => inst2.GetProduct().GetId()) ?? "",
                    ChannelId = "",
                    IsPrerelease = isPrerelease,
                    IsComplete = SafeCall(() => inst2.IsComplete()) ?? true,
                    IsLaunchable = SafeCall(() => inst2.IsLaunchable()) ?? true,
                    DevEnvPath = productPath,
                    TextTransformPath = Path.Combine(path, "Common7", "IDE", "TextTransform.exe"),
                    HasTextTemplatingComponent = hasT4,
                    DiscoverySource = "setup-configuration-api",
                };
                log($"setup-configuration-api: found {result.Describe()} t4Component={hasT4}");
            }
            catch (Exception ex)
            {
                log($"setup-configuration-api: instance skipped -> {ex.GetType().Name}: {ex.Message}");
            }
            if (result != null) yield return result;
        }
    }

    private static string[] SafePackages(ISetupInstance2 inst)
    {
        try { return inst.GetPackages().Select(p => p.GetId()).ToArray(); }
        catch { return Array.Empty<string>(); }
    }

    private static T? SafeCall<T>(Func<T> f) where T : struct { try { return f(); } catch { return null; } }
    private static string? SafeCall(Func<string> f) { try { return f(); } catch { return null; } }
}
