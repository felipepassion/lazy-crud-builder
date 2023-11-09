using LazyCrudBuilder.Core.Application.DTO.Extensions;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Events;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Notifications;
using LazyCrudBuilder.CrossCutting.Infra.Log.Entries;
using LazyCrudBuilder.CrossCutting.Infra.Log.Extensions;
using LazyCrudBuilder.CrossCutting.Infra.Log.Providers;

namespace LazyCrudBuilder.Core.Domain.Extensions
{
    public static class NotificationExtensions
    {
        public static void PublishLog<T>(this Serilog.ILogger _logger, T notification)
            where T : BaseEvent
        {
            var className = typeof(T).Name;

            notification.Title = string.IsNullOrWhiteSpace(notification.Title) ?
               $"[{notification.Context.ServiceName} {notification.Context.OperationName}]" :
               $"{notification.Title}";

            _logger.Write(notification.ProjectedAs<LogEntry>());
        }

        public static void PublishLogError<T>(this Serilog.ILogger _logger, T notification)
            where T : ErrorEvent
        {
            var className = typeof(T).Name;

            notification.Title = string.IsNullOrWhiteSpace(notification.Title) ?
               $"[{notification.Context.ServiceName} {notification.Context.OperationName}]" :
               $"{notification.Title}";

            _logger.Error(notification.Exception, notification.Title, notification.ProjectedAs<LogEntry>());
        }

        public static void Write<T>(this ILogProvider logger, T notification)
            where T : BaseEvent
        {
            Task.Run(() => logger.Provider.PublishLog(notification));
        }

        public static void WriteError<T>(this ILogProvider logger, T notification)
            where T : ErrorEvent
        {
            Task.Run(() => logger.Provider.PublishLogError(notification));
        }

    }
}
