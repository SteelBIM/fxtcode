using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using System.Data;

namespace FXT.DataCenter.Domain.Services
{
    /// <summary>
    /// 商业房号接口
    /// </summary>
    public interface IDat_House_Biz
    {
        /// <summary>
        /// 获取商业房号列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IQueryable<Dat_House_Biz> GetDat_House_BizList(Dat_House_Biz model, bool self = true);
        /// <summary>
        /// 获取商业房号信息
        /// </summary>
        /// <param name="houseId"></param>
        /// <returns></returns>
        Dat_House_Biz GetDat_House_BizById(int houseId, int CityId, int FxtCompanyId);
        /// <summary>
        /// 新增商业房号
        /// </summary>
        /// <param name="modal">商业房号model</param>
        /// <returns></returns>
        int AddDat_House_Biz(Dat_House_Biz model);

        /// <summary>
        /// 修改商业房号
        /// </summary>
        /// <param name="modal">商业房号model</param>
        /// <returns></returns>
        int UpdateDat_House_Biz(Dat_House_Biz model, int currFxtCompanyId);

        /// <summary>
        /// 删除商业房号
        /// </summary>
        /// <param name="houseId">商业房号houseId</param>
        /// <returns></returns>
        bool DeleteDat_House_Biz(int houseId, string userName, int cityId, int fxtCompanyId, int poductTypeCode, int currFxtCompanyId);

        /// <summary>
        /// 获取房号列表
        /// </summary>
        /// <param name="floorId">楼层ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <returns></returns>
        IQueryable<Dat_House_Biz> GetDat_House_BizByFloorId(long floorId, int cityId, int fxtCompanyId);
        /// <summary>
        /// 获取房号列表
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="floorId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        IQueryable<Dat_House_Biz> GetHouseList(int buildingId, int floorId, int cityId, int fxtCompanyId);
        /// <summary>
        /// 房号名称是否存在
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="floorId"></param>
        /// <param name="houseName"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        bool IsExistHouseName(long buildingId, long floorId, string houseName, int cityId, int fxtCompanyId);

        /// <summary>
        /// 获取房号数量
        /// </summary>
        /// <param name="projectId">商业街Id</param>
        /// <param name="buildingId">楼栋数量</param>
        /// <param name="floorId">楼层号</param>
        /// <returns></returns>
        int GetHouseCount(int projectId, int buildingId, int floorId, int cityId, int fxtcompanyId);

        /// <summary>
        /// 获取房号ID
        /// </summary>
        /// <param name="buildingId">楼栋ID</param>
        /// <param name="houseName">房号名</param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        long GetHouseId(long buildingId, string houseName, int cityId, int fxtCompanyId);

        DataTable HouseSelfDefineExport(Dat_House_Biz houseBiz, List<string> houseAttr, int CityId, int FxtCompanyId, bool self = true);

        long IsExistHouseId(long buildingId, long floorId, string houseName, int cityId, int fxtCompanyId);
    }
}
