using Lazy.Crud.Core.Application.DTO.Aggregates.CommonAgg.Models;
using Lazy.Crud.Core.Application.DTO.Aggregates.CommonAgg.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FluentValidation;
namespace Lazy.Crud.DefaultTemplate.Application.DTO.Aggregates.DefaultTemplateAgg.Validators {
    public class BaseDefaultTemplateAggValidator<T> : BaseValidator<T>
        where T : EntityDTO
    {
            public BaseDefaultTemplateAggValidator() : base(){ }
            public BaseDefaultTemplateAggValidator(HttpClient db) : base(db){ }
    }
}
namespace Lazy.Crud.DefaultTemplate.Application.DTO.Aggregates.DefaultTemplateAgg.Validators 
{
	using Requests;
    public partial class DefaultEntityStep1Validator : BaseDefaultTemplateAggValidator<DefaultEntityDTO>
	{
        public DefaultEntityStep1Validator(HttpClient db)
                    : base(db)
        {
            
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class DefaultTemplateAggSettingsStep1Validator : BaseDefaultTemplateAggValidator<DefaultTemplateAggSettingsDTO>
	{
        public DefaultTemplateAggSettingsStep1Validator(HttpClient db)
                    : base(db)
        {
            RuleFor(Q => Q.UserId).NotEmpty();
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
}
