
using MediatR;
using System.Linq.Expressions;
using FluentValidation.Results;
using LazyCrud.CrossCuting.Infra.Utils.Extensions;
using LazyCrud.Core.Application.DTO.Extensions;
using LazyCrud.Core.Application.Aggregates.Common;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Queries;
using LazyCrud.Core.Domain.CrossCutting;

namespace LazyCrud.MarketPlace.Application.Aggregates.MarketPlaceAgg.AppServices {
	using Application.DTO.Aggregates.MarketPlaceAgg.Requests;
	using Domain.Aggregates.MarketPlaceAgg.Queries.Models;
	using Domain.Aggregates.MarketPlaceAgg.Repositories;
	using Domain.Aggregates.MarketPlaceAgg.Filters;
	using Domain.Aggregates.MarketPlaceAgg.Entities;
	public partial class MarketPlaceAggSettingsAppService : BaseAppService, IMarketPlaceAggSettingsAppService {	
		public IMarketPlaceAggSettingsRepository _marketPlaceAggSettingsRepository;
		public MarketPlaceAggSettingsAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, IMarketPlaceAggSettingsRepository marketPlaceAggSettingsRepository) : base(ctx, mediator) { _marketPlaceAggSettingsRepository = marketPlaceAggSettingsRepository; }	
		public async Task<MarketPlaceAggSettingsDTO> Get(IQueryModel<MarketPlaceAggSettings> request) {
            return (await _marketPlaceAggSettingsRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<MarketPlaceAggSettingsDTO>()));
        }
		public void Dispose()
        {
			_marketPlaceAggSettingsRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<MarketPlaceAggSettings> request, int? page = null, int? size = null, Expression<Func<MarketPlaceAggSettings, T>> selector = null) {
			return await _marketPlaceAggSettingsRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<MarketPlaceAggSettings>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<MarketPlaceAggSettings> request, Expression<Func<MarketPlaceAggSettings, T>> selector = null)
        {
            return (await _marketPlaceAggSettingsRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<MarketPlaceAggSettingsDTO>> GetAll(IQueryModel<MarketPlaceAggSettings> request, int? page = null, int? size = null) {
            return await _marketPlaceAggSettingsRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<MarketPlaceAggSettings>(),
                selector: x => x.ProjectedAs<MarketPlaceAggSettingsDTO>());
        }
		public async Task<IEnumerable<MarketPlaceAggSettingsListiningDTO>> GetAllSummary(IQueryModel<MarketPlaceAggSettings> request, int? page = null, int? size = null)
        {
            return await _marketPlaceAggSettingsRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<MarketPlaceAggSettings>(),
                selector: x => x.ProjectedAs<MarketPlaceAggSettingsListiningDTO>());
        }

		public Task<DomainResponse> Create(MarketPlaceAggSettingsDTO request, bool updateIfExists = true, IQueryModel<MarketPlaceAggSettings> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<MarketPlaceAggSettings> request) { return await _marketPlaceAggSettingsRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<MarketPlaceAggSettings> searchQuery, MarketPlaceAggSettingsDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<MarketPlaceAggSettings> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<MarketPlaceAggSettings> request){ return _mediator.Send(request.Command); }
	}
}
