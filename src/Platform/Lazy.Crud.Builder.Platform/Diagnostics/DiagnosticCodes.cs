using System.Collections.Generic;

namespace Lazy.Crud.Builder.Platform.Diagnostics
{
    /// <summary>
    /// The stable diagnostic code table. Codes never change meaning; new codes are appended.
    /// Documented in docs/reference/diagnostics.md and consumed by the CLI, the VSIX and the harness.
    /// </summary>
    public static class DiagnosticCodes
    {
        // 1xxx — I/O, manifest and migration
        /// <summary>Writing a generated file failed (locked, unwritable, invalid path).</summary>
        public const string FileWriteFailed = "LC1001";
        /// <summary>Creating the output directory failed.</summary>
        public const string DirectoryCreateFailed = "LC1002";
        /// <summary>A manifest exists but cannot be parsed.</summary>
        public const string ManifestUnreadable = "LC1003";
        /// <summary>Saving the manifest failed; the run must not report success.</summary>
        public const string ManifestSaveFailed = "LC1004";
        /// <summary>Manifests for one aggregate exist under more than one candidate root.</summary>
        public const string ManifestRootConflict = "LC1005";
        /// <summary>A v1 manifest entry could not be expressed relative to the solution root.</summary>
        public const string MigrationUnresolvedPath = "LC1006";
        /// <summary>A v1 manifest entry had no content hash; recorded as unknown.</summary>
        public const string MigrationUnknownHash = "LC1007";
        /// <summary>A v1 manifest entry had no ownership and the registry could not supply one.</summary>
        public const string MigrationUnknownOwnership = "LC1008";
        /// <summary>A v1 manifest was read and migrated in memory.</summary>
        public const string ManifestMigrated = "LC1009";
        /// <summary>The machine-readable diagnostics file could not be written.</summary>
        public const string DiagnosticsFileUnwritable = "LC1010";

        // 2xxx — ownership, drift and protection
        /// <summary>A GeneratorOwned file was modified outside the generator (hash mismatch).</summary>
        public const string Drifted = "LC2001";
        /// <summary>A GeneratorOwned file has no trustworthy baseline and its content differs from the regenerated body.</summary>
        public const string Unverified = "LC2002";
        /// <summary>A file exists with unknown ownership; treated as user-owned and protected.</summary>
        public const string UnknownOwnership = "LC2003";
        /// <summary>A template with no registry entry produced output; treated as user-owned.</summary>
        public const string UnregisteredTemplate = "LC2004";
        /// <summary>A GeneratedOnce file already exists and was left unchanged.</summary>
        public const string GeneratedOnceSkipped = "LC2005";
        /// <summary>A drifted or unverified file was overwritten because the force flag was supplied.</summary>
        public const string ForcedOverwrite = "LC2006";
        /// <summary>A drifted or unverified file was left untouched because the skip-modified flag was supplied.</summary>
        public const string SkippedModified = "LC2007";
        /// <summary>An unverified file whose regenerated body matched the on-disk body adopted its baseline.</summary>
        public const string UnverifiedAdopted = "LC2008";
        /// <summary>The marker is missing or malformed but the body matches the recorded hash.</summary>
        public const string MarkerProblem = "LC2009";
        /// <summary>A path classified Manual was requested for writing and refused.</summary>
        public const string ManualPathRefused = "LC2010";
        /// <summary>A file carries only a v1 marker; it is recognized as generated but not authorized for overwrite.</summary>
        public const string LegacyMarkerDetected = "LC2011";
        /// <summary>A regenerated body carries no content and would have replaced a file that has some; the write was refused.</summary>
        public const string OutputCollapsed = "LC2012";
        /// <summary>A collapsed output was retried and the retry produced a body with content; nothing was overwritten and the run continues.</summary>
        public const string OutputCollapseRetried = "LC2013";

        // 3xxx — templates, settings and model resolution
        /// <summary>The configured template source could not be resolved.</summary>
        public const string TemplateResolutionFailed = "LC3001";
        /// <summary>Local template development mode is in use.</summary>
        public const string LocalTemplateMode = "LC3002";
        /// <summary>A deprecated templates package id was resolved to the canonical id.</summary>
        public const string DeprecatedTemplatesPackageId = "LC3003";
        /// <summary>lazy.settings could not be located from the template location.</summary>
        public const string SettingsNotFound = "LC3004";
        /// <summary>A project or type in the model could not be resolved (tolerated).</summary>
        public const string ModelResolutionFailed = "LC3005";
        /// <summary>The resolved template source (kind, location, version) for the run.</summary>
        public const string TemplateSourceResolved = "LC3006";

