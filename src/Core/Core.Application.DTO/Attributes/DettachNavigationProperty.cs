using Lazy.Crud.Core.Application.DTO.Attributes;
using System;

namespace Lazy.Crud.Core.Domain.Attributes.T4
{
    /// <summary>
    /// Attribute used to mark a navigation property that should be detached or excluded
    /// from T4-based code generation, mapping or serialization processes.
    /// </summary>
    /// <remarks>
    /// Apply this attribute to navigation or reference properties when you want to
    /// prevent T4 templates, mappers or serializers from traversing or including
    /// the marked property (for example, to avoid circular references or exclude
    /// large related entities from generated DTOs).
    ///
    /// This attribute is a simple marker and does not currently expose any
    /// configurable properties. If additional configuration is required later,
    /// add properties to this attribute class.
    /// </remarks>
    /// <category>Code Generation (T4)</category>
    public class DettachNavigationProperty : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DettachNavigationProperty"/> attribute.
        /// </summary>
        public DettachNavigationProperty()
        {
        }
    }
}
