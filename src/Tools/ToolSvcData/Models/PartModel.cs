using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSvcData.Models
{
    public class PartModel
    {
        public int PartId { get; set; }
        public int ItemNameId { get; set; }
        public string? PartName { get; set; }
    }
}
