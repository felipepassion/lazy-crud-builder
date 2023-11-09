        
namespace LazyCrudBuilder.Users.Application.DTO.Aggregates.UsersAgg.Validators {
    using Requests;
    public partial class UserProfileStep1Validator : BaseUsersAggValidator<UserProfileDTO>
	{
        partial void ConfigureAdditionalValidations() {}
    }
    public partial class UserProfileStep2Validator : BaseUsersAggValidator<UserProfileDTO>
	{
        partial void ConfigureAdditionalValidations() {}
    }
}
