using Microsoft.EntityFrameworkCore;
using Niu.Nutri.Core.Domain.Attributes.T4;

namespace Niu.Nutri.Core.Domain.Aggregates.CommonAgg.ValueObjects
{
    [Keyless]
    public class ImageFileInfo
    {
        [Step(1), RequiredT4]
        public byte[]? Image { get; set; }
    }
}
