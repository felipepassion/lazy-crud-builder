using MediatR;
using LazyCrudBuilder.Core.Domain.Extensions;
using LazyCrudBuilder.CrossCutting.Infra.Log.Providers;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Events.Handles;
using LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.ModelEvents;

namespace LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.EventHandlers
{
    public partial class UserProfileAccessEventHandler : BaseEventHandler,
        INotificationHandler<UserProfileAccessCreatedEvent>,
        INotificationHandler<UserProfileAccessDeletedEvent>,
        INotificationHandler<UserProfileAccessUpdatedEvent>,
        INotificationHandler<UserProfileAccessActivatedEvent>,
        INotificationHandler<UserProfileAccessDeactivatedEvent>{
        public UserProfileAccessEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(UserProfileAccessCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserProfileAccessDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserProfileAccessActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserProfileAccessUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserProfileAccessDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
    public partial class UserCurrentAccessSelectedEventHandler : BaseEventHandler,
        INotificationHandler<UserCurrentAccessSelectedCreatedEvent>,
        INotificationHandler<UserCurrentAccessSelectedDeletedEvent>,
        INotificationHandler<UserCurrentAccessSelectedUpdatedEvent>,
        INotificationHandler<UserCurrentAccessSelectedActivatedEvent>,
        INotificationHandler<UserCurrentAccessSelectedDeactivatedEvent>{
        public UserCurrentAccessSelectedEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(UserCurrentAccessSelectedCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserCurrentAccessSelectedDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserCurrentAccessSelectedActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserCurrentAccessSelectedUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserCurrentAccessSelectedDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
    public partial class UserProfileListEventHandler : BaseEventHandler,
        INotificationHandler<UserProfileListCreatedEvent>,
        INotificationHandler<UserProfileListDeletedEvent>,
        INotificationHandler<UserProfileListUpdatedEvent>,
        INotificationHandler<UserProfileListActivatedEvent>,
        INotificationHandler<UserProfileListDeactivatedEvent>{
        public UserProfileListEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(UserProfileListCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserProfileListDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserProfileListActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserProfileListUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserProfileListDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
    public partial class UserProfileEventHandler : BaseEventHandler,
        INotificationHandler<UserProfileCreatedEvent>,
        INotificationHandler<UserProfileDeletedEvent>,
        INotificationHandler<UserProfileUpdatedEvent>,
        INotificationHandler<UserProfileActivatedEvent>,
        INotificationHandler<UserProfileDeactivatedEvent>{
        public UserProfileEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(UserProfileCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserProfileDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserProfileActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserProfileUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserProfileDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
    public partial class UsersAggSettingsEventHandler : BaseEventHandler,
        INotificationHandler<UsersAggSettingsCreatedEvent>,
        INotificationHandler<UsersAggSettingsDeletedEvent>,
        INotificationHandler<UsersAggSettingsUpdatedEvent>,
        INotificationHandler<UsersAggSettingsActivatedEvent>,
        INotificationHandler<UsersAggSettingsDeactivatedEvent>{
        public UsersAggSettingsEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(UsersAggSettingsCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UsersAggSettingsDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UsersAggSettingsActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UsersAggSettingsUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UsersAggSettingsDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
    public partial class UserEventHandler : BaseEventHandler,
        INotificationHandler<UserCreatedEvent>,
        INotificationHandler<UserDeletedEvent>,
        INotificationHandler<UserUpdatedEvent>,
        INotificationHandler<UserActivatedEvent>,
        INotificationHandler<UserDeactivatedEvent>{
        public UserEventHandler(ILogProvider logProvider, IServiceProvider serviceProvider):base(logProvider, serviceProvider){}
        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserActivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
        public async Task Handle(UserDeactivatedEvent notification, CancellationToken cancellationToken){PublishLog(notification);}
    }
}
