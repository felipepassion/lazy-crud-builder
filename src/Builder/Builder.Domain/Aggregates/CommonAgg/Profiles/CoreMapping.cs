using AutoMapper;
using Lazy.Crud.Builder.Application.DTO.Aggregates.CommonAgg.ValueObjects;
using Lazy.Crud.Builder.Application.DTO.Seedwork.ValueObjects;
using Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.ValueObjects;

namespace Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.Profiles
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

