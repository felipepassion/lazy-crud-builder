using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niu.Nutri.Core.Domain.Extensions
{
    public static class HttpContextExtensions
    {
        public async static Task<string> ReadBodyAsString(this HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            string result;

            // Assegura que o stream esteja no começo
            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);

            using (var reader = new StreamReader(httpContext.Request.Body))
            {
                result = await reader.ReadToEndAsync();

                // Assegura que o stream esteja no começo para que ele possa ser lido novamente por outros componentes.
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            return result;
        }
    }

}
