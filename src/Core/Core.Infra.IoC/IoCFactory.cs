using CrossCutting.Application.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LazyCrud.Core.Application.DTO.Seedwork;
using LazyCrud.CrossCutting.Infra.Log.Contexts;
using LazyCrud.CrossCutting.Infra.Log.Providers;

namespace LazyCrud.Core.Infra.IoC
{
    public partial class IoCFactory : IBaseIoC
    {
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

        public void Configure(IConfiguration configuration, IServiceCollection services)
        {
            ConfigureFactories();
			ConfigureLog(services);
            ConfigureMediatR(services);
            ConfigureAppServices(services);
            ConfigureRepositories(services);
            ConfigureDatabase(services, configuration);
            ConfigureMappings();
        }

        void ConfigureMappings()
        {
            MapperFactory.Setup(this.GetType().Namespace!.Replace("Infra.IoC", "Domain"));
        }

        void ConfigureLog(IServiceCollection services)
        {
            services.AddScoped<ILogRequestContext, LogRequestContext>();
            services.AddSingleton<ILogProvider, LoggerProvider>();
        }

        void ConfigureMediatR(IServiceCollection services)
        {
            //services.AddMediatR(typeof(ExceptionEventHandler).GetTypeInfo().Assembly);
        }

        void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
        }

        void ConfigureRepositories(IServiceCollection services)
        {
            ConfigureAdditionalRepositories();
        }

        void ConfigureAppServices(IServiceCollection services)
        {
            services.AddScoped<IEmailSender,  EmailSender>();
            ConfigureAdditionalAppServices(services);
        }

        #endregion
    }
}
