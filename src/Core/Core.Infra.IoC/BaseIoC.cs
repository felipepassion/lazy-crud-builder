using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Niu.Nutri.Core.Infra.IoC
{
    public interface IBaseIoC
    {
        void Configure(IConfiguration configuration, IServiceCollection services);
        public static IBaseIoC Current { get; set; }
    }
}
