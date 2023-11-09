namespace LazyCrudBuilder.Core.Application.DTO.Attributes
{
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
