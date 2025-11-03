using System;
using Lazy.Crud.Core.Application.DTO.Attributes;

namespace Lazy.Crud.Core.Domain.Attributes.T4
{
    /// <summary>
    /// Indicates that a class must implement a specific interface or contract expected by T4 templates.
    /// </summary>
    /// <remarks>
    /// This attribute acts as a marker for tools and generators to verify that the
    /// decorated type implements required members. It does not itself enforce implementation.
    /// </remarks>
    [H1("Must Implement (T4)")]
    [Category("Code Generation (T4)", "Indicates the class must implement a contract expected by templates.")]
    public class MustImplementT4 : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MustImplementT4"/> attribute.
        /// </summary>
        public MustImplementT4()
        {
        }
    }
}
