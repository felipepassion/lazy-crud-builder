using Lazy.Crud.Core.Domain.Aggregates.CommonAgg.Repositories;
using Lazy.Crud.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Entities;
using Lazy.Crud.DefaultTemplate.Domain.Aggregates.UsersAgg.Entities;

namespace Lazy.Crud.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Repositories 
{
	public partial interface IDefaultEntityRepository : IRepository<DefaultEntity> { }
	public partial interface IDefaultEntityMongoRepository : IMongoRepository<DefaultEntity> { }

	public partial interface IDefaultTemplateAggSettingsRepository : IRepository<DefaultTemplateAggSettings> { }
	public partial interface IDefaultTemplateAggSettingsMongoRepository : IMongoRepository<DefaultTemplateAggSettings> { }

}
namespace Lazy.Crud.DefaultTemplate.Domain.Aggregates.UsersAgg.Repositories 
{
	public partial interface IUserRepository : IRepository<User> { }
	public partial interface IUserMongoRepository : IMongoRepository<User> { }

}
