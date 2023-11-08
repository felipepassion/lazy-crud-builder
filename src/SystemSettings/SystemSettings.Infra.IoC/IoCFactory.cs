using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LazyCrud.SystemSettings.Infra.IoC
{
    public partial class IoCFactory
    {
        partial void PreConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            //this.connectionString = configuration.GetConnectionString("CargasTabelas");
        }
    }
}
