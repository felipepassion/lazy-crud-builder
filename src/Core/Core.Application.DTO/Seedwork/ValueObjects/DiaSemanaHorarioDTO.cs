namespace LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects
{
    public class DiaSemanaHorarioDTO
    {
        public bool Active { get; set; }
        public TimeOnly HoraInicial { get; set; }
        public TimeOnly HoraFinal { get; set; }
    }
}