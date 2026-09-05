using System;

namespace Lazy.Crud.Builder.Application.DTO.Attributes;

/// <summary>
/// Places the feature generated for the decorated entity under a main menu section.
/// </summary>
/// <remarks>
/// Apply to an aggregate root entity. The generated client navigation groups the entity's pages
/// under the named section; use <see cref="SubMenuSection"/> to nest it one level deeper.
/// </remarks>
/// <category>UI/Menu</category>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class MenuSection : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MenuSection"/> attribute.
    /// </summary>
    /// <param name="name">The display name of the main menu section.</param>
    public MenuSection(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the display name of the main menu section.
    /// </summary>
    public string Name { get; }
}
