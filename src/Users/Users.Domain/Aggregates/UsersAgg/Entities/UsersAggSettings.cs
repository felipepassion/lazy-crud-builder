using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.ValueObjects;
using LazyCrudBuilder.Core.Domain.Attributes.T4;
using System.ComponentModel.DataAnnotations;

namespace LazyCrudBuilder.Users.Domain.Aggregates.UsersAgg.Entities
{
    [MigrationOrder(3), AggregateSettingsT4, EndpointsT4(EndpointTypes.HttpAll)]
    public class UsersAggSettings : BaseAggregateSettings
    {
        [Required]
        public int UserId { get; set; }
    }
}
