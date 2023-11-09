using LazyCrudBuilder.Core.Application.DTO.Attributes;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrudBuilder.Core.Domain.Attributes.T4;

namespace LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Entities
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
