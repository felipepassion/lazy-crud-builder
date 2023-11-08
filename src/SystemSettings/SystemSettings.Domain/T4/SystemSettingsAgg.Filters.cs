using LazyCrud.CrossCuting.Infra.Utils.Extensions;
using System.Linq.Expressions;
using LazyCrud.Core.Domain.Seedwork.Specification;
using LazyCrud.CrossCuting.Infra.Utils.Extensions;

namespace LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Filters{
	using Entities;
	using Specifications;
	using Queries.Models;
	public static class SystemPanelSubItemFilters 
	{
	    public static Expression<Func<SystemPanelSubItem, bool>> GetFilters(this SystemPanelSubItemQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<SystemPanelSubItem> GetFiltersSpecification(this SystemPanelSubItemQueryModel request, bool isOrSpecification = false) 
		{
			Specification<SystemPanelSubItem> filter = new DirectSpecification<SystemPanelSubItem>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (request.SystemPanelSubItemIdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelSubItemIdEqual(request.SystemPanelSubItemIdEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelSubItemIdEqual(request.SystemPanelSubItemIdEqual.Value);
				}
				if (request.SystemPanelSubItemIdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelSubItemIdNotEqual(request.SystemPanelSubItemIdNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelSubItemIdNotEqual(request.SystemPanelSubItemIdNotEqual.Value);
				}
				if (request.SystemPanelSubItemIdContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelSubItemIdContains(request.SystemPanelSubItemIdContains);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelSubItemIdContains(request.SystemPanelSubItemIdContains);
				}
				if (request.SystemPanelSubItemIdNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelSubItemIdNotContains(request.SystemPanelSubItemIdNotContains);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelSubItemIdNotContains(request.SystemPanelSubItemIdNotContains);
				}
				if (request.SystemPanelSubItemIdSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelSubItemIdGreaterThanOrEqual(request.SystemPanelSubItemIdSince.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelSubItemIdGreaterThanOrEqual(request.SystemPanelSubItemIdSince.Value);
				}
				if (request.SystemPanelSubItemIdUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelSubItemIdLessThan(request.SystemPanelSubItemIdUntil.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelSubItemIdLessThan(request.SystemPanelSubItemIdUntil.Value);
				}
				if (request.SystemPanelSubItemIdNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelSubItemIdNotEqual(request.SystemPanelSubItemIdNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelSubItemIdNotEqual(request.SystemPanelSubItemIdNotEqual.Value);
				}
				if (request.SystemPanelSubItemIdLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelSubItemIdLessThan(request.SystemPanelSubItemIdLessThan.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelSubItemIdLessThan(request.SystemPanelSubItemIdLessThan.Value);
				}
				if (request.SystemPanelSubItemIdGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelSubItemIdGreaterThan(request.SystemPanelSubItemIdGreaterThan.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelSubItemIdGreaterThan(request.SystemPanelSubItemIdGreaterThan.Value);
				}
				if (request.SystemPanelSubItemIdLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelSubItemIdLessThanOrEqual(request.SystemPanelSubItemIdLessThanOrEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelSubItemIdLessThanOrEqual(request.SystemPanelSubItemIdLessThanOrEqual.Value);
				}
				if (request.SystemPanelSubItemIdGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelSubItemIdGreaterThanOrEqual(request.SystemPanelSubItemIdGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelSubItemIdGreaterThanOrEqual(request.SystemPanelSubItemIdGreaterThanOrEqual.Value);
				}
				if (request.SystemPanelIdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelIdEqual(request.SystemPanelIdEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelIdEqual(request.SystemPanelIdEqual.Value);
				}
				if (request.SystemPanelIdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelIdNotEqual(request.SystemPanelIdNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelIdNotEqual(request.SystemPanelIdNotEqual.Value);
				}
				if (request.SystemPanelIdContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelIdContains(request.SystemPanelIdContains);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelIdContains(request.SystemPanelIdContains);
				}
				if (request.SystemPanelIdNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelIdNotContains(request.SystemPanelIdNotContains);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelIdNotContains(request.SystemPanelIdNotContains);
				}
				if (request.SystemPanelIdSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelIdGreaterThanOrEqual(request.SystemPanelIdSince.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelIdGreaterThanOrEqual(request.SystemPanelIdSince.Value);
				}
				if (request.SystemPanelIdUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelIdLessThan(request.SystemPanelIdUntil.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelIdLessThan(request.SystemPanelIdUntil.Value);
				}
				if (request.SystemPanelIdNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelIdNotEqual(request.SystemPanelIdNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelIdNotEqual(request.SystemPanelIdNotEqual.Value);
				}
				if (request.SystemPanelIdLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelIdLessThan(request.SystemPanelIdLessThan.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelIdLessThan(request.SystemPanelIdLessThan.Value);
				}
				if (request.SystemPanelIdGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelIdGreaterThan(request.SystemPanelIdGreaterThan.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelIdGreaterThan(request.SystemPanelIdGreaterThan.Value);
				}
				if (request.SystemPanelIdLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelIdLessThanOrEqual(request.SystemPanelIdLessThanOrEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelIdLessThanOrEqual(request.SystemPanelIdLessThanOrEqual.Value);
				}
				if (request.SystemPanelIdGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.SystemPanelIdGreaterThanOrEqual(request.SystemPanelIdGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.SystemPanelIdGreaterThanOrEqual(request.SystemPanelIdGreaterThanOrEqual.Value);
				}
				if (request.IsSubItemEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.IsSubItemEqual(request.IsSubItemEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.IsSubItemEqual(request.IsSubItemEqual.Value);
				}
				if (!string.IsNullOrWhiteSpace(request.IconEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.IconEqual(request.IconEqual);
					else
						filter &= SystemPanelSubItemSpecifications.IconEqual(request.IconEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.IconNotEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.IconNotEqual(request.IconNotEqual);
					else
						filter &= SystemPanelSubItemSpecifications.IconNotEqual(request.IconNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.IconContains)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.IconContains(request.IconContains);
					else
						filter &= SystemPanelSubItemSpecifications.IconContains(request.IconContains);
				}
				if (!string.IsNullOrWhiteSpace(request.IconStartsWith)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.IconStartsWith(request.IconStartsWith);
					else
						filter &= SystemPanelSubItemSpecifications.IconStartsWith(request.IconStartsWith);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DescriptionEqual(request.DescriptionEqual);
					else
						filter &= SystemPanelSubItemSpecifications.DescriptionEqual(request.DescriptionEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionNotEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DescriptionNotEqual(request.DescriptionNotEqual);
					else
						filter &= SystemPanelSubItemSpecifications.DescriptionNotEqual(request.DescriptionNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionContains)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DescriptionContains(request.DescriptionContains);
					else
						filter &= SystemPanelSubItemSpecifications.DescriptionContains(request.DescriptionContains);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionStartsWith)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DescriptionStartsWith(request.DescriptionStartsWith);
					else
						filter &= SystemPanelSubItemSpecifications.DescriptionStartsWith(request.DescriptionStartsWith);
				}
				if (!string.IsNullOrWhiteSpace(request.UrlEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UrlEqual(request.UrlEqual);
					else
						filter &= SystemPanelSubItemSpecifications.UrlEqual(request.UrlEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.UrlNotEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UrlNotEqual(request.UrlNotEqual);
					else
						filter &= SystemPanelSubItemSpecifications.UrlNotEqual(request.UrlNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.UrlContains)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UrlContains(request.UrlContains);
					else
						filter &= SystemPanelSubItemSpecifications.UrlContains(request.UrlContains);
				}
				if (!string.IsNullOrWhiteSpace(request.UrlStartsWith)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UrlStartsWith(request.UrlStartsWith);
					else
						filter &= SystemPanelSubItemSpecifications.UrlStartsWith(request.UrlStartsWith);
				}
				if (request.LinkDiretoEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.LinkDiretoEqual(request.LinkDiretoEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.LinkDiretoEqual(request.LinkDiretoEqual.Value);
				}
				if (request.ActionButtonEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.ActionButtonEqual(request.ActionButtonEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.ActionButtonEqual(request.ActionButtonEqual.Value);
				}
				if (request.CurrentStepEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
				}
				if (request.CurrentStepNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CurrentStepContains(request.CurrentStepContains);
					else
						filter &= SystemPanelSubItemSpecifications.CurrentStepContains(request.CurrentStepContains);
				}
				if (request.CurrentStepNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
					else
						filter &= SystemPanelSubItemSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
				}
				if (request.CurrentStepSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
				}
				if (request.CurrentStepUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
				}
				if (request.CurrentStepNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
				}
				if (request.CurrentStepGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
				}
				if (request.CurrentStepLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
				}
				if (request.CurrentStepGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
				}
				if (request.MaxStepsEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
				}
				if (request.MaxStepsNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.MaxStepsContains(request.MaxStepsContains);
					else
						filter &= SystemPanelSubItemSpecifications.MaxStepsContains(request.MaxStepsContains);
				}
				if (request.MaxStepsNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
					else
						filter &= SystemPanelSubItemSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
				}
				if (request.MaxStepsSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
					else
						filter &= SystemPanelSubItemSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
				}
				if (request.MaxStepsUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
					else
						filter &= SystemPanelSubItemSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
				}
				if (request.MaxStepsNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
					else
						filter &= SystemPanelSubItemSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
				}
				if (request.MaxStepsGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
					else
						filter &= SystemPanelSubItemSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
				}
				if (request.MaxStepsLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
				}
				if (request.MaxStepsGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
				}
				if (request.RegisterDoneEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
				}
				if (request.ActiveEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.ActiveEqual(request.ActiveEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.ActiveEqual(request.ActiveEqual.Value);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= SystemPanelSubItemSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= SystemPanelSubItemSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= SystemPanelSubItemSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= SystemPanelSubItemSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= SystemPanelSubItemSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= SystemPanelSubItemSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= SystemPanelSubItemSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= SystemPanelSubItemSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= SystemPanelSubItemSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= SystemPanelSubItemSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= SystemPanelSubItemSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= SystemPanelSubItemSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= SystemPanelSubItemSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= SystemPanelSubItemSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.IdContains(request.IdContains);
					else
						filter &= SystemPanelSubItemSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= SystemPanelSubItemSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= SystemPanelSubItemSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= SystemPanelSubItemSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= SystemPanelSubItemSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= SystemPanelSubItemSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSubItemSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= SystemPanelSubItemSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
	public static class SystemPanelFilters 
	{
	    public static Expression<Func<SystemPanel, bool>> GetFilters(this SystemPanelQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<SystemPanel> GetFiltersSpecification(this SystemPanelQueryModel request, bool isOrSpecification = false) 
		{
			Specification<SystemPanel> filter = new DirectSpecification<SystemPanel>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (!string.IsNullOrWhiteSpace(request.IconEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.IconEqual(request.IconEqual);
					else
						filter &= SystemPanelSpecifications.IconEqual(request.IconEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.IconNotEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.IconNotEqual(request.IconNotEqual);
					else
						filter &= SystemPanelSpecifications.IconNotEqual(request.IconNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.IconContains)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.IconContains(request.IconContains);
					else
						filter &= SystemPanelSpecifications.IconContains(request.IconContains);
				}
				if (!string.IsNullOrWhiteSpace(request.IconStartsWith)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.IconStartsWith(request.IconStartsWith);
					else
						filter &= SystemPanelSpecifications.IconStartsWith(request.IconStartsWith);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DescriptionEqual(request.DescriptionEqual);
					else
						filter &= SystemPanelSpecifications.DescriptionEqual(request.DescriptionEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionNotEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DescriptionNotEqual(request.DescriptionNotEqual);
					else
						filter &= SystemPanelSpecifications.DescriptionNotEqual(request.DescriptionNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionContains)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DescriptionContains(request.DescriptionContains);
					else
						filter &= SystemPanelSpecifications.DescriptionContains(request.DescriptionContains);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionStartsWith)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DescriptionStartsWith(request.DescriptionStartsWith);
					else
						filter &= SystemPanelSpecifications.DescriptionStartsWith(request.DescriptionStartsWith);
				}
				if (request.CurrentStepEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
					else
						filter &= SystemPanelSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
				}
				if (request.CurrentStepNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= SystemPanelSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CurrentStepContains(request.CurrentStepContains);
					else
						filter &= SystemPanelSpecifications.CurrentStepContains(request.CurrentStepContains);
				}
				if (request.CurrentStepNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
					else
						filter &= SystemPanelSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
				}
				if (request.CurrentStepSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
					else
						filter &= SystemPanelSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
				}
				if (request.CurrentStepUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
					else
						filter &= SystemPanelSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
				}
				if (request.CurrentStepNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= SystemPanelSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
					else
						filter &= SystemPanelSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
				}
				if (request.CurrentStepGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
					else
						filter &= SystemPanelSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
				}
				if (request.CurrentStepLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
					else
						filter &= SystemPanelSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
				}
				if (request.CurrentStepGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
				}
				if (request.MaxStepsEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
					else
						filter &= SystemPanelSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
				}
				if (request.MaxStepsNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= SystemPanelSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.MaxStepsContains(request.MaxStepsContains);
					else
						filter &= SystemPanelSpecifications.MaxStepsContains(request.MaxStepsContains);
				}
				if (request.MaxStepsNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
					else
						filter &= SystemPanelSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
				}
				if (request.MaxStepsSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
					else
						filter &= SystemPanelSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
				}
				if (request.MaxStepsUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
					else
						filter &= SystemPanelSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
				}
				if (request.MaxStepsNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= SystemPanelSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
					else
						filter &= SystemPanelSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
				}
				if (request.MaxStepsGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
					else
						filter &= SystemPanelSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
				}
				if (request.MaxStepsLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
					else
						filter &= SystemPanelSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
				}
				if (request.MaxStepsGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
				}
				if (request.RegisterDoneEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
					else
						filter &= SystemPanelSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
				}
				if (request.ActiveEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.ActiveEqual(request.ActiveEqual.Value);
					else
						filter &= SystemPanelSpecifications.ActiveEqual(request.ActiveEqual.Value);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= SystemPanelSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= SystemPanelSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= SystemPanelSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= SystemPanelSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= SystemPanelSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= SystemPanelSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= SystemPanelSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= SystemPanelSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= SystemPanelSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= SystemPanelSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= SystemPanelSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= SystemPanelSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= SystemPanelSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= SystemPanelSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= SystemPanelSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= SystemPanelSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= SystemPanelSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= SystemPanelSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= SystemPanelSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= SystemPanelSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= SystemPanelSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= SystemPanelSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= SystemPanelSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= SystemPanelSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= SystemPanelSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= SystemPanelSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= SystemPanelSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= SystemPanelSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= SystemPanelSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= SystemPanelSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= SystemPanelSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= SystemPanelSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.IdContains(request.IdContains);
					else
						filter &= SystemPanelSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= SystemPanelSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= SystemPanelSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= SystemPanelSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= SystemPanelSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= SystemPanelSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= SystemPanelSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
	public static class SystemPanelGroupFilters 
	{
	    public static Expression<Func<SystemPanelGroup, bool>> GetFilters(this SystemPanelGroupQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<SystemPanelGroup> GetFiltersSpecification(this SystemPanelGroupQueryModel request, bool isOrSpecification = false) 
		{
			Specification<SystemPanelGroup> filter = new DirectSpecification<SystemPanelGroup>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (!string.IsNullOrWhiteSpace(request.IconEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.IconEqual(request.IconEqual);
					else
						filter &= SystemPanelGroupSpecifications.IconEqual(request.IconEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.IconNotEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.IconNotEqual(request.IconNotEqual);
					else
						filter &= SystemPanelGroupSpecifications.IconNotEqual(request.IconNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.IconContains)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.IconContains(request.IconContains);
					else
						filter &= SystemPanelGroupSpecifications.IconContains(request.IconContains);
				}
				if (!string.IsNullOrWhiteSpace(request.IconStartsWith)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.IconStartsWith(request.IconStartsWith);
					else
						filter &= SystemPanelGroupSpecifications.IconStartsWith(request.IconStartsWith);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DescriptionEqual(request.DescriptionEqual);
					else
						filter &= SystemPanelGroupSpecifications.DescriptionEqual(request.DescriptionEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionNotEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DescriptionNotEqual(request.DescriptionNotEqual);
					else
						filter &= SystemPanelGroupSpecifications.DescriptionNotEqual(request.DescriptionNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionContains)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DescriptionContains(request.DescriptionContains);
					else
						filter &= SystemPanelGroupSpecifications.DescriptionContains(request.DescriptionContains);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionStartsWith)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DescriptionStartsWith(request.DescriptionStartsWith);
					else
						filter &= SystemPanelGroupSpecifications.DescriptionStartsWith(request.DescriptionStartsWith);
				}
				if (!string.IsNullOrWhiteSpace(request.CodeEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CodeEqual(request.CodeEqual);
					else
						filter &= SystemPanelGroupSpecifications.CodeEqual(request.CodeEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.CodeNotEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CodeNotEqual(request.CodeNotEqual);
					else
						filter &= SystemPanelGroupSpecifications.CodeNotEqual(request.CodeNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.CodeContains)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CodeContains(request.CodeContains);
					else
						filter &= SystemPanelGroupSpecifications.CodeContains(request.CodeContains);
				}
				if (!string.IsNullOrWhiteSpace(request.CodeStartsWith)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CodeStartsWith(request.CodeStartsWith);
					else
						filter &= SystemPanelGroupSpecifications.CodeStartsWith(request.CodeStartsWith);
				}
				if (request.CurrentStepEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
				}
				if (request.CurrentStepNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CurrentStepContains(request.CurrentStepContains);
					else
						filter &= SystemPanelGroupSpecifications.CurrentStepContains(request.CurrentStepContains);
				}
				if (request.CurrentStepNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
					else
						filter &= SystemPanelGroupSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
				}
				if (request.CurrentStepSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
					else
						filter &= SystemPanelGroupSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
				}
				if (request.CurrentStepUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
					else
						filter &= SystemPanelGroupSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
				}
				if (request.CurrentStepNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
					else
						filter &= SystemPanelGroupSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
				}
				if (request.CurrentStepGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
					else
						filter &= SystemPanelGroupSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
				}
				if (request.CurrentStepLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
				}
				if (request.CurrentStepGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
				}
				if (request.MaxStepsEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
				}
				if (request.MaxStepsNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.MaxStepsContains(request.MaxStepsContains);
					else
						filter &= SystemPanelGroupSpecifications.MaxStepsContains(request.MaxStepsContains);
				}
				if (request.MaxStepsNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
					else
						filter &= SystemPanelGroupSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
				}
				if (request.MaxStepsSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
					else
						filter &= SystemPanelGroupSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
				}
				if (request.MaxStepsUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
					else
						filter &= SystemPanelGroupSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
				}
				if (request.MaxStepsNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
					else
						filter &= SystemPanelGroupSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
				}
				if (request.MaxStepsGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
					else
						filter &= SystemPanelGroupSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
				}
				if (request.MaxStepsLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
				}
				if (request.MaxStepsGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
				}
				if (request.RegisterDoneEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
				}
				if (request.ActiveEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.ActiveEqual(request.ActiveEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.ActiveEqual(request.ActiveEqual.Value);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= SystemPanelGroupSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= SystemPanelGroupSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= SystemPanelGroupSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= SystemPanelGroupSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= SystemPanelGroupSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= SystemPanelGroupSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= SystemPanelGroupSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= SystemPanelGroupSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= SystemPanelGroupSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= SystemPanelGroupSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= SystemPanelGroupSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= SystemPanelGroupSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= SystemPanelGroupSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= SystemPanelGroupSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= SystemPanelGroupSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= SystemPanelGroupSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= SystemPanelGroupSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= SystemPanelGroupSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.IdContains(request.IdContains);
					else
						filter &= SystemPanelGroupSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= SystemPanelGroupSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= SystemPanelGroupSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= SystemPanelGroupSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= SystemPanelGroupSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= SystemPanelGroupSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemPanelGroupSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= SystemPanelGroupSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
	public static class CargaTabelaFilters 
	{
	    public static Expression<Func<CargaTabela, bool>> GetFilters(this CargaTabelaQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<CargaTabela> GetFiltersSpecification(this CargaTabelaQueryModel request, bool isOrSpecification = false) 
		{
			Specification<CargaTabela> filter = new DirectSpecification<CargaTabela>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (!string.IsNullOrWhiteSpace(request.TableNameEqual)) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TableNameEqual(request.TableNameEqual);
					else
						filter &= CargaTabelaSpecifications.TableNameEqual(request.TableNameEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.TableNameNotEqual)) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TableNameNotEqual(request.TableNameNotEqual);
					else
						filter &= CargaTabelaSpecifications.TableNameNotEqual(request.TableNameNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.TableNameContains)) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TableNameContains(request.TableNameContains);
					else
						filter &= CargaTabelaSpecifications.TableNameContains(request.TableNameContains);
				}
				if (!string.IsNullOrWhiteSpace(request.TableNameStartsWith)) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TableNameStartsWith(request.TableNameStartsWith);
					else
						filter &= CargaTabelaSpecifications.TableNameStartsWith(request.TableNameStartsWith);
				}
				if (!string.IsNullOrWhiteSpace(request.FilePathEqual)) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.FilePathEqual(request.FilePathEqual);
					else
						filter &= CargaTabelaSpecifications.FilePathEqual(request.FilePathEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.FilePathNotEqual)) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.FilePathNotEqual(request.FilePathNotEqual);
					else
						filter &= CargaTabelaSpecifications.FilePathNotEqual(request.FilePathNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.FilePathContains)) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.FilePathContains(request.FilePathContains);
					else
						filter &= CargaTabelaSpecifications.FilePathContains(request.FilePathContains);
				}
				if (!string.IsNullOrWhiteSpace(request.FilePathStartsWith)) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.FilePathStartsWith(request.FilePathStartsWith);
					else
						filter &= CargaTabelaSpecifications.FilePathStartsWith(request.FilePathStartsWith);
				}
				if (request.IsInitializedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.IsInitializedEqual(request.IsInitializedEqual.Value);
					else
						filter &= CargaTabelaSpecifications.IsInitializedEqual(request.IsInitializedEqual.Value);
				}
				if (request.TotalEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TotalEqual(request.TotalEqual.Value);
					else
						filter &= CargaTabelaSpecifications.TotalEqual(request.TotalEqual.Value);
				}
				if (request.TotalNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TotalNotEqual(request.TotalNotEqual.Value);
					else
						filter &= CargaTabelaSpecifications.TotalNotEqual(request.TotalNotEqual.Value);
				}
				if (request.TotalContains != null)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TotalContains(request.TotalContains);
					else
						filter &= CargaTabelaSpecifications.TotalContains(request.TotalContains);
				}
				if (request.TotalNotContains != null)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TotalNotContains(request.TotalNotContains);
					else
						filter &= CargaTabelaSpecifications.TotalNotContains(request.TotalNotContains);
				}
				if (request.TotalSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TotalGreaterThanOrEqual(request.TotalSince.Value);
					else
						filter &= CargaTabelaSpecifications.TotalGreaterThanOrEqual(request.TotalSince.Value);
				}
				if (request.TotalUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TotalLessThan(request.TotalUntil.Value);
					else
						filter &= CargaTabelaSpecifications.TotalLessThan(request.TotalUntil.Value);
				}
				if (request.TotalNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TotalNotEqual(request.TotalNotEqual.Value);
					else
						filter &= CargaTabelaSpecifications.TotalNotEqual(request.TotalNotEqual.Value);
				}
				if (request.TotalLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TotalLessThan(request.TotalLessThan.Value);
					else
						filter &= CargaTabelaSpecifications.TotalLessThan(request.TotalLessThan.Value);
				}
				if (request.TotalGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TotalGreaterThan(request.TotalGreaterThan.Value);
					else
						filter &= CargaTabelaSpecifications.TotalGreaterThan(request.TotalGreaterThan.Value);
				}
				if (request.TotalLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TotalLessThanOrEqual(request.TotalLessThanOrEqual.Value);
					else
						filter &= CargaTabelaSpecifications.TotalLessThanOrEqual(request.TotalLessThanOrEqual.Value);
				}
				if (request.TotalGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.TotalGreaterThanOrEqual(request.TotalGreaterThanOrEqual.Value);
					else
						filter &= CargaTabelaSpecifications.TotalGreaterThanOrEqual(request.TotalGreaterThanOrEqual.Value);
				}
				if (request.CurrentStepEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
					else
						filter &= CargaTabelaSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
				}
				if (request.CurrentStepNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= CargaTabelaSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepContains != null)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CurrentStepContains(request.CurrentStepContains);
					else
						filter &= CargaTabelaSpecifications.CurrentStepContains(request.CurrentStepContains);
				}
				if (request.CurrentStepNotContains != null)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
					else
						filter &= CargaTabelaSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
				}
				if (request.CurrentStepSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
					else
						filter &= CargaTabelaSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
				}
				if (request.CurrentStepUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
					else
						filter &= CargaTabelaSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
				}
				if (request.CurrentStepNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= CargaTabelaSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
					else
						filter &= CargaTabelaSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
				}
				if (request.CurrentStepGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
					else
						filter &= CargaTabelaSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
				}
				if (request.CurrentStepLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
					else
						filter &= CargaTabelaSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
				}
				if (request.CurrentStepGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
					else
						filter &= CargaTabelaSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
				}
				if (request.MaxStepsEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
					else
						filter &= CargaTabelaSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
				}
				if (request.MaxStepsNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= CargaTabelaSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsContains != null)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.MaxStepsContains(request.MaxStepsContains);
					else
						filter &= CargaTabelaSpecifications.MaxStepsContains(request.MaxStepsContains);
				}
				if (request.MaxStepsNotContains != null)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
					else
						filter &= CargaTabelaSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
				}
				if (request.MaxStepsSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
					else
						filter &= CargaTabelaSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
				}
				if (request.MaxStepsUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
					else
						filter &= CargaTabelaSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
				}
				if (request.MaxStepsNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= CargaTabelaSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
					else
						filter &= CargaTabelaSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
				}
				if (request.MaxStepsGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
					else
						filter &= CargaTabelaSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
				}
				if (request.MaxStepsLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
					else
						filter &= CargaTabelaSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
				}
				if (request.MaxStepsGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
					else
						filter &= CargaTabelaSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
				}
				if (request.RegisterDoneEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
					else
						filter &= CargaTabelaSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
				}
				if (request.ActiveEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.ActiveEqual(request.ActiveEqual.Value);
					else
						filter &= CargaTabelaSpecifications.ActiveEqual(request.ActiveEqual.Value);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= CargaTabelaSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= CargaTabelaSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= CargaTabelaSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= CargaTabelaSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= CargaTabelaSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= CargaTabelaSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= CargaTabelaSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= CargaTabelaSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= CargaTabelaSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= CargaTabelaSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= CargaTabelaSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= CargaTabelaSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= CargaTabelaSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= CargaTabelaSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= CargaTabelaSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= CargaTabelaSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= CargaTabelaSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= CargaTabelaSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= CargaTabelaSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= CargaTabelaSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= CargaTabelaSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= CargaTabelaSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= CargaTabelaSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= CargaTabelaSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= CargaTabelaSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= CargaTabelaSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= CargaTabelaSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= CargaTabelaSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= CargaTabelaSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= CargaTabelaSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= CargaTabelaSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= CargaTabelaSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= CargaTabelaSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= CargaTabelaSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= CargaTabelaSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.IdContains(request.IdContains);
					else
						filter &= CargaTabelaSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= CargaTabelaSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= CargaTabelaSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= CargaTabelaSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= CargaTabelaSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= CargaTabelaSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= CargaTabelaSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= CargaTabelaSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
	public static class SystemSettingsAggSettingsFilters 
	{
	    public static Expression<Func<SystemSettingsAggSettings, bool>> GetFilters(this SystemSettingsAggSettingsQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<SystemSettingsAggSettings> GetFiltersSpecification(this SystemSettingsAggSettingsQueryModel request, bool isOrSpecification = false) 
		{
			Specification<SystemSettingsAggSettings> filter = new DirectSpecification<SystemSettingsAggSettings>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (request.AutoSaveSettingsEnabledEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.AutoSaveSettingsEnabledEqual(request.AutoSaveSettingsEnabledEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.AutoSaveSettingsEnabledEqual(request.AutoSaveSettingsEnabledEqual.Value);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= SystemSettingsAggSettingsSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= SystemSettingsAggSettingsSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= SystemSettingsAggSettingsSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= SystemSettingsAggSettingsSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= SystemSettingsAggSettingsSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= SystemSettingsAggSettingsSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.IdContains(request.IdContains);
					else
						filter &= SystemSettingsAggSettingsSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= SystemSettingsAggSettingsSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= SystemSettingsAggSettingsSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= SystemSettingsAggSettingsSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= SystemSettingsAggSettingsSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= SystemSettingsAggSettingsSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= SystemSettingsAggSettingsSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= SystemSettingsAggSettingsSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
}

