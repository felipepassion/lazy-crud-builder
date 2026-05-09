namespace Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Profiles
{
public class ProductsAggCoreProfile : Builder.Domain.Aggregates.CommonAgg.Profiles.CoreAggProfile { }
}

namespace Lazy.Crud.Products.Domain.Aggregates.ProductsAgg.Profiles
{
using Application.DTO.Aggregates.ProductsAgg.Requests;
using Entities;
public partial class ProductsAggProfile : Profile
{
	public ProductsAggProfile()
	{
		CreateMap<ProductsDTO, Products>()
			.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
		CreateMap<Products, ProductsDTO>();
		ConfigureAdditionalProfiles();
	}
	partial void ConfigureAdditionalProfiles();
}
}

