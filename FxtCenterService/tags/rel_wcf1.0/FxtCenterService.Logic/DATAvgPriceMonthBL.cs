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
    }
}
