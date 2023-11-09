using LazyCrud.Core.Domain.Aggregates.CommonAgg.Events;
using LazyCrud.CrossCutting.Infra.Log.Contexts;

namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.ModelEvents
{
    using ModelEvents;
    using Entities;
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
}
