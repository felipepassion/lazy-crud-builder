using LazyCrudBuilder.Core.Domain.Attributes.T4;
using LazyCrudBuilder.Core.Application.DTO.Attributes;
using System.ComponentModel.DataAnnotations;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrudBuilder.SystemSettings.Domain.Aggregates.UsersAgg.Entities;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities
{
    [DoNotReplaceAfterGenerated]
    [EndpointsT4(EndpointTypes.HttpAll), Steppable(1), H2("Sidebar")]
    public class SystemPanel : SteppableEntity
    {
        [Step(1), DisplayOnList(0)]
        public string? Icon { get; set; }

        [Step(1), Title, DisplayOnList, Required, Unique, DisplayName("Menu")]
        public string Description { get; set; }

        public List<SystemPanelGroup> GroupOfMenus { get; set; } = new List<SystemPanelGroup>();

        [ListingPicker]
        public List<SystemPanelSubItem> SubItems { get; set; } = new List<SystemPanelSubItem>();

        [ListingPicker]
        public List<UserProfileAccess> AccessesOfMyProfile { get; set; } = new List<UserProfileAccess>();
    }
}