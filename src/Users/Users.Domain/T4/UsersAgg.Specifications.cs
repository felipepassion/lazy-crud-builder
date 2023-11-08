using LazyCrud.Core.Domain.Seedwork.Specification;
using Microsoft.EntityFrameworkCore;
namespace LazyCrud.Users.Domain.Aggregates.UsersAgg.Specifications {
	using Entities;
   public partial class UserProfileAccessSpecifications {
				public static Specification<UserProfileAccess> DescriptionContains(string value) {
			return new DirectSpecification<UserProfileAccess>(p => EF.Functions.Like(p.Description.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<UserProfileAccess> DescriptionStartsWith(string value) {
			return new DirectSpecification<UserProfileAccess>(p => EF.Functions.Like(p.Description.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<UserProfileAccess> DescriptionEqual(params string[] values) {
			return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.Description));
		}
		public static Specification<UserProfileAccess> DescriptionNotEqual(string value) {
			return new DirectSpecification<UserProfileAccess>(p => p.Description != value);
		}
		public static Specification<UserProfileAccess> DescriptionIsNull() {
            return new DirectSpecification<UserProfileAccess>(p => p.Description == null);
        }
		public static Specification<UserProfileAccess> DescriptionIsNotNull() {
            return new DirectSpecification<UserProfileAccess>(p => p.Description != null);
        }
		
					public static Specification<UserProfileAccess> UserProfileIdContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.UserProfileId));
        }
		public static Specification<UserProfileAccess> UserProfileIdNotContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => !values.Contains(p.UserProfileId));
        }
		public static Specification<UserProfileAccess> UserProfileIdEqual(params int[] values) {
			return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.UserProfileId));
        }
        public static Specification<UserProfileAccess> UserProfileIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.UserProfileId >= value);
        }
        public static Specification<UserProfileAccess> UserProfileIdLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.UserProfileId <= value);
        }
        public static Specification<UserProfileAccess> UserProfileIdNotEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.UserProfileId != value);
        }
        public static Specification<UserProfileAccess> UserProfileIdGreaterThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.UserProfileId > value);
        }
        public static Specification<UserProfileAccess> UserProfileIdLessThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.UserProfileId < value);
        }
		
					public static Specification<UserProfileAccess> SystemPanelSubItemIdContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.SystemPanelSubItemId.Value));
        }
		public static Specification<UserProfileAccess> SystemPanelSubItemIdNotContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => !values.Contains(p.SystemPanelSubItemId.Value));
        }
		public static Specification<UserProfileAccess> SystemPanelSubItemIdEqual(params int[] values) {
			return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.SystemPanelSubItemId.Value));
        }
        public static Specification<UserProfileAccess> SystemPanelSubItemIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelSubItemId >= value);
        }
        public static Specification<UserProfileAccess> SystemPanelSubItemIdLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelSubItemId <= value);
        }
        public static Specification<UserProfileAccess> SystemPanelSubItemIdNotEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelSubItemId != value);
        }
        public static Specification<UserProfileAccess> SystemPanelSubItemIdGreaterThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelSubItemId > value);
        }
        public static Specification<UserProfileAccess> SystemPanelSubItemIdLessThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelSubItemId < value);
        }
		
					public static Specification<UserProfileAccess> SystemPanelIdContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.SystemPanelId.Value));
        }
		public static Specification<UserProfileAccess> SystemPanelIdNotContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => !values.Contains(p.SystemPanelId.Value));
        }
		public static Specification<UserProfileAccess> SystemPanelIdEqual(params int[] values) {
			return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.SystemPanelId.Value));
        }
        public static Specification<UserProfileAccess> SystemPanelIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelId >= value);
        }
        public static Specification<UserProfileAccess> SystemPanelIdLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelId <= value);
        }
        public static Specification<UserProfileAccess> SystemPanelIdNotEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelId != value);
        }
        public static Specification<UserProfileAccess> SystemPanelIdGreaterThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelId > value);
        }
        public static Specification<UserProfileAccess> SystemPanelIdLessThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelId < value);
        }
		
					public static Specification<UserProfileAccess> SystemPanelGroupIdContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.SystemPanelGroupId.Value));
        }
		public static Specification<UserProfileAccess> SystemPanelGroupIdNotContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => !values.Contains(p.SystemPanelGroupId.Value));
        }
		public static Specification<UserProfileAccess> SystemPanelGroupIdEqual(params int[] values) {
			return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.SystemPanelGroupId.Value));
        }
        public static Specification<UserProfileAccess> SystemPanelGroupIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelGroupId >= value);
        }
        public static Specification<UserProfileAccess> SystemPanelGroupIdLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelGroupId <= value);
        }
        public static Specification<UserProfileAccess> SystemPanelGroupIdNotEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelGroupId != value);
        }
        public static Specification<UserProfileAccess> SystemPanelGroupIdGreaterThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelGroupId > value);
        }
        public static Specification<UserProfileAccess> SystemPanelGroupIdLessThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.SystemPanelGroupId < value);
        }
		
					public static Specification<UserProfileAccess> IsSubItemEqual(bool value) {
			return new DirectSpecification<UserProfileAccess>(p => p.IsSubItem == value);
		}
		
					public static Specification<UserProfileAccess> ParentIdContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.ParentId.Value));
        }
		public static Specification<UserProfileAccess> ParentIdNotContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => !values.Contains(p.ParentId.Value));
        }
		public static Specification<UserProfileAccess> ParentIdEqual(params int[] values) {
			return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.ParentId.Value));
        }
        public static Specification<UserProfileAccess> ParentIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.ParentId >= value);
        }
        public static Specification<UserProfileAccess> ParentIdLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.ParentId <= value);
        }
        public static Specification<UserProfileAccess> ParentIdNotEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.ParentId != value);
        }
        public static Specification<UserProfileAccess> ParentIdGreaterThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.ParentId > value);
        }
        public static Specification<UserProfileAccess> ParentIdLessThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.ParentId < value);
        }
		
					public static Specification<UserProfileAccess> IsDirectLinkEqual(bool value) {
			return new DirectSpecification<UserProfileAccess>(p => p.IsDirectLink == value);
		}
		
					public static Specification<UserProfileAccess> CanInsertEqual(bool value) {
			return new DirectSpecification<UserProfileAccess>(p => p.CanInsert == value);
		}
		
					public static Specification<UserProfileAccess> CanUpdateEqual(bool value) {
			return new DirectSpecification<UserProfileAccess>(p => p.CanUpdate == value);
		}
		
					public static Specification<UserProfileAccess> CanListEqual(bool value) {
			return new DirectSpecification<UserProfileAccess>(p => p.CanList == value);
		}
		
					public static Specification<UserProfileAccess> CanDeleteEqual(bool value) {
			return new DirectSpecification<UserProfileAccess>(p => p.CanDelete == value);
		}
		
					public static Specification<UserProfileAccess> CurrentStepContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.CurrentStep));
        }
		public static Specification<UserProfileAccess> CurrentStepNotContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => !values.Contains(p.CurrentStep));
        }
		public static Specification<UserProfileAccess> CurrentStepEqual(params int[] values) {
			return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.CurrentStep));
        }
        public static Specification<UserProfileAccess> CurrentStepGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.CurrentStep >= value);
        }
        public static Specification<UserProfileAccess> CurrentStepLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.CurrentStep <= value);
        }
        public static Specification<UserProfileAccess> CurrentStepNotEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.CurrentStep != value);
        }
        public static Specification<UserProfileAccess> CurrentStepGreaterThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.CurrentStep > value);
        }
        public static Specification<UserProfileAccess> CurrentStepLessThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.CurrentStep < value);
        }
		
					public static Specification<UserProfileAccess> MaxStepsContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.MaxSteps.Value));
        }
		public static Specification<UserProfileAccess> MaxStepsNotContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => !values.Contains(p.MaxSteps.Value));
        }
		public static Specification<UserProfileAccess> MaxStepsEqual(params int[] values) {
			return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.MaxSteps.Value));
        }
        public static Specification<UserProfileAccess> MaxStepsGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.MaxSteps >= value);
        }
        public static Specification<UserProfileAccess> MaxStepsLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.MaxSteps <= value);
        }
        public static Specification<UserProfileAccess> MaxStepsNotEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.MaxSteps != value);
        }
        public static Specification<UserProfileAccess> MaxStepsGreaterThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.MaxSteps > value);
        }
        public static Specification<UserProfileAccess> MaxStepsLessThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.MaxSteps < value);
        }
		
					public static Specification<UserProfileAccess> RegisterDoneEqual(bool value) {
			return new DirectSpecification<UserProfileAccess>(p => p.RegisterDone == value);
		}
		public static Specification<UserProfileAccess> RegisterDoneIsNull() {
            return new DirectSpecification<UserProfileAccess>(p => p.RegisterDone == null);
        }
		public static Specification<UserProfileAccess> RegisterDoneIsNotNull() {
            return new DirectSpecification<UserProfileAccess>(p => p.RegisterDone != null);
        }
		
					public static Specification<UserProfileAccess> ActiveEqual(bool value) {
			return new DirectSpecification<UserProfileAccess>(p => p.Active == value);
		}
		public static Specification<UserProfileAccess> ActiveIsNull() {
            return new DirectSpecification<UserProfileAccess>(p => p.Active == null);
        }
		public static Specification<UserProfileAccess> ActiveIsNotNull() {
            return new DirectSpecification<UserProfileAccess>(p => p.Active != null);
        }
		
					public static Specification<UserProfileAccess> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<UserProfileAccess> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfileAccess>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<UserProfileAccess> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<UserProfileAccess> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.CreatedAt >= value);
        }
        public static Specification<UserProfileAccess> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.CreatedAt <= value);
        }
        public static Specification<UserProfileAccess> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.CreatedAt != value);
        }
        public static Specification<UserProfileAccess> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.CreatedAt > value);
        }
        public static Specification<UserProfileAccess> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.CreatedAt < value);
        }
		
					public static Specification<UserProfileAccess> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<UserProfileAccess> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfileAccess>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<UserProfileAccess> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<UserProfileAccess> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.UpdatedAt >= value);
        }
        public static Specification<UserProfileAccess> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.UpdatedAt <= value);
        }
        public static Specification<UserProfileAccess> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.UpdatedAt != value);
        }
        public static Specification<UserProfileAccess> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.UpdatedAt > value);
        }
        public static Specification<UserProfileAccess> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.UpdatedAt < value);
        }
		
					public static Specification<UserProfileAccess> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<UserProfileAccess> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfileAccess>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<UserProfileAccess> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<UserProfileAccess> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.DeletedAt >= value);
        }
        public static Specification<UserProfileAccess> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.DeletedAt <= value);
        }
        public static Specification<UserProfileAccess> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.DeletedAt != value);
        }
        public static Specification<UserProfileAccess> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.DeletedAt > value);
        }
        public static Specification<UserProfileAccess> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UserProfileAccess>(p => p.DeletedAt < value);
        }
		
					public static Specification<UserProfileAccess> IdContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.Id));
        }
		public static Specification<UserProfileAccess> IdNotContains(params int[] values) {
            return new DirectSpecification<UserProfileAccess>(p => !values.Contains(p.Id));
        }
		public static Specification<UserProfileAccess> IdEqual(params int[] values) {
			return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.Id));
        }
        public static Specification<UserProfileAccess> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.Id >= value);
        }
        public static Specification<UserProfileAccess> IdLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.Id <= value);
        }
        public static Specification<UserProfileAccess> IdNotEqual(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.Id != value);
        }
        public static Specification<UserProfileAccess> IdGreaterThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.Id > value);
        }
        public static Specification<UserProfileAccess> IdLessThan(int value) {
            return new DirectSpecification<UserProfileAccess>(p => p.Id < value);
        }
		
					public static Specification<UserProfileAccess> ExternalIdContains(string value) {
			return new DirectSpecification<UserProfileAccess>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<UserProfileAccess> ExternalIdStartsWith(string value) {
			return new DirectSpecification<UserProfileAccess>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<UserProfileAccess> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<UserProfileAccess>(p => values.Contains(p.ExternalId));
		}
		public static Specification<UserProfileAccess> ExternalIdNotEqual(string value) {
			return new DirectSpecification<UserProfileAccess>(p => p.ExternalId != value);
		}
		public static Specification<UserProfileAccess> ExternalIdIsNull() {
            return new DirectSpecification<UserProfileAccess>(p => p.ExternalId == null);
        }
		public static Specification<UserProfileAccess> ExternalIdIsNotNull() {
            return new DirectSpecification<UserProfileAccess>(p => p.ExternalId != null);
        }
		
					public static Specification<UserProfileAccess> IsDeletedEqual(bool value) {
			return new DirectSpecification<UserProfileAccess>(p => p.IsDeleted == value);
		}
		
	   }
   public partial class UserCurrentAccessSelectedSpecifications {
				public static Specification<UserCurrentAccessSelected> UserProfileIdContains(params int[] values) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => values.Contains(p.UserProfileId.Value));
        }
		public static Specification<UserCurrentAccessSelected> UserProfileIdNotContains(params int[] values) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => !values.Contains(p.UserProfileId.Value));
        }
		public static Specification<UserCurrentAccessSelected> UserProfileIdEqual(params int[] values) {
			return new DirectSpecification<UserCurrentAccessSelected>(p => values.Contains(p.UserProfileId.Value));
        }
        public static Specification<UserCurrentAccessSelected> UserProfileIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.UserProfileId >= value);
        }
        public static Specification<UserCurrentAccessSelected> UserProfileIdLessThanOrEqual(int value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.UserProfileId <= value);
        }
        public static Specification<UserCurrentAccessSelected> UserProfileIdNotEqual(int value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.UserProfileId != value);
        }
        public static Specification<UserCurrentAccessSelected> UserProfileIdGreaterThan(int value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.UserProfileId > value);
        }
        public static Specification<UserCurrentAccessSelected> UserProfileIdLessThan(int value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.UserProfileId < value);
        }
		
					public static Specification<UserCurrentAccessSelected> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<UserCurrentAccessSelected> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<UserCurrentAccessSelected> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UserCurrentAccessSelected>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<UserCurrentAccessSelected> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.CreatedAt >= value);
        }
        public static Specification<UserCurrentAccessSelected> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.CreatedAt <= value);
        }
        public static Specification<UserCurrentAccessSelected> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.CreatedAt != value);
        }
        public static Specification<UserCurrentAccessSelected> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.CreatedAt > value);
        }
        public static Specification<UserCurrentAccessSelected> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.CreatedAt < value);
        }
		
					public static Specification<UserCurrentAccessSelected> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<UserCurrentAccessSelected> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<UserCurrentAccessSelected> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UserCurrentAccessSelected>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<UserCurrentAccessSelected> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.UpdatedAt >= value);
        }
        public static Specification<UserCurrentAccessSelected> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.UpdatedAt <= value);
        }
        public static Specification<UserCurrentAccessSelected> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.UpdatedAt != value);
        }
        public static Specification<UserCurrentAccessSelected> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.UpdatedAt > value);
        }
        public static Specification<UserCurrentAccessSelected> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.UpdatedAt < value);
        }
		
					public static Specification<UserCurrentAccessSelected> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<UserCurrentAccessSelected> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<UserCurrentAccessSelected> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UserCurrentAccessSelected>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<UserCurrentAccessSelected> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.DeletedAt >= value);
        }
        public static Specification<UserCurrentAccessSelected> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.DeletedAt <= value);
        }
        public static Specification<UserCurrentAccessSelected> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.DeletedAt != value);
        }
        public static Specification<UserCurrentAccessSelected> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.DeletedAt > value);
        }
        public static Specification<UserCurrentAccessSelected> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.DeletedAt < value);
        }
		
					public static Specification<UserCurrentAccessSelected> IdContains(params int[] values) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => values.Contains(p.Id));
        }
		public static Specification<UserCurrentAccessSelected> IdNotContains(params int[] values) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => !values.Contains(p.Id));
        }
		public static Specification<UserCurrentAccessSelected> IdEqual(params int[] values) {
			return new DirectSpecification<UserCurrentAccessSelected>(p => values.Contains(p.Id));
        }
        public static Specification<UserCurrentAccessSelected> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.Id >= value);
        }
        public static Specification<UserCurrentAccessSelected> IdLessThanOrEqual(int value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.Id <= value);
        }
        public static Specification<UserCurrentAccessSelected> IdNotEqual(int value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.Id != value);
        }
        public static Specification<UserCurrentAccessSelected> IdGreaterThan(int value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.Id > value);
        }
        public static Specification<UserCurrentAccessSelected> IdLessThan(int value) {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.Id < value);
        }
		
					public static Specification<UserCurrentAccessSelected> ExternalIdContains(string value) {
			return new DirectSpecification<UserCurrentAccessSelected>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<UserCurrentAccessSelected> ExternalIdStartsWith(string value) {
			return new DirectSpecification<UserCurrentAccessSelected>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<UserCurrentAccessSelected> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<UserCurrentAccessSelected>(p => values.Contains(p.ExternalId));
		}
		public static Specification<UserCurrentAccessSelected> ExternalIdNotEqual(string value) {
			return new DirectSpecification<UserCurrentAccessSelected>(p => p.ExternalId != value);
		}
		public static Specification<UserCurrentAccessSelected> ExternalIdIsNull() {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.ExternalId == null);
        }
		public static Specification<UserCurrentAccessSelected> ExternalIdIsNotNull() {
            return new DirectSpecification<UserCurrentAccessSelected>(p => p.ExternalId != null);
        }
		
					public static Specification<UserCurrentAccessSelected> IsDeletedEqual(bool value) {
			return new DirectSpecification<UserCurrentAccessSelected>(p => p.IsDeleted == value);
		}
		
	   }
   public partial class UserProfileListSpecifications {
				public static Specification<UserProfileList> UserIdContains(params int[] values) {
            return new DirectSpecification<UserProfileList>(p => values.Contains(p.UserId.Value));
        }
		public static Specification<UserProfileList> UserIdNotContains(params int[] values) {
            return new DirectSpecification<UserProfileList>(p => !values.Contains(p.UserId.Value));
        }
		public static Specification<UserProfileList> UserIdEqual(params int[] values) {
			return new DirectSpecification<UserProfileList>(p => values.Contains(p.UserId.Value));
        }
        public static Specification<UserProfileList> UserIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfileList>(p => p.UserId >= value);
        }
        public static Specification<UserProfileList> UserIdLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfileList>(p => p.UserId <= value);
        }
        public static Specification<UserProfileList> UserIdNotEqual(int value) {
            return new DirectSpecification<UserProfileList>(p => p.UserId != value);
        }
        public static Specification<UserProfileList> UserIdGreaterThan(int value) {
            return new DirectSpecification<UserProfileList>(p => p.UserId > value);
        }
        public static Specification<UserProfileList> UserIdLessThan(int value) {
            return new DirectSpecification<UserProfileList>(p => p.UserId < value);
        }
		
				
					public static Specification<UserProfileList> CurrentStepContains(params int[] values) {
            return new DirectSpecification<UserProfileList>(p => values.Contains(p.CurrentStep));
        }
		public static Specification<UserProfileList> CurrentStepNotContains(params int[] values) {
            return new DirectSpecification<UserProfileList>(p => !values.Contains(p.CurrentStep));
        }
		public static Specification<UserProfileList> CurrentStepEqual(params int[] values) {
			return new DirectSpecification<UserProfileList>(p => values.Contains(p.CurrentStep));
        }
        public static Specification<UserProfileList> CurrentStepGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfileList>(p => p.CurrentStep >= value);
        }
        public static Specification<UserProfileList> CurrentStepLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfileList>(p => p.CurrentStep <= value);
        }
        public static Specification<UserProfileList> CurrentStepNotEqual(int value) {
            return new DirectSpecification<UserProfileList>(p => p.CurrentStep != value);
        }
        public static Specification<UserProfileList> CurrentStepGreaterThan(int value) {
            return new DirectSpecification<UserProfileList>(p => p.CurrentStep > value);
        }
        public static Specification<UserProfileList> CurrentStepLessThan(int value) {
            return new DirectSpecification<UserProfileList>(p => p.CurrentStep < value);
        }
		
					public static Specification<UserProfileList> MaxStepsContains(params int[] values) {
            return new DirectSpecification<UserProfileList>(p => values.Contains(p.MaxSteps.Value));
        }
		public static Specification<UserProfileList> MaxStepsNotContains(params int[] values) {
            return new DirectSpecification<UserProfileList>(p => !values.Contains(p.MaxSteps.Value));
        }
		public static Specification<UserProfileList> MaxStepsEqual(params int[] values) {
			return new DirectSpecification<UserProfileList>(p => values.Contains(p.MaxSteps.Value));
        }
        public static Specification<UserProfileList> MaxStepsGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfileList>(p => p.MaxSteps >= value);
        }
        public static Specification<UserProfileList> MaxStepsLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfileList>(p => p.MaxSteps <= value);
        }
        public static Specification<UserProfileList> MaxStepsNotEqual(int value) {
            return new DirectSpecification<UserProfileList>(p => p.MaxSteps != value);
        }
        public static Specification<UserProfileList> MaxStepsGreaterThan(int value) {
            return new DirectSpecification<UserProfileList>(p => p.MaxSteps > value);
        }
        public static Specification<UserProfileList> MaxStepsLessThan(int value) {
            return new DirectSpecification<UserProfileList>(p => p.MaxSteps < value);
        }
		
					public static Specification<UserProfileList> RegisterDoneEqual(bool value) {
			return new DirectSpecification<UserProfileList>(p => p.RegisterDone == value);
		}
		public static Specification<UserProfileList> RegisterDoneIsNull() {
            return new DirectSpecification<UserProfileList>(p => p.RegisterDone == null);
        }
		public static Specification<UserProfileList> RegisterDoneIsNotNull() {
            return new DirectSpecification<UserProfileList>(p => p.RegisterDone != null);
        }
		
					public static Specification<UserProfileList> ActiveEqual(bool value) {
			return new DirectSpecification<UserProfileList>(p => p.Active == value);
		}
		public static Specification<UserProfileList> ActiveIsNull() {
            return new DirectSpecification<UserProfileList>(p => p.Active == null);
        }
		public static Specification<UserProfileList> ActiveIsNotNull() {
            return new DirectSpecification<UserProfileList>(p => p.Active != null);
        }
		
					public static Specification<UserProfileList> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfileList>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<UserProfileList> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfileList>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<UserProfileList> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UserProfileList>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<UserProfileList> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.CreatedAt >= value);
        }
        public static Specification<UserProfileList> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.CreatedAt <= value);
        }
        public static Specification<UserProfileList> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.CreatedAt != value);
        }
        public static Specification<UserProfileList> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.CreatedAt > value);
        }
        public static Specification<UserProfileList> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.CreatedAt < value);
        }
		
					public static Specification<UserProfileList> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfileList>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<UserProfileList> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfileList>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<UserProfileList> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UserProfileList>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<UserProfileList> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.UpdatedAt >= value);
        }
        public static Specification<UserProfileList> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.UpdatedAt <= value);
        }
        public static Specification<UserProfileList> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.UpdatedAt != value);
        }
        public static Specification<UserProfileList> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.UpdatedAt > value);
        }
        public static Specification<UserProfileList> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.UpdatedAt < value);
        }
		
					public static Specification<UserProfileList> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfileList>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<UserProfileList> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfileList>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<UserProfileList> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UserProfileList>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<UserProfileList> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.DeletedAt >= value);
        }
        public static Specification<UserProfileList> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.DeletedAt <= value);
        }
        public static Specification<UserProfileList> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.DeletedAt != value);
        }
        public static Specification<UserProfileList> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.DeletedAt > value);
        }
        public static Specification<UserProfileList> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UserProfileList>(p => p.DeletedAt < value);
        }
		
					public static Specification<UserProfileList> IdContains(params int[] values) {
            return new DirectSpecification<UserProfileList>(p => values.Contains(p.Id));
        }
		public static Specification<UserProfileList> IdNotContains(params int[] values) {
            return new DirectSpecification<UserProfileList>(p => !values.Contains(p.Id));
        }
		public static Specification<UserProfileList> IdEqual(params int[] values) {
			return new DirectSpecification<UserProfileList>(p => values.Contains(p.Id));
        }
        public static Specification<UserProfileList> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfileList>(p => p.Id >= value);
        }
        public static Specification<UserProfileList> IdLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfileList>(p => p.Id <= value);
        }
        public static Specification<UserProfileList> IdNotEqual(int value) {
            return new DirectSpecification<UserProfileList>(p => p.Id != value);
        }
        public static Specification<UserProfileList> IdGreaterThan(int value) {
            return new DirectSpecification<UserProfileList>(p => p.Id > value);
        }
        public static Specification<UserProfileList> IdLessThan(int value) {
            return new DirectSpecification<UserProfileList>(p => p.Id < value);
        }
		
					public static Specification<UserProfileList> ExternalIdContains(string value) {
			return new DirectSpecification<UserProfileList>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<UserProfileList> ExternalIdStartsWith(string value) {
			return new DirectSpecification<UserProfileList>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<UserProfileList> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<UserProfileList>(p => values.Contains(p.ExternalId));
		}
		public static Specification<UserProfileList> ExternalIdNotEqual(string value) {
			return new DirectSpecification<UserProfileList>(p => p.ExternalId != value);
		}
		public static Specification<UserProfileList> ExternalIdIsNull() {
            return new DirectSpecification<UserProfileList>(p => p.ExternalId == null);
        }
		public static Specification<UserProfileList> ExternalIdIsNotNull() {
            return new DirectSpecification<UserProfileList>(p => p.ExternalId != null);
        }
		
					public static Specification<UserProfileList> IsDeletedEqual(bool value) {
			return new DirectSpecification<UserProfileList>(p => p.IsDeleted == value);
		}
		
	   }
   public partial class UserProfileSpecifications {
				public static Specification<UserProfile> DescriptionContains(string value) {
			return new DirectSpecification<UserProfile>(p => EF.Functions.Like(p.Description.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<UserProfile> DescriptionStartsWith(string value) {
			return new DirectSpecification<UserProfile>(p => EF.Functions.Like(p.Description.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<UserProfile> DescriptionEqual(params string[] values) {
			return new DirectSpecification<UserProfile>(p => values.Contains(p.Description));
		}
		public static Specification<UserProfile> DescriptionNotEqual(string value) {
			return new DirectSpecification<UserProfile>(p => p.Description != value);
		}
		public static Specification<UserProfile> DescriptionIsNull() {
            return new DirectSpecification<UserProfile>(p => p.Description == null);
        }
		public static Specification<UserProfile> DescriptionIsNotNull() {
            return new DirectSpecification<UserProfile>(p => p.Description != null);
        }
		
					public static Specification<UserProfile> CodeContains(string value) {
			return new DirectSpecification<UserProfile>(p => EF.Functions.Like(p.Code.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<UserProfile> CodeStartsWith(string value) {
			return new DirectSpecification<UserProfile>(p => EF.Functions.Like(p.Code.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<UserProfile> CodeEqual(params string[] values) {
			return new DirectSpecification<UserProfile>(p => values.Contains(p.Code));
		}
		public static Specification<UserProfile> CodeNotEqual(string value) {
			return new DirectSpecification<UserProfile>(p => p.Code != value);
		}
		public static Specification<UserProfile> CodeIsNull() {
            return new DirectSpecification<UserProfile>(p => p.Code == null);
        }
		public static Specification<UserProfile> CodeIsNotNull() {
            return new DirectSpecification<UserProfile>(p => p.Code != null);
        }
		
					public static Specification<UserProfile> InitialPageContains(string value) {
			return new DirectSpecification<UserProfile>(p => EF.Functions.Like(p.InitialPage.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<UserProfile> InitialPageStartsWith(string value) {
			return new DirectSpecification<UserProfile>(p => EF.Functions.Like(p.InitialPage.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<UserProfile> InitialPageEqual(params string[] values) {
			return new DirectSpecification<UserProfile>(p => values.Contains(p.InitialPage));
		}
		public static Specification<UserProfile> InitialPageNotEqual(string value) {
			return new DirectSpecification<UserProfile>(p => p.InitialPage != value);
		}
		public static Specification<UserProfile> InitialPageIsNull() {
            return new DirectSpecification<UserProfile>(p => p.InitialPage == null);
        }
		public static Specification<UserProfile> InitialPageIsNotNull() {
            return new DirectSpecification<UserProfile>(p => p.InitialPage != null);
        }
		
					public static Specification<UserProfile> IsPrivateProfileEqual(bool value) {
			return new DirectSpecification<UserProfile>(p => p.IsPrivateProfile == value);
		}
		
				
					public static Specification<UserProfile> CurrentStepContains(params int[] values) {
            return new DirectSpecification<UserProfile>(p => values.Contains(p.CurrentStep));
        }
		public static Specification<UserProfile> CurrentStepNotContains(params int[] values) {
            return new DirectSpecification<UserProfile>(p => !values.Contains(p.CurrentStep));
        }
		public static Specification<UserProfile> CurrentStepEqual(params int[] values) {
			return new DirectSpecification<UserProfile>(p => values.Contains(p.CurrentStep));
        }
        public static Specification<UserProfile> CurrentStepGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfile>(p => p.CurrentStep >= value);
        }
        public static Specification<UserProfile> CurrentStepLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfile>(p => p.CurrentStep <= value);
        }
        public static Specification<UserProfile> CurrentStepNotEqual(int value) {
            return new DirectSpecification<UserProfile>(p => p.CurrentStep != value);
        }
        public static Specification<UserProfile> CurrentStepGreaterThan(int value) {
            return new DirectSpecification<UserProfile>(p => p.CurrentStep > value);
        }
        public static Specification<UserProfile> CurrentStepLessThan(int value) {
            return new DirectSpecification<UserProfile>(p => p.CurrentStep < value);
        }
		
					public static Specification<UserProfile> MaxStepsContains(params int[] values) {
            return new DirectSpecification<UserProfile>(p => values.Contains(p.MaxSteps.Value));
        }
		public static Specification<UserProfile> MaxStepsNotContains(params int[] values) {
            return new DirectSpecification<UserProfile>(p => !values.Contains(p.MaxSteps.Value));
        }
		public static Specification<UserProfile> MaxStepsEqual(params int[] values) {
			return new DirectSpecification<UserProfile>(p => values.Contains(p.MaxSteps.Value));
        }
        public static Specification<UserProfile> MaxStepsGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfile>(p => p.MaxSteps >= value);
        }
        public static Specification<UserProfile> MaxStepsLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfile>(p => p.MaxSteps <= value);
        }
        public static Specification<UserProfile> MaxStepsNotEqual(int value) {
            return new DirectSpecification<UserProfile>(p => p.MaxSteps != value);
        }
        public static Specification<UserProfile> MaxStepsGreaterThan(int value) {
            return new DirectSpecification<UserProfile>(p => p.MaxSteps > value);
        }
        public static Specification<UserProfile> MaxStepsLessThan(int value) {
            return new DirectSpecification<UserProfile>(p => p.MaxSteps < value);
        }
		
					public static Specification<UserProfile> RegisterDoneEqual(bool value) {
			return new DirectSpecification<UserProfile>(p => p.RegisterDone == value);
		}
		public static Specification<UserProfile> RegisterDoneIsNull() {
            return new DirectSpecification<UserProfile>(p => p.RegisterDone == null);
        }
		public static Specification<UserProfile> RegisterDoneIsNotNull() {
            return new DirectSpecification<UserProfile>(p => p.RegisterDone != null);
        }
		
					public static Specification<UserProfile> ActiveEqual(bool value) {
			return new DirectSpecification<UserProfile>(p => p.Active == value);
		}
		public static Specification<UserProfile> ActiveIsNull() {
            return new DirectSpecification<UserProfile>(p => p.Active == null);
        }
		public static Specification<UserProfile> ActiveIsNotNull() {
            return new DirectSpecification<UserProfile>(p => p.Active != null);
        }
		
					public static Specification<UserProfile> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfile>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<UserProfile> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfile>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<UserProfile> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UserProfile>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<UserProfile> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.CreatedAt >= value);
        }
        public static Specification<UserProfile> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.CreatedAt <= value);
        }
        public static Specification<UserProfile> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.CreatedAt != value);
        }
        public static Specification<UserProfile> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.CreatedAt > value);
        }
        public static Specification<UserProfile> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.CreatedAt < value);
        }
		
					public static Specification<UserProfile> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfile>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<UserProfile> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfile>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<UserProfile> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UserProfile>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<UserProfile> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.UpdatedAt >= value);
        }
        public static Specification<UserProfile> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.UpdatedAt <= value);
        }
        public static Specification<UserProfile> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.UpdatedAt != value);
        }
        public static Specification<UserProfile> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.UpdatedAt > value);
        }
        public static Specification<UserProfile> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.UpdatedAt < value);
        }
		
					public static Specification<UserProfile> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfile>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<UserProfile> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UserProfile>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<UserProfile> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UserProfile>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<UserProfile> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.DeletedAt >= value);
        }
        public static Specification<UserProfile> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.DeletedAt <= value);
        }
        public static Specification<UserProfile> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.DeletedAt != value);
        }
        public static Specification<UserProfile> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.DeletedAt > value);
        }
        public static Specification<UserProfile> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UserProfile>(p => p.DeletedAt < value);
        }
		
					public static Specification<UserProfile> IdContains(params int[] values) {
            return new DirectSpecification<UserProfile>(p => values.Contains(p.Id));
        }
		public static Specification<UserProfile> IdNotContains(params int[] values) {
            return new DirectSpecification<UserProfile>(p => !values.Contains(p.Id));
        }
		public static Specification<UserProfile> IdEqual(params int[] values) {
			return new DirectSpecification<UserProfile>(p => values.Contains(p.Id));
        }
        public static Specification<UserProfile> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<UserProfile>(p => p.Id >= value);
        }
        public static Specification<UserProfile> IdLessThanOrEqual(int value) {
            return new DirectSpecification<UserProfile>(p => p.Id <= value);
        }
        public static Specification<UserProfile> IdNotEqual(int value) {
            return new DirectSpecification<UserProfile>(p => p.Id != value);
        }
        public static Specification<UserProfile> IdGreaterThan(int value) {
            return new DirectSpecification<UserProfile>(p => p.Id > value);
        }
        public static Specification<UserProfile> IdLessThan(int value) {
            return new DirectSpecification<UserProfile>(p => p.Id < value);
        }
		
					public static Specification<UserProfile> ExternalIdContains(string value) {
			return new DirectSpecification<UserProfile>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<UserProfile> ExternalIdStartsWith(string value) {
			return new DirectSpecification<UserProfile>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<UserProfile> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<UserProfile>(p => values.Contains(p.ExternalId));
		}
		public static Specification<UserProfile> ExternalIdNotEqual(string value) {
			return new DirectSpecification<UserProfile>(p => p.ExternalId != value);
		}
		public static Specification<UserProfile> ExternalIdIsNull() {
            return new DirectSpecification<UserProfile>(p => p.ExternalId == null);
        }
		public static Specification<UserProfile> ExternalIdIsNotNull() {
            return new DirectSpecification<UserProfile>(p => p.ExternalId != null);
        }
		
					public static Specification<UserProfile> IsDeletedEqual(bool value) {
			return new DirectSpecification<UserProfile>(p => p.IsDeleted == value);
		}
		
	   }
   public partial class UsersAggSettingsSpecifications {
				public static Specification<UsersAggSettings> UserIdContains(params int[] values) {
            return new DirectSpecification<UsersAggSettings>(p => values.Contains(p.UserId));
        }
		public static Specification<UsersAggSettings> UserIdNotContains(params int[] values) {
            return new DirectSpecification<UsersAggSettings>(p => !values.Contains(p.UserId));
        }
		public static Specification<UsersAggSettings> UserIdEqual(params int[] values) {
			return new DirectSpecification<UsersAggSettings>(p => values.Contains(p.UserId));
        }
        public static Specification<UsersAggSettings> UserIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<UsersAggSettings>(p => p.UserId >= value);
        }
        public static Specification<UsersAggSettings> UserIdLessThanOrEqual(int value) {
            return new DirectSpecification<UsersAggSettings>(p => p.UserId <= value);
        }
        public static Specification<UsersAggSettings> UserIdNotEqual(int value) {
            return new DirectSpecification<UsersAggSettings>(p => p.UserId != value);
        }
        public static Specification<UsersAggSettings> UserIdGreaterThan(int value) {
            return new DirectSpecification<UsersAggSettings>(p => p.UserId > value);
        }
        public static Specification<UsersAggSettings> UserIdLessThan(int value) {
            return new DirectSpecification<UsersAggSettings>(p => p.UserId < value);
        }
		
					public static Specification<UsersAggSettings> AutoSaveSettingsEnabledEqual(bool value) {
			return new DirectSpecification<UsersAggSettings>(p => p.AutoSaveSettingsEnabled == value);
		}
		
					public static Specification<UsersAggSettings> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UsersAggSettings>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<UsersAggSettings> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UsersAggSettings>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<UsersAggSettings> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UsersAggSettings>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<UsersAggSettings> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.CreatedAt >= value);
        }
        public static Specification<UsersAggSettings> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.CreatedAt <= value);
        }
        public static Specification<UsersAggSettings> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.CreatedAt != value);
        }
        public static Specification<UsersAggSettings> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.CreatedAt > value);
        }
        public static Specification<UsersAggSettings> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.CreatedAt < value);
        }
		
					public static Specification<UsersAggSettings> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UsersAggSettings>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<UsersAggSettings> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UsersAggSettings>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<UsersAggSettings> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UsersAggSettings>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<UsersAggSettings> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.UpdatedAt >= value);
        }
        public static Specification<UsersAggSettings> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.UpdatedAt <= value);
        }
        public static Specification<UsersAggSettings> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.UpdatedAt != value);
        }
        public static Specification<UsersAggSettings> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.UpdatedAt > value);
        }
        public static Specification<UsersAggSettings> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.UpdatedAt < value);
        }
		
					public static Specification<UsersAggSettings> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<UsersAggSettings>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<UsersAggSettings> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<UsersAggSettings>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<UsersAggSettings> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<UsersAggSettings>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<UsersAggSettings> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.DeletedAt >= value);
        }
        public static Specification<UsersAggSettings> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.DeletedAt <= value);
        }
        public static Specification<UsersAggSettings> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.DeletedAt != value);
        }
        public static Specification<UsersAggSettings> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.DeletedAt > value);
        }
        public static Specification<UsersAggSettings> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<UsersAggSettings>(p => p.DeletedAt < value);
        }
		
					public static Specification<UsersAggSettings> IdContains(params int[] values) {
            return new DirectSpecification<UsersAggSettings>(p => values.Contains(p.Id));
        }
		public static Specification<UsersAggSettings> IdNotContains(params int[] values) {
            return new DirectSpecification<UsersAggSettings>(p => !values.Contains(p.Id));
        }
		public static Specification<UsersAggSettings> IdEqual(params int[] values) {
			return new DirectSpecification<UsersAggSettings>(p => values.Contains(p.Id));
        }
        public static Specification<UsersAggSettings> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<UsersAggSettings>(p => p.Id >= value);
        }
        public static Specification<UsersAggSettings> IdLessThanOrEqual(int value) {
            return new DirectSpecification<UsersAggSettings>(p => p.Id <= value);
        }
        public static Specification<UsersAggSettings> IdNotEqual(int value) {
            return new DirectSpecification<UsersAggSettings>(p => p.Id != value);
        }
        public static Specification<UsersAggSettings> IdGreaterThan(int value) {
            return new DirectSpecification<UsersAggSettings>(p => p.Id > value);
        }
        public static Specification<UsersAggSettings> IdLessThan(int value) {
            return new DirectSpecification<UsersAggSettings>(p => p.Id < value);
        }
		
					public static Specification<UsersAggSettings> ExternalIdContains(string value) {
			return new DirectSpecification<UsersAggSettings>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<UsersAggSettings> ExternalIdStartsWith(string value) {
			return new DirectSpecification<UsersAggSettings>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<UsersAggSettings> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<UsersAggSettings>(p => values.Contains(p.ExternalId));
		}
		public static Specification<UsersAggSettings> ExternalIdNotEqual(string value) {
			return new DirectSpecification<UsersAggSettings>(p => p.ExternalId != value);
		}
		public static Specification<UsersAggSettings> ExternalIdIsNull() {
            return new DirectSpecification<UsersAggSettings>(p => p.ExternalId == null);
        }
		public static Specification<UsersAggSettings> ExternalIdIsNotNull() {
            return new DirectSpecification<UsersAggSettings>(p => p.ExternalId != null);
        }
		
					public static Specification<UsersAggSettings> IsDeletedEqual(bool value) {
			return new DirectSpecification<UsersAggSettings>(p => p.IsDeleted == value);
		}
		
	   }
   public partial class UserSpecifications {
				public static Specification<User> UserNameContains(string value) {
			return new DirectSpecification<User>(p => EF.Functions.Like(p.UserName.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<User> UserNameStartsWith(string value) {
			return new DirectSpecification<User>(p => EF.Functions.Like(p.UserName.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<User> UserNameEqual(params string[] values) {
			return new DirectSpecification<User>(p => values.Contains(p.UserName));
		}
		public static Specification<User> UserNameNotEqual(string value) {
			return new DirectSpecification<User>(p => p.UserName != value);
		}
		public static Specification<User> UserNameIsNull() {
            return new DirectSpecification<User>(p => p.UserName == null);
        }
		public static Specification<User> UserNameIsNotNull() {
            return new DirectSpecification<User>(p => p.UserName != null);
        }
		
					public static Specification<User> NameContains(string value) {
			return new DirectSpecification<User>(p => EF.Functions.Like(p.Name.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<User> NameStartsWith(string value) {
			return new DirectSpecification<User>(p => EF.Functions.Like(p.Name.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<User> NameEqual(params string[] values) {
			return new DirectSpecification<User>(p => values.Contains(p.Name));
		}
		public static Specification<User> NameNotEqual(string value) {
			return new DirectSpecification<User>(p => p.Name != value);
		}
		public static Specification<User> NameIsNull() {
            return new DirectSpecification<User>(p => p.Name == null);
        }
		public static Specification<User> NameIsNotNull() {
            return new DirectSpecification<User>(p => p.Name != null);
        }
		
					public static Specification<User> BirthDateContains(params System.DateOnly[] values) {
            return new DirectSpecification<User>(p => values.Contains(p.BirthDate.Value));
        }
		public static Specification<User> BirthDateNotContains(params System.DateOnly[] values) {
            return new DirectSpecification<User>(p => !values.Contains(p.BirthDate.Value));
        }
		public static Specification<User> BirthDateEqual(params System.DateOnly[] values) {
			return new DirectSpecification<User>(p => values.Contains(p.BirthDate.Value));
        }
        public static Specification<User> BirthDateGreaterThanOrEqual(System.DateOnly value) {
            return new DirectSpecification<User>(p => p.BirthDate >= value);
        }
        public static Specification<User> BirthDateLessThanOrEqual(System.DateOnly value) {
            return new DirectSpecification<User>(p => p.BirthDate <= value);
        }
        public static Specification<User> BirthDateNotEqual(System.DateOnly value) {
            return new DirectSpecification<User>(p => p.BirthDate != value);
        }
        public static Specification<User> BirthDateGreaterThan(System.DateOnly value) {
            return new DirectSpecification<User>(p => p.BirthDate > value);
        }
        public static Specification<User> BirthDateLessThan(System.DateOnly value) {
            return new DirectSpecification<User>(p => p.BirthDate < value);
        }
		
					public static Specification<User> GenderEqual(params LazyCrud.Users.Enumerations.GenderEnum[] values) {
			return new DirectSpecification<User>(p => values.Contains(p.Gender));
		}
		public static Specification<User> GenderNotEqual(LazyCrud.Users.Enumerations.GenderEnum value) {
			return new DirectSpecification<User>(p => p.Gender != value);
		}
	
					public static Specification<User> NeedResetPasswordEqual(bool value) {
			return new DirectSpecification<User>(p => p.NeedResetPassword == value);
		}
		public static Specification<User> NeedResetPasswordIsNull() {
            return new DirectSpecification<User>(p => p.NeedResetPassword == null);
        }
		public static Specification<User> NeedResetPasswordIsNotNull() {
            return new DirectSpecification<User>(p => p.NeedResetPassword != null);
        }
		
					public static Specification<User> NeedSendOnboardingMailEqual(bool value) {
			return new DirectSpecification<User>(p => p.NeedSendOnboardingMail == value);
		}
		public static Specification<User> NeedSendOnboardingMailIsNull() {
            return new DirectSpecification<User>(p => p.NeedSendOnboardingMail == null);
        }
		public static Specification<User> NeedSendOnboardingMailIsNotNull() {
            return new DirectSpecification<User>(p => p.NeedSendOnboardingMail != null);
        }
		
					public static Specification<User> ProfilePictureContains(string value) {
			return new DirectSpecification<User>(p => EF.Functions.Like(p.ProfilePicture.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<User> ProfilePictureStartsWith(string value) {
			return new DirectSpecification<User>(p => EF.Functions.Like(p.ProfilePicture.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<User> ProfilePictureEqual(params string[] values) {
			return new DirectSpecification<User>(p => values.Contains(p.ProfilePicture));
		}
		public static Specification<User> ProfilePictureNotEqual(string value) {
			return new DirectSpecification<User>(p => p.ProfilePicture != value);
		}
		public static Specification<User> ProfilePictureIsNull() {
            return new DirectSpecification<User>(p => p.ProfilePicture == null);
        }
		public static Specification<User> ProfilePictureIsNotNull() {
            return new DirectSpecification<User>(p => p.ProfilePicture != null);
        }
		
				
					public static Specification<User> CanUpdatePasswordEqual(bool value) {
			return new DirectSpecification<User>(p => p.CanUpdatePassword == value);
		}
		public static Specification<User> CanUpdatePasswordIsNull() {
            return new DirectSpecification<User>(p => p.CanUpdatePassword == null);
        }
		public static Specification<User> CanUpdatePasswordIsNotNull() {
            return new DirectSpecification<User>(p => p.CanUpdatePassword != null);
        }
		
				
				
					public static Specification<User> CurrentStepContains(params int[] values) {
            return new DirectSpecification<User>(p => values.Contains(p.CurrentStep));
        }
		public static Specification<User> CurrentStepNotContains(params int[] values) {
            return new DirectSpecification<User>(p => !values.Contains(p.CurrentStep));
        }
		public static Specification<User> CurrentStepEqual(params int[] values) {
			return new DirectSpecification<User>(p => values.Contains(p.CurrentStep));
        }
        public static Specification<User> CurrentStepGreaterThanOrEqual(int value) {
            return new DirectSpecification<User>(p => p.CurrentStep >= value);
        }
        public static Specification<User> CurrentStepLessThanOrEqual(int value) {
            return new DirectSpecification<User>(p => p.CurrentStep <= value);
        }
        public static Specification<User> CurrentStepNotEqual(int value) {
            return new DirectSpecification<User>(p => p.CurrentStep != value);
        }
        public static Specification<User> CurrentStepGreaterThan(int value) {
            return new DirectSpecification<User>(p => p.CurrentStep > value);
        }
        public static Specification<User> CurrentStepLessThan(int value) {
            return new DirectSpecification<User>(p => p.CurrentStep < value);
        }
		
					public static Specification<User> MaxStepsContains(params int[] values) {
            return new DirectSpecification<User>(p => values.Contains(p.MaxSteps.Value));
        }
		public static Specification<User> MaxStepsNotContains(params int[] values) {
            return new DirectSpecification<User>(p => !values.Contains(p.MaxSteps.Value));
        }
		public static Specification<User> MaxStepsEqual(params int[] values) {
			return new DirectSpecification<User>(p => values.Contains(p.MaxSteps.Value));
        }
        public static Specification<User> MaxStepsGreaterThanOrEqual(int value) {
            return new DirectSpecification<User>(p => p.MaxSteps >= value);
        }
        public static Specification<User> MaxStepsLessThanOrEqual(int value) {
            return new DirectSpecification<User>(p => p.MaxSteps <= value);
        }
        public static Specification<User> MaxStepsNotEqual(int value) {
            return new DirectSpecification<User>(p => p.MaxSteps != value);
        }
        public static Specification<User> MaxStepsGreaterThan(int value) {
            return new DirectSpecification<User>(p => p.MaxSteps > value);
        }
        public static Specification<User> MaxStepsLessThan(int value) {
            return new DirectSpecification<User>(p => p.MaxSteps < value);
        }
		
					public static Specification<User> RegisterDoneEqual(bool value) {
			return new DirectSpecification<User>(p => p.RegisterDone == value);
		}
		public static Specification<User> RegisterDoneIsNull() {
            return new DirectSpecification<User>(p => p.RegisterDone == null);
        }
		public static Specification<User> RegisterDoneIsNotNull() {
            return new DirectSpecification<User>(p => p.RegisterDone != null);
        }
		
					public static Specification<User> ActiveEqual(bool value) {
			return new DirectSpecification<User>(p => p.Active == value);
		}
		public static Specification<User> ActiveIsNull() {
            return new DirectSpecification<User>(p => p.Active == null);
        }
		public static Specification<User> ActiveIsNotNull() {
            return new DirectSpecification<User>(p => p.Active != null);
        }
		
					public static Specification<User> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<User>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<User> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<User>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<User> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<User>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<User> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<User>(p => p.CreatedAt >= value);
        }
        public static Specification<User> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<User>(p => p.CreatedAt <= value);
        }
        public static Specification<User> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<User>(p => p.CreatedAt != value);
        }
        public static Specification<User> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<User>(p => p.CreatedAt > value);
        }
        public static Specification<User> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<User>(p => p.CreatedAt < value);
        }
		
					public static Specification<User> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<User>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<User> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<User>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<User> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<User>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<User> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<User>(p => p.UpdatedAt >= value);
        }
        public static Specification<User> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<User>(p => p.UpdatedAt <= value);
        }
        public static Specification<User> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<User>(p => p.UpdatedAt != value);
        }
        public static Specification<User> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<User>(p => p.UpdatedAt > value);
        }
        public static Specification<User> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<User>(p => p.UpdatedAt < value);
        }
		
					public static Specification<User> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<User>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<User> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<User>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<User> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<User>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<User> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<User>(p => p.DeletedAt >= value);
        }
        public static Specification<User> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<User>(p => p.DeletedAt <= value);
        }
        public static Specification<User> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<User>(p => p.DeletedAt != value);
        }
        public static Specification<User> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<User>(p => p.DeletedAt > value);
        }
        public static Specification<User> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<User>(p => p.DeletedAt < value);
        }
		
					public static Specification<User> IdContains(params int[] values) {
            return new DirectSpecification<User>(p => values.Contains(p.Id));
        }
		public static Specification<User> IdNotContains(params int[] values) {
            return new DirectSpecification<User>(p => !values.Contains(p.Id));
        }
		public static Specification<User> IdEqual(params int[] values) {
			return new DirectSpecification<User>(p => values.Contains(p.Id));
        }
        public static Specification<User> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<User>(p => p.Id >= value);
        }
        public static Specification<User> IdLessThanOrEqual(int value) {
            return new DirectSpecification<User>(p => p.Id <= value);
        }
        public static Specification<User> IdNotEqual(int value) {
            return new DirectSpecification<User>(p => p.Id != value);
        }
        public static Specification<User> IdGreaterThan(int value) {
            return new DirectSpecification<User>(p => p.Id > value);
        }
        public static Specification<User> IdLessThan(int value) {
            return new DirectSpecification<User>(p => p.Id < value);
        }
		
					public static Specification<User> ExternalIdContains(string value) {
			return new DirectSpecification<User>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<User> ExternalIdStartsWith(string value) {
			return new DirectSpecification<User>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<User> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<User>(p => values.Contains(p.ExternalId));
		}
		public static Specification<User> ExternalIdNotEqual(string value) {
			return new DirectSpecification<User>(p => p.ExternalId != value);
		}
		public static Specification<User> ExternalIdIsNull() {
            return new DirectSpecification<User>(p => p.ExternalId == null);
        }
		public static Specification<User> ExternalIdIsNotNull() {
            return new DirectSpecification<User>(p => p.ExternalId != null);
        }
		
					public static Specification<User> IsDeletedEqual(bool value) {
			return new DirectSpecification<User>(p => p.IsDeleted == value);
		}
		
	   }
}
