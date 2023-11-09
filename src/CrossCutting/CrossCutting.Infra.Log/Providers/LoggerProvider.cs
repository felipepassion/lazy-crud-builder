using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using LazyCrudBuilder.CrossCutting.Infra.Log.Entries;
using LazyCrudBuilder.CrossCutting.Infra.Log.Extensions;
using LazyCrudBuilder.CrossCutting.Infra.Log.SeedWork;
using ILogger = Serilog.ILogger;
using Serilog.Events;

namespace LazyCrudBuilder.CrossCutting.Infra.Log.Providers
{
    public partial class LoggerProvider : ILogProvider
    {
        public ILogger Provider { get; }

        public LoggerProvider(IConfiguration configuration)
        {
            Provider = LoggerFactory.CreateLog(configuration);
        }

        public void Write(LogEntry entry)
        {
            Task.Run(() => Provider.Write(entry));
        }

        public void Write(string message, string action)
        {
            Write(new LogEntry(message, action));
        }

        public void WriteError(LogEntry entry)
        {
            Task.Run(() =>
               Provider.Write(LogEventLevel.Error, new Exception(entry.Title), "Erro")
            );
        }

        public void WriteError(Exception ex, LogEntry entry)
        {
            Task.Run(() =>
               Provider.Write(LogEventLevel.Error, ex, "Erro")
            );
        }

    }
}
