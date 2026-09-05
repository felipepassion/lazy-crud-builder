using Lazy.Crud.Builder.Platform.Descriptor;
using Lazy.Crud.Builder.Platform.Diagnostics;
using Lazy.Crud.Builder.Platform.Json;
using Lazy.Crud.Builder.Platform.Settings;
using Lazy.Crud.Builder.Platform.Tool.VisualStudio;
using Xunit;

namespace Lazy.Crud.Builder.Platform.Tests;

public class JsonLiteTests
{
    [Fact]
    public void RoundTripsNestedDocuments()
    {
        var text = "{\"a\": 1, \"b\": [true, null, \"x\\ny\", 2.5], \"c\": {\"d\": \"e\"}}";
        var o = JsonLite.ParseObject(text);
        Assert.Equal(1, o.GetInteger("a"));
        var arr = o.GetArray("b")!;
        Assert.Equal(true, arr[0]);
        Assert.Null(arr[1]);
        Assert.Equal("x\ny", arr[2]);
        Assert.Equal(2.5, arr[3]);
        Assert.Equal("e", o.GetObject("c")!.GetString("d"));
        var written = JsonLite.Write(o);
        var again = JsonLite.ParseObject(written);
        Assert.Equal("e", again.GetObject("c")!.GetString("d"));
        Assert.Equal(new[] { "a", "b", "c" }, again.Keys.ToArray());
    }

    [Fact]
    public void RejectsMalformedInput()
    {
        Assert.Throws<FormatException>(() => JsonLite.Parse("{ \"a\": }"));
        Assert.Throws<FormatException>(() => JsonLite.Parse("[1, 2"));
    }
}

public class LazySettingsTests
{
    [Fact]
    public void ReadsKnownKeysAndDefaults()
    {
        var dir = Path.Combine(Path.GetTempPath(), "lazycrud-tests", Guid.NewGuid().ToString("N"), "src", "Mods");
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, "lazy.settings");
        File.WriteAllText(path, "<?xml version=\"1.0\" encoding=\"utf-8\"?><SettingsFile xmlns=\"http://schemas.microsoft.com/VisualStudio/2004/01/settings\"><Settings>" +
            "<Setting Name=\"GenerationOutputPath\" Type=\"System.String\" Scope=\"Application\"><Value Profile=\"(Default)\">Generated</Value></Setting>" +
            "<Setting Name=\"MaxParallelT4\" Type=\"System.Int32\" Scope=\"Application\"><Value Profile=\"(Default)\">3</Value></Setting>" +
            "<Setting Name=\"UseNugetTemplates\" Type=\"System.Boolean\" Scope=\"Application\"><Value Profile=\"(Default)\">True</Value></Setting>" +
            "</Settings></SettingsFile>");
        var root = Path.GetDirectoryName(Path.GetDirectoryName(dir))!;
        var s = LazySettings.ForSolution(root);
        Assert.Equal(path, s.Path);
        Assert.Equal("Generated", s.GenerationOutputPath);
        Assert.Equal(3, s.MaxParallelT4);
        Assert.True(s.UseNugetTemplates);
        Assert.True(s.GeneratedCodeMarkers);
        Assert.Equal(TemplateRegistry.CanonicalTemplatesPackageId, s.TemplatesNuGetPackageId);
        Assert.Equal("{Aggregate}.Application.DTO", s.LayerPattern("Application.DTO"));
        var none = LazySettings.ForSolution(Path.Combine(root, "nowhere"));
        Assert.Null(none.Path);
        Assert.Equal("LazyCode", none.GenerationOutputPath);
    }
}

public class DescriptorResolverTests
{
    private static string Doc(string version, string source) =>
        "{ \"schemaVersion\": 1, \"contractVersion\": 1, \"platformVersion\": \"" + version + "\", \"layers\": [], \"templates\": { \"packageId\": \"Lazy.Crud.Builder.Templates\", \"registry\": [] }, \"attributes\": [], \"capabilities\": [\"x\"], \"compatibility\": { \"supportedContractVersionMin\": 1, \"supportedContractVersionMax\": 1, \"minimumUpgradableContractVersion\": 1, \"minimumManifestSchemaVersion\": 1, \"manifestSchemaVersion\": 2 } }";

