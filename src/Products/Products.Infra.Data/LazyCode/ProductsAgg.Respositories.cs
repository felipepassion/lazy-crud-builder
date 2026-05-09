namespace Lazy.Crud.Products.Infra.Data.Aggregates.ProductsAgg.Repositories
{
	using Domain.Aggregates.ProductsAgg.Repositories;
	using Context;
	public partial class ProductsRepository : Repository<Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Entities.Products>, IProductsRepository { public ProductsRepository(ProductsAggContext ctx) : base(ctx) { } }

}
