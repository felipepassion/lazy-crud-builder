using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace LazyCrud.CrossCuting.Infra.Utils.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return null;

            return str[0].ToString().ToLower() + str.Substring(1, str.Length - 1);
        }

        public static bool TryDeserializeJSON<T>(this string request, out T result)
            where T : class
        {
            if (!string.IsNullOrWhiteSpace(request))
            {
                try
                {
                    result = JsonConvert.DeserializeObject<T>(request);
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

        public static Expression<Func<T, object>>[] GetPropertyListSelector<T>(this string propertyName)
        {
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
    }
}
