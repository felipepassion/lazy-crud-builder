using Microsoft.EntityFrameworkCore;
using Lazy.Crud.Core.Domain.Attributes.T4;

namespace Lazy.Crud.Core.Domain.Aggregates.CommonAgg.ValueObjects
{
    [Keyless]
    public class ImageFileInfo
    {
        [Step(1), RequiredT4]
        public byte[]? Image { get; set; }
    }
}
