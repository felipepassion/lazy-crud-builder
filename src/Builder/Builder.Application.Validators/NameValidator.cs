using System.Text.RegularExpressions;

namespace Lazy.Crud.Builder.Application.Validators
{
    public static class NameValidator
    {
        public static bool ValidName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return true;
            }

            name = name.Trim();

            if (name.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length <= 1)
            {
                return false;
            }

            var regex = new Regex(@"^[\p{L}]+(?:\s+[\p{L}]+)+$");
            return regex.IsMatch(name);
        }
    }
}
