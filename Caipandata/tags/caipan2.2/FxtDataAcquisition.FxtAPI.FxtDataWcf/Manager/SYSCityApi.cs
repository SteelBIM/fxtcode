using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager
{
    public static class SYSCityApi
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(SYSCityApi));
        /// <summary>
        /// 获取所有城市
        /// </summary>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static List<FxtApi_SYSCity> GetAllCity(FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_SYSCity> list = new List<FxtApi_SYSCity>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                string name = "GetAllCity";
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, "", _fxtApi: fxtApi));
                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return null;
                }
                list = JsonHelp.ParseJSONList<FxtApi_SYSCity>(jsonStr);
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error("GetAllCity()", ex);
            }
            return list;
        }

        /// <summary>
        /// 根据城市名称获取城市信息
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static FxtApi_SYSCity GetCityByCityName(string cityName, FxtAPIClientExtend _fxtApi = null)
        {
            FxtApi_SYSCity city = null;
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {

                string name = "GetCityByCityName";
                var para = new { cityName = cityName };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));
                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return null;
                }
                city = jsonStr.ParseJSONjss<FxtApi_SYSCity>();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetCityByCityName(string cityName:{0})", cityName == null ? "null" : ""), ex);
            }
            return city;
        }
    }
}
