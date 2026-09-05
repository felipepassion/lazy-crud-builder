namespace Lazy.Crud.Builder.Application.DTO.Attributes
{
    /// <summary>
    /// Renders the decorated navigation property as a listing picker component (a searchable list the user picks an item from), with an optional title.
    /// </summary>
    /// <category>UI/Selectors</category>
    public class ListingPicker : Attribute
    {
        /// <summary>
        /// Gets or sets the title shown above the picker.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListingPicker"/> attribute without a title.
        /// </summary>
        public ListingPicker()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListingPicker"/> attribute with a title.
        /// </summary>
        /// <param name="title">The title shown above the picker.</param>
        public ListingPicker(string title)
        {
            Title = title;
        }
    }
}
