using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using System.Data;

namespace FXT.DataCenter.Domain.Services
{
    /// <summary>
    /// 房号接口
    /// </summary>
    public interface IDAT_House
    {
        /// <summary>
        /// 新增房号
        /// </summary>
        /// <param name="h_item"></param>
        /// <returns></returns>
        int AddHouse(DAT_House h_item);

        /// <summary>
        /// 获取房号列表
        /// </summary>
        /// <param name="BuildingId">楼栋ID</param>
        /// <param name="CityID">城市ID</param>
        /// <param name="FxtCompanyId">公司ID</param>
        /// <returns></returns>
        IQueryable<DAT_House> GetHouseInfoByBuild(int BuildingId, int CityID, int FxtCompanyId);

        /// <summary>
        /// 得到房号列表
        /// </summary>
        /// <param name="CityID"></param>
        /// <param name="FxtCompanyId"></param>
        /// <param name="BuildingId"></param>
        /// <returns></returns>
        DataSet GetHouseListByBuildingId(int CityID, int FxtCompanyId, int BuildingId);

        /// <summary>
        /// 获取房号名称列表
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        IQueryable<DAT_House> GetHouseNameList(int buildingId, int cityId, int fxtCompanyId);

        /// <summary>
        /// 更新房号
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int UpdateHouse(DAT_House item, int currFxtCompanyId);

        /// <summary>
        /// 更新房号
        /// </summary>
        /// <param name="item"></param>
        /// <param name="currFxtCompanyId"></param>
        /// <param name="modifiedProperty"></param>
        /// <returns></returns>
        int UpdateHouse4Excel(DAT_House item, int currFxtCompanyId, List<string> modifiedProperty);

        /// <summary>
        /// 获取房号数量
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtcompanyId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        int GetHouseCount(int cityId, int fxtcompanyId, int projectId);

        /// <summary>
        /// 验证房号是否存在
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="buildingId">楼栋ID</param>
        /// <param name="houseName">房号名称</param>
        /// <param name="unitNo">室号</param>
        /// <returns></returns>
        DAT_House ValidateHouseNo(int cityId, int fxtCompanyId, int buildingId, string floorNo, string unitNo);

        string GetProjectName(string buildId, int cityId, int fxtcompanyId);

        string GetBuildName(string buildId, int cityId, int fxtcompanyId);
        /// <summary>
        /// 房号新增
        /// 2014-11-18
        /// </summary>
        /// <param name="item"></param>
        int AddHouseEndity(DAT_House item);
        /// <summary>
        /// 房号更新
        /// 2014-11-18
        /// </summary>
        /// <param name="item"></param>
        //int UpdateHouseEndity(DAT_House item, int currfxtcompanyId);
        /// <summary>
        /// 房号删除
        /// 2014-11-18
        /// </summary>
        /// <param name="item"></param>
        int DeleteHouseEndity(DAT_House item, int currfxtcompanyId);

        /// <summary>
        /// 根据房号获取单条记录
        /// </summary>
        /// <param name="houseId"></param>
        /// <returns></returns>
        DAT_House GetHouseInfoById(int houseId, int fxtcompanyId, int cityId);

        IQueryable<DAT_House> GetUnitNo(int buildId, int cityId, int fxtCompanyId);

        int GetHouseId(int buildingId, string houseName, int cityId, int fxtCompanyId);

        // 设置房号系数差
        void SetHouseRatio(int cityid, int fxtcompanyid, string saveUserName, int key, int projectid, string buildingids, out int sussnum, out int count);

        // 设置房号VQ系数
        int SetVQHouseRatio(int cityid, int fxtcompanyid, string saveUserName, int projectid, int parentProductTypeCode);

        int UpdateHouseWeight(DAT_House house, int currFxtCompanyId);

        IQueryable<DAT_House> ExportHouseList(int projectid, int buildingid, int cityId, int fxtCompanyId);
    }
}
