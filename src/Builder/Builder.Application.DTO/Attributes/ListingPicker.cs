namespace Lazy.Crud.Builder.Application.DTO.Attributes
{
    /// <summary>
    /// Renders a listing selector component with an optional title.
    /// </summary>
    /// <category>UI/Selectors</category>
    public class ListingPicker : Attribute
    {
        public string Title { get; set; }
        public ListingPicker()
        {
            
        }
        public ListingPicker(string title)
        {
            Title = title;
        }
    }
}
