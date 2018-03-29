using FxtDataAcquisition.NHibernate.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using FxtDataAcquisition.Domain.DTO.FxtDataCenterDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using System.Web.Caching;

namespace FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager
{
    public static class DataCenterAreaApi
    {
        /// <summary>
        /// 获取行政区列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="username"></param>
        /// <param name="signname"></param>
        /// <param name="appList"></param>
        /// <returns></returns>
        public static List<FxtApi_SYSArea> GetAreaByCityId(int cityId, string username, string signname, List<UserCenter_Apps> appList)
        {
            List<FxtApi_SYSArea> areaList = CacheHelper.GetCache("areaList") as List<FxtApi_SYSArea>;
            if (areaList == null)
            {
                CAS.Common.MVC4.LogHelper.Info("写入行政区缓存");
                //var para = new { cityid = cityId };
                var para = new { };
                DataCenterResult result = Common.PostDataCenter(username, signname, Common.garealist, para, appList);
                List<FxtApi_SYSArea> list = new List<FxtApi_SYSArea>();
                if (!string.IsNullOrEmpty(result.data))
                {
                    areaList = result.data.ParseJSONList<FxtApi_SYSArea>();
                }
                HttpHelper.CurrentCache.Insert("areaList", areaList, null, Cache.NoAbsoluteExpiration, DateTime.Now.AddDays(1) - DateTime.Now);
                areaList = areaList.Where(m => m.CityId == cityId).ToList();
            }
            else
            {
                CAS.Common.MVC4.LogHelper.Info("读取行政区缓存");
                areaList = areaList.Where(m => m.CityId == cityId).ToList();
            }

            return areaList;
        }

        /// <summary>
        /// 获取片区列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="username"></param>
        /// <param name="signname"></param>
        /// <param name="appList"></param>
        /// <returns></returns>
        public static List<FxtApi_SYSSubArea> GetSubAreaByAreaId(int areaId, string username, string signname, List<UserCenter_Apps> appList)
        {
            var para = new { areaid = areaId };
            DataCenterResult result = Common.PostDataCenter(username, signname, Common.gsubarealist, para, appList);
            List<FxtApi_SYSSubArea> list = new List<FxtApi_SYSSubArea>();
            if (!string.IsNullOrEmpty(result.data))
            {
                list = result.data.ParseJSONList<FxtApi_SYSSubArea>();
            }
            return list;
        }
    }
}
