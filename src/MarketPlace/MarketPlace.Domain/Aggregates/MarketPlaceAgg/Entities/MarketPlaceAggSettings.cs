

using LazyCrud.MarketPlace.Domain.Aggregates.UsersAgg.Entities;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.ValueObjects;
using LazyCrud.Core.Domain.Attributes.T4;
using System.ComponentModel.DataAnnotations;

namespace LazyCrud.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Entities
{
    [AggregateSettingsT4, EndpointsT4(EndpointTypes.HttpAll)]
    public class MarketPlaceAggSettings : BaseAggregateSettings
    {
        [Required]
        public int UserId { get; set; }

        [IgnorePropertyT4OnRequest]
        public User User { get; set; }
    }
}
