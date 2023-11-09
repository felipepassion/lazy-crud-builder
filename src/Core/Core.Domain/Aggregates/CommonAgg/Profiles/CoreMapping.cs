using AutoMapper;
using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.ValueObjects;

namespace LazyCrud.Core.Domain.Aggregates.CommonAgg.Profiles
{
    public partial class CoreAggProfile : Profile
    {
        public CoreAggProfile()
        {
            CreateMap<AutoSaveSettings, AutoSaveSettingsDTO>()
                .ReverseMap();
            CreateMap<ContactNumero, ContactNumeroDTO>().ReverseMap();
        }
    }
}

