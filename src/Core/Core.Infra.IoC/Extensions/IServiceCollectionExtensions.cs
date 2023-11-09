using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LazyCrud.Core.Infra.IoC.Extensions
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
    }
}
