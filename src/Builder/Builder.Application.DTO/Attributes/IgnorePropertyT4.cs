using System;

namespace Lazy.Crud.Builder.Domain.Attributes.T4
{
    /// <summary>
    /// Marks a property to be ignored by T4 generation tools.
    /// </summary>
    /// <category>Code Generation (T4)</category>
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
