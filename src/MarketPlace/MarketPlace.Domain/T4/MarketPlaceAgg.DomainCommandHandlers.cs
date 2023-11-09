
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
}
