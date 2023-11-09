using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Repositories;
using LazyCrudBuilder.SystemSettings.Domain.Aggregates.UsersAgg.Entities;
using LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities;

namespace LazyCrudBuilder.SystemSettings.Domain.Aggregates.UsersAgg.Repositories 
{
	public partial interface IUserProfileAccessRepository : IRepository<UserProfileAccess> { }
	public partial interface IUserProfileAccessMongoRepository : IMongoRepository<UserProfileAccess> { }

	public partial interface IUserRepository : IRepository<User> { }
	public partial interface IUserMongoRepository : IMongoRepository<User> { }

}
namespace LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Repositories 
{
	public partial interface ISystemPanelSubItemRepository : IRepository<SystemPanelSubItem> { }
	public partial interface ISystemPanelSubItemMongoRepository : IMongoRepository<SystemPanelSubItem> { }

	public partial interface ISystemPanelRepository : IRepository<SystemPanel> { }
	public partial interface ISystemPanelMongoRepository : IMongoRepository<SystemPanel> { }

	public partial interface ISystemPanelGroupRepository : IRepository<SystemPanelGroup> { }
	public partial interface ISystemPanelGroupMongoRepository : IMongoRepository<SystemPanelGroup> { }

	public partial interface ICargaTabelaRepository : IRepository<CargaTabela> { }
	public partial interface ICargaTabelaMongoRepository : IMongoRepository<CargaTabela> { }

	public partial interface ISystemSettingsAggSettingsRepository : IRepository<SystemSettingsAggSettings> { }
	public partial interface ISystemSettingsAggSettingsMongoRepository : IMongoRepository<SystemSettingsAggSettings> { }

}
