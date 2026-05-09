namespace Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.CommandHandlers{
using Filters;
using ModelEvents;
using Repositories;
using CommandModels;
using Entities;
using Queries.Models;
using Application.DTO.Aggregates.ProductsAgg.Requests;
public class BaseProductsAggCommandHandler<T> : BaseCommandHandler<T> where T : IEntity {public BaseProductsAggCommandHandler(IServiceProvider provider,IMediator mediator):base(mediator, provider){}}
public partial class ProductsCommandHandler : BaseProductsAggCommandHandler<Products>,
    IRequestHandler<CreateProductsCommand,DomainResponse>,
    IRequestHandler<DeleteRangeProductsCommand,DomainResponse>,
    IRequestHandler<DeleteProductsCommand,DomainResponse>,
    IRequestHandler<UpdateRangeProductsCommand,DomainResponse>,
    IRequestHandler<UpdateProductsCommand,DomainResponse>,
    IRequestHandler<ActiveProductsCommand,DomainResponse>,
    IRequestHandler<DeactiveProductsCommand,DomainResponse>
{
    IProductsRepository _productsRepository;

    public ProductsCommandHandler(IServiceProvider provider,IMediator mediator,IProductsRepository productsRepository ) : base(provider,mediator) { _productsRepository = productsRepository; }

    partial void OnCreate(Products entity);
    partial void OnUpdate(Products entity);

    public async Task<DomainResponse> Handle(CreateProductsCommand command,CancellationToken cancellationToken) {

        Products entity;
        if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
        {
            var filter = ProductsFilters.GetFilters(command.Query ?? new ProductsQueryModel { ExternalIdEqual = command.Request.ExternalId });
            entity = await _productsRepository.FindAsync(filter, includeAll: false);
            if (entity != null)
            {
                if (command.UpdateIfExists)
                    return await Handle(new UpdateProductsCommand(
                        command.Context,
                        new Queries.Models.ProductsQueryModel { ExternalIdEqual = command.Request.ExternalId },
                        command.Request),
                    cancellationToken);
            }
        }
        entity = command.Request.ProjectedAs<Entities.Products>();
        entity.AddDomainEvent(new ProductsCreatedEvent(command.Context,entity));

        _productsRepository.UnitOfWork.ResolveAttaches(entity);
        var creationResult = await OnCreateAsync(entity);
        if (!creationResult.Success) return creationResult;
		_productsRepository.Add(entity);

        var result = await Commit(_productsRepository.UnitOfWork, entity.ProjectedAs<ProductsDTO>());
        result.Data = entity.ProjectedAs<ProductsDTO>();
        return result;
    }

    public async Task<DomainResponse> Handle(DeleteProductsCommand command,CancellationToken cancellationToken) {
        var filter = ProductsFilters.GetFilters(command.Query);
		var entity = await _productsRepository.FindAsync(filter);

        if(entity is null) {
            return AddError($"Entity {nameof(Products)} not found with the request.");
        }
        
        if (command.IsLogicalDeletion)
            entity.Delete();
        else
			_productsRepository.Delete(entity);

        var deleteResult = await OnDeleteAsync(entity);
        if (!deleteResult.Success) return deleteResult;

        entity.AddDomainEvent(new ProductsDeletedEvent(command.Context,entity));
        return await Commit(_productsRepository.UnitOfWork);
    }

    public async Task<DomainResponse> Handle(DeleteRangeProductsCommand command,CancellationToken cancellationToken) {
        var filter = ProductsFilters.GetFilters(command.Query);
		var entities = await _productsRepository.FindAllAsync(filter);

		_productsRepository.DeleteRange(entities);

        PublishLog(command);
        
        return await Commit(_productsRepository.UnitOfWork);
    }

    public async Task<DomainResponse> Handle(UpdateProductsCommand command,CancellationToken cancellationToken) {
        return await Handle(new UpdateRangeProductsCommand(command.Context,command.Query,command.Request),cancellationToken);
    }

    public async Task<DomainResponse> Handle(UpdateRangeProductsCommand command,CancellationToken cancellationToken) {
        var entities = new List<Products>();
        foreach (var item in command.Query)
        {
            var entity = command.Entity as Products ?? await _productsRepository.FindAsync(ProductsFilters.GetFilters(item.Key));
            
            if(entity == null) {
                if(command.CreateIfNotExists)
                    return await Handle(new CreateProductsCommand(command.Context,item.Value),cancellationToken);
                return AddError($"Entity {nameof(Products)} not found with the request.");
            }
            var entityAfter = item.Value.ProjectedAs<Products>();
            _productsRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
            entity.Update(entityAfter,"Id");
            var updateResult = await OnUpdateAsync(entity, entityAfter);
            if (!updateResult.Success) return updateResult;
            entity.AddDomainEvent(new ProductsUpdatedEvent(command.Context, entity));
        }
        
        PublishLog(command);

        return await Commit(_productsRepository.UnitOfWork, command.Entity.ProjectedAs<ProductsDTO>());
    }
     
    public async Task<DomainResponse> Handle(ActiveProductsCommand command,CancellationToken cancellationToken) {
        var products = await _productsRepository.FindAsync(filter: ProductsFilters.GetFilters(command.Query));
        //products.Disable();

        PublishLog(command);
        
        return await Commit(_productsRepository.UnitOfWork);
    }

    public async Task<DomainResponse> Handle(DeactiveProductsCommand command,CancellationToken cancellationToken) {
        var products = await _productsRepository.FindAsync(filter: ProductsFilters.GetFilters(command.Query));
        //products.Enable();

        PublishLog(command);
        
        return await Commit(_productsRepository.UnitOfWork);
    }
}
}
