using AutoMapper;
namespace Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Profiles
{
    public class DefaultTemplateAggCoreProfile : Core.Domain.Aggregates.CommonAgg.Profiles.CoreAggProfile { }
}

namespace Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Profiles
{
	using Application.DTO.Aggregates.DefaultTemplateAgg.Requests;
	using Entities;
	public partial class DefaultTemplateAggProfile : Profile
	{
		public DefaultTemplateAggProfile()
		{
			CreateMap<DefaultEntityDTO, DefaultEntity>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<DefaultEntity, DefaultEntityDTO>();
			CreateMap<DefaultTemplateAggSettingsDTO, DefaultTemplateAggSettings>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<DefaultTemplateAggSettings, DefaultTemplateAggSettingsDTO>();
			ConfigureAdditionalProfiles();
		}
		partial void ConfigureAdditionalProfiles();
	}
}

namespace Niu.Nutri.DefaultTemplate.Domain.Aggregates.UsersAgg.Profiles
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

