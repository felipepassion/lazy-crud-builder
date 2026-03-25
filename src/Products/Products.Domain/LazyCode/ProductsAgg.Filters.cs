using Lazy.Crud.CrossCutting.Infra.Utils.Extensions;
using System.Linq.Expressions;
using Lazy.Crud.Builder.Domain.Seedwork.Specification;
using Lazy.Crud.CrossCutting.Infra.Utils.Extensions;

namespace Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Filters{
	using Entities;
	using Specifications;
	using Queries.Models;
	public static class ProductsFilters 
	{
	    public static Expression<Func<Products, bool>> GetFilters(this ProductsQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<Products> GetFiltersSpecification(this ProductsQueryModel request, bool isOrSpecification = false) 
		{
			isOrSpecification = request.IsOrSpecification;
			Specification<Products> filter = new DirectSpecification<Products>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (!string.IsNullOrWhiteSpace(request.TestPropertyEqual)) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.TestPropertyEqual(request.TestPropertyEqual);
					else
						filter &= ProductsSpecifications.TestPropertyEqual(request.TestPropertyEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.TestPropertyNotEqual)) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.TestPropertyNotEqual(request.TestPropertyNotEqual);
					else
						filter &= ProductsSpecifications.TestPropertyNotEqual(request.TestPropertyNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.TestPropertyNotEqual)) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.TestPropertyNotEqual(request.TestPropertyNotEqual);
					else
						filter &= ProductsSpecifications.TestPropertyNotEqual(request.TestPropertyNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.TestPropertyContains)) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.TestPropertyContains(request.TestPropertyContains);
					else
						filter &= ProductsSpecifications.TestPropertyContains(request.TestPropertyContains);
				}
				if (!string.IsNullOrWhiteSpace(request.TestPropertyStartsWith)) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.TestPropertyStartsWith(request.TestPropertyStartsWith);
					else
						filter &= ProductsSpecifications.TestPropertyStartsWith(request.TestPropertyStartsWith);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= ProductsSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= ProductsSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= ProductsSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= ProductsSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= ProductsSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= ProductsSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= ProductsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= ProductsSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= ProductsSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= ProductsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= ProductsSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= ProductsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= ProductsSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= ProductsSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= ProductsSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= ProductsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= ProductsSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= ProductsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= ProductsSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= ProductsSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= ProductsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= ProductsSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= ProductsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= ProductsSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= ProductsSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= ProductsSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= ProductsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= ProductsSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= ProductsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= ProductsSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= ProductsSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= ProductsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= ProductsSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= ProductsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= ProductsSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= ProductsSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= ProductsSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= ProductsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= ProductsSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= ProductsSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.IdContains(request.IdContains);
					else
						filter &= ProductsSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= ProductsSpecifications.IdNotContains(request.IdNotContains);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= ProductsSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= ProductsSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
}

