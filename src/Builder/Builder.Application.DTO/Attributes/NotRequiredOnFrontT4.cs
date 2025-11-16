using System;

namespace Lazy.Crud.Builder.Domain.Attributes.T4
{
    /// <summary>
    /// Marks a property as not required on front-end models even if it is required in the domain.
    /// </summary>
    /// <category>Code Generation (T4)</category>
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
