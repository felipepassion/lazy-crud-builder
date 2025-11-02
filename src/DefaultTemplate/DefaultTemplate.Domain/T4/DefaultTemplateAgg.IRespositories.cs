using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Repositories;
using Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Entities;
using Niu.Nutri.DefaultTemplate.Domain.Aggregates.UsersAgg.Entities;

namespace Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Repositories 
{
	public partial interface IDefaultEntityRepository : IRepository<DefaultEntity> { }
	public partial interface IDefaultEntityMongoRepository : IMongoRepository<DefaultEntity> { }

	public partial interface IDefaultTemplateAggSettingsRepository : IRepository<DefaultTemplateAggSettings> { }
	public partial interface IDefaultTemplateAggSettingsMongoRepository : IMongoRepository<DefaultTemplateAggSettings> { }

}
namespace Niu.Nutri.DefaultTemplate.Domain.Aggregates.UsersAgg.Repositories 
{
	public partial interface IUserRepository : IRepository<User> { }
	public partial interface IUserMongoRepository : IMongoRepository<User> { }

}
