using LazyCrud.Core.Application.DTO.Attributes;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.Attributes.T4;

namespace LazyCrud.Users.Domain.Aggregates.UsersAgg.Entities
{
    [EndpointsT4(EndpointTypes.HttpAll)]
    public class UserProfileAccess : SteppableEntity
    {
        [Title]
        public string Description { get; set; }

        public int UserProfileId { get; set; }

        public int? SystemPanelSubItemId { get; set; }
        public int? SystemPanelId { get; set; }
        public int? SystemPanelGroupId { get; set; }

        public bool IsSubItem { get; set; }
        public int? ParentId { get; set; }
        public bool IsDirectLink { get; set; }

        public bool CanInsert { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanList { get; set; }
        public bool CanDelete { get; set; }
    }
}
