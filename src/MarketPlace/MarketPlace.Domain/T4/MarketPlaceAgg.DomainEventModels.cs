using LazyCrud.Core.Domain.Aggregates.CommonAgg.Events;
using LazyCrud.CrossCutting.Infra.Log.Contexts;

namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.ModelEvents
{
    using ModelEvents;
    using Entities;
    public partial class ProdutoCreatedEvent : BaseEvent
    {
        public ProdutoCreatedEvent(ILogRequestContext ctx, Produto data) 
            : base(ctx, data) { }
    }
    public partial class ProdutoDeletedEvent : BaseEvent
    {
        public ProdutoDeletedEvent(ILogRequestContext ctx, Produto data) 
            : base(ctx, data) { }
    }
    public partial class ProdutoDeletedRangeEvent : BaseEvent
    {
        public ProdutoDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<Produto> data) 
            : base(ctx, data) { }
    }
    public partial class ProdutoActivatedEvent : BaseEvent
    {
        public ProdutoActivatedEvent(ILogRequestContext ctx, Produto data) 
            : base(ctx, data) { }
    }
    public partial class ProdutoUpdatedEvent : BaseEvent
    {
        public ProdutoUpdatedEvent(ILogRequestContext ctx, Produto data) 
            : base(ctx, data) { }
    }
    public partial class ProdutoUpdatedRangeEvent : BaseEvent
    {
        public ProdutoUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<Produto> data) 
            : base(ctx, data) { }
    }
    public partial class ProdutoDeactivatedEvent : BaseEvent
    {
        public ProdutoDeactivatedEvent(ILogRequestContext ctx, Produto data) 
            : base(ctx, data) { }
    }
    public partial class MarketPlaceAggSettingsCreatedEvent : BaseEvent
    {
        public MarketPlaceAggSettingsCreatedEvent(ILogRequestContext ctx, MarketPlaceAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class MarketPlaceAggSettingsDeletedEvent : BaseEvent
    {
        public MarketPlaceAggSettingsDeletedEvent(ILogRequestContext ctx, MarketPlaceAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class MarketPlaceAggSettingsDeletedRangeEvent : BaseEvent
    {
        public MarketPlaceAggSettingsDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<MarketPlaceAggSettings> data) 
            : base(ctx, data) { }
    }
    public partial class MarketPlaceAggSettingsActivatedEvent : BaseEvent
    {
        public MarketPlaceAggSettingsActivatedEvent(ILogRequestContext ctx, MarketPlaceAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class MarketPlaceAggSettingsUpdatedEvent : BaseEvent
    {
        public MarketPlaceAggSettingsUpdatedEvent(ILogRequestContext ctx, MarketPlaceAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class MarketPlaceAggSettingsUpdatedRangeEvent : BaseEvent
    {
        public MarketPlaceAggSettingsUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<MarketPlaceAggSettings> data) 
            : base(ctx, data) { }
    }
    public partial class MarketPlaceAggSettingsDeactivatedEvent : BaseEvent
    {
        public MarketPlaceAggSettingsDeactivatedEvent(ILogRequestContext ctx, MarketPlaceAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class CarrinhoCreatedEvent : BaseEvent
    {
        public CarrinhoCreatedEvent(ILogRequestContext ctx, Carrinho data) 
            : base(ctx, data) { }
    }
    public partial class CarrinhoDeletedEvent : BaseEvent
    {
        public CarrinhoDeletedEvent(ILogRequestContext ctx, Carrinho data) 
            : base(ctx, data) { }
    }
    public partial class CarrinhoDeletedRangeEvent : BaseEvent
    {
        public CarrinhoDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<Carrinho> data) 
            : base(ctx, data) { }
    }
    public partial class CarrinhoActivatedEvent : BaseEvent
    {
        public CarrinhoActivatedEvent(ILogRequestContext ctx, Carrinho data) 
            : base(ctx, data) { }
    }
    public partial class CarrinhoUpdatedEvent : BaseEvent
    {
        public CarrinhoUpdatedEvent(ILogRequestContext ctx, Carrinho data) 
            : base(ctx, data) { }
    }
    public partial class CarrinhoUpdatedRangeEvent : BaseEvent
    {
        public CarrinhoUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<Carrinho> data) 
            : base(ctx, data) { }
    }
    public partial class CarrinhoDeactivatedEvent : BaseEvent
    {
        public CarrinhoDeactivatedEvent(ILogRequestContext ctx, Carrinho data) 
            : base(ctx, data) { }
    }
    public partial class CategoriaprodutoCreatedEvent : BaseEvent
    {
        public CategoriaprodutoCreatedEvent(ILogRequestContext ctx, Categoriaproduto data) 
            : base(ctx, data) { }
    }
    public partial class CategoriaprodutoDeletedEvent : BaseEvent
    {
        public CategoriaprodutoDeletedEvent(ILogRequestContext ctx, Categoriaproduto data) 
            : base(ctx, data) { }
    }
    public partial class CategoriaprodutoDeletedRangeEvent : BaseEvent
    {
        public CategoriaprodutoDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<Categoriaproduto> data) 
            : base(ctx, data) { }
    }
    public partial class CategoriaprodutoActivatedEvent : BaseEvent
    {
        public CategoriaprodutoActivatedEvent(ILogRequestContext ctx, Categoriaproduto data) 
            : base(ctx, data) { }
    }
    public partial class CategoriaprodutoUpdatedEvent : BaseEvent
    {
        public CategoriaprodutoUpdatedEvent(ILogRequestContext ctx, Categoriaproduto data) 
            : base(ctx, data) { }
    }
    public partial class CategoriaprodutoUpdatedRangeEvent : BaseEvent
    {
        public CategoriaprodutoUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<Categoriaproduto> data) 
            : base(ctx, data) { }
    }
    public partial class CategoriaprodutoDeactivatedEvent : BaseEvent
    {
        public CategoriaprodutoDeactivatedEvent(ILogRequestContext ctx, Categoriaproduto data) 
            : base(ctx, data) { }
    }
}
