using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Models;
using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FluentValidation;
namespace LazyCrud.MarketPlace.Application.DTO.Aggregates.MarketPlaceAgg.Validators {
    public class BaseMarketPlaceAggValidator<T> : BaseValidator<T>
        where T : EntityDTO
    {
            public BaseMarketPlaceAggValidator() : base(){ }
            public BaseMarketPlaceAggValidator(HttpClient db) : base(db){ }
    }
}
namespace LazyCrud.MarketPlace.Application.DTO.Aggregates.MarketPlaceAgg.Validators 
{
	using Requests;
    public partial class MarketPlaceAggSettingsStep1Validator : BaseMarketPlaceAggValidator<MarketPlaceAggSettingsDTO>
	{
        public MarketPlaceAggSettingsStep1Validator(HttpClient db)
                    : base(db)
        {
            RuleFor(Q => Q.UserId).NotEmpty();
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
}
