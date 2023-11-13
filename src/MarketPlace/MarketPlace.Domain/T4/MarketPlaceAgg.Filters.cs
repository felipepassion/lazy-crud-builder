using LazyCrud.CrossCuting.Infra.Utils.Extensions;
using System.Linq.Expressions;
using LazyCrud.Core.Domain.Seedwork.Specification;
using LazyCrud.CrossCuting.Infra.Utils.Extensions;

namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Filters{
	using Entities;
	using Specifications;
	using Queries.Models;
	public static class ProdutoFilters 
	{
	    public static Expression<Func<Produto, bool>> GetFilters(this ProdutoQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<Produto> GetFiltersSpecification(this ProdutoQueryModel request, bool isOrSpecification = false) 
		{
			Specification<Produto> filter = new DirectSpecification<Produto>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (!string.IsNullOrWhiteSpace(request.NomeEqual)) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.NomeEqual(request.NomeEqual);
					else
						filter &= ProdutoSpecifications.NomeEqual(request.NomeEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.NomeNotEqual)) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.NomeNotEqual(request.NomeNotEqual);
					else
						filter &= ProdutoSpecifications.NomeNotEqual(request.NomeNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.NomeContains)) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.NomeContains(request.NomeContains);
					else
						filter &= ProdutoSpecifications.NomeContains(request.NomeContains);
				}
				if (!string.IsNullOrWhiteSpace(request.NomeStartsWith)) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.NomeStartsWith(request.NomeStartsWith);
					else
						filter &= ProdutoSpecifications.NomeStartsWith(request.NomeStartsWith);
				}
				if (request.EstoqueEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.EstoqueEqual(request.EstoqueEqual.Value);
					else
						filter &= ProdutoSpecifications.EstoqueEqual(request.EstoqueEqual.Value);
				}
				if (request.EstoqueNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.EstoqueNotEqual(request.EstoqueNotEqual.Value);
					else
						filter &= ProdutoSpecifications.EstoqueNotEqual(request.EstoqueNotEqual.Value);
				}
				if (request.EstoqueContains != null)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.EstoqueContains(request.EstoqueContains);
					else
						filter &= ProdutoSpecifications.EstoqueContains(request.EstoqueContains);
				}
				if (request.EstoqueNotContains != null)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.EstoqueNotContains(request.EstoqueNotContains);
					else
						filter &= ProdutoSpecifications.EstoqueNotContains(request.EstoqueNotContains);
				}
				if (request.EstoqueSince.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.EstoqueGreaterThanOrEqual(request.EstoqueSince.Value);
					else
						filter &= ProdutoSpecifications.EstoqueGreaterThanOrEqual(request.EstoqueSince.Value);
				}
				if (request.EstoqueUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.EstoqueLessThan(request.EstoqueUntil.Value);
					else
						filter &= ProdutoSpecifications.EstoqueLessThan(request.EstoqueUntil.Value);
				}
				if (request.EstoqueNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.EstoqueNotEqual(request.EstoqueNotEqual.Value);
					else
						filter &= ProdutoSpecifications.EstoqueNotEqual(request.EstoqueNotEqual.Value);
				}
				if (request.EstoqueLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.EstoqueLessThan(request.EstoqueLessThan.Value);
					else
						filter &= ProdutoSpecifications.EstoqueLessThan(request.EstoqueLessThan.Value);
				}
				if (request.EstoqueGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.EstoqueGreaterThan(request.EstoqueGreaterThan.Value);
					else
						filter &= ProdutoSpecifications.EstoqueGreaterThan(request.EstoqueGreaterThan.Value);
				}
				if (request.EstoqueLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.EstoqueLessThanOrEqual(request.EstoqueLessThanOrEqual.Value);
					else
						filter &= ProdutoSpecifications.EstoqueLessThanOrEqual(request.EstoqueLessThanOrEqual.Value);
				}
				if (request.EstoqueGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.EstoqueGreaterThanOrEqual(request.EstoqueGreaterThanOrEqual.Value);
					else
						filter &= ProdutoSpecifications.EstoqueGreaterThanOrEqual(request.EstoqueGreaterThanOrEqual.Value);
				}
				if (request.PrecoEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.PrecoEqual(request.PrecoEqual.Value);
					else
						filter &= ProdutoSpecifications.PrecoEqual(request.PrecoEqual.Value);
				}
				if (request.PrecoNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.PrecoNotEqual(request.PrecoNotEqual.Value);
					else
						filter &= ProdutoSpecifications.PrecoNotEqual(request.PrecoNotEqual.Value);
				}
				if (request.PrecoContains != null)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.PrecoContains(request.PrecoContains);
					else
						filter &= ProdutoSpecifications.PrecoContains(request.PrecoContains);
				}
				if (request.PrecoNotContains != null)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.PrecoNotContains(request.PrecoNotContains);
					else
						filter &= ProdutoSpecifications.PrecoNotContains(request.PrecoNotContains);
				}
				if (request.PrecoSince.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.PrecoGreaterThanOrEqual(request.PrecoSince.Value);
					else
						filter &= ProdutoSpecifications.PrecoGreaterThanOrEqual(request.PrecoSince.Value);
				}
				if (request.PrecoUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.PrecoLessThan(request.PrecoUntil.Value);
					else
						filter &= ProdutoSpecifications.PrecoLessThan(request.PrecoUntil.Value);
				}
				if (request.PrecoNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.PrecoNotEqual(request.PrecoNotEqual.Value);
					else
						filter &= ProdutoSpecifications.PrecoNotEqual(request.PrecoNotEqual.Value);
				}
				if (request.PrecoLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.PrecoLessThan(request.PrecoLessThan.Value);
					else
						filter &= ProdutoSpecifications.PrecoLessThan(request.PrecoLessThan.Value);
				}
				if (request.PrecoGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.PrecoGreaterThan(request.PrecoGreaterThan.Value);
					else
						filter &= ProdutoSpecifications.PrecoGreaterThan(request.PrecoGreaterThan.Value);
				}
				if (request.PrecoLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.PrecoLessThanOrEqual(request.PrecoLessThanOrEqual.Value);
					else
						filter &= ProdutoSpecifications.PrecoLessThanOrEqual(request.PrecoLessThanOrEqual.Value);
				}
				if (request.PrecoGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.PrecoGreaterThanOrEqual(request.PrecoGreaterThanOrEqual.Value);
					else
						filter &= ProdutoSpecifications.PrecoGreaterThanOrEqual(request.PrecoGreaterThanOrEqual.Value);
				}
				if (request.CategoriaIdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CategoriaIdEqual(request.CategoriaIdEqual.Value);
					else
						filter &= ProdutoSpecifications.CategoriaIdEqual(request.CategoriaIdEqual.Value);
				}
				if (request.CategoriaIdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CategoriaIdNotEqual(request.CategoriaIdNotEqual.Value);
					else
						filter &= ProdutoSpecifications.CategoriaIdNotEqual(request.CategoriaIdNotEqual.Value);
				}
				if (request.CategoriaIdContains != null)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CategoriaIdContains(request.CategoriaIdContains);
					else
						filter &= ProdutoSpecifications.CategoriaIdContains(request.CategoriaIdContains);
				}
				if (request.CategoriaIdNotContains != null)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CategoriaIdNotContains(request.CategoriaIdNotContains);
					else
						filter &= ProdutoSpecifications.CategoriaIdNotContains(request.CategoriaIdNotContains);
				}
				if (request.CategoriaIdSince.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CategoriaIdGreaterThanOrEqual(request.CategoriaIdSince.Value);
					else
						filter &= ProdutoSpecifications.CategoriaIdGreaterThanOrEqual(request.CategoriaIdSince.Value);
				}
				if (request.CategoriaIdUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CategoriaIdLessThan(request.CategoriaIdUntil.Value);
					else
						filter &= ProdutoSpecifications.CategoriaIdLessThan(request.CategoriaIdUntil.Value);
				}
				if (request.CategoriaIdNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CategoriaIdNotEqual(request.CategoriaIdNotEqual.Value);
					else
						filter &= ProdutoSpecifications.CategoriaIdNotEqual(request.CategoriaIdNotEqual.Value);
				}
				if (request.CategoriaIdLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CategoriaIdLessThan(request.CategoriaIdLessThan.Value);
					else
						filter &= ProdutoSpecifications.CategoriaIdLessThan(request.CategoriaIdLessThan.Value);
				}
				if (request.CategoriaIdGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CategoriaIdGreaterThan(request.CategoriaIdGreaterThan.Value);
					else
						filter &= ProdutoSpecifications.CategoriaIdGreaterThan(request.CategoriaIdGreaterThan.Value);
				}
				if (request.CategoriaIdLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CategoriaIdLessThanOrEqual(request.CategoriaIdLessThanOrEqual.Value);
					else
						filter &= ProdutoSpecifications.CategoriaIdLessThanOrEqual(request.CategoriaIdLessThanOrEqual.Value);
				}
				if (request.CategoriaIdGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CategoriaIdGreaterThanOrEqual(request.CategoriaIdGreaterThanOrEqual.Value);
					else
						filter &= ProdutoSpecifications.CategoriaIdGreaterThanOrEqual(request.CategoriaIdGreaterThanOrEqual.Value);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= ProdutoSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= ProdutoSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= ProdutoSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= ProdutoSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= ProdutoSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= ProdutoSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= ProdutoSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= ProdutoSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= ProdutoSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= ProdutoSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= ProdutoSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= ProdutoSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= ProdutoSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= ProdutoSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= ProdutoSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= ProdutoSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= ProdutoSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= ProdutoSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= ProdutoSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= ProdutoSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= ProdutoSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= ProdutoSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= ProdutoSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= ProdutoSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= ProdutoSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= ProdutoSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= ProdutoSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= ProdutoSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= ProdutoSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= ProdutoSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= ProdutoSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= ProdutoSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= ProdutoSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= ProdutoSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= ProdutoSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.IdContains(request.IdContains);
					else
						filter &= ProdutoSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= ProdutoSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= ProdutoSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= ProdutoSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= ProdutoSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= ProdutoSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProdutoSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= ProdutoSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
	public static class MarketPlaceAggSettingsFilters 
	{
	    public static Expression<Func<MarketPlaceAggSettings, bool>> GetFilters(this MarketPlaceAggSettingsQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<MarketPlaceAggSettings> GetFiltersSpecification(this MarketPlaceAggSettingsQueryModel request, bool isOrSpecification = false) 
		{
			Specification<MarketPlaceAggSettings> filter = new DirectSpecification<MarketPlaceAggSettings>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (request.UserIdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UserIdEqual(request.UserIdEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UserIdEqual(request.UserIdEqual.Value);
				}
				if (request.UserIdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UserIdNotEqual(request.UserIdNotEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UserIdNotEqual(request.UserIdNotEqual.Value);
				}
				if (request.UserIdContains != null)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UserIdContains(request.UserIdContains);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UserIdContains(request.UserIdContains);
				}
				if (request.UserIdNotContains != null)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UserIdNotContains(request.UserIdNotContains);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UserIdNotContains(request.UserIdNotContains);
				}
				if (request.UserIdSince.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UserIdGreaterThanOrEqual(request.UserIdSince.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UserIdGreaterThanOrEqual(request.UserIdSince.Value);
				}
				if (request.UserIdUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UserIdLessThan(request.UserIdUntil.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UserIdLessThan(request.UserIdUntil.Value);
				}
				if (request.UserIdNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UserIdNotEqual(request.UserIdNotEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UserIdNotEqual(request.UserIdNotEqual.Value);
				}
				if (request.UserIdLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UserIdLessThan(request.UserIdLessThan.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UserIdLessThan(request.UserIdLessThan.Value);
				}
				if (request.UserIdGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UserIdGreaterThan(request.UserIdGreaterThan.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UserIdGreaterThan(request.UserIdGreaterThan.Value);
				}
				if (request.UserIdLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UserIdLessThanOrEqual(request.UserIdLessThanOrEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UserIdLessThanOrEqual(request.UserIdLessThanOrEqual.Value);
				}
				if (request.UserIdGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UserIdGreaterThanOrEqual(request.UserIdGreaterThanOrEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UserIdGreaterThanOrEqual(request.UserIdGreaterThanOrEqual.Value);
				}
				if (request.AutoSaveSettingsEnabledEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.AutoSaveSettingsEnabledEqual(request.AutoSaveSettingsEnabledEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.AutoSaveSettingsEnabledEqual(request.AutoSaveSettingsEnabledEqual.Value);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= MarketPlaceAggSettingsSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= MarketPlaceAggSettingsSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= MarketPlaceAggSettingsSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= MarketPlaceAggSettingsSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.IdContains(request.IdContains);
					else
						filter &= MarketPlaceAggSettingsSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= MarketPlaceAggSettingsSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= MarketPlaceAggSettingsSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= MarketPlaceAggSettingsSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= MarketPlaceAggSettingsSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= MarketPlaceAggSettingsSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= MarketPlaceAggSettingsSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= MarketPlaceAggSettingsSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
	public static class CarrinhoFilters 
	{
	    public static Expression<Func<Carrinho, bool>> GetFilters(this CarrinhoQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<Carrinho> GetFiltersSpecification(this CarrinhoQueryModel request, bool isOrSpecification = false) 
		{
			Specification<Carrinho> filter = new DirectSpecification<Carrinho>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (request.ProdutoIdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ProdutoIdEqual(request.ProdutoIdEqual.Value);
					else
						filter &= CarrinhoSpecifications.ProdutoIdEqual(request.ProdutoIdEqual.Value);
				}
				if (request.ProdutoIdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ProdutoIdNotEqual(request.ProdutoIdNotEqual.Value);
					else
						filter &= CarrinhoSpecifications.ProdutoIdNotEqual(request.ProdutoIdNotEqual.Value);
				}
				if (request.ProdutoIdContains != null)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ProdutoIdContains(request.ProdutoIdContains);
					else
						filter &= CarrinhoSpecifications.ProdutoIdContains(request.ProdutoIdContains);
				}
				if (request.ProdutoIdNotContains != null)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ProdutoIdNotContains(request.ProdutoIdNotContains);
					else
						filter &= CarrinhoSpecifications.ProdutoIdNotContains(request.ProdutoIdNotContains);
				}
				if (request.ProdutoIdSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ProdutoIdGreaterThanOrEqual(request.ProdutoIdSince.Value);
					else
						filter &= CarrinhoSpecifications.ProdutoIdGreaterThanOrEqual(request.ProdutoIdSince.Value);
				}
				if (request.ProdutoIdUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ProdutoIdLessThan(request.ProdutoIdUntil.Value);
					else
						filter &= CarrinhoSpecifications.ProdutoIdLessThan(request.ProdutoIdUntil.Value);
				}
				if (request.ProdutoIdNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ProdutoIdNotEqual(request.ProdutoIdNotEqual.Value);
					else
						filter &= CarrinhoSpecifications.ProdutoIdNotEqual(request.ProdutoIdNotEqual.Value);
				}
				if (request.ProdutoIdLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ProdutoIdLessThan(request.ProdutoIdLessThan.Value);
					else
						filter &= CarrinhoSpecifications.ProdutoIdLessThan(request.ProdutoIdLessThan.Value);
				}
				if (request.ProdutoIdGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ProdutoIdGreaterThan(request.ProdutoIdGreaterThan.Value);
					else
						filter &= CarrinhoSpecifications.ProdutoIdGreaterThan(request.ProdutoIdGreaterThan.Value);
				}
				if (request.ProdutoIdLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ProdutoIdLessThanOrEqual(request.ProdutoIdLessThanOrEqual.Value);
					else
						filter &= CarrinhoSpecifications.ProdutoIdLessThanOrEqual(request.ProdutoIdLessThanOrEqual.Value);
				}
				if (request.ProdutoIdGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ProdutoIdGreaterThanOrEqual(request.ProdutoIdGreaterThanOrEqual.Value);
					else
						filter &= CarrinhoSpecifications.ProdutoIdGreaterThanOrEqual(request.ProdutoIdGreaterThanOrEqual.Value);
				}
				if (request.UsuarioIdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UsuarioIdEqual(request.UsuarioIdEqual.Value);
					else
						filter &= CarrinhoSpecifications.UsuarioIdEqual(request.UsuarioIdEqual.Value);
				}
				if (request.UsuarioIdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UsuarioIdNotEqual(request.UsuarioIdNotEqual.Value);
					else
						filter &= CarrinhoSpecifications.UsuarioIdNotEqual(request.UsuarioIdNotEqual.Value);
				}
				if (request.UsuarioIdContains != null)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UsuarioIdContains(request.UsuarioIdContains);
					else
						filter &= CarrinhoSpecifications.UsuarioIdContains(request.UsuarioIdContains);
				}
				if (request.UsuarioIdNotContains != null)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UsuarioIdNotContains(request.UsuarioIdNotContains);
					else
						filter &= CarrinhoSpecifications.UsuarioIdNotContains(request.UsuarioIdNotContains);
				}
				if (request.UsuarioIdSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UsuarioIdGreaterThanOrEqual(request.UsuarioIdSince.Value);
					else
						filter &= CarrinhoSpecifications.UsuarioIdGreaterThanOrEqual(request.UsuarioIdSince.Value);
				}
				if (request.UsuarioIdUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UsuarioIdLessThan(request.UsuarioIdUntil.Value);
					else
						filter &= CarrinhoSpecifications.UsuarioIdLessThan(request.UsuarioIdUntil.Value);
				}
				if (request.UsuarioIdNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UsuarioIdNotEqual(request.UsuarioIdNotEqual.Value);
					else
						filter &= CarrinhoSpecifications.UsuarioIdNotEqual(request.UsuarioIdNotEqual.Value);
				}
				if (request.UsuarioIdLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UsuarioIdLessThan(request.UsuarioIdLessThan.Value);
					else
						filter &= CarrinhoSpecifications.UsuarioIdLessThan(request.UsuarioIdLessThan.Value);
				}
				if (request.UsuarioIdGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UsuarioIdGreaterThan(request.UsuarioIdGreaterThan.Value);
					else
						filter &= CarrinhoSpecifications.UsuarioIdGreaterThan(request.UsuarioIdGreaterThan.Value);
				}
				if (request.UsuarioIdLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UsuarioIdLessThanOrEqual(request.UsuarioIdLessThanOrEqual.Value);
					else
						filter &= CarrinhoSpecifications.UsuarioIdLessThanOrEqual(request.UsuarioIdLessThanOrEqual.Value);
				}
				if (request.UsuarioIdGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UsuarioIdGreaterThanOrEqual(request.UsuarioIdGreaterThanOrEqual.Value);
					else
						filter &= CarrinhoSpecifications.UsuarioIdGreaterThanOrEqual(request.UsuarioIdGreaterThanOrEqual.Value);
				}
				if (request.PagamentoEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.PagamentoEqual(request.PagamentoEqual.Value);
					else
						filter &= CarrinhoSpecifications.PagamentoEqual(request.PagamentoEqual.Value);
				}
				if (request.PagamentoNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.PagamentoNotEqual(request.PagamentoNotEqual.Value);
					else
						filter &= CarrinhoSpecifications.PagamentoNotEqual(request.PagamentoNotEqual.Value);
				}
				if (request.PagamentoContains != null)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.PagamentoContains(request.PagamentoContains);
					else
						filter &= CarrinhoSpecifications.PagamentoContains(request.PagamentoContains);
				}
				if (request.PagamentoNotContains != null)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.PagamentoNotContains(request.PagamentoNotContains);
					else
						filter &= CarrinhoSpecifications.PagamentoNotContains(request.PagamentoNotContains);
				}
				if (request.PagamentoSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.PagamentoGreaterThanOrEqual(request.PagamentoSince.Value);
					else
						filter &= CarrinhoSpecifications.PagamentoGreaterThanOrEqual(request.PagamentoSince.Value);
				}
				if (request.PagamentoUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.PagamentoLessThan(request.PagamentoUntil.Value);
					else
						filter &= CarrinhoSpecifications.PagamentoLessThan(request.PagamentoUntil.Value);
				}
				if (request.PagamentoNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.PagamentoNotEqual(request.PagamentoNotEqual.Value);
					else
						filter &= CarrinhoSpecifications.PagamentoNotEqual(request.PagamentoNotEqual.Value);
				}
				if (request.PagamentoLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.PagamentoLessThan(request.PagamentoLessThan.Value);
					else
						filter &= CarrinhoSpecifications.PagamentoLessThan(request.PagamentoLessThan.Value);
				}
				if (request.PagamentoGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.PagamentoGreaterThan(request.PagamentoGreaterThan.Value);
					else
						filter &= CarrinhoSpecifications.PagamentoGreaterThan(request.PagamentoGreaterThan.Value);
				}
				if (request.PagamentoLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.PagamentoLessThanOrEqual(request.PagamentoLessThanOrEqual.Value);
					else
						filter &= CarrinhoSpecifications.PagamentoLessThanOrEqual(request.PagamentoLessThanOrEqual.Value);
				}
				if (request.PagamentoGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.PagamentoGreaterThanOrEqual(request.PagamentoGreaterThanOrEqual.Value);
					else
						filter &= CarrinhoSpecifications.PagamentoGreaterThanOrEqual(request.PagamentoGreaterThanOrEqual.Value);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= CarrinhoSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= CarrinhoSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= CarrinhoSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= CarrinhoSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= CarrinhoSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= CarrinhoSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= CarrinhoSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= CarrinhoSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= CarrinhoSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= CarrinhoSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= CarrinhoSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= CarrinhoSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= CarrinhoSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= CarrinhoSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= CarrinhoSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= CarrinhoSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= CarrinhoSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= CarrinhoSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= CarrinhoSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= CarrinhoSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= CarrinhoSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= CarrinhoSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= CarrinhoSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= CarrinhoSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= CarrinhoSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= CarrinhoSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= CarrinhoSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= CarrinhoSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= CarrinhoSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= CarrinhoSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= CarrinhoSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= CarrinhoSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= CarrinhoSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= CarrinhoSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= CarrinhoSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.IdContains(request.IdContains);
					else
						filter &= CarrinhoSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= CarrinhoSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= CarrinhoSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= CarrinhoSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= CarrinhoSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= CarrinhoSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CarrinhoSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= CarrinhoSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
	public static class CategoriaprodutoFilters 
	{
	    public static Expression<Func<Categoriaproduto, bool>> GetFilters(this CategoriaprodutoQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<Categoriaproduto> GetFiltersSpecification(this CategoriaprodutoQueryModel request, bool isOrSpecification = false) 
		{
			Specification<Categoriaproduto> filter = new DirectSpecification<Categoriaproduto>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= CategoriaprodutoSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= CategoriaprodutoSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= CategoriaprodutoSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= CategoriaprodutoSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= CategoriaprodutoSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= CategoriaprodutoSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= CategoriaprodutoSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= CategoriaprodutoSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= CategoriaprodutoSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= CategoriaprodutoSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= CategoriaprodutoSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= CategoriaprodutoSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= CategoriaprodutoSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= CategoriaprodutoSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= CategoriaprodutoSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= CategoriaprodutoSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= CategoriaprodutoSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= CategoriaprodutoSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.IdContains(request.IdContains);
					else
						filter &= CategoriaprodutoSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= CategoriaprodutoSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= CategoriaprodutoSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= CategoriaprodutoSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= CategoriaprodutoSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= CategoriaprodutoSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CategoriaprodutoSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= CategoriaprodutoSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
}

