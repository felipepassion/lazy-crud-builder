using MediatR;
using LazyCrud.Core.Domain.Extensions;
using LazyCrud.CrossCutting.Infra.Log.Providers;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Events.Handles;
using LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.ModelEvents;

namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.EventHandlers
{
    public partial class MarketPlaceAggSettingsEventHandler : BaseEventHandler,
        INotificationHandler<MarketPlaceAggSettingsCreatedEvent>,
        INotificationHandler<MarketPlaceAggSettingsDeletedEvent>,
        INotificationHandler<MarketPlaceAggSettingsUpdatedEvent>,
        INotificationHandler<MarketPlaceAggSettingsActivatedEvent>,
        INotificationHandler<MarketPlaceAggSettingsDeactivatedEvent>{
        public MarketPlaceAggSettingsEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(MarketPlaceAggSettingsCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(MarketPlaceAggSettingsDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(MarketPlaceAggSettingsActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(MarketPlaceAggSettingsUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(MarketPlaceAggSettingsDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
}
