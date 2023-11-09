using LazyCrud.Core.Domain.Seedwork.Specification;
using Microsoft.EntityFrameworkCore;
namespace LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Specifications {
	using Entities;
   public partial class SystemPanelSubItemSpecifications {
			
					public static Specification<SystemPanelSubItem> SystemPanelSubItemIdContains(params int[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.SystemPanelSubItemId.Value));
        }
		public static Specification<SystemPanelSubItem> SystemPanelSubItemIdNotContains(params int[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => !values.Contains(p.SystemPanelSubItemId.Value));
        }
		public static Specification<SystemPanelSubItem> SystemPanelSubItemIdEqual(params int[] values) {
			return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.SystemPanelSubItemId.Value));
        }
        public static Specification<SystemPanelSubItem> SystemPanelSubItemIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.SystemPanelSubItemId >= value);
        }
        public static Specification<SystemPanelSubItem> SystemPanelSubItemIdLessThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.SystemPanelSubItemId <= value);
        }
        public static Specification<SystemPanelSubItem> SystemPanelSubItemIdNotEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.SystemPanelSubItemId != value);
        }
        public static Specification<SystemPanelSubItem> SystemPanelSubItemIdGreaterThan(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.SystemPanelSubItemId > value);
        }
        public static Specification<SystemPanelSubItem> SystemPanelSubItemIdLessThan(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.SystemPanelSubItemId < value);
        }
		
					public static Specification<SystemPanelSubItem> SystemPanelIdContains(params int[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.SystemPanelId));
        }
		public static Specification<SystemPanelSubItem> SystemPanelIdNotContains(params int[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => !values.Contains(p.SystemPanelId));
        }
		public static Specification<SystemPanelSubItem> SystemPanelIdEqual(params int[] values) {
			return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.SystemPanelId));
        }
        public static Specification<SystemPanelSubItem> SystemPanelIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.SystemPanelId >= value);
        }
        public static Specification<SystemPanelSubItem> SystemPanelIdLessThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.SystemPanelId <= value);
        }
        public static Specification<SystemPanelSubItem> SystemPanelIdNotEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.SystemPanelId != value);
        }
        public static Specification<SystemPanelSubItem> SystemPanelIdGreaterThan(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.SystemPanelId > value);
        }
        public static Specification<SystemPanelSubItem> SystemPanelIdLessThan(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.SystemPanelId < value);
        }
		
					public static Specification<SystemPanelSubItem> IsSubItemEqual(bool value) {
			return new DirectSpecification<SystemPanelSubItem>(p => p.IsSubItem == value);
		}
		
					public static Specification<SystemPanelSubItem> IconContains(string value) {
			return new DirectSpecification<SystemPanelSubItem>(p => EF.Functions.Like(p.Icon.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<SystemPanelSubItem> IconStartsWith(string value) {
			return new DirectSpecification<SystemPanelSubItem>(p => EF.Functions.Like(p.Icon.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<SystemPanelSubItem> IconEqual(params string[] values) {
			return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.Icon));
		}
		public static Specification<SystemPanelSubItem> IconNotEqual(string value) {
			return new DirectSpecification<SystemPanelSubItem>(p => p.Icon != value);
		}
		public static Specification<SystemPanelSubItem> IconIsNull() {
            return new DirectSpecification<SystemPanelSubItem>(p => p.Icon == null);
        }
		public static Specification<SystemPanelSubItem> IconIsNotNull() {
            return new DirectSpecification<SystemPanelSubItem>(p => p.Icon != null);
        }
		
					public static Specification<SystemPanelSubItem> DescriptionContains(string value) {
			return new DirectSpecification<SystemPanelSubItem>(p => EF.Functions.Like(p.Description.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<SystemPanelSubItem> DescriptionStartsWith(string value) {
			return new DirectSpecification<SystemPanelSubItem>(p => EF.Functions.Like(p.Description.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<SystemPanelSubItem> DescriptionEqual(params string[] values) {
			return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.Description));
		}
		public static Specification<SystemPanelSubItem> DescriptionNotEqual(string value) {
			return new DirectSpecification<SystemPanelSubItem>(p => p.Description != value);
		}
		public static Specification<SystemPanelSubItem> DescriptionIsNull() {
            return new DirectSpecification<SystemPanelSubItem>(p => p.Description == null);
        }
		public static Specification<SystemPanelSubItem> DescriptionIsNotNull() {
            return new DirectSpecification<SystemPanelSubItem>(p => p.Description != null);
        }
		
					public static Specification<SystemPanelSubItem> UrlContains(string value) {
			return new DirectSpecification<SystemPanelSubItem>(p => EF.Functions.Like(p.Url.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<SystemPanelSubItem> UrlStartsWith(string value) {
			return new DirectSpecification<SystemPanelSubItem>(p => EF.Functions.Like(p.Url.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<SystemPanelSubItem> UrlEqual(params string[] values) {
			return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.Url));
		}
		public static Specification<SystemPanelSubItem> UrlNotEqual(string value) {
			return new DirectSpecification<SystemPanelSubItem>(p => p.Url != value);
		}
		public static Specification<SystemPanelSubItem> UrlIsNull() {
            return new DirectSpecification<SystemPanelSubItem>(p => p.Url == null);
        }
		public static Specification<SystemPanelSubItem> UrlIsNotNull() {
            return new DirectSpecification<SystemPanelSubItem>(p => p.Url != null);
        }
		
					public static Specification<SystemPanelSubItem> LinkDiretoEqual(bool value) {
			return new DirectSpecification<SystemPanelSubItem>(p => p.LinkDireto == value);
		}
		
					public static Specification<SystemPanelSubItem> ActionButtonEqual(bool value) {
			return new DirectSpecification<SystemPanelSubItem>(p => p.ActionButton == value);
		}
		
					public static Specification<SystemPanelSubItem> CurrentStepContains(params int[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.CurrentStep));
        }
		public static Specification<SystemPanelSubItem> CurrentStepNotContains(params int[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => !values.Contains(p.CurrentStep));
        }
		public static Specification<SystemPanelSubItem> CurrentStepEqual(params int[] values) {
			return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.CurrentStep));
        }
        public static Specification<SystemPanelSubItem> CurrentStepGreaterThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.CurrentStep >= value);
        }
        public static Specification<SystemPanelSubItem> CurrentStepLessThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.CurrentStep <= value);
        }
        public static Specification<SystemPanelSubItem> CurrentStepNotEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.CurrentStep != value);
        }
        public static Specification<SystemPanelSubItem> CurrentStepGreaterThan(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.CurrentStep > value);
        }
        public static Specification<SystemPanelSubItem> CurrentStepLessThan(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.CurrentStep < value);
        }
		
					public static Specification<SystemPanelSubItem> MaxStepsContains(params int[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.MaxSteps.Value));
        }
		public static Specification<SystemPanelSubItem> MaxStepsNotContains(params int[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => !values.Contains(p.MaxSteps.Value));
        }
		public static Specification<SystemPanelSubItem> MaxStepsEqual(params int[] values) {
			return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.MaxSteps.Value));
        }
        public static Specification<SystemPanelSubItem> MaxStepsGreaterThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.MaxSteps >= value);
        }
        public static Specification<SystemPanelSubItem> MaxStepsLessThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.MaxSteps <= value);
        }
        public static Specification<SystemPanelSubItem> MaxStepsNotEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.MaxSteps != value);
        }
        public static Specification<SystemPanelSubItem> MaxStepsGreaterThan(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.MaxSteps > value);
        }
        public static Specification<SystemPanelSubItem> MaxStepsLessThan(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.MaxSteps < value);
        }
		
					public static Specification<SystemPanelSubItem> RegisterDoneEqual(bool value) {
			return new DirectSpecification<SystemPanelSubItem>(p => p.RegisterDone == value);
		}
		public static Specification<SystemPanelSubItem> RegisterDoneIsNull() {
            return new DirectSpecification<SystemPanelSubItem>(p => p.RegisterDone == null);
        }
		public static Specification<SystemPanelSubItem> RegisterDoneIsNotNull() {
            return new DirectSpecification<SystemPanelSubItem>(p => p.RegisterDone != null);
        }
		
					public static Specification<SystemPanelSubItem> ActiveEqual(bool value) {
			return new DirectSpecification<SystemPanelSubItem>(p => p.Active == value);
		}
		public static Specification<SystemPanelSubItem> ActiveIsNull() {
            return new DirectSpecification<SystemPanelSubItem>(p => p.Active == null);
        }
		public static Specification<SystemPanelSubItem> ActiveIsNotNull() {
            return new DirectSpecification<SystemPanelSubItem>(p => p.Active != null);
        }
		
					public static Specification<SystemPanelSubItem> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<SystemPanelSubItem> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<SystemPanelSubItem> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<SystemPanelSubItem> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.CreatedAt >= value);
        }
        public static Specification<SystemPanelSubItem> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.CreatedAt <= value);
        }
        public static Specification<SystemPanelSubItem> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.CreatedAt != value);
        }
        public static Specification<SystemPanelSubItem> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.CreatedAt > value);
        }
        public static Specification<SystemPanelSubItem> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.CreatedAt < value);
        }
		
					public static Specification<SystemPanelSubItem> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<SystemPanelSubItem> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<SystemPanelSubItem> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<SystemPanelSubItem> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.UpdatedAt >= value);
        }
        public static Specification<SystemPanelSubItem> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.UpdatedAt <= value);
        }
        public static Specification<SystemPanelSubItem> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.UpdatedAt != value);
        }
        public static Specification<SystemPanelSubItem> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.UpdatedAt > value);
        }
        public static Specification<SystemPanelSubItem> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.UpdatedAt < value);
        }
		
					public static Specification<SystemPanelSubItem> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<SystemPanelSubItem> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<SystemPanelSubItem> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<SystemPanelSubItem> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.DeletedAt >= value);
        }
        public static Specification<SystemPanelSubItem> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.DeletedAt <= value);
        }
        public static Specification<SystemPanelSubItem> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.DeletedAt != value);
        }
        public static Specification<SystemPanelSubItem> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.DeletedAt > value);
        }
        public static Specification<SystemPanelSubItem> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.DeletedAt < value);
        }
		
					public static Specification<SystemPanelSubItem> IdContains(params int[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.Id));
        }
		public static Specification<SystemPanelSubItem> IdNotContains(params int[] values) {
            return new DirectSpecification<SystemPanelSubItem>(p => !values.Contains(p.Id));
        }
		public static Specification<SystemPanelSubItem> IdEqual(params int[] values) {
			return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.Id));
        }
        public static Specification<SystemPanelSubItem> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.Id >= value);
        }
        public static Specification<SystemPanelSubItem> IdLessThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.Id <= value);
        }
        public static Specification<SystemPanelSubItem> IdNotEqual(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.Id != value);
        }
        public static Specification<SystemPanelSubItem> IdGreaterThan(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.Id > value);
        }
        public static Specification<SystemPanelSubItem> IdLessThan(int value) {
            return new DirectSpecification<SystemPanelSubItem>(p => p.Id < value);
        }
		
					public static Specification<SystemPanelSubItem> ExternalIdContains(string value) {
			return new DirectSpecification<SystemPanelSubItem>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<SystemPanelSubItem> ExternalIdStartsWith(string value) {
			return new DirectSpecification<SystemPanelSubItem>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<SystemPanelSubItem> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<SystemPanelSubItem>(p => values.Contains(p.ExternalId));
		}
		public static Specification<SystemPanelSubItem> ExternalIdNotEqual(string value) {
			return new DirectSpecification<SystemPanelSubItem>(p => p.ExternalId != value);
		}
		public static Specification<SystemPanelSubItem> ExternalIdIsNull() {
            return new DirectSpecification<SystemPanelSubItem>(p => p.ExternalId == null);
        }
		public static Specification<SystemPanelSubItem> ExternalIdIsNotNull() {
            return new DirectSpecification<SystemPanelSubItem>(p => p.ExternalId != null);
        }
		
					public static Specification<SystemPanelSubItem> IsDeletedEqual(bool value) {
			return new DirectSpecification<SystemPanelSubItem>(p => p.IsDeleted == value);
		}
		
	   }
   public partial class SystemPanelSpecifications {
				public static Specification<SystemPanel> IconContains(string value) {
			return new DirectSpecification<SystemPanel>(p => EF.Functions.Like(p.Icon.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<SystemPanel> IconStartsWith(string value) {
			return new DirectSpecification<SystemPanel>(p => EF.Functions.Like(p.Icon.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<SystemPanel> IconEqual(params string[] values) {
			return new DirectSpecification<SystemPanel>(p => values.Contains(p.Icon));
		}
		public static Specification<SystemPanel> IconNotEqual(string value) {
			return new DirectSpecification<SystemPanel>(p => p.Icon != value);
		}
		public static Specification<SystemPanel> IconIsNull() {
            return new DirectSpecification<SystemPanel>(p => p.Icon == null);
        }
		public static Specification<SystemPanel> IconIsNotNull() {
            return new DirectSpecification<SystemPanel>(p => p.Icon != null);
        }
		
					public static Specification<SystemPanel> DescriptionContains(string value) {
			return new DirectSpecification<SystemPanel>(p => EF.Functions.Like(p.Description.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<SystemPanel> DescriptionStartsWith(string value) {
			return new DirectSpecification<SystemPanel>(p => EF.Functions.Like(p.Description.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<SystemPanel> DescriptionEqual(params string[] values) {
			return new DirectSpecification<SystemPanel>(p => values.Contains(p.Description));
		}
		public static Specification<SystemPanel> DescriptionNotEqual(string value) {
			return new DirectSpecification<SystemPanel>(p => p.Description != value);
		}
		public static Specification<SystemPanel> DescriptionIsNull() {
            return new DirectSpecification<SystemPanel>(p => p.Description == null);
        }
		public static Specification<SystemPanel> DescriptionIsNotNull() {
            return new DirectSpecification<SystemPanel>(p => p.Description != null);
        }
		
				
				
				
					public static Specification<SystemPanel> CurrentStepContains(params int[] values) {
            return new DirectSpecification<SystemPanel>(p => values.Contains(p.CurrentStep));
        }
		public static Specification<SystemPanel> CurrentStepNotContains(params int[] values) {
            return new DirectSpecification<SystemPanel>(p => !values.Contains(p.CurrentStep));
        }
		public static Specification<SystemPanel> CurrentStepEqual(params int[] values) {
			return new DirectSpecification<SystemPanel>(p => values.Contains(p.CurrentStep));
        }
        public static Specification<SystemPanel> CurrentStepGreaterThanOrEqual(int value) {
            return new DirectSpecification<SystemPanel>(p => p.CurrentStep >= value);
        }
        public static Specification<SystemPanel> CurrentStepLessThanOrEqual(int value) {
            return new DirectSpecification<SystemPanel>(p => p.CurrentStep <= value);
        }
        public static Specification<SystemPanel> CurrentStepNotEqual(int value) {
            return new DirectSpecification<SystemPanel>(p => p.CurrentStep != value);
        }
        public static Specification<SystemPanel> CurrentStepGreaterThan(int value) {
            return new DirectSpecification<SystemPanel>(p => p.CurrentStep > value);
        }
        public static Specification<SystemPanel> CurrentStepLessThan(int value) {
            return new DirectSpecification<SystemPanel>(p => p.CurrentStep < value);
        }
		
					public static Specification<SystemPanel> MaxStepsContains(params int[] values) {
            return new DirectSpecification<SystemPanel>(p => values.Contains(p.MaxSteps.Value));
        }
		public static Specification<SystemPanel> MaxStepsNotContains(params int[] values) {
            return new DirectSpecification<SystemPanel>(p => !values.Contains(p.MaxSteps.Value));
        }
		public static Specification<SystemPanel> MaxStepsEqual(params int[] values) {
			return new DirectSpecification<SystemPanel>(p => values.Contains(p.MaxSteps.Value));
        }
        public static Specification<SystemPanel> MaxStepsGreaterThanOrEqual(int value) {
            return new DirectSpecification<SystemPanel>(p => p.MaxSteps >= value);
        }
        public static Specification<SystemPanel> MaxStepsLessThanOrEqual(int value) {
            return new DirectSpecification<SystemPanel>(p => p.MaxSteps <= value);
        }
        public static Specification<SystemPanel> MaxStepsNotEqual(int value) {
            return new DirectSpecification<SystemPanel>(p => p.MaxSteps != value);
        }
        public static Specification<SystemPanel> MaxStepsGreaterThan(int value) {
            return new DirectSpecification<SystemPanel>(p => p.MaxSteps > value);
        }
        public static Specification<SystemPanel> MaxStepsLessThan(int value) {
            return new DirectSpecification<SystemPanel>(p => p.MaxSteps < value);
        }
		
					public static Specification<SystemPanel> RegisterDoneEqual(bool value) {
			return new DirectSpecification<SystemPanel>(p => p.RegisterDone == value);
		}
		public static Specification<SystemPanel> RegisterDoneIsNull() {
            return new DirectSpecification<SystemPanel>(p => p.RegisterDone == null);
        }
		public static Specification<SystemPanel> RegisterDoneIsNotNull() {
            return new DirectSpecification<SystemPanel>(p => p.RegisterDone != null);
        }
		
					public static Specification<SystemPanel> ActiveEqual(bool value) {
			return new DirectSpecification<SystemPanel>(p => p.Active == value);
		}
		public static Specification<SystemPanel> ActiveIsNull() {
            return new DirectSpecification<SystemPanel>(p => p.Active == null);
        }
		public static Specification<SystemPanel> ActiveIsNotNull() {
            return new DirectSpecification<SystemPanel>(p => p.Active != null);
        }
		
					public static Specification<SystemPanel> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanel>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<SystemPanel> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanel>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<SystemPanel> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<SystemPanel>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<SystemPanel> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.CreatedAt >= value);
        }
        public static Specification<SystemPanel> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.CreatedAt <= value);
        }
        public static Specification<SystemPanel> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.CreatedAt != value);
        }
        public static Specification<SystemPanel> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.CreatedAt > value);
        }
        public static Specification<SystemPanel> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.CreatedAt < value);
        }
		
					public static Specification<SystemPanel> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanel>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<SystemPanel> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanel>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<SystemPanel> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<SystemPanel>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<SystemPanel> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.UpdatedAt >= value);
        }
        public static Specification<SystemPanel> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.UpdatedAt <= value);
        }
        public static Specification<SystemPanel> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.UpdatedAt != value);
        }
        public static Specification<SystemPanel> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.UpdatedAt > value);
        }
        public static Specification<SystemPanel> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.UpdatedAt < value);
        }
		
					public static Specification<SystemPanel> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanel>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<SystemPanel> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanel>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<SystemPanel> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<SystemPanel>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<SystemPanel> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.DeletedAt >= value);
        }
        public static Specification<SystemPanel> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.DeletedAt <= value);
        }
        public static Specification<SystemPanel> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.DeletedAt != value);
        }
        public static Specification<SystemPanel> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.DeletedAt > value);
        }
        public static Specification<SystemPanel> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<SystemPanel>(p => p.DeletedAt < value);
        }
		
					public static Specification<SystemPanel> IdContains(params int[] values) {
            return new DirectSpecification<SystemPanel>(p => values.Contains(p.Id));
        }
		public static Specification<SystemPanel> IdNotContains(params int[] values) {
            return new DirectSpecification<SystemPanel>(p => !values.Contains(p.Id));
        }
		public static Specification<SystemPanel> IdEqual(params int[] values) {
			return new DirectSpecification<SystemPanel>(p => values.Contains(p.Id));
        }
        public static Specification<SystemPanel> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<SystemPanel>(p => p.Id >= value);
        }
        public static Specification<SystemPanel> IdLessThanOrEqual(int value) {
            return new DirectSpecification<SystemPanel>(p => p.Id <= value);
        }
        public static Specification<SystemPanel> IdNotEqual(int value) {
            return new DirectSpecification<SystemPanel>(p => p.Id != value);
        }
        public static Specification<SystemPanel> IdGreaterThan(int value) {
            return new DirectSpecification<SystemPanel>(p => p.Id > value);
        }
        public static Specification<SystemPanel> IdLessThan(int value) {
            return new DirectSpecification<SystemPanel>(p => p.Id < value);
        }
		
					public static Specification<SystemPanel> ExternalIdContains(string value) {
			return new DirectSpecification<SystemPanel>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<SystemPanel> ExternalIdStartsWith(string value) {
			return new DirectSpecification<SystemPanel>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<SystemPanel> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<SystemPanel>(p => values.Contains(p.ExternalId));
		}
		public static Specification<SystemPanel> ExternalIdNotEqual(string value) {
			return new DirectSpecification<SystemPanel>(p => p.ExternalId != value);
		}
		public static Specification<SystemPanel> ExternalIdIsNull() {
            return new DirectSpecification<SystemPanel>(p => p.ExternalId == null);
        }
		public static Specification<SystemPanel> ExternalIdIsNotNull() {
            return new DirectSpecification<SystemPanel>(p => p.ExternalId != null);
        }
		
					public static Specification<SystemPanel> IsDeletedEqual(bool value) {
			return new DirectSpecification<SystemPanel>(p => p.IsDeleted == value);
		}
		
	   }
   public partial class SystemPanelGroupSpecifications {
				public static Specification<SystemPanelGroup> IconContains(string value) {
			return new DirectSpecification<SystemPanelGroup>(p => EF.Functions.Like(p.Icon.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<SystemPanelGroup> IconStartsWith(string value) {
			return new DirectSpecification<SystemPanelGroup>(p => EF.Functions.Like(p.Icon.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<SystemPanelGroup> IconEqual(params string[] values) {
			return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.Icon));
		}
		public static Specification<SystemPanelGroup> IconNotEqual(string value) {
			return new DirectSpecification<SystemPanelGroup>(p => p.Icon != value);
		}
		public static Specification<SystemPanelGroup> IconIsNull() {
            return new DirectSpecification<SystemPanelGroup>(p => p.Icon == null);
        }
		public static Specification<SystemPanelGroup> IconIsNotNull() {
            return new DirectSpecification<SystemPanelGroup>(p => p.Icon != null);
        }
		
					public static Specification<SystemPanelGroup> DescriptionContains(string value) {
			return new DirectSpecification<SystemPanelGroup>(p => EF.Functions.Like(p.Description.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<SystemPanelGroup> DescriptionStartsWith(string value) {
			return new DirectSpecification<SystemPanelGroup>(p => EF.Functions.Like(p.Description.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<SystemPanelGroup> DescriptionEqual(params string[] values) {
			return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.Description));
		}
		public static Specification<SystemPanelGroup> DescriptionNotEqual(string value) {
			return new DirectSpecification<SystemPanelGroup>(p => p.Description != value);
		}
		public static Specification<SystemPanelGroup> DescriptionIsNull() {
            return new DirectSpecification<SystemPanelGroup>(p => p.Description == null);
        }
		public static Specification<SystemPanelGroup> DescriptionIsNotNull() {
            return new DirectSpecification<SystemPanelGroup>(p => p.Description != null);
        }
		
					public static Specification<SystemPanelGroup> CodeContains(string value) {
			return new DirectSpecification<SystemPanelGroup>(p => EF.Functions.Like(p.Code.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<SystemPanelGroup> CodeStartsWith(string value) {
			return new DirectSpecification<SystemPanelGroup>(p => EF.Functions.Like(p.Code.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<SystemPanelGroup> CodeEqual(params string[] values) {
			return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.Code));
		}
		public static Specification<SystemPanelGroup> CodeNotEqual(string value) {
			return new DirectSpecification<SystemPanelGroup>(p => p.Code != value);
		}
		public static Specification<SystemPanelGroup> CodeIsNull() {
            return new DirectSpecification<SystemPanelGroup>(p => p.Code == null);
        }
		public static Specification<SystemPanelGroup> CodeIsNotNull() {
            return new DirectSpecification<SystemPanelGroup>(p => p.Code != null);
        }
		
				
				
					public static Specification<SystemPanelGroup> CurrentStepContains(params int[] values) {
            return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.CurrentStep));
        }
		public static Specification<SystemPanelGroup> CurrentStepNotContains(params int[] values) {
            return new DirectSpecification<SystemPanelGroup>(p => !values.Contains(p.CurrentStep));
        }
		public static Specification<SystemPanelGroup> CurrentStepEqual(params int[] values) {
			return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.CurrentStep));
        }
        public static Specification<SystemPanelGroup> CurrentStepGreaterThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.CurrentStep >= value);
        }
        public static Specification<SystemPanelGroup> CurrentStepLessThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.CurrentStep <= value);
        }
        public static Specification<SystemPanelGroup> CurrentStepNotEqual(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.CurrentStep != value);
        }
        public static Specification<SystemPanelGroup> CurrentStepGreaterThan(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.CurrentStep > value);
        }
        public static Specification<SystemPanelGroup> CurrentStepLessThan(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.CurrentStep < value);
        }
		
					public static Specification<SystemPanelGroup> MaxStepsContains(params int[] values) {
            return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.MaxSteps.Value));
        }
		public static Specification<SystemPanelGroup> MaxStepsNotContains(params int[] values) {
            return new DirectSpecification<SystemPanelGroup>(p => !values.Contains(p.MaxSteps.Value));
        }
		public static Specification<SystemPanelGroup> MaxStepsEqual(params int[] values) {
			return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.MaxSteps.Value));
        }
        public static Specification<SystemPanelGroup> MaxStepsGreaterThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.MaxSteps >= value);
        }
        public static Specification<SystemPanelGroup> MaxStepsLessThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.MaxSteps <= value);
        }
        public static Specification<SystemPanelGroup> MaxStepsNotEqual(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.MaxSteps != value);
        }
        public static Specification<SystemPanelGroup> MaxStepsGreaterThan(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.MaxSteps > value);
        }
        public static Specification<SystemPanelGroup> MaxStepsLessThan(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.MaxSteps < value);
        }
		
					public static Specification<SystemPanelGroup> RegisterDoneEqual(bool value) {
			return new DirectSpecification<SystemPanelGroup>(p => p.RegisterDone == value);
		}
		public static Specification<SystemPanelGroup> RegisterDoneIsNull() {
            return new DirectSpecification<SystemPanelGroup>(p => p.RegisterDone == null);
        }
		public static Specification<SystemPanelGroup> RegisterDoneIsNotNull() {
            return new DirectSpecification<SystemPanelGroup>(p => p.RegisterDone != null);
        }
		
					public static Specification<SystemPanelGroup> ActiveEqual(bool value) {
			return new DirectSpecification<SystemPanelGroup>(p => p.Active == value);
		}
		public static Specification<SystemPanelGroup> ActiveIsNull() {
            return new DirectSpecification<SystemPanelGroup>(p => p.Active == null);
        }
		public static Specification<SystemPanelGroup> ActiveIsNotNull() {
            return new DirectSpecification<SystemPanelGroup>(p => p.Active != null);
        }
		
					public static Specification<SystemPanelGroup> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<SystemPanelGroup> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanelGroup>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<SystemPanelGroup> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<SystemPanelGroup> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.CreatedAt >= value);
        }
        public static Specification<SystemPanelGroup> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.CreatedAt <= value);
        }
        public static Specification<SystemPanelGroup> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.CreatedAt != value);
        }
        public static Specification<SystemPanelGroup> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.CreatedAt > value);
        }
        public static Specification<SystemPanelGroup> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.CreatedAt < value);
        }
		
					public static Specification<SystemPanelGroup> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<SystemPanelGroup> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanelGroup>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<SystemPanelGroup> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<SystemPanelGroup> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.UpdatedAt >= value);
        }
        public static Specification<SystemPanelGroup> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.UpdatedAt <= value);
        }
        public static Specification<SystemPanelGroup> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.UpdatedAt != value);
        }
        public static Specification<SystemPanelGroup> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.UpdatedAt > value);
        }
        public static Specification<SystemPanelGroup> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.UpdatedAt < value);
        }
		
					public static Specification<SystemPanelGroup> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<SystemPanelGroup> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemPanelGroup>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<SystemPanelGroup> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<SystemPanelGroup> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.DeletedAt >= value);
        }
        public static Specification<SystemPanelGroup> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.DeletedAt <= value);
        }
        public static Specification<SystemPanelGroup> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.DeletedAt != value);
        }
        public static Specification<SystemPanelGroup> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.DeletedAt > value);
        }
        public static Specification<SystemPanelGroup> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.DeletedAt < value);
        }
		
					public static Specification<SystemPanelGroup> IdContains(params int[] values) {
            return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.Id));
        }
		public static Specification<SystemPanelGroup> IdNotContains(params int[] values) {
            return new DirectSpecification<SystemPanelGroup>(p => !values.Contains(p.Id));
        }
		public static Specification<SystemPanelGroup> IdEqual(params int[] values) {
			return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.Id));
        }
        public static Specification<SystemPanelGroup> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.Id >= value);
        }
        public static Specification<SystemPanelGroup> IdLessThanOrEqual(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.Id <= value);
        }
        public static Specification<SystemPanelGroup> IdNotEqual(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.Id != value);
        }
        public static Specification<SystemPanelGroup> IdGreaterThan(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.Id > value);
        }
        public static Specification<SystemPanelGroup> IdLessThan(int value) {
            return new DirectSpecification<SystemPanelGroup>(p => p.Id < value);
        }
		
					public static Specification<SystemPanelGroup> ExternalIdContains(string value) {
			return new DirectSpecification<SystemPanelGroup>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<SystemPanelGroup> ExternalIdStartsWith(string value) {
			return new DirectSpecification<SystemPanelGroup>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<SystemPanelGroup> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<SystemPanelGroup>(p => values.Contains(p.ExternalId));
		}
		public static Specification<SystemPanelGroup> ExternalIdNotEqual(string value) {
			return new DirectSpecification<SystemPanelGroup>(p => p.ExternalId != value);
		}
		public static Specification<SystemPanelGroup> ExternalIdIsNull() {
            return new DirectSpecification<SystemPanelGroup>(p => p.ExternalId == null);
        }
		public static Specification<SystemPanelGroup> ExternalIdIsNotNull() {
            return new DirectSpecification<SystemPanelGroup>(p => p.ExternalId != null);
        }
		
					public static Specification<SystemPanelGroup> IsDeletedEqual(bool value) {
			return new DirectSpecification<SystemPanelGroup>(p => p.IsDeleted == value);
		}
		
	   }
   public partial class CargaTabelaSpecifications {
				public static Specification<CargaTabela> TableNameContains(string value) {
			return new DirectSpecification<CargaTabela>(p => EF.Functions.Like(p.TableName.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<CargaTabela> TableNameStartsWith(string value) {
			return new DirectSpecification<CargaTabela>(p => EF.Functions.Like(p.TableName.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<CargaTabela> TableNameEqual(params string[] values) {
			return new DirectSpecification<CargaTabela>(p => values.Contains(p.TableName));
		}
		public static Specification<CargaTabela> TableNameNotEqual(string value) {
			return new DirectSpecification<CargaTabela>(p => p.TableName != value);
		}
		public static Specification<CargaTabela> TableNameIsNull() {
            return new DirectSpecification<CargaTabela>(p => p.TableName == null);
        }
		public static Specification<CargaTabela> TableNameIsNotNull() {
            return new DirectSpecification<CargaTabela>(p => p.TableName != null);
        }
		
					public static Specification<CargaTabela> FilePathContains(string value) {
			return new DirectSpecification<CargaTabela>(p => EF.Functions.Like(p.FilePath.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<CargaTabela> FilePathStartsWith(string value) {
			return new DirectSpecification<CargaTabela>(p => EF.Functions.Like(p.FilePath.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<CargaTabela> FilePathEqual(params string[] values) {
			return new DirectSpecification<CargaTabela>(p => values.Contains(p.FilePath));
		}
		public static Specification<CargaTabela> FilePathNotEqual(string value) {
			return new DirectSpecification<CargaTabela>(p => p.FilePath != value);
		}
		public static Specification<CargaTabela> FilePathIsNull() {
            return new DirectSpecification<CargaTabela>(p => p.FilePath == null);
        }
		public static Specification<CargaTabela> FilePathIsNotNull() {
            return new DirectSpecification<CargaTabela>(p => p.FilePath != null);
        }
		
					public static Specification<CargaTabela> IsInitializedEqual(bool value) {
			return new DirectSpecification<CargaTabela>(p => p.IsInitialized == value);
		}
		
							public static Specification<CargaTabela> TotalContains(params int[] values) {
            return new DirectSpecification<CargaTabela>(p => values.Contains(p.Total.Value));
        }
		public static Specification<CargaTabela> TotalNotContains(params int[] values) {
            return new DirectSpecification<CargaTabela>(p => !values.Contains(p.Total.Value));
        }
		public static Specification<CargaTabela> TotalEqual(params int[] values) {
			return new DirectSpecification<CargaTabela>(p => values.Contains(p.Total.Value));
        }
        public static Specification<CargaTabela> TotalGreaterThanOrEqual(int value) {
            return new DirectSpecification<CargaTabela>(p => p.Total >= value);
        }
        public static Specification<CargaTabela> TotalLessThanOrEqual(int value) {
            return new DirectSpecification<CargaTabela>(p => p.Total <= value);
        }
        public static Specification<CargaTabela> TotalNotEqual(int value) {
            return new DirectSpecification<CargaTabela>(p => p.Total != value);
        }
        public static Specification<CargaTabela> TotalGreaterThan(int value) {
            return new DirectSpecification<CargaTabela>(p => p.Total > value);
        }
        public static Specification<CargaTabela> TotalLessThan(int value) {
            return new DirectSpecification<CargaTabela>(p => p.Total < value);
        }
		
					public static Specification<CargaTabela> CurrentStepContains(params int[] values) {
            return new DirectSpecification<CargaTabela>(p => values.Contains(p.CurrentStep));
        }
		public static Specification<CargaTabela> CurrentStepNotContains(params int[] values) {
            return new DirectSpecification<CargaTabela>(p => !values.Contains(p.CurrentStep));
        }
		public static Specification<CargaTabela> CurrentStepEqual(params int[] values) {
			return new DirectSpecification<CargaTabela>(p => values.Contains(p.CurrentStep));
        }
        public static Specification<CargaTabela> CurrentStepGreaterThanOrEqual(int value) {
            return new DirectSpecification<CargaTabela>(p => p.CurrentStep >= value);
        }
        public static Specification<CargaTabela> CurrentStepLessThanOrEqual(int value) {
            return new DirectSpecification<CargaTabela>(p => p.CurrentStep <= value);
        }
        public static Specification<CargaTabela> CurrentStepNotEqual(int value) {
            return new DirectSpecification<CargaTabela>(p => p.CurrentStep != value);
        }
        public static Specification<CargaTabela> CurrentStepGreaterThan(int value) {
            return new DirectSpecification<CargaTabela>(p => p.CurrentStep > value);
        }
        public static Specification<CargaTabela> CurrentStepLessThan(int value) {
            return new DirectSpecification<CargaTabela>(p => p.CurrentStep < value);
        }
		
					public static Specification<CargaTabela> MaxStepsContains(params int[] values) {
            return new DirectSpecification<CargaTabela>(p => values.Contains(p.MaxSteps.Value));
        }
		public static Specification<CargaTabela> MaxStepsNotContains(params int[] values) {
            return new DirectSpecification<CargaTabela>(p => !values.Contains(p.MaxSteps.Value));
        }
		public static Specification<CargaTabela> MaxStepsEqual(params int[] values) {
			return new DirectSpecification<CargaTabela>(p => values.Contains(p.MaxSteps.Value));
        }
        public static Specification<CargaTabela> MaxStepsGreaterThanOrEqual(int value) {
            return new DirectSpecification<CargaTabela>(p => p.MaxSteps >= value);
        }
        public static Specification<CargaTabela> MaxStepsLessThanOrEqual(int value) {
            return new DirectSpecification<CargaTabela>(p => p.MaxSteps <= value);
        }
        public static Specification<CargaTabela> MaxStepsNotEqual(int value) {
            return new DirectSpecification<CargaTabela>(p => p.MaxSteps != value);
        }
        public static Specification<CargaTabela> MaxStepsGreaterThan(int value) {
            return new DirectSpecification<CargaTabela>(p => p.MaxSteps > value);
        }
        public static Specification<CargaTabela> MaxStepsLessThan(int value) {
            return new DirectSpecification<CargaTabela>(p => p.MaxSteps < value);
        }
		
					public static Specification<CargaTabela> RegisterDoneEqual(bool value) {
			return new DirectSpecification<CargaTabela>(p => p.RegisterDone == value);
		}
		public static Specification<CargaTabela> RegisterDoneIsNull() {
            return new DirectSpecification<CargaTabela>(p => p.RegisterDone == null);
        }
		public static Specification<CargaTabela> RegisterDoneIsNotNull() {
            return new DirectSpecification<CargaTabela>(p => p.RegisterDone != null);
        }
		
					public static Specification<CargaTabela> ActiveEqual(bool value) {
			return new DirectSpecification<CargaTabela>(p => p.Active == value);
		}
		public static Specification<CargaTabela> ActiveIsNull() {
            return new DirectSpecification<CargaTabela>(p => p.Active == null);
        }
		public static Specification<CargaTabela> ActiveIsNotNull() {
            return new DirectSpecification<CargaTabela>(p => p.Active != null);
        }
		
					public static Specification<CargaTabela> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<CargaTabela>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<CargaTabela> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<CargaTabela>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<CargaTabela> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<CargaTabela>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<CargaTabela> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.CreatedAt >= value);
        }
        public static Specification<CargaTabela> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.CreatedAt <= value);
        }
        public static Specification<CargaTabela> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.CreatedAt != value);
        }
        public static Specification<CargaTabela> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.CreatedAt > value);
        }
        public static Specification<CargaTabela> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.CreatedAt < value);
        }
		
					public static Specification<CargaTabela> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<CargaTabela>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<CargaTabela> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<CargaTabela>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<CargaTabela> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<CargaTabela>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<CargaTabela> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.UpdatedAt >= value);
        }
        public static Specification<CargaTabela> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.UpdatedAt <= value);
        }
        public static Specification<CargaTabela> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.UpdatedAt != value);
        }
        public static Specification<CargaTabela> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.UpdatedAt > value);
        }
        public static Specification<CargaTabela> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.UpdatedAt < value);
        }
		
					public static Specification<CargaTabela> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<CargaTabela>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<CargaTabela> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<CargaTabela>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<CargaTabela> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<CargaTabela>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<CargaTabela> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.DeletedAt >= value);
        }
        public static Specification<CargaTabela> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.DeletedAt <= value);
        }
        public static Specification<CargaTabela> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.DeletedAt != value);
        }
        public static Specification<CargaTabela> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.DeletedAt > value);
        }
        public static Specification<CargaTabela> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<CargaTabela>(p => p.DeletedAt < value);
        }
		
					public static Specification<CargaTabela> IdContains(params int[] values) {
            return new DirectSpecification<CargaTabela>(p => values.Contains(p.Id));
        }
		public static Specification<CargaTabela> IdNotContains(params int[] values) {
            return new DirectSpecification<CargaTabela>(p => !values.Contains(p.Id));
        }
		public static Specification<CargaTabela> IdEqual(params int[] values) {
			return new DirectSpecification<CargaTabela>(p => values.Contains(p.Id));
        }
        public static Specification<CargaTabela> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<CargaTabela>(p => p.Id >= value);
        }
        public static Specification<CargaTabela> IdLessThanOrEqual(int value) {
            return new DirectSpecification<CargaTabela>(p => p.Id <= value);
        }
        public static Specification<CargaTabela> IdNotEqual(int value) {
            return new DirectSpecification<CargaTabela>(p => p.Id != value);
        }
        public static Specification<CargaTabela> IdGreaterThan(int value) {
            return new DirectSpecification<CargaTabela>(p => p.Id > value);
        }
        public static Specification<CargaTabela> IdLessThan(int value) {
            return new DirectSpecification<CargaTabela>(p => p.Id < value);
        }
		
					public static Specification<CargaTabela> ExternalIdContains(string value) {
			return new DirectSpecification<CargaTabela>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<CargaTabela> ExternalIdStartsWith(string value) {
			return new DirectSpecification<CargaTabela>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<CargaTabela> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<CargaTabela>(p => values.Contains(p.ExternalId));
		}
		public static Specification<CargaTabela> ExternalIdNotEqual(string value) {
			return new DirectSpecification<CargaTabela>(p => p.ExternalId != value);
		}
		public static Specification<CargaTabela> ExternalIdIsNull() {
            return new DirectSpecification<CargaTabela>(p => p.ExternalId == null);
        }
		public static Specification<CargaTabela> ExternalIdIsNotNull() {
            return new DirectSpecification<CargaTabela>(p => p.ExternalId != null);
        }
		
					public static Specification<CargaTabela> IsDeletedEqual(bool value) {
			return new DirectSpecification<CargaTabela>(p => p.IsDeleted == value);
		}
		
	   }
   public partial class SystemSettingsAggSettingsSpecifications {
				public static Specification<SystemSettingsAggSettings> AutoSaveSettingsEnabledEqual(bool value) {
			return new DirectSpecification<SystemSettingsAggSettings>(p => p.AutoSaveSettingsEnabled == value);
		}
		
					public static Specification<SystemSettingsAggSettings> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<SystemSettingsAggSettings> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<SystemSettingsAggSettings> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<SystemSettingsAggSettings>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<SystemSettingsAggSettings> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.CreatedAt >= value);
        }
        public static Specification<SystemSettingsAggSettings> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.CreatedAt <= value);
        }
        public static Specification<SystemSettingsAggSettings> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.CreatedAt != value);
        }
        public static Specification<SystemSettingsAggSettings> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.CreatedAt > value);
        }
        public static Specification<SystemSettingsAggSettings> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.CreatedAt < value);
        }
		
					public static Specification<SystemSettingsAggSettings> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<SystemSettingsAggSettings> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<SystemSettingsAggSettings> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<SystemSettingsAggSettings>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<SystemSettingsAggSettings> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.UpdatedAt >= value);
        }
        public static Specification<SystemSettingsAggSettings> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.UpdatedAt <= value);
        }
        public static Specification<SystemSettingsAggSettings> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.UpdatedAt != value);
        }
        public static Specification<SystemSettingsAggSettings> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.UpdatedAt > value);
        }
        public static Specification<SystemSettingsAggSettings> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.UpdatedAt < value);
        }
		
					public static Specification<SystemSettingsAggSettings> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<SystemSettingsAggSettings> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<SystemSettingsAggSettings> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<SystemSettingsAggSettings>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<SystemSettingsAggSettings> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.DeletedAt >= value);
        }
        public static Specification<SystemSettingsAggSettings> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.DeletedAt <= value);
        }
        public static Specification<SystemSettingsAggSettings> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.DeletedAt != value);
        }
        public static Specification<SystemSettingsAggSettings> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.DeletedAt > value);
        }
        public static Specification<SystemSettingsAggSettings> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.DeletedAt < value);
        }
		
					public static Specification<SystemSettingsAggSettings> IdContains(params int[] values) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => values.Contains(p.Id));
        }
		public static Specification<SystemSettingsAggSettings> IdNotContains(params int[] values) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => !values.Contains(p.Id));
        }
		public static Specification<SystemSettingsAggSettings> IdEqual(params int[] values) {
			return new DirectSpecification<SystemSettingsAggSettings>(p => values.Contains(p.Id));
        }
        public static Specification<SystemSettingsAggSettings> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.Id >= value);
        }
        public static Specification<SystemSettingsAggSettings> IdLessThanOrEqual(int value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.Id <= value);
        }
        public static Specification<SystemSettingsAggSettings> IdNotEqual(int value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.Id != value);
        }
        public static Specification<SystemSettingsAggSettings> IdGreaterThan(int value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.Id > value);
        }
        public static Specification<SystemSettingsAggSettings> IdLessThan(int value) {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.Id < value);
        }
		
					public static Specification<SystemSettingsAggSettings> ExternalIdContains(string value) {
			return new DirectSpecification<SystemSettingsAggSettings>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<SystemSettingsAggSettings> ExternalIdStartsWith(string value) {
			return new DirectSpecification<SystemSettingsAggSettings>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<SystemSettingsAggSettings> ExternalIdEqual(params string[] values) {
			return new DirectSpecification<SystemSettingsAggSettings>(p => values.Contains(p.ExternalId));
		}
		public static Specification<SystemSettingsAggSettings> ExternalIdNotEqual(string value) {
			return new DirectSpecification<SystemSettingsAggSettings>(p => p.ExternalId != value);
		}
		public static Specification<SystemSettingsAggSettings> ExternalIdIsNull() {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.ExternalId == null);
        }
		public static Specification<SystemSettingsAggSettings> ExternalIdIsNotNull() {
            return new DirectSpecification<SystemSettingsAggSettings>(p => p.ExternalId != null);
        }
		
					public static Specification<SystemSettingsAggSettings> IsDeletedEqual(bool value) {
			return new DirectSpecification<SystemSettingsAggSettings>(p => p.IsDeleted == value);
		}
		
	   }
}
