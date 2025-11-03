using Lazy.Crud.Core.Domain.Extensions;
using Lazy.Crud.CrossCutting.Infra.Log.Providers;

namespace Lazy.Crud.Core.Domain.Aggregates.CommonAgg.Events.Handles
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
