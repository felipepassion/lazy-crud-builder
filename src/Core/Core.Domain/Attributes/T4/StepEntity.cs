namespace LazyCrud.Core.Domain.Attributes.T4
{
    public class Steppable : Attribute
    {
        public int Quantity { get; set; }
        public Steppable(int quantity)
        {
            
            Quantity = quantity;
        }

        public Steppable()
        {

        }
    }
}
