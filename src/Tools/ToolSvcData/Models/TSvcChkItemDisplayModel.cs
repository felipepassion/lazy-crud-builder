using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolSvcData.Models.Interfaces;

namespace ToolSvcData.Models
{
    public class TSvcChkItemDisplayModel : ITSvcChkItemModel
    {
        public int TSvcChkItemId { get; set; }
        public int ToolSvcId { get; set; }
        public int CheckItemId { get; set; }
        public int Answer { get; set; }
        public string? EnteredBy { get; set; }
        public DateTime? EnteredDate { get; set; }
        public string? LastChgBy { get; set; }
        public DateTime? LastChgDate { get; set; }
        public string? Category { get; set; }
        public string? Prompt { get; set; }

    }
}
