using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSvcData.Models
{
    public class ToolPartModel
    {
        public int ToolPartId { get; set; }
        public int ToolTypeId { get; set; }
        public string? SvcType { get; set; }
        public int PartId { get; set; }
        public bool? WorkOrder { get; set; }
        public bool? Condition { get; set; }
        public string? ImagePath { get; set; }
        public int DisplayOrder { get; set; }
        public bool? Active { get; set; }
    }
}
