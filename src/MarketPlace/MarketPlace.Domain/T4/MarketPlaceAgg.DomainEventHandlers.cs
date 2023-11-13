using MediatR;
using LazyCrud.Core.Domain.Extensions;
using LazyCrud.CrossCutting.Infra.Log.Providers;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Events.Handles;
using LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.ModelEvents;

namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.EventHandlers
{
    public partial class ProdutoEventHandler : BaseEventHandler,
        INotificationHandler<ProdutoCreatedEvent>,
        INotificationHandler<ProdutoDeletedEvent>,
        INotificationHandler<ProdutoUpdatedEvent>,
        INotificationHandler<ProdutoActivatedEvent>,
        INotificationHandler<ProdutoDeactivatedEvent>{
        public ProdutoEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(ProdutoCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(ProdutoDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(ProdutoActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(ProdutoUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(ProdutoDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
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
    public partial class CarrinhoEventHandler : BaseEventHandler,
        INotificationHandler<CarrinhoCreatedEvent>,
        INotificationHandler<CarrinhoDeletedEvent>,
        INotificationHandler<CarrinhoUpdatedEvent>,
        INotificationHandler<CarrinhoActivatedEvent>,
        INotificationHandler<CarrinhoDeactivatedEvent>{
        public CarrinhoEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(CarrinhoCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(CarrinhoDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(CarrinhoActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(CarrinhoUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(CarrinhoDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
    public partial class CategoriaprodutoEventHandler : BaseEventHandler,
        INotificationHandler<CategoriaprodutoCreatedEvent>,
        INotificationHandler<CategoriaprodutoDeletedEvent>,
        INotificationHandler<CategoriaprodutoUpdatedEvent>,
        INotificationHandler<CategoriaprodutoActivatedEvent>,
        INotificationHandler<CategoriaprodutoDeactivatedEvent>{
        public CategoriaprodutoEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(CategoriaprodutoCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(CategoriaprodutoDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(CategoriaprodutoActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(CategoriaprodutoUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(CategoriaprodutoDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
}
