using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Niu.Nutri.Core.Api.DTO;
using Niu.Nutri.WebApp.Controllers;

namespace Niu.Nutri.Core.Api.Factory
{
    public class HttpFactoryBuilder
    {
        public static HttpClient Build(HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            var schema = httpContext.Request;

            HttpClientHandler handler = new HttpClientHandler { };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var client = new HttpClient(handler);
            client.BaseAddress = new Uri($"{schema.Scheme}://{schema.Host.Value}");

            return client;
        }

        public static HttpClient Build(string controllerName, IConfiguration configuration, HttpContext context)
        {
            var route = FindRoute(controllerName, configuration);

            return Build(route, context);
        }

        public static HttpClient Build<T>(T controller, IConfiguration configuration, HttpContext httpContext)
            where T : BaseController
        {
            var route = FindRoute(controller.GetType().FullName.Split(".")[2], configuration);

            return Build(route, httpContext);
        }

        public static ApiRoutesDTO FindRoute(string controllerName, IConfiguration configuration)
        {
            var apis = configuration.GetSection("Apis").Get<IEnumerable<ApiRoutesDTO>>();
            if (apis == null) throw new ArgumentNullException(nameof(apis));

            return apis!.First(x => x.Name.Equals(controllerName.Replace("Controller", ""), StringComparison.InvariantCultureIgnoreCase));
        }

        public static HttpClient Build(ApiRoutesDTO? route, HttpContext httpContext)
        {
            if (route == null)
                throw new ArgumentNullException(nameof(route));

            var currentUri = new Uri($"{route.BaseUrl}/{route.Route}");
            HttpClientHandler handler = new HttpClientHandler { };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            var client = new HttpClient(handler) { BaseAddress = currentUri };

            if(httpContext?.Request.Headers.TryGetValue("Cookie", out var val) == true)
                client.DefaultRequestHeaders.Add("Cookie", [val]);

            return client;
        }
    }
}
