using Microsoft.EntityFrameworkCore;

namespace LazyCrud.Core.Domain.Aggregates.CommonAgg.ValueObjects
{
    [Keyless]
    public class AutoSaveSettings
    {
        public bool Enabled { get; set; } = true;
    }
}
