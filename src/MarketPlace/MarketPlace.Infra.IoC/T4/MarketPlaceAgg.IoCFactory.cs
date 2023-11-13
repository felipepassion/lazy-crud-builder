
using LazyCrud.MarketPlace.Infra.Data.Aggregates.MarketPlaceAgg.Repositories;
using LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Repositories;
using LazyCrud.MarketPlace.Infra.Data.Aggregates.MarketPlaceAgg.Repositories;
using LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Repositories;
using LazyCrud.MarketPlace.Application.Aggregates.MarketPlaceAgg.AppServices;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using LazyCrud.CrossCutting.Infra.Log.Contexts;
using LazyCrud.CrossCutting.Infra.Log.Providers;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Commands;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using LazyCrud.Core.Application.DTO.Seedwork;
using LazyCrud.Core.Infra.IoC;

namespace LazyCrud.MarketPlace.Infra.IoC {

	using Infra.Data.Context;
	using Domain.Aggregates.MarketPlaceAgg.CommandHandlers;
	

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
            services.AddMediatR((x) => { x.RegisterServicesFromAssembly(typeof(BaseMarketPlaceAggCommandHandler<>).GetTypeInfo().Assembly); });
						
		}

		void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
			PreConfigureDatabase(services, configuration);
			if(string.IsNullOrWhiteSpace(connectionString)) 
				connectionString = configuration.GetConnectionString("DefaultConnection")!;
			services.AddDbContext<MarketPlaceAggContext>(options =>
			options.UseSqlServer(connectionString));
		}

		void ConfigureRepositories(IServiceCollection services) {

            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IMarketPlaceAggSettingsRepository, MarketPlaceAggSettingsRepository>();
            services.AddScoped<ICarrinhoRepository, CarrinhoRepository>();
            services.AddScoped<ICategoriaprodutoRepository, CategoriaprodutoRepository>();
			
			ConfigureAdditionalRepositories();
		}

		void ConfigureAppServices(IServiceCollection services) {

            services.AddScoped<IProdutoAppService, ProdutoAppService>();
            services.AddScoped<IMarketPlaceAggSettingsAppService, MarketPlaceAggSettingsAppService>();
            services.AddScoped<ICarrinhoAppService, CarrinhoAppService>();
            services.AddScoped<ICategoriaprodutoAppService, CategoriaprodutoAppService>();
		
			ConfigureAdditionalAppServices(services);
		}

        #endregion
    }
}
