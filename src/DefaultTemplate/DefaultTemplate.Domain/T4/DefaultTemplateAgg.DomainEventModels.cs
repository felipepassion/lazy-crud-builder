using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Events;
using Niu.Nutri.CrossCutting.Infra.Log.Contexts;

namespace Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.ModelEvents
{
    using ModelEvents;
    using Entities;
    public partial class DefaultEntityCreatedEvent : BaseEvent
    {
        public DefaultEntityCreatedEvent(ILogRequestContext ctx, DefaultEntity data) 
            : base(ctx, data) { }
    }
    public partial class DefaultEntityDeletedEvent : BaseEvent
    {
        public DefaultEntityDeletedEvent(ILogRequestContext ctx, DefaultEntity data) 
            : base(ctx, data) { }
    }
    public partial class DefaultEntityDeletedRangeEvent : BaseEvent
    {
        public DefaultEntityDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<DefaultEntity> data) 
            : base(ctx, data) { }
    }
    public partial class DefaultEntityActivatedEvent : BaseEvent
    {
        public DefaultEntityActivatedEvent(ILogRequestContext ctx, DefaultEntity data) 
            : base(ctx, data) { }
    }
    public partial class DefaultEntityUpdatedEvent : BaseEvent
    {
        public DefaultEntityUpdatedEvent(ILogRequestContext ctx, DefaultEntity data) 
            : base(ctx, data) { }
    }
    public partial class DefaultEntityUpdatedRangeEvent : BaseEvent
    {
        public DefaultEntityUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<DefaultEntity> data) 
            : base(ctx, data) { }
    }
    public partial class DefaultEntityDeactivatedEvent : BaseEvent
    {
        public DefaultEntityDeactivatedEvent(ILogRequestContext ctx, DefaultEntity data) 
            : base(ctx, data) { }
    }
    public partial class DefaultTemplateAggSettingsCreatedEvent : BaseEvent
    {
        public DefaultTemplateAggSettingsCreatedEvent(ILogRequestContext ctx, DefaultTemplateAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class DefaultTemplateAggSettingsDeletedEvent : BaseEvent
    {
        public DefaultTemplateAggSettingsDeletedEvent(ILogRequestContext ctx, DefaultTemplateAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class DefaultTemplateAggSettingsDeletedRangeEvent : BaseEvent
    {
        public DefaultTemplateAggSettingsDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<DefaultTemplateAggSettings> data) 
            : base(ctx, data) { }
    }
    public partial class DefaultTemplateAggSettingsActivatedEvent : BaseEvent
    {
        public DefaultTemplateAggSettingsActivatedEvent(ILogRequestContext ctx, DefaultTemplateAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class DefaultTemplateAggSettingsUpdatedEvent : BaseEvent
    {
        public DefaultTemplateAggSettingsUpdatedEvent(ILogRequestContext ctx, DefaultTemplateAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class DefaultTemplateAggSettingsUpdatedRangeEvent : BaseEvent
    {
        public DefaultTemplateAggSettingsUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<DefaultTemplateAggSettings> data) 
            : base(ctx, data) { }
    }
    public partial class DefaultTemplateAggSettingsDeactivatedEvent : BaseEvent
    {
        public DefaultTemplateAggSettingsDeactivatedEvent(ILogRequestContext ctx, DefaultTemplateAggSettings data) 
            : base(ctx, data) { }
    }
}
