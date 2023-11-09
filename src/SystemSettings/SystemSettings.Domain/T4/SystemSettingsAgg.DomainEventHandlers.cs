using MediatR;
using LazyCrudBuilder.Core.Domain.Extensions;
using LazyCrudBuilder.CrossCutting.Infra.Log.Providers;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Events.Handles;
using LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.ModelEvents;

namespace LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.EventHandlers
{
    public partial class SystemPanelSubItemEventHandler : BaseEventHandler,
        INotificationHandler<SystemPanelSubItemCreatedEvent>,
        INotificationHandler<SystemPanelSubItemDeletedEvent>,
        INotificationHandler<SystemPanelSubItemUpdatedEvent>,
        INotificationHandler<SystemPanelSubItemActivatedEvent>,
        INotificationHandler<SystemPanelSubItemDeactivatedEvent>{
        public SystemPanelSubItemEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(SystemPanelSubItemCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemPanelSubItemDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemPanelSubItemActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemPanelSubItemUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemPanelSubItemDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
    public partial class SystemPanelEventHandler : BaseEventHandler,
        INotificationHandler<SystemPanelCreatedEvent>,
        INotificationHandler<SystemPanelDeletedEvent>,
        INotificationHandler<SystemPanelUpdatedEvent>,
        INotificationHandler<SystemPanelActivatedEvent>,
        INotificationHandler<SystemPanelDeactivatedEvent>{
        public SystemPanelEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(SystemPanelCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemPanelDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemPanelActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemPanelUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemPanelDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
    public partial class SystemPanelGroupEventHandler : BaseEventHandler,
        INotificationHandler<SystemPanelGroupCreatedEvent>,
        INotificationHandler<SystemPanelGroupDeletedEvent>,
        INotificationHandler<SystemPanelGroupUpdatedEvent>,
        INotificationHandler<SystemPanelGroupActivatedEvent>,
        INotificationHandler<SystemPanelGroupDeactivatedEvent>{
        public SystemPanelGroupEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(SystemPanelGroupCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemPanelGroupDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemPanelGroupActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemPanelGroupUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemPanelGroupDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
    public partial class CargaTabelaEventHandler : BaseEventHandler,
        INotificationHandler<CargaTabelaCreatedEvent>,
        INotificationHandler<CargaTabelaDeletedEvent>,
        INotificationHandler<CargaTabelaUpdatedEvent>,
        INotificationHandler<CargaTabelaActivatedEvent>,
        INotificationHandler<CargaTabelaDeactivatedEvent>{
        public CargaTabelaEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(CargaTabelaCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(CargaTabelaDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(CargaTabelaActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(CargaTabelaUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(CargaTabelaDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
    public partial class SystemSettingsAggSettingsEventHandler : BaseEventHandler,
        INotificationHandler<SystemSettingsAggSettingsCreatedEvent>,
        INotificationHandler<SystemSettingsAggSettingsDeletedEvent>,
        INotificationHandler<SystemSettingsAggSettingsUpdatedEvent>,
        INotificationHandler<SystemSettingsAggSettingsActivatedEvent>,
        INotificationHandler<SystemSettingsAggSettingsDeactivatedEvent>{
        public SystemSettingsAggSettingsEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(SystemSettingsAggSettingsCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemSettingsAggSettingsDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemSettingsAggSettingsActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemSettingsAggSettingsUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(SystemSettingsAggSettingsDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
}
