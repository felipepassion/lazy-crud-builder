using MediatR;
using Niu.Nutri.Core.Domain.CrossCutting;
using Niu.Nutri.Core.Application.DTO.Extensions;
using Niu.Nutri.CrossCuting.Infra.Utils.Extensions;
using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Entities;
using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Commands.Handles;

namespace Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.CommandHandlers
{
    using Filters;
    using ModelEvents;
    using Repositories;
    using CommandModels;
    using Entities;
    using Queries.Models;
    using Application.DTO.Aggregates.DefaultTemplateAgg.Requests;
    
    public class BaseDefaultTemplateAggCommandHandler<T> : BaseCommandHandler<T> where T : IEntity {public BaseDefaultTemplateAggCommandHandler(IServiceProvider provider,IMediator mediator):base(mediator, provider){}}
    public partial class DefaultEntityCommandHandler : BaseDefaultTemplateAggCommandHandler<DefaultEntity>,
        IRequestHandler<CreateDefaultEntityCommand,DomainResponse>,
        IRequestHandler<DeleteRangeDefaultEntityCommand,DomainResponse>,
        IRequestHandler<DeleteDefaultEntityCommand,DomainResponse>,
        IRequestHandler<UpdateRangeDefaultEntityCommand,DomainResponse>,
        IRequestHandler<UpdateDefaultEntityCommand,DomainResponse>,
        IRequestHandler<ActiveDefaultEntityCommand,DomainResponse>,
        IRequestHandler<DeactiveDefaultEntityCommand,DomainResponse>
    {
        IDefaultEntityRepository _defaultEntityRepository;

        public DefaultEntityCommandHandler(IServiceProvider provider,IMediator mediator,IDefaultEntityRepository defaultEntityRepository ) : base(provider,mediator) { _defaultEntityRepository = defaultEntityRepository; }

        partial void OnCreate(DefaultEntity entity);
        partial void OnUpdate(DefaultEntity entity);

        public async Task<DomainResponse> Handle(CreateDefaultEntityCommand command,CancellationToken cancellationToken) {

            DefaultEntity entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = DefaultEntityFilters.GetFilters(command.Query ?? new DefaultEntityQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _defaultEntityRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateDefaultEntityCommand(
                            command.Context,
                            new Queries.Models.DefaultEntityQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.DefaultEntity>();
            entity.AddDomainEvent(new DefaultEntityCreatedEvent(command.Context,entity));

            _defaultEntityRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_defaultEntityRepository.Add(entity);

            var result = await Commit(_defaultEntityRepository.UnitOfWork, entity.ProjectedAs<DefaultEntityDTO>());
            result.Data = entity.ProjectedAs<DefaultEntityDTO>();
            return result;
        }

        public async Task<DomainResponse> Handle(DeleteDefaultEntityCommand command,CancellationToken cancellationToken) {
            var filter = DefaultEntityFilters.GetFilters(command.Query);
			var entity = await _defaultEntityRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(DefaultEntity)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _defaultEntityRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new DefaultEntityDeletedEvent(command.Context,entity));
            return await Commit(_defaultEntityRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeDefaultEntityCommand command,CancellationToken cancellationToken) {
            var filter = DefaultEntityFilters.GetFilters(command.Query);
			var entities = await _defaultEntityRepository.FindAllAsync(filter);

			_defaultEntityRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_defaultEntityRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateDefaultEntityCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeDefaultEntityCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeDefaultEntityCommand command,CancellationToken cancellationToken) {
            var entities = new List<DefaultEntity>();
            foreach (var item in command.Query)
            {
                var entity = command.Entity as DefaultEntity ?? await _defaultEntityRepository.FindAsync(DefaultEntityFilters.GetFilters(item.Key));
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateDefaultEntityCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(DefaultEntity)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<DefaultEntity>();
                _defaultEntityRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new DefaultEntityUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_defaultEntityRepository.UnitOfWork, command.Entity.ProjectedAs<DefaultEntityDTO>());
        }
         
        public async Task<DomainResponse> Handle(ActiveDefaultEntityCommand command,CancellationToken cancellationToken) {
            var defaultentity = await _defaultEntityRepository.FindAsync(filter: DefaultEntityFilters.GetFilters(command.Query));
            //defaultentity.Disable();

            PublishLog(command);
            
            return await Commit(_defaultEntityRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveDefaultEntityCommand command,CancellationToken cancellationToken) {
            var defaultentity = await _defaultEntityRepository.FindAsync(filter: DefaultEntityFilters.GetFilters(command.Query));
            //defaultentity.Enable();

            PublishLog(command);
            
            return await Commit(_defaultEntityRepository.UnitOfWork);
        }
    }
    public partial class DefaultTemplateAggSettingsCommandHandler : BaseDefaultTemplateAggCommandHandler<DefaultTemplateAggSettings>,
        IRequestHandler<CreateDefaultTemplateAggSettingsCommand,DomainResponse>,
        IRequestHandler<DeleteRangeDefaultTemplateAggSettingsCommand,DomainResponse>,
        IRequestHandler<DeleteDefaultTemplateAggSettingsCommand,DomainResponse>,
        IRequestHandler<UpdateRangeDefaultTemplateAggSettingsCommand,DomainResponse>,
        IRequestHandler<UpdateDefaultTemplateAggSettingsCommand,DomainResponse>,
        IRequestHandler<ActiveDefaultTemplateAggSettingsCommand,DomainResponse>,
        IRequestHandler<DeactiveDefaultTemplateAggSettingsCommand,DomainResponse>
    {
        IDefaultTemplateAggSettingsRepository _defaultTemplateAggSettingsRepository;

        public DefaultTemplateAggSettingsCommandHandler(IServiceProvider provider,IMediator mediator,IDefaultTemplateAggSettingsRepository defaultTemplateAggSettingsRepository ) : base(provider,mediator) { _defaultTemplateAggSettingsRepository = defaultTemplateAggSettingsRepository; }

        partial void OnCreate(DefaultTemplateAggSettings entity);
        partial void OnUpdate(DefaultTemplateAggSettings entity);

        public async Task<DomainResponse> Handle(CreateDefaultTemplateAggSettingsCommand command,CancellationToken cancellationToken) {

            DefaultTemplateAggSettings entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = DefaultTemplateAggSettingsFilters.GetFilters(command.Query ?? new DefaultTemplateAggSettingsQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _defaultTemplateAggSettingsRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateDefaultTemplateAggSettingsCommand(
                            command.Context,
                            new Queries.Models.DefaultTemplateAggSettingsQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.DefaultTemplateAggSettings>();
            entity.AddDomainEvent(new DefaultTemplateAggSettingsCreatedEvent(command.Context,entity));

            _defaultTemplateAggSettingsRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_defaultTemplateAggSettingsRepository.Add(entity);

            var result = await Commit(_defaultTemplateAggSettingsRepository.UnitOfWork, entity.ProjectedAs<DefaultTemplateAggSettingsDTO>());
            result.Data = entity.ProjectedAs<DefaultTemplateAggSettingsDTO>();
            return result;
        }

        public async Task<DomainResponse> Handle(DeleteDefaultTemplateAggSettingsCommand command,CancellationToken cancellationToken) {
            var filter = DefaultTemplateAggSettingsFilters.GetFilters(command.Query);
			var entity = await _defaultTemplateAggSettingsRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(DefaultTemplateAggSettings)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _defaultTemplateAggSettingsRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new DefaultTemplateAggSettingsDeletedEvent(command.Context,entity));
            return await Commit(_defaultTemplateAggSettingsRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeDefaultTemplateAggSettingsCommand command,CancellationToken cancellationToken) {
            var filter = DefaultTemplateAggSettingsFilters.GetFilters(command.Query);
			var entities = await _defaultTemplateAggSettingsRepository.FindAllAsync(filter);

			_defaultTemplateAggSettingsRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_defaultTemplateAggSettingsRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateDefaultTemplateAggSettingsCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeDefaultTemplateAggSettingsCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeDefaultTemplateAggSettingsCommand command,CancellationToken cancellationToken) {
            var entities = new List<DefaultTemplateAggSettings>();
            foreach (var item in command.Query)
            {
                var entity = command.Entity as DefaultTemplateAggSettings ?? await _defaultTemplateAggSettingsRepository.FindAsync(DefaultTemplateAggSettingsFilters.GetFilters(item.Key));
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateDefaultTemplateAggSettingsCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(DefaultTemplateAggSettings)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<DefaultTemplateAggSettings>();
                _defaultTemplateAggSettingsRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new DefaultTemplateAggSettingsUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_defaultTemplateAggSettingsRepository.UnitOfWork, command.Entity.ProjectedAs<DefaultTemplateAggSettingsDTO>());
        }
         
        public async Task<DomainResponse> Handle(ActiveDefaultTemplateAggSettingsCommand command,CancellationToken cancellationToken) {
            var defaulttemplateaggsettings = await _defaultTemplateAggSettingsRepository.FindAsync(filter: DefaultTemplateAggSettingsFilters.GetFilters(command.Query));
            //defaulttemplateaggsettings.Disable();

            PublishLog(command);
            
            return await Commit(_defaultTemplateAggSettingsRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveDefaultTemplateAggSettingsCommand command,CancellationToken cancellationToken) {
            var defaulttemplateaggsettings = await _defaultTemplateAggSettingsRepository.FindAsync(filter: DefaultTemplateAggSettingsFilters.GetFilters(command.Query));
            //defaulttemplateaggsettings.Enable();

            PublishLog(command);
            
            return await Commit(_defaultTemplateAggSettingsRepository.UnitOfWork);
        }
    }
}
