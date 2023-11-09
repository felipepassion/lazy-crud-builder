using LazyCrudBuilder.Core.Application.DTO.Attributes;
using LazyCrudBuilder.Core.Domain.Attributes.T4;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.ValueObjects
{
    [Keyless]
    public class ImageFileInfo
    {
        [Required, RegisterOrder(0), IgnorePropertyT4, DisplayOnList]
        public byte[]? Image { get; set; }

        public string Src { get; set; }
    }
}
