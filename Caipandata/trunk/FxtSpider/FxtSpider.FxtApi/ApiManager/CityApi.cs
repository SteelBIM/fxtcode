using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.FxtApi.Model;
using FxtSpider.FxtApi.Fxt.Api;
using Newtonsoft.Json;
using FxtSpider.Common;
using log4net;
using FxtSpider.FxtApi.FxtApiClientManager;

namespace FxtSpider.FxtApi.ApiManager
{
    public static class CityApi
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(CityApi));
        /// <summary>
        /// 根据fxt省份ID获取城市信息
        /// </summary>
        /// <param name="fxtProvinceId"></param>
        /// <returns></returns>
        public static List<FxtApi_SYSCity> GetCityByProvinceId(int fxtProvinceId, FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_SYSCity> list = new List<FxtApi_SYSCity>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {

                string name = "GetCity";
                var para = new { provinceId = fxtProvinceId };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));   
                FxtApi_PublicResult result = FxtApi_PublicResult.ConvertToObj(jsonStr);
                if (result == null || string.IsNullOrEmpty(Convert.ToString(result.data)))
                {
                    fxtApi.Abort();
                    return new List<FxtApi_SYSCity>();
                }

                list = FxtApi_SYSCity.ConvertToObjList(Convert.ToString(result.data));
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format(" GetCityByProvinceId(int provinceId:{0})", fxtProvinceId), ex);
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
                city = FxtApi_SYSCity.ConvertToObj(jsonStr);
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetCityByCityName(string cityName:{0})", cityName == null ? "null" : ""), ex);
            }
            return city;
        }
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
                list = FxtApi_SYSCity.ConvertToObjList(jsonStr);
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error("GetAllCity()", ex);
            }
            return list;
        }
    }
}
