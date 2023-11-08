
using MediatR;
using System.Linq.Expressions;
using FluentValidation.Results;
using LazyCrud.CrossCuting.Infra.Utils.Extensions;
using LazyCrud.Core.Application.DTO.Extensions;
using LazyCrud.Core.Application.Aggregates.Common;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Queries;
using LazyCrud.Core.Domain.CrossCutting;

namespace LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices {
	using Application.DTO.Aggregates.UsersAgg.Requests;
	using Domain.Aggregates.UsersAgg.Queries.Models;
	using Domain.Aggregates.UsersAgg.Repositories;
	using Domain.Aggregates.UsersAgg.Filters;
	using Domain.Aggregates.UsersAgg.Entities;
	public partial class UserProfileAccessAppService : BaseAppService, IUserProfileAccessAppService {	
		public IUserProfileAccessRepository _userProfileAccessRepository;
		public UserProfileAccessAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, IUserProfileAccessRepository userProfileAccessRepository) : base(ctx, mediator) { _userProfileAccessRepository = userProfileAccessRepository; }	
		public async Task<UserProfileAccessDTO> Get(IQueryModel<UserProfileAccess> request) {
            return (await _userProfileAccessRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<UserProfileAccessDTO>()));
        }
		public void Dispose()
        {
			_userProfileAccessRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<UserProfileAccess> request, int? page = null, int? size = null, Expression<Func<UserProfileAccess, T>> selector = null) {
			return await _userProfileAccessRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UserProfileAccess>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<UserProfileAccess> request, Expression<Func<UserProfileAccess, T>> selector = null)
        {
            return (await _userProfileAccessRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<UserProfileAccessDTO>> GetAll(IQueryModel<UserProfileAccess> request, int? page = null, int? size = null) {
            return await _userProfileAccessRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UserProfileAccess>(),
                selector: x => x.ProjectedAs<UserProfileAccessDTO>());
        }
		public async Task<IEnumerable<UserProfileAccessListiningDTO>> GetAllSummary(IQueryModel<UserProfileAccess> request, int? page = null, int? size = null)
        {
            return await _userProfileAccessRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UserProfileAccess>(),
                selector: x => x.ProjectedAs<UserProfileAccessListiningDTO>());
        }

		public Task<DomainResponse> Create(UserProfileAccessDTO request, bool updateIfExists = true, IQueryModel<UserProfileAccess> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<UserProfileAccess> request) { return await _userProfileAccessRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<UserProfileAccess> searchQuery, UserProfileAccessDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<UserProfileAccess> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<UserProfileAccess> request){ return _mediator.Send(request.Command); }
	}
	public partial class UserCurrentAccessSelectedAppService : BaseAppService, IUserCurrentAccessSelectedAppService {	
		public IUserCurrentAccessSelectedRepository _userCurrentAccessSelectedRepository;
		public UserCurrentAccessSelectedAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, IUserCurrentAccessSelectedRepository userCurrentAccessSelectedRepository) : base(ctx, mediator) { _userCurrentAccessSelectedRepository = userCurrentAccessSelectedRepository; }	
		public async Task<UserCurrentAccessSelectedDTO> Get(IQueryModel<UserCurrentAccessSelected> request) {
            return (await _userCurrentAccessSelectedRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<UserCurrentAccessSelectedDTO>()));
        }
		public void Dispose()
        {
			_userCurrentAccessSelectedRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<UserCurrentAccessSelected> request, int? page = null, int? size = null, Expression<Func<UserCurrentAccessSelected, T>> selector = null) {
			return await _userCurrentAccessSelectedRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UserCurrentAccessSelected>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<UserCurrentAccessSelected> request, Expression<Func<UserCurrentAccessSelected, T>> selector = null)
        {
            return (await _userCurrentAccessSelectedRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<UserCurrentAccessSelectedDTO>> GetAll(IQueryModel<UserCurrentAccessSelected> request, int? page = null, int? size = null) {
            return await _userCurrentAccessSelectedRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UserCurrentAccessSelected>(),
                selector: x => x.ProjectedAs<UserCurrentAccessSelectedDTO>());
        }
		public async Task<IEnumerable<UserCurrentAccessSelectedListiningDTO>> GetAllSummary(IQueryModel<UserCurrentAccessSelected> request, int? page = null, int? size = null)
        {
            return await _userCurrentAccessSelectedRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UserCurrentAccessSelected>(),
                selector: x => x.ProjectedAs<UserCurrentAccessSelectedListiningDTO>());
        }

		public Task<DomainResponse> Create(UserCurrentAccessSelectedDTO request, bool updateIfExists = true, IQueryModel<UserCurrentAccessSelected> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<UserCurrentAccessSelected> request) { return await _userCurrentAccessSelectedRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<UserCurrentAccessSelected> searchQuery, UserCurrentAccessSelectedDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<UserCurrentAccessSelected> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<UserCurrentAccessSelected> request){ return _mediator.Send(request.Command); }
	}
	public partial class UserProfileListAppService : BaseAppService, IUserProfileListAppService {	
		public IUserProfileListRepository _userProfileListRepository;
		public UserProfileListAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, IUserProfileListRepository userProfileListRepository) : base(ctx, mediator) { _userProfileListRepository = userProfileListRepository; }	
		public async Task<UserProfileListDTO> Get(IQueryModel<UserProfileList> request) {
            return (await _userProfileListRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<UserProfileListDTO>()));
        }
		public void Dispose()
        {
			_userProfileListRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<UserProfileList> request, int? page = null, int? size = null, Expression<Func<UserProfileList, T>> selector = null) {
			return await _userProfileListRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UserProfileList>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<UserProfileList> request, Expression<Func<UserProfileList, T>> selector = null)
        {
            return (await _userProfileListRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<UserProfileListDTO>> GetAll(IQueryModel<UserProfileList> request, int? page = null, int? size = null) {
            return await _userProfileListRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UserProfileList>(),
                selector: x => x.ProjectedAs<UserProfileListDTO>());
        }
		public async Task<IEnumerable<UserProfileListListiningDTO>> GetAllSummary(IQueryModel<UserProfileList> request, int? page = null, int? size = null)
        {
            return await _userProfileListRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UserProfileList>(),
                selector: x => x.ProjectedAs<UserProfileListListiningDTO>());
        }

		public Task<DomainResponse> Create(UserProfileListDTO request, bool updateIfExists = true, IQueryModel<UserProfileList> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<UserProfileList> request) { return await _userProfileListRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<UserProfileList> searchQuery, UserProfileListDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<UserProfileList> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<UserProfileList> request){ return _mediator.Send(request.Command); }
	}
	public partial class UserProfileAppService : BaseAppService, IUserProfileAppService {	
		public IUserProfileRepository _userProfileRepository;
		public UserProfileAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, IUserProfileRepository userProfileRepository) : base(ctx, mediator) { _userProfileRepository = userProfileRepository; }	
		public async Task<UserProfileDTO> Get(IQueryModel<UserProfile> request) {
            return (await _userProfileRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<UserProfileDTO>()));
        }
		public void Dispose()
        {
			_userProfileRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<UserProfile> request, int? page = null, int? size = null, Expression<Func<UserProfile, T>> selector = null) {
			return await _userProfileRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UserProfile>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<UserProfile> request, Expression<Func<UserProfile, T>> selector = null)
        {
            return (await _userProfileRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<UserProfileDTO>> GetAll(IQueryModel<UserProfile> request, int? page = null, int? size = null) {
            return await _userProfileRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UserProfile>(),
                selector: x => x.ProjectedAs<UserProfileDTO>());
        }
		public async Task<IEnumerable<UserProfileListiningDTO>> GetAllSummary(IQueryModel<UserProfile> request, int? page = null, int? size = null)
        {
            return await _userProfileRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UserProfile>(),
                selector: x => x.ProjectedAs<UserProfileListiningDTO>());
        }

		public Task<DomainResponse> Create(UserProfileDTO request, bool updateIfExists = true, IQueryModel<UserProfile> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<UserProfile> request) { return await _userProfileRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<UserProfile> searchQuery, UserProfileDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<UserProfile> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<UserProfile> request){ return _mediator.Send(request.Command); }
	}
	public partial class UsersAggSettingsAppService : BaseAppService, IUsersAggSettingsAppService {	
		public IUsersAggSettingsRepository _usersAggSettingsRepository;
		public UsersAggSettingsAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, IUsersAggSettingsRepository usersAggSettingsRepository) : base(ctx, mediator) { _usersAggSettingsRepository = usersAggSettingsRepository; }	
		public async Task<UsersAggSettingsDTO> Get(IQueryModel<UsersAggSettings> request) {
            return (await _usersAggSettingsRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<UsersAggSettingsDTO>()));
        }
		public void Dispose()
        {
			_usersAggSettingsRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<UsersAggSettings> request, int? page = null, int? size = null, Expression<Func<UsersAggSettings, T>> selector = null) {
			return await _usersAggSettingsRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UsersAggSettings>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<UsersAggSettings> request, Expression<Func<UsersAggSettings, T>> selector = null)
        {
            return (await _usersAggSettingsRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<UsersAggSettingsDTO>> GetAll(IQueryModel<UsersAggSettings> request, int? page = null, int? size = null) {
            return await _usersAggSettingsRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UsersAggSettings>(),
                selector: x => x.ProjectedAs<UsersAggSettingsDTO>());
        }
		public async Task<IEnumerable<UsersAggSettingsListiningDTO>> GetAllSummary(IQueryModel<UsersAggSettings> request, int? page = null, int? size = null)
        {
            return await _usersAggSettingsRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<UsersAggSettings>(),
                selector: x => x.ProjectedAs<UsersAggSettingsListiningDTO>());
        }

		public Task<DomainResponse> Create(UsersAggSettingsDTO request, bool updateIfExists = true, IQueryModel<UsersAggSettings> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<UsersAggSettings> request) { return await _usersAggSettingsRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<UsersAggSettings> searchQuery, UsersAggSettingsDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<UsersAggSettings> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<UsersAggSettings> request){ return _mediator.Send(request.Command); }
	}
	public partial class UserAppService : BaseAppService, IUserAppService {	
		public IUserRepository _userRepository;
		public UserAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, IUserRepository userRepository) : base(ctx, mediator) { _userRepository = userRepository; }	
		public async Task<UserDTO> Get(IQueryModel<User> request) {
            return (await _userRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<UserDTO>()));
        }
		public void Dispose()
        {
			_userRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<User> request, int? page = null, int? size = null, Expression<Func<User, T>> selector = null) {
			return await _userRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<User>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<User> request, Expression<Func<User, T>> selector = null)
        {
            return (await _userRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<UserDTO>> GetAll(IQueryModel<User> request, int? page = null, int? size = null) {
            return await _userRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<User>(),
                selector: x => x.ProjectedAs<UserDTO>());
        }
		public async Task<IEnumerable<UserListiningDTO>> GetAllSummary(IQueryModel<User> request, int? page = null, int? size = null)
        {
            return await _userRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<User>(),
                selector: x => x.ProjectedAs<UserListiningDTO>());
        }

		public Task<DomainResponse> Create(UserDTO request, bool updateIfExists = true, IQueryModel<User> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<User> request) { return await _userRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<User> searchQuery, UserDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<User> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<User> request){ return _mediator.Send(request.Command); }
	}
}
