using System.ComponentModel.DataAnnotations;

namespace Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects
{
    public class ContactNumberDTO
    {
        [Required(ErrorMessage = "'Contato' precisa ser informado")]
        public string Numero { get; set; }
    }
}
