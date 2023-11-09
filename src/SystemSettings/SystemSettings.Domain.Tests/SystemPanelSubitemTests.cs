using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LazyCrud.CrossCutting.Infra.Log.Contexts;
using LazyCrud.SystemSettings.Application.Aggregates.SystemSettingsAgg.AppServices;
using LazyCrud.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests;
using LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.CommandModels;
using LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Queries.Models;
using LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Repositories;

namespace LazyCrud.SystemSettings.Domain.Tests
{
    public class SystemPanelSubitemTests
    {
        [Fact]
        public async Task Create_New_SubItem()
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            var _serviceCollection = new ServiceCollection();
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            _serviceCollection.AddSingleton(Configuration);

            _serviceCollection.AddLogging(logging => logging.AddConsole());

            SystemSettings.Infra.IoC.IoCFactory.Current.Configure(Configuration, _serviceCollection);
            Users.Infra.IoC.IoCFactory.Current.Configure(Configuration, _serviceCollection);
            Users.Identity.Infra.IoC.IoCFactory.Current.Configure(Configuration, _serviceCollection);

            LazyCrud.Core.Infra.IoC.IoCFactory.Current.Configure(Configuration, _serviceCollection);

            using var ServiceProvider = _serviceCollection.BuildServiceProvider();

            using var menuRepository = ServiceProvider.GetRequiredService<ISystemPanelSubItemRepository>();
            var menuAppService = ServiceProvider.GetRequiredService<ISystemPanelSubItemAppService>();
            var _logRequestContext = ServiceProvider.GetRequiredService<ILogRequestContext>();

            var id = Guid.NewGuid().ToString();

            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<SystemPanelSubItemDTO>("{\"subItems\":[],\"systemPanelSubItemId\":null,\"systemPanelId\":2,\"isSubItem\":false,\"icon\":null,\"description\":\"sub-New Menu-6\",\"url\":null,\"linkDireto\":false,\"actionButton\":false,\"currentStep\":0,\"registerDone\":false,\"maxSteps\":1,\"active\":true,\"isCreated\":false,\"command\":null,\"id\":null,\"externalId\":\"e09a012c-03b5-4158-a48d-a15202f4d865\",\"fieldsToValidate\":[],\"createdAt\":\"2023-10-30T14:37:23.757Z\",\"updatedAt\":null,\"tititleWithSubtitle\":\"sub-New Menu-6\",\"isSubmiting\":false,\"isManuallySubmiting\":false,\"title\":\"sub-New Menu-6\",\"subTitle\":null,\"displayNameTitle\":\"Description\",\"displayNameSubTitle\":\"Url\",\"titlePropertyName\":\"Description\",\"h1\":null,\"h2\":\"Submenu\",\"h1AndTitle\":\"sub-New Menu-6\",\"h2AndSubTitle\":\"Submenu\",\"customTitleOrH2\":\"sub-New Menu-6\",\"customTitleOrH1\":\"sub-New Menu-6\",\"subTitlePropertyName\":\"Url\"}");

            obj.Command = new CreateSystemPanelSubItemCommand(_logRequestContext, obj);

            await menuRepository.ExecuteCommandAsync("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");
            var createResult1 = await menuAppService.Create(obj);

            var assert = await menuAppService.Get(new SystemPanelSubItemQueryModel { IdEqual = obj.Id });

            assert.Equals(assert.SubItems.Count, obj.SubItems.Count);

            Assert.Empty(createResult1.Errors);
        }

    }
}