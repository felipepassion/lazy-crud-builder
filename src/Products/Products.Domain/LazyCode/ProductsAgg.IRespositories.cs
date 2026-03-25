// Removed global usings now centralized
using Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Entities;

namespace Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Repositories 
{
public partial interface IProductsRepository : IRepository<Entities.Products> { }
public partial interface IProductsMongoRepository : IMongoRepository<Entities.Products> { }

}
