using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Repositories;
using LazyCrudBuilder.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Entities;
using LazyCrudBuilder.MarketPlace.Domain.Aggregates.UsersAgg.Entities;

namespace LazyCrudBuilder.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Repositories 
{
	public partial interface IMarketPlaceAggSettingsRepository : IRepository<MarketPlaceAggSettings> { }
	public partial interface IMarketPlaceAggSettingsMongoRepository : IMongoRepository<MarketPlaceAggSettings> { }

}
namespace LazyCrudBuilder.MarketPlace.Domain.Aggregates.UsersAgg.Repositories 
{
	public partial interface IUserRepository : IRepository<User> { }
	public partial interface IUserMongoRepository : IMongoRepository<User> { }

}
