using AutoMapper;
namespace LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Profiles
{
    public class UsersAggCoreProfile : Core.Domain.Aggregates.CommonAgg.Profiles.CoreAggProfile { }
}

namespace LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Profiles
{
	using Application.DTO.Aggregates.UsersAgg.Requests;
	using Entities;
	public partial class UsersAggProfile : Profile
	{
		public UsersAggProfile()
		{
			CreateMap<UserProfileAccessDTO, UserProfileAccess>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<UserProfileAccess, UserProfileAccessDTO>();
			CreateMap<UserCurrentAccessSelectedDTO, UserCurrentAccessSelected>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<UserCurrentAccessSelected, UserCurrentAccessSelectedDTO>();
			CreateMap<UserProfileListDTO, UserProfileList>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<UserProfileList, UserProfileListDTO>();
			CreateMap<UserProfileDTO, UserProfile>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<UserProfile, UserProfileDTO>();
			CreateMap<UsersAggSettingsDTO, UsersAggSettings>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<UsersAggSettings, UsersAggSettingsDTO>();
			CreateMap<UserDTO, User>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<User, UserDTO>();
			CreateMap<UserContactDTO, UserContact>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<UserContact, UserContactDTO>();
			ConfigureAdditionalProfiles();
		}
		partial void ConfigureAdditionalProfiles();
	}
}

namespace LazyCrudBuilder.Users.Domain.Aggregates.SystemSettingsAgg.Profiles
{
	using Application.DTO.Aggregates.SystemSettingsAgg.Requests;
	using Entities;
	public partial class SystemSettingsAggProfile : Profile
	{
		public SystemSettingsAggProfile()
		{
			CreateMap<SystemPanelSubItemDTO, SystemPanelSubItem>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<SystemPanelSubItem, SystemPanelSubItemDTO>();
			CreateMap<SystemPanelDTO, SystemPanel>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<SystemPanel, SystemPanelDTO>();
			CreateMap<SystemPanelGroupDTO, SystemPanelGroup>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<SystemPanelGroup, SystemPanelGroupDTO>();
			ConfigureAdditionalProfiles();
		}
		partial void ConfigureAdditionalProfiles();
	}
}

