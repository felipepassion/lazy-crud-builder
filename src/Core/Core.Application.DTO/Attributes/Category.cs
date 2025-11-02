using System;

namespace Niu.Nutri.Core.Application.DTO.Attributes
{
    /// <summary>
    /// Organizational category for project classes/attributes.
    /// Use it to group by area (UI, T4, Validation, etc.) and briefly describe the purpose.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public sealed class Category : Attribute
    {
        /// <summary>
        /// Category name (e.g., "Code Generation (T4)", "UI/List").
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Short description of the decorated type purpose.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// Creates a new category attribute.
        /// </summary>
        /// <param name="name">Category name.</param>
        /// <param name="description">Purpose description.</param>
        public Category(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }
    }
}
