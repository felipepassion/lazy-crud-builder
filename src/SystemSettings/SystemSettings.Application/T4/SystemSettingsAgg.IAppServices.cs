using System.Linq.Expressions;
using FluentValidation.Results;
using LazyCrudBuilder.Core.Domain.CrossCutting;
using LazyCrudBuilder.Core.Application.Aggregates.Common;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Queries;

using LazyCrudBuilder.Core.Domain.Seedwork.Specification;
namespace LazyCrudBuilder.SystemSettings.Application.Aggregates.SystemSettingsAgg.AppServices {
	using Domain.Aggregates.SystemSettingsAgg.Entities;
	using Domain.Aggregates.SystemSettingsAgg.Queries;
	using Application.DTO.Aggregates.SystemSettingsAgg.Requests;
	public partial interface ISystemPanelSubItemAppService : IBaseAppService {	
		public Task<SystemPanelSubItemDTO> Get(IQueryModel<SystemPanelSubItem> request);
		public Task<int> CountAsync(IQueryModel<SystemPanelSubItem> request);
		public Task<IEnumerable<SystemPanelSubItemDTO>> GetAll(IQueryModel<SystemPanelSubItem> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<SystemPanelSubItem> request, Expression<Func<Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanelSubItem, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<SystemPanelSubItem> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanelSubItem, T>> selector = null);
		public Task<IEnumerable<SystemPanelSubItemListiningDTO>> GetAllSummary(IQueryModel<SystemPanelSubItem> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(SystemPanelSubItemDTO request, bool updateIfExists = true, IQueryModel<SystemPanelSubItem> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<SystemPanelSubItem> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<SystemPanelSubItem> request);
		public Task Update(IQueryModel<SystemPanelSubItem> searchQuery, SystemPanelSubItemDTO request, bool createIfNotExists = true);
	}
	public partial interface ISystemPanelAppService : IBaseAppService {	
		public Task<SystemPanelDTO> Get(IQueryModel<SystemPanel> request);
		public Task<int> CountAsync(IQueryModel<SystemPanel> request);
		public Task<IEnumerable<SystemPanelDTO>> GetAll(IQueryModel<SystemPanel> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<SystemPanel> request, Expression<Func<Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanel, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<SystemPanel> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanel, T>> selector = null);
		public Task<IEnumerable<SystemPanelListiningDTO>> GetAllSummary(IQueryModel<SystemPanel> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(SystemPanelDTO request, bool updateIfExists = true, IQueryModel<SystemPanel> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<SystemPanel> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<SystemPanel> request);
		public Task Update(IQueryModel<SystemPanel> searchQuery, SystemPanelDTO request, bool createIfNotExists = true);
	}
	public partial interface ISystemPanelGroupAppService : IBaseAppService {	
		public Task<SystemPanelGroupDTO> Get(IQueryModel<SystemPanelGroup> request);
		public Task<int> CountAsync(IQueryModel<SystemPanelGroup> request);
		public Task<IEnumerable<SystemPanelGroupDTO>> GetAll(IQueryModel<SystemPanelGroup> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<SystemPanelGroup> request, Expression<Func<Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanelGroup, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<SystemPanelGroup> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.SystemSettingsAgg.Entities.SystemPanelGroup, T>> selector = null);
		public Task<IEnumerable<SystemPanelGroupListiningDTO>> GetAllSummary(IQueryModel<SystemPanelGroup> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(SystemPanelGroupDTO request, bool updateIfExists = true, IQueryModel<SystemPanelGroup> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<SystemPanelGroup> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<SystemPanelGroup> request);
		public Task Update(IQueryModel<SystemPanelGroup> searchQuery, SystemPanelGroupDTO request, bool createIfNotExists = true);
	}
	public partial interface ICargaTabelaAppService : IBaseAppService {	
		public Task<CargaTabelaDTO> Get(IQueryModel<CargaTabela> request);
		public Task<int> CountAsync(IQueryModel<CargaTabela> request);
		public Task<IEnumerable<CargaTabelaDTO>> GetAll(IQueryModel<CargaTabela> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<CargaTabela> request, Expression<Func<Domain.Aggregates.SystemSettingsAgg.Entities.CargaTabela, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<CargaTabela> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.SystemSettingsAgg.Entities.CargaTabela, T>> selector = null);
		public Task<IEnumerable<CargaTabelaListiningDTO>> GetAllSummary(IQueryModel<CargaTabela> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(CargaTabelaDTO request, bool updateIfExists = true, IQueryModel<CargaTabela> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<CargaTabela> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<CargaTabela> request);
		public Task Update(IQueryModel<CargaTabela> searchQuery, CargaTabelaDTO request, bool createIfNotExists = true);
	}
	public partial interface ISystemSettingsAggSettingsAppService : IBaseAppService {	
		public Task<SystemSettingsAggSettingsDTO> Get(IQueryModel<SystemSettingsAggSettings> request);
		public Task<int> CountAsync(IQueryModel<SystemSettingsAggSettings> request);
		public Task<IEnumerable<SystemSettingsAggSettingsDTO>> GetAll(IQueryModel<SystemSettingsAggSettings> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<SystemSettingsAggSettings> request, Expression<Func<Domain.Aggregates.SystemSettingsAgg.Entities.SystemSettingsAggSettings, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<SystemSettingsAggSettings> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.SystemSettingsAgg.Entities.SystemSettingsAggSettings, T>> selector = null);
		public Task<IEnumerable<SystemSettingsAggSettingsListiningDTO>> GetAllSummary(IQueryModel<SystemSettingsAggSettings> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(SystemSettingsAggSettingsDTO request, bool updateIfExists = true, IQueryModel<SystemSettingsAggSettings> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<SystemSettingsAggSettings> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<SystemSettingsAggSettings> request);
		public Task Update(IQueryModel<SystemSettingsAggSettings> searchQuery, SystemSettingsAggSettingsDTO request, bool createIfNotExists = true);
	}
}
