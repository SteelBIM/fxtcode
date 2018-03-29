using System;
using System.Linq;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface ICityAvgPrice
    {
        /// <summary>
        /// 获取城市均价列表
        /// </summary>
        /// <param name="caseDateFrom">开始时间</param>
        /// <param name="caseDateTo">结束时间</param>
        /// <param name="areaId">行政区Id</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalCount"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        IQueryable<DAT_AvgPrice_Month> GetCityAvgPrices(DateTime caseDateFrom, DateTime caseDateTo, int areaId, int cityId, int pageSize, int pageIndex, out int totalCount);

        /// <summary>
        /// 根据ID获取单条城市均价
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns> 
        DAT_AvgPrice_Month GetCityAvgPrice(int id);

        /// <summary>
        /// 获取城市均价
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="AvgPriceDate"></param>
        /// <returns></returns>
        DAT_AvgPrice_Month IsExistCityAvgPrice(int cityId, int areaId, DateTime AvgPriceDate);

        /// <summary>
        /// 增加城市均价
        /// </summary>
        /// <param name="avgPrice"></param>
        /// <returns></returns>
        int AddCityAvgPrice(DAT_AvgPrice_Month avgPrice);

        /// <summary>
        /// 修改城市均价
        /// </summary>
        /// <param name="avgPrice"></param>
        /// <returns></returns>
        int UpdateAvgPrice(DAT_AvgPrice_Month avgPrice);

        /// <summary>
        /// 删除城市均价
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteAvgPrice(int id);

        /// <summary>
        /// 获取均价
        /// </summary>
        /// <returns></returns>
        DAT_AvgPrice_Month GetAvgPrice(DAT_AvgPrice_Month avgPrice);


    }
}
