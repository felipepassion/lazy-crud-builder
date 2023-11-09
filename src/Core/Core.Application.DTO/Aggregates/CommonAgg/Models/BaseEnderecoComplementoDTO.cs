using System.ComponentModel;

namespace LazyCrudBuilder.Core.Application.DTO.Aggregates.CommonAgg.Models
{
    public interface IBaseEnderecoComplementoDTO<T>
        where T : IBaseEnderecoDTO, new()
    {
        public int GrupoEmpresaId { get; set; }

        public abstract int? EnderecoId { get; set; }

        public abstract string Numero { get; set; }

        public abstract string Referencia { get; set; }

        public abstract string TipoEndereco { get; set; }

        public abstract string Complemento { get; set; }

        public abstract T Endereco { get; set; }
    }

    public abstract class BaseEnderecoComplementoDTO<T> : EntityDTO
        where T : BaseEnderecoDTO, new()
    {
        public BaseEnderecoComplementoDTO()
        {
            this.Endereco = new T();
        }

        public int GrupoEmpresaId { get; set; }

        public abstract int? EnderecoId { get; set; }

        [DisplayName("Número")] 
        public abstract string Numero { get; set; }

        [DisplayName("Referência")] 
        public abstract string Referencia { get; set; }

        [DisplayName("Tipo de Endereço")] 
        public abstract string TipoEndereco { get; set; }

        [DisplayName("Complemento")]
        public abstract string Complemento { get; set; }

        public abstract T Endereco { get; set; }
    }

    public interface IBaseEnderecoComplemento
    {

    }
}
