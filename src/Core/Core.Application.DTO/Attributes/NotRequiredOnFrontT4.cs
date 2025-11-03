using System;
using Lazy.Crud.Core.Application.DTO.Attributes;

namespace Lazy.Crud.Core.Domain.Attributes.T4
{
    /// <summary>
    /// Marks a property as not required on front-end models even if it is required in the domain.
    /// </summary>
    [H1("Not Required on Front-end")]
    [Category("Code Generation (T4)", "Marks the property as optional in the front-end.")]
    public class NotRequiredOnFrontT4 : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotRequiredOnFrontT4"/> attribute.
        /// </summary>
        public NotRequiredOnFrontT4()
        {
        }
    }
}
