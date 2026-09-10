using System.Text.RegularExpressions;
using Lazy.Crud.Builder.Platform.Tool.Descriptor;
using Lazy.Crud.Builder.Platform.Tool.Pipeline;
using Xunit;

namespace Lazy.Crud.Builder.Platform.Tests;

/// <summary>
/// The compatibility matrix, the descriptor's mixedVersionRanges and harness scenario R3 describe one mixed combination,
/// and it is one NuGet restores. They disagreed: the matrix and the descriptor let three packages that other packages
/// depend on stay at 1.0.29 alongside 1.1.1 -- a downgrade NuGet rejects (NU1605) -- while R3 pinned Api and WebApi one
/// patch version behind, and the sandbox has no WebApi project, so only the Api pin ever took effect.
/// </summary>
public class MixedVersionMatrixTests
{
    private static string Repo => FindRepo();

    private static string FindRepo()
    {
        var dir = AppContext.BaseDirectory;
        while (dir != null && !File.Exists(Path.Combine(dir, "LazyCrudBuilder.sln"))) dir = Path.GetDirectoryName(dir);
        return dir ?? throw new InvalidOperationException("repository root not found");
    }

    private static List<string> Sorted(IEnumerable<string> ids) => ids.OrderBy(x => x, StringComparer.Ordinal).ToList();

    /// <summary>Each project under src/ that ships as a package, by the projects it references (which become its package dependencies).</summary>
    private static Dictionary<string, List<string>> DependentsByPackage()
    {
        var dependents = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var project in Directory.EnumerateFiles(Path.Combine(Repo, "src"), "*.csproj", SearchOption.AllDirectories))
        {
            var name = Path.GetFileNameWithoutExtension(project);
            var text = File.ReadAllText(project);
            if (name.EndsWith(".Tests", StringComparison.Ordinal) || Regex.IsMatch(text, @"<IsPackable>\s*false\s*</IsPackable>", RegexOptions.IgnoreCase)) continue;
            foreach (Match m in Regex.Matches(text, @"<ProjectReference\s+Include=""([^""]+)"""))
            {
                var referenced = Path.GetFileNameWithoutExtension(m.Groups[1].Value.Replace('\\', Path.DirectorySeparatorChar));
                if (!dependents.TryGetValue(referenced, out var list)) dependents[referenced] = list = new List<string>();
                list.Add(name);
            }
        }
        return dependents;
    }

    /// <summary>
    /// A package may be pinned below the others only if nothing depends on it: the platform's dependencies are minimum
    /// versions, so a package that depends on it would require the higher version, and NuGet rejects the downgrade.
    /// </summary>
    [Fact]
    public void NothingDependsOnAPackageThatMayLag()
    {
        Assert.NotEmpty(DescriptorBuilder.PackagesThatMayLagOnePatch);
        var dependents = DependentsByPackage();
        foreach (var id in DescriptorBuilder.PackagesThatMayLagOnePatch)
        {
            var by = dependents.TryGetValue(id, out var list) ? list : new List<string>();
            Assert.True(by.Count == 0,
                id + " may lag the other packages, but " + string.Join(", ", by) + " depend on it: a lower pin would be a downgrade NuGet rejects (NU1605)");
        }
    }

    /// <summary>R3 pins exactly the packages the matrix lets lag, one patch version behind every other package.</summary>
    [Fact]
    public void ScenarioR3PinsExactlyThePackagesThatMayLag()
    {
        var script = File.ReadAllText(Path.Combine(Repo, "scripts", "e2e.ps1"));
        var r3 = Regex.Match(script, @"Add-Scenario 'R3'.*?\n}\r?\n", RegexOptions.Singleline);
        Assert.True(r3.Success, "scripts/e2e.ps1 defines no scenario R3");

        var pinned = Regex.Matches(r3.Value, @"'--override',\s*""([^=""]+)=\$version""").Select(m => m.Groups[1].Value);
        Assert.Equal(Sorted(DescriptorBuilder.PackagesThatMayLagOnePatch), Sorted(pinned));
        Assert.Contains("'--version', $mixedVersion", r3.Value, StringComparison.Ordinal);
    }

    /// <summary>
    /// An override of a package the sandbox never references is ignored without a word: R3 passed a WebApi pin for as long
    /// as it existed, and the sandbox has no WebApi project. Every package the matrix lets lag is one R3's sandbox restores.
    /// </summary>
    [Fact]
    public void ThePackagesThatMayLagAreOnesTheSandboxReferences()
    {
        foreach (var id in DescriptorBuilder.PackagesThatMayLagOnePatch)
            Assert.True(AggregateLayout.Layers.Any(l => l.RuntimePackageId == id || l.ExtraPlatformPackages.Contains(id)),
                id + " may lag, but no project of the sandbox references it, so R3 would pin it without effect");
    }

    /// <summary>The descriptor and the matrix in docs/compatibility.md name the same packages as supported to lag.</summary>
    [Fact]
    public void TheDescriptorAndTheMatrixNameThePackagesThatMayLag()
    {
        var statements = DescriptorBuilder.MixedVersionStatements("9.9.9");
        foreach (var id in DescriptorBuilder.PackagesThatMayLagOnePatch)
            Assert.Contains(statements, s => s.StartsWith(id + ": one patch version behind every other package", StringComparison.Ordinal));

        var matrix = File.ReadAllText(Path.Combine(Repo, "docs", "compatibility.md")).Replace("\r\n", "\n");
        var section = Regex.Match(matrix, @"## Mixed package versions\n(.*?)\n## ", RegexOptions.Singleline).Groups[1].Value;
        Assert.False(string.IsNullOrEmpty(section), "docs/compatibility.md has no Mixed package versions section");

        // The packages named in the combination cell of every row the matrix marks supported.
        var supported = section.Split('\n')
            .Where(line => Regex.IsMatch(line, @"^\|[^|]*\|\s*Yes\b"))
            .SelectMany(line => Regex.Matches(line.Split('|')[1], @"`(Lazy\.Crud\.[A-Za-z.]+)`").Select(m => m.Groups[1].Value))
            .Distinct();
        Assert.Equal(Sorted(DescriptorBuilder.PackagesThatMayLagOnePatch), Sorted(supported));
    }
}
