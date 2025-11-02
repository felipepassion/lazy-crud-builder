using System.Linq.Expressions;
using Niu.Nutri.Core.Domain.CrossCutting;
using Niu.Nutri.Core.Application.Aggregates.Common;

namespace Niu.Nutri.DefaultTemplate.Application.Aggregates.DefaultTemplateAgg.AppServices {
	using Application.DTO.Aggregates.DefaultTemplateAgg.Requests;
    using Domain.Aggregates.DefaultTemplateAgg.Queries.Models;
	public partial interface IDefaultEntityAppService : IBaseAppService {	
		public Task<DefaultEntityDTO> Get(DefaultEntityQueryModel request);
		public Task<int> CountAsync(DefaultEntityQueryModel request);
		public Task<IEnumerable<DefaultEntityDTO>> GetAll(DefaultEntityQueryModel request, int? page = null, int? size = null);
		public Task<T> Select<T>(DefaultEntityQueryModel request, Expression<Func<Domain.Aggregates.DefaultTemplateAgg.Entities.DefaultEntity, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(DefaultEntityQueryModel request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.DefaultTemplateAgg.Entities.DefaultEntity, T>> selector = null);
		public Task<IEnumerable<DefaultEntityListiningDTO>> GetAllSummary(DefaultEntityQueryModel request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(DefaultEntityDTO request, bool updateIfExists = true, DefaultEntityQueryModel searchQuery = null);
		public Task<DomainResponse> Delete(DefaultEntityQueryModel request);
		public Task<DomainResponse> DeleteRange(DefaultEntityQueryModel request);
		public Task Update(DefaultEntityQueryModel searchQuery, DefaultEntityDTO request, bool createIfNotExists = true);
	}
	public partial interface IDefaultTemplateAggSettingsAppService : IBaseAppService {	
		public Task<DefaultTemplateAggSettingsDTO> Get(DefaultTemplateAggSettingsQueryModel request);
		public Task<int> CountAsync(DefaultTemplateAggSettingsQueryModel request);
		public Task<IEnumerable<DefaultTemplateAggSettingsDTO>> GetAll(DefaultTemplateAggSettingsQueryModel request, int? page = null, int? size = null);
		public Task<T> Select<T>(DefaultTemplateAggSettingsQueryModel request, Expression<Func<Domain.Aggregates.DefaultTemplateAgg.Entities.DefaultTemplateAggSettings, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(DefaultTemplateAggSettingsQueryModel request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.DefaultTemplateAgg.Entities.DefaultTemplateAggSettings, T>> selector = null);
		public Task<IEnumerable<DefaultTemplateAggSettingsListiningDTO>> GetAllSummary(DefaultTemplateAggSettingsQueryModel request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(DefaultTemplateAggSettingsDTO request, bool updateIfExists = true, DefaultTemplateAggSettingsQueryModel searchQuery = null);
		public Task<DomainResponse> Delete(DefaultTemplateAggSettingsQueryModel request);
		public Task<DomainResponse> DeleteRange(DefaultTemplateAggSettingsQueryModel request);
		public Task Update(DefaultTemplateAggSettingsQueryModel searchQuery, DefaultTemplateAggSettingsDTO request, bool createIfNotExists = true);
	}
}
