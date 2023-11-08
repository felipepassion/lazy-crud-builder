
using MediatR;
using FluentValidation.Results;
using LazyCrud.Core.Domain.CrossCutting;
using LazyCrud.Core.Application.DTO.Extensions;
using LazyCrud.CrossCuting.Infra.Utils.Extensions;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.ValueObjects;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Commands.Handles;

namespace LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.CommandHandlers
{
    using Filters;
    using ModelEvents;
    using Repositories;
    using CommandModels;
    using Entities;
    using Specifications;
    using Queries.Models;
    
    public class BaseSystemSettingsAggCommandHandler<T> : BaseCommandHandler<T> where T : IEntity {public BaseSystemSettingsAggCommandHandler(IServiceProvider provider,IMediator mediator):base(mediator, provider){}}
    public partial class SystemPanelSubItemCommandHandler : BaseSystemSettingsAggCommandHandler<SystemPanelSubItem>,
        IRequestHandler<CreateSystemPanelSubItemCommand,DomainResponse>,
        IRequestHandler<DeleteRangeSystemPanelSubItemCommand,DomainResponse>,
        IRequestHandler<DeleteSystemPanelSubItemCommand,DomainResponse>,
        IRequestHandler<UpdateRangeSystemPanelSubItemCommand,DomainResponse>,
        IRequestHandler<UpdateSystemPanelSubItemCommand,DomainResponse>,
        IRequestHandler<ActiveSystemPanelSubItemCommand,DomainResponse>,
        IRequestHandler<DeactiveSystemPanelSubItemCommand,DomainResponse>
    {
        ISystemPanelSubItemRepository _systemPanelSubItemRepository;

        public SystemPanelSubItemCommandHandler(IServiceProvider provider,IMediator mediator,ISystemPanelSubItemRepository systemPanelSubItemRepository ) : base(provider,mediator) { _systemPanelSubItemRepository = systemPanelSubItemRepository; }

        partial void OnCreate(SystemPanelSubItem entity);
        partial void OnUpdate(SystemPanelSubItem entity);

        public async Task<DomainResponse> Handle(CreateSystemPanelSubItemCommand command,CancellationToken cancellationToken) {

            SystemPanelSubItem entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = SystemPanelSubItemFilters.GetFilters(command.Query ?? new SystemPanelSubItemQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _systemPanelSubItemRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateSystemPanelSubItemCommand(
                            command.Context,
                            new Queries.Models.SystemPanelSubItemQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.SystemPanelSubItem>();
            entity.AddDomainEvent(new SystemPanelSubItemCreatedEvent(command.Context,entity));

            _systemPanelSubItemRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_systemPanelSubItemRepository.Add(entity);

            return await Commit(_systemPanelSubItemRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteSystemPanelSubItemCommand command,CancellationToken cancellationToken) {
            var filter = SystemPanelSubItemFilters.GetFilters(command.Query);
			var entity = await _systemPanelSubItemRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(SystemPanelSubItem)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _systemPanelSubItemRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new SystemPanelSubItemDeletedEvent(command.Context,entity));
            return await Commit(_systemPanelSubItemRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeSystemPanelSubItemCommand command,CancellationToken cancellationToken) {
            var filter = SystemPanelSubItemFilters.GetFilters(command.Query);
			var entities = await _systemPanelSubItemRepository.FindAllAsync(filter);

			_systemPanelSubItemRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_systemPanelSubItemRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateSystemPanelSubItemCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeSystemPanelSubItemCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeSystemPanelSubItemCommand command,CancellationToken cancellationToken) {
            var entities = new List<SystemPanelSubItem>();
            foreach (var item in command.Query)
            {
                var filter = SystemPanelSubItemFilters.GetFilters(item.Key);
                var entity = await _systemPanelSubItemRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateSystemPanelSubItemCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(SystemPanelSubItem)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<SystemPanelSubItem>();
                _systemPanelSubItemRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new SystemPanelSubItemUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_systemPanelSubItemRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveSystemPanelSubItemCommand command,CancellationToken cancellationToken) {
            var systempanelsubitem = await _systemPanelSubItemRepository.FindAsync(filter: SystemPanelSubItemFilters.GetFilters(command.Query));
            //systempanelsubitem.Disable();

            PublishLog(command);
            
            return await Commit(_systemPanelSubItemRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveSystemPanelSubItemCommand command,CancellationToken cancellationToken) {
            var systempanelsubitem = await _systemPanelSubItemRepository.FindAsync(filter: SystemPanelSubItemFilters.GetFilters(command.Query));
            //systempanelsubitem.Enable();

            PublishLog(command);
            
            return await Commit(_systemPanelSubItemRepository.UnitOfWork);
        }
    }
    public partial class SystemPanelCommandHandler : BaseSystemSettingsAggCommandHandler<SystemPanel>,
        IRequestHandler<CreateSystemPanelCommand,DomainResponse>,
        IRequestHandler<DeleteRangeSystemPanelCommand,DomainResponse>,
        IRequestHandler<DeleteSystemPanelCommand,DomainResponse>,
        IRequestHandler<UpdateRangeSystemPanelCommand,DomainResponse>,
        IRequestHandler<UpdateSystemPanelCommand,DomainResponse>,
        IRequestHandler<ActiveSystemPanelCommand,DomainResponse>,
        IRequestHandler<DeactiveSystemPanelCommand,DomainResponse>
    {
        ISystemPanelRepository _systemPanelRepository;

        public SystemPanelCommandHandler(IServiceProvider provider,IMediator mediator,ISystemPanelRepository systemPanelRepository ) : base(provider,mediator) { _systemPanelRepository = systemPanelRepository; }

        partial void OnCreate(SystemPanel entity);
        partial void OnUpdate(SystemPanel entity);

        public async Task<DomainResponse> Handle(CreateSystemPanelCommand command,CancellationToken cancellationToken) {

            SystemPanel entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = SystemPanelFilters.GetFilters(command.Query ?? new SystemPanelQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _systemPanelRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateSystemPanelCommand(
                            command.Context,
                            new Queries.Models.SystemPanelQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.SystemPanel>();
            entity.AddDomainEvent(new SystemPanelCreatedEvent(command.Context,entity));

            _systemPanelRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_systemPanelRepository.Add(entity);

            return await Commit(_systemPanelRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteSystemPanelCommand command,CancellationToken cancellationToken) {
            var filter = SystemPanelFilters.GetFilters(command.Query);
			var entity = await _systemPanelRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(SystemPanel)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _systemPanelRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new SystemPanelDeletedEvent(command.Context,entity));
            return await Commit(_systemPanelRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeSystemPanelCommand command,CancellationToken cancellationToken) {
            var filter = SystemPanelFilters.GetFilters(command.Query);
			var entities = await _systemPanelRepository.FindAllAsync(filter);

			_systemPanelRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_systemPanelRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateSystemPanelCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeSystemPanelCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeSystemPanelCommand command,CancellationToken cancellationToken) {
            var entities = new List<SystemPanel>();
            foreach (var item in command.Query)
            {
                var filter = SystemPanelFilters.GetFilters(item.Key);
                var entity = await _systemPanelRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateSystemPanelCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(SystemPanel)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<SystemPanel>();
                _systemPanelRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new SystemPanelUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_systemPanelRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveSystemPanelCommand command,CancellationToken cancellationToken) {
            var systempanel = await _systemPanelRepository.FindAsync(filter: SystemPanelFilters.GetFilters(command.Query));
            //systempanel.Disable();

            PublishLog(command);
            
            return await Commit(_systemPanelRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveSystemPanelCommand command,CancellationToken cancellationToken) {
            var systempanel = await _systemPanelRepository.FindAsync(filter: SystemPanelFilters.GetFilters(command.Query));
            //systempanel.Enable();

            PublishLog(command);
            
            return await Commit(_systemPanelRepository.UnitOfWork);
        }
    }
    public partial class SystemPanelGroupCommandHandler : BaseSystemSettingsAggCommandHandler<SystemPanelGroup>,
        IRequestHandler<CreateSystemPanelGroupCommand,DomainResponse>,
        IRequestHandler<DeleteRangeSystemPanelGroupCommand,DomainResponse>,
        IRequestHandler<DeleteSystemPanelGroupCommand,DomainResponse>,
        IRequestHandler<UpdateRangeSystemPanelGroupCommand,DomainResponse>,
        IRequestHandler<UpdateSystemPanelGroupCommand,DomainResponse>,
        IRequestHandler<ActiveSystemPanelGroupCommand,DomainResponse>,
        IRequestHandler<DeactiveSystemPanelGroupCommand,DomainResponse>
    {
        ISystemPanelGroupRepository _systemPanelGroupRepository;

        public SystemPanelGroupCommandHandler(IServiceProvider provider,IMediator mediator,ISystemPanelGroupRepository systemPanelGroupRepository ) : base(provider,mediator) { _systemPanelGroupRepository = systemPanelGroupRepository; }

        partial void OnCreate(SystemPanelGroup entity);
        partial void OnUpdate(SystemPanelGroup entity);

        public async Task<DomainResponse> Handle(CreateSystemPanelGroupCommand command,CancellationToken cancellationToken) {

            SystemPanelGroup entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = SystemPanelGroupFilters.GetFilters(command.Query ?? new SystemPanelGroupQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _systemPanelGroupRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateSystemPanelGroupCommand(
                            command.Context,
                            new Queries.Models.SystemPanelGroupQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.SystemPanelGroup>();
            entity.AddDomainEvent(new SystemPanelGroupCreatedEvent(command.Context,entity));

            _systemPanelGroupRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_systemPanelGroupRepository.Add(entity);

            return await Commit(_systemPanelGroupRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteSystemPanelGroupCommand command,CancellationToken cancellationToken) {
            var filter = SystemPanelGroupFilters.GetFilters(command.Query);
			var entity = await _systemPanelGroupRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(SystemPanelGroup)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _systemPanelGroupRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new SystemPanelGroupDeletedEvent(command.Context,entity));
            return await Commit(_systemPanelGroupRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeSystemPanelGroupCommand command,CancellationToken cancellationToken) {
            var filter = SystemPanelGroupFilters.GetFilters(command.Query);
			var entities = await _systemPanelGroupRepository.FindAllAsync(filter);

			_systemPanelGroupRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_systemPanelGroupRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateSystemPanelGroupCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeSystemPanelGroupCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeSystemPanelGroupCommand command,CancellationToken cancellationToken) {
            var entities = new List<SystemPanelGroup>();
            foreach (var item in command.Query)
            {
                var filter = SystemPanelGroupFilters.GetFilters(item.Key);
                var entity = await _systemPanelGroupRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateSystemPanelGroupCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(SystemPanelGroup)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<SystemPanelGroup>();
                _systemPanelGroupRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new SystemPanelGroupUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_systemPanelGroupRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveSystemPanelGroupCommand command,CancellationToken cancellationToken) {
            var systempanelgroup = await _systemPanelGroupRepository.FindAsync(filter: SystemPanelGroupFilters.GetFilters(command.Query));
            //systempanelgroup.Disable();

            PublishLog(command);
            
            return await Commit(_systemPanelGroupRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveSystemPanelGroupCommand command,CancellationToken cancellationToken) {
            var systempanelgroup = await _systemPanelGroupRepository.FindAsync(filter: SystemPanelGroupFilters.GetFilters(command.Query));
            //systempanelgroup.Enable();

            PublishLog(command);
            
            return await Commit(_systemPanelGroupRepository.UnitOfWork);
        }
    }
    public partial class CargaTabelaCommandHandler : BaseSystemSettingsAggCommandHandler<CargaTabela>,
        IRequestHandler<CreateCargaTabelaCommand,DomainResponse>,
        IRequestHandler<DeleteRangeCargaTabelaCommand,DomainResponse>,
        IRequestHandler<DeleteCargaTabelaCommand,DomainResponse>,
        IRequestHandler<UpdateRangeCargaTabelaCommand,DomainResponse>,
        IRequestHandler<UpdateCargaTabelaCommand,DomainResponse>,
        IRequestHandler<ActiveCargaTabelaCommand,DomainResponse>,
        IRequestHandler<DeactiveCargaTabelaCommand,DomainResponse>
    {
        ICargaTabelaRepository _cargaTabelaRepository;

        public CargaTabelaCommandHandler(IServiceProvider provider,IMediator mediator,ICargaTabelaRepository cargaTabelaRepository ) : base(provider,mediator) { _cargaTabelaRepository = cargaTabelaRepository; }

        partial void OnCreate(CargaTabela entity);
        partial void OnUpdate(CargaTabela entity);

        public async Task<DomainResponse> Handle(CreateCargaTabelaCommand command,CancellationToken cancellationToken) {

            CargaTabela entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = CargaTabelaFilters.GetFilters(command.Query ?? new CargaTabelaQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _cargaTabelaRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateCargaTabelaCommand(
                            command.Context,
                            new Queries.Models.CargaTabelaQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.CargaTabela>();
            entity.AddDomainEvent(new CargaTabelaCreatedEvent(command.Context,entity));

            _cargaTabelaRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_cargaTabelaRepository.Add(entity);

            return await Commit(_cargaTabelaRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteCargaTabelaCommand command,CancellationToken cancellationToken) {
            var filter = CargaTabelaFilters.GetFilters(command.Query);
			var entity = await _cargaTabelaRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(CargaTabela)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _cargaTabelaRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new CargaTabelaDeletedEvent(command.Context,entity));
            return await Commit(_cargaTabelaRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeCargaTabelaCommand command,CancellationToken cancellationToken) {
            var filter = CargaTabelaFilters.GetFilters(command.Query);
			var entities = await _cargaTabelaRepository.FindAllAsync(filter);

			_cargaTabelaRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_cargaTabelaRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateCargaTabelaCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeCargaTabelaCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeCargaTabelaCommand command,CancellationToken cancellationToken) {
            var entities = new List<CargaTabela>();
            foreach (var item in command.Query)
            {
                var filter = CargaTabelaFilters.GetFilters(item.Key);
                var entity = await _cargaTabelaRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateCargaTabelaCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(CargaTabela)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<CargaTabela>();
                _cargaTabelaRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new CargaTabelaUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_cargaTabelaRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveCargaTabelaCommand command,CancellationToken cancellationToken) {
            var cargatabela = await _cargaTabelaRepository.FindAsync(filter: CargaTabelaFilters.GetFilters(command.Query));
            //cargatabela.Disable();

            PublishLog(command);
            
            return await Commit(_cargaTabelaRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveCargaTabelaCommand command,CancellationToken cancellationToken) {
            var cargatabela = await _cargaTabelaRepository.FindAsync(filter: CargaTabelaFilters.GetFilters(command.Query));
            //cargatabela.Enable();

            PublishLog(command);
            
            return await Commit(_cargaTabelaRepository.UnitOfWork);
        }
    }
    public partial class SystemSettingsAggSettingsCommandHandler : BaseSystemSettingsAggCommandHandler<SystemSettingsAggSettings>,
        IRequestHandler<CreateSystemSettingsAggSettingsCommand,DomainResponse>,
        IRequestHandler<DeleteRangeSystemSettingsAggSettingsCommand,DomainResponse>,
        IRequestHandler<DeleteSystemSettingsAggSettingsCommand,DomainResponse>,
        IRequestHandler<UpdateRangeSystemSettingsAggSettingsCommand,DomainResponse>,
        IRequestHandler<UpdateSystemSettingsAggSettingsCommand,DomainResponse>,
        IRequestHandler<ActiveSystemSettingsAggSettingsCommand,DomainResponse>,
        IRequestHandler<DeactiveSystemSettingsAggSettingsCommand,DomainResponse>
    {
        ISystemSettingsAggSettingsRepository _systemSettingsAggSettingsRepository;

        public SystemSettingsAggSettingsCommandHandler(IServiceProvider provider,IMediator mediator,ISystemSettingsAggSettingsRepository systemSettingsAggSettingsRepository ) : base(provider,mediator) { _systemSettingsAggSettingsRepository = systemSettingsAggSettingsRepository; }

        partial void OnCreate(SystemSettingsAggSettings entity);
        partial void OnUpdate(SystemSettingsAggSettings entity);

        public async Task<DomainResponse> Handle(CreateSystemSettingsAggSettingsCommand command,CancellationToken cancellationToken) {

            SystemSettingsAggSettings entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = SystemSettingsAggSettingsFilters.GetFilters(command.Query ?? new SystemSettingsAggSettingsQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _systemSettingsAggSettingsRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateSystemSettingsAggSettingsCommand(
                            command.Context,
                            new Queries.Models.SystemSettingsAggSettingsQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.SystemSettingsAggSettings>();
            entity.AddDomainEvent(new SystemSettingsAggSettingsCreatedEvent(command.Context,entity));

            _systemSettingsAggSettingsRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_systemSettingsAggSettingsRepository.Add(entity);

            return await Commit(_systemSettingsAggSettingsRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteSystemSettingsAggSettingsCommand command,CancellationToken cancellationToken) {
            var filter = SystemSettingsAggSettingsFilters.GetFilters(command.Query);
			var entity = await _systemSettingsAggSettingsRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(SystemSettingsAggSettings)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _systemSettingsAggSettingsRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new SystemSettingsAggSettingsDeletedEvent(command.Context,entity));
            return await Commit(_systemSettingsAggSettingsRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeSystemSettingsAggSettingsCommand command,CancellationToken cancellationToken) {
            var filter = SystemSettingsAggSettingsFilters.GetFilters(command.Query);
			var entities = await _systemSettingsAggSettingsRepository.FindAllAsync(filter);

			_systemSettingsAggSettingsRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_systemSettingsAggSettingsRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateSystemSettingsAggSettingsCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeSystemSettingsAggSettingsCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeSystemSettingsAggSettingsCommand command,CancellationToken cancellationToken) {
            var entities = new List<SystemSettingsAggSettings>();
            foreach (var item in command.Query)
            {
                var filter = SystemSettingsAggSettingsFilters.GetFilters(item.Key);
                var entity = await _systemSettingsAggSettingsRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateSystemSettingsAggSettingsCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(SystemSettingsAggSettings)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<SystemSettingsAggSettings>();
                _systemSettingsAggSettingsRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new SystemSettingsAggSettingsUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_systemSettingsAggSettingsRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveSystemSettingsAggSettingsCommand command,CancellationToken cancellationToken) {
            var systemsettingsaggsettings = await _systemSettingsAggSettingsRepository.FindAsync(filter: SystemSettingsAggSettingsFilters.GetFilters(command.Query));
            //systemsettingsaggsettings.Disable();

            PublishLog(command);
            
            return await Commit(_systemSettingsAggSettingsRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveSystemSettingsAggSettingsCommand command,CancellationToken cancellationToken) {
            var systemsettingsaggsettings = await _systemSettingsAggSettingsRepository.FindAsync(filter: SystemSettingsAggSettingsFilters.GetFilters(command.Query));
            //systemsettingsaggsettings.Enable();

            PublishLog(command);
            
            return await Commit(_systemSettingsAggSettingsRepository.UnitOfWork);
        }
    }
}
