using LazyCrudBuilder.CrossCuting.Infra.Utils.Extensions;
using System.Linq.Expressions;
using LazyCrudBuilder.Core.Domain.Seedwork.Specification;
using LazyCrudBuilder.CrossCuting.Infra.Utils.Extensions;

namespace LazyCrudBuilder.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Filters{
	using Entities;
	using Specifications;
	using Queries.Models;
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
}

