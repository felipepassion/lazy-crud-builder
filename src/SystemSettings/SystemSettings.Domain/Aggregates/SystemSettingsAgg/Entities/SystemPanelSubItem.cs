using LazyCrudBuilder.Core.Application.DTO.Attributes;
using LazyCrudBuilder.Core.Domain.Attributes.T4;
using LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.ValueObjects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities
{
    [Steppable(1), EndpointsT4(EndpointTypes.HttpAll), H2("Submenu"), DoNotReplaceAfterGenerated]
    public class SystemPanelSubItem : BasePainel
    {
        [NotRequiredOnFrontT4, ListingPicker]
        public List<SystemPanelSubItem> SubItems { get; set; }
        public int? SystemPanelSubItemId { get; set; }

        public int SystemPanelId { get; set; }
        
        [Step(1), DisplayName("É Direct link?")]
        public bool IsSubItem { get; set; }
    }
}
