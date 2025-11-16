namespace Lazy.Crud.Builder.Application.DTO.Attributes
{
    /// <summary>
    /// Controls how a property is rendered in list views (order and optional template).
    /// </summary>
    /// <category>UI/List</category>
    public class DisplayOnList : Attribute
    {
        /// <summary>
        /// Creates a new attribute with default settings.
        /// </summary>
        public DisplayOnList()
        {

        }
        /// <summary>
        /// Creates a new attribute with display order and an optional template.
        /// </summary>
        /// <param name="order">Sorting order in the list.</param>
        /// <param name="template">Optional display template.</param>
        public DisplayOnList(int order, DisplayOnListTemplates template = 0)
        {
            Order = order;
            Template = template;
        }

        /// <summary>
        /// Sorting order inside the list/grid view.
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// Optional predefined template for rendering.
        /// </summary>
        public DisplayOnListTemplates Template { get; }
    }

    public enum DisplayOnListTemplates
    {
        Status
    }
}
