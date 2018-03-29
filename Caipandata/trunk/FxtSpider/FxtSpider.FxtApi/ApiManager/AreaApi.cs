using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtSpider.DAL.LinqToSql;
using FxtSpider.FxtApi.Fxt.Api;
using FxtSpider.FxtApi.Model;
using FxtSpider.Dll.Manager;
using FxtSpider.FxtApi.Common;
using FxtSpider.Common;
using log4net;
using FxtSpider.FxtApi.FxtApiClientManager;
using Newtonsoft.Json.Linq;

namespace FxtSpider.FxtApi.ApiManager
{
    /// <summary>
    /// 行政区api
    /// </summary>
    public static class AreaApi
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(AreaApi));
        /// <summary>
        /// 根据城市名称获取行政区列表
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        public static List<FxtApi_SYSArea> GetAreaByCityName(string cityName, FxtAPIClientExtend _fxtApi = null)
        {
            List<FxtApi_SYSArea> list = new List<FxtApi_SYSArea>();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {

                string name = "GetAreaByCityName";
                var para = new { cityName = cityName};
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));                
                if (string.IsNullOrEmpty(jsonStr))
                {
                    return new List<FxtApi_SYSArea>();
                }
                list = FxtApi_SYSArea.ConvertToObjList(jsonStr);
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("GetAreaByCityName(cityName:{0})", cityName == null ? "null" : cityName), ex);
                fxtApi.Abort();
            }
            return list;
        }
        /// <summary>
        /// 根据城市id获取行政区列表
        /// </summary>
        /// <param name="fxtCityId"></param>
        /// <returns></returns>
        public static List<FxtApi_SYSArea> GetAreaByCityId(int fxtCityId, FxtAPIClientExtend _fxtApi = null)
        {
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            List<FxtApi_SYSArea> list = new List<FxtApi_SYSArea>();
            try
            {

                string name = "GetArea";
                var para = new { cityId = fxtCityId };
                string jsonStr = Convert.ToString(EntranceApi.Entrance(name, para.ToJSONjss(), _fxtApi: fxtApi));   
                FxtApi_PublicResult result = FxtApi_PublicResult.ConvertToObj(jsonStr);
                if (result == null || string.IsNullOrEmpty(Convert.ToString(result.data)))
                {
                    fxtApi.Abort();
                    return new List<FxtApi_SYSArea>();
                }
                list = FxtApi_SYSArea.ConvertToObjList(Convert.ToString(result.data));
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetAreaByCityId(cityName:{0})", fxtCityId), ex);
            }
            return list;
        }

        public static List<FxtApi_SYSArea> GetAreaByAreaIds(int[] areaIds, FxtAPIClientExtend _fxtApi = null)
        {
            string areaIdsStr = areaIds.ConvertToString();
            List<FxtApi_SYSArea> list = new List<FxtApi_SYSArea>();
            if (string.IsNullOrEmpty(areaIdsStr))
            {
                return list;
            }
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                JObject jObjPara = new JObject();
                jObjPara.Add(new JProperty("areaIds", areaIdsStr));
                string methodName = "GetSYSAreaByAreaIds";
                string jsonStr = Convert.ToString(EntranceApi.Entrance(methodName, jObjPara.ToJSONjss(), _fxtApi: fxtApi));
                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return new List<FxtApi_SYSArea>();
                }
                list = FxtApi_SYSArea.ConvertToObjList(jsonStr);
                list = list.DecodeField();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetAreaByAreaIds(int[] areaIds, FxtAPIClientExtend _fxtApi = null),areaIds={0}",
                    areaIdsStr==null?"null":areaIdsStr), ex);
            }
            return list;
        }

    }
}
