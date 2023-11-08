using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolSvcData.Models.Interfaces;

namespace ToolSvcData.Models
{
    public class TSvcCondModel : ITSvcCondModel
    {
        public int TSvcCondId { get; set; }
        public int ToolSvcId { get; set; }
        public int ToolPartId { get; set; }
        public string? PartName { get; set; }
        public int Condition { get; set; }
        public string? Comment { get; set; }
        public string? EnteredBy { get; set; }
        public DateTime? EnteredDate { get; set; }
        public string? LastChgBy { get; set; }
        public DateTime? LastChgDate { get; set; }
    }
}
