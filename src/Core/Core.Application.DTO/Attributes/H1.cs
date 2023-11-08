namespace LazyCrud.Core.Application.DTO.Attributes
{
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
