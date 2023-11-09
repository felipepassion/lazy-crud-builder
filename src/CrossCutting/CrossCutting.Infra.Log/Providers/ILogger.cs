using LazyCrudBuilder.CrossCutting.Infra.Log.Entries;
using Serilog;

namespace LazyCrudBuilder.CrossCutting.Infra.Log.Providers
{
    public interface ILogProvider
    {
        public ILogger Provider { get; }

        void Write(LogEntry entry);
        void Write(string message, string action);
        void WriteError(LogEntry entry);
        void WriteError(Exception ex, LogEntry entry);
    }
}
