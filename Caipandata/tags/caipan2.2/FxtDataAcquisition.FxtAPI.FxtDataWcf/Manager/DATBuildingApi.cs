using FxtDataAcquisition.NHibernate.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;

namespace FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager
{
    public static class DATBuildingApi
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(DATBuildingApi));
        /// <summary>
        /// 根据楼栋ID获取楼栋详细信息,(根据查勘权限和字表)
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static DATBuilding GetBuildingDetailByBuildingIdAndCityIdAndCompanyId(int buildingId, int cityId, int companyId, FxtAPIClientExtend _fxtApi = null)
        {
            DATBuilding obj = null;
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetBuildingDetailByBuildingIdAndCityIdAndCompanyId";

                var para = new { buildingId = buildingId, cityId = cityId, companyId = companyId };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

                FxtApi_PublicResult result = jsonStr.ParseJSONjss<FxtApi_PublicResult>();
                if (result == null || string.IsNullOrEmpty(Convert.ToString(result.data)))
                {
                    fxtApi.Abort();
                    return obj;
                }
                obj = Convert.ToString(result.data).ParseJSONjss<DATBuilding>();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error("GetBuildingDetailByBuildingIdAndCityIdAndCompanyId()", ex);
            }
            return obj;
        }
    }
}
