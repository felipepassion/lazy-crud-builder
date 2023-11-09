using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LazyCrudBuilder.Core.Infra.IoC
{
    public interface IBaseIoC
    {
        void Configure(IConfiguration configuration, IServiceCollection services);
        public static IBaseIoC Current { get; set; }
    }
}
