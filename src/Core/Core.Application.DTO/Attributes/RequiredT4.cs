using System;
using Lazy.Crud.Core.Application.DTO.Attributes;

/// <summary>
/// Indicates that a property is required for validation and optionally provides an error message.
/// </summary>
namespace Lazy.Crud.Core.Domain.Attributes.T4
{
    [H1("Required (T4)")]
    [Category("Validation", "Marks a property as required for validation with optional custom error message.")]
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
