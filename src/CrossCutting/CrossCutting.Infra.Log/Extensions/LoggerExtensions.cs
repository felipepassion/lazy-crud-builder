using Niu.Nutri.CrossCutting.Infra.Log.Entries;
using Serilog;
using Serilog.Context;
using Serilog.Core.Enrichers;
using System.Diagnostics;

namespace Niu.Nutri.CrossCutting.Infra.Log.Extensions
{
    public static class LoggerExtensions
    {
        public static Stopwatch Write(this ILogger logger, LogEntry entry)
        {
            using (LogContext.PushProperty("Action", entry.Action))
            using (LogContext.PushProperty("MessageType", "Request"))
            using (LogContext.PushProperty("RequestKey", entry.RequestKey))
            using (LogContext.PushProperty("Content", entry.Content, true))
            {
                entry.Title = "[{Application}]: " + entry.Title;
                switch (entry.Level)
                {
                    case Serilog.Events.LogEventLevel.Verbose:
                    case Serilog.Events.LogEventLevel.Debug:
                    case Serilog.Events.LogEventLevel.Information:
                        logger.Information(entry.Title);
                        break;
                    case Serilog.Events.LogEventLevel.Warning:
                        logger.Warning(entry.Title);
                        break;
                    case Serilog.Events.LogEventLevel.Error:
                    case Serilog.Events.LogEventLevel.Fatal:
                        logger.Error(entry.Exception, entry.Title);
                        break;
                }
            }

            Stopwatch stopwatch = new();
            stopwatch.Start();
            return stopwatch;
        }
    }
}