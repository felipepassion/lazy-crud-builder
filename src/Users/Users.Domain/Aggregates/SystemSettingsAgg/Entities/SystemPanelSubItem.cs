using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrudBuilder.Core.Domain.Attributes.T4;

namespace LazyCrudBuilder.Users.Domain.Aggregates.SystemSettingsAgg.Entities
{
    [EndpointsT4(EndpointTypes.HttpAll)]
    public class SystemPanelSubItem : Entity
    {
        public int? SystemPanelSubItemId { get; set; }
        public int SystemPanelId { get; set; }
        public bool IsSubItem { get; set; }
        public bool LinkDireto { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool ActionButton { get; set; }

    }
}