    [Fact]
    public void SinglePackageResolves()
    {
        var diags = new DiagnosticCollector();
        var d = DescriptorResolver.Resolve(new[] { new DescriptorCandidate("pkgA", Doc("1.1.0", "a")) }, diags);
        Assert.Equal("1.1.0", d!.PlatformVersion);
        Assert.Empty(diags.Items);
    }

    [Fact]
    public void AgreeingPackagesProduceNoWarning()
    {
        var diags = new DiagnosticCollector();
        var d = DescriptorResolver.Resolve(new[] { new DescriptorCandidate("pkgA", Doc("1.1.0", "a")), new DescriptorCandidate("pkgB", Doc("1.1.0", "b")) }, diags);
        Assert.Equal("1.1.0", d!.PlatformVersion);
        Assert.DoesNotContain(diags.Items, x => x.Code == DiagnosticCodes.DescriptorConflict);
    }

    [Fact]
    public void HighestPlatformVersionWinsWithDiagnostic()
    {
        var diags = new DiagnosticCollector();
        var d = DescriptorResolver.Resolve(new[] { new DescriptorCandidate("pkgOld", Doc("1.1.0", "a")), new DescriptorCandidate("pkgNew", Doc("1.10.0", "b")), new DescriptorCandidate("pkgMid", Doc("1.2.0", "c")) }, diags);
        Assert.Equal("1.10.0", d!.PlatformVersion);
        Assert.Equal("pkgNew", d.Source);
        var w = Assert.Single(diags.Items, x => x.Code == DiagnosticCodes.DescriptorConflict);
        Assert.Contains("pkgOld", w.Message);
        Assert.Contains("pkgMid", w.Message);
        Assert.Equal("1.1.0", w.Data["pkgOld"]);
    }

    [Fact]
    public void MissingAndMalformedAreErrors()
    {
        var diags = new DiagnosticCollector();
        Assert.Null(DescriptorResolver.Resolve(Array.Empty<DescriptorCandidate>(), diags));
        Assert.Contains(diags.Items, x => x.Code == DiagnosticCodes.DescriptorMissing);
        diags.Clear();
        Assert.Null(DescriptorResolver.Resolve(new[] { new DescriptorCandidate("bad", "{ \"platformVersion\": \"1.0\" }") }, diags));
        Assert.Contains(diags.Items, x => x.Code == DiagnosticCodes.DescriptorMissing && x.Path == "bad");
    }

    [Fact]
    public void AdditiveFieldsDoNotBreakTheReader()
    {
        var json = Doc("1.1.0", "a").Replace("\"capabilities\"", "\"futureField\": {\"x\": [1,2]}, \"capabilities\"");
        var d = PlatformDescriptor.FromJson(json, "x");
        Assert.Equal(1, d.ContractVersion);
        Assert.Null(d.Compatibility.HostRequirements);
    }

    [Fact]
    public void ContractNegotiation()
    {
        var d = PlatformDescriptor.FromJson(Doc("1.1.0", "a"), "x");
        var diags = new DiagnosticCollector();
        Assert.True(DescriptorResolver.IsContractSupported(d, 1, 1, diags));
        Assert.False(DescriptorResolver.IsContractSupported(d, 2, 3, diags));
        Assert.Contains(diags.Items, x => x.Code == DiagnosticCodes.ContractVersionMismatch);
    }
}

public class VsInstanceLocatorTests
{
    private static VsInstance Make(string version, bool prerelease, string path, bool t4 = true) => new()
    {
        InstanceId = path, InstallationPath = path, InstallationVersion = version, IsPrerelease = prerelease,
        IsComplete = true, IsLaunchable = true, DisplayName = "VS", ProductId = "Pro", HasTextTemplatingComponent = t4,
        DevEnvPath = Environment.ProcessPath!, TextTransformPath = Environment.ProcessPath!, DiscoverySource = "test",
    };

