namespace Niu.Nutri.Core.Application.DTO.Seedwork.ValueObjects
{
    public class DiaSemanaHorarioDTO
    {
        public bool Active { get; set; }
        public TimeOnly HoraInicial { get; set; }
        public TimeOnly HoraFinal { get; set; }
    }
}