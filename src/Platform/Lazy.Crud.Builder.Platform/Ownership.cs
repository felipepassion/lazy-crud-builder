using System;

namespace Lazy.Crud.Builder.Platform
{
    /// <summary>
    /// The frozen three-value ownership model for every file the generator writes. These names are
    /// canonical: the descriptor, the marker, the manifest, diagnostics, documentation and every
    /// consuming surface use exactly these values and never a synonym.
    /// </summary>
    public enum Ownership
    {
        /// <summary>
        /// Replaceable. Regenerated on every run, but never overwritten before its current hash is
        /// compared against the last generated hash; an external divergence produces an explicit
        /// diagnostic and never a silent loss.
        /// </summary>
        GeneratorOwned = 0,

        /// <summary>
        /// User-owned after scaffold. Created when the file does not exist; once it exists it is
        /// never overwritten automatically.
        /// </summary>
        GeneratedOnce = 1,

        /// <summary>
        /// Outside the generator's ownership. Never created, modified or deleted by the generator,
        /// under any flag.
        /// </summary>
        Manual = 2,
    }

    /// <summary>Exact-name conversions for <see cref="Ownership"/> and the legacy boolean mapping.</summary>
    public static class OwnershipNames
    {
        public const string GeneratorOwned = "GeneratorOwned";
        public const string GeneratedOnce = "GeneratedOnce";
        public const string Manual = "Manual";

        /// <summary>The value recorded when ownership could not be established (migration).</summary>
        public const string Unknown = "Unknown";

        public static string ToName(Ownership ownership)
        {
            switch (ownership)
            {
                case Ownership.GeneratorOwned: return GeneratorOwned;
                case Ownership.GeneratedOnce: return GeneratedOnce;
                case Ownership.Manual: return Manual;
                default: throw new ArgumentOutOfRangeException(nameof(ownership), ownership, "Unknown ownership value");
            }
        }

        public static bool TryParse(string value, out Ownership ownership)
        {
            ownership = Ownership.GeneratorOwned;
            if (string.IsNullOrWhiteSpace(value)) return false;
            switch (value.Trim())
            {
                case GeneratorOwned: ownership = Ownership.GeneratorOwned; return true;
                case GeneratedOnce: ownership = Ownership.GeneratedOnce; return true;
                case Manual: ownership = Ownership.Manual; return true;
                default: return false;
            }
        }

        /// <summary>
        /// Backward-compatible mapping of the historical <c>recreateFileIfExists</c> boolean:
        /// <c>true</c> maps to <see cref="Ownership.GeneratorOwned"/>, <c>false</c> maps to
        /// <see cref="Ownership.GeneratedOnce"/>. Behavior for existing call sites is unchanged.
        /// </summary>
        public static Ownership FromLegacyRecreateFlag(bool recreateFileIfExists)
        {
            return recreateFileIfExists ? Ownership.GeneratorOwned : Ownership.GeneratedOnce;
        }
    }
}
