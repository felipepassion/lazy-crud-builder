using System;
using Lazy.Crud.Core.Application.DTO.Attributes;

namespace Lazy.Crud.Core.Domain.Attributes.T4
{
    /// <summary>
    /// Marks a property that should be ignored by front-end generation or mapping.
    /// </summary>
    /// <remarks>
    /// When applied to a property, T4 templates and front-end mappers should not
    /// include this property in generated DTOs or UI models.
    /// </remarks>
    [H1("Ignore on Front-end")]
    [Category("Code Generation (T4)", "Ignore property during front-end generation/mapping.")]
    public class IgnorePropertyOnFrontT4 : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IgnorePropertyOnFrontT4"/> attribute.
        /// </summary>
        public IgnorePropertyOnFrontT4()
        {
        }
    }
}
