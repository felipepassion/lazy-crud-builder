using Microsoft.EntityFrameworkCore;

namespace LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.ValueObjects
{
    [Keyless]
    public class AutoSaveSettings
    {
        public bool Enabled { get; set; } = true;
    }
}
