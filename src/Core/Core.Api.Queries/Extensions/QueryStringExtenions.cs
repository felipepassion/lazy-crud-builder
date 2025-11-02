using System.Reflection;
using System.Web;

namespace Niu.Nutri.Core.Api.Queries.Extensions
{
    public static class QueryStringExtenions
    {
        public static string ToQueryString(this object obj)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var queryParameters = new List<string>();
            foreach (var property in properties)
            {
                var value = property.GetValue(obj, null);
                if (value != null) // Ignorando propriedades com valores nulos
                {
                    // Convertendo valores booleanos para lower case para compatibilidade com padrões de query string
                    string stringValue = value is bool ? value.ToString().ToLower() : value.ToString();
                    queryParameters.Add($"{HttpUtility.UrlEncode(property.Name)}={HttpUtility.UrlEncode(stringValue)}");
                }
            }

            return string.Join("&", queryParameters);
        }
    }   
}
