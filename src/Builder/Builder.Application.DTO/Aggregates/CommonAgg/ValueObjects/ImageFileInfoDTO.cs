using System.ComponentModel.DataAnnotations;

namespace Lazy.Crud.Builder.Application.DTO.Aggregates.CommonAgg.ValueObjects
{
    public interface IImageFileInfoDTO
    {
        byte[]? Image { get; set; }
        string? Name { get; set; }
        string? Name2 { get; }
        string? Src { get; set; }
        string? Type { get; set; }
        bool IsLoading { get; set; }
    }

    public class ImageFileInfoDTO : IImageFileInfoDTO
    {
        public string? Type { get; set; }

        public string? Name { get; set; }

        public string Name2 => Src == null ? "Arquivo" : string.Join(".", Src?.Split(@"/").Last()?.Split(".").TakeLast(2)!);

        [Required]
        public byte[]? Image { get; set; }

        public string? Src { get; set; }
        public bool IsLoading { get; set; }
    }
}
