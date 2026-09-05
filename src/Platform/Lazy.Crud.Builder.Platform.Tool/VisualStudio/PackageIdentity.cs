using System.Runtime.InteropServices;
using System.Text;

namespace Lazy.Crud.Builder.Platform.Tool.VisualStudio;

/// <summary>
/// Detects whether this process runs with MSIX package identity (for example when started from a
/// packaged desktop application's terminal). Such processes see %LOCALAPPDATA% and %APPDATA% through
/// file-system virtualization (writes land in the package's LocalCache), while the devenv.exe that
/// transforms the templates does not share that view. Generating into a virtualized path then fails
/// with "could not find a part of the path" errors that are otherwise inexplicable.
/// </summary>
public static class PackageIdentity
{
    private const int AppModelErrorNoPackage = 15700;

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    private static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder? packageFullName);

    public static string? CurrentPackageFullName()
    {
        if (!OperatingSystem.IsWindows()) return null;
        try
        {
            int length = 0;
            int rc = GetCurrentPackageFullName(ref length, null);
            if (rc == AppModelErrorNoPackage || length == 0) return null;
            var sb = new StringBuilder(length);
            rc = GetCurrentPackageFullName(ref length, sb);
            return rc == 0 ? sb.ToString() : null;
        }
        catch (Exception ex) when (ex is DllNotFoundException || ex is EntryPointNotFoundException)
        {
            return null;
        }
    }

    /// <summary>
    /// Returns a warning when <paramref name="path"/> lies under a virtualized profile directory of a
    /// process with package identity; null otherwise.
    /// </summary>
    public static string? DescribeVirtualizedPath(string path)
    {
        var package = CurrentPackageFullName();
        if (package == null) return null;
        var full = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar);
        foreach (var folder in new[] { Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolder.ApplicationData })
        {
            string root;
            try { root = Environment.GetFolderPath(folder); } catch { continue; }
            if (string.IsNullOrEmpty(root)) continue;
            if (full.StartsWith(root.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                return "this process runs with package identity (" + package + "); " + root + " is virtualized for it and Visual Studio does not share that view. Generate into a directory outside AppData (for example under the user profile or a drive root).";
        }
        return null;
    }
}
