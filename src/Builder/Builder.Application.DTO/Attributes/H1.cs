namespace Lazy.Crud.Builder.Application.DTO.Attributes
{
    /// <summary>
    /// Primary heading (H1) shown at the top of the generated pages and components for the decorated member.
    /// </summary>
    /// <category>UI/Titles</category>
    public class H1 : Attribute
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
        /// Initializes a new instance of the <see cref="H1"/> attribute with a heading.
        /// </summary>
        /// <param name="title">The heading text.</param>
        public H1(string title)
        {
            this.Title = title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="H1"/> attribute with a heading and a subtitle.
        /// </summary>
        /// <param name="title">The heading text.</param>
        /// <param name="subTitle">The text shown under the heading.</param>
        public H1(string title, string subTitle)
            : this(title)
        {
            this.SubTitle = subTitle;
        }
    }
}
