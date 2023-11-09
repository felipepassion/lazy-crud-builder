using LazyCrud.Core.Domain.Seedwork.Specification;
using Microsoft.EntityFrameworkCore;
namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Specifications {
	using Entities;
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
}
