using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSvcData.Models
{
    public class CAMModel
    {
        public string? ToolSerialNum { get; set; }
        //public int EpCustNum { get; set; }                  //Epicor CustNum
        public string? EpCustName { get; set; }
        public string? RigName { get; set; }               //TurboTrax RigName
        public string? EpJobNum { get; set; }                //Work Order (Epicor JobHead JobNum) -- TravelerNum
        public int UsageNum { get; set; }
        public int RigJobNum { get; set; }                 // Rig Job Number
        public DateTime? SvcStartedDate { get; set; }
        public string? TopConn { get; set; }
        //public int ToolSizeId { get; set; }
        public string? ToolSize { get; set; }
        public string? Stabilizer1 { get; set; }
        public string? Stabilizer2 { get; set; }
        public string? BottomConn { get; set; }
        public bool RnD { get; set; }
        public string? RnDReason { get; set; }
        public decimal EndPlay { get; set; }
        public string? EndPlayInitials { get; set; }
        public bool? FreeRotation { get; set; }
        public string? Lobe { get; set; }
        public string? Stage { get; set; }
        public string? MotorDegreeType { get; set; }
        public decimal MotorDegreeSetting { get; set; }
        public DateTime MotorDegreeDate { get; set; }
        public string? MotorDegreeInitials { get; set; }
        public decimal WearPadSize { get; set; }
        public DateTime WearPadDate { get; set; }
        public string? WearPadInitials { get; set; }
        public bool? BacklashKeys { get; set; }
        public bool? Float { get; set; }
        public decimal FloatSize { get; set; }
        public string? AssemblyBy { get; set; }
        public DateTime AssemblyDate { get; set; }
        public string? Comments { get; set; }
        public string? SupervisorSignature { get; set; }
        public DateTime SupervisorDate { get; set; }
    }
}
