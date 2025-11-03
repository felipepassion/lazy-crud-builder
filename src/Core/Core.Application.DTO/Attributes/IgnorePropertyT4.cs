using System;
using Lazy.Crud.Core.Application.DTO.Attributes;

namespace Lazy.Crud.Core.Domain.Attributes.T4
{
    /// <summary>
    /// Marks a property to be ignored by T4 generation tools.
    /// </summary>
    [H1("Ignore (T4)")]
    [Category("Code Generation (T4)", "Ignore property in T4 templates in general.")]
    public class IgnorePropertyT4 : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IgnorePropertyT4"/> attribute.
        /// </summary>
        public IgnorePropertyT4()
        {
        }
    }
}
