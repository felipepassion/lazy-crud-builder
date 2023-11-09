using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LazyCrud.Core.Application.DTO.Attributes;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.Attributes.T4;
using LazyCrud.SystemSettings.Domain.Aggregates.UsersAgg.Entities;

namespace LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities
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