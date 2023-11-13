

using LazyCrud.Core.Infra.Data.Contexts;
using LazyCrud.Core.Infra.IoC;
using LazyCrud.Users.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LazyCrud.Marketplace.WebApi {
    public static partial class IoCFactory {
       
		public static void InjectDependencies(this IServiceCollection services, IConfiguration configuration) {

            LazyCrud.Users.Identity.Infra.IoC.IoCFactory.Current.Configure(configuration, services);
            LazyCrud.SystemSettings.Infra.IoC.IoCFactory.Current.Configure(configuration, services);
            LazyCrud.Users.Infra.IoC.IoCFactory.Current.Configure(configuration, services);
            LazyCrud.MarketPlace.Infra.IoC.IoCFactory.Current.Configure(configuration, services);
			
            LazyCrud.Core.Infra.IoC.IoCFactory.Current.Configure(configuration, services);
		}

        public static void Migrate(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var contexts = (from asm in AppDomain.CurrentDomain.GetAssemblies()
                                from type in asm.GetTypes()
                                where type.IsClass && type.BaseType == typeof(BaseContext) || type.BaseType == typeof(IdentityDbContext<ApplicationUser, IdentityRole<int>, int>)
                                select type).ToArray();

                foreach (var item in contexts)
                {
                    (scope.ServiceProvider.GetRequiredService(item) as DbContext)
                        .Database.Migrate();
                }
            }
        }
    }
}