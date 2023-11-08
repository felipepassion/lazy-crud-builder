using AutoMapper;
namespace LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Profiles
{
    public class SystemSettingsAggCoreProfile : Core.Domain.Aggregates.CommonAgg.Profiles.CoreAggProfile { }
}

namespace LazyCrud.SystemSettings.Domain.Aggregates.UsersAgg.Profiles
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
			CreateMap<UserDTO, User>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<User, UserDTO>();
			ConfigureAdditionalProfiles();
		}
		partial void ConfigureAdditionalProfiles();
	}
}

namespace LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Profiles
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
			CreateMap<CargaTabelaDTO, CargaTabela>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<CargaTabela, CargaTabelaDTO>();
			CreateMap<SystemSettingsAggSettingsDTO, SystemSettingsAggSettings>()
				.ForMember(x=>x.ExternalId, opt => opt.MapFrom(x=>x.ExternalId ?? Guid.NewGuid().ToString()));
			CreateMap<SystemSettingsAggSettings, SystemSettingsAggSettingsDTO>();
			ConfigureAdditionalProfiles();
		}
		partial void ConfigureAdditionalProfiles();
	}
}

