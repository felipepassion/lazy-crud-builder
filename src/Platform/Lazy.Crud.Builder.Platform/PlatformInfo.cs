using System;
using System.Reflection;

namespace Lazy.Crud.Builder.Platform
{
    /// <summary>
    /// Identity of the platform build this library belongs to. The version comes from the single
    /// central version source (Directory.Build.props) through the assembly informational version,
    /// so the library, every package and the descriptor agree by construction.
    /// </summary>
    public static class PlatformInfo
    {
        /// <summary>Version of the shared cross-repository contract expressed by this build.</summary>
        public const int ContractVersion = 1;

        /// <summary>Lowest contractVersion this build still upgrades from.</summary>
        public const int MinimumUpgradableContractVersion = 1;

        /// <summary>Descriptor document schema version.</summary>
        public const int DescriptorSchemaVersion = 1;

        /// <summary>Manifest document schema version written by this build.</summary>
        public const int ManifestSchemaVersion = 2;

        /// <summary>Lowest manifest schemaVersion understood by this build (v1 is migrated).</summary>
        public const int MinimumManifestSchemaVersion = 1;

        public const string GeneratorIdentity = "Lazy.Crud.Builder";

        private static string _version;

        /// <summary>The platform version (equal to the NuGet package version of every package in the release).</summary>
        public static string Version
        {
            get
            {
                if (_version != null) return _version;
                var asm = typeof(PlatformInfo).Assembly;
                var info = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                var v = info != null ? info.InformationalVersion : null;
                if (string.IsNullOrEmpty(v))
                {
                    v = asm.GetName().Version != null ? asm.GetName().Version.ToString(3) : "0.0.0";
                }
                var plus = v.IndexOf('+');
                if (plus > 0) v = v.Substring(0, plus);
                _version = v;
                return _version;
            }
        }

        /// <summary>Parses a dotted version for ordering (prerelease suffixes are ignored for ordering).</summary>
        public static Version ParseVersion(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new Version(0, 0);
            var core = text;
            var dash = core.IndexOf('-');
            if (dash > 0) core = core.Substring(0, dash);
            var plus = core.IndexOf('+');
            if (plus > 0) core = core.Substring(0, plus);
            Version v;
            if (System.Version.TryParse(core.IndexOf('.') < 0 ? core + ".0" : core, out v)) return v;
            return new Version(0, 0);
        }
    }
}
