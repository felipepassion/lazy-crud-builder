using System.Text;
using Lazy.Crud.Builder.Platform.Descriptor;
using Lazy.Crud.Builder.Platform.Tool.Descriptor;
using Lazy.Crud.Builder.Platform.Tool.Fixtures;

namespace Lazy.Crud.Builder.Platform.Tool.Commands;

public static class DescriptorCommand
{
    public static int Run(CommandLine o)
    {
        var sub = o.Positional.FirstOrDefault() ?? "generate";
        if (sub != "generate") throw new CommandLine.UsageException("descriptor generate ...");
        var repo = Path.GetFullPath(o.Require("repo"));
        var version = o.Get("version") ?? PlatformInfo.Version;
        var assembly = Path.GetFullPath(o.Require("attributes-assembly"));
        var output = Path.GetFullPath(o.Require("output"));
        var log = new Action<string>(m => Console.Error.WriteLine("  " + m));

        if (!File.Exists(assembly)) { Console.Error.WriteLine("attributes assembly not found: " + assembly); return ExitCodes.InvalidFixtureOrSandbox; }
        var inspections = AttributeCatalogBuilder.Inspect(assembly, log);
        var problems = inspections.SelectMany(i => i.Problems.Select(p => i.Entry.FullName + ": " + p)).ToList();
        if (problems.Count > 0)
        {
            Console.Error.WriteLine("attribute metadata contract violations:");
            foreach (var p in problems) Console.Error.WriteLine("  - " + p);
            return ExitCodes.AssertionFailure;
        }
        var descriptor = DescriptorBuilder.Build(version, inspections.Where(i => i.DerivesFromAttribute).Select(i => i.Entry).ToList());
        var json = descriptor.ToJson();
        Directory.CreateDirectory(Path.GetDirectoryName(output)!);
        File.WriteAllText(output, json, new UTF8Encoding(false));
        log($"descriptor written: {output} (platformVersion {version}, {descriptor.Templates.Registry.Count} templates, {descriptor.Attributes.Count} attributes)");

        var contracts = o.Get("fixtures");
        if (contracts != null)
        {
            var dir = FixtureSet.VersionDirectory(Path.GetFullPath(contracts), descriptor.ContractVersion);
            var files = FixtureSet.Produce(descriptor);
            FixtureSet.WriteAll(dir, files);
            log($"fixtures written: {dir} ({files.Count} files)");
        }
        return ExitCodes.Success;
    }
}
