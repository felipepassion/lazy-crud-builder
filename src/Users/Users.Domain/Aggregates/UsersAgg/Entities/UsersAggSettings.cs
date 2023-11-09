using LazyCrud.Core.Domain.Aggregates.CommonAgg.ValueObjects;
using LazyCrud.Core.Domain.Attributes.T4;
using System.ComponentModel.DataAnnotations;

namespace LazyCrud.Users.Domain.Aggregates.UsersAgg.Entities
{
    [MigrationOrder(3), AggregateSettingsT4, EndpointsT4(EndpointTypes.HttpAll)]
    public class UsersAggSettings : BaseAggregateSettings
    {
        [Required]
        public int UserId { get; set; }
    }
}
