using System;
using Lazy.Crud.Core.Application.DTO.Attributes;

namespace Lazy.Crud.Core.Domain.Attributes.T4
{
    /// <summary>
    /// Marks a property that should be ignored when generating listing views.
    /// </summary>
    [H1("Ignore on Listing")]
    [Category("Code Generation (T4)", "Ignore property when generating list views (grids/tables).")]
    public class IgnorePropertyOnListingT4 : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IgnorePropertyOnListingT4"/> attribute.
        /// </summary>
        public IgnorePropertyOnListingT4()
        {
        }
    }
}
