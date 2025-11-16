using Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.Entities;

namespace Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.ValueObjects
{
    public class BaseAggregateSettings : Entity
    {
        public bool AutoSaveSettingsEnabled { get; set; } = true;
    }
}
