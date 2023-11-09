using LazyCrudBuilder.Core.Application.DTO.Aggregates.CommonAgg.Models;
using LazyCrudBuilder.Core.Application.DTO.Aggregates.CommonAgg.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FluentValidation;
using AutoMapper;
using LazyCrudBuilder.Core.Application.DTO.Attributes;
namespace LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Profiles
{
	using Application.DTO.Aggregates.SystemSettingsAgg.Requests;
	using Entities;
	public partial class SystemPanelSubItemListiningProfile : Profile
	{
		public SystemPanelSubItemListiningProfile()
		{
			 CreateMap<SystemPanelSubItem, SystemPanelSubItemListiningDTO>();
		}
	}
	public partial class SystemPanelListiningProfile : Profile
	{
		public SystemPanelListiningProfile()
		{
			 CreateMap<SystemPanel, SystemPanelListiningDTO>().ForMember(x=>x.Icon, opt => opt.MapFrom(x=>x.Icon)).ForMember(x=>x.Description, opt => opt.MapFrom(x=>x.Description));
		}
	}
	public partial class SystemPanelGroupListiningProfile : Profile
	{
		public SystemPanelGroupListiningProfile()
		{
			 CreateMap<SystemPanelGroup, SystemPanelGroupListiningDTO>().ForMember(x=>x.Description, opt => opt.MapFrom(x=>x.Description)).ForMember(x=>x.Code, opt => opt.MapFrom(x=>x.Code)).ForMember(x=>x.SubItems, opt => opt.MapFrom(x=>x.SubItems));
		}
	}
	public partial class CargaTabelaListiningProfile : Profile
	{
		public CargaTabelaListiningProfile()
		{
			 CreateMap<CargaTabela, CargaTabelaListiningDTO>().ForMember(x=>x.TableName, opt => opt.MapFrom(x=>x.TableName)).ForMember(x=>x.FilePath, opt => opt.MapFrom(x=>x.FilePath));
		}
	}
	public partial class SystemSettingsAggSettingsListiningProfile : Profile
	{
		public SystemSettingsAggSettingsListiningProfile()
		{
			 CreateMap<SystemSettingsAggSettings, SystemSettingsAggSettingsListiningDTO>();
		}
	}
}
