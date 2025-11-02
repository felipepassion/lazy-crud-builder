namespace Niu.Nutri.Core.Application.DTO.Attributes
{
    /// <summary>
    /// Renders a listing selector component with an optional title.
    /// </summary>
    [Category("UI/Selectors", "Renders a listing selector with optional title.")]
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
