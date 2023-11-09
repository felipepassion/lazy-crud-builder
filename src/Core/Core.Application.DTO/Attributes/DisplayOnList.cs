namespace LazyCrudBuilder.Core.Application.DTO.Attributes
{
    public class DisplayOnList : Attribute
    {
        public DisplayOnList()
        {

        }
        public DisplayOnList(int order, DisplayOnListTemplates template = 0)
        {
            Order = order;
            Template = template;
        }

        public int Order { get; set; }
        public DisplayOnListTemplates Template { get; }
    }

    public enum DisplayOnListTemplates
    {
        Status
    }
}
