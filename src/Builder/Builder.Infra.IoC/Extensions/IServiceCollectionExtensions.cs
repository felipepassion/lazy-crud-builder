using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lazy.Crud.Builder.Application.DTO.Http.Models.CommonAgg.Commands.Responses;
using System.Reflection;

namespace Lazy.Crud.Builder.Infra.IoC.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void InjectT4Dependencies(this IServiceCollection services, AppDomain domain, IConfiguration configuration)
        {
            var ioCs = (from asm in domain.GetAssemblies()
                        from type in asm.GetTypes()
                        where type.IsClass && type.Name == "IoCFactory"
                        select type).ToArray();

            foreach (var item in ioCs)
            {
                (item.GetProperty("Current", BindingFlags.Public | BindingFlags.Static) as IBaseIoC)
                    .Configure(configuration, services);
            }
        }

        public static IMvcBuilder AddBadRequestCustomValidator(this IMvcBuilder services)
        {
            services.ConfigureApiBehaviorOptions(options =>
            options.InvalidModelStateResponseFactory = actionContext =>
            {
                var modelState = actionContext.ModelState.Values;
                var allErrors = actionContext.ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(new GetHttpResponseDTO
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Errors = allErrors.Select(e => e.ErrorMessage).ToArray()
                });
            });

            return services;
        }
    }
}
