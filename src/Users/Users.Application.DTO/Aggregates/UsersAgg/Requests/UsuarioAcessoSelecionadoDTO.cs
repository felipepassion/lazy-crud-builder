
using LazyCrud.Users.Application.DTO.Aggregates.SystemSettingsAgg.Requests;
using LazyCrud.Users.Enumerations;

namespace LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests
{
    public partial class UserCurrentAccessSelectedDTO
    {
        public UserDTO User { get; set; }
        public UserProfileDTO UserProfile { get; set; }
        public SystemPanelSubItemDTO SelectedPage { get; set; } = new SystemPanelSubItemDTO();
        public UserProfileAccessDTO AccessOfThisPage { get; set; } = new UserProfileAccessDTO();
        public bool IsInitialized { get; set; }
        public Func<Task> RefreshHeaderUserInfos { get; set; }
        public Func<Task> RefreshFooterUserInfos { get; set; }
        public UserRole? MyCurrentRole => Enum.TryParse<UserRole>(this.UserProfile?.Code, out var val) ? val : null;
    }
}
