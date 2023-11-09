using MediatR;
using LazyCrudBuilder.Core.Domain.Extensions;
using LazyCrudBuilder.CrossCutting.Infra.Log.Providers;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Events.Handles;
using LazyCrudBuilder.MarketPlace.Domain.Aggregates.MarketPlaceAgg.ModelEvents;

namespace LazyCrudBuilder.MarketPlace.Domain.Aggregates.MarketPlaceAgg.EventHandlers
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
