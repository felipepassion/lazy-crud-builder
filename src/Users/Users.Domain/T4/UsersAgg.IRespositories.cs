using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Repositories;
using LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Entities;
using LazyCrudBuilder.Users.Domain.Aggregates.SystemSettingsAgg.Entities;

namespace LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Repositories 
{
	public partial interface IUserProfileAccessRepository : IRepository<UserProfileAccess> { }
	public partial interface IUserProfileAccessMongoRepository : IMongoRepository<UserProfileAccess> { }

	public partial interface IUserCurrentAccessSelectedRepository : IRepository<UserCurrentAccessSelected> { }
	public partial interface IUserCurrentAccessSelectedMongoRepository : IMongoRepository<UserCurrentAccessSelected> { }

	public partial interface IUserProfileListRepository : IRepository<UserProfileList> { }
	public partial interface IUserProfileListMongoRepository : IMongoRepository<UserProfileList> { }

	public partial interface IUserProfileRepository : IRepository<UserProfile> { }
	public partial interface IUserProfileMongoRepository : IMongoRepository<UserProfile> { }

	public partial interface IUsersAggSettingsRepository : IRepository<UsersAggSettings> { }
	public partial interface IUsersAggSettingsMongoRepository : IMongoRepository<UsersAggSettings> { }

	public partial interface IUserRepository : IRepository<User> { }
	public partial interface IUserMongoRepository : IMongoRepository<User> { }

	public partial interface IUserContactRepository : IRepository<UserContact> { }
	public partial interface IUserContactMongoRepository : IMongoRepository<UserContact> { }

}
namespace LazyCrudBuilder.Users.Domain.Aggregates.SystemSettingsAgg.Repositories 
{
	public partial interface ISystemPanelSubItemRepository : IRepository<SystemPanelSubItem> { }
	public partial interface ISystemPanelSubItemMongoRepository : IMongoRepository<SystemPanelSubItem> { }

	public partial interface ISystemPanelRepository : IRepository<SystemPanel> { }
	public partial interface ISystemPanelMongoRepository : IMongoRepository<SystemPanel> { }

	public partial interface ISystemPanelGroupRepository : IRepository<SystemPanelGroup> { }
	public partial interface ISystemPanelGroupMongoRepository : IMongoRepository<SystemPanelGroup> { }

}
