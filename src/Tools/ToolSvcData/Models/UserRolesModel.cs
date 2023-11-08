using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ToolSvcData.Models
{
    public class UserRolesModel
    {
        public bool IsAdministrator { get; set; } = false;
        public bool IsService { get; set; } = false;
        public bool IsSupervisor { get; set; } = false;

    }
}
