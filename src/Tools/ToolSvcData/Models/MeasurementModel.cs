using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSvcData.Models
{
    public class MeasurementModel
    {
        public int MeasurementId { get; set; }
        public int ToolPartId { get; set; }
        public string? Description { get; set; }
        public string? DataType { get; set; }
        public bool? Active { get; set; }
    }
}
