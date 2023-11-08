using CrossCutting.Application.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LazyCrud.CrossCutting.Infra.Log.Contexts;
using LazyCrud.Users.Application.Aggregates.UsersAgg.AppServices;
using LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests;
using LazyCrud.Users.Domain.Aggregates.UsersAgg.CommandModels;
using LazyCrud.Users.Domain.Aggregates.UsersAgg.Queries.Models;
using LazyCrud.Users.Domain.Aggregates.UsersAgg.Repositories;

namespace LazyCrud.Users.Domain.Tests
{
    public class UserDomainTests
    {
        [Fact]
        public async Task Alteracao_De_Perfis_De_Acesso_User_Nova_Conta_Sucesso()
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

            using var _profileRepo = ServiceProvider.GetRequiredService<IUserProfileAppService>();
            using var _userRepository = ServiceProvider.GetRequiredService<IUserRepository>();
            var _userAppService = ServiceProvider.GetRequiredService<IUserAppService>();
            var _logRequestContext = ServiceProvider.GetRequiredService<ILogRequestContext>();

            var id = Guid.NewGuid().ToString();

            var cpf = CPFGenerator.Generate();
            var userDTO = new UserDTO
            {
                UserName = Guid.NewGuid().ToString(),
                Contact = new UserContactDTO
                {
                    Email = $"{Guid.NewGuid().ToString()}@email.com"
                },
                Name = string.Join("", Guid.NewGuid().ToString().Replace("-", " ").Where(x => !char.IsDigit(x))),
                ExternalId = Guid.NewGuid().ToString()
            };
            userDTO.Command = new CreateUserCommand(_logRequestContext, userDTO);

            await _userRepository.ExecuteCommandAsync("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\"");
            var createResult1 = await _userAppService.Create(userDTO);

            if (createResult1.Errors.Count > 0)
                throw new Exception(createResult1.Errors.LastOrDefault().Value);

            var updatedUser = await _userAppService.Get(new UserQueryModel { ExternalIdEqual = userDTO.ExternalId });
            updatedUser.Command = new CreateUserCommand(_logRequestContext, updatedUser);

            var profile = await _profileRepo.Get(new UserProfileQueryModel { });
            updatedUser.Accesses.Add(new UserProfileListDTO
            {
                UserId = userDTO.Id,
                UserProfiles = new List<UserProfileDTO>
                {
                    profile
                }
            });
            //var createResult2 = await _userAppService.Create(updatedUser);

            //if (createResult2.Errors.Count > 0)
            //    throw new Exception(createResult1.Errors.LastOrDefault().Value);

            ////updatedUser = await _userRepository.FindAsync(x => x.Id == userDTO.Id);
            //updatedUser = await _userAppService.Get(new UserQueryModel { ExternalIdEqual = userDTO.ExternalId });
            //Assert.NotEmpty(updatedUser.Accesses);

