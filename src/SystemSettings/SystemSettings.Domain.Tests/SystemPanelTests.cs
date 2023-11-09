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
    public class SystemPanelTests
    {
        [Fact]
        public async Task New_SystemPanel_SubItem()
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

            using var menuRepository = ServiceProvider.GetRequiredService<ISystemPanelRepository>();
            var menuAppService = ServiceProvider.GetRequiredService<ISystemPanelAppService>();
            var _logRequestContext = ServiceProvider.GetRequiredService<ILogRequestContext>();

            var id = Guid.NewGuid().ToString();

            var userDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<SystemPanelDTO>("{\"$id\":\"1\",\"Icon\":null,\"Description\":\"MENU ADM - SYSTEM\",\"Code\":\"ADMIN\",\"SubItems\":[{\"$id\":\"2\",\"Icon\":\"/imgs/menus/1-adm-system/sidebar-closed/1-menu-estruturacao/1-menu-estruturacao-master.svg\",\"Description\":\"Group of Menus\",\"GroupOfMenus\":[{\"$ref\":\"1\"}],\"SubItems\":[],\"AccessesOfMyProfile\":[],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":1,\"ExternalId\":\"57963abb-4dcf-46ee-a308-22b6e9894f03\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-10-25T17:19:56\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"Group of Menus\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"Group of Menus\",\"SubTitle\":null,\"DisplayNameTitle\":\"Menu\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Sidebar\",\"H1AndTitle\":\"Group of Menus\",\"H2AndSubTitle\":\"Sidebar\",\"CustomTitleOrH2\":\"Group of Menus\",\"CustomTitleOrH1\":\"Group of Menus\",\"SubTitlePropertyName\":null},{\"$id\":\"3\",\"Icon\":null,\"Description\":\"kjkj dsfsdfsd fsdf 44444444444444\",\"GroupOfMenus\":[],\"SubItems\":[],\"AccessesOfMyProfile\":[],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":5,\"ExternalId\":\"9cf747bd-c6de-48d2-a564-4a376750724f\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-10-29T01:31:18\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"kjkj dsfsdfsd fsdf 44444444444444\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"kjkj dsfsdfsd fsdf 44444444444444\",\"SubTitle\":null,\"DisplayNameTitle\":\"Menu\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Sidebar\",\"H1AndTitle\":\"kjkj dsfsdfsd fsdf 44444444444444\",\"H2AndSubTitle\":\"Sidebar\",\"CustomTitleOrH2\":\"kjkj dsfsdfsd fsdf 44444444444444\",\"CustomTitleOrH1\":\"kjkj dsfsdfsd fsdf 44444444444444\",\"SubTitlePropertyName\":null},{\"$id\":\"4\",\"Icon\":null,\"Description\":\"test 123\",\"GroupOfMenus\":[],\"SubItems\":[],\"AccessesOfMyProfile\":[],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":4,\"ExternalId\":\"9896fef5-2f04-4fb9-9866-0ac80c84f528\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-10-29T01:13:04\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"test 123\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"test 123\",\"SubTitle\":null,\"DisplayNameTitle\":\"Menu\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Sidebar\",\"H1AndTitle\":\"test 123\",\"H2AndSubTitle\":\"Sidebar\",\"CustomTitleOrH2\":\"test 123\",\"CustomTitleOrH1\":\"test 123\",\"SubTitlePropertyName\":null},{\"$id\":\"5\",\"Icon\":null,\"Description\":\"Novo Menu 123\",\"GroupOfMenus\":[],\"SubItems\":[],\"AccessesOfMyProfile\":[],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":3,\"ExternalId\":\"c06cc4ce-2796-40d1-89e9-d877734c7176\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-10-29T01:11:00\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"Novo Menu 123\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"Novo Menu 123\",\"SubTitle\":null,\"DisplayNameTitle\":\"Menu\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Sidebar\",\"H1AndTitle\":\"Novo Menu 123\",\"H2AndSubTitle\":\"Sidebar\",\"CustomTitleOrH2\":\"Novo Menu 123\",\"CustomTitleOrH1\":\"Novo Menu 123\",\"SubTitlePropertyName\":null},{\"$id\":\"6\",\"Icon\":null,\"Description\":\"New Menu 2222222 sjfhsk hdkjf hdkjfhkj\",\"GroupOfMenus\":[],\"SubItems\":[],\"AccessesOfMyProfile\":[],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":2,\"ExternalId\":\"55a303d3-3a0e-45b6-8bc8-bb920009d2f4\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-10-27T17:45:52\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"New Menu 2222222 sjfhsk hdkjf hdkjfhkj\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"New Menu 2222222 sjfhsk hdkjf hdkjfhkj\",\"SubTitle\":null,\"DisplayNameTitle\":\"Menu\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Sidebar\",\"H1AndTitle\":\"New Menu 2222222 sjfhsk hdkjf hdkjfhkj\",\"H2AndSubTitle\":\"Sidebar\",\"CustomTitleOrH2\":\"New Menu 2222222 sjfhsk hdkjf hdkjfhkj\",\"CustomTitleOrH1\":\"New Menu 2222222 sjfhsk hdkjf hdkjfhkj\",\"SubTitlePropertyName\":null}],\"AccessesOfMyProfile\":[{\"$id\":\"7\",\"Description\":\"Administrators - SYSTEM\",\"CanInsert\":true,\"CanUpdate\":true,\"CanList\":true,\"CanDelete\":true,\"SystemPanelSubItemId\":0,\"SystemPanelId\":0,\"SystemPanelGroupId\":1,\"IsSubItem\":false,\"ParentId\":null,\"CurrentStep\":0,\"RegisterDone\":false,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":1,\"ExternalId\":\"d28e794a-7758-433a-aad8-641bc93faddf\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-10-25T17:19:57\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"Administrators - SYSTEM\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"Administrators - SYSTEM\",\"SubTitle\":null,\"DisplayNameTitle\":\"Description\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":null,\"H1AndTitle\":\"Administrators - SYSTEM\",\"H2AndSubTitle\":\"\",\"CustomTitleOrH2\":\"Administrators - SYSTEM\",\"CustomTitleOrH1\":\"Administrators - SYSTEM\",\"SubTitlePropertyName\":null}],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":1,\"ExternalId\":\"3454d46f-59ec-4f18-b822-081e3087bded\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-10-25T17:19:56\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"MENU ADM - SYSTEM - ADMIN\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"MENU ADM - SYSTEM\",\"SubTitle\":\"ADMIN\",\"DisplayNameTitle\":\"Description\",\"DisplayNameSubTitle\":\"Code\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Grupo de Menus / Painéis\",\"H1AndTitle\":\"MENU ADM - SYSTEM\",\"H2AndSubTitle\":\"Grupo de Menus / Painéis - ADMIN\",\"CustomTitleOrH2\":\"MENU ADM - SYSTEM\",\"CustomTitleOrH1\":\"MENU ADM - SYSTEM\",\"SubTitlePropertyName\":\"Code\"}");

            userDTO.Command = new CreateSystemPanelCommand(_logRequestContext, userDTO);

            await menuRepository.ExecuteCommandAsync("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");
            var createResult1 = await menuAppService.Create(userDTO);

            var assert = await menuAppService.Get(new SystemPanelQueryModel { IdEqual = userDTO.Id });

            assert.Equals(assert.SubItems.Count, userDTO.SubItems.Count);

            Assert.Empty(createResult1.Errors);
        }

        [Fact]
        public async Task Alteração_Menu_Já_Existente_Adição_De_Um_Filho_SubMenu()
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

            using var menuRepository = ServiceProvider.GetRequiredService<ISystemPanelRepository>();
            var menuAppService = ServiceProvider.GetRequiredService<ISystemPanelAppService>();
            var _logRequestContext = ServiceProvider.GetRequiredService<ILogRequestContext>();

            var id = Guid.NewGuid().ToString();

            var userDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<SystemPanelDTO>("{\"$id\":\"1\",\"Icon\":null,\"Description\":\"MENU ADM - SYSTEM\",\"Code\":\"ADMIN\",\"SubItems\":[{\"$id\":\"2\",\"Icon\":\"/imgs/menus/1-adm-system/sidebar-closed/1-menu-estruturacao/1-menu-estruturacao-master.svg\",\"Description\":\"Group of Menus\",\"GroupOfMenus\":[{\"$ref\":\"1\"}],\"SubItems\":[],\"AccessesOfMyProfile\":[],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":1,\"ExternalId\":\"57963abb-4dcf-46ee-a308-22b6e9894f03\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-10-25T17:19:56\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"Group of Menus\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"Group of Menus\",\"SubTitle\":null,\"DisplayNameTitle\":\"Menu\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Sidebar\",\"H1AndTitle\":\"Group of Menus\",\"H2AndSubTitle\":\"Sidebar\",\"CustomTitleOrH2\":\"Group of Menus\",\"CustomTitleOrH1\":\"Group of Menus\",\"SubTitlePropertyName\":null},{\"$id\":\"3\",\"Icon\":null,\"Description\":\"kjkj dsfsdfsd fsdf 44444444444444\",\"GroupOfMenus\":[],\"SubItems\":[],\"AccessesOfMyProfile\":[],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":5,\"ExternalId\":\"9cf747bd-c6de-48d2-a564-4a376750724f\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-10-29T01:31:18\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"kjkj dsfsdfsd fsdf 44444444444444\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"kjkj dsfsdfsd fsdf 44444444444444\",\"SubTitle\":null,\"DisplayNameTitle\":\"Menu\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Sidebar\",\"H1AndTitle\":\"kjkj dsfsdfsd fsdf 44444444444444\",\"H2AndSubTitle\":\"Sidebar\",\"CustomTitleOrH2\":\"kjkj dsfsdfsd fsdf 44444444444444\",\"CustomTitleOrH1\":\"kjkj dsfsdfsd fsdf 44444444444444\",\"SubTitlePropertyName\":null},{\"$id\":\"4\",\"Icon\":null,\"Description\":\"test 123\",\"GroupOfMenus\":[],\"SubItems\":[],\"AccessesOfMyProfile\":[],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":4,\"ExternalId\":\"9896fef5-2f04-4fb9-9866-0ac80c84f528\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-10-29T01:13:04\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"test 123\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"test 123\",\"SubTitle\":null,\"DisplayNameTitle\":\"Menu\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Sidebar\",\"H1AndTitle\":\"test 123\",\"H2AndSubTitle\":\"Sidebar\",\"CustomTitleOrH2\":\"test 123\",\"CustomTitleOrH1\":\"test 123\",\"SubTitlePropertyName\":null},{\"$id\":\"5\",\"Icon\":null,\"Description\":\"Novo Menu 123\",\"GroupOfMenus\":[],\"SubItems\":[],\"AccessesOfMyProfile\":[],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":3,\"ExternalId\":\"c06cc4ce-2796-40d1-89e9-d877734c7176\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-10-29T01:11:00\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"Novo Menu 123\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"Novo Menu 123\",\"SubTitle\":null,\"DisplayNameTitle\":\"Menu\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Sidebar\",\"H1AndTitle\":\"Novo Menu 123\",\"H2AndSubTitle\":\"Sidebar\",\"CustomTitleOrH2\":\"Novo Menu 123\",\"CustomTitleOrH1\":\"Novo Menu 123\",\"SubTitlePropertyName\":null},{\"$id\":\"6\",\"Icon\":null,\"Description\":\"New Menu 2222222 sjfhsk hdkjf hdkjfhkj\",\"GroupOfMenus\":[],\"SubItems\":[],\"AccessesOfMyProfile\":[],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":2,\"ExternalId\":\"55a303d3-3a0e-45b6-8bc8-bb920009d2f4\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-10-27T17:45:52\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"New Menu 2222222 sjfhsk hdkjf hdkjfhkj\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"New Menu 2222222 sjfhsk hdkjf hdkjfhkj\",\"SubTitle\":null,\"DisplayNameTitle\":\"Menu\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Sidebar\",\"H1AndTitle\":\"New Menu 2222222 sjfhsk hdkjf hdkjfhkj\",\"H2AndSubTitle\":\"Sidebar\",\"CustomTitleOrH2\":\"New Menu 2222222 sjfhsk hdkjf hdkjfhkj\",\"CustomTitleOrH1\":\"New Menu 2222222 sjfhsk hdkjf hdkjfhkj\",\"SubTitlePropertyName\":null}],\"AccessesOfMyProfile\":[{\"$id\":\"7\",\"Description\":\"Administrators - SYSTEM\",\"CanInsert\":true,\"CanUpdate\":true,\"CanList\":true,\"CanDelete\":true,\"SystemPanelSubItemId\":0,\"SystemPanelId\":0,\"SystemPanelGroupId\":1,\"IsSubItem\":false,\"ParentId\":null,\"CurrentStep\":0,\"RegisterDone\":false,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":1,\"ExternalId\":\"d28e794a-7758-433a-aad8-641bc93faddf\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-10-25T17:19:57\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"Administrators - SYSTEM\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"Administrators - SYSTEM\",\"SubTitle\":null,\"DisplayNameTitle\":\"Description\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":null,\"H1AndTitle\":\"Administrators - SYSTEM\",\"H2AndSubTitle\":\"\",\"CustomTitleOrH2\":\"Administrators - SYSTEM\",\"CustomTitleOrH1\":\"Administrators - SYSTEM\",\"SubTitlePropertyName\":null}],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":1,\"ExternalId\":\"3454d46f-59ec-4f18-b822-081e3087bded\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-10-25T17:19:56\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"MENU ADM - SYSTEM - ADMIN\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"MENU ADM - SYSTEM\",\"SubTitle\":\"ADMIN\",\"DisplayNameTitle\":\"Description\",\"DisplayNameSubTitle\":\"Code\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Grupo de Menus / Painéis\",\"H1AndTitle\":\"MENU ADM - SYSTEM\",\"H2AndSubTitle\":\"Grupo de Menus / Painéis - ADMIN\",\"CustomTitleOrH2\":\"MENU ADM - SYSTEM\",\"CustomTitleOrH1\":\"MENU ADM - SYSTEM\",\"SubTitlePropertyName\":\"Code\"}");

            userDTO.Command = new CreateSystemPanelCommand(_logRequestContext, userDTO);

            await menuRepository.ExecuteCommandAsync("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");
            var createResult1 = await menuAppService.Create(userDTO);

            var assert = await menuAppService.Get(new SystemPanelQueryModel { IdEqual = userDTO.Id });

            assert.Equals(assert.SubItems.Count, userDTO.SubItems.Count);

            Assert.Empty(createResult1.Errors);
        }

        [Fact]
        public async Task Ateração_Painel_Menu_Permanecer_O_Mesmo()
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

            using var menuRepository = ServiceProvider.GetRequiredService<ISystemPanelGroupRepository>();
            var menuAppService = ServiceProvider.GetRequiredService<ISystemPanelGroupAppService>();
            var _logRequestContext = ServiceProvider.GetRequiredService<ILogRequestContext>();

            var id = Guid.NewGuid().ToString();

            var userDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<SystemPanelGroupDTO>("{\"$id\":\"1\",\"Icon\":{\"$id\":\"2\",\"Type\":null,\"Name\":null,\"Name2\":\"Arquivo\",\"Image\":null,\"Src\":null,\"IsLoading\":false},\"Description\":\"asdasd\",\"Code\":\"asdasdas\",\"SubItems\":[{\"$id\":\"3\",\"Icon\":{\"$id\":\"4\",\"Type\":null,\"Name\":null,\"Name2\":\"1-menu-estruturacao-master.svg\",\"Image\":null,\"Src\":\"/imgs/menus/1-adm-system/sidebar-closed/1-menu-estruturacao/1-menu-estruturacao-master.svg\",\"IsLoading\":false},\"Description\":\"ADM - Menus e Usuários\",\"GroupOfMenus\":[{\"$ref\":\"1\"}],\"SubItems\":[],\"AccessesOfMyProfile\":[],\"CurrentStep\":0,\"RegisterDone\":false,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":1,\"ExternalId\":\"ac6688e4-1163-45d3-b041-592aa994360e\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-08-21T22:37:24\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"ADM - Menus e Usuários\",\"Title\":\"ADM - Menus e Usuários\",\"SubTitle\":null,\"DisplayNameTitle\":\"Menu\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Sidebar\",\"H1AndTitle\":\"ADM - Menus e Usuários\",\"H2AndSubTitle\":\"Sidebar\",\"CustomTitleOrH2\":\"ADM - Menus e Usuários\",\"CustomTitleOrH1\":\"ADM - Menus e Usuários\",\"SubTitlePropertyName\":null}],\"AccessesOfMyProfile\":[],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":6,\"ExternalId\":\"7bf31862-b3d4-44a6-9eb0-56dbc60fd5aa\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-08-21T23:46:29\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"asdasd - asdasdas\",\"Title\":\"asdasd\",\"SubTitle\":\"asdasdas\",\"DisplayNameTitle\":\"Description\",\"DisplayNameSubTitle\":\"Code\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Grupo de Menus / Painéis\",\"H1AndTitle\":\"asdasd\",\"H2AndSubTitle\":\"Grupo de Menus / Painéis - asdasdas\",\"CustomTitleOrH2\":\"asdasd\",\"CustomTitleOrH1\":\"asdasd\",\"SubTitlePropertyName\":\"Code\"}");

            userDTO.Command = new CreateSystemPanelGroupCommand(_logRequestContext, userDTO);

            await menuRepository.ExecuteCommandAsync("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");
            var createResult1 = await menuAppService.Create(userDTO);

            Assert.Empty(createResult1.Errors);
        }

        [Fact]
        public async Task Ateração_Painel_Menu_Remover_Elemento_Existente()
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

            using var menuRepository = ServiceProvider.GetRequiredService<ISystemPanelGroupRepository>();
            var menuAppService = ServiceProvider.GetRequiredService<ISystemPanelGroupAppService>();
            var _logRequestContext = ServiceProvider.GetRequiredService<ILogRequestContext>();

            var id = Guid.NewGuid().ToString();

            var userDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<SystemPanelGroupDTO>("{\"$id\":\"1\",\"Icon\":{\"$id\":\"2\",\"Type\":null,\"Name\":null,\"Name2\":\"Arquivo\",\"Image\":null,\"Src\":null,\"IsLoading\":false},\"Description\":\"asdasd\",\"Code\":\"asdasdas\",\"SubItems\":[{\"$id\":\"3\",\"Icon\":{\"$id\":\"4\",\"Type\":null,\"Name\":null,\"Name2\":\"1-menu-estruturacao-master.svg\",\"Image\":null,\"Src\":\"/imgs/menus/1-adm-system/sidebar-closed/1-menu-estruturacao/1-menu-estruturacao-master.svg\",\"IsLoading\":false},\"Description\":\"ADM - Menus e Usuários\",\"GroupOfMenus\":[{\"$ref\":\"1\"}],\"SubItems\":[],\"AccessesOfMyProfile\":[],\"CurrentStep\":0,\"RegisterDone\":false,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":1,\"ExternalId\":\"ac6688e4-1163-45d3-b041-592aa994360e\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-08-21T22:37:24\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"ADM - Menus e Usuários\",\"Title\":\"ADM - Menus e Usuários\",\"SubTitle\":null,\"DisplayNameTitle\":\"Menu\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Sidebar\",\"H1AndTitle\":\"ADM - Menus e Usuários\",\"H2AndSubTitle\":\"Sidebar\",\"CustomTitleOrH2\":\"ADM - Menus e Usuários\",\"CustomTitleOrH1\":\"ADM - Menus e Usuários\",\"SubTitlePropertyName\":null},{\"$id\":\"5\",\"Icon\":{\"$id\":\"6\",\"Type\":null,\"Name\":null,\"Name2\":\"Arquivo\",\"Image\":null,\"Src\":null,\"IsLoading\":false},\"Description\":\"sub-relatórios-15\",\"GroupOfMenus\":[],\"SubItems\":[],\"AccessesOfMyProfile\":[],\"CurrentStep\":1,\"RegisterDone\":false,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":59,\"ExternalId\":\"b20f42d9-f223-4d73-a206-34dbcae375f8\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-06-06T23:45:10\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"sub-relatórios-15\",\"Title\":\"sub-relatórios-15\",\"SubTitle\":null,\"DisplayNameTitle\":\"Menu\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Sidebar\",\"H1AndTitle\":\"sub-relatórios-15\",\"H2AndSubTitle\":\"Sidebar\",\"CustomTitleOrH2\":\"sub-relatórios-15\",\"CustomTitleOrH1\":\"sub-relatórios-15\",\"SubTitlePropertyName\":null}],\"AccessesOfMyProfile\":[],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":6,\"ExternalId\":\"7bf31862-b3d4-44a6-9eb0-56dbc60fd5aa\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-08-21T23:46:29\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"asdasd - asdasdas\",\"Title\":\"asdasd\",\"SubTitle\":\"asdasdas\",\"DisplayNameTitle\":\"Description\",\"DisplayNameSubTitle\":\"Code\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Grupo de Menus / Painéis\",\"H1AndTitle\":\"asdasd\",\"H2AndSubTitle\":\"Grupo de Menus / Painéis - asdasdas\",\"CustomTitleOrH2\":\"asdasd\",\"CustomTitleOrH1\":\"asdasd\",\"SubTitlePropertyName\":\"Code\"}");

            userDTO.Command = new CreateSystemPanelGroupCommand(_logRequestContext, userDTO);

            await menuRepository.ExecuteCommandAsync("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");
            var createResult1 = await menuAppService.Create(userDTO);
            Assert.Empty(createResult1.Errors);


            var userDTO2 = Newtonsoft.Json.JsonConvert.DeserializeObject<SystemPanelGroupDTO>("{\"$id\":\"1\",\"Icon\":{\"$id\":\"2\",\"Type\":null,\"Name\":null,\"Name2\":\"Arquivo\",\"Image\":null,\"Src\":null,\"IsLoading\":false},\"Description\":\"asdasd\",\"Code\":\"asdasdas\",\"SubItems\":[{\"$id\":\"3\",\"Icon\":{\"$id\":\"4\",\"Type\":null,\"Name\":null,\"Name2\":\"1-menu-estruturacao-master.svg\",\"Image\":null,\"Src\":\"/imgs/menus/1-adm-system/sidebar-closed/1-menu-estruturacao/1-menu-estruturacao-master.svg\",\"IsLoading\":false},\"Description\":\"ADM - Menus e Usuários\",\"GroupOfMenus\":[{\"$ref\":\"1\"}],\"SubItems\":[],\"AccessesOfMyProfile\":[],\"CurrentStep\":0,\"RegisterDone\":false,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":1,\"ExternalId\":\"ac6688e4-1163-45d3-b041-592aa994360e\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-08-21T22:37:24\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"ADM - Menus e Usuários\",\"Title\":\"ADM - Menus e Usuários\",\"SubTitle\":null,\"DisplayNameTitle\":\"Menu\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Sidebar\",\"H1AndTitle\":\"ADM - Menus e Usuários\",\"H2AndSubTitle\":\"Sidebar\",\"CustomTitleOrH2\":\"ADM - Menus e Usuários\",\"CustomTitleOrH1\":\"ADM - Menus e Usuários\",\"SubTitlePropertyName\":null}],\"AccessesOfMyProfile\":[],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":6,\"ExternalId\":\"7bf31862-b3d4-44a6-9eb0-56dbc60fd5aa\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-08-21T23:46:29\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"asdasd - asdasdas\",\"Title\":\"asdasd\",\"SubTitle\":\"asdasdas\",\"DisplayNameTitle\":\"Description\",\"DisplayNameSubTitle\":\"Code\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":\"Grupo de Menus / Painéis\",\"H1AndTitle\":\"asdasd\",\"H2AndSubTitle\":\"Grupo de Menus / Painéis - asdasdas\",\"CustomTitleOrH2\":\"asdasd\",\"CustomTitleOrH1\":\"asdasd\",\"SubTitlePropertyName\":\"Code\"}");
            userDTO2.Command = new CreateSystemPanelGroupCommand(_logRequestContext, userDTO2);
            var createResult2 = await menuAppService.Create(userDTO2);

            Assert.Empty(createResult2.Errors);
        }
    }
}