using AutoMapper;
namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Profiles
{
    public class MarketPlaceAggCoreProfile : Core.Domain.Aggregates.CommonAgg.Profiles.CoreAggProfile { }
}

namespace LazyCrud.MarketPlace.Domain.Aggregates.UsersAgg.Profiles
{
	using Application.DTO.Aggregates.UsersAgg.Requests;
	using Entities;
	public partial class UsersAggProfile : Profile
	{
		public UsersAggProfile()
		{
			CreateMap<ProdutoDTO, Produto>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<Produto, ProdutoDTO>();
			CreateMap<UserDTO, User>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<User, UserDTO>();
			CreateMap<CarrinhoDTO, Carrinho>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<Carrinho, CarrinhoDTO>();
			CreateMap<CategoriaprodutoDTO, Categoriaproduto>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<Categoriaproduto, CategoriaprodutoDTO>();
			ConfigureAdditionalProfiles();
		}
		partial void ConfigureAdditionalProfiles();
	}
}

namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Profiles
{
	using Application.DTO.Aggregates.MarketPlaceAgg.Requests;
	using Entities;
	public partial class MarketPlaceAggProfile : Profile
	{
		public MarketPlaceAggProfile()
		{
			CreateMap<MarketPlaceAggSettingsDTO, MarketPlaceAggSettings>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<MarketPlaceAggSettings, MarketPlaceAggSettingsDTO>();
			ConfigureAdditionalProfiles();
		}
		partial void ConfigureAdditionalProfiles();
	}
}

