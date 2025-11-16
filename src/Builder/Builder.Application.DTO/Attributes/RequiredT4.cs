using System;

namespace Lazy.Crud.Builder.Domain.Attributes.T4
{
    /// <summary>
    /// Indicates that a property is required for validation and optionally provides an error message.
    /// </summary>
    /// <category>Validation</category>
    public class RequiredT4 : Attribute
    {
        /// <summary>
        /// Gets or sets the error message to be used when validation fails.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredT4"/> attribute with a custom error message.
        /// </summary>
        /// <param name="errorMessage">The error message to use when validation fails.</param>
        public RequiredT4(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiredT4"/> attribute with default settings.
        /// </summary>
        public RequiredT4()
        {

        }
    }
}
