using LazyCrudBuilder.CrossCuting.Infra.Utils.Extensions;
using System.Linq.Expressions;
using LazyCrudBuilder.Core.Domain.Seedwork.Specification;
using LazyCrudBuilder.CrossCuting.Infra.Utils.Extensions;

namespace LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Filters{
	using Entities;
	using Specifications;
	using Queries.Models;
	public static class UserProfileAccessFilters 
	{
	    public static Expression<Func<UserProfileAccess, bool>> GetFilters(this UserProfileAccessQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<UserProfileAccess> GetFiltersSpecification(this UserProfileAccessQueryModel request, bool isOrSpecification = false) 
		{
			Specification<UserProfileAccess> filter = new DirectSpecification<UserProfileAccess>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (!string.IsNullOrWhiteSpace(request.DescriptionEqual)) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DescriptionEqual(request.DescriptionEqual);
					else
						filter &= UserProfileAccessSpecifications.DescriptionEqual(request.DescriptionEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionNotEqual)) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DescriptionNotEqual(request.DescriptionNotEqual);
					else
						filter &= UserProfileAccessSpecifications.DescriptionNotEqual(request.DescriptionNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionContains)) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DescriptionContains(request.DescriptionContains);
					else
						filter &= UserProfileAccessSpecifications.DescriptionContains(request.DescriptionContains);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionStartsWith)) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DescriptionStartsWith(request.DescriptionStartsWith);
					else
						filter &= UserProfileAccessSpecifications.DescriptionStartsWith(request.DescriptionStartsWith);
				}
				if (request.UserProfileIdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UserProfileIdEqual(request.UserProfileIdEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.UserProfileIdEqual(request.UserProfileIdEqual.Value);
				}
				if (request.UserProfileIdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UserProfileIdNotEqual(request.UserProfileIdNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.UserProfileIdNotEqual(request.UserProfileIdNotEqual.Value);
				}
				if (request.UserProfileIdContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UserProfileIdContains(request.UserProfileIdContains);
					else
						filter &= UserProfileAccessSpecifications.UserProfileIdContains(request.UserProfileIdContains);
				}
				if (request.UserProfileIdNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UserProfileIdNotContains(request.UserProfileIdNotContains);
					else
						filter &= UserProfileAccessSpecifications.UserProfileIdNotContains(request.UserProfileIdNotContains);
				}
				if (request.UserProfileIdSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UserProfileIdGreaterThanOrEqual(request.UserProfileIdSince.Value);
					else
						filter &= UserProfileAccessSpecifications.UserProfileIdGreaterThanOrEqual(request.UserProfileIdSince.Value);
				}
				if (request.UserProfileIdUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UserProfileIdLessThan(request.UserProfileIdUntil.Value);
					else
						filter &= UserProfileAccessSpecifications.UserProfileIdLessThan(request.UserProfileIdUntil.Value);
				}
				if (request.UserProfileIdNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UserProfileIdNotEqual(request.UserProfileIdNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.UserProfileIdNotEqual(request.UserProfileIdNotEqual.Value);
				}
				if (request.UserProfileIdLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UserProfileIdLessThan(request.UserProfileIdLessThan.Value);
					else
						filter &= UserProfileAccessSpecifications.UserProfileIdLessThan(request.UserProfileIdLessThan.Value);
				}
				if (request.UserProfileIdGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UserProfileIdGreaterThan(request.UserProfileIdGreaterThan.Value);
					else
						filter &= UserProfileAccessSpecifications.UserProfileIdGreaterThan(request.UserProfileIdGreaterThan.Value);
				}
				if (request.UserProfileIdLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UserProfileIdLessThanOrEqual(request.UserProfileIdLessThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.UserProfileIdLessThanOrEqual(request.UserProfileIdLessThanOrEqual.Value);
				}
				if (request.UserProfileIdGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UserProfileIdGreaterThanOrEqual(request.UserProfileIdGreaterThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.UserProfileIdGreaterThanOrEqual(request.UserProfileIdGreaterThanOrEqual.Value);
				}
				if (request.SystemPanelSubItemIdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelSubItemIdEqual(request.SystemPanelSubItemIdEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelSubItemIdEqual(request.SystemPanelSubItemIdEqual.Value);
				}
				if (request.SystemPanelSubItemIdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelSubItemIdNotEqual(request.SystemPanelSubItemIdNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelSubItemIdNotEqual(request.SystemPanelSubItemIdNotEqual.Value);
				}
				if (request.SystemPanelSubItemIdContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelSubItemIdContains(request.SystemPanelSubItemIdContains);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelSubItemIdContains(request.SystemPanelSubItemIdContains);
				}
				if (request.SystemPanelSubItemIdNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelSubItemIdNotContains(request.SystemPanelSubItemIdNotContains);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelSubItemIdNotContains(request.SystemPanelSubItemIdNotContains);
				}
				if (request.SystemPanelSubItemIdSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelSubItemIdGreaterThanOrEqual(request.SystemPanelSubItemIdSince.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelSubItemIdGreaterThanOrEqual(request.SystemPanelSubItemIdSince.Value);
				}
				if (request.SystemPanelSubItemIdUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelSubItemIdLessThan(request.SystemPanelSubItemIdUntil.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelSubItemIdLessThan(request.SystemPanelSubItemIdUntil.Value);
				}
				if (request.SystemPanelSubItemIdNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelSubItemIdNotEqual(request.SystemPanelSubItemIdNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelSubItemIdNotEqual(request.SystemPanelSubItemIdNotEqual.Value);
				}
				if (request.SystemPanelSubItemIdLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelSubItemIdLessThan(request.SystemPanelSubItemIdLessThan.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelSubItemIdLessThan(request.SystemPanelSubItemIdLessThan.Value);
				}
				if (request.SystemPanelSubItemIdGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelSubItemIdGreaterThan(request.SystemPanelSubItemIdGreaterThan.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelSubItemIdGreaterThan(request.SystemPanelSubItemIdGreaterThan.Value);
				}
				if (request.SystemPanelSubItemIdLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelSubItemIdLessThanOrEqual(request.SystemPanelSubItemIdLessThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelSubItemIdLessThanOrEqual(request.SystemPanelSubItemIdLessThanOrEqual.Value);
				}
				if (request.SystemPanelSubItemIdGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelSubItemIdGreaterThanOrEqual(request.SystemPanelSubItemIdGreaterThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelSubItemIdGreaterThanOrEqual(request.SystemPanelSubItemIdGreaterThanOrEqual.Value);
				}
				if (request.SystemPanelIdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelIdEqual(request.SystemPanelIdEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelIdEqual(request.SystemPanelIdEqual.Value);
				}
				if (request.SystemPanelIdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelIdNotEqual(request.SystemPanelIdNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelIdNotEqual(request.SystemPanelIdNotEqual.Value);
				}
				if (request.SystemPanelIdContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelIdContains(request.SystemPanelIdContains);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelIdContains(request.SystemPanelIdContains);
				}
				if (request.SystemPanelIdNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelIdNotContains(request.SystemPanelIdNotContains);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelIdNotContains(request.SystemPanelIdNotContains);
				}
				if (request.SystemPanelIdSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelIdGreaterThanOrEqual(request.SystemPanelIdSince.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelIdGreaterThanOrEqual(request.SystemPanelIdSince.Value);
				}
				if (request.SystemPanelIdUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelIdLessThan(request.SystemPanelIdUntil.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelIdLessThan(request.SystemPanelIdUntil.Value);
				}
				if (request.SystemPanelIdNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelIdNotEqual(request.SystemPanelIdNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelIdNotEqual(request.SystemPanelIdNotEqual.Value);
				}
				if (request.SystemPanelIdLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelIdLessThan(request.SystemPanelIdLessThan.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelIdLessThan(request.SystemPanelIdLessThan.Value);
				}
				if (request.SystemPanelIdGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelIdGreaterThan(request.SystemPanelIdGreaterThan.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelIdGreaterThan(request.SystemPanelIdGreaterThan.Value);
				}
				if (request.SystemPanelIdLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelIdLessThanOrEqual(request.SystemPanelIdLessThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelIdLessThanOrEqual(request.SystemPanelIdLessThanOrEqual.Value);
				}
				if (request.SystemPanelIdGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelIdGreaterThanOrEqual(request.SystemPanelIdGreaterThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelIdGreaterThanOrEqual(request.SystemPanelIdGreaterThanOrEqual.Value);
				}
				if (request.SystemPanelGroupIdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelGroupIdEqual(request.SystemPanelGroupIdEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelGroupIdEqual(request.SystemPanelGroupIdEqual.Value);
				}
				if (request.SystemPanelGroupIdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelGroupIdNotEqual(request.SystemPanelGroupIdNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelGroupIdNotEqual(request.SystemPanelGroupIdNotEqual.Value);
				}
				if (request.SystemPanelGroupIdContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelGroupIdContains(request.SystemPanelGroupIdContains);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelGroupIdContains(request.SystemPanelGroupIdContains);
				}
				if (request.SystemPanelGroupIdNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelGroupIdNotContains(request.SystemPanelGroupIdNotContains);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelGroupIdNotContains(request.SystemPanelGroupIdNotContains);
				}
				if (request.SystemPanelGroupIdSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelGroupIdGreaterThanOrEqual(request.SystemPanelGroupIdSince.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelGroupIdGreaterThanOrEqual(request.SystemPanelGroupIdSince.Value);
				}
				if (request.SystemPanelGroupIdUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelGroupIdLessThan(request.SystemPanelGroupIdUntil.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelGroupIdLessThan(request.SystemPanelGroupIdUntil.Value);
				}
				if (request.SystemPanelGroupIdNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelGroupIdNotEqual(request.SystemPanelGroupIdNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelGroupIdNotEqual(request.SystemPanelGroupIdNotEqual.Value);
				}
				if (request.SystemPanelGroupIdLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelGroupIdLessThan(request.SystemPanelGroupIdLessThan.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelGroupIdLessThan(request.SystemPanelGroupIdLessThan.Value);
				}
				if (request.SystemPanelGroupIdGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelGroupIdGreaterThan(request.SystemPanelGroupIdGreaterThan.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelGroupIdGreaterThan(request.SystemPanelGroupIdGreaterThan.Value);
				}
				if (request.SystemPanelGroupIdLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelGroupIdLessThanOrEqual(request.SystemPanelGroupIdLessThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelGroupIdLessThanOrEqual(request.SystemPanelGroupIdLessThanOrEqual.Value);
				}
				if (request.SystemPanelGroupIdGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.SystemPanelGroupIdGreaterThanOrEqual(request.SystemPanelGroupIdGreaterThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.SystemPanelGroupIdGreaterThanOrEqual(request.SystemPanelGroupIdGreaterThanOrEqual.Value);
				}
				if (request.IsSubItemEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.IsSubItemEqual(request.IsSubItemEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.IsSubItemEqual(request.IsSubItemEqual.Value);
				}
				if (request.ParentIdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ParentIdEqual(request.ParentIdEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.ParentIdEqual(request.ParentIdEqual.Value);
				}
				if (request.ParentIdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ParentIdNotEqual(request.ParentIdNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.ParentIdNotEqual(request.ParentIdNotEqual.Value);
				}
				if (request.ParentIdContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ParentIdContains(request.ParentIdContains);
					else
						filter &= UserProfileAccessSpecifications.ParentIdContains(request.ParentIdContains);
				}
				if (request.ParentIdNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ParentIdNotContains(request.ParentIdNotContains);
					else
						filter &= UserProfileAccessSpecifications.ParentIdNotContains(request.ParentIdNotContains);
				}
				if (request.ParentIdSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ParentIdGreaterThanOrEqual(request.ParentIdSince.Value);
					else
						filter &= UserProfileAccessSpecifications.ParentIdGreaterThanOrEqual(request.ParentIdSince.Value);
				}
				if (request.ParentIdUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ParentIdLessThan(request.ParentIdUntil.Value);
					else
						filter &= UserProfileAccessSpecifications.ParentIdLessThan(request.ParentIdUntil.Value);
				}
				if (request.ParentIdNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ParentIdNotEqual(request.ParentIdNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.ParentIdNotEqual(request.ParentIdNotEqual.Value);
				}
				if (request.ParentIdLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ParentIdLessThan(request.ParentIdLessThan.Value);
					else
						filter &= UserProfileAccessSpecifications.ParentIdLessThan(request.ParentIdLessThan.Value);
				}
				if (request.ParentIdGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ParentIdGreaterThan(request.ParentIdGreaterThan.Value);
					else
						filter &= UserProfileAccessSpecifications.ParentIdGreaterThan(request.ParentIdGreaterThan.Value);
				}
				if (request.ParentIdLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ParentIdLessThanOrEqual(request.ParentIdLessThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.ParentIdLessThanOrEqual(request.ParentIdLessThanOrEqual.Value);
				}
				if (request.ParentIdGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ParentIdGreaterThanOrEqual(request.ParentIdGreaterThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.ParentIdGreaterThanOrEqual(request.ParentIdGreaterThanOrEqual.Value);
				}
				if (request.IsDirectLinkEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.IsDirectLinkEqual(request.IsDirectLinkEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.IsDirectLinkEqual(request.IsDirectLinkEqual.Value);
				}
				if (request.CanInsertEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CanInsertEqual(request.CanInsertEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.CanInsertEqual(request.CanInsertEqual.Value);
				}
				if (request.CanUpdateEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CanUpdateEqual(request.CanUpdateEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.CanUpdateEqual(request.CanUpdateEqual.Value);
				}
				if (request.CanListEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CanListEqual(request.CanListEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.CanListEqual(request.CanListEqual.Value);
				}
				if (request.CanDeleteEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CanDeleteEqual(request.CanDeleteEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.CanDeleteEqual(request.CanDeleteEqual.Value);
				}
				if (request.CurrentStepEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
				}
				if (request.CurrentStepNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CurrentStepContains(request.CurrentStepContains);
					else
						filter &= UserProfileAccessSpecifications.CurrentStepContains(request.CurrentStepContains);
				}
				if (request.CurrentStepNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
					else
						filter &= UserProfileAccessSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
				}
				if (request.CurrentStepSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
					else
						filter &= UserProfileAccessSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
				}
				if (request.CurrentStepUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
					else
						filter &= UserProfileAccessSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
				}
				if (request.CurrentStepNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
					else
						filter &= UserProfileAccessSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
				}
				if (request.CurrentStepGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
					else
						filter &= UserProfileAccessSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
				}
				if (request.CurrentStepLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
				}
				if (request.CurrentStepGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
				}
				if (request.MaxStepsEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
				}
				if (request.MaxStepsNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.MaxStepsContains(request.MaxStepsContains);
					else
						filter &= UserProfileAccessSpecifications.MaxStepsContains(request.MaxStepsContains);
				}
				if (request.MaxStepsNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
					else
						filter &= UserProfileAccessSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
				}
				if (request.MaxStepsSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
					else
						filter &= UserProfileAccessSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
				}
				if (request.MaxStepsUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
					else
						filter &= UserProfileAccessSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
				}
				if (request.MaxStepsNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
					else
						filter &= UserProfileAccessSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
				}
				if (request.MaxStepsGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
					else
						filter &= UserProfileAccessSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
				}
				if (request.MaxStepsLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
				}
				if (request.MaxStepsGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
				}
				if (request.RegisterDoneEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
				}
				if (request.ActiveEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ActiveEqual(request.ActiveEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.ActiveEqual(request.ActiveEqual.Value);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= UserProfileAccessSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= UserProfileAccessSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= UserProfileAccessSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= UserProfileAccessSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= UserProfileAccessSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= UserProfileAccessSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= UserProfileAccessSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= UserProfileAccessSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= UserProfileAccessSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= UserProfileAccessSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= UserProfileAccessSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= UserProfileAccessSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= UserProfileAccessSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= UserProfileAccessSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= UserProfileAccessSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= UserProfileAccessSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= UserProfileAccessSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= UserProfileAccessSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.IdContains(request.IdContains);
					else
						filter &= UserProfileAccessSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= UserProfileAccessSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= UserProfileAccessSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= UserProfileAccessSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= UserProfileAccessSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= UserProfileAccessSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileAccessSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= UserProfileAccessSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
	public static class UserCurrentAccessSelectedFilters 
	{
	    public static Expression<Func<UserCurrentAccessSelected, bool>> GetFilters(this UserCurrentAccessSelectedQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<UserCurrentAccessSelected> GetFiltersSpecification(this UserCurrentAccessSelectedQueryModel request, bool isOrSpecification = false) 
		{
			Specification<UserCurrentAccessSelected> filter = new DirectSpecification<UserCurrentAccessSelected>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (request.UserProfileIdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UserProfileIdEqual(request.UserProfileIdEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UserProfileIdEqual(request.UserProfileIdEqual.Value);
				}
				if (request.UserProfileIdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UserProfileIdNotEqual(request.UserProfileIdNotEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UserProfileIdNotEqual(request.UserProfileIdNotEqual.Value);
				}
				if (request.UserProfileIdContains != null)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UserProfileIdContains(request.UserProfileIdContains);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UserProfileIdContains(request.UserProfileIdContains);
				}
				if (request.UserProfileIdNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UserProfileIdNotContains(request.UserProfileIdNotContains);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UserProfileIdNotContains(request.UserProfileIdNotContains);
				}
				if (request.UserProfileIdSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UserProfileIdGreaterThanOrEqual(request.UserProfileIdSince.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UserProfileIdGreaterThanOrEqual(request.UserProfileIdSince.Value);
				}
				if (request.UserProfileIdUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UserProfileIdLessThan(request.UserProfileIdUntil.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UserProfileIdLessThan(request.UserProfileIdUntil.Value);
				}
				if (request.UserProfileIdNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UserProfileIdNotEqual(request.UserProfileIdNotEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UserProfileIdNotEqual(request.UserProfileIdNotEqual.Value);
				}
				if (request.UserProfileIdLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UserProfileIdLessThan(request.UserProfileIdLessThan.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UserProfileIdLessThan(request.UserProfileIdLessThan.Value);
				}
				if (request.UserProfileIdGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UserProfileIdGreaterThan(request.UserProfileIdGreaterThan.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UserProfileIdGreaterThan(request.UserProfileIdGreaterThan.Value);
				}
				if (request.UserProfileIdLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UserProfileIdLessThanOrEqual(request.UserProfileIdLessThanOrEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UserProfileIdLessThanOrEqual(request.UserProfileIdLessThanOrEqual.Value);
				}
				if (request.UserProfileIdGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UserProfileIdGreaterThanOrEqual(request.UserProfileIdGreaterThanOrEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UserProfileIdGreaterThanOrEqual(request.UserProfileIdGreaterThanOrEqual.Value);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= UserCurrentAccessSelectedSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= UserCurrentAccessSelectedSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= UserCurrentAccessSelectedSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= UserCurrentAccessSelectedSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.IdContains(request.IdContains);
					else
						filter &= UserCurrentAccessSelectedSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= UserCurrentAccessSelectedSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= UserCurrentAccessSelectedSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= UserCurrentAccessSelectedSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= UserCurrentAccessSelectedSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= UserCurrentAccessSelectedSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserCurrentAccessSelectedSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= UserCurrentAccessSelectedSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
	public static class UserProfileListFilters 
	{
	    public static Expression<Func<UserProfileList, bool>> GetFilters(this UserProfileListQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<UserProfileList> GetFiltersSpecification(this UserProfileListQueryModel request, bool isOrSpecification = false) 
		{
			Specification<UserProfileList> filter = new DirectSpecification<UserProfileList>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (request.UserIdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UserIdEqual(request.UserIdEqual.Value);
					else
						filter &= UserProfileListSpecifications.UserIdEqual(request.UserIdEqual.Value);
				}
				if (request.UserIdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UserIdNotEqual(request.UserIdNotEqual.Value);
					else
						filter &= UserProfileListSpecifications.UserIdNotEqual(request.UserIdNotEqual.Value);
				}
				if (request.UserIdContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UserIdContains(request.UserIdContains);
					else
						filter &= UserProfileListSpecifications.UserIdContains(request.UserIdContains);
				}
				if (request.UserIdNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UserIdNotContains(request.UserIdNotContains);
					else
						filter &= UserProfileListSpecifications.UserIdNotContains(request.UserIdNotContains);
				}
				if (request.UserIdSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UserIdGreaterThanOrEqual(request.UserIdSince.Value);
					else
						filter &= UserProfileListSpecifications.UserIdGreaterThanOrEqual(request.UserIdSince.Value);
				}
				if (request.UserIdUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UserIdLessThan(request.UserIdUntil.Value);
					else
						filter &= UserProfileListSpecifications.UserIdLessThan(request.UserIdUntil.Value);
				}
				if (request.UserIdNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UserIdNotEqual(request.UserIdNotEqual.Value);
					else
						filter &= UserProfileListSpecifications.UserIdNotEqual(request.UserIdNotEqual.Value);
				}
				if (request.UserIdLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UserIdLessThan(request.UserIdLessThan.Value);
					else
						filter &= UserProfileListSpecifications.UserIdLessThan(request.UserIdLessThan.Value);
				}
				if (request.UserIdGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UserIdGreaterThan(request.UserIdGreaterThan.Value);
					else
						filter &= UserProfileListSpecifications.UserIdGreaterThan(request.UserIdGreaterThan.Value);
				}
				if (request.UserIdLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UserIdLessThanOrEqual(request.UserIdLessThanOrEqual.Value);
					else
						filter &= UserProfileListSpecifications.UserIdLessThanOrEqual(request.UserIdLessThanOrEqual.Value);
				}
				if (request.UserIdGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UserIdGreaterThanOrEqual(request.UserIdGreaterThanOrEqual.Value);
					else
						filter &= UserProfileListSpecifications.UserIdGreaterThanOrEqual(request.UserIdGreaterThanOrEqual.Value);
				}
				if (request.CurrentStepEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
					else
						filter &= UserProfileListSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
				}
				if (request.CurrentStepNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= UserProfileListSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CurrentStepContains(request.CurrentStepContains);
					else
						filter &= UserProfileListSpecifications.CurrentStepContains(request.CurrentStepContains);
				}
				if (request.CurrentStepNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
					else
						filter &= UserProfileListSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
				}
				if (request.CurrentStepSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
					else
						filter &= UserProfileListSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
				}
				if (request.CurrentStepUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
					else
						filter &= UserProfileListSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
				}
				if (request.CurrentStepNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= UserProfileListSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
					else
						filter &= UserProfileListSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
				}
				if (request.CurrentStepGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
					else
						filter &= UserProfileListSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
				}
				if (request.CurrentStepLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
					else
						filter &= UserProfileListSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
				}
				if (request.CurrentStepGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
					else
						filter &= UserProfileListSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
				}
				if (request.MaxStepsEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
					else
						filter &= UserProfileListSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
				}
				if (request.MaxStepsNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= UserProfileListSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.MaxStepsContains(request.MaxStepsContains);
					else
						filter &= UserProfileListSpecifications.MaxStepsContains(request.MaxStepsContains);
				}
				if (request.MaxStepsNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
					else
						filter &= UserProfileListSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
				}
				if (request.MaxStepsSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
					else
						filter &= UserProfileListSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
				}
				if (request.MaxStepsUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
					else
						filter &= UserProfileListSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
				}
				if (request.MaxStepsNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= UserProfileListSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
					else
						filter &= UserProfileListSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
				}
				if (request.MaxStepsGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
					else
						filter &= UserProfileListSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
				}
				if (request.MaxStepsLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
					else
						filter &= UserProfileListSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
				}
				if (request.MaxStepsGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
					else
						filter &= UserProfileListSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
				}
				if (request.RegisterDoneEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
					else
						filter &= UserProfileListSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
				}
				if (request.ActiveEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.ActiveEqual(request.ActiveEqual.Value);
					else
						filter &= UserProfileListSpecifications.ActiveEqual(request.ActiveEqual.Value);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= UserProfileListSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= UserProfileListSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= UserProfileListSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= UserProfileListSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= UserProfileListSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= UserProfileListSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= UserProfileListSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= UserProfileListSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= UserProfileListSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= UserProfileListSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= UserProfileListSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= UserProfileListSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= UserProfileListSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= UserProfileListSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= UserProfileListSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= UserProfileListSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= UserProfileListSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= UserProfileListSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= UserProfileListSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= UserProfileListSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= UserProfileListSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= UserProfileListSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= UserProfileListSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= UserProfileListSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= UserProfileListSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= UserProfileListSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= UserProfileListSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= UserProfileListSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= UserProfileListSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= UserProfileListSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= UserProfileListSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= UserProfileListSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= UserProfileListSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= UserProfileListSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= UserProfileListSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.IdContains(request.IdContains);
					else
						filter &= UserProfileListSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= UserProfileListSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= UserProfileListSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= UserProfileListSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= UserProfileListSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= UserProfileListSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileListSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= UserProfileListSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
	public static class UserProfileFilters 
	{
	    public static Expression<Func<UserProfile, bool>> GetFilters(this UserProfileQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<UserProfile> GetFiltersSpecification(this UserProfileQueryModel request, bool isOrSpecification = false) 
		{
			Specification<UserProfile> filter = new DirectSpecification<UserProfile>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (!string.IsNullOrWhiteSpace(request.DescriptionEqual)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DescriptionEqual(request.DescriptionEqual);
					else
						filter &= UserProfileSpecifications.DescriptionEqual(request.DescriptionEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionNotEqual)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DescriptionNotEqual(request.DescriptionNotEqual);
					else
						filter &= UserProfileSpecifications.DescriptionNotEqual(request.DescriptionNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionContains)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DescriptionContains(request.DescriptionContains);
					else
						filter &= UserProfileSpecifications.DescriptionContains(request.DescriptionContains);
				}
				if (!string.IsNullOrWhiteSpace(request.DescriptionStartsWith)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DescriptionStartsWith(request.DescriptionStartsWith);
					else
						filter &= UserProfileSpecifications.DescriptionStartsWith(request.DescriptionStartsWith);
				}
				if (!string.IsNullOrWhiteSpace(request.CodeEqual)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CodeEqual(request.CodeEqual);
					else
						filter &= UserProfileSpecifications.CodeEqual(request.CodeEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.CodeNotEqual)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CodeNotEqual(request.CodeNotEqual);
					else
						filter &= UserProfileSpecifications.CodeNotEqual(request.CodeNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.CodeContains)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CodeContains(request.CodeContains);
					else
						filter &= UserProfileSpecifications.CodeContains(request.CodeContains);
				}
				if (!string.IsNullOrWhiteSpace(request.CodeStartsWith)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CodeStartsWith(request.CodeStartsWith);
					else
						filter &= UserProfileSpecifications.CodeStartsWith(request.CodeStartsWith);
				}
				if (!string.IsNullOrWhiteSpace(request.InitialPageEqual)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.InitialPageEqual(request.InitialPageEqual);
					else
						filter &= UserProfileSpecifications.InitialPageEqual(request.InitialPageEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.InitialPageNotEqual)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.InitialPageNotEqual(request.InitialPageNotEqual);
					else
						filter &= UserProfileSpecifications.InitialPageNotEqual(request.InitialPageNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.InitialPageContains)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.InitialPageContains(request.InitialPageContains);
					else
						filter &= UserProfileSpecifications.InitialPageContains(request.InitialPageContains);
				}
				if (!string.IsNullOrWhiteSpace(request.InitialPageStartsWith)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.InitialPageStartsWith(request.InitialPageStartsWith);
					else
						filter &= UserProfileSpecifications.InitialPageStartsWith(request.InitialPageStartsWith);
				}
				if (request.IsPrivateProfileEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.IsPrivateProfileEqual(request.IsPrivateProfileEqual.Value);
					else
						filter &= UserProfileSpecifications.IsPrivateProfileEqual(request.IsPrivateProfileEqual.Value);
				}
				if (request.CurrentStepEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
					else
						filter &= UserProfileSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
				}
				if (request.CurrentStepNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= UserProfileSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CurrentStepContains(request.CurrentStepContains);
					else
						filter &= UserProfileSpecifications.CurrentStepContains(request.CurrentStepContains);
				}
				if (request.CurrentStepNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
					else
						filter &= UserProfileSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
				}
				if (request.CurrentStepSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
					else
						filter &= UserProfileSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
				}
				if (request.CurrentStepUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
					else
						filter &= UserProfileSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
				}
				if (request.CurrentStepNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= UserProfileSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
					else
						filter &= UserProfileSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
				}
				if (request.CurrentStepGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
					else
						filter &= UserProfileSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
				}
				if (request.CurrentStepLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
					else
						filter &= UserProfileSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
				}
				if (request.CurrentStepGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
					else
						filter &= UserProfileSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
				}
				if (request.MaxStepsEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
					else
						filter &= UserProfileSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
				}
				if (request.MaxStepsNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= UserProfileSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.MaxStepsContains(request.MaxStepsContains);
					else
						filter &= UserProfileSpecifications.MaxStepsContains(request.MaxStepsContains);
				}
				if (request.MaxStepsNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
					else
						filter &= UserProfileSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
				}
				if (request.MaxStepsSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
					else
						filter &= UserProfileSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
				}
				if (request.MaxStepsUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
					else
						filter &= UserProfileSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
				}
				if (request.MaxStepsNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= UserProfileSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
					else
						filter &= UserProfileSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
				}
				if (request.MaxStepsGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
					else
						filter &= UserProfileSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
				}
				if (request.MaxStepsLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
					else
						filter &= UserProfileSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
				}
				if (request.MaxStepsGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
					else
						filter &= UserProfileSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
				}
				if (request.RegisterDoneEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
					else
						filter &= UserProfileSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
				}
				if (request.ActiveEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.ActiveEqual(request.ActiveEqual.Value);
					else
						filter &= UserProfileSpecifications.ActiveEqual(request.ActiveEqual.Value);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= UserProfileSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= UserProfileSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= UserProfileSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= UserProfileSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= UserProfileSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= UserProfileSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= UserProfileSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= UserProfileSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= UserProfileSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= UserProfileSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= UserProfileSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= UserProfileSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= UserProfileSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= UserProfileSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= UserProfileSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= UserProfileSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= UserProfileSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= UserProfileSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= UserProfileSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= UserProfileSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= UserProfileSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= UserProfileSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= UserProfileSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= UserProfileSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= UserProfileSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= UserProfileSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= UserProfileSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= UserProfileSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= UserProfileSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= UserProfileSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= UserProfileSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= UserProfileSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= UserProfileSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= UserProfileSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= UserProfileSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.IdContains(request.IdContains);
					else
						filter &= UserProfileSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= UserProfileSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= UserProfileSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= UserProfileSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= UserProfileSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= UserProfileSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserProfileSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= UserProfileSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
	public static class UsersAggSettingsFilters 
	{
	    public static Expression<Func<UsersAggSettings, bool>> GetFilters(this UsersAggSettingsQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<UsersAggSettings> GetFiltersSpecification(this UsersAggSettingsQueryModel request, bool isOrSpecification = false) 
		{
			Specification<UsersAggSettings> filter = new DirectSpecification<UsersAggSettings>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (request.UserIdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UserIdEqual(request.UserIdEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.UserIdEqual(request.UserIdEqual.Value);
				}
				if (request.UserIdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UserIdNotEqual(request.UserIdNotEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.UserIdNotEqual(request.UserIdNotEqual.Value);
				}
				if (request.UserIdContains != null)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UserIdContains(request.UserIdContains);
					else
						filter &= UsersAggSettingsSpecifications.UserIdContains(request.UserIdContains);
				}
				if (request.UserIdNotContains != null)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UserIdNotContains(request.UserIdNotContains);
					else
						filter &= UsersAggSettingsSpecifications.UserIdNotContains(request.UserIdNotContains);
				}
				if (request.UserIdSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UserIdGreaterThanOrEqual(request.UserIdSince.Value);
					else
						filter &= UsersAggSettingsSpecifications.UserIdGreaterThanOrEqual(request.UserIdSince.Value);
				}
				if (request.UserIdUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UserIdLessThan(request.UserIdUntil.Value);
					else
						filter &= UsersAggSettingsSpecifications.UserIdLessThan(request.UserIdUntil.Value);
				}
				if (request.UserIdNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UserIdNotEqual(request.UserIdNotEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.UserIdNotEqual(request.UserIdNotEqual.Value);
				}
				if (request.UserIdLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UserIdLessThan(request.UserIdLessThan.Value);
					else
						filter &= UsersAggSettingsSpecifications.UserIdLessThan(request.UserIdLessThan.Value);
				}
				if (request.UserIdGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UserIdGreaterThan(request.UserIdGreaterThan.Value);
					else
						filter &= UsersAggSettingsSpecifications.UserIdGreaterThan(request.UserIdGreaterThan.Value);
				}
				if (request.UserIdLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UserIdLessThanOrEqual(request.UserIdLessThanOrEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.UserIdLessThanOrEqual(request.UserIdLessThanOrEqual.Value);
				}
				if (request.UserIdGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UserIdGreaterThanOrEqual(request.UserIdGreaterThanOrEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.UserIdGreaterThanOrEqual(request.UserIdGreaterThanOrEqual.Value);
				}
				if (request.AutoSaveSettingsEnabledEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.AutoSaveSettingsEnabledEqual(request.AutoSaveSettingsEnabledEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.AutoSaveSettingsEnabledEqual(request.AutoSaveSettingsEnabledEqual.Value);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= UsersAggSettingsSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= UsersAggSettingsSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= UsersAggSettingsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= UsersAggSettingsSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= UsersAggSettingsSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= UsersAggSettingsSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= UsersAggSettingsSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= UsersAggSettingsSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= UsersAggSettingsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= UsersAggSettingsSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= UsersAggSettingsSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= UsersAggSettingsSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= UsersAggSettingsSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= UsersAggSettingsSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= UsersAggSettingsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= UsersAggSettingsSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= UsersAggSettingsSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= UsersAggSettingsSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.IdContains(request.IdContains);
					else
						filter &= UsersAggSettingsSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= UsersAggSettingsSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= UsersAggSettingsSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= UsersAggSettingsSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= UsersAggSettingsSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= UsersAggSettingsSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UsersAggSettingsSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= UsersAggSettingsSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
	public static class UserFilters 
	{
	    public static Expression<Func<User, bool>> GetFilters(this UserQueryModel request, bool isOrSpecification = false)

		{ return request.GetFiltersSpecification(isOrSpecification).SatisfiedBy(); }
		public static Specification<User> GetFiltersSpecification(this UserQueryModel request, bool isOrSpecification = false) 
		{
			Specification<User> filter = new DirectSpecification<User>(p => request.IsEmpty() || !isOrSpecification);
			if(request is not null)
			{
				if (!string.IsNullOrWhiteSpace(request.UserNameEqual)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UserNameEqual(request.UserNameEqual);
					else
						filter &= UserSpecifications.UserNameEqual(request.UserNameEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.UserNameNotEqual)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UserNameNotEqual(request.UserNameNotEqual);
					else
						filter &= UserSpecifications.UserNameNotEqual(request.UserNameNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.UserNameContains)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UserNameContains(request.UserNameContains);
					else
						filter &= UserSpecifications.UserNameContains(request.UserNameContains);
				}
				if (!string.IsNullOrWhiteSpace(request.UserNameStartsWith)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UserNameStartsWith(request.UserNameStartsWith);
					else
						filter &= UserSpecifications.UserNameStartsWith(request.UserNameStartsWith);
				}
				if (!string.IsNullOrWhiteSpace(request.NameEqual)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.NameEqual(request.NameEqual);
					else
						filter &= UserSpecifications.NameEqual(request.NameEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.NameNotEqual)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.NameNotEqual(request.NameNotEqual);
					else
						filter &= UserSpecifications.NameNotEqual(request.NameNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.NameContains)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.NameContains(request.NameContains);
					else
						filter &= UserSpecifications.NameContains(request.NameContains);
				}
				if (!string.IsNullOrWhiteSpace(request.NameStartsWith)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.NameStartsWith(request.NameStartsWith);
					else
						filter &= UserSpecifications.NameStartsWith(request.NameStartsWith);
				}
				if (request.BirthDateEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.BirthDateEqual(request.BirthDateEqual.Value);
					else
						filter &= UserSpecifications.BirthDateEqual(request.BirthDateEqual.Value);
				}
				if (request.BirthDateNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.BirthDateNotEqual(request.BirthDateNotEqual.Value);
					else
						filter &= UserSpecifications.BirthDateNotEqual(request.BirthDateNotEqual.Value);
				}
				if (request.BirthDateContains != null)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.BirthDateContains(request.BirthDateContains);
					else
						filter &= UserSpecifications.BirthDateContains(request.BirthDateContains);
				}
				if (request.BirthDateNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.BirthDateNotContains(request.BirthDateNotContains);
					else
						filter &= UserSpecifications.BirthDateNotContains(request.BirthDateNotContains);
				}
				if (request.BirthDateSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.BirthDateGreaterThanOrEqual(request.BirthDateSince.Value);
					else
						filter &= UserSpecifications.BirthDateGreaterThanOrEqual(request.BirthDateSince.Value);
				}
				if (request.BirthDateUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.BirthDateLessThan(request.BirthDateUntil.Value);
					else
						filter &= UserSpecifications.BirthDateLessThan(request.BirthDateUntil.Value);
				}
				if (request.BirthDateNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.BirthDateNotEqual(request.BirthDateNotEqual.Value);
					else
						filter &= UserSpecifications.BirthDateNotEqual(request.BirthDateNotEqual.Value);
				}
				if (request.BirthDateLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.BirthDateLessThan(request.BirthDateLessThan.Value);
					else
						filter &= UserSpecifications.BirthDateLessThan(request.BirthDateLessThan.Value);
				}
				if (request.BirthDateGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.BirthDateGreaterThan(request.BirthDateGreaterThan.Value);
					else
						filter &= UserSpecifications.BirthDateGreaterThan(request.BirthDateGreaterThan.Value);
				}
				if (request.BirthDateLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.BirthDateLessThanOrEqual(request.BirthDateLessThanOrEqual.Value);
					else
						filter &= UserSpecifications.BirthDateLessThanOrEqual(request.BirthDateLessThanOrEqual.Value);
				}
				if (request.BirthDateGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.BirthDateGreaterThanOrEqual(request.BirthDateGreaterThanOrEqual.Value);
					else
						filter &= UserSpecifications.BirthDateGreaterThanOrEqual(request.BirthDateGreaterThanOrEqual.Value);
				}
				if (request.GenderEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.GenderEqual(request.GenderEqual.Value);
					else
						filter &= UserSpecifications.GenderEqual(request.GenderEqual.Value);
				}
				if (request.NeedResetPasswordEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.NeedResetPasswordEqual(request.NeedResetPasswordEqual.Value);
					else
						filter &= UserSpecifications.NeedResetPasswordEqual(request.NeedResetPasswordEqual.Value);
				}
				if (request.NeedSendOnboardingMailEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.NeedSendOnboardingMailEqual(request.NeedSendOnboardingMailEqual.Value);
					else
						filter &= UserSpecifications.NeedSendOnboardingMailEqual(request.NeedSendOnboardingMailEqual.Value);
				}
				if (!string.IsNullOrWhiteSpace(request.ProfilePictureEqual)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.ProfilePictureEqual(request.ProfilePictureEqual);
					else
						filter &= UserSpecifications.ProfilePictureEqual(request.ProfilePictureEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ProfilePictureNotEqual)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.ProfilePictureNotEqual(request.ProfilePictureNotEqual);
					else
						filter &= UserSpecifications.ProfilePictureNotEqual(request.ProfilePictureNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ProfilePictureContains)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.ProfilePictureContains(request.ProfilePictureContains);
					else
						filter &= UserSpecifications.ProfilePictureContains(request.ProfilePictureContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ProfilePictureStartsWith)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.ProfilePictureStartsWith(request.ProfilePictureStartsWith);
					else
						filter &= UserSpecifications.ProfilePictureStartsWith(request.ProfilePictureStartsWith);
				}
				if (request.CanUpdatePasswordEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CanUpdatePasswordEqual(request.CanUpdatePasswordEqual.Value);
					else
						filter &= UserSpecifications.CanUpdatePasswordEqual(request.CanUpdatePasswordEqual.Value);
				}
				if (request.CurrentStepEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
					else
						filter &= UserSpecifications.CurrentStepEqual(request.CurrentStepEqual.Value);
				}
				if (request.CurrentStepNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= UserSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepContains != null)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CurrentStepContains(request.CurrentStepContains);
					else
						filter &= UserSpecifications.CurrentStepContains(request.CurrentStepContains);
				}
				if (request.CurrentStepNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
					else
						filter &= UserSpecifications.CurrentStepNotContains(request.CurrentStepNotContains);
				}
				if (request.CurrentStepSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
					else
						filter &= UserSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepSince.Value);
				}
				if (request.CurrentStepUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
					else
						filter &= UserSpecifications.CurrentStepLessThan(request.CurrentStepUntil.Value);
				}
				if (request.CurrentStepNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
					else
						filter &= UserSpecifications.CurrentStepNotEqual(request.CurrentStepNotEqual.Value);
				}
				if (request.CurrentStepLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
					else
						filter &= UserSpecifications.CurrentStepLessThan(request.CurrentStepLessThan.Value);
				}
				if (request.CurrentStepGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
					else
						filter &= UserSpecifications.CurrentStepGreaterThan(request.CurrentStepGreaterThan.Value);
				}
				if (request.CurrentStepLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
					else
						filter &= UserSpecifications.CurrentStepLessThanOrEqual(request.CurrentStepLessThanOrEqual.Value);
				}
				if (request.CurrentStepGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
					else
						filter &= UserSpecifications.CurrentStepGreaterThanOrEqual(request.CurrentStepGreaterThanOrEqual.Value);
				}
				if (request.MaxStepsEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
					else
						filter &= UserSpecifications.MaxStepsEqual(request.MaxStepsEqual.Value);
				}
				if (request.MaxStepsNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= UserSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsContains != null)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.MaxStepsContains(request.MaxStepsContains);
					else
						filter &= UserSpecifications.MaxStepsContains(request.MaxStepsContains);
				}
				if (request.MaxStepsNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
					else
						filter &= UserSpecifications.MaxStepsNotContains(request.MaxStepsNotContains);
				}
				if (request.MaxStepsSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
					else
						filter &= UserSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsSince.Value);
				}
				if (request.MaxStepsUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
					else
						filter &= UserSpecifications.MaxStepsLessThan(request.MaxStepsUntil.Value);
				}
				if (request.MaxStepsNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
					else
						filter &= UserSpecifications.MaxStepsNotEqual(request.MaxStepsNotEqual.Value);
				}
				if (request.MaxStepsLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
					else
						filter &= UserSpecifications.MaxStepsLessThan(request.MaxStepsLessThan.Value);
				}
				if (request.MaxStepsGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
					else
						filter &= UserSpecifications.MaxStepsGreaterThan(request.MaxStepsGreaterThan.Value);
				}
				if (request.MaxStepsLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
					else
						filter &= UserSpecifications.MaxStepsLessThanOrEqual(request.MaxStepsLessThanOrEqual.Value);
				}
				if (request.MaxStepsGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
					else
						filter &= UserSpecifications.MaxStepsGreaterThanOrEqual(request.MaxStepsGreaterThanOrEqual.Value);
				}
				if (request.RegisterDoneEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
					else
						filter &= UserSpecifications.RegisterDoneEqual(request.RegisterDoneEqual.Value);
				}
				if (request.ActiveEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.ActiveEqual(request.ActiveEqual.Value);
					else
						filter &= UserSpecifications.ActiveEqual(request.ActiveEqual.Value);
				}
				if (request.CreatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
					else
						filter &= UserSpecifications.CreatedAtEqual(request.CreatedAtEqual.Value);
				}
				if (request.CreatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= UserSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CreatedAtContains(request.CreatedAtContains);
					else
						filter &= UserSpecifications.CreatedAtContains(request.CreatedAtContains);
				}
				if (request.CreatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
					else
						filter &= UserSpecifications.CreatedAtNotContains(request.CreatedAtNotContains);
				}
				if (request.CreatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
					else
						filter &= UserSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtSince.Value);
				}
				if (request.CreatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
					else
						filter &= UserSpecifications.CreatedAtLessThan(request.CreatedAtUntil.Value);
				}
				if (request.CreatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
					else
						filter &= UserSpecifications.CreatedAtNotEqual(request.CreatedAtNotEqual.Value);
				}
				if (request.CreatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
					else
						filter &= UserSpecifications.CreatedAtLessThan(request.CreatedAtLessThan.Value);
				}
				if (request.CreatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
					else
						filter &= UserSpecifications.CreatedAtGreaterThan(request.CreatedAtGreaterThan.Value);
				}
				if (request.CreatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
					else
						filter &= UserSpecifications.CreatedAtLessThanOrEqual(request.CreatedAtLessThanOrEqual.Value);
				}
				if (request.CreatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
					else
						filter &= UserSpecifications.CreatedAtGreaterThanOrEqual(request.CreatedAtGreaterThanOrEqual.Value);
				}
				if (request.UpdatedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
					else
						filter &= UserSpecifications.UpdatedAtEqual(request.UpdatedAtEqual.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= UserSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UpdatedAtContains(request.UpdatedAtContains);
					else
						filter &= UserSpecifications.UpdatedAtContains(request.UpdatedAtContains);
				}
				if (request.UpdatedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
					else
						filter &= UserSpecifications.UpdatedAtNotContains(request.UpdatedAtNotContains);
				}
				if (request.UpdatedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
					else
						filter &= UserSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtSince.Value);
				}
				if (request.UpdatedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
					else
						filter &= UserSpecifications.UpdatedAtLessThan(request.UpdatedAtUntil.Value);
				}
				if (request.UpdatedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
					else
						filter &= UserSpecifications.UpdatedAtNotEqual(request.UpdatedAtNotEqual.Value);
				}
				if (request.UpdatedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
					else
						filter &= UserSpecifications.UpdatedAtLessThan(request.UpdatedAtLessThan.Value);
				}
				if (request.UpdatedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
					else
						filter &= UserSpecifications.UpdatedAtGreaterThan(request.UpdatedAtGreaterThan.Value);
				}
				if (request.UpdatedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
					else
						filter &= UserSpecifications.UpdatedAtLessThanOrEqual(request.UpdatedAtLessThanOrEqual.Value);
				}
				if (request.UpdatedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
					else
						filter &= UserSpecifications.UpdatedAtGreaterThanOrEqual(request.UpdatedAtGreaterThanOrEqual.Value);
				}
				if (request.DeletedAtEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
					else
						filter &= UserSpecifications.DeletedAtEqual(request.DeletedAtEqual.Value);
				}
				if (request.DeletedAtNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= UserSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtContains != null)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.DeletedAtContains(request.DeletedAtContains);
					else
						filter &= UserSpecifications.DeletedAtContains(request.DeletedAtContains);
				}
				if (request.DeletedAtNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
					else
						filter &= UserSpecifications.DeletedAtNotContains(request.DeletedAtNotContains);
				}
				if (request.DeletedAtSince.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
					else
						filter &= UserSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtSince.Value);
				}
				if (request.DeletedAtUntil.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
					else
						filter &= UserSpecifications.DeletedAtLessThan(request.DeletedAtUntil.Value);
				}
				if (request.DeletedAtNotEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
					else
						filter &= UserSpecifications.DeletedAtNotEqual(request.DeletedAtNotEqual.Value);
				}
				if (request.DeletedAtLessThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
					else
						filter &= UserSpecifications.DeletedAtLessThan(request.DeletedAtLessThan.Value);
				}
				if (request.DeletedAtGreaterThan.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
					else
						filter &= UserSpecifications.DeletedAtGreaterThan(request.DeletedAtGreaterThan.Value);
				}
				if (request.DeletedAtLessThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
					else
						filter &= UserSpecifications.DeletedAtLessThanOrEqual(request.DeletedAtLessThanOrEqual.Value);
				}
				if (request.DeletedAtGreaterThanOrEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
					else
						filter &= UserSpecifications.DeletedAtGreaterThanOrEqual(request.DeletedAtGreaterThanOrEqual.Value);
				}
				if (request.IdEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.IdEqual(request.IdEqual.Value);
					else
						filter &= UserSpecifications.IdEqual(request.IdEqual.Value);
				}
				if (request.IdNotEqual.HasValue)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.IdNotEqual(request.IdNotEqual.Value);
					else
						filter &= UserSpecifications.IdNotEqual(request.IdNotEqual.Value);
				}
				if (request.IdContains != null)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.IdContains(request.IdContains);
					else
						filter &= UserSpecifications.IdContains(request.IdContains);
				}
				if (request.IdNotContains != null)
				{
					if (isOrSpecification)
						filter |= UserSpecifications.IdNotContains(request.IdNotContains);
					else
						filter &= UserSpecifications.IdNotContains(request.IdNotContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdEqual)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.ExternalIdEqual(request.ExternalIdEqual);
					else
						filter &= UserSpecifications.ExternalIdEqual(request.ExternalIdEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdNotEqual)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
					else
						filter &= UserSpecifications.ExternalIdNotEqual(request.ExternalIdNotEqual);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdContains)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.ExternalIdContains(request.ExternalIdContains);
					else
						filter &= UserSpecifications.ExternalIdContains(request.ExternalIdContains);
				}
				if (!string.IsNullOrWhiteSpace(request.ExternalIdStartsWith)) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
					else
						filter &= UserSpecifications.ExternalIdStartsWith(request.ExternalIdStartsWith);
				}
				if (request.IsDeletedEqual.HasValue) 
				{
					if (isOrSpecification)
						filter |= UserSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
					else
						filter &= UserSpecifications.IsDeletedEqual(request.IsDeletedEqual.Value);
				}
			}
			return filter;
		}
	}
}

