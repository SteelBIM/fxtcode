using CAS.Entity.FxtProject;
using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.Logic
{
    /// <summary>
    /// 城市均价
    /// </summary>
    public class DATAvgPriceBL
    {
        /// <summary>
        /// 城市均价列表
        /// </summary>
        public static List<DatAvgPricePush> GetAvgPriceList(DateTime? time)
        {
            return DATAvgPriceDA.GetAvgPriceList(time);
        }

        /// <summary>
        /// 获取最后一个月城市均价列表
        /// </summary>
        public static List<DatAvgPricePush> GetAvgPriceLastMonthList()
        {
            return DATAvgPriceDA.GetAvgPriceLastMonthList();
        }
    }
}