            var userDTO2 = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDTO>("{\"$id\":\"1\",\"UserName\":\"2390708\",\"Name\":\"ded bf e a cbb\",\"BirthDate\":null,\"Gender\":\"M\",\"NeedResetPassword\":null,\"NeedSendOnboardingMail\":true,\"ProfilePicture\":null,\"Contact\":{\"$id\":\"2\",\"Contacts\":[],\"ContactNumbers\":\"\",\"Email\":\"21621f93-d677-4a32-b9f3-3574577d6745@email.com\",\"IsCreated\":true,\"Command\":null,\"Id\":18,\"ExternalId\":\"f167a1eb-02c5-438d-a017-7f7538127e7d\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-11-06T03:36:15\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":null,\"SubTitle\":null,\"DisplayNameTitle\":\"Título\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":null,\"H1\":null,\"H2\":null,\"H1AndTitle\":\"\",\"H2AndSubTitle\":\"\",\"CustomTitleOrH2\":null,\"CustomTitleOrH1\":null,\"SubTitlePropertyName\":null},\"CanUpdatePassword\":null,\"SelectedAccess\":{\"$id\":\"3\",\"User\":null,\"UserProfile\":null,\"SelectedPage\":{\"$id\":\"4\",\"SystemPanelSubItemId\":null,\"SystemPanelId\":0,\"IsSubItem\":false,\"LinkDireto\":false,\"Description\":null,\"Url\":null,\"ActionButton\":false,\"IsCreated\":false,\"Command\":null,\"Id\":null,\"ExternalId\":null,\"FieldsToValidate\":[],\"CreatedAt\":\"2023-11-06T06:15:56.876Z\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":null,\"SubTitle\":null,\"DisplayNameTitle\":\"Título\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":null,\"H1\":null,\"H2\":null,\"H1AndTitle\":\"\",\"H2AndSubTitle\":\"\",\"CustomTitleOrH2\":null,\"CustomTitleOrH1\":null,\"SubTitlePropertyName\":null},\"AccessOfThisPage\":{\"$id\":\"5\",\"Description\":null,\"UserProfileId\":0,\"SystemPanelSubItemId\":null,\"SystemPanelId\":null,\"SystemPanelGroupId\":null,\"IsSubItem\":false,\"ParentId\":null,\"IsDirectLink\":false,\"CanInsert\":false,\"CanUpdate\":false,\"CanList\":false,\"CanDelete\":false,\"CurrentStep\":0,\"RegisterDone\":false,\"MaxSteps\":1,\"Active\":null,\"IsCreated\":false,\"Command\":null,\"Id\":null,\"ExternalId\":null,\"FieldsToValidate\":[],\"CreatedAt\":\"2023-11-06T06:15:56.876Z\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":null,\"SubTitle\":null,\"DisplayNameTitle\":\"Description\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":null,\"H1AndTitle\":\"\",\"H2AndSubTitle\":\"\",\"CustomTitleOrH2\":null,\"CustomTitleOrH1\":null,\"SubTitlePropertyName\":null},\"IsInitialized\":false,\"RefreshHeaderUserInfos\":null,\"MyCurrentRole\":null,\"UserProfileId\":null,\"IsCreated\":false,\"Command\":null,\"Id\":null,\"ExternalId\":null,\"FieldsToValidate\":[],\"CreatedAt\":\"2023-11-06T06:15:56.876Z\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":null,\"SubTitle\":null,\"DisplayNameTitle\":\"Título\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":null,\"H1\":null,\"H2\":null,\"H1AndTitle\":\"\",\"H2AndSubTitle\":\"\",\"CustomTitleOrH2\":null,\"CustomTitleOrH1\":null,\"SubTitlePropertyName\":null},\"Accesses\":[{\"$id\":\"6\",\"Expanded\":true,\"UserId\":1,\"UserProfiles\":[{\"$id\":\"7\",\"Description\":\"ADM - SYSTEM\",\"Code\":\"ADMIN\",\"InitialPage\":\"/Relatorios\",\"IsPrivateProfile\":false,\"Accesses\":[{\"$id\":\"8\",\"Description\":\"Administrators - SYSTEM\",\"UserProfileId\":1,\"SystemPanelSubItemId\":null,\"SystemPanelId\":null,\"SystemPanelGroupId\":1,\"IsSubItem\":false,\"ParentId\":null,\"IsDirectLink\":false,\"CanInsert\":true,\"CanUpdate\":true,\"CanList\":true,\"CanDelete\":true,\"CurrentStep\":0,\"RegisterDone\":false,\"MaxSteps\":1,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":1,\"ExternalId\":\"aef67896-b7e3-4d79-8c59-ccb99bad2569\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-11-05T16:38:14\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"Administrators - SYSTEM\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"Administrators - SYSTEM\",\"SubTitle\":null,\"DisplayNameTitle\":\"Description\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":null,\"H1AndTitle\":\"Administrators - SYSTEM\",\"H2AndSubTitle\":\"\",\"CustomTitleOrH2\":\"Administrators - SYSTEM\",\"CustomTitleOrH1\":\"Administrators - SYSTEM\",\"SubTitlePropertyName\":null}],\"CurrentStep\":0,\"RegisterDone\":false,\"MaxSteps\":2,\"Active\":true,\"IsCreated\":true,\"Command\":null,\"Id\":1,\"ExternalId\":\"d986bb97-736f-40fd-9d7a-974ea9c50020\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-11-05T16:38:14\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"ADM - SYSTEM - ADMIN\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"ADM - SYSTEM\",\"SubTitle\":\"ADMIN\",\"DisplayNameTitle\":\"Name do Perfil\",\"DisplayNameSubTitle\":\"Code\",\"TitlePropertyName\":\"Description\",\"H1\":null,\"H2\":null,\"H1AndTitle\":\"ADM - SYSTEM\",\"H2AndSubTitle\":\" - ADMIN\",\"CustomTitleOrH2\":\"ADM - SYSTEM\",\"CustomTitleOrH1\":\"ADM - SYSTEM\",\"SubTitlePropertyName\":\"Code\"}],\"CurrentStep\":0,\"RegisterDone\":false,\"MaxSteps\":1,\"Active\":null,\"IsCreated\":false,\"Command\":null,\"Id\":null,\"ExternalId\":\"e6489250-5182-4243-a2f8-f089c1c68235\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-11-06T06:16:05.017Z\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":null,\"SubTitle\":null,\"DisplayNameTitle\":\"Título\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":null,\"H1\":null,\"H2\":null,\"H1AndTitle\":\"\",\"H2AndSubTitle\":\"\",\"CustomTitleOrH2\":null,\"CustomTitleOrH1\":null,\"SubTitlePropertyName\":null}],\"CurrentStep\":1,\"RegisterDone\":true,\"MaxSteps\":1,\"Active\":null,\"IsCreated\":true,\"Command\":null,\"Id\":18,\"ExternalId\":\"8c789fd0-4323-4959-82af-341c99bb7eca\",\"FieldsToValidate\":[],\"CreatedAt\":\"2023-11-06T03:36:15\",\"UpdatedAt\":null,\"TititleWithSubtitle\":\"ded bf e a cbb\",\"IsSubmiting\":false,\"IsManuallySubmiting\":false,\"Title\":\"ded bf e a cbb\",\"SubTitle\":null,\"DisplayNameTitle\":\"Name\",\"DisplayNameSubTitle\":\"SubTítulo\",\"TitlePropertyName\":\"Name\",\"H1\":\"REGISTER USER AND PROFILE ACCESSES\",\"H2\":\"User Registration\",\"H1AndTitle\":\"REGISTER USER AND PROFILE ACCESSES - ded bf e a cbb\",\"H2AndSubTitle\":\"User Registration\",\"CustomTitleOrH2\":\"ded bf e a cbb\",\"CustomTitleOrH1\":\"ded bf e a cbb\",\"SubTitlePropertyName\":null}");
            //userDTO2!.ExternalId = userDTO!.ExternalId;
            userDTO2.Command = new CreateUserCommand(_logRequestContext, userDTO2);
            var createResult2 = await _userAppService.Create(userDTO2);

            //updatedUser = await _userRepository.FindAsync(x => x.Id == userDTO.Id);
            updatedUser = await _userAppService.Get(new UserQueryModel { ExternalIdEqual = userDTO2.ExternalId });
            Assert.NotEmpty(updatedUser.Accesses);

            //Assert.Empty(createResult2.Errors);

        }
    }
}
