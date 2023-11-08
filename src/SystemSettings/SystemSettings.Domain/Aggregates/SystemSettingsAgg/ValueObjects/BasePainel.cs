using System.ComponentModel.DataAnnotations;
using LazyCrud.Core.Application.DTO.Attributes;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrud.Core.Domain.Attributes.T4;

namespace LazyCrud.SystemSettings.Domain.Aggregates.SystemSettingsAgg.ValueObjects
{
    public class BasePainel : SteppableEntity
    {
        [Step(1)]
        public string? Icon { get; set; }

        [Step(1), Title, DisplayOnList, Required, Unique]
        public string Description { get; set; }

        [Step(1), Subtitle, DisplayOnList]
        public string? Url { get; set; }

        [Step(1)]
        public bool LinkDireto { get; set; } = true;

        [Step(1)]
        public bool ActionButton { get; set; }
    }
}
