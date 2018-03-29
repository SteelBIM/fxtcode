using System;
using System.Collections.Generic;
using System.Linq;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Application.Interfaces.House
{
    public interface ICityAvgPriceServices
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


        #region 非业务接口

        /// <summary>
        /// 获取行政区名称列表
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        IList<CompanyProduct_Module> GetAreaName(int cityId);

        /// <summary>
        /// 写系统日志
        /// </summary>        
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="userName">用户名</param>
        /// <param name="logType">对象类型7002006（楼盘、楼栋、房号、住宅案例）</param>
        /// <param name="eventType">操作类型7001001(新增、修改、删除)</param>
        /// <param name="objectId">对象ID</param>
        /// <param name="objectName">对象名称</param>
        /// <param name="remarks">操作描述</param>
        /// <param name="webIp">IP地址</param>
        /// <returns></returns>
        int InsertLog(int cityId, int fxtCompanyId, string userId, string userName, int logType, int eventType, string objectId, string objectName, string remarks, string webIp);

        #endregion
    }
}
