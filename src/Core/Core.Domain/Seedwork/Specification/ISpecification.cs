using System;
using System.Linq.Expressions;

namespace Niu.Nutri.Core.Domain.Seedwork.Specification {

    /// <summary>
    /// http://en.wikipedia.org/wiki/Specification_pattern
    /// </summary>
    /// <typeparam name="T">Type of entity</typeparam>
    public interface ISpecification<T> where T : class {
        
        /// <summary>
        /// Check if this specification is satisfied by a 
        /// specific expression lambda
        /// </summary>
        /// <returns></returns>
        Expression<Func<T, bool>> SatisfiedBy();
    }
}
