
using MediatR;
using System.Linq.Expressions;
using Niu.Nutri.CrossCuting.Infra.Utils.Extensions;
using Niu.Nutri.Core.Application.DTO.Extensions;
using Niu.Nutri.Core.Application.Aggregates.Common;
using Niu.Nutri.Core.Domain.CrossCutting;

namespace Niu.Nutri.DefaultTemplate.Application.Aggregates.DefaultTemplateAgg.AppServices {
	using Application.DTO.Aggregates.DefaultTemplateAgg.Requests;
	using Domain.Aggregates.DefaultTemplateAgg.Queries.Models;
	using Domain.Aggregates.DefaultTemplateAgg.Repositories;
	using Domain.Aggregates.DefaultTemplateAgg.Filters;
	using Domain.Aggregates.DefaultTemplateAgg.Entities;
	using Domain.Aggregates.DefaultTemplateAgg.CommandModels;
	public partial class DefaultEntityAppService : BaseAppService, IDefaultEntityAppService {	
		public IDefaultEntityRepository _defaultEntityRepository;
		public DefaultEntityAppService(IMediator mediator, Niu.Nutri.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, IDefaultEntityRepository defaultEntityRepository) : base(ctx, mediator) { _defaultEntityRepository = defaultEntityRepository; }	
		public async Task<DefaultEntityDTO> Get(DefaultEntityQueryModel request) {
            return (await _defaultEntityRepository.FindAsync(filter: DefaultEntityFilters.GetFilters(request, isOrSpecification: request.IsOrSpecification), selector: x => x.ProjectedAs<DefaultEntityDTO>()));
        }
		public void Dispose()
        {
			_defaultEntityRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(DefaultEntityQueryModel request, int? page = null, int? size = null, Expression<Func<DefaultEntity, T>> selector = null) {
			return await _defaultEntityRepository.SelectAllAsync(
                filter: DefaultEntityFilters.GetFilters(request, isOrSpecification: request.IsOrSpecification),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<DefaultEntity>(),
                selector: selector);
		}
		public async Task<T> Select<T>(DefaultEntityQueryModel request, Expression<Func<DefaultEntity, T>> selector = null)
        {
            return (await _defaultEntityRepository.FindAsync(filter: DefaultEntityFilters.GetFilters(request, isOrSpecification: true), selector: selector));
        }
        public async Task<IEnumerable<DefaultEntityDTO>> GetAll(DefaultEntityQueryModel request, int? page = null, int? size = null) {
            return await _defaultEntityRepository.FindAllAsync(
                filter: DefaultEntityFilters.GetFilters(request, isOrSpecification: true),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<DefaultEntity>(),
                selector: x => x.ProjectedAs<DefaultEntityDTO>());
        }
		public async Task<IEnumerable<DefaultEntityListiningDTO>> GetAllSummary(DefaultEntityQueryModel request, int? page = null, int? size = null)
        {
            return await _defaultEntityRepository.FindAllAsync(
                filter: DefaultEntityFilters.GetFilters(request, isOrSpecification: true),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<DefaultEntity>(),
                selector: x => x.ProjectedAs<DefaultEntityListiningDTO>());
        }

		public Task<DomainResponse> Create(DefaultEntityDTO request, bool updateIfExists = true, DefaultEntityQueryModel searchQuery = null) { return _mediator.Send(new CreateDefaultEntityCommand(_logRequestContext, request)); }
		public async Task<int> CountAsync(DefaultEntityQueryModel request) { return await _defaultEntityRepository.CountAsync(filter: DefaultEntityFilters.GetFilters(request, isOrSpecification: true)); }
		public Task Update(DefaultEntityQueryModel searchQuery, DefaultEntityDTO request, bool createIfNotExists = true) { return _mediator.Send(new UpdateDefaultEntityCommand(_logRequestContext, searchQuery, request)); }
		public Task<DomainResponse> DeleteRange(DefaultEntityQueryModel request){ return _mediator.Send(new DeleteRangeDefaultEntityCommand(_logRequestContext, request)); }
		public Task<DomainResponse> Delete(DefaultEntityQueryModel request){ return _mediator.Send(new DeleteDefaultEntityCommand(_logRequestContext, request)); }
	}
	public partial class DefaultTemplateAggSettingsAppService : BaseAppService, IDefaultTemplateAggSettingsAppService {	
		public IDefaultTemplateAggSettingsRepository _defaultTemplateAggSettingsRepository;
		public DefaultTemplateAggSettingsAppService(IMediator mediator, Niu.Nutri.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, IDefaultTemplateAggSettingsRepository defaultTemplateAggSettingsRepository) : base(ctx, mediator) { _defaultTemplateAggSettingsRepository = defaultTemplateAggSettingsRepository; }	
		public async Task<DefaultTemplateAggSettingsDTO> Get(DefaultTemplateAggSettingsQueryModel request) {
            return (await _defaultTemplateAggSettingsRepository.FindAsync(filter: DefaultTemplateAggSettingsFilters.GetFilters(request, isOrSpecification: request.IsOrSpecification), selector: x => x.ProjectedAs<DefaultTemplateAggSettingsDTO>()));
        }
		public void Dispose()
        {
			_defaultTemplateAggSettingsRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(DefaultTemplateAggSettingsQueryModel request, int? page = null, int? size = null, Expression<Func<DefaultTemplateAggSettings, T>> selector = null) {
			return await _defaultTemplateAggSettingsRepository.SelectAllAsync(
                filter: DefaultTemplateAggSettingsFilters.GetFilters(request, isOrSpecification: request.IsOrSpecification),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<DefaultTemplateAggSettings>(),
                selector: selector);
		}
		public async Task<T> Select<T>(DefaultTemplateAggSettingsQueryModel request, Expression<Func<DefaultTemplateAggSettings, T>> selector = null)
        {
            return (await _defaultTemplateAggSettingsRepository.FindAsync(filter: DefaultTemplateAggSettingsFilters.GetFilters(request, isOrSpecification: true), selector: selector));
        }
        public async Task<IEnumerable<DefaultTemplateAggSettingsDTO>> GetAll(DefaultTemplateAggSettingsQueryModel request, int? page = null, int? size = null) {
            return await _defaultTemplateAggSettingsRepository.FindAllAsync(
                filter: DefaultTemplateAggSettingsFilters.GetFilters(request, isOrSpecification: true),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<DefaultTemplateAggSettings>(),
                selector: x => x.ProjectedAs<DefaultTemplateAggSettingsDTO>());
        }
		public async Task<IEnumerable<DefaultTemplateAggSettingsListiningDTO>> GetAllSummary(DefaultTemplateAggSettingsQueryModel request, int? page = null, int? size = null)
        {
            return await _defaultTemplateAggSettingsRepository.FindAllAsync(
                filter: DefaultTemplateAggSettingsFilters.GetFilters(request, isOrSpecification: true),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<DefaultTemplateAggSettings>(),
                selector: x => x.ProjectedAs<DefaultTemplateAggSettingsListiningDTO>());
        }

		public Task<DomainResponse> Create(DefaultTemplateAggSettingsDTO request, bool updateIfExists = true, DefaultTemplateAggSettingsQueryModel searchQuery = null) { return _mediator.Send(new CreateDefaultTemplateAggSettingsCommand(_logRequestContext, request)); }
		public async Task<int> CountAsync(DefaultTemplateAggSettingsQueryModel request) { return await _defaultTemplateAggSettingsRepository.CountAsync(filter: DefaultTemplateAggSettingsFilters.GetFilters(request, isOrSpecification: true)); }
		public Task Update(DefaultTemplateAggSettingsQueryModel searchQuery, DefaultTemplateAggSettingsDTO request, bool createIfNotExists = true) { return _mediator.Send(new UpdateDefaultTemplateAggSettingsCommand(_logRequestContext, searchQuery, request)); }
		public Task<DomainResponse> DeleteRange(DefaultTemplateAggSettingsQueryModel request){ return _mediator.Send(new DeleteRangeDefaultTemplateAggSettingsCommand(_logRequestContext, request)); }
		public Task<DomainResponse> Delete(DefaultTemplateAggSettingsQueryModel request){ return _mediator.Send(new DeleteDefaultTemplateAggSettingsCommand(_logRequestContext, request)); }
	}
}
