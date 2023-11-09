using AutoMapper;
using LazyCrudBuilder.Users.Identity;
using LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Entities;
using LazyCrudBuilder.Users.Application.DTO.Aggregates.UsersAgg.Requests;

namespace LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Profiles
{
    public class UserProfileProfile : Profile
    {
        public UserProfileProfile()
        {
            CreateMap<ApplicationUser, User>()
                .ForPath(x=>x.Contact.Email, x=>x.MapFrom(x=>x.UserName))
                .ForPath(x=>x.CurrentStep, x=>x.MapFrom(x=> 1))
                .ForMember(x=>x.Name, x=>x.MapFrom(x=>x.Name));

            CreateMap<User, ApplicationUser>()
                .ForMember(x => x.Email, x => x.MapFrom(x => x.Contact.Email))
                .ForMember(x => x.UserName, x => x.MapFrom(x => x.Contact.Email))
                .ForMember(x => x.Id, x => x.MapFrom(x => x.Id))
                .ForMember(x => x.EmailConfirmed, x => x.MapFrom(x => true))
                .ForMember(x => x.Name, x => x.MapFrom(x => x.Name));

            CreateMap<ApplicationUser, UserListiningDTO>()
                .ForPath(x => x.Contact_Email, x => x.MapFrom(x => x.UserName))
                .ForMember(x => x.Name, x => x.MapFrom(x => x.Name));
        }
    }
}
