using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSvcData.Models
{
    public class ToolSvcModel
    {
        public int ToolSvcId { get; set; }
        public int ToolTypeId { get; set; }
        public string? SvcType { get; set; }
        public string? ToolSerialNum { get; set; }
        public string? ToolSize { get; set; }
        public string? EpCustName { get; set; }
        public string? RigName { get; set; }               //TurboTrax RigName
        public string? EpJobNum { get; set; }              //Work Order (Epicor JobHead JobNum) -- TravelerNum
        public string? EpPartNum { get; set; }             //Work Order Part Num
        public int UsageNum { get; set; }
        public int RigJobNum { get; set; }                 // Rig Job Number
        public string? Comment { get; set; }
        public string? EnteredBy { get; set; }
        public DateTime? EnteredDate { get; set; }
        public string? LastChgBy { get; set; }
        public DateTime? LastChgDate { get; set; }

    }
}
