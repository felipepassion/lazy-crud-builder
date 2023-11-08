using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSvcData.Models
{
    public class ToolModel
    {
        public int UsageNum { get; set; }
        public string? SerialNum { get; set; }
        public string? JobNum { get; set; }
        public string? PartNum { get; set;}
        public string? Status { get; set; }
        public string? ShopStatus { get; set; }
        public bool? UsedOnJob { get; set; }
    }
}
