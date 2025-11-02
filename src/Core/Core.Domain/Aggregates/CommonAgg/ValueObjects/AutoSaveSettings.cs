using Microsoft.EntityFrameworkCore;

namespace Niu.Nutri.Core.Domain.Aggregates.CommonAgg.ValueObjects
{
    [Keyless]
    public class AutoSaveSettings
    {
        public bool Enabled { get; set; } = true;
    }
}
