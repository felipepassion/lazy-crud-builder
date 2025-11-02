namespace Niu.Nutri.Core.Application.DTO.Attributes
{
    /// <summary>
    /// Indicates that a property should be ignored in CSV mapping.
    /// </summary>
    [Category("Serialization/CSV", "Ignore the property in CSV mapping.")]
    public class IgnoreCsvMap : Attribute
    {
    }
}
