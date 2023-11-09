namespace LazyCrud.Core.Domain.Attributes.T4
{
    public class Unique : Attribute
    {
        public Unique()
        {
            
        }

        public Unique(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
