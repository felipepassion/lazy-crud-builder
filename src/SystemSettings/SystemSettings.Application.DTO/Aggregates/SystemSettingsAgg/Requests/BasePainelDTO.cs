using System.ComponentModel.DataAnnotations;
using LazyCrud.Core.Application.DTO.Aggregates.CommonAgg.Models;
using LazyCrud.Core.Application.DTO.Attributes;

namespace LazyCrud.SystemSettings.Application.DTO.Aggregates.SystemSettingsAgg.Requests
{
    public class BasePainelDTO : SteppableEntityDTO
    {
        public string? Icon { get; set; }

        [Title, Required]
        public string Description { get; set; }

        [Subtitle]
        public string? Url { get; set; }
        
        public bool LinkDireto { get; set; }
        public bool ActionButton { get; set; }

    }
}
