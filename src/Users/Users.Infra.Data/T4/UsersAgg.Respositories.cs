using Microsoft.Extensions.Configuration;
using LazyCrud.Core.Infra.Data.Repositories;
using LazyCrud.Users.Infra.Data.Context;

using LazyCrud.Users.Domain.Aggregates.UsersAgg.Entities;
using LazyCrud.Users.Domain.Aggregates.SystemSettingsAgg.Entities;

namespace LazyCrud.Users.Infra.Data.Aggregates.UsersAgg.Repositories
{
	using LazyCrud.Users.Domain.Aggregates.UsersAgg.Repositories;
	public partial class UserProfileAccessRepository : Repository<UserProfileAccess>, IUserProfileAccessRepository { public UserProfileAccessRepository(UsersAggContext ctx) : base(ctx) { } }

	public partial class UserCurrentAccessSelectedRepository : Repository<UserCurrentAccessSelected>, IUserCurrentAccessSelectedRepository { public UserCurrentAccessSelectedRepository(UsersAggContext ctx) : base(ctx) { } }

	public partial class UserProfileListRepository : Repository<UserProfileList>, IUserProfileListRepository { public UserProfileListRepository(UsersAggContext ctx) : base(ctx) { } }

	public partial class UserProfileRepository : Repository<UserProfile>, IUserProfileRepository { public UserProfileRepository(UsersAggContext ctx) : base(ctx) { } }

	public partial class UsersAggSettingsRepository : Repository<UsersAggSettings>, IUsersAggSettingsRepository { public UsersAggSettingsRepository(UsersAggContext ctx) : base(ctx) { } }

	public partial class UserRepository : Repository<User>, IUserRepository { public UserRepository(UsersAggContext ctx) : base(ctx) { } }

}
namespace LazyCrud.Users.Infra.Data.Aggregates.SystemSettingsAgg.Repositories
{
	using LazyCrud.Users.Domain.Aggregates.SystemSettingsAgg.Repositories;
	public partial class SystemPanelSubItemRepository : Repository<SystemPanelSubItem>, ISystemPanelSubItemRepository { public SystemPanelSubItemRepository(UsersAggContext ctx) : base(ctx) { } }

	public partial class SystemPanelRepository : Repository<SystemPanel>, ISystemPanelRepository { public SystemPanelRepository(UsersAggContext ctx) : base(ctx) { } }

}
