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
	public partial interface IProdutoAppService : IBaseAppService {	
		public Task<ProdutoDTO> Get(IQueryModel<Produto> request);
		public Task<int> CountAsync(IQueryModel<Produto> request);
		public Task<IEnumerable<ProdutoDTO>> GetAll(IQueryModel<Produto> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<Produto> request, Expression<Func<Domain.Aggregates.MarketPlaceAgg.Entities.Produto, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<Produto> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.MarketPlaceAgg.Entities.Produto, T>> selector = null);
		public Task<IEnumerable<ProdutoListiningDTO>> GetAllSummary(IQueryModel<Produto> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(ProdutoDTO request, bool updateIfExists = true, IQueryModel<Produto> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<Produto> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<Produto> request);
		public Task Update(IQueryModel<Produto> searchQuery, ProdutoDTO request, bool createIfNotExists = true);
	}
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
	public partial interface ICarrinhoAppService : IBaseAppService {	
		public Task<CarrinhoDTO> Get(IQueryModel<Carrinho> request);
		public Task<int> CountAsync(IQueryModel<Carrinho> request);
		public Task<IEnumerable<CarrinhoDTO>> GetAll(IQueryModel<Carrinho> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<Carrinho> request, Expression<Func<Domain.Aggregates.MarketPlaceAgg.Entities.Carrinho, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<Carrinho> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.MarketPlaceAgg.Entities.Carrinho, T>> selector = null);
		public Task<IEnumerable<CarrinhoListiningDTO>> GetAllSummary(IQueryModel<Carrinho> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(CarrinhoDTO request, bool updateIfExists = true, IQueryModel<Carrinho> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<Carrinho> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<Carrinho> request);
		public Task Update(IQueryModel<Carrinho> searchQuery, CarrinhoDTO request, bool createIfNotExists = true);
	}
	public partial interface ICategoriaprodutoAppService : IBaseAppService {	
		public Task<CategoriaprodutoDTO> Get(IQueryModel<Categoriaproduto> request);
		public Task<int> CountAsync(IQueryModel<Categoriaproduto> request);
		public Task<IEnumerable<CategoriaprodutoDTO>> GetAll(IQueryModel<Categoriaproduto> request, int? page = null, int? size = null);
		public Task<T> Select<T>(IQueryModel<Categoriaproduto> request, Expression<Func<Domain.Aggregates.MarketPlaceAgg.Entities.Categoriaproduto, T>> selector = null);
		public Task<IEnumerable<T>> GetAll<T>(IQueryModel<Categoriaproduto> request, int? page = null, int? size = null, Expression<Func<Domain.Aggregates.MarketPlaceAgg.Entities.Categoriaproduto, T>> selector = null);
		public Task<IEnumerable<CategoriaprodutoListiningDTO>> GetAllSummary(IQueryModel<Categoriaproduto> request, int? page = null, int? size = null);

		public Task<DomainResponse> Create(CategoriaprodutoDTO request, bool updateIfExists = true, IQueryModel<Categoriaproduto> searchQuery = null);
		public Task<DomainResponse> Delete(IQueryModel<Categoriaproduto> request);
		public Task<DomainResponse> DeleteRange(IQueryModel<Categoriaproduto> request);
		public Task Update(IQueryModel<Categoriaproduto> searchQuery, CategoriaprodutoDTO request, bool createIfNotExists = true);
	}
}
