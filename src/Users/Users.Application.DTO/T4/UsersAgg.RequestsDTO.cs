

using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LazyCrud.Core.Application.DTO.Attributes;

namespace LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests 
{
public partial class UserProfileAccessDTO : SteppableEntityDTO
	{
	    [Title] public  string Description { get; set; }
	    public  int UserProfileId { get; set; }
	    public  int? SystemPanelSubItemId { get; set; }
	    public  int? SystemPanelId { get; set; }
	    public  int? SystemPanelGroupId { get; set; }
	    public  bool IsSubItem { get; set; }
	    public  int? ParentId { get; set; }
	    public  bool IsDirectLink { get; set; }
	    public  bool CanInsert { get; set; }
	    public  bool CanUpdate { get; set; }
	    public  bool CanList { get; set; }
	    public  bool CanDelete { get; set; }
	}
public partial class UserCurrentAccessSelectedDTO : EntityDTO
	{
	    public  int? UserProfileId { get; set; }
	}
public partial class UserProfileListDTO : SteppableEntityDTO
	{
	    public  int? UserId { get; set; }
	    public List<LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileDTO> UserProfiles { get; set; } = new List<LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileDTO>();
	}
public partial class UserProfileDTO : SteppableEntityDTO
	{
	    [DisplayOnList,DisplayName("Name do Perfil"),Title] public  string Description { get; set; }
	    [DisplayOnList,DisplayName("Code"),Subtitle] public  string Code { get; set; }
	    [DisplayOnList] public  string InitialPage { get; set; }
	    [DisplayOnList,DisplayName("Private Profile?")] public  bool IsPrivateProfile { get; set; }
	    public List<LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileAccessDTO> Accesses { get; set; } = new List<LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileAccessDTO>();
	}
public partial class UsersAggSettingsDTO : BaseAggregateSettingsDTO
	{
	    public  int UserId { get; set; }
	}
 [H1("REGISTER USER AND PROFILE ACCESSES")]  [HideTitleOnHeader]  [H2("User Registration")] public partial class UserDTO : SteppableEntityDTO
	{
	    public  string UserName { get; set; }
	    [DisplayOnList(2),Title] public  string Name { get; set; }
	    [DisplayOnList(4)] public  System.DateOnly? BirthDate { get; set; }
	    [DisplayOnList(5)] public  LazyCrud.Users.Enumerations.GenderEnum Gender { get; set; }
	    public  bool? NeedResetPassword { get; set; }
	    public  bool? NeedSendOnboardingMail { get; set; }
	    public  string ProfilePicture { get; set; }
	    public LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserContactDTO Contact { get; set; } = new LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserContactDTO();
	    [DisplayOnList(8)] public  bool? CanUpdatePassword { get; set; }
	    public LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserCurrentAccessSelectedDTO SelectedAccess { get; set; } = new LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserCurrentAccessSelectedDTO();
	    public List<LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileListDTO> Accesses { get; set; } = new List<LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileListDTO>();
	}
public partial class UserContactDTO : EntityDTO
	{
	    public List<LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects.ContactNumeroDTO> Contacts { get; set; } = new List<LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects.ContactNumeroDTO>();
	    [DisplayOnList(6)] public  string ContactNumbers { get; set; }
	    [DisplayOnList(7),DisplayName("E-Mail")] public  string Email { get; set; }
	}
}
namespace LazyCrud.Users.Application.DTO.Aggregates.SystemSettingsAgg.Requests 
{
public partial class SystemPanelSubItemDTO : EntityDTO
	{
	    public  int? SystemPanelSubItemId { get; set; }
	    public  int SystemPanelId { get; set; }
	    public  bool IsSubItem { get; set; }
	    public  bool LinkDireto { get; set; }
	    public  string Description { get; set; }
	    public  string Url { get; set; }
	    public  bool ActionButton { get; set; }
	}
public partial class SystemPanelDTO : EntityDTO
	{
	    public  string Description { get; set; }
	    public List<LazyCrud.Users.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelSubItemDTO> SubItems { get; set; } = new List<LazyCrud.Users.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelSubItemDTO>();
	    public List<LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileAccessDTO> AccessesOfMyProfile { get; set; } = new List<LazyCrud.Users.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileAccessDTO>();
	}
public partial class SystemPanelGroupDTO : EntityDTO
	{
	    [DisplayName("Description"),Title] public  string Description { get; set; }
	    [DisplayName("Menus")] public List<LazyCrud.Users.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelDTO> SubItems { get; set; } = new List<LazyCrud.Users.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelDTO>();
	}
}
