using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Niu.Nutri.CrossCutting.Infra.Log.Entries;
using Niu.Nutri.CrossCutting.Infra.Log.Extensions;
using Niu.Nutri.CrossCutting.Infra.Log.SeedWork;
using ILogger = Serilog.ILogger;
using Serilog.Events;

namespace Niu.Nutri.CrossCutting.Infra.Log.Providers
{
    public partial class LoggerProvider : ILogProvider, Microsoft.Extensions.Logging.ILogger
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

        public void Log<TState>(
            Microsoft.Extensions.Logging.LogLevel logLevel, Microsoft.Extensions.Logging.EventId eventId,
            TState state, Exception? exception,
            Func<TState, Exception?, 
                string> formatter)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }
    }
}
