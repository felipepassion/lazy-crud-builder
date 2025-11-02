using Destructurama;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Niu.Nutri.CrossCutting.Infra.Log.SeedWork
{
    public class LoggerFactory
    {
        public static Guid ExecutionKey { get; set; } = Guid.NewGuid();
        public static ILogger CreateLog(IConfiguration configuration)
        {
            var section = configuration.GetSection("Logging");

            var loggerConfiguration = new LoggerConfiguration()
                 .Enrich.WithProperty("Domain", configuration["Domain"])
                 .Enrich.WithProperty("Application", configuration["Application"])
                 .Enrich.WithProperty("ProductVersion", configuration["ProductVersion"])
                 .Enrich.WithProperty("MachineName", Environment.MachineName)
                 .Enrich.WithProperty("InstanceKey", Guid.NewGuid().ToString())
                 .Enrich.WithProperty("ExecutionKey", ExecutionKey)
                 .Enrich.FromLogContext();

            if (!string.IsNullOrEmpty(section["SeqUrl"]))
                loggerConfiguration
                    .WriteTo.Seq(serverUrl: section["SeqUrl"]!)
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
