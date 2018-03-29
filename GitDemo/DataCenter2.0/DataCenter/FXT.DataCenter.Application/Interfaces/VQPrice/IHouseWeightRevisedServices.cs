using System.Linq;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Application.Interfaces.VQPrice
{
    public interface IHouseWeightRevisedServices
    {
        /// <summary>
        /// 获取房号修正系数
        /// </summary>
        /// <param name="datWeightHouse"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        IQueryable<DatWeightHouse> GetWeightHouses(int ProjectId, int BuildingId, string HouseName, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, int type, int pageIndex, int pageSize, out int totalCount, bool self = true);

        /// <summary>
        /// 获取单个房号修正系数对象
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="houseId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        DatWeightHouse GetWeightHouse(int projectId, int buildingId, int houseId, int cityId, int ParentShowDataCompanyId, int ParentProductTypeCode);

        /// <summary>
        /// 修改房号修正系数
        /// </summary>
        /// <param name="datWeightHouse"></param>
        /// <returns></returns>
        int UpdateWeightHouse(DatWeightHouse datWeightHouse);

        /// <summary>
        /// 增加房号修正系数
        /// </summary>
        /// <param name="datWeightHouse"></param>
        /// <returns></returns>
        int AddWeightHouse(DatWeightHouse datWeightHouse);

        #region 非业务接口

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
