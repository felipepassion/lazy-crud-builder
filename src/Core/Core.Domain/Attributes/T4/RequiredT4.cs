namespace LazyCrudBuilder.Core.Domain.Attributes.T4
{
    public class RequiredT4 : Attribute
    {
        public string ErrorMessage { get; set; }

        public RequiredT4(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public RequiredT4()
        {

        }
    }
}
