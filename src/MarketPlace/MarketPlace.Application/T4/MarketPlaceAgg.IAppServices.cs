using System.Linq.Expressions;
using FluentValidation.Results;
using LazyCrud.Core.Domain.CrossCutting;
using LazyCrud.Core.Application.Aggregates.Common;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Queries;

using LazyCrud.Core.Domain.Seedwork.Specification;
namespace LazyCrud.MarketPlace.Application.Aggregates.MarketPlaceAgg.AppServices {
	using Domain.Aggregates.MarketPlaceAgg.Entities;
	using Domain.Aggregates.MarketPlaceAgg.Queries;
	using Application.DTO.Aggregates.MarketPlaceAgg.Requests;
	public partial interface IMarketPlaceAggSettingsAppService : IBaseAppService {	
		public Task<MarketPlaceAggSettingsDTO> Get(IQueryModel<MarketPlaceAggSettings> request);
		public Task<int> CountAsync(IQueryModel<MarketPlaceAggSettings> request);
		public Task<IEnumerable<MarketPlaceAggSettingsDTO>> GetAll(IQueryModel<MarketPlaceAggSettings> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<MarketPlaceAggSettings> request, Expression<Func<Domain.Aggregates.MarketPlaceAgg.Entities.MarketPlaceAggSettings, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<MarketPlaceAggSettings> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.MarketPlaceAgg.Entities.MarketPlaceAggSettings, T>> selector = null);
		public Task<IEnumerable<MarketPlaceAggSettingsListiningDTO>> GetAllSummary(IQueryModel<MarketPlaceAggSettings> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(MarketPlaceAggSettingsDTO request, bool updateIfExists = true, IQueryModel<MarketPlaceAggSettings> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<MarketPlaceAggSettings> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<MarketPlaceAggSettings> request);
		public Task Update(IQueryModel<MarketPlaceAggSettings> searchQuery, MarketPlaceAggSettingsDTO request, bool createIfNotExists = true);
	}
}
