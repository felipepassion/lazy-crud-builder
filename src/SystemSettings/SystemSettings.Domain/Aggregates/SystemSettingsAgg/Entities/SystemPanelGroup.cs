using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LazyCrudBuilder.Core.Application.DTO.Attributes;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrudBuilder.Core.Domain.Attributes.T4;
using LazyCrudBuilder.SystemSettings.Domain.Aggregates.UsersAgg.Entities;

namespace LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities
{
    [EndpointsT4(EndpointTypes.HttpAll), Steppable(1), H2("Grupo de Menus / Painéis"), DoNotReplaceAfterGenerated]
    public class SystemPanelGroup : SteppableEntity
    {
        [Step(1)]
        public string? Icon { get; set; }

        [Step(1), Title, DisplayOnList, Required, Unique, DisplayName("Description")]
        public string Description { get; set; }

        [Step(1), Subtitle, DisplayOnList, Required, Unique, DisplayName("Code")]
        public string Code { get; set; }

        [Step(1), ListingPicker, DisplayName("Menus"), DisplayOnList]
        public List<SystemPanel> SubItems { get; set; } = [];

        [ListingPicker]
        public List<UserProfileAccess> AccessesOfMyProfile { get; set; } = [];
    }
}