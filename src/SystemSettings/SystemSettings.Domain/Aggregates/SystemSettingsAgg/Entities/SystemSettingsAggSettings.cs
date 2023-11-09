using LazyCrud.Core.Application.DTO.Attributes;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.ValueObjects;
using LazyCrud.Core.Domain.Attributes.T4;

namespace LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities
{
    [MigrationOrder(2), AggregateSettingsT4, EndpointsT4(EndpointTypes.HttpAll), DoNotReplaceAfterGenerated]
    public class SystemSettingsAggSettings : BaseAggregateSettings
    {
    }
}
