using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Models;
using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FluentValidation;
namespace LazyCrud.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Validators {
    public class BaseSystemSettingsAggValidator<T> : BaseValidator<T>
        where T : EntityDTO
    {
            public BaseSystemSettingsAggValidator() : base(){ }
            public BaseSystemSettingsAggValidator(HttpClient db) : base(db){ }
    }
}
namespace LazyCrud.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Validators 
{
	using Requests;
    public partial class SystemPanelSubItemStep1Validator : BaseSystemSettingsAggValidator<SystemPanelSubItemDTO>
	{
        public SystemPanelSubItemStep1Validator(HttpClient db)
                    : base(db)
        {
            RuleFor(x=>x).MustAsync((x, y) => BeUnique<SystemPanelSubItemDTO>(x.ExternalId, "Description", x.Description, CancellationToken.None)).WithMessage("Description já existente.").WithName("Description");RuleFor(Q => Q.Description).NotEmpty();
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class SystemPanelSubItemStep2Validator : BaseSystemSettingsAggValidator<SystemPanelSubItemDTO>
	{
        public SystemPanelSubItemStep2Validator(HttpClient db)
                    : base(db)
        {
            
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class SystemPanelStep1Validator : BaseSystemSettingsAggValidator<SystemPanelDTO>
	{
        public SystemPanelStep1Validator(HttpClient db)
                    : base(db)
        {
            RuleFor(x=>x).MustAsync((x, y) => BeUnique<SystemPanelDTO>(x.ExternalId, "Description", x.Description, CancellationToken.None)).WithMessage("'Menu' já existente.").WithName("Description");RuleFor(Q => Q.Description).NotEmpty();
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class SystemPanelStep2Validator : BaseSystemSettingsAggValidator<SystemPanelDTO>
	{
        public SystemPanelStep2Validator(HttpClient db)
                    : base(db)
        {
            
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class SystemPanelGroupStep1Validator : BaseSystemSettingsAggValidator<SystemPanelGroupDTO>
	{
        public SystemPanelGroupStep1Validator(HttpClient db)
                    : base(db)
        {
            RuleFor(x=>x).MustAsync((x, y) => BeUnique<SystemPanelGroupDTO>(x.ExternalId, "Description", x.Description, CancellationToken.None)).WithMessage("'Description' já existente.").WithName("Description");RuleFor(Q => Q.Description).NotEmpty();RuleFor(x=>x).MustAsync((x, y) => BeUnique<SystemPanelGroupDTO>(x.ExternalId, "Code", x.Code, CancellationToken.None)).WithMessage("'Code' já existente.").WithName("Code");RuleFor(Q => Q.Code).NotEmpty();
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class SystemPanelGroupStep2Validator : BaseSystemSettingsAggValidator<SystemPanelGroupDTO>
	{
        public SystemPanelGroupStep2Validator(HttpClient db)
                    : base(db)
        {
            
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class CargaTabelaStep1Validator : BaseSystemSettingsAggValidator<CargaTabelaDTO>
	{
        public CargaTabelaStep1Validator(HttpClient db)
                    : base(db)
        {
            RuleFor(Q => Q.ArquivoCSV).NotEmpty();
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class SystemSettingsAggSettingsStep1Validator : BaseSystemSettingsAggValidator<SystemSettingsAggSettingsDTO>
	{
        public SystemSettingsAggSettingsStep1Validator(HttpClient db)
                    : base(db)
        {
            
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class SystemSettingsAggSettingsStep2Validator : BaseSystemSettingsAggValidator<SystemSettingsAggSettingsDTO>
	{
        public SystemSettingsAggSettingsStep2Validator(HttpClient db)
                    : base(db)
        {
            
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
}
