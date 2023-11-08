using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.Attributes.T4;
using LazyCrud.Users.Domain.Aggregates.UsersAgg.Entities;

namespace LazyCrud.Users.Domain.Aggregates.SystemSettingsAgg.Entities
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