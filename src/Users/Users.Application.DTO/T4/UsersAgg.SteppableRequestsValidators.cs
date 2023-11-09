using LazyCrudBuilder.Core.Application.DTO.Aggregates.CommonAgg.Models;
using LazyCrudBuilder.Core.Application.DTO.Aggregates.CommonAgg.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FluentValidation;
namespace LazyCrudBuilder.Users.Application.DTO.Aggregates.UsersAgg.Validators {
    public class BaseUsersAggValidator<T> : BaseValidator<T>
        where T : EntityDTO
    {
            public BaseUsersAggValidator() : base(){ }
            public BaseUsersAggValidator(HttpClient db) : base(db){ }
    }
}
namespace LazyCrudBuilder.Users.Application.DTO.Aggregates.UsersAgg.Validators 
{
	using Requests;
    public partial class UserProfileAccessStep1Validator : BaseUsersAggValidator<UserProfileAccessDTO>
	{
        public UserProfileAccessStep1Validator(HttpClient db)
                    : base(db)
        {
            
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class UserCurrentAccessSelectedStep1Validator : BaseUsersAggValidator<UserCurrentAccessSelectedDTO>
	{
        public UserCurrentAccessSelectedStep1Validator(HttpClient db)
                    : base(db)
        {
            
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class UserProfileListStep1Validator : BaseUsersAggValidator<UserProfileListDTO>
	{
        public UserProfileListStep1Validator(HttpClient db)
                    : base(db)
        {
            
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class UserProfileStep1Validator : BaseUsersAggValidator<UserProfileDTO>
	{
        public UserProfileStep1Validator(HttpClient db)
                    : base(db)
        {
            RuleFor(Q => Q.Description).NotEmpty();
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class UserProfileStep2Validator : BaseUsersAggValidator<UserProfileDTO>
	{
        public UserProfileStep2Validator(HttpClient db)
                    : base(db)
        {
            
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class UsersAggSettingsStep1Validator : BaseUsersAggValidator<UsersAggSettingsDTO>
	{
        public UsersAggSettingsStep1Validator(HttpClient db)
                    : base(db)
        {
            RuleFor(Q => Q.UserId).NotEmpty();
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class UsersAggSettingsStep2Validator : BaseUsersAggValidator<UsersAggSettingsDTO>
	{
        public UsersAggSettingsStep2Validator(HttpClient db)
                    : base(db)
        {
            RuleFor(Q => Q.UserId).NotEmpty();
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class UsersAggSettingsStep3Validator : BaseUsersAggValidator<UsersAggSettingsDTO>
	{
        public UsersAggSettingsStep3Validator(HttpClient db)
                    : base(db)
        {
            RuleFor(Q => Q.UserId).NotEmpty();
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class UserStep1Validator : BaseUsersAggValidator<UserDTO>
	{
        public UserStep1Validator(HttpClient db)
                    : base(db)
        {
            RuleFor(Q => Q.Name).NotEmpty();RuleFor(Q => Q.Contact.Email).NotEmpty();
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class UserStep2Validator : BaseUsersAggValidator<UserDTO>
	{
        public UserStep2Validator(HttpClient db)
                    : base(db)
        {
            
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
}
