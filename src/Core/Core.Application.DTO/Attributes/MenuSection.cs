namespace Niu.Nutri.Core.Application.DTO.Attributes;

/// <summary>
/// Defines the main menu section where the feature is placed.
/// </summary>
[Category("UI/Menu", "Defines the main menu section where the feature appears.")]
public class MenuSection
{
 public required string Name { get; set; }
}
