using LazyCrudBuilder.Core.Application.DTO.Aggregates.CommonAgg.Models;

namespace LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.UsersAgg.Requests
{
    public partial class UserProfileAccessDTO : SteppableEntityDTO
    {
        public UserProfileAccessDTO()
        {
            
        }
        public UserProfileAccessDTO(string description, int systemPanelId, int systemPanelSubItemId)
        {
            Description = description;
            SystemPanelSubItemId = systemPanelSubItemId;
            SystemPanelId = systemPanelId;
        }
    }
}
