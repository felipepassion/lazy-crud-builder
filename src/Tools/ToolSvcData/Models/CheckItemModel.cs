using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSvcData.Models
{
    public class CheckItemModel
    {
        public int CheckItemId { get; set; }
        public int ToolTypeId { get; set; }
        public string? SvcType { get; set; }
        public string? Category { get; set; }
        public string? Prompt { get; set; }
        public int DisplayOrder { get; set; }
        public int Active { get; set; }
    }
}
