using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Niu.Nutri.CrossCuting.Infra.Utils.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return null;

            return str[0].ToString().ToLower() + str.Substring(1, str.Length - 1);
        }
        public static string ToPascalCase(this string input)
        {
            // Converte a string para Pascal Case
            // (primeira letra maiúscula, seguida de letras maiúsculas)
            // Exemplo: "PascalCase"
            return char.ToUpper(input[0]) + input.Substring(1);
        }
        public static bool TryDeserializeJSON<T>(this string request, out T result)
            where T : class
        {
            if (!string.IsNullOrWhiteSpace(request))
            {
                try
                {
                    result = JsonConvert.DeserializeObject<T>(request.Replace("\r\n", "").Replace(@"\n", ""))!;
                    return result != null;
                }
                catch { }
            }

            result = null;
            return false;
        }

        public static Expression<Func<T, object>> GetPropertySelector<T>(this string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) throw new NullReferenceException(nameof(propertyName));
            var arg = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(arg, propertyName);
            //return the property as object
            var conv = Expression.Convert(property, typeof(object));
            var exp = Expression.Lambda<Func<T, object>>(conv, new ParameterExpression[] { arg });
            return exp;
        }

        public static string GetPropertyNameBySelector<T>(this Expression<Func<T, object>> selector)
        {
            if (selector == null) throw new NullReferenceException(nameof(selector));
            var body = selector.Body;
            if (body is UnaryExpression)
            {
                var unary = body as UnaryExpression;
                if (unary?.Operand is MemberExpression)
                {
                    var member = unary.Operand as MemberExpression;
                    return member?.Member.Name;
                }
            }
            else if (body is MemberExpression)
            {
                var member = body as MemberExpression;
                return member?.Member.Name;
            }
            return null;
        }

        public static Expression<Func<T, object>>[]? GetPropertyListSelector<T>(this string? propertyName)
        {
            if (propertyName == null) return null;

            var result = new List<Expression<Func<T, object>>>();
            if (propertyName != null)
            {
                foreach (var item in propertyName?.Split(","))
                {
                    result.Add(GetPropertySelector<T>(propertyName));
                }
            }
            return result.ToArray();
        }

        public static string StringBetween(this string STR, string FirstString, string LastString)
        {
            string FinalString;
            int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
            int Pos2 = STR.IndexOf(LastString);
            FinalString = STR.Substring(Pos1, Pos2 - Pos1);
            return FinalString;
        }

        public static string FormatNumber(
            this string numberAsStr,
            int? min = null,
            int? max = null,
            bool allowSpecialNumberInputs = false)
        {
            if (!int.TryParse(numberAsStr, out var numberAsInt))
                return string.Empty;

            if (min.HasValue)
            {
                if (numberAsInt < min)
                {
                    return min.ToString();
                }
            }
            if (max.HasValue)
            {
                if (numberAsInt > max)
                    return max.ToString();
            }

            if (!allowSpecialNumberInputs && numberAsStr.ContainsSpecialNumberCharacters())
                return string.Empty;

            return numberAsStr;
        }

        public static bool ContainsSpecialNumberCharacters(this string str)
        {
            string[] specialChars = ["+", "e"];

            return str.Any(x => specialChars.Contains(x.ToString()));
        }

        public static string ExtractNumbers(this string str)
        {
            return string.Join("", str?.Where(x=>char.IsDigit(x)) ?? "");
        }

        public static string RemoveTelephoneMask(this string value)
        {
            value = Regex.Replace(value, @"[\-\(\)\s]", "");
            return value;
        }

        public static string ConvertImageToBase64(this string imagePath)
        {
            byte[] imageArray = File.ReadAllBytes(imagePath);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);
           return base64ImageRepresentation;
        }
    }
}
