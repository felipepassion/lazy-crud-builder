using MediatR;
using Niu.Nutri.Core.Domain.Extensions;
using Niu.Nutri.CrossCutting.Infra.Log.Providers;
using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Events.Handles;
using Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.ModelEvents;

namespace Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.EventHandlers
{
    public partial class DefaultEntityEventHandler : BaseEventHandler,
        INotificationHandler<DefaultEntityCreatedEvent>,
        INotificationHandler<DefaultEntityDeletedEvent>,
        INotificationHandler<DefaultEntityUpdatedEvent>,
        INotificationHandler<DefaultEntityActivatedEvent>,
        INotificationHandler<DefaultEntityDeactivatedEvent>{
        public DefaultEntityEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(DefaultEntityCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(DefaultEntityDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(DefaultEntityActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(DefaultEntityUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(DefaultEntityDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
    public partial class DefaultTemplateAggSettingsEventHandler : BaseEventHandler,
        INotificationHandler<DefaultTemplateAggSettingsCreatedEvent>,
        INotificationHandler<DefaultTemplateAggSettingsDeletedEvent>,
        INotificationHandler<DefaultTemplateAggSettingsUpdatedEvent>,
        INotificationHandler<DefaultTemplateAggSettingsActivatedEvent>,
        INotificationHandler<DefaultTemplateAggSettingsDeactivatedEvent>{
        public DefaultTemplateAggSettingsEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(DefaultTemplateAggSettingsCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(DefaultTemplateAggSettingsDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(DefaultTemplateAggSettingsActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(DefaultTemplateAggSettingsUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(DefaultTemplateAggSettingsDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
}
