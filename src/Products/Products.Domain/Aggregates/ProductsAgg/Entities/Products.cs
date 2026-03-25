
namespace Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Entities;

using Builder.Domain.Aggregates.CommonAgg.Entities;
using Builder.Domain.Attributes.T4;

[AggregateSettingsT4, EndpointsT4(EndpointTypes.HttpAll)]
public class Products : Entity
{
    public string? TestProperty { get; set; }

    //[IgnorePropertyT4OnRequest]
    //public User User { get; set; }
}
