using AutoMapper;
using Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects;
using Niu.Nutri.Core.Application.DTO.Seedwork.ValueObjects;
using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.ValueObjects;

namespace Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Profiles
{
    public partial class CoreAggProfile : Profile
    {
        public CoreAggProfile()
        {
            CreateMap<AutoSaveSettings, AutoSaveSettingsDTO>()
                .ReverseMap();
            CreateMap<ImageFileInfo, ImageFileInfoDTO>()
                .ReverseMap();
        }
    }
}

