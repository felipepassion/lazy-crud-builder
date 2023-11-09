using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using LazyCrud.CrossCutting.Infra.Log.Entries;
using LazyCrud.CrossCutting.Infra.Log.Extensions;
using LazyCrud.CrossCutting.Infra.Log.SeedWork;
using ILogger = Serilog.ILogger;
using Serilog.Events;

namespace LazyCrud.CrossCutting.Infra.Log.Providers
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
