

using LazyCrudBuilder.Core.Application.DTO.Aggregates.CommonAgg.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LazyCrudBuilder.Core.Application.DTO.Attributes;

namespace LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.UsersAgg.Requests 
{
public partial class UserProfileAccessDTO : SteppableEntityDTO
	{
	    [Title] public  string Description { get; set; }
	    public  bool CanInsert { get; set; }
	    public  bool CanUpdate { get; set; }
	    public  bool CanList { get; set; }
	    public  bool CanDelete { get; set; }
	    public  int? SystemPanelSubItemId { get; set; }
	    public  int? SystemPanelId { get; set; }
	    public  int? SystemPanelGroupId { get; set; }
	    public  bool IsSubItem { get; set; }
	    public  int? ParentId { get; set; }
	}
public partial class UserDTO : EntityDTO
	{
	}
}
namespace LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests 
{
 [H2("Submenu")] public partial class SystemPanelSubItemDTO : BasePainelDTO
	{
	    public List<LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelSubItemDTO> SubItems { get; set; } = new List<LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelSubItemDTO>();
	    public  int? SystemPanelSubItemId { get; set; }
	    public  int SystemPanelId { get; set; }
	    [DisplayName("É Direct link?")] public  bool IsSubItem { get; set; }
	}
 [H2("Sidebar")] public partial class SystemPanelDTO : SteppableEntityDTO
	{
	    [DisplayOnList(0)] public  string Icon { get; set; }
	    [DisplayOnList,DisplayName("Menu"),Title] public  string Description { get; set; }
	    public List<LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelGroupDTO> GroupOfMenus { get; set; } = new List<LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelGroupDTO>();
	    public List<LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelSubItemDTO> SubItems { get; set; } = new List<LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelSubItemDTO>();
	    public List<LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileAccessDTO> AccessesOfMyProfile { get; set; } = new List<LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileAccessDTO>();
	}
 [H2("Grupo de Menus / Painéis")] public partial class SystemPanelGroupDTO : SteppableEntityDTO
	{
	    public  string Icon { get; set; }
	    [DisplayOnList,DisplayName("Description"),Title] public  string Description { get; set; }
	    [DisplayOnList,DisplayName("Code"),Subtitle] public  string Code { get; set; }
	    [DisplayOnList,DisplayName("Menus")] public List<LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelDTO> SubItems { get; set; } = new List<LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests.SystemPanelDTO>();
	    public List<LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileAccessDTO> AccessesOfMyProfile { get; set; } = new List<LazyCrudBuilder.SystemSettings.Application.DTO.Aggregates.UsersAgg.Requests.UserProfileAccessDTO>();
	}
public partial class CargaTabelaDTO : SteppableEntityDTO
	{
	    [DisplayOnList(0),DisplayName("Name da Tabela"),Title] public  string TableName { get; set; }
	    [DisplayOnList(1),DisplayName("Caminho do arquivo .csv")] public  string FilePath { get; set; }
	    [DisplayName("Inicializado?")] public  bool IsInitialized { get; set; }
	    public  byte[] ArquivoCSV { get; set; }
	    public  int? Total { get; set; }
	}
public partial class SystemSettingsAggSettingsDTO : BaseAggregateSettingsDTO
	{
	}
}
