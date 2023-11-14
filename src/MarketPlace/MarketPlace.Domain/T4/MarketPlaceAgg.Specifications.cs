using LazyCrud.Core.Domain.Seedwork.Specification;
using Microsoft.EntityFrameworkCore;
namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Specifications {
	using Entities;
   public partial class ProdutoSpecifications {
				public static Specification<Produto> NomeContains(string value) {
			return new DirectSpecification<Produto>(p => EF.Functions.Like(p.Nome.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<Produto> NomeStartsWith(string value) {
			return new DirectSpecification<Produto>(p => EF.Functions.Like(p.Nome.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<Produto> NomeEqual(params string[] values) {
			return new DirectSpecification<Produto>(p => values.Contains(p.Nome));
		}
		public static Specification<Produto> NomeNotEqual(string value) {
			return new DirectSpecification<Produto>(p => p.Nome != value);
		}
		public static Specification<Produto> NomeIsNull() {
            return new DirectSpecification<Produto>(p => p.Nome == null);
        }
		public static Specification<Produto> NomeIsNotNull() {
            return new DirectSpecification<Produto>(p => p.Nome != null);
        }
		
					public static Specification<Produto> EstoqueContains(params int[] values) {
            return new DirectSpecification<Produto>(p => values.Contains(p.Estoque));
        }
		public static Specification<Produto> EstoqueNotContains(params int[] values) {
            return new DirectSpecification<Produto>(p => !values.Contains(p.Estoque));
        }
		public static Specification<Produto> EstoqueEqual(params int[] values) {
			return new DirectSpecification<Produto>(p => values.Contains(p.Estoque));
        }
        public static Specification<Produto> EstoqueGreaterThanOrEqual(int value) {
            return new DirectSpecification<Produto>(p => p.Estoque >= value);
        }
        public static Specification<Produto> EstoqueLessThanOrEqual(int value) {
            return new DirectSpecification<Produto>(p => p.Estoque <= value);
        }
        public static Specification<Produto> EstoqueNotEqual(int value) {
            return new DirectSpecification<Produto>(p => p.Estoque != value);
        }
        public static Specification<Produto> EstoqueGreaterThan(int value) {
            return new DirectSpecification<Produto>(p => p.Estoque > value);
        }
        public static Specification<Produto> EstoqueLessThan(int value) {
            return new DirectSpecification<Produto>(p => p.Estoque < value);
        }
		
					public static Specification<Produto> PrecoContains(params int[] values) {
            return new DirectSpecification<Produto>(p => values.Contains(p.Preco));
        }
		public static Specification<Produto> PrecoNotContains(params int[] values) {
            return new DirectSpecification<Produto>(p => !values.Contains(p.Preco));
        }
		public static Specification<Produto> PrecoEqual(params int[] values) {
			return new DirectSpecification<Produto>(p => values.Contains(p.Preco));
        }
        public static Specification<Produto> PrecoGreaterThanOrEqual(int value) {
            return new DirectSpecification<Produto>(p => p.Preco >= value);
        }
        public static Specification<Produto> PrecoLessThanOrEqual(int value) {
            return new DirectSpecification<Produto>(p => p.Preco <= value);
        }
        public static Specification<Produto> PrecoNotEqual(int value) {
            return new DirectSpecification<Produto>(p => p.Preco != value);
        }
        public static Specification<Produto> PrecoGreaterThan(int value) {
            return new DirectSpecification<Produto>(p => p.Preco > value);
        }
        public static Specification<Produto> PrecoLessThan(int value) {
            return new DirectSpecification<Produto>(p => p.Preco < value);
        }
		
					public static Specification<Produto> CategoriaIdContains(params int[] values) {
            return new DirectSpecification<Produto>(p => values.Contains(p.CategoriaId));
        }
		public static Specification<Produto> CategoriaIdNotContains(params int[] values) {
            return new DirectSpecification<Produto>(p => !values.Contains(p.CategoriaId));
        }
		public static Specification<Produto> CategoriaIdEqual(params int[] values) {
			return new DirectSpecification<Produto>(p => values.Contains(p.CategoriaId));
        }
        public static Specification<Produto> CategoriaIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<Produto>(p => p.CategoriaId >= value);
        }
        public static Specification<Produto> CategoriaIdLessThanOrEqual(int value) {
            return new DirectSpecification<Produto>(p => p.CategoriaId <= value);
        }
        public static Specification<Produto> CategoriaIdNotEqual(int value) {
            return new DirectSpecification<Produto>(p => p.CategoriaId != value);
        }
        public static Specification<Produto> CategoriaIdGreaterThan(int value) {
            return new DirectSpecification<Produto>(p => p.CategoriaId > value);
        }
        public static Specification<Produto> CategoriaIdLessThan(int value) {
            return new DirectSpecification<Produto>(p => p.CategoriaId < value);
        }
		
					public static Specification<Produto> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<Produto>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<Produto> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<Produto>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<Produto> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<Produto>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<Produto> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.CreatedAt >= value);
        }
        public static Specification<Produto> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.CreatedAt <= value);
        }
        public static Specification<Produto> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.CreatedAt != value);
        }
        public static Specification<Produto> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.CreatedAt > value);
        }
        public static Specification<Produto> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.CreatedAt < value);
        }
		
					public static Specification<Produto> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<Produto>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<Produto> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<Produto>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<Produto> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<Produto>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<Produto> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.UpdatedAt >= value);
        }
        public static Specification<Produto> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.UpdatedAt <= value);
        }
        public static Specification<Produto> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.UpdatedAt != value);
        }
        public static Specification<Produto> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.UpdatedAt > value);
        }
        public static Specification<Produto> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.UpdatedAt < value);
        }
		
					public static Specification<Produto> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<Produto>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<Produto> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<Produto>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<Produto> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<Produto>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<Produto> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.DeletedAt >= value);
        }
        public static Specification<Produto> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.DeletedAt <= value);
        }
        public static Specification<Produto> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.DeletedAt != value);
        }
        public static Specification<Produto> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.DeletedAt > value);
        }
        public static Specification<Produto> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<Produto>(p => p.DeletedAt < value);
        }
		
					public static Specification<Produto> IdContains(params int[] values) {
            return new DirectSpecification<Produto>(p => values.Contains(p.Id));
        }
		public static Specification<Produto> IdNotContains(params int[] values) {
            return new DirectSpecification<Produto>(p => !values.Contains(p.Id));
        }
		public static Specification<Produto> IdEqual(params int[] values) {
			return new DirectSpecification<Produto>(p => values.Contains(p.Id));
        }
        public static Specification<Produto> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<Produto>(p => p.Id >= value);
        }
        public static Specification<Produto> IdLessThanOrEqual(int value) {
            return new DirectSpecification<Produto>(p => p.Id <= value);
        }
        public static Specification<Produto> IdNotEqual(int value) {
            return new DirectSpecification<Produto>(p => p.Id != value);
        }
        public static Specification<Produto> IdGreaterThan(int value) {
            return new DirectSpecification<Produto>(p => p.Id > value);
        }
        public static Specification<Produto> IdLessThan(int value) {
            return new DirectSpecification<Produto>(p => p.Id < value);
        }
		
					public static Specification<Produto> ExternalIdContains(string value) {
			return new DirectSpecification<Produto>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<Produto> ExternalIdStartsWith(string value) {
			return new DirectSpecification<Produto>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<Produto> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<Produto>(p => values.Contains(p.ExternalId));
		}
		public static Specification<Produto> ExternalIdNotEqual(string value) {
			return new DirectSpecification<Produto>(p => p.ExternalId != value);
		}
		public static Specification<Produto> ExternalIdIsNull() {
            return new DirectSpecification<Produto>(p => p.ExternalId == null);
        }
		public static Specification<Produto> ExternalIdIsNotNull() {
            return new DirectSpecification<Produto>(p => p.ExternalId != null);
        }
		
					public static Specification<Produto> IsDeletedEqual(bool value) {
			return new DirectSpecification<Produto>(p => p.IsDeleted == value);
		}
		
	   }
   public partial class MarketPlaceAggSettingsSpecifications {
				public static Specification<MarketPlaceAggSettings> UserIdContains(params int[] values) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => values.Contains(p.UserId));
        }
		public static Specification<MarketPlaceAggSettings> UserIdNotContains(params int[] values) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => !values.Contains(p.UserId));
        }
		public static Specification<MarketPlaceAggSettings> UserIdEqual(params int[] values) {
			return new DirectSpecification<MarketPlaceAggSettings>(p => values.Contains(p.UserId));
        }
        public static Specification<MarketPlaceAggSettings> UserIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.UserId >= value);
        }
        public static Specification<MarketPlaceAggSettings> UserIdLessThanOrEqual(int value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.UserId <= value);
        }
        public static Specification<MarketPlaceAggSettings> UserIdNotEqual(int value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.UserId != value);
        }
        public static Specification<MarketPlaceAggSettings> UserIdGreaterThan(int value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.UserId > value);
        }
        public static Specification<MarketPlaceAggSettings> UserIdLessThan(int value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.UserId < value);
        }
		
					public static Specification<MarketPlaceAggSettings> AutoSaveSettingsEnabledEqual(bool value) {
			return new DirectSpecification<MarketPlaceAggSettings>(p => p.AutoSaveSettingsEnabled == value);
		}
		
					public static Specification<MarketPlaceAggSettings> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<MarketPlaceAggSettings> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<MarketPlaceAggSettings> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<MarketPlaceAggSettings>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<MarketPlaceAggSettings> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.CreatedAt >= value);
        }
        public static Specification<MarketPlaceAggSettings> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.CreatedAt <= value);
        }
        public static Specification<MarketPlaceAggSettings> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.CreatedAt != value);
        }
        public static Specification<MarketPlaceAggSettings> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.CreatedAt > value);
        }
        public static Specification<MarketPlaceAggSettings> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.CreatedAt < value);
        }
		
					public static Specification<MarketPlaceAggSettings> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<MarketPlaceAggSettings> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<MarketPlaceAggSettings> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<MarketPlaceAggSettings>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<MarketPlaceAggSettings> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.UpdatedAt >= value);
        }
        public static Specification<MarketPlaceAggSettings> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.UpdatedAt <= value);
        }
        public static Specification<MarketPlaceAggSettings> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.UpdatedAt != value);
        }
        public static Specification<MarketPlaceAggSettings> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.UpdatedAt > value);
        }
        public static Specification<MarketPlaceAggSettings> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.UpdatedAt < value);
        }
		
					public static Specification<MarketPlaceAggSettings> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<MarketPlaceAggSettings> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<MarketPlaceAggSettings> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<MarketPlaceAggSettings>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<MarketPlaceAggSettings> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.DeletedAt >= value);
        }
        public static Specification<MarketPlaceAggSettings> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.DeletedAt <= value);
        }
        public static Specification<MarketPlaceAggSettings> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.DeletedAt != value);
        }
        public static Specification<MarketPlaceAggSettings> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.DeletedAt > value);
        }
        public static Specification<MarketPlaceAggSettings> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.DeletedAt < value);
        }
		
					public static Specification<MarketPlaceAggSettings> IdContains(params int[] values) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => values.Contains(p.Id));
        }
		public static Specification<MarketPlaceAggSettings> IdNotContains(params int[] values) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => !values.Contains(p.Id));
        }
		public static Specification<MarketPlaceAggSettings> IdEqual(params int[] values) {
			return new DirectSpecification<MarketPlaceAggSettings>(p => values.Contains(p.Id));
        }
        public static Specification<MarketPlaceAggSettings> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.Id >= value);
        }
        public static Specification<MarketPlaceAggSettings> IdLessThanOrEqual(int value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.Id <= value);
        }
        public static Specification<MarketPlaceAggSettings> IdNotEqual(int value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.Id != value);
        }
        public static Specification<MarketPlaceAggSettings> IdGreaterThan(int value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.Id > value);
        }
        public static Specification<MarketPlaceAggSettings> IdLessThan(int value) {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.Id < value);
        }
		
					public static Specification<MarketPlaceAggSettings> ExternalIdContains(string value) {
			return new DirectSpecification<MarketPlaceAggSettings>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<MarketPlaceAggSettings> ExternalIdStartsWith(string value) {
			return new DirectSpecification<MarketPlaceAggSettings>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<MarketPlaceAggSettings> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<MarketPlaceAggSettings>(p => values.Contains(p.ExternalId));
		}
		public static Specification<MarketPlaceAggSettings> ExternalIdNotEqual(string value) {
			return new DirectSpecification<MarketPlaceAggSettings>(p => p.ExternalId != value);
		}
		public static Specification<MarketPlaceAggSettings> ExternalIdIsNull() {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.ExternalId == null);
        }
		public static Specification<MarketPlaceAggSettings> ExternalIdIsNotNull() {
            return new DirectSpecification<MarketPlaceAggSettings>(p => p.ExternalId != null);
        }
		
					public static Specification<MarketPlaceAggSettings> IsDeletedEqual(bool value) {
			return new DirectSpecification<MarketPlaceAggSettings>(p => p.IsDeleted == value);
		}
		
	   }
   public partial class CarrinhoSpecifications {
				public static Specification<Carrinho> ProdutoIdContains(params int[] values) {
            return new DirectSpecification<Carrinho>(p => values.Contains(p.ProdutoId));
        }
		public static Specification<Carrinho> ProdutoIdNotContains(params int[] values) {
            return new DirectSpecification<Carrinho>(p => !values.Contains(p.ProdutoId));
        }
		public static Specification<Carrinho> ProdutoIdEqual(params int[] values) {
			return new DirectSpecification<Carrinho>(p => values.Contains(p.ProdutoId));
        }
        public static Specification<Carrinho> ProdutoIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<Carrinho>(p => p.ProdutoId >= value);
        }
        public static Specification<Carrinho> ProdutoIdLessThanOrEqual(int value) {
            return new DirectSpecification<Carrinho>(p => p.ProdutoId <= value);
        }
        public static Specification<Carrinho> ProdutoIdNotEqual(int value) {
            return new DirectSpecification<Carrinho>(p => p.ProdutoId != value);
        }
        public static Specification<Carrinho> ProdutoIdGreaterThan(int value) {
            return new DirectSpecification<Carrinho>(p => p.ProdutoId > value);
        }
        public static Specification<Carrinho> ProdutoIdLessThan(int value) {
            return new DirectSpecification<Carrinho>(p => p.ProdutoId < value);
        }
		
					public static Specification<Carrinho> UsuarioIdContains(params int[] values) {
            return new DirectSpecification<Carrinho>(p => values.Contains(p.UsuarioId));
        }
		public static Specification<Carrinho> UsuarioIdNotContains(params int[] values) {
            return new DirectSpecification<Carrinho>(p => !values.Contains(p.UsuarioId));
        }
		public static Specification<Carrinho> UsuarioIdEqual(params int[] values) {
			return new DirectSpecification<Carrinho>(p => values.Contains(p.UsuarioId));
        }
        public static Specification<Carrinho> UsuarioIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<Carrinho>(p => p.UsuarioId >= value);
        }
        public static Specification<Carrinho> UsuarioIdLessThanOrEqual(int value) {
            return new DirectSpecification<Carrinho>(p => p.UsuarioId <= value);
        }
        public static Specification<Carrinho> UsuarioIdNotEqual(int value) {
            return new DirectSpecification<Carrinho>(p => p.UsuarioId != value);
        }
        public static Specification<Carrinho> UsuarioIdGreaterThan(int value) {
            return new DirectSpecification<Carrinho>(p => p.UsuarioId > value);
        }
        public static Specification<Carrinho> UsuarioIdLessThan(int value) {
            return new DirectSpecification<Carrinho>(p => p.UsuarioId < value);
        }
		
					public static Specification<Carrinho> PagamentoContains(params int[] values) {
            return new DirectSpecification<Carrinho>(p => values.Contains(p.Pagamento));
        }
		public static Specification<Carrinho> PagamentoNotContains(params int[] values) {
            return new DirectSpecification<Carrinho>(p => !values.Contains(p.Pagamento));
        }
		public static Specification<Carrinho> PagamentoEqual(params int[] values) {
			return new DirectSpecification<Carrinho>(p => values.Contains(p.Pagamento));
        }
        public static Specification<Carrinho> PagamentoGreaterThanOrEqual(int value) {
            return new DirectSpecification<Carrinho>(p => p.Pagamento >= value);
        }
        public static Specification<Carrinho> PagamentoLessThanOrEqual(int value) {
            return new DirectSpecification<Carrinho>(p => p.Pagamento <= value);
        }
        public static Specification<Carrinho> PagamentoNotEqual(int value) {
            return new DirectSpecification<Carrinho>(p => p.Pagamento != value);
        }
        public static Specification<Carrinho> PagamentoGreaterThan(int value) {
            return new DirectSpecification<Carrinho>(p => p.Pagamento > value);
        }
        public static Specification<Carrinho> PagamentoLessThan(int value) {
            return new DirectSpecification<Carrinho>(p => p.Pagamento < value);
        }
		
					public static Specification<Carrinho> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<Carrinho>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<Carrinho> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<Carrinho>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<Carrinho> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<Carrinho>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<Carrinho> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.CreatedAt >= value);
        }
        public static Specification<Carrinho> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.CreatedAt <= value);
        }
        public static Specification<Carrinho> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.CreatedAt != value);
        }
        public static Specification<Carrinho> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.CreatedAt > value);
        }
        public static Specification<Carrinho> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.CreatedAt < value);
        }
		
					public static Specification<Carrinho> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<Carrinho>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<Carrinho> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<Carrinho>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<Carrinho> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<Carrinho>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<Carrinho> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.UpdatedAt >= value);
        }
        public static Specification<Carrinho> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.UpdatedAt <= value);
        }
        public static Specification<Carrinho> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.UpdatedAt != value);
        }
        public static Specification<Carrinho> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.UpdatedAt > value);
        }
        public static Specification<Carrinho> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.UpdatedAt < value);
        }
		
					public static Specification<Carrinho> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<Carrinho>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<Carrinho> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<Carrinho>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<Carrinho> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<Carrinho>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<Carrinho> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.DeletedAt >= value);
        }
        public static Specification<Carrinho> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.DeletedAt <= value);
        }
        public static Specification<Carrinho> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.DeletedAt != value);
        }
        public static Specification<Carrinho> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.DeletedAt > value);
        }
        public static Specification<Carrinho> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<Carrinho>(p => p.DeletedAt < value);
        }
		
					public static Specification<Carrinho> IdContains(params int[] values) {
            return new DirectSpecification<Carrinho>(p => values.Contains(p.Id));
        }
		public static Specification<Carrinho> IdNotContains(params int[] values) {
            return new DirectSpecification<Carrinho>(p => !values.Contains(p.Id));
        }
		public static Specification<Carrinho> IdEqual(params int[] values) {
			return new DirectSpecification<Carrinho>(p => values.Contains(p.Id));
        }
        public static Specification<Carrinho> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<Carrinho>(p => p.Id >= value);
        }
        public static Specification<Carrinho> IdLessThanOrEqual(int value) {
            return new DirectSpecification<Carrinho>(p => p.Id <= value);
        }
        public static Specification<Carrinho> IdNotEqual(int value) {
            return new DirectSpecification<Carrinho>(p => p.Id != value);
        }
        public static Specification<Carrinho> IdGreaterThan(int value) {
            return new DirectSpecification<Carrinho>(p => p.Id > value);
        }
        public static Specification<Carrinho> IdLessThan(int value) {
            return new DirectSpecification<Carrinho>(p => p.Id < value);
        }
		
					public static Specification<Carrinho> ExternalIdContains(string value) {
			return new DirectSpecification<Carrinho>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<Carrinho> ExternalIdStartsWith(string value) {
			return new DirectSpecification<Carrinho>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<Carrinho> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<Carrinho>(p => values.Contains(p.ExternalId));
		}
		public static Specification<Carrinho> ExternalIdNotEqual(string value) {
			return new DirectSpecification<Carrinho>(p => p.ExternalId != value);
		}
		public static Specification<Carrinho> ExternalIdIsNull() {
            return new DirectSpecification<Carrinho>(p => p.ExternalId == null);
        }
		public static Specification<Carrinho> ExternalIdIsNotNull() {
            return new DirectSpecification<Carrinho>(p => p.ExternalId != null);
        }
		
					public static Specification<Carrinho> IsDeletedEqual(bool value) {
			return new DirectSpecification<Carrinho>(p => p.IsDeleted == value);
		}
		
	   }
   public partial class CategoriaprodutoSpecifications {
				public static Specification<Categoriaproduto> NomeContains(string value) {
			return new DirectSpecification<Categoriaproduto>(p => EF.Functions.Like(p.Nome.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<Categoriaproduto> NomeStartsWith(string value) {
			return new DirectSpecification<Categoriaproduto>(p => EF.Functions.Like(p.Nome.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<Categoriaproduto> NomeEqual(params string[] values) {
			return new DirectSpecification<Categoriaproduto>(p => values.Contains(p.Nome));
		}
		public static Specification<Categoriaproduto> NomeNotEqual(string value) {
			return new DirectSpecification<Categoriaproduto>(p => p.Nome != value);
		}
		public static Specification<Categoriaproduto> NomeIsNull() {
            return new DirectSpecification<Categoriaproduto>(p => p.Nome == null);
        }
		public static Specification<Categoriaproduto> NomeIsNotNull() {
            return new DirectSpecification<Categoriaproduto>(p => p.Nome != null);
        }
		
					public static Specification<Categoriaproduto> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<Categoriaproduto>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<Categoriaproduto> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<Categoriaproduto>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<Categoriaproduto> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<Categoriaproduto>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<Categoriaproduto> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.CreatedAt >= value);
        }
        public static Specification<Categoriaproduto> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.CreatedAt <= value);
        }
        public static Specification<Categoriaproduto> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.CreatedAt != value);
        }
        public static Specification<Categoriaproduto> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.CreatedAt > value);
        }
        public static Specification<Categoriaproduto> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.CreatedAt < value);
        }
		
					public static Specification<Categoriaproduto> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<Categoriaproduto>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<Categoriaproduto> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<Categoriaproduto>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<Categoriaproduto> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<Categoriaproduto>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<Categoriaproduto> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.UpdatedAt >= value);
        }
        public static Specification<Categoriaproduto> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.UpdatedAt <= value);
        }
        public static Specification<Categoriaproduto> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.UpdatedAt != value);
        }
        public static Specification<Categoriaproduto> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.UpdatedAt > value);
        }
        public static Specification<Categoriaproduto> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.UpdatedAt < value);
        }
		
					public static Specification<Categoriaproduto> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<Categoriaproduto>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<Categoriaproduto> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<Categoriaproduto>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<Categoriaproduto> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<Categoriaproduto>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<Categoriaproduto> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.DeletedAt >= value);
        }
        public static Specification<Categoriaproduto> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.DeletedAt <= value);
        }
        public static Specification<Categoriaproduto> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.DeletedAt != value);
        }
        public static Specification<Categoriaproduto> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.DeletedAt > value);
        }
        public static Specification<Categoriaproduto> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<Categoriaproduto>(p => p.DeletedAt < value);
        }
		
					public static Specification<Categoriaproduto> IdContains(params int[] values) {
            return new DirectSpecification<Categoriaproduto>(p => values.Contains(p.Id));
        }
		public static Specification<Categoriaproduto> IdNotContains(params int[] values) {
            return new DirectSpecification<Categoriaproduto>(p => !values.Contains(p.Id));
        }
		public static Specification<Categoriaproduto> IdEqual(params int[] values) {
			return new DirectSpecification<Categoriaproduto>(p => values.Contains(p.Id));
        }
        public static Specification<Categoriaproduto> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<Categoriaproduto>(p => p.Id >= value);
        }
        public static Specification<Categoriaproduto> IdLessThanOrEqual(int value) {
            return new DirectSpecification<Categoriaproduto>(p => p.Id <= value);
        }
        public static Specification<Categoriaproduto> IdNotEqual(int value) {
            return new DirectSpecification<Categoriaproduto>(p => p.Id != value);
        }
        public static Specification<Categoriaproduto> IdGreaterThan(int value) {
            return new DirectSpecification<Categoriaproduto>(p => p.Id > value);
        }
        public static Specification<Categoriaproduto> IdLessThan(int value) {
            return new DirectSpecification<Categoriaproduto>(p => p.Id < value);
        }
		
					public static Specification<Categoriaproduto> ExternalIdContains(string value) {
			return new DirectSpecification<Categoriaproduto>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<Categoriaproduto> ExternalIdStartsWith(string value) {
			return new DirectSpecification<Categoriaproduto>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<Categoriaproduto> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<Categoriaproduto>(p => values.Contains(p.ExternalId));
		}
		public static Specification<Categoriaproduto> ExternalIdNotEqual(string value) {
			return new DirectSpecification<Categoriaproduto>(p => p.ExternalId != value);
		}
		public static Specification<Categoriaproduto> ExternalIdIsNull() {
            return new DirectSpecification<Categoriaproduto>(p => p.ExternalId == null);
        }
		public static Specification<Categoriaproduto> ExternalIdIsNotNull() {
            return new DirectSpecification<Categoriaproduto>(p => p.ExternalId != null);
        }
		
					public static Specification<Categoriaproduto> IsDeletedEqual(bool value) {
			return new DirectSpecification<Categoriaproduto>(p => p.IsDeleted == value);
		}
		
	   }
}
