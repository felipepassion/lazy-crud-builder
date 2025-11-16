using System;

namespace Lazy.Crud.Core.Domain.Attributes.T4
{
    /// <summary>
    /// Marks a property or set of properties as unique and optionally provides a message.
    /// </summary>
    /// <category>Validation</category>
    public class Unique : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Unique"/> attribute.
        /// </summary>
        public Unique()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Unique"/> attribute with a custom message.
        /// </summary>
        /// <param name="message">The message associated with the uniqueness constraint.</param>
        public Unique(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets the message associated with the uniqueness constraint.
        /// </summary>
        public string Message { get; }
    }
}
