using System.ComponentModel.DataAnnotations;
using LazyCrudBuilder.Core.Application.DTO.Attributes;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Entities;
using LazyCrudBuilder.Core.Domain.Attributes.T4;

namespace LazyCrudBuilder.SystemSettings.Domain.Aggregates.SystemSettingsAgg.ValueObjects
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
