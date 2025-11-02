using Niu.Nutri.Core.Application.DTO.Attributes;
using System;

/// <summary>
/// Specifies the execution order for migrations generated from aggregates or seed files.
/// </summary>
namespace Niu.Nutri.Core.Domain.Attributes.T4
{
    /// <summary>
    /// Specifies the execution order for migrations generated from aggregates or seed files.
    /// </summary>
    [H1("Migration Order")]
    [Category("Code Generation (T4)", "Defines execution order for generated migrations/seeds.")]
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
