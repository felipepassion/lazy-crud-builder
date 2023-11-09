using CrossCutting.Application.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LazyCrudBuilder.CrossCutting.Infra.Log.Contexts;
using LazyCrudBuilder.Users.Application.Aggregates.UsersAgg.AppServices;
using LazyCrudBuilder.Users.Application.DTO.Aggregates.UsersAgg.Requests;
using LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.CommandModels;
using LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Queries.Models;
using LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Repositories;

namespace LazyCrudBuilder.Users.Domain.Tests
{
    public class UserProfileListTests
    {
        [Fact]
        public async Task Get_UserProfileLists()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            var _serviceCollection = new ServiceCollection();
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var mailSender = new EmailSender();
            _serviceCollection.AddSingleton(Configuration);

            _serviceCollection.AddLogging(logging => logging.AddConsole());

            Users.Infra.IoC.IoCFactory.Current.Configure(Configuration, _serviceCollection);
            Identity.Infra.IoC.IoCFactory.Current.Configure(Configuration, _serviceCollection);

            Core.Infra.IoC.IoCFactory.Current.Configure(Configuration, _serviceCollection);

            using var ServiceProvider = _serviceCollection.BuildServiceProvider();

            using var _profileRepo = ServiceProvider.GetRequiredService<IUserProfileListAppService>();
            using var _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
            var _userAppService = ServiceProvider.GetRequiredService<IUserAppService>();
            var _logRequestContext = ServiceProvider.GetRequiredService<ILogRequestContext>();


            var test = await _profileRepo.GetAll(new UserProfileListQueryModel { });

            Assert.NotEmpty(test);
        }
    }
}
