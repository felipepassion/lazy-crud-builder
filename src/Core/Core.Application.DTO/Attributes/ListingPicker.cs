namespace LazyCrudBuilder.Core.Application.DTO.Attributes
{
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
