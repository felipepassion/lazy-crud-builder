

using LazyCrudBuilder.MarketPlace.Domain.Aggregates.UsersAgg.Entities;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.ValueObjects;
using LazyCrudBuilder.Core.Domain.Attributes.T4;
using System.ComponentModel.DataAnnotations;

namespace LazyCrudBuilder.MarketPlace.Domain.Aggregates.MarketPlaceAgg.Entities
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
