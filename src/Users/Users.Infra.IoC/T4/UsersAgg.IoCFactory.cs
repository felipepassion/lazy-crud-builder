
using LazyCrudBuilder.Users.Infra.Data.Aggregates.UsersAgg.Repositories;
using LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Repositories;
using LazyCrudBuilder.Users.Infra.Data.Aggregates.SystemSettingsAgg.Repositories;
using LazyCrudBuilder.Users.Domain.Aggregates.SystemSettingsAgg.Repositories;
using LazyCrudBuilder.Users.Infra.Data.Aggregates.UsersAgg.Repositories;
using LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Repositories;
using LazyCrudBuilder.Users.Application.Aggregates.UsersAgg.AppServices;
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

namespace LazyCrudBuilder.Users.Infra.IoC {

	using Infra.Data.Context;
	using Domain.Aggregates.UsersAgg.CommandHandlers;
	

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
            services.AddMediatR((x) => { x.RegisterServicesFromAssembly(typeof(BaseUsersAggCommandHandler<>).GetTypeInfo().Assembly); });
						
		}

		void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
			PreConfigureDatabase(services, configuration);
			if(string.IsNullOrWhiteSpace(connectionString)) 
				connectionString = configuration.GetConnectionString("DefaultConnection")!;
			services.AddDbContext<UsersAggContext>(options =>
			options.UseSqlServer(connectionString));
		}

		void ConfigureRepositories(IServiceCollection services) {

            services.AddScoped<IUserProfileAccessRepository, UserProfileAccessRepository>();
            services.AddScoped<IUserCurrentAccessSelectedRepository, UserCurrentAccessSelectedRepository>();
            services.AddScoped<IUserProfileListRepository, UserProfileListRepository>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IUsersAggSettingsRepository, UsersAggSettingsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISystemPanelSubItemRepository, SystemPanelSubItemRepository>();
            services.AddScoped<ISystemPanelRepository, SystemPanelRepository>();
			
			ConfigureAdditionalRepositories();
		}

		void ConfigureAppServices(IServiceCollection services) {

            services.AddScoped<IUserProfileAccessAppService, UserProfileAccessAppService>();
            services.AddScoped<IUserCurrentAccessSelectedAppService, UserCurrentAccessSelectedAppService>();
            services.AddScoped<IUserProfileListAppService, UserProfileListAppService>();
            services.AddScoped<IUserProfileAppService, UserProfileAppService>();
            services.AddScoped<IUsersAggSettingsAppService, UsersAggSettingsAppService>();
            services.AddScoped<IUserAppService, UserAppService>();
		
			ConfigureAdditionalAppServices(services);
		}

        #endregion
    }
}
