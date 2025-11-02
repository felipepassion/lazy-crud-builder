namespace Niu.Nutri.Core.Application.DTO.Attributes
{
    /// <summary>
    /// Instructs to skip automatic validations for the decorated property.
    /// </summary>
    [Category("Validation", "Ignore automatic validations for the property.")]
    public class IgnoreValidation : Attribute
    {
    }
}
