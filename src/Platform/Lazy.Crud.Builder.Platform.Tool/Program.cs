using Lazy.Crud.Builder.Platform.Tool.Commands;

namespace Lazy.Crud.Builder.Platform.Tool;

public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length == 0 || args[0] is "-h" or "--help" or "help") { Console.Error.WriteLine(Usage); return ExitCodes.UsageError; }
        try
        {
            var options = CommandLine.Parse(args.Skip(1).ToArray());
            switch (args[0].ToLowerInvariant())
            {
                case "toolchain": return ToolchainCommand.Run(options);
                case "generate": return GenerateCommand.Run(options);
                case "sandbox": return SandboxCommand.Run(options);
                case "scaffold": return SandboxCommand.Run(CommandLine.Parse(new[] { "create" }.Concat(args.Skip(1)).ToArray()));
                case "descriptor": return DescriptorCommand.Run(options);
                case "validate": return ValidateCommand.Run(options);
                case "hash": return HashCommand.Run(options);
                case "manifest": return ManifestCommand.Run(options);
                case "probe": return ProbeCommand.Run(options);
                default:
                    Console.Error.WriteLine("unknown command: " + args[0]);
                    Console.Error.WriteLine(Usage);
                    return ExitCodes.UsageError;
            }
        }
        catch (CommandLine.UsageException ex)
        {
            Console.Error.WriteLine("usage error: " + ex.Message);
            return ExitCodes.UsageError;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("FATAL: " + ex);
            return ExitCodes.HarnessFailure;
        }
    }

    private const string Usage = @"Lazy.Crud.Builder.Platform.Tool - Lazy CRUD platform tooling

commands:
  toolchain [--vs-instance <path>] [--json]        resolve the Visual Studio T4 toolchain (exit 3 when unavailable)
  generate --solution <dir|sln> --aggregate <name> [--resolution protect|force|skip-modified]
           [--templates local:<ProjectFilesDir>|nuget] [--report <file>] [--no-tests] [--skip-restore]
                                                    unattended generation through the resolved Visual Studio toolchain
  sandbox create --output <dir> --aggregate <name> [--fixture <dir>] --version <v> [--feed <dir>]...
           [--templates local:<dir>|nuget] [--clean-room] [--packages-folder <dir>] [--platform-assembly <dll>]
           [--templates-package <id>] [--override <PackageId>=<version>]... [--no-tests] [--max-parallel <n>] [--no-restore]
  scaffold --output <dir> --aggregate <name> --version <v> [--feed <dir>]
                                                    quickstart: a consumer solution with a sample aggregate root (no fixture)
  descriptor generate --repo <dir> --version <v> --attributes-assembly <dll> --output <file> [--fixtures <contractsDir>]
  validate tree --repo <dir>                         registry/templates/call sites/attributes/versions/silent catches/docs/fixtures
  validate packages --repo <dir> --packages <dir>    produced .nupkg set against the descriptor
  validate fixtures --repo <dir> [--attributes-assembly <dll>]   committed fixtures equal the current build and match their schemas
  validate manifest --file <manifest.json> --repo <dir>          a manifest document against the published schema
  hash --file <path>                                 frozen content hash of a file
  manifest migrate --solution <dir> --aggregate <name>           migrate a v1 manifest in place (writes v2)
  probe <sln> <tt>...                                run custom tools in an embedded Visual Studio session

exit codes: 0 success, 1 assertion/operation failure, 2 usage, 3 prerequisite unavailable, 4 invalid fixture/sandbox,
            5 timeout, 6 contract mismatch, 7 ownership conflict, 8 internal failure";
}

/// <summary>Minimal --key value / --flag parser.</summary>
public sealed class CommandLine
{
    public sealed class UsageException : Exception { public UsageException(string m) : base(m) { } }

    private readonly Dictionary<string, List<string>> _values = new(StringComparer.OrdinalIgnoreCase);
    public List<string> Positional { get; } = new();

    public static CommandLine Parse(string[] args)
    {
        var c = new CommandLine();
        for (int i = 0; i < args.Length; i++)
        {
            var a = args[i];
            if (a.StartsWith("--", StringComparison.Ordinal))
            {
                var key = a.Substring(2);
                var eq = key.IndexOf('=');
                if (eq > 0) { c.Add(key.Substring(0, eq), key.Substring(eq + 1)); continue; }
                if (i + 1 < args.Length && !args[i + 1].StartsWith("--", StringComparison.Ordinal)) { c.Add(key, args[++i]); }
                else c.Add(key, "true");
            }
            else c.Positional.Add(a);
        }
        return c;
    }

    private void Add(string key, string value)
    {
        if (!_values.TryGetValue(key, out var list)) _values[key] = list = new List<string>();
        list.Add(value);
    }

    public string? Get(string key) => _values.TryGetValue(key, out var l) ? l[^1] : null;
    public IReadOnlyList<string> GetAll(string key) => _values.TryGetValue(key, out var l) ? l : Array.Empty<string>();
    public bool Has(string key) => _values.ContainsKey(key);
    public bool Flag(string key) => Has(key) && !string.Equals(Get(key), "false", StringComparison.OrdinalIgnoreCase);
    public string Require(string key) => Get(key) ?? throw new UsageException("--" + key + " is required");
    public int GetInt(string key, int defaultValue) => int.TryParse(Get(key), out var v) ? v : defaultValue;
}
