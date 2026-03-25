namespace Lazy.Crud.Products.Application.DTO.Aggregates.ProductsAgg.Validators{
public class BaseProductsAggValidator<T> : BaseValidator<T>
    where T : EntityDTO
{
    public BaseProductsAggValidator() : base(){ }
    public BaseProductsAggValidator(HttpClient db) : base(db){ }
}
}
namespace Lazy.Crud.Products.Application.DTO.Aggregates.ProductsAgg.Validators 
{
using Requests;
public partial class ProductsStep1Validator : BaseProductsAggValidator<ProductsDTO>
{
    public ProductsStep1Validator(HttpClient db)
                : base(db)
    {
        
        ConfigureAdditionalValidations();
    }
    partial void ConfigureAdditionalValidations();
}
}
