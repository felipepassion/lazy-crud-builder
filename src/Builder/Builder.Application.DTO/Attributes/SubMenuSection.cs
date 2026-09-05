using System;

namespace Lazy.Crud.Builder.Application.DTO.Attributes;

/// <summary>
/// Places the feature generated for the decorated entity under a sub-section of a main menu section.
/// </summary>
/// <remarks>
/// Apply to an aggregate root entity together with, or instead of, <see cref="MenuSection"/>.
/// The parent name must match the main section the sub-section belongs to.
/// </remarks>
/// <category>UI/Menu</category>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class SubMenuSection : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubMenuSection"/> attribute.
    /// </summary>
    /// <param name="parentName">The display name of the main menu section this sub-section belongs to.</param>
    /// <param name="name">The display name of the sub-section.</param>
    public SubMenuSection(string parentName, string name)
    {
        ParentName = parentName;
        Name = name;
    }

    /// <summary>
    /// Gets the display name of the parent main menu section.
    /// </summary>
    public string ParentName { get; }

    /// <summary>
    /// Gets the display name of the sub-section.
    /// </summary>
    public string Name { get; }
}
