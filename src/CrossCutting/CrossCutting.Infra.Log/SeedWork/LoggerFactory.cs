using Destructurama;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace LazyCrudBuilder.CrossCutting.Infra.Log.SeedWork
{
    public class LoggerFactory
    {
        public static Guid ExecutionKey { get; set; } = Guid.NewGuid();
        public static ILogger CreateLog(IConfiguration configuration)
        {
            var section = configuration.GetSection("Logging");

            var loggerConfiguration = new LoggerConfiguration()
                 .Enrich.WithProperty("Domain", section["Domain"])
                 .Enrich.WithProperty("Application", section["Application"])
                 .Enrich.WithProperty("ProductVersion", section["ProductVersion"])
                 .Enrich.WithProperty("MachineName", Environment.MachineName)
                 .Enrich.WithProperty("InstanceKey", Guid.NewGuid().ToString())
                 .Enrich.WithProperty("ExecutionKey", ExecutionKey)
                 .Enrich.FromLogContext();

            if (!string.IsNullOrEmpty(section["Seq:Url"]))
                loggerConfiguration
                    .WriteTo.Seq(serverUrl: section["Seq:Url"]!)
                    .MinimumLevel.Is(GetMinimumLevelLog(section["Seq:MinimumLevel"]!));

            loggerConfiguration.Destructure.JsonNetTypes();

            return loggerConfiguration
                .CreateLogger();
        }

        private static LogEventLevel GetMinimumLevelLog(string minimumLevel)
        {
            Enum.TryParse(minimumLevel, true, out LogEventLevel logEventLevel);
            return logEventLevel;
        }
    }
}
