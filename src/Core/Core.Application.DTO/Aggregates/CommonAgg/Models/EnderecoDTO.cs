using LazyCrudBuilder.Core.Application.DTO.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LazyCrudBuilder.Core.Application.DTO.Aggregates.CommonAgg.Models
{
    public interface IBaseEnderecoDTO
    {
        public string CEP { get; set; }
        public string? Logradouro { get; set; }
        public string? TipoLogradouro { get; set; }
        public string? Bairro_Distrito { get; set; }
        public string? Cidade_Localidade { get; set; }
        public string? UF { get; set; }
    }
    public class BaseEnderecoDTO : EntityDTO
    {
        [Required, Title, DisplayName("CEP")]
        public string CEP { get; set; }
        
        [Required, Subtitle]
        public string? Logradouro { get; set; }
        
        [Required]
        public string? TipoLogradouro { get; set; }
        
        [Required, DisplayName("Bairro / Distrito")]
        public string? Bairro_Distrito { get; set; }

        [Required, DisplayName("Cidade / Localidade"), DisplayOnList(order: 3)]
        public string? Cidade_Localidade { get; set; }
        
        [Required]
        public string? UF { get; set; }
    }
}
