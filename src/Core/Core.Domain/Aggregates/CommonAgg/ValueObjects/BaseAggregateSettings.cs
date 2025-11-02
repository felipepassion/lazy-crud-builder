using Niu.Nutri.Core.Domain.Aggregates.CommonAgg.Entities;

namespace Niu.Nutri.Core.Domain.Aggregates.CommonAgg.ValueObjects
{
    public class BaseAggregateSettings : Entity
    {
        public bool AutoSaveSettingsEnabled { get; set; } = true;
    }
}
