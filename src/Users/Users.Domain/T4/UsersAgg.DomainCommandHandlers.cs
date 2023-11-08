
using MediatR;
using FluentValidation.Results;
using LazyCrud.Core.Domain.CrossCutting;
using LazyCrud.Core.Application.DTO.Extensions;
using LazyCrud.CrossCuting.Infra.Utils.Extensions;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.ValueObjects;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Commands.Handles;

namespace LazyCrud.Users.Domain.Aggregates.UsersAgg.CommandHandlers
{
    using Filters;
    using ModelEvents;
    using Repositories;
    using CommandModels;
    using Entities;
    using Specifications;
    using Queries.Models;
    
    public class BaseUsersAggCommandHandler<T> : BaseCommandHandler<T> where T : IEntity {public BaseUsersAggCommandHandler(IServiceProvider provider,IMediator mediator):base(mediator, provider){}}
    public partial class UserProfileAccessCommandHandler : BaseUsersAggCommandHandler<UserProfileAccess>,
        IRequestHandler<CreateUserProfileAccessCommand,DomainResponse>,
        IRequestHandler<DeleteRangeUserProfileAccessCommand,DomainResponse>,
        IRequestHandler<DeleteUserProfileAccessCommand,DomainResponse>,
        IRequestHandler<UpdateRangeUserProfileAccessCommand,DomainResponse>,
        IRequestHandler<UpdateUserProfileAccessCommand,DomainResponse>,
        IRequestHandler<ActiveUserProfileAccessCommand,DomainResponse>,
        IRequestHandler<DeactiveUserProfileAccessCommand,DomainResponse>
    {
        IUserProfileAccessRepository _userProfileAccessRepository;

        public UserProfileAccessCommandHandler(IServiceProvider provider,IMediator mediator,IUserProfileAccessRepository userProfileAccessRepository ) : base(provider,mediator) { _userProfileAccessRepository = userProfileAccessRepository; }

        partial void OnCreate(UserProfileAccess entity);
        partial void OnUpdate(UserProfileAccess entity);

        public async Task<DomainResponse> Handle(CreateUserProfileAccessCommand command,CancellationToken cancellationToken) {

            UserProfileAccess entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = UserProfileAccessFilters.GetFilters(command.Query ?? new UserProfileAccessQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _userProfileAccessRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateUserProfileAccessCommand(
                            command.Context,
                            new Queries.Models.UserProfileAccessQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.UserProfileAccess>();
            entity.AddDomainEvent(new UserProfileAccessCreatedEvent(command.Context,entity));

            _userProfileAccessRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_userProfileAccessRepository.Add(entity);

            return await Commit(_userProfileAccessRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteUserProfileAccessCommand command,CancellationToken cancellationToken) {
            var filter = UserProfileAccessFilters.GetFilters(command.Query);
			var entity = await _userProfileAccessRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(UserProfileAccess)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _userProfileAccessRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new UserProfileAccessDeletedEvent(command.Context,entity));
            return await Commit(_userProfileAccessRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeUserProfileAccessCommand command,CancellationToken cancellationToken) {
            var filter = UserProfileAccessFilters.GetFilters(command.Query);
			var entities = await _userProfileAccessRepository.FindAllAsync(filter);

			_userProfileAccessRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_userProfileAccessRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateUserProfileAccessCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeUserProfileAccessCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeUserProfileAccessCommand command,CancellationToken cancellationToken) {
            var entities = new List<UserProfileAccess>();
            foreach (var item in command.Query)
            {
                var filter = UserProfileAccessFilters.GetFilters(item.Key);
                var entity = await _userProfileAccessRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateUserProfileAccessCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(UserProfileAccess)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<UserProfileAccess>();
                _userProfileAccessRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new UserProfileAccessUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_userProfileAccessRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveUserProfileAccessCommand command,CancellationToken cancellationToken) {
            var userprofileaccess = await _userProfileAccessRepository.FindAsync(filter: UserProfileAccessFilters.GetFilters(command.Query));
            //userprofileaccess.Disable();

            PublishLog(command);
            
            return await Commit(_userProfileAccessRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveUserProfileAccessCommand command,CancellationToken cancellationToken) {
            var userprofileaccess = await _userProfileAccessRepository.FindAsync(filter: UserProfileAccessFilters.GetFilters(command.Query));
            //userprofileaccess.Enable();

            PublishLog(command);
            
            return await Commit(_userProfileAccessRepository.UnitOfWork);
        }
    }
    public partial class UserCurrentAccessSelectedCommandHandler : BaseUsersAggCommandHandler<UserCurrentAccessSelected>,
        IRequestHandler<CreateUserCurrentAccessSelectedCommand,DomainResponse>,
        IRequestHandler<DeleteRangeUserCurrentAccessSelectedCommand,DomainResponse>,
        IRequestHandler<DeleteUserCurrentAccessSelectedCommand,DomainResponse>,
        IRequestHandler<UpdateRangeUserCurrentAccessSelectedCommand,DomainResponse>,
        IRequestHandler<UpdateUserCurrentAccessSelectedCommand,DomainResponse>,
        IRequestHandler<ActiveUserCurrentAccessSelectedCommand,DomainResponse>,
        IRequestHandler<DeactiveUserCurrentAccessSelectedCommand,DomainResponse>
    {
        IUserCurrentAccessSelectedRepository _userCurrentAccessSelectedRepository;

        public UserCurrentAccessSelectedCommandHandler(IServiceProvider provider,IMediator mediator,IUserCurrentAccessSelectedRepository userCurrentAccessSelectedRepository ) : base(provider,mediator) { _userCurrentAccessSelectedRepository = userCurrentAccessSelectedRepository; }

        partial void OnCreate(UserCurrentAccessSelected entity);
        partial void OnUpdate(UserCurrentAccessSelected entity);

        public async Task<DomainResponse> Handle(CreateUserCurrentAccessSelectedCommand command,CancellationToken cancellationToken) {

            UserCurrentAccessSelected entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = UserCurrentAccessSelectedFilters.GetFilters(command.Query ?? new UserCurrentAccessSelectedQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _userCurrentAccessSelectedRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateUserCurrentAccessSelectedCommand(
                            command.Context,
                            new Queries.Models.UserCurrentAccessSelectedQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.UserCurrentAccessSelected>();
            entity.AddDomainEvent(new UserCurrentAccessSelectedCreatedEvent(command.Context,entity));

            _userCurrentAccessSelectedRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_userCurrentAccessSelectedRepository.Add(entity);

            return await Commit(_userCurrentAccessSelectedRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteUserCurrentAccessSelectedCommand command,CancellationToken cancellationToken) {
            var filter = UserCurrentAccessSelectedFilters.GetFilters(command.Query);
			var entity = await _userCurrentAccessSelectedRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(UserCurrentAccessSelected)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _userCurrentAccessSelectedRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new UserCurrentAccessSelectedDeletedEvent(command.Context,entity));
            return await Commit(_userCurrentAccessSelectedRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeUserCurrentAccessSelectedCommand command,CancellationToken cancellationToken) {
            var filter = UserCurrentAccessSelectedFilters.GetFilters(command.Query);
			var entities = await _userCurrentAccessSelectedRepository.FindAllAsync(filter);

			_userCurrentAccessSelectedRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_userCurrentAccessSelectedRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateUserCurrentAccessSelectedCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeUserCurrentAccessSelectedCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeUserCurrentAccessSelectedCommand command,CancellationToken cancellationToken) {
            var entities = new List<UserCurrentAccessSelected>();
            foreach (var item in command.Query)
            {
                var filter = UserCurrentAccessSelectedFilters.GetFilters(item.Key);
                var entity = await _userCurrentAccessSelectedRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateUserCurrentAccessSelectedCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(UserCurrentAccessSelected)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<UserCurrentAccessSelected>();
                _userCurrentAccessSelectedRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new UserCurrentAccessSelectedUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_userCurrentAccessSelectedRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveUserCurrentAccessSelectedCommand command,CancellationToken cancellationToken) {
            var usercurrentaccessselected = await _userCurrentAccessSelectedRepository.FindAsync(filter: UserCurrentAccessSelectedFilters.GetFilters(command.Query));
            //usercurrentaccessselected.Disable();

            PublishLog(command);
            
            return await Commit(_userCurrentAccessSelectedRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveUserCurrentAccessSelectedCommand command,CancellationToken cancellationToken) {
            var usercurrentaccessselected = await _userCurrentAccessSelectedRepository.FindAsync(filter: UserCurrentAccessSelectedFilters.GetFilters(command.Query));
            //usercurrentaccessselected.Enable();

            PublishLog(command);
            
            return await Commit(_userCurrentAccessSelectedRepository.UnitOfWork);
        }
    }
    public partial class UserProfileListCommandHandler : BaseUsersAggCommandHandler<UserProfileList>,
        IRequestHandler<CreateUserProfileListCommand,DomainResponse>,
        IRequestHandler<DeleteRangeUserProfileListCommand,DomainResponse>,
        IRequestHandler<DeleteUserProfileListCommand,DomainResponse>,
        IRequestHandler<UpdateRangeUserProfileListCommand,DomainResponse>,
        IRequestHandler<UpdateUserProfileListCommand,DomainResponse>,
        IRequestHandler<ActiveUserProfileListCommand,DomainResponse>,
        IRequestHandler<DeactiveUserProfileListCommand,DomainResponse>
    {
        IUserProfileListRepository _userProfileListRepository;

        public UserProfileListCommandHandler(IServiceProvider provider,IMediator mediator,IUserProfileListRepository userProfileListRepository ) : base(provider,mediator) { _userProfileListRepository = userProfileListRepository; }

        partial void OnCreate(UserProfileList entity);
        partial void OnUpdate(UserProfileList entity);

        public async Task<DomainResponse> Handle(CreateUserProfileListCommand command,CancellationToken cancellationToken) {

            UserProfileList entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = UserProfileListFilters.GetFilters(command.Query ?? new UserProfileListQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _userProfileListRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateUserProfileListCommand(
                            command.Context,
                            new Queries.Models.UserProfileListQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.UserProfileList>();
            entity.AddDomainEvent(new UserProfileListCreatedEvent(command.Context,entity));

            _userProfileListRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_userProfileListRepository.Add(entity);

            return await Commit(_userProfileListRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteUserProfileListCommand command,CancellationToken cancellationToken) {
            var filter = UserProfileListFilters.GetFilters(command.Query);
			var entity = await _userProfileListRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(UserProfileList)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _userProfileListRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new UserProfileListDeletedEvent(command.Context,entity));
            return await Commit(_userProfileListRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeUserProfileListCommand command,CancellationToken cancellationToken) {
            var filter = UserProfileListFilters.GetFilters(command.Query);
			var entities = await _userProfileListRepository.FindAllAsync(filter);

			_userProfileListRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_userProfileListRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateUserProfileListCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeUserProfileListCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeUserProfileListCommand command,CancellationToken cancellationToken) {
            var entities = new List<UserProfileList>();
            foreach (var item in command.Query)
            {
                var filter = UserProfileListFilters.GetFilters(item.Key);
                var entity = await _userProfileListRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateUserProfileListCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(UserProfileList)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<UserProfileList>();
                _userProfileListRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new UserProfileListUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_userProfileListRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveUserProfileListCommand command,CancellationToken cancellationToken) {
            var userprofilelist = await _userProfileListRepository.FindAsync(filter: UserProfileListFilters.GetFilters(command.Query));
            //userprofilelist.Disable();

            PublishLog(command);
            
            return await Commit(_userProfileListRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveUserProfileListCommand command,CancellationToken cancellationToken) {
            var userprofilelist = await _userProfileListRepository.FindAsync(filter: UserProfileListFilters.GetFilters(command.Query));
            //userprofilelist.Enable();

            PublishLog(command);
            
            return await Commit(_userProfileListRepository.UnitOfWork);
        }
    }
    public partial class UserProfileCommandHandler : BaseUsersAggCommandHandler<UserProfile>,
        IRequestHandler<CreateUserProfileCommand,DomainResponse>,
        IRequestHandler<DeleteRangeUserProfileCommand,DomainResponse>,
        IRequestHandler<DeleteUserProfileCommand,DomainResponse>,
        IRequestHandler<UpdateRangeUserProfileCommand,DomainResponse>,
        IRequestHandler<UpdateUserProfileCommand,DomainResponse>,
        IRequestHandler<ActiveUserProfileCommand,DomainResponse>,
        IRequestHandler<DeactiveUserProfileCommand,DomainResponse>
    {
        IUserProfileRepository _userProfileRepository;

        public UserProfileCommandHandler(IServiceProvider provider,IMediator mediator,IUserProfileRepository userProfileRepository ) : base(provider,mediator) { _userProfileRepository = userProfileRepository; }

        partial void OnCreate(UserProfile entity);
        partial void OnUpdate(UserProfile entity);

        public async Task<DomainResponse> Handle(CreateUserProfileCommand command,CancellationToken cancellationToken) {

            UserProfile entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = UserProfileFilters.GetFilters(command.Query ?? new UserProfileQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _userProfileRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateUserProfileCommand(
                            command.Context,
                            new Queries.Models.UserProfileQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.UserProfile>();
            entity.AddDomainEvent(new UserProfileCreatedEvent(command.Context,entity));

            _userProfileRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_userProfileRepository.Add(entity);

            return await Commit(_userProfileRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteUserProfileCommand command,CancellationToken cancellationToken) {
            var filter = UserProfileFilters.GetFilters(command.Query);
			var entity = await _userProfileRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(UserProfile)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _userProfileRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new UserProfileDeletedEvent(command.Context,entity));
            return await Commit(_userProfileRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeUserProfileCommand command,CancellationToken cancellationToken) {
            var filter = UserProfileFilters.GetFilters(command.Query);
			var entities = await _userProfileRepository.FindAllAsync(filter);

			_userProfileRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_userProfileRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateUserProfileCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeUserProfileCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeUserProfileCommand command,CancellationToken cancellationToken) {
            var entities = new List<UserProfile>();
            foreach (var item in command.Query)
            {
                var filter = UserProfileFilters.GetFilters(item.Key);
                var entity = await _userProfileRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateUserProfileCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(UserProfile)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<UserProfile>();
                _userProfileRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new UserProfileUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_userProfileRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveUserProfileCommand command,CancellationToken cancellationToken) {
            var userprofile = await _userProfileRepository.FindAsync(filter: UserProfileFilters.GetFilters(command.Query));
            //userprofile.Disable();

            PublishLog(command);
            
            return await Commit(_userProfileRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveUserProfileCommand command,CancellationToken cancellationToken) {
            var userprofile = await _userProfileRepository.FindAsync(filter: UserProfileFilters.GetFilters(command.Query));
            //userprofile.Enable();

            PublishLog(command);
            
            return await Commit(_userProfileRepository.UnitOfWork);
        }
    }
    public partial class UsersAggSettingsCommandHandler : BaseUsersAggCommandHandler<UsersAggSettings>,
        IRequestHandler<CreateUsersAggSettingsCommand,DomainResponse>,
        IRequestHandler<DeleteRangeUsersAggSettingsCommand,DomainResponse>,
        IRequestHandler<DeleteUsersAggSettingsCommand,DomainResponse>,
        IRequestHandler<UpdateRangeUsersAggSettingsCommand,DomainResponse>,
        IRequestHandler<UpdateUsersAggSettingsCommand,DomainResponse>,
        IRequestHandler<ActiveUsersAggSettingsCommand,DomainResponse>,
        IRequestHandler<DeactiveUsersAggSettingsCommand,DomainResponse>
    {
        IUsersAggSettingsRepository _usersAggSettingsRepository;

        public UsersAggSettingsCommandHandler(IServiceProvider provider,IMediator mediator,IUsersAggSettingsRepository usersAggSettingsRepository ) : base(provider,mediator) { _usersAggSettingsRepository = usersAggSettingsRepository; }

        partial void OnCreate(UsersAggSettings entity);
        partial void OnUpdate(UsersAggSettings entity);

        public async Task<DomainResponse> Handle(CreateUsersAggSettingsCommand command,CancellationToken cancellationToken) {

            UsersAggSettings entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = UsersAggSettingsFilters.GetFilters(command.Query ?? new UsersAggSettingsQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _usersAggSettingsRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateUsersAggSettingsCommand(
                            command.Context,
                            new Queries.Models.UsersAggSettingsQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.UsersAggSettings>();
            entity.AddDomainEvent(new UsersAggSettingsCreatedEvent(command.Context,entity));

            _usersAggSettingsRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_usersAggSettingsRepository.Add(entity);

            return await Commit(_usersAggSettingsRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteUsersAggSettingsCommand command,CancellationToken cancellationToken) {
            var filter = UsersAggSettingsFilters.GetFilters(command.Query);
			var entity = await _usersAggSettingsRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(UsersAggSettings)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _usersAggSettingsRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new UsersAggSettingsDeletedEvent(command.Context,entity));
            return await Commit(_usersAggSettingsRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeUsersAggSettingsCommand command,CancellationToken cancellationToken) {
            var filter = UsersAggSettingsFilters.GetFilters(command.Query);
			var entities = await _usersAggSettingsRepository.FindAllAsync(filter);

			_usersAggSettingsRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_usersAggSettingsRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateUsersAggSettingsCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeUsersAggSettingsCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeUsersAggSettingsCommand command,CancellationToken cancellationToken) {
            var entities = new List<UsersAggSettings>();
            foreach (var item in command.Query)
            {
                var filter = UsersAggSettingsFilters.GetFilters(item.Key);
                var entity = await _usersAggSettingsRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateUsersAggSettingsCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(UsersAggSettings)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<UsersAggSettings>();
                _usersAggSettingsRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new UsersAggSettingsUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_usersAggSettingsRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveUsersAggSettingsCommand command,CancellationToken cancellationToken) {
            var usersaggsettings = await _usersAggSettingsRepository.FindAsync(filter: UsersAggSettingsFilters.GetFilters(command.Query));
            //usersaggsettings.Disable();

            PublishLog(command);
            
            return await Commit(_usersAggSettingsRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveUsersAggSettingsCommand command,CancellationToken cancellationToken) {
            var usersaggsettings = await _usersAggSettingsRepository.FindAsync(filter: UsersAggSettingsFilters.GetFilters(command.Query));
            //usersaggsettings.Enable();

            PublishLog(command);
            
            return await Commit(_usersAggSettingsRepository.UnitOfWork);
        }
    }
    public partial class UserCommandHandler : BaseUsersAggCommandHandler<User>,
        IRequestHandler<CreateUserCommand,DomainResponse>,
        IRequestHandler<DeleteRangeUserCommand,DomainResponse>,
        IRequestHandler<DeleteUserCommand,DomainResponse>,
        IRequestHandler<UpdateRangeUserCommand,DomainResponse>,
        IRequestHandler<UpdateUserCommand,DomainResponse>,
        IRequestHandler<ActiveUserCommand,DomainResponse>,
        IRequestHandler<DeactiveUserCommand,DomainResponse>
    {
        IUserRepository _userRepository;

        public UserCommandHandler(IServiceProvider provider,IMediator mediator,IUserRepository userRepository ) : base(provider,mediator) { _userRepository = userRepository; }

        partial void OnCreate(User entity);
        partial void OnUpdate(User entity);

        public async Task<DomainResponse> Handle(CreateUserCommand command,CancellationToken cancellationToken) {

            User entity;
            if (command.Query != null || !string.IsNullOrWhiteSpace(command.Request.ExternalId))
            {
                var filter = UserFilters.GetFilters(command.Query ?? new UserQueryModel { ExternalIdEqual = command.Request.ExternalId });
                entity = await _userRepository.FindAsync(filter, includeAll: false);
                if (entity != null)
                {
                    if (command.UpdateIfExists)
                        return await Handle(new UpdateUserCommand(
                            command.Context,
                            new Queries.Models.UserQueryModel { ExternalIdEqual = command.Request.ExternalId },
                            command.Request),
                        cancellationToken);
                }
            }
            entity = command.Request.ProjectedAs<Entities.User>();
            entity.AddDomainEvent(new UserCreatedEvent(command.Context,entity));

            _userRepository.UnitOfWork.ResolveAttaches(entity);
            var creationResult = await OnCreateAsync(entity);
            if (!creationResult.Success) return creationResult;
			_userRepository.Add(entity);

            return await Commit(_userRepository.UnitOfWork, entity);
        }

        public async Task<DomainResponse> Handle(DeleteUserCommand command,CancellationToken cancellationToken) {
            var filter = UserFilters.GetFilters(command.Query);
			var entity = await _userRepository.FindAsync(filter);

            if(entity is null) {
                return AddError($"Entity {nameof(User)} not found with the request.");
            }
            
            if (command.IsLogicalDeletion)
                entity.Delete();
            else
			    _userRepository.Delete(entity);

            var deleteResult = await OnDeleteAsync(entity);
            if (!deleteResult.Success) return deleteResult;

            entity.AddDomainEvent(new UserDeletedEvent(command.Context,entity));
            return await Commit(_userRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeleteRangeUserCommand command,CancellationToken cancellationToken) {
            var filter = UserFilters.GetFilters(command.Query);
			var entities = await _userRepository.FindAllAsync(filter);

			_userRepository.DeleteRange(entities);

            PublishLog(command);
            
            return await Commit(_userRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(UpdateUserCommand command,CancellationToken cancellationToken) {
            return await Handle(new UpdateRangeUserCommand(command.Context,command.Query,command.Request),cancellationToken);
        }

        public async Task<DomainResponse> Handle(UpdateRangeUserCommand command,CancellationToken cancellationToken) {
            var entities = new List<User>();
            foreach (var item in command.Query)
            {
                var filter = UserFilters.GetFilters(item.Key);
                var entity = await _userRepository.FindAsync(filter);
                
                if(entity == null) {
                    if(command.CreateIfNotExists)
                        return await Handle(new CreateUserCommand(command.Context,item.Value),cancellationToken);
                    return AddError($"Entity {nameof(User)} not found with the request.");
                }
                var entityAfter = item.Value.ProjectedAs<User>();
                _userRepository.UnitOfWork.ResolveAttachesOnUpdate(entity, entityAfter);
                entity.Update(entityAfter,"Id");
                var updateResult = await OnUpdateAsync(entity, entityAfter);
                if (!updateResult.Success) return updateResult;
                entity.AddDomainEvent(new UserUpdatedEvent(command.Context, entity));
            }
            
            PublishLog(command);

            return await Commit(_userRepository.UnitOfWork);
        }
         
        public async Task<DomainResponse> Handle(ActiveUserCommand command,CancellationToken cancellationToken) {
            var user = await _userRepository.FindAsync(filter: UserFilters.GetFilters(command.Query));
            //user.Disable();

            PublishLog(command);
            
            return await Commit(_userRepository.UnitOfWork);
        }

        public async Task<DomainResponse> Handle(DeactiveUserCommand command,CancellationToken cancellationToken) {
            var user = await _userRepository.FindAsync(filter: UserFilters.GetFilters(command.Query));
            //user.Enable();

            PublishLog(command);
            
            return await Commit(_userRepository.UnitOfWork);
        }
    }
}
