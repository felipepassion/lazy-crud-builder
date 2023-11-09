using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Entities;

namespace LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.ValueObjects
{
    public class BaseAggregateSettings : Entity
    {
        public bool AutoSaveSettingsEnabled { get; set; } = true;
    }
}
