namespace LazyCrudBuilder.Core.Domain.Attributes.T4
{
    public class Step : Attribute
    {
        public int Position { get; set; }
        public Step(int position)
        {
            Position = position;
        }
    }
}
