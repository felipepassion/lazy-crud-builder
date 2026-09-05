namespace Lazy.Crud.Builder.Application.DTO.Attributes
{
    /// <summary>
    /// Secondary heading (H2) shown in the generated pages and components for the decorated member.
    /// </summary>
    /// <category>UI/Titles</category>
    public class H2 : Attribute
    {
        /// <summary>
        /// Gets or sets the heading text.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the optional text shown under the heading.
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="H2"/> attribute with a heading.
        /// </summary>
        /// <param name="title">The heading text.</param>
        public H2(string title)
        {
            this.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="H2"/> attribute with a heading and a subtitle.
        /// </summary>
        /// <param name="title">The heading text.</param>
        /// <param name="subTitle">The text shown under the heading.</param>
        public H2(string title, string subTitle)
            : this(title)
        {
            this.SubTitle = subTitle;
        }
    }
}
