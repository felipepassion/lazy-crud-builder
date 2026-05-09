using MediatR;
using Lazy.Crud.Builder.Domain.Extensions;
using Lazy.Crud.CrossCutting.Infra.Log.Providers;
using Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.Events.Handles;
using Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.ModelEvents;

namespace Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.EventHandlers
{
public partial class ProductsEventHandler : BaseEventHandler,
    INotificationHandler<ProductsCreatedEvent>,
    INotificationHandler<ProductsDeletedEvent>,
    INotificationHandler<ProductsUpdatedEvent>,
    INotificationHandler<ProductsActivatedEvent>,
    INotificationHandler<ProductsDeactivatedEvent>{
    public ProductsEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
    public async Task Handle(ProductsCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}    
    public async Task Handle(ProductsDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}    
    public async Task Handle(ProductsActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}    
    public async Task Handle(ProductsUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}    
    public async Task Handle(ProductsDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}    
}
}
