using System.ComponentModel.DataAnnotations;

namespace Lazy.Crud.Builder.Application.DTO.Aggregates.CommonAgg.ValueObjects
{
    public class ContactNumberDTO
    {
        [Required(ErrorMessage = "'Contato' precisa ser informado")]
        public string Numero { get; set; }
    }
}
