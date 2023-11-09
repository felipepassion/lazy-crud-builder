using LazyCrudBuilder.Core.Application.DTO.Aggregates.CommonAgg.Models;
using LazyCrudBuilder.Core.Application.DTO.Aggregates.CommonAgg.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FluentValidation;
using AutoMapper;
using LazyCrudBuilder.Core.Application.DTO.Attributes;
namespace LazyCrudBuilder.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Profiles
{
	using Application.DTO.Aggregates.MarketPlaceAgg.Requests;
	using Entities;
	public partial class MarketPlaceAggSettingsListiningProfile : Profile
	{
		public MarketPlaceAggSettingsListiningProfile()
		{
			 CreateMap<MarketPlaceAggSettings, MarketPlaceAggSettingsListiningDTO>();
		}
	}
}
