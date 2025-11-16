using System;

namespace Lazy.Crud.Builder.Domain.Attributes.T4
{
    /// <summary>
    /// Marks a property that should be ignored on generated request models.
    /// </summary>
    /// <category>Code Generation (T4)</category>
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
