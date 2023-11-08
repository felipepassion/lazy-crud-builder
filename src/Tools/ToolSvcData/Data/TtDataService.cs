using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolSvcData.Access;
using ToolSvcData.Models;

namespace ToolSvcData.Data
{
    public class TtDataService
    {
        private readonly ISqlDataAccess _dataAccess;

        public TtDataService(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<ToolModel> Tool_GetJobNum(string serialNum)
        {
            var _t = await _dataAccess.LoadData<ToolModel, dynamic>("dbo.Tool_GetJobNum",
                                                                                   new { SerialNum = serialNum },
                                                                                   "DefaultConnection");
            return _t.FirstOrDefault();
        }

        public async Task<BuildModel> Build_GetByUsageNum(int usageNum)
        {
            var _b = await _dataAccess.LoadData<BuildModel, dynamic>("dbo.Build_GetByUsageNum",
                                                                                   new { UsageNum = usageNum },
                                                                                   "DefaultConnection");
            return _b.FirstOrDefault();
        }

        public async Task<List<BuildLineModel>> BuildLine_GetByBuildId(int buildId)
        {
            var _bl = await _dataAccess.LoadData<BuildLineModel, dynamic>("dbo.BuildLine_GetByBuildId",
                                                                                   new
                                                                                   {
                                                                                       BuildId = buildId                                                                                       
                                                                                   },
                                                                                   "DefaultConnection");
            return _bl.ToList();
        }


    }
}
