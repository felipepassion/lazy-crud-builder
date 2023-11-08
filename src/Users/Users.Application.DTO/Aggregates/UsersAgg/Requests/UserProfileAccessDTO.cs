

using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Models;

namespace LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests
{
    public partial class UserProfileAccessDTO : SteppableEntityDTO
    {
        public UserProfileAccessDTO()
        {
        }

        public UserProfileAccessDTO(int? userProfileId, string description, int? systemPanelId, int? systemPanelSubItemId, bool isSubItem, int? parentId, bool isDirectLink, bool actionButton = false)
        {
            if(userProfileId.HasValue)
                UserProfileId = userProfileId.Value;
            Description = description;
            SystemPanelSubItemId = systemPanelSubItemId;
            SystemPanelId = systemPanelId;
            IsSubItem = isSubItem;
            ParentId = parentId;
            IsDirectLink = isDirectLink || actionButton;
            this.ExternalId = Guid.NewGuid().ToString();
        }
    }
}
