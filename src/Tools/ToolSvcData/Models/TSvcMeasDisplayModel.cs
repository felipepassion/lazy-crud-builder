using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolSvcData.Models.Interfaces;

namespace ToolSvcData.Models
{
    public class TSvcMeasDisplayModel : ITSvcMeasModel
    {
        public int TSvcMeasId { get; set; }
        public int ToolSvcId { get; set; }
        public int MeasurementId { get; set; }
        public decimal DecVal { get; set; }
        public int IntVal { get; set; }
        public string? EnteredBy { get; set; }
        public DateTime? EnteredDate { get; set; }
        public string? LastChgBy { get; set; }
        public DateTime? LastChgDate { get; set; }
        public string? PartName { get; set; }
        public string? Description { get; set; }
        public string? DataType { get; set; }

    }
}
