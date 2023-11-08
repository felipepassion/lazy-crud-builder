using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSvcLib.Models
{
    public class ButtonModel
    {
        public string? DisabledContent { get; set; }
        public string? DisabledCssClass { get; set; }
        public string? EnabledContent { get; set; }
        public string? EnabledCssClass { get; set; }
        public string? InProcessContent { get; set; }
        public string? InProcessCssClass { get; set; }
        public string? SuccessContent { get; set; }
        public string? SuccessCssClass { get; set; }
        public string? FailedContent { get; set; }
        public string? FailedCssClass { get; set; }
    }
}
