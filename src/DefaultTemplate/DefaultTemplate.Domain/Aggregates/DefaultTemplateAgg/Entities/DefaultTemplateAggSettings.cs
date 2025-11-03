


using Lazy.Crud.DefaultTemplate.Domain.Aggregates.UsersAgg.Entities;
using Lazy.Crud.Core.Domain.Aggregates.CommonAgg.ValueObjects;
using Lazy.Crud.Core.Domain.Attributes.T4;
using System.ComponentModel.DataAnnotations;

namespace Lazy.Crud.DefaultTemplate.Domain.Aggregates.DefaultTemplateAgg.Entities
{
    [AggregateSettingsT4, EndpointsT4(EndpointTypes.HttpAll)]
    public class DefaultTemplateAggSettings : BaseAggregateSettings
    {
        [Required]
        public int UserId { get; set; }

        [IgnorePropertyT4OnRequest]
        public User User { get; set; }
    }
}
