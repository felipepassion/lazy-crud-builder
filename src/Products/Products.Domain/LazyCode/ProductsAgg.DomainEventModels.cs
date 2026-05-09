using Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.Events;
using Lazy.Crud.CrossCutting.Infra.Log.Contexts;

namespace Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.ModelEvents
{
using ModelEvents;
using Entities;
public partial class ProductsCreatedEvent : BaseEvent
{
    public ProductsCreatedEvent(ILogRequestContext ctx, Products data) 
        : base(ctx, data) { }
}
public partial class ProductsDeletedEvent : BaseEvent
{
    public ProductsDeletedEvent(ILogRequestContext ctx, Products data) 
        : base(ctx, data) { }
}
public partial class ProductsDeletedRangeEvent : BaseEvent
{
    public ProductsDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<Products> data) 
        : base(ctx, data) { }
}
public partial class ProductsActivatedEvent : BaseEvent
{
    public ProductsActivatedEvent(ILogRequestContext ctx, Products data) 
        : base(ctx, data) { }
}
public partial class ProductsUpdatedEvent : BaseEvent
{
    public ProductsUpdatedEvent(ILogRequestContext ctx, Products data) 
        : base(ctx, data) { }
}
public partial class ProductsUpdatedRangeEvent : BaseEvent
{
    public ProductsUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<Products> data) 
        : base(ctx, data) { }
}
public partial class ProductsDeactivatedEvent : BaseEvent
{
    public ProductsDeactivatedEvent(ILogRequestContext ctx, Products data) 
        : base(ctx, data) { }
}
}
