using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolSvcData.Models.Interfaces;

namespace ToolSvcData.Models
{
    public class TSvcBuildDisplayModel : ITSvcBuildModel
    {
        public int TSvcBuildId { get; set; }
        public int ToolSvcId { get; set; }
        public int ToolPartId { get; set; }
        public int ItemNameId { get; set; }
        public string? PartName { get; set; }
        public int BuildLineId { get; set; }
        public int ComponentId { get; set; }
        public string? PartNum { get; set; }
        public string? BuildWO { get; set; }
        public string? ActualWO { get; set; }
        public string? EnteredBy { get; set; }
        public DateTime? EnteredDate { get; set; }
        public string? LastChgBy { get; set; }
        public DateTime? LastChgDate { get; set; }

    }
}
