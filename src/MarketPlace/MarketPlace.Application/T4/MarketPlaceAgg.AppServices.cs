
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
	public partial class ProdutoAppService : BaseAppService, IProdutoAppService {	
		public IProdutoRepository _produtoRepository;
		public ProdutoAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, IProdutoRepository produtoRepository) : base(ctx, mediator) { _produtoRepository = produtoRepository; }	
		public async Task<ProdutoDTO> Get(IQueryModel<Produto> request) {
            return (await _produtoRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<ProdutoDTO>()));
        }
		public void Dispose()
        {
			_produtoRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<Produto> request, int? page = null, int? size = null, Expression<Func<Produto, T>> selector = null) {
			return await _produtoRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<Produto>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<Produto> request, Expression<Func<Produto, T>> selector = null)
        {
            return (await _produtoRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<ProdutoDTO>> GetAll(IQueryModel<Produto> request, int? page = null, int? size = null) {
            return await _produtoRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<Produto>(),
                selector: x => x.ProjectedAs<ProdutoDTO>());
        }
		public async Task<IEnumerable<ProdutoListiningDTO>> GetAllSummary(IQueryModel<Produto> request, int? page = null, int? size = null)
        {
            return await _produtoRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<Produto>(),
                selector: x => x.ProjectedAs<ProdutoListiningDTO>());
        }

		public Task<DomainResponse> Create(ProdutoDTO request, bool updateIfExists = true, IQueryModel<Produto> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<Produto> request) { return await _produtoRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<Produto> searchQuery, ProdutoDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<Produto> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<Produto> request){ return _mediator.Send(request.Command); }
	}
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
	public partial class CarrinhoAppService : BaseAppService, ICarrinhoAppService {	
		public ICarrinhoRepository _carrinhoRepository;
		public CarrinhoAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, ICarrinhoRepository carrinhoRepository) : base(ctx, mediator) { _carrinhoRepository = carrinhoRepository; }	
		public async Task<CarrinhoDTO> Get(IQueryModel<Carrinho> request) {
            return (await _carrinhoRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<CarrinhoDTO>()));
        }
		public void Dispose()
        {
			_carrinhoRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<Carrinho> request, int? page = null, int? size = null, Expression<Func<Carrinho, T>> selector = null) {
			return await _carrinhoRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<Carrinho>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<Carrinho> request, Expression<Func<Carrinho, T>> selector = null)
        {
            return (await _carrinhoRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<CarrinhoDTO>> GetAll(IQueryModel<Carrinho> request, int? page = null, int? size = null) {
            return await _carrinhoRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<Carrinho>(),
                selector: x => x.ProjectedAs<CarrinhoDTO>());
        }
		public async Task<IEnumerable<CarrinhoListiningDTO>> GetAllSummary(IQueryModel<Carrinho> request, int? page = null, int? size = null)
        {
            return await _carrinhoRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<Carrinho>(),
                selector: x => x.ProjectedAs<CarrinhoListiningDTO>());
        }

		public Task<DomainResponse> Create(CarrinhoDTO request, bool updateIfExists = true, IQueryModel<Carrinho> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<Carrinho> request) { return await _carrinhoRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<Carrinho> searchQuery, CarrinhoDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<Carrinho> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<Carrinho> request){ return _mediator.Send(request.Command); }
	}
	public partial class CategoriaprodutoAppService : BaseAppService, ICategoriaprodutoAppService {	
		public ICategoriaprodutoRepository _categoriaprodutoRepository;
		public CategoriaprodutoAppService(IMediator mediator, LazyCrud.CrossCutting.Infra.Log.Contexts.ILogRequestContext ctx, ICategoriaprodutoRepository categoriaprodutoRepository) : base(ctx, mediator) { _categoriaprodutoRepository = categoriaprodutoRepository; }	
		public async Task<CategoriaprodutoDTO> Get(IQueryModel<Categoriaproduto> request) {
            return (await _categoriaprodutoRepository.FindAsync(filter: request.GetFilter(), selector: x => x.ProjectedAs<CategoriaprodutoDTO>()));
        }
		public void Dispose()
        {
			_categoriaprodutoRepository = null;
        }
		public async Task<IEnumerable<T>> GetAll<T>(IQueryModel<Categoriaproduto> request, int? page = null, int? size = null, Expression<Func<Categoriaproduto, T>> selector = null) {
			return await _categoriaprodutoRepository.SelectAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<Categoriaproduto>(),
                selector: selector);
		}
		public async Task<T> Select<T>(IQueryModel<Categoriaproduto> request, Expression<Func<Categoriaproduto, T>> selector = null)
        {
            return (await _categoriaprodutoRepository.FindAsync(filter: request.GetFilter(), selector: selector));
        }
        public async Task<IEnumerable<CategoriaprodutoDTO>> GetAll(IQueryModel<Categoriaproduto> request, int? page = null, int? size = null) {
            return await _categoriaprodutoRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
				ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<Categoriaproduto>(),
                selector: x => x.ProjectedAs<CategoriaprodutoDTO>());
        }
		public async Task<IEnumerable<CategoriaprodutoListiningDTO>> GetAllSummary(IQueryModel<Categoriaproduto> request, int? page = null, int? size = null)
        {
            return await _categoriaprodutoRepository.FindAllAsync(
                filter: request.GetFilter(),
                take: size,
                skip: page * size,
                ascending: request.OrderByDesc != true,
                orderBy: request.OrderBy.GetPropertyListSelector<Categoriaproduto>(),
                selector: x => x.ProjectedAs<CategoriaprodutoListiningDTO>());
        }

		public Task<DomainResponse> Create(CategoriaprodutoDTO request, bool updateIfExists = true, IQueryModel<Categoriaproduto> searchQuery = null) { return _mediator.Send(request.Command); }
		public async Task<int> CountAsync(IQueryModel<Categoriaproduto> request) { return await _categoriaprodutoRepository.CountAsync(filter: request.GetFilter()); }
		public Task Update(IQueryModel<Categoriaproduto> searchQuery, CategoriaprodutoDTO request, bool createIfNotExists = true) { return _mediator.Send(request.Command); }
		public Task<DomainResponse> DeleteRange(IQueryModel<Categoriaproduto> request){ return _mediator.Send(request.Command); }
		public Task<DomainResponse> Delete(IQueryModel<Categoriaproduto> request){ return _mediator.Send(request.Command); }
	}
}
