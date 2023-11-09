using LazyCrudBuilder.Core.Application.DTO.Attributes;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrudBuilder.Core.Domain.Attributes.T4;
using LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Entities;
using System.ComponentModel;

namespace LazyCrudBuilder.Users.Domain.Aggregates.SystemSettingsAgg.Entities
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