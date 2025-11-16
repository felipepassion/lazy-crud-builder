using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lazy.Crud.Builder.Application.DTO.Attributes;

namespace Lazy.Crud.Builder.Domain.Attributes.Auth
{
    /// <summary>
    /// Validation attribute that accepts either a valid email address or a phone number.
    /// </summary>
    /// <category>Validation</category>
    public class EmailOrPhone : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var input = value as string;
            if (string.IsNullOrEmpty(input))
            {
                return new ValidationResult("The field is required.");
            }

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            var phoneRegex = new Regex(@"^\(?\d{2}\)?[\s-]?9?\d{4}-?\d{4}$");

            if (emailRegex.IsMatch(input) || phoneRegex.IsMatch(input))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("The field must be a valid email address or phone number.");
        }
    }
}