        // 4xxx — toolchain and transformation
        /// <summary>The Visual Studio T4 toolchain (or the .NET SDK) could not be resolved.</summary>
        public const string PrerequisiteUnavailable = "LC4001";
        /// <summary>The resolved toolchain (instance, version, channel, tooling path).</summary>
        public const string ToolchainResolved = "LC4002";
        /// <summary>An explicit toolchain override was applied.</summary>
        public const string ToolchainOverrideApplied = "LC4003";
        /// <summary>A transformation step produced none of its expected outputs.</summary>
        public const string TransformationProducedNothing = "LC4004";
        /// <summary>The T4 engine reported an error while transforming a template.</summary>
        public const string TransformationFailed = "LC4005";

        // 5xxx — contracts and descriptor
        /// <summary>The descriptor contractVersion is outside the supported range.</summary>
        public const string ContractVersionMismatch = "LC5001";
        /// <summary>Installed packages embed descriptors that disagree; the highest platformVersion won.</summary>
        public const string DescriptorConflict = "LC5002";
        /// <summary>No platform descriptor could be resolved.</summary>
        public const string DescriptorMissing = "LC5003";
        /// <summary>A capability was disabled by configuration.</summary>
        public const string CapabilityDisabled = "LC5004";
        /// <summary>A manifest entry references a templateId absent from the registry.</summary>
        public const string UnknownTemplateId = "LC5005";

        private static readonly Dictionary<string, string> Descriptions = new Dictionary<string, string>
        {
            [FileWriteFailed] = "Writing a generated file failed.",
            [DirectoryCreateFailed] = "Creating an output directory failed.",
            [ManifestUnreadable] = "A manifest exists but cannot be parsed.",
            [ManifestSaveFailed] = "Saving the manifest failed.",
            [ManifestRootConflict] = "Manifests for one aggregate exist under more than one root.",
            [MigrationUnresolvedPath] = "A v1 manifest entry could not be made relative to the solution root.",
            [MigrationUnknownHash] = "A v1 manifest entry had no content hash.",
            [MigrationUnknownOwnership] = "A v1 manifest entry had no ownership.",
            [ManifestMigrated] = "A v1 manifest was migrated in memory.",
            [DiagnosticsFileUnwritable] = "The machine-readable diagnostics file could not be written.",
            [Drifted] = "A GeneratorOwned file was modified outside the generator.",
            [Unverified] = "A GeneratorOwned file has no trustworthy baseline and differs from the regenerated body.",
            [UnknownOwnership] = "A file has unknown ownership and is treated as user-owned.",
            [UnregisteredTemplate] = "A template without a registry entry produced output.",
            [GeneratedOnceSkipped] = "A GeneratedOnce file already exists and was left unchanged.",
            [ForcedOverwrite] = "A protected file was overwritten because force was supplied.",
            [SkippedModified] = "A protected file was skipped because skip-modified was supplied.",
            [UnverifiedAdopted] = "An unverified file was adopted because its content already matched.",
            [MarkerProblem] = "The marker is missing or malformed but the body matches.",
            [ManualPathRefused] = "A Manual path was refused for writing.",
            [LegacyMarkerDetected] = "A file carries only a v1 marker.",
            [OutputCollapsed] = "A regenerated body carries no content and the existing file was preserved.",
            [OutputCollapseRetried] = "A collapsed output was retried and the retry produced content.",
            [TemplateResolutionFailed] = "The template source could not be resolved.",
            [LocalTemplateMode] = "Local template development mode is active.",
            [DeprecatedTemplatesPackageId] = "A deprecated templates package id resolved to the canonical id.",
            [SettingsNotFound] = "lazy.settings could not be located.",
            [ModelResolutionFailed] = "A project or type in the model could not be resolved.",
            [TemplateSourceResolved] = "The template source resolved for the run.",
            [PrerequisiteUnavailable] = "A required prerequisite (Visual Studio T4 toolchain or .NET SDK) is unavailable.",
            [ToolchainResolved] = "The Visual Studio toolchain resolved for the run.",
            [ToolchainOverrideApplied] = "An explicit toolchain override was applied.",
            [TransformationProducedNothing] = "A transformation produced none of its expected outputs.",
            [TransformationFailed] = "The T4 engine reported an error.",
            [ContractVersionMismatch] = "The descriptor contractVersion is outside the supported range.",
            [DescriptorConflict] = "Installed descriptors disagree.",
            [DescriptorMissing] = "No platform descriptor could be resolved.",
            [CapabilityDisabled] = "A capability was disabled by configuration.",
            [UnknownTemplateId] = "A templateId is absent from the registry.",
        };

        public static IReadOnlyDictionary<string, string> All { get { return Descriptions; } }

        public static string Describe(string code)
        {
            string d;
            return Descriptions.TryGetValue(code ?? string.Empty, out d) ? d : "Unknown diagnostic code.";
        }
    }
}
