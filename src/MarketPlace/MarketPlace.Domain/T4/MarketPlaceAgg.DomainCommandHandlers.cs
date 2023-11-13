
using MediatR;
using FluentValidation.Results;
using LazyCrud.Core.Domain.CrossCutting;
using LazyCrud.Core.Application.DTO.Extensions;
using LazyCrud.CrossCuting.Infra.Utils.Extensions;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.ValueObjects;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Commands.Handles;

namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.CommandHandlers
{
    using Filters;
    using ModelEvents;
    using Repositories;
    using CommandModels;
    using Entities;
    using Specifications;
    using Queries.Models;
    
    public class BaseMarketPlaceAggCommandHandler<T> : BaseCommandHandler<T> where T : IEntity {public BaseMarketPlaceAggCommandHandler(IServiceProvider provider,IMediator mediator):base(mediator, provider){}}
    public partial class ProdutoCommandHandler : BaseMarketPlaceAggCommandHandler<Produto>,
        IRequestHandler<CreateProdutoCommand,DomainResponse>,
        IRequestHandler<DeleteRangeProdutoCommand,DomainResponse>,
        IRequestHandler<DeleteProdutoCommand,DomainResponse>,
        IRequestHandler<UpdateRangeProdutoCommand,DomainResponse>,
        IRequestHandler<UpdateProdutoCommand,DomainResponse>,
        IRequestHandler<ActiveProdutoCommand,DomainResponse>,
        IRequestHandler<DeactiveProdutoCommand,DomainResponse>
    {
        IProdutoRepository _produtoRepository;

        public ProdutoCommandHandler(IServiceProvider provider,IMediator mediator,IProdutoRepository produtoRepository ) : base(provider,mediator) { _produtoRepository = produtoRepository; }

        partial void OnCreate(Produto entity);
        partial void OnUpdate(Produto entity);

        public async Task<DomainResponse> Handle(CreateProdutoCommand command,CancellationToken cancellationToken) {

            Produto entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = ProdutoFilters.GetFilters(command.Query ?? new ProdutoQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _produtoRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateProdutoCommand(
                            command.Context,
                            new Queries.Models.ProdutoQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.Produto>();
            entity.AddDomainEvent(new ProdutoCreatedEvent(command.Context,entity));

            _produtoRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_produtoRepository.Add(entity);

            return await Commit(_produtoRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteProdutoCommand command,CancellationToken cancellationToken) {
            var filter = ProdutoFilters.GetFilters(command.Query);
			var entity = await _produtoRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(Produto)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _produtoRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new ProdutoDeletedEvent(command.Context,entity));
            return await Commit(_produtoRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeProdutoCommand command,CancellationToken cancellationToken) {
            var filter = ProdutoFilters.GetFilters(command.Query);
			var entities = await _produtoRepository.FindAllAsync(filter);

			_produtoRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_produtoRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateProdutoCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeProdutoCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeProdutoCommand command,CancellationToken cancellationToken) {
            var entities = new List<Produto>();
            foreach (var item in command.Query)
            {
                var filter = ProdutoFilters.GetFilters(item.Key);
                var entity = await _produtoRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateProdutoCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(Produto)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<Produto>();
                _produtoRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new ProdutoUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_produtoRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveProdutoCommand command,CancellationToken cancellationToken) {
            var produto = await _produtoRepository.FindAsync(filter: ProdutoFilters.GetFilters(command.Query));
            //produto.Disable();

            PublishLog(command);
            
            return await Commit(_produtoRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveProdutoCommand command,CancellationToken cancellationToken) {
            var produto = await _produtoRepository.FindAsync(filter: ProdutoFilters.GetFilters(command.Query));
            //produto.Enable();

            PublishLog(command);
            
            return await Commit(_produtoRepository.UnitOfWork);
        }
    }
    public partial class MarketPlaceAggSettingsCommandHandler : BaseMarketPlaceAggCommandHandler<MarketPlaceAggSettings>,
        IRequestHandler<CreateMarketPlaceAggSettingsCommand,DomainResponse>,
        IRequestHandler<DeleteRangeMarketPlaceAggSettingsCommand,DomainResponse>,
        IRequestHandler<DeleteMarketPlaceAggSettingsCommand,DomainResponse>,
        IRequestHandler<UpdateRangeMarketPlaceAggSettingsCommand,DomainResponse>,
        IRequestHandler<UpdateMarketPlaceAggSettingsCommand,DomainResponse>,
        IRequestHandler<ActiveMarketPlaceAggSettingsCommand,DomainResponse>,
        IRequestHandler<DeactiveMarketPlaceAggSettingsCommand,DomainResponse>
    {
        IMarketPlaceAggSettingsRepository _marketPlaceAggSettingsRepository;

        public MarketPlaceAggSettingsCommandHandler(IServiceProvider provider,IMediator mediator,IMarketPlaceAggSettingsRepository marketPlaceAggSettingsRepository ) : base(provider,mediator) { _marketPlaceAggSettingsRepository = marketPlaceAggSettingsRepository; }

        partial void OnCreate(MarketPlaceAggSettings entity);
        partial void OnUpdate(MarketPlaceAggSettings entity);

        public async Task<DomainResponse> Handle(CreateMarketPlaceAggSettingsCommand command,CancellationToken cancellationToken) {

            MarketPlaceAggSettings entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = MarketPlaceAggSettingsFilters.GetFilters(command.Query ?? new MarketPlaceAggSettingsQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _marketPlaceAggSettingsRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateMarketPlaceAggSettingsCommand(
                            command.Context,
                            new Queries.Models.MarketPlaceAggSettingsQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.MarketPlaceAggSettings>();
            entity.AddDomainEvent(new MarketPlaceAggSettingsCreatedEvent(command.Context,entity));

            _marketPlaceAggSettingsRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_marketPlaceAggSettingsRepository.Add(entity);

            return await Commit(_marketPlaceAggSettingsRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteMarketPlaceAggSettingsCommand command,CancellationToken cancellationToken) {
            var filter = MarketPlaceAggSettingsFilters.GetFilters(command.Query);
			var entity = await _marketPlaceAggSettingsRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(MarketPlaceAggSettings)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _marketPlaceAggSettingsRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new MarketPlaceAggSettingsDeletedEvent(command.Context,entity));
            return await Commit(_marketPlaceAggSettingsRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeMarketPlaceAggSettingsCommand command,CancellationToken cancellationToken) {
            var filter = MarketPlaceAggSettingsFilters.GetFilters(command.Query);
			var entities = await _marketPlaceAggSettingsRepository.FindAllAsync(filter);

			_marketPlaceAggSettingsRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_marketPlaceAggSettingsRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateMarketPlaceAggSettingsCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeMarketPlaceAggSettingsCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeMarketPlaceAggSettingsCommand command,CancellationToken cancellationToken) {
            var entities = new List<MarketPlaceAggSettings>();
            foreach (var item in command.Query)
            {
                var filter = MarketPlaceAggSettingsFilters.GetFilters(item.Key);
                var entity = await _marketPlaceAggSettingsRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateMarketPlaceAggSettingsCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(MarketPlaceAggSettings)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<MarketPlaceAggSettings>();
                _marketPlaceAggSettingsRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new MarketPlaceAggSettingsUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_marketPlaceAggSettingsRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveMarketPlaceAggSettingsCommand command,CancellationToken cancellationToken) {
            var marketplaceaggsettings = await _marketPlaceAggSettingsRepository.FindAsync(filter: MarketPlaceAggSettingsFilters.GetFilters(command.Query));
            //marketplaceaggsettings.Disable();

            PublishLog(command);
            
            return await Commit(_marketPlaceAggSettingsRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveMarketPlaceAggSettingsCommand command,CancellationToken cancellationToken) {
            var marketplaceaggsettings = await _marketPlaceAggSettingsRepository.FindAsync(filter: MarketPlaceAggSettingsFilters.GetFilters(command.Query));
            //marketplaceaggsettings.Enable();

            PublishLog(command);
            
            return await Commit(_marketPlaceAggSettingsRepository.UnitOfWork);
        }
    }
    public partial class CarrinhoCommandHandler : BaseMarketPlaceAggCommandHandler<Carrinho>,
        IRequestHandler<CreateCarrinhoCommand,DomainResponse>,
        IRequestHandler<DeleteRangeCarrinhoCommand,DomainResponse>,
        IRequestHandler<DeleteCarrinhoCommand,DomainResponse>,
        IRequestHandler<UpdateRangeCarrinhoCommand,DomainResponse>,
        IRequestHandler<UpdateCarrinhoCommand,DomainResponse>,
        IRequestHandler<ActiveCarrinhoCommand,DomainResponse>,
        IRequestHandler<DeactiveCarrinhoCommand,DomainResponse>
    {
        ICarrinhoRepository _carrinhoRepository;

        public CarrinhoCommandHandler(IServiceProvider provider,IMediator mediator,ICarrinhoRepository carrinhoRepository ) : base(provider,mediator) { _carrinhoRepository = carrinhoRepository; }

        partial void OnCreate(Carrinho entity);
        partial void OnUpdate(Carrinho entity);

        public async Task<DomainResponse> Handle(CreateCarrinhoCommand command,CancellationToken cancellationToken) {

            Carrinho entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = CarrinhoFilters.GetFilters(command.Query ?? new CarrinhoQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _carrinhoRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateCarrinhoCommand(
                            command.Context,
                            new Queries.Models.CarrinhoQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.Carrinho>();
            entity.AddDomainEvent(new CarrinhoCreatedEvent(command.Context,entity));

            _carrinhoRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_carrinhoRepository.Add(entity);

            return await Commit(_carrinhoRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteCarrinhoCommand command,CancellationToken cancellationToken) {
            var filter = CarrinhoFilters.GetFilters(command.Query);
			var entity = await _carrinhoRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(Carrinho)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _carrinhoRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new CarrinhoDeletedEvent(command.Context,entity));
            return await Commit(_carrinhoRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeCarrinhoCommand command,CancellationToken cancellationToken) {
            var filter = CarrinhoFilters.GetFilters(command.Query);
			var entities = await _carrinhoRepository.FindAllAsync(filter);

			_carrinhoRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_carrinhoRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateCarrinhoCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeCarrinhoCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeCarrinhoCommand command,CancellationToken cancellationToken) {
            var entities = new List<Carrinho>();
            foreach (var item in command.Query)
            {
                var filter = CarrinhoFilters.GetFilters(item.Key);
                var entity = await _carrinhoRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateCarrinhoCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(Carrinho)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<Carrinho>();
                _carrinhoRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new CarrinhoUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_carrinhoRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveCarrinhoCommand command,CancellationToken cancellationToken) {
            var carrinho = await _carrinhoRepository.FindAsync(filter: CarrinhoFilters.GetFilters(command.Query));
            //carrinho.Disable();

            PublishLog(command);
            
            return await Commit(_carrinhoRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveCarrinhoCommand command,CancellationToken cancellationToken) {
            var carrinho = await _carrinhoRepository.FindAsync(filter: CarrinhoFilters.GetFilters(command.Query));
            //carrinho.Enable();

            PublishLog(command);
            
            return await Commit(_carrinhoRepository.UnitOfWork);
        }
    }
    public partial class CategoriaprodutoCommandHandler : BaseMarketPlaceAggCommandHandler<Categoriaproduto>,
        IRequestHandler<CreateCategoriaprodutoCommand,DomainResponse>,
        IRequestHandler<DeleteRangeCategoriaprodutoCommand,DomainResponse>,
        IRequestHandler<DeleteCategoriaprodutoCommand,DomainResponse>,
        IRequestHandler<UpdateRangeCategoriaprodutoCommand,DomainResponse>,
        IRequestHandler<UpdateCategoriaprodutoCommand,DomainResponse>,
        IRequestHandler<ActiveCategoriaprodutoCommand,DomainResponse>,
        IRequestHandler<DeactiveCategoriaprodutoCommand,DomainResponse>
    {
        ICategoriaprodutoRepository _categoriaprodutoRepository;

        public CategoriaprodutoCommandHandler(IServiceProvider provider,IMediator mediator,ICategoriaprodutoRepository categoriaprodutoRepository ) : base(provider,mediator) { _categoriaprodutoRepository = categoriaprodutoRepository; }

        partial void OnCreate(Categoriaproduto entity);
        partial void OnUpdate(Categoriaproduto entity);

        public async Task<DomainResponse> Handle(CreateCategoriaprodutoCommand command,CancellationToken cancellationToken) {

            Categoriaproduto entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = CategoriaprodutoFilters.GetFilters(command.Query ?? new CategoriaprodutoQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _categoriaprodutoRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateCategoriaprodutoCommand(
                            command.Context,
                            new Queries.Models.CategoriaprodutoQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.Categoriaproduto>();
            entity.AddDomainEvent(new CategoriaprodutoCreatedEvent(command.Context,entity));

            _categoriaprodutoRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_categoriaprodutoRepository.Add(entity);

            return await Commit(_categoriaprodutoRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteCategoriaprodutoCommand command,CancellationToken cancellationToken) {
            var filter = CategoriaprodutoFilters.GetFilters(command.Query);
			var entity = await _categoriaprodutoRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(Categoriaproduto)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _categoriaprodutoRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new CategoriaprodutoDeletedEvent(command.Context,entity));
            return await Commit(_categoriaprodutoRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeCategoriaprodutoCommand command,CancellationToken cancellationToken) {
            var filter = CategoriaprodutoFilters.GetFilters(command.Query);
			var entities = await _categoriaprodutoRepository.FindAllAsync(filter);

			_categoriaprodutoRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_categoriaprodutoRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateCategoriaprodutoCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeCategoriaprodutoCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeCategoriaprodutoCommand command,CancellationToken cancellationToken) {
            var entities = new List<Categoriaproduto>();
            foreach (var item in command.Query)
            {
                var filter = CategoriaprodutoFilters.GetFilters(item.Key);
                var entity = await _categoriaprodutoRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateCategoriaprodutoCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(Categoriaproduto)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<Categoriaproduto>();
                _categoriaprodutoRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new CategoriaprodutoUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_categoriaprodutoRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveCategoriaprodutoCommand command,CancellationToken cancellationToken) {
            var categoriaproduto = await _categoriaprodutoRepository.FindAsync(filter: CategoriaprodutoFilters.GetFilters(command.Query));
            //categoriaproduto.Disable();

            PublishLog(command);
            
            return await Commit(_categoriaprodutoRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveCategoriaprodutoCommand command,CancellationToken cancellationToken) {
            var categoriaproduto = await _categoriaprodutoRepository.FindAsync(filter: CategoriaprodutoFilters.GetFilters(command.Query));
            //categoriaproduto.Enable();

            PublishLog(command);
            
            return await Commit(_categoriaprodutoRepository.UnitOfWork);
        }
    }
}
