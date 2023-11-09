using LazyCrudBuilder.Core.Domain.Extensions;
using LazyCrudBuilder.CrossCutting.Infra.Log.Providers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Events.Handles
{
    public class ExceptionEventHandler : INotificationHandler<ErrorEvent>
    {
        protected ILogProvider _logProvider;
        protected IServiceProvider _serviceProvider;

        public ExceptionEventHandler(IServiceProvider serviceProvider)
        {
            _logProvider = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ILogProvider>();
            _serviceProvider = serviceProvider;
        }

        public async Task Handle(ErrorEvent notification, CancellationToken cancellationToken)
        {
            _logProvider.Write(notification);
        }
    }
}