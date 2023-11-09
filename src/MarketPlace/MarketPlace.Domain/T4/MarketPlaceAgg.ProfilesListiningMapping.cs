using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Models;
using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FluentValidation;
using AutoMapper;
using LazyCrud.Core.Application.DTO.Attributes;
namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Profiles
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
