using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolSvcData.Models
{
    public class TSvcDrMudMotorModel
    {
        public int TSvcDrMudMotorId { get; set; }
        public int  ToolSvcId { get; set; }
        public string? TopConn { get; set; }
        public string? Stabilizer1 { get; set; }
        public string? Stabilizer2 { get; set; }
        public string? BottomConn { get; set; }
        public bool RnD { get; set; }
        public string? RnDReason { get; set; }
        public decimal EndPlay { get; set; }
        public string? EndPlayEnteredBy { get; set; }
        public bool? FreeRotation { get; set; }
        public string? Lobe { get; set; }
        public string? Stage { get; set; }
        public string? MotorDegreeType { get; set; }
        public decimal MotorDegreeSetting { get; set; }
        public DateTime? MotorDegreeEnteredDate { get; set; }
        public string? MotorDegreeEnteredBy { get; set; }
        public decimal WearPadSize { get; set; }
        public DateTime? WearPadDate { get; set; }
        public string? WearPadEnteredBy { get; set; }
        public bool? BacklashKeys { get; set; }
        public bool? Float { get; set; }
        public decimal FloatSize { get; set; }
        public string? CameraUsed { get; set; }
        public string? CameraUsedEnteredBy { get; set; }
        public string? DisassemblyBy { get; set; }
        public DateTime? DisassemblyDate { get; set; }
        public string? Comments { get; set; }
        public string? SupervisorApproved { get; set; }
        public DateTime? SupervisorApprovedDate { get; set; }
        public string? EnteredBy { get; set; }
        public DateTime? EnteredDate { get; set; }
        public string? LastChgBy { get; set; }
        public DateTime? LastChgDate { get; set; }
    }
}
