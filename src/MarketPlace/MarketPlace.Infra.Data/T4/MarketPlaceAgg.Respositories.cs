using Microsoft.Extensions.Configuration;
using LazyCrud.Core.Infra.Data.Repositories;
using LazyCrud.MarketPlace.Infra.Data.Context;

using LazyCrud.MarketPlace.Domain.Aggregates.UsersAgg.Entities;
using LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Entities;

namespace LazyCrud.MarketPlace.Infra.Data.Aggregates.UsersAgg.Repositories
{
	using LazyCrud.MarketPlace.Domain.Aggregates.UsersAgg.Repositories;
	public partial class ProdutoRepository : Repository<Produto>, IProdutoRepository { public ProdutoRepository(MarketPlaceAggContext ctx) : base(ctx) { } }

	public partial class CarrinhoRepository : Repository<Carrinho>, ICarrinhoRepository { public CarrinhoRepository(MarketPlaceAggContext ctx) : base(ctx) { } }

	public partial class CategoriaprodutoRepository : Repository<Categoriaproduto>, ICategoriaprodutoRepository { public CategoriaprodutoRepository(MarketPlaceAggContext ctx) : base(ctx) { } }

}
namespace LazyCrud.MarketPlace.Infra.Data.Aggregates.MarketPlaceAgg.Repositories
{
	using LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Repositories;
	public partial class MarketPlaceAggSettingsRepository : Repository<MarketPlaceAggSettings>, IMarketPlaceAggSettingsRepository { public MarketPlaceAggSettingsRepository(MarketPlaceAggContext ctx) : base(ctx) { } }

}
