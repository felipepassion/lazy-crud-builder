
using LazyCrudBuilder.SystemSettings.Infra.Data.Aggregates.SystemSettingsAgg.Repositories;
using LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Repositories;
using LazyCrudBuilder.SystemSettings.Infra.Data.Aggregates.SystemSettingsAgg.Repositories;
using LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Repositories;
using LazyCrudBuilder.SystemSettings.Application.Aggregates.SystemSettingsAgg.AppServices;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using LazyCrudBuilder.CrossCutting.Infra.Log.Contexts;
using LazyCrudBuilder.CrossCutting.Infra.Log.Providers;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Commands;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using LazyCrudBuilder.Core.Application.DTO.Seedwork;
using LazyCrudBuilder.Core.Infra.IoC;

namespace LazyCrudBuilder.SystemSettings.Infra.IoC {

	using Infra.Data.Context;
	using Domain.Aggregates.SystemSettingsAgg.CommandHandlers;
	

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
        }

		void ConfigureMappings()
		{
			MapperFactory.Setup(this.GetType().Namespace.Replace("Infra.IoC","Domain"));
		}

		void ConfigureMediatR(IServiceCollection services)
		{
            services.AddMediatR((x) => { x.RegisterServicesFromAssembly(typeof(BaseCommand).GetTypeInfo().Assembly); });
            services.AddMediatR((x) => { x.RegisterServicesFromAssembly(typeof(BaseSystemSettingsAggCommandHandler<>).GetTypeInfo().Assembly); });
						
		}

		void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
			PreConfigureDatabase(services, configuration);
			if(string.IsNullOrWhiteSpace(connectionString)) 
				connectionString = configuration.GetConnectionString("DefaultConnection")!;
			services.AddDbContext<SystemSettingsAggContext>(options =>
			options.UseSqlServer(connectionString));
		}

		void ConfigureRepositories(IServiceCollection services) {

            services.AddScoped<ISystemPanelSubItemRepository, SystemPanelSubItemRepository>();
            services.AddScoped<ISystemPanelRepository, SystemPanelRepository>();
            services.AddScoped<ISystemPanelGroupRepository, SystemPanelGroupRepository>();
            services.AddScoped<ICargaTabelaRepository, CargaTabelaRepository>();
            services.AddScoped<ISystemSettingsAggSettingsRepository, SystemSettingsAggSettingsRepository>();
			
			ConfigureAdditionalRepositories();
		}

		void ConfigureAppServices(IServiceCollection services) {

            services.AddScoped<ISystemPanelSubItemAppService, SystemPanelSubItemAppService>();
            services.AddScoped<ISystemPanelAppService, SystemPanelAppService>();
            services.AddScoped<ISystemPanelGroupAppService, SystemPanelGroupAppService>();
            services.AddScoped<ICargaTabelaAppService, CargaTabelaAppService>();
            services.AddScoped<ISystemSettingsAggSettingsAppService, SystemSettingsAggSettingsAppService>();
		
			ConfigureAdditionalAppServices(services);
		}

        #endregion
    }
}
