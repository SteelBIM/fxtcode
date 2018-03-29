using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;

namespace FxtCenterService.Logic
{
    public class SYSCityBL
    {
        /// <summary>
        /// 获取cityid设置获取案例月份数
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        public static int GetCityCaseMonth(int cityid)
        {
            return SYSCityDA.GetCityCaseMonth(cityid);
        }
        /// <summary>
        /// 获取城市列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="provinceid">省份ID</param>
        /// <returns></returns>
        public static List<SYSCity> GetSYSCityList(SearchBase search, int provinceid)
        {
            return SYSCityDA.GetSYSCityList(search, provinceid);
        }

        /// <summary>
        /// 获取城市信息
        /// </summary>
        /// <param name="search"></param>
        /// <param name="provinceid">省份ID</param>
        /// <returns></returns>
        public static string GetSYSCityInfo(SearchBase search, int cityid, string cityname)
        {
            DataSet ds = SYSCityDA.GetSYSCityInfo(search, cityid, cityname);
            return ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson();
        }

        /// <summary>
        /// 获取行政区
        /// </summary>
        /// <param name="search"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public static string GetSYSAreaInfo(SearchBase search, int areaid)
        {
            DataSet ds = SYSCityDA.GetSYSAreaInfo(search, areaid);
            return ds == null || ds.Tables.Count <= 0 ? "" : ds.Tables[0].ToJson();
        }
    }
}
