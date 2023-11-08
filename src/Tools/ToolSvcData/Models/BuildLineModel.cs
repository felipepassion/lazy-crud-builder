using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSvcData.Models
{
    public class BuildLineModel
    {
        public int BuildLineId { get; set; }
        public int ComponentId { get; set; }
        public int ItemId { get; set; }
        public int ItemNameId { get; set; }
        public string? PartName { get; set; }
        public string? PartNum { get; set; }
        public string? WorkOrder { get; set; }
    }
}
