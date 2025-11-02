namespace Niu.Nutri.Core.Application.DTO.Attributes
{
    /// <summary>
    /// Secondary heading (H2) shown in pages/components.
    /// </summary>
    [Category("UI/Titles", "Secondary heading (H2) for pages/components.")]
    public class H2 : Attribute
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }

        public H2(string title)
        {
            this.Title = title;
        }

        public H2(string title, string subTitle)
            : this(title)
        {
            this.SubTitle = subTitle;
        }
    }
}
