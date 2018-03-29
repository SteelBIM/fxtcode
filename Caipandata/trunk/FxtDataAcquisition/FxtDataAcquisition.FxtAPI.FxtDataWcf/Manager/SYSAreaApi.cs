using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtDataAcquisition.Common;
using log4net;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;

namespace FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager
{
    public class SYSAreaApi
    {
        public static readonly ILog log = LogManager.GetLogger(typeof(SYSAreaApi));
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
                FxtApi_PublicResult result = jsonStr.ParseJSONjss<FxtApi_PublicResult>();
                if (result == null || string.IsNullOrEmpty(Convert.ToString(result.data)))
                {
                    fxtApi.Abort();
                    return new List<FxtApi_SYSArea>();
                }
                list = Convert.ToString(result.data).ParseJSONList<FxtApi_SYSArea>();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetAreaByCityId(cityName:{0})", fxtCityId), ex);
            }
            return list;
        }

        /// <summary>
        /// 根据多个行政区ID获取多个行政区信息
        /// </summary>
        /// <param name="areaIds"></param>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
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
                var jObjPara = new { areaIds = areaIdsStr };
                string methodName = "GetSYSAreaByAreaIds";
                string jsonStr = Convert.ToString(EntranceApi.Entrance(methodName, jObjPara.ToJSONjss(), _fxtApi: fxtApi));
                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return new List<FxtApi_SYSArea>();
                }
                list = jsonStr.ParseJSONList<FxtApi_SYSArea>(); ;
                list = list.DecodeField();
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetAreaByAreaIds(int[] areaIds, FxtAPIClientExtend _fxtApi = null),areaIds={0}",
                    areaIdsStr == null ? "null" : areaIdsStr), ex);
            }
            return list;
        }

        /// <summary>
        /// 根据行政区ID获取行政区信息
        /// </summary>
        /// <param name="areaId"></param>
        /// <param name="_fxtApi"></param>
        /// <returns></returns>
        public static FxtApi_SYSArea GetAreaByAreaId(int areaId, FxtAPIClientExtend _fxtApi = null)
        {
            FxtApi_SYSArea obj = new FxtApi_SYSArea();
            FxtAPIClientExtend fxtApi = new FxtAPIClientExtend(_fxtApi);
            try
            {
                var jObjPara = new { areaId = areaId };
                string methodName = "GetAreaByAreaId";
                string jsonStr = Convert.ToString(EntranceApi.Entrance(methodName, jObjPara.ToJSONjss(), _fxtApi: fxtApi));
                if (string.IsNullOrEmpty(jsonStr))
                {
                    fxtApi.Abort();
                    return null;
                }
                obj = jsonStr.ParseJSONjss<FxtApi_SYSArea>(); 
                fxtApi.Abort();
            }
            catch (Exception ex)
            {
                fxtApi.Abort();
                log.Error(string.Format("GetAreaByAreaId(),areaId={0}",
                    areaId), ex);
            }
            return obj;
        }
    }
}
