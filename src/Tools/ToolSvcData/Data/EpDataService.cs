using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolSvcData.Access;
using ToolSvcData.Models;

namespace ToolSvcData.Data
{
    public class EpDataService
    {
        private readonly ISqlDataAccess _dataAccess;

        public EpDataService(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<JobProdModel> JobProd_GetByJobNum(string epJobNum)
        {
            var _job = await _dataAccess.LoadData<JobProdModel, dynamic>("dbo.JobProd_GetByJobNum",
                                                                                   new { EpJobNum = epJobNum },
                                                                                   "DefaultConnection");
            return _job.FirstOrDefault();
        }

        //public async Task<EpPartModel> GetEpPartById(string partNum)
        //{
        //    var _p = await _dataAccess.LoadData<EpPartModel, dynamic>("dbo.EpLk_GetEpPartById",
        //                                                                           new { PartNum = partNum },
        //                                                                           "DefaultConnection");
        //    return _p.FirstOrDefault();
        //}

        //public async Task<List<SalesOrderModel>> GetSalesOrderById(int orderNum)
        //{
        //    var _so = await _dataAccess.LoadData<SalesOrderModel, dynamic>("dbo.EpLk_GetSalesOrderById",
        //                                                                           new { OrderNum = orderNum },
        //                                                                           "DefaultConnection");
        //    return _so.ToList();
        //}

        //public async Task<List<SalesOrderDtlModel>> GetSoDtlById(int orderNum)
        //{
        //    var _soDtl = await _dataAccess.LoadData<SalesOrderDtlModel, dynamic>("dbo.EpLk_GetSoDtlById",
        //                                                                           new { OrderNum = orderNum },
        //                                                                           "DefaultConnection");
        //    return _soDtl.ToList();
        //}

        //public async Task<List<EpCustModel>> GetEpCusts(string? custName)
        //{
        //    var _ec = await _dataAccess.LoadData<EpCustModel, dynamic>("dbo.EpLk_GetEpCusts",
        //                                                                           new { CustName = custName },
        //                                                                           "DefaultConnection");
        //    return _ec.ToList();
        //}

        //public async Task<List<VendorModel>> GetEpVendors()
        //{
        //    var _v = await _dataAccess.LoadData<VendorModel, dynamic>("dbo.EpLk_GetEpVendors",
        //                                                                           new {},
        //                                                                           "DefaultConnection");
        //    return _v.ToList();
        //}

        //public async Task<List<VendorModel>> GetEpVendorsByGroupCode(string groupCode)
        //{
        //    var _v = await _dataAccess.LoadData<VendorModel, dynamic>("dbo.EpLk_GetEpVendorsByGroupCode",
        //                                                                           new { GroupCode = groupCode },
        //                                                                           "DefaultConnection");
        //    return _v.ToList();
        //}


        //public async Task<List<TaxRegionModel>> GetTaxRegionsAR()
        //{
        //    var _tr = await _dataAccess.LoadData<TaxRegionModel, dynamic>("dbo.EpLk_GetTaxRegionsAR",
        //                                                                           new { },
        //                                                                           "DefaultConnection");
        //    return _tr.ToList();
        //}

        //public async Task<List<TaxRegionModel>> GetTaxRegions3P()
        //{
        //    var _tr = await _dataAccess.LoadData<TaxRegionModel, dynamic>("dbo.EpLk_GetTaxRegions3P",
        //                                                                           new { },
        //                                                                           "DefaultConnection");
        //    return _tr.ToList();
        //}

        //public async Task<List<CodeDescModel>> GetTermsCodeAll()
        //{
        //    var _tc = await _dataAccess.LoadData<CodeDescModel, dynamic>("dbo.EpLk_GetTermsCodeAll",
        //                                                                           new { },
        //                                                                           "DefaultConnection");
        //    return _tc.ToList();
        //}

        //public async Task<List<CodeDescModel>> GetShipViaAll()
        //{
        //    var _sv = await _dataAccess.LoadData<CodeDescModel, dynamic>("dbo.EpLk_GetShipViaAll",
        //                                                                           new { },
        //                                                                           "DefaultConnection");
        //    return _sv.ToList();
        //}


        //public async Task<List<JobPartModel>> GetJobPartByJobNum(string jobNum)
        //{
        //    var _jp = await _dataAccess.LoadData<JobPartModel, dynamic>("dbo.EpLk_GetJobPartByJobNum",
        //                                                                           new { JobNum = jobNum },
        //                                                                           "DefaultConnection");
        //    return _jp.ToList();
        //}

        //public async Task<List<EpBuyerModel>> GetAllBuyers()
        //{
        //    var _b = await _dataAccess.LoadData<EpBuyerModel, dynamic>("dbo.EpLk_GetAllBuyers",
        //                                                                           new {  },
        //                                                                           "DefaultConnection");
        //    return _b.ToList();
        //}

        //public async Task<List<PartNumContextModel>> GetPartNumByContext(string context)
        //{
        //    var _pnc = await _dataAccess.LoadData<PartNumContextModel, dynamic>("dbo.EpLk_GetPartNumByContext",
        //                                                                           new { Context = context },
        //                                                                           "DefaultConnection");
        //    return _pnc.ToList();
        //}

        //public async Task<List<TaxRateModel>> GetTaxRatesByVendorNum(int vendorNum)
        //{
        //    var _tr = await _dataAccess.LoadData<TaxRateModel, dynamic>("dbo.EpLk_GetTaxRatesByVendorNum",
        //                                                                           new { VendorNum = vendorNum },
        //                                                                           "DefaultConnection");
        //    return _tr.ToList();
        //}
        //public async Task<List<TaxRateModel>> GetTaxRatesByRegionCode(string taxRegionCode)
        //{
        //    var _tr = await _dataAccess.LoadData<TaxRateModel, dynamic>("dbo.EpLk_GetTaxRatesByRegionCode",
        //                                                                           new { TaxRegionCode = taxRegionCode },
        //                                                                           "DefaultConnection");
        //    return _tr.ToList();
        //}
    }

}
