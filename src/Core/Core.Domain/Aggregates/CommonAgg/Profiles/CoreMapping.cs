using AutoMapper;
using Lazy.Crud.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects;
using Lazy.Crud.Core.Application.DTO.Seedwork.ValueObjects;
using Lazy.Crud.Core.Domain.Aggregates.CommonAgg.ValueObjects;

namespace Lazy.Crud.Core.Domain.Aggregates.CommonAgg.Profiles
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

