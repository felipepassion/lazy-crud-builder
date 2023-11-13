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
    public partial class ProdutoStep1Validator : BaseMarketPlaceAggValidator<ProdutoDTO>
	{
        public ProdutoStep1Validator(HttpClient db)
                    : base(db)
        {
            
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
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
    public partial class CarrinhoStep1Validator : BaseMarketPlaceAggValidator<CarrinhoDTO>
	{
        public CarrinhoStep1Validator(HttpClient db)
                    : base(db)
        {
            
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
    public partial class CategoriaprodutoStep1Validator : BaseMarketPlaceAggValidator<CategoriaprodutoDTO>
	{
        public CategoriaprodutoStep1Validator(HttpClient db)
                    : base(db)
        {
            
            ConfigureAdditionalValidations();
        }
        partial void ConfigureAdditionalValidations();
    }
}
