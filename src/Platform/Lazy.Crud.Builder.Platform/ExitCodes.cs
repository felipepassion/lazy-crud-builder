using System.Collections.Generic;

namespace Lazy.Crud.Builder.Platform
{
    /// <summary>
    /// The shared exit-code table (contract F6). One table for the Builder harness, the CLI and the
    /// VSIX-facing engine: a code means the same thing everywhere.
    /// </summary>
    public static class ExitCodes
    {
        /// <summary>Every required scenario or the requested operation fully succeeded.</summary>
        public const int Success = 0;
        /// <summary>An assertion did not hold or the operation failed (generic failure).</summary>
        public const int AssertionFailure = 1;
        /// <summary>The command line was invalid or a required argument was missing.</summary>
        public const int UsageError = 2;
        /// <summary>A prerequisite (Visual Studio T4 toolchain, .NET SDK, environment) is unavailable.</summary>
        public const int PrerequisiteUnavailable = 3;
        /// <summary>A fixture could not be loaded or a sandbox could not be created.</summary>
        public const int InvalidFixtureOrSandbox = 4;
        /// <summary>A scenario or the run exceeded its timeout budget.</summary>
        public const int Timeout = 5;
        /// <summary>A resolved descriptor's contractVersion is outside the supported range, or versions disagree.</summary>
        public const int ContractMismatch = 6;
        /// <summary>The run was refused because drifted, unverified or unknown-ownership files require an explicit resolution.</summary>
        public const int OwnershipConflict = 7;
        /// <summary>The harness itself failed (internal error), as opposed to a scenario failing.</summary>
        public const int HarnessFailure = 8;
        /// <summary>
        /// Generation produced an output that carries no content where the existing file has some. The
        /// write was refused and nothing was lost, but the run generated a defective artifact and must
        /// not be treated as successful.
        /// </summary>
        public const int GenerationDefect = 9;

        public static readonly IReadOnlyDictionary<int, string> Table = new Dictionary<int, string>
        {
            [Success] = "Success",
            [AssertionFailure] = "AssertionFailure",
            [UsageError] = "UsageError",
            [PrerequisiteUnavailable] = "PrerequisiteUnavailable",
            [InvalidFixtureOrSandbox] = "InvalidFixtureOrSandbox",
            [Timeout] = "Timeout",
            [ContractMismatch] = "ContractMismatch",
            [OwnershipConflict] = "OwnershipConflict",
            [HarnessFailure] = "HarnessFailure",
            [GenerationDefect] = "GenerationDefect",
        };

        public static string Name(int code)
        {
            string n;
            return Table.TryGetValue(code, out n) ? n : "Unknown";
        }
    }
}
