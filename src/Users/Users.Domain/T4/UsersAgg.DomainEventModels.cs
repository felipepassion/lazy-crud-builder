using LazyCrud.Core.Domain.Aggregates.CommonAgg.Events;
using LazyCrud.CrossCutting.Infra.Log.Contexts;

namespace LazyCrud.Users.Domain.Aggregates.UsersAgg.ModelEvents
{
    using ModelEvents;
    using Entities;
    public partial class UserProfileAccessCreatedEvent : BaseEvent
    {
        public UserProfileAccessCreatedEvent(ILogRequestContext ctx, UserProfileAccess data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileAccessDeletedEvent : BaseEvent
    {
        public UserProfileAccessDeletedEvent(ILogRequestContext ctx, UserProfileAccess data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileAccessDeletedRangeEvent : BaseEvent
    {
        public UserProfileAccessDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<UserProfileAccess> data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileAccessActivatedEvent : BaseEvent
    {
        public UserProfileAccessActivatedEvent(ILogRequestContext ctx, UserProfileAccess data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileAccessUpdatedEvent : BaseEvent
    {
        public UserProfileAccessUpdatedEvent(ILogRequestContext ctx, UserProfileAccess data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileAccessUpdatedRangeEvent : BaseEvent
    {
        public UserProfileAccessUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<UserProfileAccess> data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileAccessDeactivatedEvent : BaseEvent
    {
        public UserProfileAccessDeactivatedEvent(ILogRequestContext ctx, UserProfileAccess data) 
            : base(ctx, data) { }
    }
    public partial class UserCurrentAccessSelectedCreatedEvent : BaseEvent
    {
        public UserCurrentAccessSelectedCreatedEvent(ILogRequestContext ctx, UserCurrentAccessSelected data) 
            : base(ctx, data) { }
    }
    public partial class UserCurrentAccessSelectedDeletedEvent : BaseEvent
    {
        public UserCurrentAccessSelectedDeletedEvent(ILogRequestContext ctx, UserCurrentAccessSelected data) 
            : base(ctx, data) { }
    }
    public partial class UserCurrentAccessSelectedDeletedRangeEvent : BaseEvent
    {
        public UserCurrentAccessSelectedDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<UserCurrentAccessSelected> data) 
            : base(ctx, data) { }
    }
    public partial class UserCurrentAccessSelectedActivatedEvent : BaseEvent
    {
        public UserCurrentAccessSelectedActivatedEvent(ILogRequestContext ctx, UserCurrentAccessSelected data) 
            : base(ctx, data) { }
    }
    public partial class UserCurrentAccessSelectedUpdatedEvent : BaseEvent
    {
        public UserCurrentAccessSelectedUpdatedEvent(ILogRequestContext ctx, UserCurrentAccessSelected data) 
            : base(ctx, data) { }
    }
    public partial class UserCurrentAccessSelectedUpdatedRangeEvent : BaseEvent
    {
        public UserCurrentAccessSelectedUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<UserCurrentAccessSelected> data) 
            : base(ctx, data) { }
    }
    public partial class UserCurrentAccessSelectedDeactivatedEvent : BaseEvent
    {
        public UserCurrentAccessSelectedDeactivatedEvent(ILogRequestContext ctx, UserCurrentAccessSelected data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileListCreatedEvent : BaseEvent
    {
        public UserProfileListCreatedEvent(ILogRequestContext ctx, UserProfileList data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileListDeletedEvent : BaseEvent
    {
        public UserProfileListDeletedEvent(ILogRequestContext ctx, UserProfileList data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileListDeletedRangeEvent : BaseEvent
    {
        public UserProfileListDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<UserProfileList> data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileListActivatedEvent : BaseEvent
    {
        public UserProfileListActivatedEvent(ILogRequestContext ctx, UserProfileList data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileListUpdatedEvent : BaseEvent
    {
        public UserProfileListUpdatedEvent(ILogRequestContext ctx, UserProfileList data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileListUpdatedRangeEvent : BaseEvent
    {
        public UserProfileListUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<UserProfileList> data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileListDeactivatedEvent : BaseEvent
    {
        public UserProfileListDeactivatedEvent(ILogRequestContext ctx, UserProfileList data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileCreatedEvent : BaseEvent
    {
        public UserProfileCreatedEvent(ILogRequestContext ctx, UserProfile data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileDeletedEvent : BaseEvent
    {
        public UserProfileDeletedEvent(ILogRequestContext ctx, UserProfile data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileDeletedRangeEvent : BaseEvent
    {
        public UserProfileDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<UserProfile> data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileActivatedEvent : BaseEvent
    {
        public UserProfileActivatedEvent(ILogRequestContext ctx, UserProfile data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileUpdatedEvent : BaseEvent
    {
        public UserProfileUpdatedEvent(ILogRequestContext ctx, UserProfile data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileUpdatedRangeEvent : BaseEvent
    {
        public UserProfileUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<UserProfile> data) 
            : base(ctx, data) { }
    }
    public partial class UserProfileDeactivatedEvent : BaseEvent
    {
        public UserProfileDeactivatedEvent(ILogRequestContext ctx, UserProfile data) 
            : base(ctx, data) { }
    }
    public partial class UsersAggSettingsCreatedEvent : BaseEvent
    {
        public UsersAggSettingsCreatedEvent(ILogRequestContext ctx, UsersAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class UsersAggSettingsDeletedEvent : BaseEvent
    {
        public UsersAggSettingsDeletedEvent(ILogRequestContext ctx, UsersAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class UsersAggSettingsDeletedRangeEvent : BaseEvent
    {
        public UsersAggSettingsDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<UsersAggSettings> data) 
            : base(ctx, data) { }
    }
    public partial class UsersAggSettingsActivatedEvent : BaseEvent
    {
        public UsersAggSettingsActivatedEvent(ILogRequestContext ctx, UsersAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class UsersAggSettingsUpdatedEvent : BaseEvent
    {
        public UsersAggSettingsUpdatedEvent(ILogRequestContext ctx, UsersAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class UsersAggSettingsUpdatedRangeEvent : BaseEvent
    {
        public UsersAggSettingsUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<UsersAggSettings> data) 
            : base(ctx, data) { }
    }
    public partial class UsersAggSettingsDeactivatedEvent : BaseEvent
    {
        public UsersAggSettingsDeactivatedEvent(ILogRequestContext ctx, UsersAggSettings data) 
            : base(ctx, data) { }
    }
    public partial class UserCreatedEvent : BaseEvent
    {
        public UserCreatedEvent(ILogRequestContext ctx, User data) 
            : base(ctx, data) { }
    }
    public partial class UserDeletedEvent : BaseEvent
    {
        public UserDeletedEvent(ILogRequestContext ctx, User data) 
            : base(ctx, data) { }
    }
    public partial class UserDeletedRangeEvent : BaseEvent
    {
        public UserDeletedRangeEvent(ILogRequestContext ctx, IEnumerable<User> data) 
            : base(ctx, data) { }
    }
    public partial class UserActivatedEvent : BaseEvent
    {
        public UserActivatedEvent(ILogRequestContext ctx, User data) 
            : base(ctx, data) { }
    }
    public partial class UserUpdatedEvent : BaseEvent
    {
        public UserUpdatedEvent(ILogRequestContext ctx, User data) 
            : base(ctx, data) { }
    }
    public partial class UserUpdatedRangeEvent : BaseEvent
    {
        public UserUpdatedRangeEvent(ILogRequestContext ctx, IEnumerable<User> data) 
            : base(ctx, data) { }
    }
    public partial class UserDeactivatedEvent : BaseEvent
    {
        public UserDeactivatedEvent(ILogRequestContext ctx, User data) 
            : base(ctx, data) { }
    }
}
