using System;
using Lazy.Crud.Core.Application.DTO.Attributes;

namespace Lazy.Crud.Core.Domain.Attributes.T4
{
    /// <summary>
    /// Marks a property that should be ignored on generated request models.
    /// </summary>
    [H1("Ignore on Request")]
    [Category("Code Generation (T4)", "Ignore property in generated Request models.")]
    public class IgnorePropertyT4OnRequest : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IgnorePropertyT4OnRequest"/> attribute.
        /// </summary>
        public IgnorePropertyT4OnRequest()
        {
        }
    }
}
