using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrudBuilder.Core.Domain.Attributes.T4;
using LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Entities;

namespace LazyCrudBuilder.Users.Domain.Aggregates.SystemSettingsAgg.Entities
{
    [EndpointsT4(EndpointTypes.HttpAll)]
    public class SystemPanel : Entity
    {
        public string Description { get; set; }

        public List<SystemPanelSubItem> SubItems { get; set; }
        public List<UserProfileAccess> AccessesOfMyProfile { get; set; }

        [IgnorePropertyT4]
        public List<SystemPanelGroup> GroupOfMenus { get; set; }
    }
}