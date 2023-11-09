
using MediatR;
using System.Linq.Expressions;
using FluentValidation.Results;
using LazyCrud.CrossCuting.Infra.Utils.Extensions;
using LazyCrud.Core.Application.DTO.Extensions;
using LazyCrud.Core.Application.Aggregates.Common;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Queries;
using LazyCrud.Core.Domain.CrossCutting;

namespace LazyCrud.SystemSettings.Application.Aggregates.SystemSettingsAgg.AppServices {
	using Application.DTO.Aggregates.SystemSettingsAgg.Requests;
	using Domain.Aggregates.SystemSettingsAgg.Queries.Models;
	using Domain.Aggregates.SystemSettingsAgg.Repositories;
	using Domain.Aggregates.SystemSettingsAgg.Filters;
	using Domain.Aggregates.SystemSettingsAgg.Entities;
	public partial class SystemPanelSubItemAppService : BaseAppService, ISystemPanelSubItemAppService {	
		public ISystemPanelSubItemRepository _systemPanelSubItemRepository;
		public SystemPanelSubItemAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, ISystemPanelSubItemRepository systemPanelSubItemRepository) : base(ctx, mediator) { _systemPanelSubItemRepository = systemPanelSubItemRepository; }	
		public async Task<SystemPanelSubItemDTO> Get(IQueryModel<SystemPanelSubItem> request) {
            return (await _systemPanelSubItemRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<SystemPanelSubItemDTO>()));
        }
		public void Dispose()
        {
			_systemPanelSubItemRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<SystemPanelSubItem> request, int? page = null, int? size = null, Expression<Func<SystemPanelSubItem, T>> selector = null) {
			return await _systemPanelSubItemRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<SystemPanelSubItem>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<SystemPanelSubItem> request, Expression<Func<SystemPanelSubItem, T>> selector = null)
        {
            return (await _systemPanelSubItemRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<SystemPanelSubItemDTO>> GetAll(IQueryModel<SystemPanelSubItem> request, int? page = null, int? size = null) {
            return await _systemPanelSubItemRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<SystemPanelSubItem>(),
                selector: x => x.ProjectedAs<SystemPanelSubItemDTO>());
        }
		public async Task<IEnumerable<SystemPanelSubItemListiningDTO>> GetAllSummary(IQueryModel<SystemPanelSubItem> request, int? page = null, int? size = null)
        {
            return await _systemPanelSubItemRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<SystemPanelSubItem>(),
                selector: x => x.ProjectedAs<SystemPanelSubItemListiningDTO>());
        }

		public Task<DomainResponse> Create(SystemPanelSubItemDTO request, bool updateIfExists = true, IQueryModel<SystemPanelSubItem> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<SystemPanelSubItem> request) { return await _systemPanelSubItemRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<SystemPanelSubItem> searchQuery, SystemPanelSubItemDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<SystemPanelSubItem> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<SystemPanelSubItem> request){ return _mediator.Send(request.Command); }
	}
	public partial class SystemPanelAppService : BaseAppService, ISystemPanelAppService {	
		public ISystemPanelRepository _systemPanelRepository;
		public SystemPanelAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, ISystemPanelRepository systemPanelRepository) : base(ctx, mediator) { _systemPanelRepository = systemPanelRepository; }	
		public async Task<SystemPanelDTO> Get(IQueryModel<SystemPanel> request) {
            return (await _systemPanelRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<SystemPanelDTO>()));
        }
		public void Dispose()
        {
			_systemPanelRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<SystemPanel> request, int? page = null, int? size = null, Expression<Func<SystemPanel, T>> selector = null) {
			return await _systemPanelRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<SystemPanel>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<SystemPanel> request, Expression<Func<SystemPanel, T>> selector = null)
        {
            return (await _systemPanelRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<SystemPanelDTO>> GetAll(IQueryModel<SystemPanel> request, int? page = null, int? size = null) {
            return await _systemPanelRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<SystemPanel>(),
                selector: x => x.ProjectedAs<SystemPanelDTO>());
        }
		public async Task<IEnumerable<SystemPanelListiningDTO>> GetAllSummary(IQueryModel<SystemPanel> request, int? page = null, int? size = null)
        {
            return await _systemPanelRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<SystemPanel>(),
                selector: x => x.ProjectedAs<SystemPanelListiningDTO>());
        }

		public Task<DomainResponse> Create(SystemPanelDTO request, bool updateIfExists = true, IQueryModel<SystemPanel> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<SystemPanel> request) { return await _systemPanelRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<SystemPanel> searchQuery, SystemPanelDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<SystemPanel> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<SystemPanel> request){ return _mediator.Send(request.Command); }
	}
	public partial class SystemPanelGroupAppService : BaseAppService, ISystemPanelGroupAppService {	
		public ISystemPanelGroupRepository _systemPanelGroupRepository;
		public SystemPanelGroupAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, ISystemPanelGroupRepository systemPanelGroupRepository) : base(ctx, mediator) { _systemPanelGroupRepository = systemPanelGroupRepository; }	
		public async Task<SystemPanelGroupDTO> Get(IQueryModel<SystemPanelGroup> request) {
            return (await _systemPanelGroupRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<SystemPanelGroupDTO>()));
        }
		public void Dispose()
        {
			_systemPanelGroupRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<SystemPanelGroup> request, int? page = null, int? size = null, Expression<Func<SystemPanelGroup, T>> selector = null) {
			return await _systemPanelGroupRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<SystemPanelGroup>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<SystemPanelGroup> request, Expression<Func<SystemPanelGroup, T>> selector = null)
        {
            return (await _systemPanelGroupRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<SystemPanelGroupDTO>> GetAll(IQueryModel<SystemPanelGroup> request, int? page = null, int? size = null) {
            return await _systemPanelGroupRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<SystemPanelGroup>(),
                selector: x => x.ProjectedAs<SystemPanelGroupDTO>());
        }
		public async Task<IEnumerable<SystemPanelGroupListiningDTO>> GetAllSummary(IQueryModel<SystemPanelGroup> request, int? page = null, int? size = null)
        {
            return await _systemPanelGroupRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<SystemPanelGroup>(),
                selector: x => x.ProjectedAs<SystemPanelGroupListiningDTO>());
        }

		public Task<DomainResponse> Create(SystemPanelGroupDTO request, bool updateIfExists = true, IQueryModel<SystemPanelGroup> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<SystemPanelGroup> request) { return await _systemPanelGroupRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<SystemPanelGroup> searchQuery, SystemPanelGroupDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<SystemPanelGroup> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<SystemPanelGroup> request){ return _mediator.Send(request.Command); }
	}
	public partial class CargaTabelaAppService : BaseAppService, ICargaTabelaAppService {	
		public ICargaTabelaRepository _cargaTabelaRepository;
		public CargaTabelaAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, ICargaTabelaRepository cargaTabelaRepository) : base(ctx, mediator) { _cargaTabelaRepository = cargaTabelaRepository; }	
		public async Task<CargaTabelaDTO> Get(IQueryModel<CargaTabela> request) {
            return (await _cargaTabelaRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<CargaTabelaDTO>()));
        }
		public void Dispose()
        {
			_cargaTabelaRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<CargaTabela> request, int? page = null, int? size = null, Expression<Func<CargaTabela, T>> selector = null) {
			return await _cargaTabelaRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<CargaTabela>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<CargaTabela> request, Expression<Func<CargaTabela, T>> selector = null)
        {
            return (await _cargaTabelaRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<CargaTabelaDTO>> GetAll(IQueryModel<CargaTabela> request, int? page = null, int? size = null) {
            return await _cargaTabelaRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<CargaTabela>(),
                selector: x => x.ProjectedAs<CargaTabelaDTO>());
        }
		public async Task<IEnumerable<CargaTabelaListiningDTO>> GetAllSummary(IQueryModel<CargaTabela> request, int? page = null, int? size = null)
        {
            return await _cargaTabelaRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<CargaTabela>(),
                selector: x => x.ProjectedAs<CargaTabelaListiningDTO>());
        }

		public Task<DomainResponse> Create(CargaTabelaDTO request, bool updateIfExists = true, IQueryModel<CargaTabela> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<CargaTabela> request) { return await _cargaTabelaRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<CargaTabela> searchQuery, CargaTabelaDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<CargaTabela> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<CargaTabela> request){ return _mediator.Send(request.Command); }
	}
	public partial class SystemSettingsAggSettingsAppService : BaseAppService, ISystemSettingsAggSettingsAppService {	
		public ISystemSettingsAggSettingsRepository _systemSettingsAggSettingsRepository;
		public SystemSettingsAggSettingsAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, ISystemSettingsAggSettingsRepository systemSettingsAggSettingsRepository) : base(ctx, mediator) { _systemSettingsAggSettingsRepository = systemSettingsAggSettingsRepository; }	
		public async Task<SystemSettingsAggSettingsDTO> Get(IQueryModel<SystemSettingsAggSettings> request) {
            return (await _systemSettingsAggSettingsRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<SystemSettingsAggSettingsDTO>()));
        }
		public void Dispose()
        {
			_systemSettingsAggSettingsRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<SystemSettingsAggSettings> request, int? page = null, int? size = null, Expression<Func<SystemSettingsAggSettings, T>> selector = null) {
			return await _systemSettingsAggSettingsRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<SystemSettingsAggSettings>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<SystemSettingsAggSettings> request, Expression<Func<SystemSettingsAggSettings, T>> selector = null)
        {
            return (await _systemSettingsAggSettingsRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<SystemSettingsAggSettingsDTO>> GetAll(IQueryModel<SystemSettingsAggSettings> request, int? page = null, int? size = null) {
            return await _systemSettingsAggSettingsRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<SystemSettingsAggSettings>(),
                selector: x => x.ProjectedAs<SystemSettingsAggSettingsDTO>());
        }
		public async Task<IEnumerable<SystemSettingsAggSettingsListiningDTO>> GetAllSummary(IQueryModel<SystemSettingsAggSettings> request, int? page = null, int? size = null)
        {
            return await _systemSettingsAggSettingsRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<SystemSettingsAggSettings>(),
                selector: x => x.ProjectedAs<SystemSettingsAggSettingsListiningDTO>());
        }

		public Task<DomainResponse> Create(SystemSettingsAggSettingsDTO request, bool updateIfExists = true, IQueryModel<SystemSettingsAggSettings> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<SystemSettingsAggSettings> request) { return await _systemSettingsAggSettingsRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<SystemSettingsAggSettings> searchQuery, SystemSettingsAggSettingsDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<SystemSettingsAggSettings> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<SystemSettingsAggSettings> request){ return _mediator.Send(request.Command); }
	}
}
