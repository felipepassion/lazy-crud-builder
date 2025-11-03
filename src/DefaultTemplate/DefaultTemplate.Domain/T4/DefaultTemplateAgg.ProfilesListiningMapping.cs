using Lazy.Crud.Core.Application.DTO.Aggregates.CommonAgg.Models;
using Lazy.Crud.Core.Application.DTO.Aggregates.CommonAgg.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FluentValidation;
using AutoMapper;
using Lazy.Crud.Core.Application.DTO.Attributes;
namespace Lazy.Crud.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Profiles
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
