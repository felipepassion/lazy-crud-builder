
using Lazy.Crud.DefaultTemplate.Application.Aggregates.DefaultTemplateAgg.AppServices;
using Lazy.Crud.DefaultTemplate.Infra.Data.Aggregates.DefaultTemplateAgg.Repositories;
using Lazy.Crud.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Lazy.Crud.Core.Domain.Aggregates.CommonAgg.Commands;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Lazy.Crud.Core.Application.DTO.Seedwork;
using Lazy.Crud.Core.Infra.IoC;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection;

namespace Lazy.Crud.DefaultTemplate.Infra.IoC {
	using Core.Infra.IoC.Extensions;
    using Core.Api.Middlewares;
	using Infra.Data.Context;
	using Domain.Aggregates.DefaultTemplateAgg.CommandHandlers;
	

    public partial class IoCFactory : IBaseIoC {
		string connectionString;
		
		partial void ConfigureFactories();
		partial void ConfigureValidators();
		partial void ConfigureAdditionalAppServices(IServiceCollection services);
		partial void ConfigureAdditionalRepositories();
		partial void PreConfigureDatabase(IServiceCollection services, IConfiguration configuration);

        #region Constructor

        private static IoCFactory _current;
		public static IoCFactory Current { get { return _current ?? (_current = new IoCFactory()); } }

        #endregion

        #region Methods

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

		void ConfigureControllers(IServiceCollection services)
		{
			services.AddControllers().AddNewtonsoftJson(options =>
			{
				//options.SerializerSettings.Converters.Add(new StringEnumConverter());
				// Use the default property (Pascal) casing
				options.SerializerSettings.ContractResolver = new DefaultContractResolver();
				var settings = options.SerializerSettings;
				settings.DateFormatString = "yyyy-MM-ddTHH:mm:ss";
				settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				settings.MissingMemberHandling = MissingMemberHandling.Ignore;
				settings.NullValueHandling = NullValueHandling.Ignore;
				settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
				settings.DefaultValueHandling = DefaultValueHandling.Ignore;
				settings.Formatting = Formatting.Indented;
				settings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
				options.AllowInputFormatterExceptionMessages = false;
				//settings.Converters.Add(new TiimeOnlyJsonConverter());
			}).AddBadRequestCustomValidator();;
		}

		void ConfigureMiddleware(IServiceCollection services)
        {
            services.AddScoped<ErrorHandlingMiddleware>();
        }

		void ConfigureMappings()
		{
			MapperFactory.Setup(this.GetType().Namespace.Replace("Infra.IoC","Domain"));
		}

		void ConfigureMediatR(IServiceCollection services)
		{
            services.AddMediatR((x) => { x.RegisterServicesFromAssembly(typeof(BaseCommand).GetTypeInfo().Assembly); });
            services.AddMediatR((x) => { x.RegisterServicesFromAssembly(typeof(BaseDefaultTemplateAggCommandHandler<>).GetTypeInfo().Assembly); });
						
		}

		void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
			PreConfigureDatabase(services, configuration);
			if(string.IsNullOrWhiteSpace(connectionString)) 
				connectionString = configuration.GetConnectionString("DefaultConnection")!;
			services.AddDbContext<DefaultTemplateAggContext>(options =>
			options.UseNpgsql(connectionString));
		}

		void ConfigureRepositories(IServiceCollection services) {

            services.AddScoped<IDefaultEntityRepository, DefaultEntityRepository>();
            services.AddScoped<IDefaultTemplateAggSettingsRepository, DefaultTemplateAggSettingsRepository>();
			
			ConfigureAdditionalRepositories();
		}

		void ConfigureAppServices(IServiceCollection services) {

            services.AddScoped<IDefaultEntityAppService, DefaultEntityAppService>();
            services.AddScoped<IDefaultTemplateAggSettingsAppService, DefaultTemplateAggSettingsAppService>();
		
			ConfigureAdditionalAppServices(services);
		}

        #endregion
    }
}
