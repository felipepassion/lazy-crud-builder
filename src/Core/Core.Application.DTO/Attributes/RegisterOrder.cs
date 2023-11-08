namespace LazyCrud.Core.Application.DTO.Attributes
{
    public class RegisterOrder : Attribute
    {
        public int Position { get; set; }
        public RegisterOrder(int position)
        {
            Position = position;
        }
    }
}
