using LazyCrud.Core.Application.DTO.Attributes;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.Attributes.T4;
using LazyCrud.Users.Domain.Aggregates.UsersAgg.Entities;
using System.ComponentModel;

namespace LazyCrud.Users.Domain.Aggregates.SystemSettingsAgg.Entities
{
    public class SystemPanelGroup : Entity
    {
        [Title, DisplayName("Description")]
        public string Description { get; set; }

        [IgnorePropertyT4]
        public List<UserProfileAccess> UserProfileAccesses { get; set; }

        [DisplayName("Menus")]
        public List<SystemPanel> SubItems { get; set; }
    }
}