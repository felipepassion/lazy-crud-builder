using Microsoft.EntityFrameworkCore;
using Lazy.Crud.Builder.Domain.Attributes.T4;

namespace Lazy.Crud.Builder.Domain.Aggregates.CommonAgg.ValueObjects
{
    [Keyless]
    public class ImageFileInfo
    {
        [Step(1), RequiredT4]
        public byte[]? Image { get; set; }
    }
}
