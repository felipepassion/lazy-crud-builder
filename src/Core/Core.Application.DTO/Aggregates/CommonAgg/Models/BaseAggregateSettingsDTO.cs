namespace Niu.Nutri.Core.Application.DTO.Aggregates.CommonAgg.Models
{
    public class BaseAggregateSettingsDTO : EntityDTO
    {
        public bool AutoSaveSettingsEnabled { get; set; } = true;
    }
}
