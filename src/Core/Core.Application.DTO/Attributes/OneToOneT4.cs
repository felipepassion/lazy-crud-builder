using System;
using Lazy.Crud.Core.Application.DTO.Attributes;

namespace Lazy.Crud.Core.Domain.Attributes.T4
{
    /// <summary>
    /// Marks a one-to-one relationship between entities for T4 generation and mapping.
    /// </summary>
    [H1("One-to-One")]
    [Category("Modeling", "One-to-one relationship flagged for T4 generation/mapping.")]
    public class OneToOne : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OneToOne"/> attribute.
        /// </summary>
        public OneToOne()
        {
        }
    }
}
