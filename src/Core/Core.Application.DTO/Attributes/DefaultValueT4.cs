using Lazy.Crud.Core.Application.DTO.Attributes;

namespace Lazy.Crud.Core.Application.DTO.Attributes
{
    /// <summary>
    /// Defines a default value consumed by T4 templates for this property.
    /// </summary>
    [H1("Default Value (T4)")]
    [Category("Code Generation (T4)", "Define default value to be used by T4 templates on properties.")]
    public class DefaultValueT4 : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultValueT4"/> class.
        /// </summary>
        /// <param name="value">The default value to apply.</param>
        public DefaultValueT4(string value)
        {
            Value = value;
        }

        /// <summary>
        /// The default value to apply.
        /// </summary>
        public string Value { get; set; }
    }
}
