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
    public static class DATHouseApi
    {

        public static readonly ILog log = LogManager.GetLogger(typeof(DATHouseApi));

        /// <summary>
        /// 根据房号ID获取房号详细信息,(根据查勘权限和字表)
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static DATHouse GetHouseDetailByHouseIdAndCityIdAndCompanyId(int houseId, int cityId, int companyId, FxtAPIClientExtend _fxtApi = null)
        {
            DATHouse obj = null;
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetHouseDetailByHouseIdAndCityIdAndCompanyId";

                var para = new { houseId = houseId, cityId = cityId, companyId = companyId };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));

                FxtApi_PublicResult result = jsonStr.ParseJSONjss<FxtApi_PublicResult>();
                if (result == null || string.IsNullOrEmpty(Convert.ToString(result.data)))
                {
                    fxtApi.Abort();
                    return obj;
                }
                obj = Convert.ToString(result.data).ParseJSONjss<DATHouse>();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error("GetHouseDetailByHouseIdAndCityIdAndCompanyId()", ex);
            }
            return obj;
        }

    }
}
