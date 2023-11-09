using LazyCrudBuilder.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace LazyCrudBuilder.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects
{
    public class ImageFileInfoDTO : IImageFileInfoDTO
    {
        public string Type { get; set; }
        [Required]
        public string Name { get; set; }
        
        public string Name2 => Src == null ? "Arquivo" : (string.Join(".", Src?.Split(@"/").Last()?.Split(".").TakeLast(2)!));


        [Required]
        public byte[] Image { get; set; }

        public string Src { get; set; }
        public bool IsLoading { get; set; }
    }
}
