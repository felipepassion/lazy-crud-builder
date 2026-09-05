using Lazy.Crud.Builder.Platform.Descriptor;
using Lazy.Crud.Builder.Platform.Tool.Pipeline;
using Lazy.Crud.Builder.Platform.Tool.VisualStudio;

namespace Lazy.Crud.Builder.Platform.Tool.Descriptor;

/// <summary>
/// Produces lazycrud.platform.json from the build: the template registry, the canonical consumer
/// layout, the attribute catalog reflected from the shipped assembly and the compatibility
/// declaration. This is the single code path for the real descriptor and its fixture.
/// </summary>
public static class DescriptorBuilder
{
    public static readonly IReadOnlyList<string> Capabilities = new[]
    {
        "platform-descriptor",
        "generated-code-ownership",
        "generated-code-markers",
        "generation-manifest-v2",
        "drift-protection",
        "generation-diagnostics",
        "attribute-metadata-contract",
        "contract-fixtures",
        "unattended-t4-generation",
        "templates-alias-resolution",
    };

    public static readonly IReadOnlyList<string> NonLayerPackages = new[]
    {
        "Lazy.Crud.Builder.Application.DTO.Http.Models",
        "Lazy.Crud.Builder.Application.Validators",
        "Lazy.Crud.Builder.WebApi",
        "Lazy.Crud.CrossCutting.Domain",
        "Lazy.Crud.CrossCutting.Infra.Log",
        "Lazy.Crud.CrossCutting.Infra.Utils",
        "Lazy.Crud.CrossCutting.Application.Mail",
        "Lazy.Crud.Builder.Platform",
        TemplateRegistry.CanonicalTemplatesPackageId,
        TemplateRegistry.DeprecatedTemplatesPackageId,
    };

    public const string HeadlessDefinition =
        "Headless means unattended: no prompt, no window to focus, no person driving the IDE. It does not mean Visual Studio is absent. " +
        "Generation always runs through the T4 tooling of the resolved Visual Studio installation (embedded devenv.exe automation session); a plain build never substitutes for transformation.";

    public static PlatformDescriptor Build(string platformVersion, IReadOnlyList<AttributeDescriptor> attributes)
    {
        var d = new PlatformDescriptor
        {
            SchemaVersion = PlatformInfo.DescriptorSchemaVersion,
            ContractVersion = PlatformInfo.ContractVersion,
            PlatformVersion = platformVersion,
        };

        foreach (var l in AggregateLayout.Layers)
        {
            var hasLayerTemplates = TemplateRegistry.Entries.Any(e => string.Equals(e.Layer, l.Id, StringComparison.Ordinal));
            d.Layers.Add(new LayerDescriptor
            {
                Id = l.Id,
                ProjectSuffix = l.ProjectSuffix,
                ProjectPattern = l.ProjectPattern,
                PackageId = l.RuntimePackageId,
                TemplatePackageId = hasLayerTemplates ? l.RuntimePackageId : null,
                TemplateContentPath = hasLayerTemplates ? l.TemplateContentPath : null,
                OutputPathPattern = "src/{Aggregate}/{Aggregate}." + l.Id + "/{OutputPath}",
            });
        }
        foreach (var t in AggregateLayout.TestLayers)
        {
            var layer = AggregateLayout.FindLayer(t.TargetLayer)!;
            var folder = t.Wrapper.SourceTemplate.Substring(0, t.Wrapper.SourceTemplate.LastIndexOf('/') + 1);
            d.TestLayers.Add(new TestLayerDescriptor
            {
                Id = t.Id,
                TargetLayer = t.TargetLayer,
                ProjectPattern = "src/{Aggregate}/" + t.ProjectPattern,
                TemplatePackageId = layer.RuntimePackageId,
                TemplateContentPath = LayerSpec.TemplateRegistryContentRoot + folder,
                OutputPathPattern = "src/{Aggregate}/" + t.ProjectPattern + "/{TestOutputPath}",
            });
        }

        d.Templates = new TemplatesDescriptor
        {
            PackageId = TemplateRegistry.CanonicalTemplatesPackageId,
            AliasPackageIds = new List<string> { TemplateRegistry.DeprecatedTemplatesPackageId },
            Version = platformVersion,
            ContentRoot = TemplateRegistry.ContentRoot,
            SharedFiles = new List<string> { "project.tt", "Tests/Tests.Common.ttinclude" },
            OutputPatternTokens = TemplateRegistry.OutputPatternTokens.ToList(),
            Registry = TemplateRegistry.Entries.Select(e => new RegistryEntryDescriptor
            {
                Id = e.Id,
                Layer = e.Layer,
                SourceFile = e.SourceFile,
                OutputPattern = e.OutputPattern,
                Ownership = OwnershipNames.ToName(e.Ownership),
                TemplateVersion = platformVersion,
                PackageId = e.PackageId,
                InPackagePath = e.InPackagePath,
                Kind = e.Kind.ToString().ToLowerInvariant(),
            }).ToList(),
        };

        d.AttributeCategories = AttributeCatalogBuilder.CategoryVocabulary.ToList();
        d.Attributes = attributes.OrderBy(a => a.FullName, StringComparer.Ordinal).ToList();
        d.Capabilities = Capabilities.ToList();
        d.NonLayerPackages = NonLayerPackages.ToList();
        d.Compatibility = new CompatibilityDescriptor
        {
            SupportedContractVersionMin = PlatformInfo.MinimumUpgradableContractVersion,
            SupportedContractVersionMax = PlatformInfo.ContractVersion,
            MinimumUpgradableContractVersion = PlatformInfo.MinimumUpgradableContractVersion,
            MinimumManifestSchemaVersion = PlatformInfo.MinimumManifestSchemaVersion,
            ManifestSchemaVersion = PlatformInfo.ManifestSchemaVersion,
            MixedVersionRanges = new List<string>
            {
                "Lazy.Crud.Builder.Application.DTO.Http.Models: 1.0.29 to " + platformVersion + " alongside runtime packages at " + platformVersion,
                "Lazy.Crud.Builder.Application.Validators: 1.0.29 to " + platformVersion + " alongside runtime packages at " + platformVersion,
                "Lazy.Crud.CrossCutting.Domain: 1.0.29 to " + platformVersion + " alongside runtime packages at " + platformVersion,
                "Lazy.Crud.Builder.Templates must equal the version of every Lazy.Crud.Builder.* layer package that ships templates",
            },
            HostRequirements = new HostRequirementsDescriptor
            {
                Platform = "windows",
                VisualStudioVersions = new List<string> { "17.x (Visual Studio 2022)", "18.x (Visual Studio 2026)" },
                VisualStudioChannels = new List<string> { "release", "preview", "insiders" },
                T4Component = VsToolchainResolution.RequiredComponentId,
                T4ComponentDisplayName = VsToolchainResolution.RequiredComponentDisplayName,
                DotnetSdk = "10.0",
                HeadlessDefinition = HeadlessDefinition,
            },
        };
        return d;
    }
}
