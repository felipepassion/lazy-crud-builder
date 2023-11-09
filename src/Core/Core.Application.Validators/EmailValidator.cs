using System.Text.RegularExpressions;

namespace LazyCrud.Core.Application.Validators
{
    public static class EmailValidator
    {
        public static bool ValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            // Expressão regular para validar endereços de email
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }
    }
}
