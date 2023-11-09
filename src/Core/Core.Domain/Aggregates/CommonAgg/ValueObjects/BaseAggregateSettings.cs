using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;

namespace LazyCrud.Core.Domain.Aggregates.CommonAgg.ValueObjects
{
    public class BaseAggregateSettings : Entity
    {
        public bool AutoSaveSettingsEnabled { get; set; } = true;
    }
}
