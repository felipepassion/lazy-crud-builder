using Niu.Nutri.Core.Domain.Seedwork.Specification;
using Microsoft.EntityFrameworkCore;
namespace Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Specifications {
	using Entities;
   public partial class DefaultEntitySpecifications {
				public static Specification<DefaultEntity> ExternalIdContains(string value) {
			return new DirectSpecification<DefaultEntity>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<DefaultEntity> ExternalIdNotContains(string value) {
			return new DirectSpecification<DefaultEntity>(p => !EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<DefaultEntity> ExternalIdStartsWith(string value) {
			return new DirectSpecification<DefaultEntity>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<DefaultEntity> ExternalIdEqual(string value) {
			return new DirectSpecification<DefaultEntity>(p => value.ToLower() == (p.ExternalId.ToLower()));
		}
		public static Specification<DefaultEntity> ExternalIdNotEqual(string value) {
			return new DirectSpecification<DefaultEntity>(p => p.ExternalId != value);
		}
		public static Specification<DefaultEntity> ExternalIdIsNull() {
            return new DirectSpecification<DefaultEntity>(p => p.ExternalId == null);
        }
		public static Specification<DefaultEntity> ExternalIdIsNotNull() {
            return new DirectSpecification<DefaultEntity>(p => p.ExternalId != null);
        }
		
					public static Specification<DefaultEntity> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<DefaultEntity>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<DefaultEntity> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<DefaultEntity>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<DefaultEntity> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<DefaultEntity>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<DefaultEntity> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.CreatedAt >= value);
        }
        public static Specification<DefaultEntity> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.CreatedAt <= value);
        }
        public static Specification<DefaultEntity> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.CreatedAt != value);
        }
        public static Specification<DefaultEntity> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.CreatedAt > value);
        }
        public static Specification<DefaultEntity> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.CreatedAt < value);
        }
		
					public static Specification<DefaultEntity> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<DefaultEntity>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<DefaultEntity> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<DefaultEntity>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<DefaultEntity> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<DefaultEntity>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<DefaultEntity> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.UpdatedAt >= value);
        }
        public static Specification<DefaultEntity> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.UpdatedAt <= value);
        }
        public static Specification<DefaultEntity> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.UpdatedAt != value);
        }
        public static Specification<DefaultEntity> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.UpdatedAt > value);
        }
        public static Specification<DefaultEntity> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.UpdatedAt < value);
        }
		
					public static Specification<DefaultEntity> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<DefaultEntity>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<DefaultEntity> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<DefaultEntity>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<DefaultEntity> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<DefaultEntity>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<DefaultEntity> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.DeletedAt >= value);
        }
        public static Specification<DefaultEntity> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.DeletedAt <= value);
        }
        public static Specification<DefaultEntity> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.DeletedAt != value);
        }
        public static Specification<DefaultEntity> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.DeletedAt > value);
        }
        public static Specification<DefaultEntity> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<DefaultEntity>(p => p.DeletedAt < value);
        }
		
					public static Specification<DefaultEntity> IdContains(params int[] values) {
            return new DirectSpecification<DefaultEntity>(p => values.Contains(p.Id));
        }
		public static Specification<DefaultEntity> IdNotContains(params int[] values) {
            return new DirectSpecification<DefaultEntity>(p => !values.Contains(p.Id));
        }
		public static Specification<DefaultEntity> IdEqual(params int[] values) {
			return new DirectSpecification<DefaultEntity>(p => values.Contains(p.Id));
        }
        public static Specification<DefaultEntity> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<DefaultEntity>(p => p.Id >= value);
        }
        public static Specification<DefaultEntity> IdLessThanOrEqual(int value) {
            return new DirectSpecification<DefaultEntity>(p => p.Id <= value);
        }
        public static Specification<DefaultEntity> IdNotEqual(int value) {
            return new DirectSpecification<DefaultEntity>(p => p.Id != value);
        }
        public static Specification<DefaultEntity> IdGreaterThan(int value) {
            return new DirectSpecification<DefaultEntity>(p => p.Id > value);
        }
        public static Specification<DefaultEntity> IdLessThan(int value) {
            return new DirectSpecification<DefaultEntity>(p => p.Id < value);
        }
		
					public static Specification<DefaultEntity> IsDeletedEqual(bool value) {
			return new DirectSpecification<DefaultEntity>(p => p.IsDeleted == value);
		}
		
	   }
   public partial class DefaultTemplateAggSettingsSpecifications {
				public static Specification<DefaultTemplateAggSettings> UserIdContains(params int[] values) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => values.Contains(p.UserId));
        }
		public static Specification<DefaultTemplateAggSettings> UserIdNotContains(params int[] values) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => !values.Contains(p.UserId));
        }
		public static Specification<DefaultTemplateAggSettings> UserIdEqual(params int[] values) {
			return new DirectSpecification<DefaultTemplateAggSettings>(p => values.Contains(p.UserId));
        }
        public static Specification<DefaultTemplateAggSettings> UserIdGreaterThanOrEqual(int value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.UserId >= value);
        }
        public static Specification<DefaultTemplateAggSettings> UserIdLessThanOrEqual(int value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.UserId <= value);
        }
        public static Specification<DefaultTemplateAggSettings> UserIdNotEqual(int value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.UserId != value);
        }
        public static Specification<DefaultTemplateAggSettings> UserIdGreaterThan(int value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.UserId > value);
        }
        public static Specification<DefaultTemplateAggSettings> UserIdLessThan(int value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.UserId < value);
        }
		
					public static Specification<DefaultTemplateAggSettings> AutoSaveSettingsEnabledEqual(bool value) {
			return new DirectSpecification<DefaultTemplateAggSettings>(p => p.AutoSaveSettingsEnabled == value);
		}
		
					public static Specification<DefaultTemplateAggSettings> ExternalIdContains(string value) {
			return new DirectSpecification<DefaultTemplateAggSettings>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<DefaultTemplateAggSettings> ExternalIdNotContains(string value) {
			return new DirectSpecification<DefaultTemplateAggSettings>(p => !EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<DefaultTemplateAggSettings> ExternalIdStartsWith(string value) {
			return new DirectSpecification<DefaultTemplateAggSettings>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<DefaultTemplateAggSettings> ExternalIdEqual(string value) {
			return new DirectSpecification<DefaultTemplateAggSettings>(p => value.ToLower() == (p.ExternalId.ToLower()));
		}
		public static Specification<DefaultTemplateAggSettings> ExternalIdNotEqual(string value) {
			return new DirectSpecification<DefaultTemplateAggSettings>(p => p.ExternalId != value);
		}
		public static Specification<DefaultTemplateAggSettings> ExternalIdIsNull() {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.ExternalId == null);
        }
		public static Specification<DefaultTemplateAggSettings> ExternalIdIsNotNull() {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.ExternalId != null);
        }
		
					public static Specification<DefaultTemplateAggSettings> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<DefaultTemplateAggSettings> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<DefaultTemplateAggSettings> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<DefaultTemplateAggSettings>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<DefaultTemplateAggSettings> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.CreatedAt >= value);
        }
        public static Specification<DefaultTemplateAggSettings> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.CreatedAt <= value);
        }
        public static Specification<DefaultTemplateAggSettings> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.CreatedAt != value);
        }
        public static Specification<DefaultTemplateAggSettings> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.CreatedAt > value);
        }
        public static Specification<DefaultTemplateAggSettings> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.CreatedAt < value);
        }
		
					public static Specification<DefaultTemplateAggSettings> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<DefaultTemplateAggSettings> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<DefaultTemplateAggSettings> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<DefaultTemplateAggSettings>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<DefaultTemplateAggSettings> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.UpdatedAt >= value);
        }
        public static Specification<DefaultTemplateAggSettings> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.UpdatedAt <= value);
        }
        public static Specification<DefaultTemplateAggSettings> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.UpdatedAt != value);
        }
        public static Specification<DefaultTemplateAggSettings> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.UpdatedAt > value);
        }
        public static Specification<DefaultTemplateAggSettings> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.UpdatedAt < value);
        }
		
					public static Specification<DefaultTemplateAggSettings> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<DefaultTemplateAggSettings> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<DefaultTemplateAggSettings> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<DefaultTemplateAggSettings>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<DefaultTemplateAggSettings> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.DeletedAt >= value);
        }
        public static Specification<DefaultTemplateAggSettings> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.DeletedAt <= value);
        }
        public static Specification<DefaultTemplateAggSettings> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.DeletedAt != value);
        }
        public static Specification<DefaultTemplateAggSettings> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.DeletedAt > value);
        }
        public static Specification<DefaultTemplateAggSettings> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.DeletedAt < value);
        }
		
					public static Specification<DefaultTemplateAggSettings> IdContains(params int[] values) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => values.Contains(p.Id));
        }
		public static Specification<DefaultTemplateAggSettings> IdNotContains(params int[] values) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => !values.Contains(p.Id));
        }
		public static Specification<DefaultTemplateAggSettings> IdEqual(params int[] values) {
			return new DirectSpecification<DefaultTemplateAggSettings>(p => values.Contains(p.Id));
        }
        public static Specification<DefaultTemplateAggSettings> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.Id >= value);
        }
        public static Specification<DefaultTemplateAggSettings> IdLessThanOrEqual(int value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.Id <= value);
        }
        public static Specification<DefaultTemplateAggSettings> IdNotEqual(int value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.Id != value);
        }
        public static Specification<DefaultTemplateAggSettings> IdGreaterThan(int value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.Id > value);
        }
        public static Specification<DefaultTemplateAggSettings> IdLessThan(int value) {
            return new DirectSpecification<DefaultTemplateAggSettings>(p => p.Id < value);
        }
		
					public static Specification<DefaultTemplateAggSettings> IsDeletedEqual(bool value) {
			return new DirectSpecification<DefaultTemplateAggSettings>(p => p.IsDeleted == value);
		}
		
	   }
}
