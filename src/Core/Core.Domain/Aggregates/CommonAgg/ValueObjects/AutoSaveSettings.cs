using Microsoft.EntityFrameworkCore;

namespace Lazy.Crud.Core.Domain.Aggregates.CommonAgg.ValueObjects
{
    [Keyless]
    public class AutoSaveSettings
    {
        public bool Enabled { get; set; } = true;
    }
}
