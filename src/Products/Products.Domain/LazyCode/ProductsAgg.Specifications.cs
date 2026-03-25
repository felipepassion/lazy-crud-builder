// Removed global usings now in global file
namespace Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Specifications {
using Entities;
public partial class ProductsSpecifications {
				public static Specification<Products> TestPropertyContains(string value) {
			return new DirectSpecification<Products>(p => EF.Functions.Like(p.TestProperty.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<Products> TestPropertyNotContains(string value) {
			return new DirectSpecification<Products>(p => !EF.Functions.Like(p.TestProperty.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<Products> TestPropertyStartsWith(string value) {
			return new DirectSpecification<Products>(p => EF.Functions.Like(p.TestProperty.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<Products> TestPropertyEqual(string value) {
			return new DirectSpecification<Products>(p => value.ToLower() == (p.TestProperty.ToLower()));
		}
		public static Specification<Products> TestPropertyNotEqual(string value) {
			return new DirectSpecification<Products>(p => p.TestProperty != value);
		}
		public static Specification<Products> TestPropertyIsNull() {
            return new DirectSpecification<Products>(p => p.TestProperty == null);
        }
		public static Specification<Products> TestPropertyIsNotNull() {
            return new DirectSpecification<Products>(p => p.TestProperty != null);
        }
		
					public static Specification<Products> ExternalIdContains(string value) {
			return new DirectSpecification<Products>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<Products> ExternalIdNotContains(string value) {
			return new DirectSpecification<Products>(p => !EF.Functions.Like(p.ExternalId.ToLower(), $"%{value.ToLower()}%"));
		}
		public static Specification<Products> ExternalIdStartsWith(string value) {
			return new DirectSpecification<Products>(p => EF.Functions.Like(p.ExternalId.ToLower(), $"{value.ToLower()}%"));
		}
	
		public static Specification<Products> ExternalIdEqual(string value) {
			return new DirectSpecification<Products>(p => value.ToLower() == (p.ExternalId.ToLower()));
		}
		public static Specification<Products> ExternalIdNotEqual(string value) {
			return new DirectSpecification<Products>(p => p.ExternalId != value);
		}
		public static Specification<Products> ExternalIdIsNull() {
            return new DirectSpecification<Products>(p => p.ExternalId == null);
        }
		public static Specification<Products> ExternalIdIsNotNull() {
            return new DirectSpecification<Products>(p => p.ExternalId != null);
        }
		
					public static Specification<Products> CreatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<Products>(p => values.Contains(p.CreatedAt.Value));
        }
		public static Specification<Products> CreatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<Products>(p => !values.Contains(p.CreatedAt.Value));
        }
		public static Specification<Products> CreatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<Products>(p => values.Contains(p.CreatedAt.Value));
        }
        public static Specification<Products> CreatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.CreatedAt >= value);
        }
        public static Specification<Products> CreatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.CreatedAt <= value);
        }
        public static Specification<Products> CreatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.CreatedAt != value);
        }
        public static Specification<Products> CreatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.CreatedAt > value);
        }
        public static Specification<Products> CreatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.CreatedAt < value);
        }
		
					public static Specification<Products> UpdatedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<Products>(p => values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<Products> UpdatedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<Products>(p => !values.Contains(p.UpdatedAt.Value));
        }
		public static Specification<Products> UpdatedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<Products>(p => values.Contains(p.UpdatedAt.Value));
        }
        public static Specification<Products> UpdatedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.UpdatedAt >= value);
        }
        public static Specification<Products> UpdatedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.UpdatedAt <= value);
        }
        public static Specification<Products> UpdatedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.UpdatedAt != value);
        }
        public static Specification<Products> UpdatedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.UpdatedAt > value);
        }
        public static Specification<Products> UpdatedAtLessThan(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.UpdatedAt < value);
        }
		
					public static Specification<Products> DeletedAtContains(params System.DateTime[] values) {
            return new DirectSpecification<Products>(p => values.Contains(p.DeletedAt.Value));
        }
		public static Specification<Products> DeletedAtNotContains(params System.DateTime[] values) {
            return new DirectSpecification<Products>(p => !values.Contains(p.DeletedAt.Value));
        }
		public static Specification<Products> DeletedAtEqual(params System.DateTime[] values) {
			return new DirectSpecification<Products>(p => values.Contains(p.DeletedAt.Value));
        }
        public static Specification<Products> DeletedAtGreaterThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.DeletedAt >= value);
        }
        public static Specification<Products> DeletedAtLessThanOrEqual(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.DeletedAt <= value);
        }
        public static Specification<Products> DeletedAtNotEqual(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.DeletedAt != value);
        }
        public static Specification<Products> DeletedAtGreaterThan(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.DeletedAt > value);
        }
        public static Specification<Products> DeletedAtLessThan(System.DateTime value) {
            return new DirectSpecification<Products>(p => p.DeletedAt < value);
        }
		
					public static Specification<Products> IdContains(params int[] values) {
            return new DirectSpecification<Products>(p => values.Contains(p.Id));
        }
		public static Specification<Products> IdNotContains(params int[] values) {
            return new DirectSpecification<Products>(p => !values.Contains(p.Id));
        }
		public static Specification<Products> IdEqual(params int[] values) {
			return new DirectSpecification<Products>(p => values.Contains(p.Id));
        }
        public static Specification<Products> IdGreaterThanOrEqual(int value) {
            return new DirectSpecification<Products>(p => p.Id >= value);
        }
        public static Specification<Products> IdLessThanOrEqual(int value) {
            return new DirectSpecification<Products>(p => p.Id <= value);
        }
        public static Specification<Products> IdNotEqual(int value) {
            return new DirectSpecification<Products>(p => p.Id != value);
        }
        public static Specification<Products> IdGreaterThan(int value) {
            return new DirectSpecification<Products>(p => p.Id > value);
        }
        public static Specification<Products> IdLessThan(int value) {
            return new DirectSpecification<Products>(p => p.Id < value);
        }
		
					public static Specification<Products> IsDeletedEqual(bool value) {
			return new DirectSpecification<Products>(p => p.IsDeleted == value);
		}
		
	}
}
