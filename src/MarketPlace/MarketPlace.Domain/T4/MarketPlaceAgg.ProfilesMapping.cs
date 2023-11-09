using AutoMapper;
namespace LazyCrudBuilder.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Profiles
{
    public class MarketPlaceAggCoreProfile : Core.Domain.Aggregates.CommonAgg.Profiles.CoreAggProfile { }
}

namespace LazyCrudBuilder.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Profiles
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

namespace LazyCrudBuilder.MarketPlace.Domain.Aggregates.UsersAgg.Profiles
{
	using Application.DTO.Aggregates.UsersAgg.Requests;
	using Entities;
	public partial class UsersAggProfile : Profile
	{
		public UsersAggProfile()
		{
			CreateMap<UserDTO, User>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<User, UserDTO>();
			ConfigureAdditionalProfiles();
		}
		partial void ConfigureAdditionalProfiles();
	}
}

