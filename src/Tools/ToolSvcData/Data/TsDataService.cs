using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using ToolSvcData.Access;
using ToolSvcData.Models;


namespace ToolSvcData.Data
{
    public class TsDataService
    {
        private readonly ISqlDataAccess _dataAccess;

        public TsDataService(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        public async Task<ToolSvcModel> ToolSvc_GetNewByJobNum(string epJobNum)
        {
            var _j = await _dataAccess.LoadData<ToolSvcModel, dynamic>("dbo.ToolSvc_GetNewByJobNum",
                                                                                   new { EpJobNum = epJobNum },
                                                                                   "DefaultConnection");
            return _j.FirstOrDefault();
        }

        public async Task<ToolSvcModel> ToolSvc_GetByJobNum(string epJobNum)
        {
            var _j = await _dataAccess.LoadData<ToolSvcModel, dynamic>("dbo.ToolSvc_GetByJobNum",
                                                                                   new { EpJobNum = epJobNum },
                                                                                   "DefaultConnection");
            return _j.FirstOrDefault();
        }

        public async Task<TSvcDrMudMotorModel> TSvcDrMudMotor_GetByToolSvcId(int toolSvcId)
        {
            var _mm = await _dataAccess.LoadData<TSvcDrMudMotorModel, dynamic>("dbo.TSvcDrMudMotor_GetByToolSvcId",
                                                                                   new { ToolSvcId = toolSvcId },
                                                                                   "DefaultConnection");
            return _mm.FirstOrDefault();
        }

        public async Task<List<ToolPartModel>> ToolPart_GetByTypeSvc(int toolTypeId, string svcType)
        {
            var _tp = await _dataAccess.LoadData<ToolPartModel, dynamic>("dbo.ToolPart_GetByTypeSvc",
                                                                                   new { ToolTypeId = toolTypeId,
                                                                                         SvcType = svcType },
                                                                                   "DefaultConnection");
            return _tp.ToList();
        }

        public async Task<List<TSvcBuildDisplayModel>> ToolPart_GetBuildList(int toolTypeId, string svcType)
        {
            var _tp = await _dataAccess.LoadData<TSvcBuildDisplayModel, dynamic>("dbo.ToolPart_GetBuildList",
                                                                                   new
                                                                                   {
                                                                                       ToolTypeId = toolTypeId,
                                                                                       SvcType = svcType
                                                                                   },
                                                                                   "DefaultConnection");
            return _tp.ToList();
        }
        
        public async Task<List<TSvcBuildDisplayModel>> TSvcBuild_GetByToolSvcId(int toolSvcId)
        {
            var _b = await _dataAccess.LoadData<TSvcBuildDisplayModel, dynamic>("dbo.TSvcBuild_GetByToolSvcId",
                                                                                   new
                                                                                   {
                                                                                       ToolSvcId = toolSvcId
                                                                                   },
                                                                                   "DefaultConnection");
            return _b.ToList();
        }

        public async Task<List<TSvcCondDisplayModel>> TSvcCond_GetDisplay(int toolTypeId, string svcType)
        {
            var _tp = await _dataAccess.LoadData<TSvcCondDisplayModel, dynamic>("dbo.TSvcCond_GetDisplay",
                                                                                   new
                                                                                   {
                                                                                       ToolTypeId = toolTypeId,
                                                                                       SvcType = svcType
                                                                                   },
                                                                                   "DefaultConnection");
            return _tp.ToList();
        }

        public async Task<List<TSvcMeasDisplayModel>> TSvcMeas_GetDisplay(int toolTypeId, string svcType)
        {
            var _m = await _dataAccess.LoadData<TSvcMeasDisplayModel, dynamic>("dbo.TSvcMeas_GetDisplay",
                                                                                   new
                                                                                   {
                                                                                       ToolTypeId = toolTypeId,
                                                                                       SvcType = svcType
                                                                                   },
                                                                                   "DefaultConnection");
            return _m.ToList();
        }

        public async Task<List<TSvcChkItemDisplayModel>> TSvcChkItem_GetDisplay(int toolTypeId, string svcType)
        {
            var _ci = await _dataAccess.LoadData<TSvcChkItemDisplayModel, dynamic>("dbo.TSvcChkItem_GetDisplay",
                                                                                   new
                                                                                   {
                                                                                       ToolTypeId = toolTypeId,
                                                                                       SvcType = svcType
                                                                                   },
                                                                                   "DefaultConnection");
            return _ci.ToList();
        }

        public async Task<List<TSvcChkItemDisplayModel>> TSvcChkItem_GetByToolSvcId(int toolSvcId)
        {
            var _ci = await _dataAccess.LoadData<TSvcChkItemDisplayModel, dynamic>("dbo.TSvcChkItem_GetByToolSvcId",
                                                                                   new
                                                                                   {
                                                                                       ToolSvcId = toolSvcId
                                                                                   },
                                                                                   "DefaultConnection");
            return _ci.ToList();
        }

        public async Task<List<TSvcCondDisplayModel>> TSvcCond_GetByToolSvcId(int toolSvcId)
        {
            var _i = await _dataAccess.LoadData<TSvcCondDisplayModel, dynamic>("dbo.TSvcCond_GetByToolSvcId",
                                                                                   new
                                                                                   {
                                                                                       ToolSvcId = toolSvcId
                                                                                   },
                                                                                   "DefaultConnection");
            return _i.ToList();
        }

        public async Task<List<TSvcMeasDisplayModel>> TSvcMeas_GetByToolSvcId(int toolSvcId)
        {
            var _m = await _dataAccess.LoadData<TSvcMeasDisplayModel, dynamic>("dbo.TSvcMeas_GetByToolSvcId",
                                                                                   new
                                                                                   {
                                                                                       ToolSvcId = toolSvcId
                                                                                   },
                                                                                   "DefaultConnection");
            return _m.ToList();
        }

        public async Task<int> ToolSvc_Insert(ToolSvcModel toolSvc)
        {
            var tSvc = new
            {
                toolSvc.ToolTypeId
               ,toolSvc.SvcType
               ,toolSvc.ToolSerialNum
               ,toolSvc.ToolSize
               ,toolSvc.EpCustName
               ,toolSvc.RigName
               ,toolSvc.EpJobNum
               ,toolSvc.EpPartNum
               ,toolSvc.UsageNum
               ,toolSvc.RigJobNum
               ,toolSvc.Comment
               ,toolSvc.EnteredBy
               ,toolSvc.EnteredDate
               ,toolSvc.LastChgBy
               ,toolSvc.LastChgDate
            };

            var _id = await _dataAccess.InsertData("dbo.ToolSvc_Insert", tSvc, "DefaultConnection");

            return _id;
        }

        public async Task<int> TSvcChkItem_Insert(TSvcChkItemDisplayModel chkItemResp)
        {
            var resp = new
            {
                chkItemResp.ToolSvcId
               ,chkItemResp.CheckItemId
               ,chkItemResp.Answer
               ,chkItemResp.EnteredBy
               ,chkItemResp.EnteredDate
               ,chkItemResp.LastChgBy
               ,chkItemResp.LastChgDate
            };

            var _id = await _dataAccess.InsertData("dbo.TSvcChkItem_Insert", resp, "DefaultConnection");

            return _id;
        }

        public async Task TSvcChkItem_Update(TSvcChkItemDisplayModel chkItemResp)
        {
            var resp = new
            {
                chkItemResp.TSvcChkItemId
               ,chkItemResp.Answer
               ,chkItemResp.EnteredBy
               ,chkItemResp.EnteredDate
               ,chkItemResp.LastChgBy
               ,chkItemResp.LastChgDate
            };

            await _dataAccess.SaveData("dbo.TSvcChkItem_Update", resp, "DefaultConnection");
        }

        public async Task ToolSvc_UpdateComment(ToolSvcModel toolSvc)
        {
            var c = new
            {
                toolSvc.ToolSvcId
               ,toolSvc.Comment
               ,toolSvc.LastChgBy
               ,toolSvc.LastChgDate
            };

            await _dataAccess.SaveData("dbo.ToolSvc_UpdateComment", c, "DefaultConnection");
        }

        public async Task TSvcDrMudMotor_UpdateRnD(TSvcDrMudMotorModel svcMM)
        {
            var r = new
            {
                svcMM.TSvcDrMudMotorId
               ,svcMM.RnD
               ,svcMM.RnDReason
               ,svcMM.LastChgBy
               ,svcMM.LastChgDate
            };

            await _dataAccess.SaveData("dbo.TSvcDrMudMotor_UpdateRnD", r, "DefaultConnection");
        }

        public async Task TSvcDrMudMotor_UpdateEndPlay(TSvcDrMudMotorModel svcMM)
        {
            var e = new
            {
                svcMM.TSvcDrMudMotorId
               ,svcMM.EndPlay
               ,svcMM.EndPlayEnteredBy
               ,svcMM.LastChgBy
               ,svcMM.LastChgDate
            };

            await _dataAccess.SaveData("dbo.TSvcDrMudMotor_UpdateEndPlay", e, "DefaultConnection");
        }

        public async Task TSvcDrMudMotor_UpdateFreeRot(TSvcDrMudMotorModel svcMM)
        {
            var fr = new
            {
                svcMM.TSvcDrMudMotorId
               ,svcMM.FreeRotation
               ,svcMM.LastChgBy
               ,svcMM.LastChgDate
            };

            await _dataAccess.SaveData("dbo.TSvcDrMudMotor_UpdateFreeRot", fr, "DefaultConnection");
        }

        public async Task TSvcDrMudMotor_UpdateBacklash(TSvcDrMudMotorModel svcMM)
        {
            var b = new
            {
                svcMM.TSvcDrMudMotorId
               ,svcMM.BacklashKeys
               ,svcMM.LastChgBy
               ,svcMM.LastChgDate
            };

            await _dataAccess.SaveData("dbo.TSvcDrMudMotor_UpdateBacklash", b, "DefaultConnection");
        }

        public async Task TSvcDrMudMotor_UpdateFloatSize(TSvcDrMudMotorModel svcMM)
        {
            var fs = new
            {
                svcMM.TSvcDrMudMotorId
               ,svcMM.Float
               ,svcMM.FloatSize
               ,svcMM.LastChgBy
               ,svcMM.LastChgDate
            };

            await _dataAccess.SaveData("dbo.TSvcDrMudMotor_UpdateFloatSize", fs, "DefaultConnection");
        }

        public async Task TSvcDrMudMotor_UpdateMotorDegree(TSvcDrMudMotorModel svcMM)
        {
            var md = new
            {
                svcMM.TSvcDrMudMotorId
               ,svcMM.MotorDegreeType
               ,svcMM.MotorDegreeSetting
               ,svcMM.MotorDegreeEnteredBy
               ,svcMM.MotorDegreeEnteredDate
            };

            await _dataAccess.SaveData("dbo.TSvcDrMudMotor_UpdateMotorDegree", md, "DefaultConnection");
        }

        public async Task TSvcDrMudMotor_UpdateWearPad(TSvcDrMudMotorModel svcMM)
        {
            var wp = new
            {
                svcMM.TSvcDrMudMotorId
               ,svcMM.WearPadSize
               ,svcMM.WearPadEnteredBy
               ,svcMM.WearPadDate
            };

            await _dataAccess.SaveData("dbo.TSvcDrMudMotor_UpdateWearPad", wp, "DefaultConnection");
        }

        public async Task TSvcBuild_UpdateWO(TSvcBuildDisplayModel svcB)
        {
            var b = new
            {
                svcB.TSvcBuildId
               ,svcB.ActualWO
               ,svcB.LastChgBy
               ,svcB.LastChgDate
            };

            await _dataAccess.SaveData("dbo.TSvcBuild_UpdateWO", b, "DefaultConnection");
        }

        public async Task TSvcCond_UpdateCondition(TSvcCondDisplayModel svcI)
        {
            var i = new
            {
                svcI.TSvcCondId
               ,svcI.Condition
               ,svcI.EnteredBy
               ,svcI.EnteredDate
               ,svcI.LastChgBy
               ,svcI.LastChgDate
            };

            await _dataAccess.SaveData("dbo.TSvcCond_UpdateCondition", i, "DefaultConnection");
        }

        public async Task TSvcCond_UpdateComment(TSvcCondDisplayModel svcI)
        {
            var i = new
            {
                svcI.TSvcCondId
               ,svcI.Comment
               ,svcI.EnteredBy
               ,svcI.EnteredDate
               ,svcI.LastChgBy
               ,svcI.LastChgDate
            };

            await _dataAccess.SaveData("dbo.TSvcCond_UpdateComment", i, "DefaultConnection");
        }

        public async Task TSvcMeas_UpdateDec(TSvcMeasDisplayModel meas)
        {
            var m = new
            {
                meas.TSvcMeasId
               ,meas.DecVal
               ,meas.EnteredBy
               ,meas.EnteredDate
               ,meas.LastChgBy
               ,meas.LastChgDate
            };

            await _dataAccess.SaveData("dbo.TSvcMeas_UpdateDec", m, "DefaultConnection");
        }

        public async Task TSvcMeas_UpdateInt(TSvcMeasDisplayModel meas)
        {
            var m = new
            {
                meas.TSvcMeasId
               ,meas.IntVal
               ,meas.EnteredBy
               ,meas.EnteredDate
               ,meas.LastChgBy
               ,meas.LastChgDate
            };

            await _dataAccess.SaveData("dbo.TSvcMeas_UpdateInt", m, "DefaultConnection");
        }

        public async Task<int> TSvcBuild_Insert(TSvcBuildDisplayModel tSvcBuild)
        {
            var b = new
            {
                tSvcBuild.ToolSvcId
               ,tSvcBuild.ToolPartId
               ,tSvcBuild.BuildLineId
               ,tSvcBuild.ComponentId
               ,tSvcBuild.PartNum
               ,tSvcBuild.BuildWO
               ,tSvcBuild.ActualWO
               ,tSvcBuild.EnteredBy
               ,tSvcBuild.EnteredDate
               ,tSvcBuild.LastChgBy
               ,tSvcBuild.LastChgDate
            };

            var _id = await _dataAccess.InsertData("dbo.TSvcBuild_Insert", b, "DefaultConnection");

            return _id;
        }


        public async Task<int> TSvcCond_Insert(TSvcCondDisplayModel tSvcCond)
        {
            var i = new
            {
                tSvcCond.ToolSvcId
               ,tSvcCond.ToolPartId
               ,tSvcCond.PartName
               ,tSvcCond.Condition
               ,tSvcCond.Comment
               ,tSvcCond.EnteredBy
               ,tSvcCond.EnteredDate
               ,tSvcCond.LastChgBy
               ,tSvcCond.LastChgDate
            };

            var _id = await _dataAccess.InsertData("dbo.TSvcCond_Insert", i, "DefaultConnection");

            return _id;
        }

        public async Task<int> TSvcMeas_Insert(TSvcMeasDisplayModel tSvcMeas)
        {
            var m = new
            {
                tSvcMeas.ToolSvcId
               ,tSvcMeas.MeasurementId
               ,tSvcMeas.DecVal
               ,tSvcMeas.IntVal
               ,tSvcMeas.EnteredBy
               ,tSvcMeas.EnteredDate
               ,tSvcMeas.LastChgBy
               ,tSvcMeas.LastChgDate
            };

            var _id = await _dataAccess.InsertData("dbo.TSvcMeas_Insert", m, "DefaultConnection");

            return _id;
        }

        public async Task<int> TSvcDrMudMotor_Insert(TSvcDrMudMotorModel tSvcDrMudMotor)
        {
            var s = new
            {
                tSvcDrMudMotor.ToolSvcId
               ,tSvcDrMudMotor.TopConn
               ,tSvcDrMudMotor.Stabilizer1
               ,tSvcDrMudMotor.Stabilizer2
               ,tSvcDrMudMotor.BottomConn
               ,tSvcDrMudMotor.RnD
               ,tSvcDrMudMotor.RnDReason
               ,tSvcDrMudMotor.EndPlay
               ,tSvcDrMudMotor.EndPlayEnteredBy
               ,tSvcDrMudMotor.FreeRotation
               ,tSvcDrMudMotor.Lobe
               ,tSvcDrMudMotor.Stage
               ,tSvcDrMudMotor.MotorDegreeType
               ,tSvcDrMudMotor.MotorDegreeSetting
               ,tSvcDrMudMotor.MotorDegreeEnteredDate
               ,tSvcDrMudMotor.MotorDegreeEnteredBy
               ,tSvcDrMudMotor.WearPadSize
               ,tSvcDrMudMotor.WearPadDate
               ,tSvcDrMudMotor.WearPadEnteredBy
               ,tSvcDrMudMotor.BacklashKeys
               ,tSvcDrMudMotor.Float
               ,tSvcDrMudMotor.FloatSize
               ,tSvcDrMudMotor.CameraUsed
               ,tSvcDrMudMotor.CameraUsedEnteredBy
               ,tSvcDrMudMotor.DisassemblyBy
               ,tSvcDrMudMotor.DisassemblyDate
               ,tSvcDrMudMotor.Comments
               ,tSvcDrMudMotor.SupervisorApproved
               ,tSvcDrMudMotor.SupervisorApprovedDate
            };

            var _id = await _dataAccess.InsertData("dbo.TSvcDrMudMotor_Insert", s, "DefaultConnection");

            return _id;
        }




        public async Task<List<string>> AspNetUsers_GetEmails()
        {
            var _ue = await _dataAccess.LoadData<string, dynamic>("dbo.AspNetUsers_GetEmails",
                                                                                   new { },
                                                                                   "DefaultConnection");
            return _ue.ToList();
        }


        public async Task<List<IdentityUserModel>> AspNetUsers_GetAll()
        {
            var _iu = await _dataAccess.LoadData<IdentityUserModel, dynamic>("dbo.AspNetUsers_GetAll",
                                                                                   new { },
                                                                                   "DefaultConnection");
            return _iu.ToList();
        }



    }
}
