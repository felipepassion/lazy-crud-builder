using LazyCrud.Core.Domain.Aggregates.CommonAgg.Repositories;
using LazyCrud.MarketPlace.Domain.Aggregates.UsersAgg.Entities;
using LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Entities;

namespace LazyCrud.MarketPlace.Domain.Aggregates.UsersAgg.Repositories 
{
	public partial interface IUserRepository : IRepository<User> { }
	public partial interface IUserMongoRepository : IMongoRepository<User> { }

}
namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Repositories 
{
	public partial interface IProdutoRepository : IRepository<Produto> { }
	public partial interface IProdutoMongoRepository : IMongoRepository<Produto> { }

	public partial interface IMarketPlaceAggSettingsRepository : IRepository<MarketPlaceAggSettings> { }
	public partial interface IMarketPlaceAggSettingsMongoRepository : IMongoRepository<MarketPlaceAggSettings> { }

	public partial interface ICarrinhoRepository : IRepository<Carrinho> { }
	public partial interface ICarrinhoMongoRepository : IMongoRepository<Carrinho> { }

	public partial interface ICategoriaprodutoRepository : IRepository<Categoriaproduto> { }
	public partial interface ICategoriaprodutoMongoRepository : IMongoRepository<Categoriaproduto> { }

}
