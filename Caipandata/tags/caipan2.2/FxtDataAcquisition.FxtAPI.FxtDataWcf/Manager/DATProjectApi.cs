using FxtDataAcquisition.NHibernate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtDataAcquisition.Common;
using Newtonsoft.Json.Linq;
using log4net;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;

namespace FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager
{
    public static class DATProjectApi
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(DATProjectApi));
        /// <summary>
        /// 根据楼盘ID获取楼盘详细信息,(根据查勘权限和字表)
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="_lnkpaList"></param>
        /// <param name="_lnkpcList"></param>
        /// <param name="phCount"></param>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static DATProject GetProjectDetailByProjectIdAndCityIdAndCompanyId(int projectId, int cityId, int companyId,
            out IList<LNKPAppendage> _lnkpaList,out IList<LNKPCompany> _lnkpcList,out int phCount, FxtAPIClientExtend _fxtApi = null)
        {
            _lnkpaList = new List<LNKPAppendage>();
            _lnkpcList = new List<LNKPCompany>();
            phCount = 0;
            DATProject proj = null;
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetProjectDetailByProjectIdAndCityIdAndCompanyId";

                var para = new { projectId=projectId, cityId=cityId, companyId=companyId};
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));
                //log.Info(string.Format("获取正式库楼盘信息（wcf）：fxtcompanyid：{0},fxtprojectid:{1},cityid:{2},jsonstri:{3}", companyId, projectId, cityId,jsonStr));
                FxtApi_PublicResult result = jsonStr.ParseJSONjss<FxtApi_PublicResult>();
                if (result == null || string.IsNullOrEmpty(Convert.ToString(result.data)))
                {
                    fxtApi.Abort();
                    return proj;
                }
                JObject jobj = JObject.Parse(Convert.ToString(result.data));
                proj = jobj.Value<string>("project").ParseJSONjss<DATProject>();
                _lnkpaList = jobj.Value<string>("appendage").ParseJSONList<LNKPAppendage>();
                phCount = jobj.Value<int>("phcount");
                _lnkpcList = jobj.Value<string>("pcompany").ParseJSONList<LNKPCompany>();

                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error("GetProjectDetailByProjectIdAndCityIdAndCompanyId()", ex);
            }
            return proj;
        }
    }
}
