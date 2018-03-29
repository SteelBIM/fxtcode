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
    public class DATAvgPriceMonthBL
    {
        /// <summary>
        /// 价格走势
        /// </summary>
        /// <param name="search"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DataSet GetDATAvgPriceMonthList(SearchBase search, int projectid)
        {
            return DATAvgPriceMonthDA.GetDATAvgPriceMonthList(search, projectid); 
        }

         /// <summary>
        /// 获取城市，行政区均价走势（不区分类型）
        /// </summary>
        /// <param name="topcnt"></param>
        /// <param name="cityid"></param>
        /// <param name="areaid"></param>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static DataSet GetCityAreaAvgPriceTrend(int topcnt, int cityid, int areaid, DateTime startdate, DateTime enddate)
        {
            return DATAvgPriceMonthDA.GetCityAreaAvgPriceTrend(topcnt, cityid, areaid, startdate, enddate); 
        }
        /// <summary>
        /// 获取行政区均价（不区分类型）
        /// 创建人:曾智磊,2014-08-05
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="months">月份，例如：2014-08</param>
        /// <returns></returns>
        public static decimal GetAreaAvgPriceTrend(int cityId, int areaId, string months)
        {
            decimal price = 0;
            if (string.IsNullOrEmpty(months))
            {
                return price;
            }
            int topcnt = 1;
            DateTime startdate = Convert.ToDateTime(Convert.ToDateTime(months).ToString("yyyy-MM-01"));
            DateTime enddate = Convert.ToDateTime(startdate.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"));
            DataSet ds = DATAvgPriceMonthDA.GetAreaAvgPriceTrend(topcnt, cityId, areaId, startdate, enddate);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                price = StringHelper.TryGetDecimal(ds.Tables[0].Rows[0]["AvgPrice"].ToString());
            }
            return price;
        }
    }
}
