using LazyCrudBuilder.Core.Domain.Extensions;
using LazyCrudBuilder.CrossCutting.Infra.Log.Providers;

namespace LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Events.Handles
{
    public class BaseEventHandler
    {
        protected readonly ILogProvider _logProvider;
        protected readonly IServiceProvider _serviceProvider;

        protected BaseEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider)
        {
            _logProvider = logProvider;
            _serviceProvider = serviceProvider;
        }
        protected BaseEventHandler(ILogProvider logProvider)
        {
            _logProvider = logProvider;
        }

        protected void PublishLog<T>(T evnt)
            where T : BaseEvent
        {
            _logProvider.Write(evnt);
        }
    }
}
