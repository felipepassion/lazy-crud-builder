using System.Linq.Expressions;
using Lazy.Crud.Builder.Domain.CrossCutting;
using Lazy.Crud.Builder.Application.Aggregates.Common;

namespace Lazy.Crud.Products.Application.Aggregates.ProductsAgg.AppServices {
using Application.DTO.Aggregates.ProductsAgg.Requests;
using Domain.Aggregates.ProductsAgg.Queries.Models;
public partial interface IProductsAppService : IBaseAppService {
	Task<ProductsDTO> Get(ProductsQueryModel request);
	Task<int> CountAsync(ProductsQueryModel request);
	Task<IEnumerable<ProductsDTO>> GetAll(ProductsQueryModel request, int? page = null, int? size = null);
	Task<T> Select<T>(ProductsQueryModel request, Expression<Func<Domain.Aggregates.ProductsAgg.Entities.Products, T>> selector = null);
	Task<IEnumerable<T>> GetAll<T>(ProductsQueryModel request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.ProductsAgg.Entities.Products, T>> selector = null);
	Task<DomainResponse> Create(ProductsDTO request, bool updateIfExists = true, ProductsQueryModel searchQuery = null);
	Task<DomainResponse> Delete(ProductsQueryModel request);
	Task<DomainResponse> DeleteRange(ProductsQueryModel request);
	Task Update(ProductsQueryModel searchQuery, ProductsDTO request, bool createIfNotExists = true);
}
}
