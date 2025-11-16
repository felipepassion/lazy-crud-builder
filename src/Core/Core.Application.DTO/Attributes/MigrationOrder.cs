using Lazy.Crud.Core.Application.DTO.Attributes;
using System;

namespace Lazy.Crud.Core.Application.DTO.Attributes
{
    /// <summary>
    /// Specifies the execution order for migrations generated from aggregates or seed files.
    /// </summary>
    /// <category>Code Generation (T4)</category>
    public class MigrationOrder : Attribute
    {
        /// <summary>
        /// Gets or sets the order in which the migration should be executed.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationOrder"/> attribute with the specified order.
        /// </summary>
        /// <param name="order">The execution order value.</param>
        public MigrationOrder(int order)
        {
            Order = order;
        }
    }
}
