using System;

namespace Lazy.Crud.Core.Domain.Attributes.T4
{
    /// <summary>
    /// Marks a property that should be ignored by front-end generation or mapping.
    /// </summary>
    /// <remarks>
    /// When applied to a property, T4 templates and front-end mappers should not
    /// include this property in generated DTOs or UI models.
    /// </remarks>
    /// <category>Code Generation (T4)</category>
    public class IgnorePropertyOnFrontT4 : Attribute
    {
    }
}
