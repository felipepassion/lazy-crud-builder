using CrossCutting.Application.Mail;
using Lazy.Crud.Builder.Application.DTO.Seedwork;
using Lazy.Crud.CrossCutting.Infra.Log.Contexts;
using Lazy.Crud.CrossCutting.Infra.Log.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lazy.Crud.Builder.Infra.IoC
{
    public partial class IoCFactory : IBaseIoC
    {
        partial void ConfigureFactories();
        partial void ConfigureAdditionalAppServices(IServiceCollection services);
        partial void ConfigureAdditionalRepositories();

        #region Constructor
        private static IoCFactory _current;
        public static IoCFactory Current { get { return _current ?? (_current = new IoCFactory()); } }
        #endregion

        #region Methods

        public void Configure(IConfiguration configuration, IServiceCollection services)
        {
            ConfigureFactories();
            ConfigureLog(services);
            ConfigureAppServices(services);
            ConfigureRepositories(services);
            ConfigureDatabase(services, configuration);
            ConfigureMappings();
            services.AddHttpContextAccessor();          
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

        void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        void ConfigureRepositories(IServiceCollection services)
        {
            ConfigureAdditionalRepositories();
        }

        void ConfigureAppServices(IServiceCollection services)
        {
            services.AddSingleton<IEmailSender, EmailSender>();
            ConfigureAdditionalAppServices(services);
        }

        #endregion
    }
}
