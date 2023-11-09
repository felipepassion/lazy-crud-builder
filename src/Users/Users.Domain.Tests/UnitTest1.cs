using LazyCrudBuilder.CrossCutting.Infra.Log.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Repositories;

namespace LazyCrudBuilder.Users.Domain.Tests
{
    public class UnitTest1
    {
        public UnitTest1()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        [Fact]
        public async Task Criacao_User_Envio_Email_Nova_Conta_Sucesso()
        {
            var _serviceCollection = new ServiceCollection();
            var Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            _serviceCollection.AddSingleton(Configuration);

            _serviceCollection.AddLogging(logging => logging.AddConsole());

            Users.Infra.IoC.IoCFactory.Current.Configure(Configuration, _serviceCollection);
            Identity.Infra.IoC.IoCFactory.Current.Configure(Configuration, _serviceCollection);

            LazyCrudBuilder.Core.Infra.IoC.IoCFactory.Current.Configure(Configuration, _serviceCollection);

            using var ServiceProvider = _serviceCollection.BuildServiceProvider();

            using var _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();

            var _logRequestContext = ServiceProvider.GetRequiredService<ILogRequestContext>();

            _userRepository.ExecuteCommandAsync("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"").Wait();
        }
    }
}