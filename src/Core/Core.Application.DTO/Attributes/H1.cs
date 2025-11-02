namespace Niu.Nutri.Core.Application.DTO.Attributes
{
    /// <summary>
    /// Primary heading (H1) shown in pages/components.
    /// </summary>
    [Category("UI/Titles", "Primary heading (H1) for pages/components.")]
    public class H1 : Attribute
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }

        public H1(string title)
        {
            this.Title = title;
        }

        public H1(string title, string subTitle)
            : this(title)
        {
            this.SubTitle = subTitle;
        }
    }
}
