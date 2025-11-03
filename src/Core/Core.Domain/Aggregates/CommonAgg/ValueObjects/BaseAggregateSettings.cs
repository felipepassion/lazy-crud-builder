using Lazy.Crud.Core.Domain.Aggregates.CommonAgg.Entities;

namespace Lazy.Crud.Core.Domain.Aggregates.CommonAgg.ValueObjects
{
    public class BaseAggregateSettings : Entity
    {
        public bool AutoSaveSettingsEnabled { get; set; } = true;
    }
}
