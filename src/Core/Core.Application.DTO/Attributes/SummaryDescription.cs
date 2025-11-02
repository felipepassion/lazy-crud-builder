namespace Niu.Nutri.Core.Application.DTO.Attributes
{
    public class SummaryDescription : Attribute
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }

        public SummaryDescription(string title)
        {
            Title = title;
        }

        public SummaryDescription(string title, string subTitle)
            : this(title)
        {
            SubTitle = subTitle;
        }
    }
}
