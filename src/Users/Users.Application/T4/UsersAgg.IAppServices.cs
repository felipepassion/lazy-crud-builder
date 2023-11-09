using System.Linq.Expressions;
using FluentValidation.Results;
using LazyCrudBuilder.Core.Domain.CrossCutting;
using LazyCrudBuilder.Core.Application.Aggregates.Common;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Queries;

using LazyCrudBuilder.Core.Domain.Seedwork.Specification;
namespace LazyCrudBuilder.Users.Application.Aggregates.UsersAgg.AppServices {
	using Domain.Aggregates.UsersAgg.Entities;
	using Domain.Aggregates.UsersAgg.Queries;
	using Application.DTO.Aggregates.UsersAgg.Requests;
	public partial interface IUserProfileAccessAppService : IBaseAppService {	
		public Task<UserProfileAccessDTO> Get(IQueryModel<UserProfileAccess> request);
		public Task<int> CountAsync(IQueryModel<UserProfileAccess> request);
		public Task<IEnumerable<UserProfileAccessDTO>> GetAll(IQueryModel<UserProfileAccess> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<UserProfileAccess> request, Expression<Func<Domain.Aggregates.UsersAgg.Entities.UserProfileAccess, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<UserProfileAccess> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.UsersAgg.Entities.UserProfileAccess, T>> selector = null);
		public Task<IEnumerable<UserProfileAccessListiningDTO>> GetAllSummary(IQueryModel<UserProfileAccess> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(UserProfileAccessDTO request, bool updateIfExists = true, IQueryModel<UserProfileAccess> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<UserProfileAccess> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<UserProfileAccess> request);
		public Task Update(IQueryModel<UserProfileAccess> searchQuery, UserProfileAccessDTO request, bool createIfNotExists = true);
	}
	public partial interface IUserCurrentAccessSelectedAppService : IBaseAppService {	
		public Task<UserCurrentAccessSelectedDTO> Get(IQueryModel<UserCurrentAccessSelected> request);
		public Task<int> CountAsync(IQueryModel<UserCurrentAccessSelected> request);
		public Task<IEnumerable<UserCurrentAccessSelectedDTO>> GetAll(IQueryModel<UserCurrentAccessSelected> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<UserCurrentAccessSelected> request, Expression<Func<Domain.Aggregates.UsersAgg.Entities.UserCurrentAccessSelected, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<UserCurrentAccessSelected> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.UsersAgg.Entities.UserCurrentAccessSelected, T>> selector = null);
		public Task<IEnumerable<UserCurrentAccessSelectedListiningDTO>> GetAllSummary(IQueryModel<UserCurrentAccessSelected> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(UserCurrentAccessSelectedDTO request, bool updateIfExists = true, IQueryModel<UserCurrentAccessSelected> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<UserCurrentAccessSelected> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<UserCurrentAccessSelected> request);
		public Task Update(IQueryModel<UserCurrentAccessSelected> searchQuery, UserCurrentAccessSelectedDTO request, bool createIfNotExists = true);
	}
	public partial interface IUserProfileListAppService : IBaseAppService {	
		public Task<UserProfileListDTO> Get(IQueryModel<UserProfileList> request);
		public Task<int> CountAsync(IQueryModel<UserProfileList> request);
		public Task<IEnumerable<UserProfileListDTO>> GetAll(IQueryModel<UserProfileList> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<UserProfileList> request, Expression<Func<Domain.Aggregates.UsersAgg.Entities.UserProfileList, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<UserProfileList> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.UsersAgg.Entities.UserProfileList, T>> selector = null);
		public Task<IEnumerable<UserProfileListListiningDTO>> GetAllSummary(IQueryModel<UserProfileList> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(UserProfileListDTO request, bool updateIfExists = true, IQueryModel<UserProfileList> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<UserProfileList> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<UserProfileList> request);
		public Task Update(IQueryModel<UserProfileList> searchQuery, UserProfileListDTO request, bool createIfNotExists = true);
	}
	public partial interface IUserProfileAppService : IBaseAppService {	
		public Task<UserProfileDTO> Get(IQueryModel<UserProfile> request);
		public Task<int> CountAsync(IQueryModel<UserProfile> request);
		public Task<IEnumerable<UserProfileDTO>> GetAll(IQueryModel<UserProfile> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<UserProfile> request, Expression<Func<Domain.Aggregates.UsersAgg.Entities.UserProfile, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<UserProfile> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.UsersAgg.Entities.UserProfile, T>> selector = null);
		public Task<IEnumerable<UserProfileListiningDTO>> GetAllSummary(IQueryModel<UserProfile> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(UserProfileDTO request, bool updateIfExists = true, IQueryModel<UserProfile> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<UserProfile> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<UserProfile> request);
		public Task Update(IQueryModel<UserProfile> searchQuery, UserProfileDTO request, bool createIfNotExists = true);
	}
	public partial interface IUsersAggSettingsAppService : IBaseAppService {	
		public Task<UsersAggSettingsDTO> Get(IQueryModel<UsersAggSettings> request);
		public Task<int> CountAsync(IQueryModel<UsersAggSettings> request);
		public Task<IEnumerable<UsersAggSettingsDTO>> GetAll(IQueryModel<UsersAggSettings> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<UsersAggSettings> request, Expression<Func<Domain.Aggregates.UsersAgg.Entities.UsersAggSettings, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<UsersAggSettings> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.UsersAgg.Entities.UsersAggSettings, T>> selector = null);
		public Task<IEnumerable<UsersAggSettingsListiningDTO>> GetAllSummary(IQueryModel<UsersAggSettings> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(UsersAggSettingsDTO request, bool updateIfExists = true, IQueryModel<UsersAggSettings> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<UsersAggSettings> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<UsersAggSettings> request);
		public Task Update(IQueryModel<UsersAggSettings> searchQuery, UsersAggSettingsDTO request, bool createIfNotExists = true);
	}
	public partial interface IUserAppService : IBaseAppService {	
		public Task<UserDTO> Get(IQueryModel<User> request);
		public Task<int> CountAsync(IQueryModel<User> request);
		public Task<IEnumerable<UserDTO>> GetAll(IQueryModel<User> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<User> request, Expression<Func<Domain.Aggregates.UsersAgg.Entities.User, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<User> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.UsersAgg.Entities.User, T>> selector = null);
		public Task<IEnumerable<UserListiningDTO>> GetAllSummary(IQueryModel<User> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(UserDTO request, bool updateIfExists = true, IQueryModel<User> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<User> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<User> request);
		public Task Update(IQueryModel<User> searchQuery, UserDTO request, bool createIfNotExists = true);
	}
}
