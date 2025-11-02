using Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Models;
using Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FluentValidation;
using AutoMapper;
using Niu.Nutri.Core.Application.DTO.Attributes;
namespace Niu.Nutri.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Profiles
{
	using Application.DTO.Aggregates.DefaultTemplateAgg.Requests;
	using Entities;
	public partial class DefaultEntityListiningProfile : Profile
	{
		public DefaultEntityListiningProfile()
		{
			 CreateMap<DefaultEntity, DefaultEntityListiningDTO>();
		}
	}
	public partial class DefaultTemplateAggSettingsListiningProfile : Profile
	{
		public DefaultTemplateAggSettingsListiningProfile()
		{
			 CreateMap<DefaultTemplateAggSettings, DefaultTemplateAggSettingsListiningDTO>();
		}
	}
}