    [Fact]
    public void SelectionPrefersReleaseThenHighestVersionThenPath()
    {
        var insiders = Make("18.10.0", true, @"C:\VS\18\Insiders");
        var release17 = Make("17.14.0", false, @"C:\VS\17\Pro");
        var release17b = Make("17.14.0", false, @"C:\VS\17\Ent");
        var selected = VsInstanceLocator.SelectDeterministically(new[] { insiders, release17b, release17 });
        Assert.Same(release17b, selected); // same version, ordinal path "C:\VS\17\Ent" < "C:\VS\17\Pro"
        Assert.Same(insiders, VsInstanceLocator.SelectDeterministically(new[] { insiders }));
        var newer = Make("17.15.0", false, @"C:\VS\17\Z");
        Assert.Same(newer, VsInstanceLocator.SelectDeterministically(new[] { release17, newer, insiders }));
        // Repeated runs are deterministic regardless of input order.
        Assert.Same(newer, VsInstanceLocator.SelectDeterministically(new[] { insiders, newer, release17b }));
    }

    [Fact]
    public void RejectionNamesTheMissingComponent()
    {
        var noT4 = Make("17.14.0", false, @"C:\VS\17\NoT4", t4: false);
        Assert.False(noT4.SatisfiesPrerequisite);
        Assert.Contains(VsToolchainResolution.RequiredComponentId, VsInstanceLocator.Reject(noT4));
    }

    [Fact]
    public void SimulatedUnavailabilityFailsWithoutDiscovery()
    {
        var previous = Environment.GetEnvironmentVariable(VsInstanceLocator.SimulateUnavailableEnvironmentVariable);
        try
        {
            Environment.SetEnvironmentVariable(VsInstanceLocator.SimulateUnavailableEnvironmentVariable, "1");
            var r = VsInstanceLocator.Resolve();
            Assert.False(r.Succeeded);
            Assert.Null(r.Selected);
            Assert.Contains("T4 tooling", r.FailureReason);
            Assert.Contains(VsToolchainResolution.RequiredComponentId, r.Remedy);
        }
        finally
        {
            Environment.SetEnvironmentVariable(VsInstanceLocator.SimulateUnavailableEnvironmentVariable, previous);
        }
    }

    [Fact]
    public void OverrideIsValidatedAndReported()
    {
        var r = VsInstanceLocator.Resolve(overridePath: Path.GetTempPath());
        Assert.False(r.Succeeded);
        Assert.True(r.OverrideApplied);
        Assert.Contains("devenv.exe", r.FailureReason);
    }
}

public class ExitCodeTests
{
    [Fact]
    public void TableCoversEveryRequiredOutcome()
    {
        Assert.Equal("Success", ExitCodes.Name(0));
        Assert.Equal("AssertionFailure", ExitCodes.Name(1));
        Assert.Equal("PrerequisiteUnavailable", ExitCodes.Name(3));
        Assert.Equal("InvalidFixtureOrSandbox", ExitCodes.Name(4));
        Assert.Equal("Timeout", ExitCodes.Name(5));
        Assert.Equal("ContractMismatch", ExitCodes.Name(6));
        Assert.Equal("OwnershipConflict", ExitCodes.Name(7));
        Assert.NotEqual(ExitCodes.AssertionFailure, ExitCodes.PrerequisiteUnavailable);
        Assert.NotEqual(ExitCodes.AssertionFailure, ExitCodes.OwnershipConflict);
        Assert.Equal(ExitCodes.Table.Count, ExitCodes.Table.Keys.Distinct().Count());
    }

    [Fact]
    public void EveryDiagnosticCodeIsDescribed()
    {
        foreach (var field in typeof(DiagnosticCodes).GetFields().Where(f => f.IsLiteral && f.FieldType == typeof(string)))
        {
            var code = (string)field.GetRawConstantValue()!;
            Assert.StartsWith("LC", code);
            Assert.NotEqual("Unknown diagnostic code.", DiagnosticCodes.Describe(code));
        }
    }
}
