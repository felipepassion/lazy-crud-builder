using Lazy.Crud.Products.Application.Aggregates.ProductsAgg.AppServices;
using Lazy.Crud.Products.Infra.Data.Aggregates.ProductsAgg.Repositories;
using Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Repositories;
namespace Lazy.Crud.Products.Infra.IoC;
using Builder.Infra.IoC.Extensions;
using Builder.Api.Middlewares;
using Infra.Data.Context;
using Domain.Aggregates.ProductsAgg.CommandHandlers;

public partial class IoCFactory : IBaseIoC {
	string connectionString;
	partial void ConfigureFactories();
	partial void ConfigureValidators();
	partial void ConfigureAdditionalAppServices(IServiceCollection services);
	partial void ConfigureAdditionalRepositories();
	partial void PreConfigureDatabase(IServiceCollection services, IConfiguration configuration);

    private static IoCFactory _current;
	public static IoCFactory Current { get { return _current ?? (_current = new IoCFactory()); } }

    public void Configure(IConfiguration configuration, IServiceCollection services) {
		ConfigureFactories();
		ConfigureMediatR(services);
		ConfigureAppServices(services);
		ConfigureRepositories(services);
		ConfigureDatabase(services, configuration);
		ConfigureMappings();
		ConfigureMiddleware(services);
		ConfigureControllers(services);
    }

	void ConfigureControllers(IServiceCollection services){
		services.AddControllers().AddNewtonsoftJson(options => {
			options.SerializerSettings.ContractResolver = new DefaultContractResolver();
			var settings = options.SerializerSettings;
			settings.DateFormatString = "yyyy-MM-ddTHH:mm:ss";
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			settings.MissingMemberHandling = MissingMemberHandling.Ignore;
			settings.NullValueHandling = NullValueHandling.Ignore;
			settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
			settings.DefaultValueHandling = DefaultValueHandling.Ignore;
			settings.Formatting = Formatting.Indented;
			settings.PreserveReferencesHandling = Formatting.Indented == Formatting.Indented ? PreserveReferencesHandling.Objects : settings.PreserveReferencesHandling;
			options.AllowInputFormatterExceptionMessages = false;
		});
	}

	void ConfigureMiddleware(IServiceCollection services){ services.AddScoped<ErrorHandlingMiddleware>(); }
	void ConfigureMappings(){ MapperFactory.Setup(this.GetType().Namespace.Replace("Infra.IoC","Domain")); }
	void ConfigureMediatR(IServiceCollection services){
        services.AddMediatR((x) => { x.RegisterServicesFromAssembly(typeof(BaseCommand).GetTypeInfo().Assembly); });
        services.AddMediatR((x) => { x.RegisterServicesFromAssembly(typeof(BaseProductsAggCommandHandler<>).GetTypeInfo().Assembly); });
			}
	void ConfigureDatabase(IServiceCollection services, IConfiguration configuration){
		PreConfigureDatabase(services, configuration);
		if(string.IsNullOrWhiteSpace(connectionString)) connectionString = configuration.GetConnectionString("DefaultConnection")!;
		services.AddDbContext<ProductsAggContext>(options => options.UseNpgsql(connectionString));
	}
	void ConfigureRepositories(IServiceCollection services){
        services.AddScoped<IProductsRepository, ProductsRepository>();
		ConfigureAdditionalRepositories();
	}
	void ConfigureAppServices(IServiceCollection services){
        services.AddScoped<IProductsAppService, ProductsAppService>();
		ConfigureAdditionalAppServices(services);
	}
}
