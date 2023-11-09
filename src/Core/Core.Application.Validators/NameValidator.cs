using System.Text.RegularExpressions;

namespace LazyCrudBuilder.Core.Application.Validators
{
    public static class NameValidator
    {
        public static bool ValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return true;
            }

            name = name.Trim();

            char firstChar = name[0];
            for (int i = 1; i < name.Length; i++)
            {
                if (name[i] != firstChar)
                {
                    break;
                }

                if (i == name.Length - 1)
                {
                    return false; 
                }
            }

            var regex = new Regex(@"^[\p{L} \.'\-]+$");
            if (!regex.IsMatch(name))
            {
                return false;
            }

            return true;
        }
    }
}
