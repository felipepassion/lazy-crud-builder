using System.ComponentModel.DataAnnotations;

namespace LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects
{
    public class ContactNumeroDTO
    {
        [Required(ErrorMessage = "'Contact' precisa ser informado")]
        public string Numero { get; set; }
    }
}
