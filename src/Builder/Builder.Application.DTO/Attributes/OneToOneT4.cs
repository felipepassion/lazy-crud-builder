using System;

namespace Lazy.Crud.Builder.Domain.Attributes.T4
{
    /// <summary>
    /// Marks a one-to-one relationship between entities for T4 generation and mapping.
    /// </summary>
    /// <category>Modeling</category>
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
