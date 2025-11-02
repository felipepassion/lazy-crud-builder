using System;
using System.Linq.Expressions;

namespace Niu.Nutri.Core.Domain.Seedwork.Specification {

    /// <summary>
    /// True specification
    /// </summary>
    /// <typeparam name="TEntity">Type of entity in this specification</typeparam>
    public sealed class TrueSpecification<TEntity> : Specification<TEntity> where TEntity : class {

        #region Specification overrides

        /// <summary>
        /// <see cref="BPay.Core.Domain.Seedwork.Specifications.ISpecification{TEntity}"/>
        /// </summary>
        /// <returns><see cref="BPay.Core.Domain.Seedwork.Specifications.ISpecification{TEntity}"/></returns>
        public override System.Linq.Expressions.Expression<Func<TEntity, bool>> SatisfiedBy() {
            //Create "result variable" transform adhoc execution plan in prepared plan
            //for more info: http://geeks.ms/blogs/unai/2010/07/91/ef-4-0-performance-tips-1.aspx
            bool result = true;
            
            Expression<Func<TEntity, bool>> trueExpression = t => result;
            return trueExpression;
        }

        #endregion
    }
}
