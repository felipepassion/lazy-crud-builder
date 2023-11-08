using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Models;
using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using FluentValidation;
using AutoMapper;
using LazyCrud.Core.Application.DTO.Attributes;
namespace LazyCrud.Users.Domain.Aggregates.UsersAgg.Profiles
{
	using Application.DTO.Aggregates.UsersAgg.Requests;
	using Entities;
	public partial class UserProfileAccessListiningProfile : Profile
	{
		public UserProfileAccessListiningProfile()
		{
			 CreateMap<UserProfileAccess, UserProfileAccessListiningDTO>();
		}
	}
	public partial class UserCurrentAccessSelectedListiningProfile : Profile
	{
		public UserCurrentAccessSelectedListiningProfile()
		{
			 CreateMap<UserCurrentAccessSelected, UserCurrentAccessSelectedListiningDTO>();
		}
	}
	public partial class UserProfileListListiningProfile : Profile
	{
		public UserProfileListListiningProfile()
		{
			 CreateMap<UserProfileList, UserProfileListListiningDTO>();
		}
	}
	public partial class UserProfileListiningProfile : Profile
	{
		public UserProfileListiningProfile()
		{
			 CreateMap<UserProfile, UserProfileListiningDTO>().ForMember(x=>x.Description, opt => opt.MapFrom(x=>x.Description)).ForMember(x=>x.Code, opt => opt.MapFrom(x=>x.Code)).ForMember(x=>x.InitialPage, opt => opt.MapFrom(x=>x.InitialPage)).ForMember(x=>x.IsPrivateProfile, opt => opt.MapFrom(x=>x.IsPrivateProfile));
		}
	}
	public partial class UsersAggSettingsListiningProfile : Profile
	{
		public UsersAggSettingsListiningProfile()
		{
			 CreateMap<UsersAggSettings, UsersAggSettingsListiningDTO>();
		}
	}
	public partial class UserListiningProfile : Profile
	{
		public UserListiningProfile()
		{
			 CreateMap<User, UserListiningDTO>().ForMember(x=>x.Name, opt => opt.MapFrom(x=>x.Name)).ForMember(x=>x.BirthDate, opt => opt.MapFrom(x=>x.BirthDate)).ForMember(x=>x.Gender, opt => opt.MapFrom(x=>x.Gender)).ForMember(x=>x.Contact_ContactNumbers, opt => opt.MapFrom(x=>x.Contact.ContactNumbers)).ForMember(x=>x.Contact_Email, opt => opt.MapFrom(x=>x.Contact.Email)).ForMember(x=>x.CanUpdatePassword, opt => opt.MapFrom(x=>x.CanUpdatePassword));
		}
	}
}
