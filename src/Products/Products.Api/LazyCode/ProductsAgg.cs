
namespace Lazy.Crud.Products.Api;
using Infra.Data.Context;
using Microsoft.AspNetCore.DataProtection;

public static partial class IoCFactory {
	public static void InjectDependencies(this IServiceCollection services, IConfiguration configuration) {
		Lazy.Crud.Products.Infra.IoC.IoCFactory.Current.Configure(configuration, services);
		Lazy.Crud.Builder.Infra.IoC.IoCFactory.Current.Configure(configuration, services);
		services.ConfigureAuthentication();
	}
	private static void ConfigureAuthentication(this IServiceCollection services){
		services.AddDataProtection().PersistKeysToDbContext<ProductsAggContext>().SetApplicationName("SharedCookieApp");
		services.AddAuthentication("Identity.Application").AddCookie("Identity.Application", options => { options.Cookie.Name = ".AspNet.SharedCookie"; });
	}
	public static void OnAppInitialized(this WebApplication app){
		using (var scope = app.Services.CreateScope()){
			var logProvider = scope.ServiceProvider.GetRequiredService<Lazy.Crud.CrossCutting.Infra.Log.Providers.ILogProvider>();
			logProvider.Write(new Lazy.Crud.CrossCutting.Infra.Log.Entries.LogEntry("------> APP | Lazy.Crud.Products.Api | STARTED <------", action: "OnAppInitialized"));
		}
	}
}
