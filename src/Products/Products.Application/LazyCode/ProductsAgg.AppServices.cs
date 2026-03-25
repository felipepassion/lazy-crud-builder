namespace Lazy.Crud.Products.Application.Aggregates.ProductsAgg.AppServices {
using Application.DTO.Aggregates.ProductsAgg.Requests;
using Domain.Aggregates.ProductsAgg.Queries.Models;
using Domain.Aggregates.ProductsAgg.Repositories;
using Domain.Aggregates.ProductsAgg.Filters;
using Domain.Aggregates.ProductsAgg.Entities;
using Domain.Aggregates.ProductsAgg.CommandModels;
public partial class ProductsAppService : BaseAppService, IProductsAppService {	
	public IProductsRepository _productsRepository;
	public ProductsAppService(IMediator mediator, CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, IProductsRepository productsRepository) : base(ctx, mediator) { _productsRepository = productsRepository; }	
	public async Task<ProductsDTO> Get(ProductsQueryModel request) {
        return (await _productsRepository.FindAsync(filter: ProductsFilters.GetFilters(request, isOrSpecification: request.IsOrSpecification), selector: x => x.ProjectedAs<ProductsDTO>()));
    }
	public void Dispose(){ _productsRepository = null; }
	public async Task<IEnumerable<T>> GetAll<T>(ProductsQueryModel request, int? page = null, int? size = null, Expression<Func<Products, T>> selector = null) {
		return await _productsRepository.SelectAllAsync(
            filter: ProductsFilters.GetFilters(request, isOrSpecification: request.IsOrSpecification),
            take: size,
            skip: page * size,
			ascending: request.OrderByDesc != true,
            orderBy: request.OrderBy.GetPropertyListSelector<Products>(),
            selector: selector);
	}
	public async Task<T> Select<T>(ProductsQueryModel request, Expression<Func<Products, T>> selector = null){
        return (await _productsRepository.FindAsync(filter: ProductsFilters.GetFilters(request, isOrSpecification: true), selector: selector));
    }
    public async Task<IEnumerable<ProductsDTO>> GetAll(ProductsQueryModel request, int? page = null, int? size = null) {
        return await _productsRepository.FindAllAsync(
            filter: ProductsFilters.GetFilters(request, isOrSpecification: true),
            take: size,
            skip: page * size,
			ascending: request.OrderByDesc != true,
            orderBy: request.OrderBy.GetPropertyListSelector<Products>(),
            selector: x => x.ProjectedAs<ProductsDTO>());
    }
	public Task<DomainResponse> Create(ProductsDTO request, bool updateIfExists = true, ProductsQueryModel searchQuery = null){ return _mediator.Send(new CreateProductsCommand(_logRequestContext, request)); }
	public async Task<int> CountAsync(ProductsQueryModel request){ return await _productsRepository.CountAsync(filter: ProductsFilters.GetFilters(request, isOrSpecification: true)); }
	public Task Update(ProductsQueryModel searchQuery, ProductsDTO request, bool createIfNotExists = true){ return _mediator.Send(new UpdateProductsCommand(_logRequestContext, searchQuery, request)); }
	public Task<DomainResponse> DeleteRange(ProductsQueryModel request){ return _mediator.Send(new DeleteRangeProductsCommand(_logRequestContext, request)); }
	public Task<DomainResponse> Delete(ProductsQueryModel request){ return _mediator.Send(new DeleteProductsCommand(_logRequestContext, request)); }
}
}
