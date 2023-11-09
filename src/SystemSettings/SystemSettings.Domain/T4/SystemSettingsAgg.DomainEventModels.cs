using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Events;
using LazyCrudBuilder.CrossCutting.Infra.Log.Contexts;

namespace LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.ModelEvents
{
    using ModelEvents;
    using Entities;
    public partial class SystemPanelSubItemCreatedEvent : BaseEvent
    {
        public SystemPanelSubItemCreatedEvent(ILogRequestContext ctx, SystemPanelSubItem data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelSubItemDeletedEvent : BaseEvent
    {
        public SystemPanelSubItemDeletedEvent(ILogRequestContext ctx, SystemPanelSubItem data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelSubItemDeletedRangeEvent : BaseEvent
    {
        public SystemPanelSubItemDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<SystemPanelSubItem> data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelSubItemActivatedEvent : BaseEvent
    {
        public SystemPanelSubItemActivatedEvent(ILogRequestContext ctx, SystemPanelSubItem data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelSubItemUpdatedEvent : BaseEvent
    {
        public SystemPanelSubItemUpdatedEvent(ILogRequestContext ctx, SystemPanelSubItem data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelSubItemUpdatedRangeEvent : BaseEvent
    {
        public SystemPanelSubItemUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<SystemPanelSubItem> data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelSubItemDeactivatedEvent : BaseEvent
    {
        public SystemPanelSubItemDeactivatedEvent(ILogRequestContext ctx, SystemPanelSubItem data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelCreatedEvent : BaseEvent
    {
        public SystemPanelCreatedEvent(ILogRequestContext ctx, SystemPanel data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelDeletedEvent : BaseEvent
    {
        public SystemPanelDeletedEvent(ILogRequestContext ctx, SystemPanel data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelDeletedRangeEvent : BaseEvent
    {
        public SystemPanelDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<SystemPanel> data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelActivatedEvent : BaseEvent
    {
        public SystemPanelActivatedEvent(ILogRequestContext ctx, SystemPanel data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelUpdatedEvent : BaseEvent
    {
        public SystemPanelUpdatedEvent(ILogRequestContext ctx, SystemPanel data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelUpdatedRangeEvent : BaseEvent
    {
        public SystemPanelUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<SystemPanel> data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelDeactivatedEvent : BaseEvent
    {
        public SystemPanelDeactivatedEvent(ILogRequestContext ctx, SystemPanel data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelGroupCreatedEvent : BaseEvent
    {
        public SystemPanelGroupCreatedEvent(ILogRequestContext ctx, SystemPanelGroup data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelGroupDeletedEvent : BaseEvent
    {
        public SystemPanelGroupDeletedEvent(ILogRequestContext ctx, SystemPanelGroup data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelGroupDeletedRangeEvent : BaseEvent
    {
        public SystemPanelGroupDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<SystemPanelGroup> data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelGroupActivatedEvent : BaseEvent
    {
        public SystemPanelGroupActivatedEvent(ILogRequestContext ctx, SystemPanelGroup data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelGroupUpdatedEvent : BaseEvent
    {
        public SystemPanelGroupUpdatedEvent(ILogRequestContext ctx, SystemPanelGroup data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelGroupUpdatedRangeEvent : BaseEvent
    {
        public SystemPanelGroupUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<SystemPanelGroup> data) 
            : base(ctx, data) { }
    }
    public partial class SystemPanelGroupDeactivatedEvent : BaseEvent
    {
        public SystemPanelGroupDeactivatedEvent(ILogRequestContext ctx, SystemPanelGroup data) 
            : base(ctx, data) { }
    }
    public partial class CargaTabelaCreatedEvent : BaseEvent
    {
        public CargaTabelaCreatedEvent(ILogRequestContext ctx, CargaTabela data) 
            : base(ctx, data) { }
    }
    public partial class CargaTabelaDeletedEvent : BaseEvent
    {
        public CargaTabelaDeletedEvent(ILogRequestContext ctx, CargaTabela data) 
            : base(ctx, data) { }
    }
    public partial class CargaTabelaDeletedRangeEvent : BaseEvent
    {
        public CargaTabelaDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<CargaTabela> data) 
            : base(ctx, data) { }
    }
    public partial class CargaTabelaActivatedEvent : BaseEvent
    {
        public CargaTabelaActivatedEvent(ILogRequestContext ctx, CargaTabela data) 
            : base(ctx, data) { }
    }
    public partial class CargaTabelaUpdatedEvent : BaseEvent
    {
        public CargaTabelaUpdatedEvent(ILogRequestContext ctx, CargaTabela data) 
            : base(ctx, data) { }
    }
    public partial class CargaTabelaUpdatedRangeEvent : BaseEvent
    {
        public CargaTabelaUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<CargaTabela> data) 
            : base(ctx, data) { }
    }
    public partial class CargaTabelaDeactivatedEvent : BaseEvent
    {
        public CargaTabelaDeactivatedEvent(ILogRequestContext ctx, CargaTabela data) 
            : base(ctx, data) { }
    }
    public partial class SystemSettingsAggSettingsCreatedEvent : BaseEvent
    {
        public SystemSettingsAggSettingsCreatedEvent(ILogRequestContext ctx, SystemSettingsAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class SystemSettingsAggSettingsDeletedEvent : BaseEvent
    {
        public SystemSettingsAggSettingsDeletedEvent(ILogRequestContext ctx, SystemSettingsAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class SystemSettingsAggSettingsDeletedRangeEvent : BaseEvent
    {
        public SystemSettingsAggSettingsDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<SystemSettingsAggSettings> data) 
            : base(ctx, data) { }
    }
    public partial class SystemSettingsAggSettingsActivatedEvent : BaseEvent
    {
        public SystemSettingsAggSettingsActivatedEvent(ILogRequestContext ctx, SystemSettingsAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class SystemSettingsAggSettingsUpdatedEvent : BaseEvent
    {
        public SystemSettingsAggSettingsUpdatedEvent(ILogRequestContext ctx, SystemSettingsAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class SystemSettingsAggSettingsUpdatedRangeEvent : BaseEvent
    {
        public SystemSettingsAggSettingsUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<SystemSettingsAggSettings> data) 
            : base(ctx, data) { }
    }
    public partial class SystemSettingsAggSettingsDeactivatedEvent : BaseEvent
    {
        public SystemSettingsAggSettingsDeactivatedEvent(ILogRequestContext ctx, SystemSettingsAggSettings data) 
            : base(ctx, data) { }
    }
}
