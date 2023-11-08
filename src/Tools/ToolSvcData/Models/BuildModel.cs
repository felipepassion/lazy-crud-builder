using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSvcData.Models
{
    public class BuildModel
    {
        public int ToolId { get; set; }
        public int BuildId { get; set;}
        public string? TopConn { get; set; }
        public string? BottomConn { get; set;}
        public string? Lobe { get; set; }
        public string? Stage { get; set; }
        public string? StatorControlNum { get; set; }
        public string? RotorControlNum { get; set; }
        public string? Slick { get; set; }
        public string? Stabilizer1 { get; set; }
        public string? Stabilizer2 { get; set; }
        public string? Adjustable { get; set; }
        public string? Fixed { get;}
        public string? KickPad { get; set;}


    }
}
