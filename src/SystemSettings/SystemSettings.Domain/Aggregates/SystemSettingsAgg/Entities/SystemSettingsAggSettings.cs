using LazyCrudBuilder.Core.Application.DTO.Attributes;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.ValueObjects;
using LazyCrudBuilder.Core.Domain.Attributes.T4;

namespace LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.Entities
{
    [MigrationOrder(2), AggregateSettingsT4, EndpointsT4(EndpointTypes.HttpAll), DoNotReplaceAfterGenerated]
    public class SystemSettingsAggSettings : BaseAggregateSettings
    {
    }
}
