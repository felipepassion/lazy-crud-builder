namespace LazyCrud.Core.Application.DTO.Attributes
{
    public class DefaultValueT4 : Attribute
    {
        public DefaultValueT4(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
