namespace Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Models
{
    public interface IActivableEntityDTO : IEntityDTO { public bool? Active { get; set; } }

    public abstract class ActivableEntityDTO : EntityDTO, IActivableEntityDTO
    {
        public bool? Active { get; set; } = true;
    }
}
