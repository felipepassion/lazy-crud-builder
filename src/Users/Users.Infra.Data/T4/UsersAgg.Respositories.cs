using Microsoft.Extensions.Configuration;
using LazyCrudBuilder.Core.Infra.Data.Repositories;
using LazyCrudBuilder.Users.Infra.Data.Context;

using LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Entities;
using LazyCrudBuilder.Users.Domain.Aggregates.SystemSettingsAgg.Entities;

namespace LazyCrudBuilder.Users.Infra.Data.Aggregates.UsersAgg.Repositories
{
	using LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Repositories;
	public partial class UserProfileAccessRepository : Repository<UserProfileAccess>, IUserProfileAccessRepository { public UserProfileAccessRepository(UsersAggContext ctx) : base(ctx) { } }

	public partial class UserCurrentAccessSelectedRepository : Repository<UserCurrentAccessSelected>, IUserCurrentAccessSelectedRepository { public UserCurrentAccessSelectedRepository(UsersAggContext ctx) : base(ctx) { } }

	public partial class UserProfileListRepository : Repository<UserProfileList>, IUserProfileListRepository { public UserProfileListRepository(UsersAggContext ctx) : base(ctx) { } }

	public partial class UserProfileRepository : Repository<UserProfile>, IUserProfileRepository { public UserProfileRepository(UsersAggContext ctx) : base(ctx) { } }

	public partial class UsersAggSettingsRepository : Repository<UsersAggSettings>, IUsersAggSettingsRepository { public UsersAggSettingsRepository(UsersAggContext ctx) : base(ctx) { } }

	public partial class UserRepository : Repository<User>, IUserRepository { public UserRepository(UsersAggContext ctx) : base(ctx) { } }

}
namespace LazyCrudBuilder.Users.Infra.Data.Aggregates.SystemSettingsAgg.Repositories
{
	using LazyCrudBuilder.Users.Domain.Aggregates.SystemSettingsAgg.Repositories;
	public partial class SystemPanelSubItemRepository : Repository<SystemPanelSubItem>, ISystemPanelSubItemRepository { public SystemPanelSubItemRepository(UsersAggContext ctx) : base(ctx) { } }

	public partial class SystemPanelRepository : Repository<SystemPanel>, ISystemPanelRepository { public SystemPanelRepository(UsersAggContext ctx) : base(ctx) { } }

}
