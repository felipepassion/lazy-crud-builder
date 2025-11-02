

using Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Niu.Nutri.Core.Application.DTO.Attributes;

namespace Niu.Nutri.DefaultTemplate.Application.DTO.Aggregates.DefaultTemplateAgg.Requests 
{
public partial class DefaultEntityDTO : EntityDTO
	{
	}
public partial class DefaultTemplateAggSettingsDTO : BaseAggregateSettingsDTO
	{
	    public  int UserId { get; set; }
	}
}
namespace Niu.Nutri.DefaultTemplate.Application.DTO.Aggregates.UsersAgg.Requests 
{
public partial class UserDTO : EntityDTO
	{
	}
}
