namespace LazyCrudBuilder.Core.Domain.Attributes.T4
{
    public class MigrationOrder : Attribute
    {
        public int Order { get; set; }

        public MigrationOrder(int order)
        {
            Order = order;
        }
    }
}
