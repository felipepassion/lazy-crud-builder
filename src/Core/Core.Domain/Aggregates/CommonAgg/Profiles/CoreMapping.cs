using AutoMapper;
using LazyCrudBuilder.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.ValueObjects;

namespace LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Profiles
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

