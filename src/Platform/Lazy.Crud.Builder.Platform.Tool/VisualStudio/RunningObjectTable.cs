using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Lazy.Crud.Builder.Platform.Tool.VisualStudio;

/// <summary>Looks up COM objects registered in the Running Object Table by display name.</summary>
public static class RunningObjectTable
{
    [DllImport("ole32.dll")]
    private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);

    [DllImport("ole32.dll")]
    private static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);

    public static IReadOnlyList<string> ListDisplayNames()
    {
        var names = new List<string>();
        if (!OperatingSystem.IsWindows()) return names;
        Marshal.ThrowExceptionForHR(GetRunningObjectTable(0, out var rot));
        Marshal.ThrowExceptionForHR(CreateBindCtx(0, out var ctx));
        rot.EnumRunning(out var enumMoniker);
        enumMoniker.Reset();
        var monikers = new IMoniker[1];
        var fetched = IntPtr.Zero;
        while (enumMoniker.Next(1, monikers, fetched) == 0)
        {
            try
            {
                monikers[0].GetDisplayName(ctx, null, out var name);
                names.Add(name);
            }
            catch { /* a moniker that cannot render its name is not one we are looking for */ }
        }
        return names;
    }

    public static object? GetObject(string displayName)
    {
        if (!OperatingSystem.IsWindows()) return null;
        Marshal.ThrowExceptionForHR(GetRunningObjectTable(0, out var rot));
        Marshal.ThrowExceptionForHR(CreateBindCtx(0, out var ctx));
        rot.EnumRunning(out var enumMoniker);
        enumMoniker.Reset();
        var monikers = new IMoniker[1];
        var fetched = IntPtr.Zero;
        while (enumMoniker.Next(1, monikers, fetched) == 0)
        {
            string name;
            try { monikers[0].GetDisplayName(ctx, null, out name); }
            catch { continue; }
            if (string.Equals(name, displayName, StringComparison.OrdinalIgnoreCase))
            {
                var hr = rot.GetObject(monikers[0], out var obj);
                if (hr == 0 && obj != null) return obj;
            }
        }
        return null;
    }
}
