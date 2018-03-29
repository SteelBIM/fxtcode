using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    /// <summary>
    /// 楼层接口
    /// </summary>
    public interface IDat_Floor_Biz
    {
        /// <summary>
        /// 获取楼层列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        IQueryable<Dat_Floor_Biz> GetDat_Floor_BizList(Dat_Floor_Biz model, bool self = true);
        /// <summary>
        /// 获取楼层信息
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        Dat_Floor_Biz GetDat_Floor_BizById(int floorId, int CityId, int FxtCompanyId);
        /// <summary>
        /// 新增楼层
        /// </summary>
        /// <param name="modal">楼层model</param>
        /// <returns></returns>
        int AddDat_Floor_Biz(Dat_Floor_Biz model);

        /// <summary>
        /// 修改楼层
        /// </summary>
        /// <param name="modal">楼层model</param>
        /// <returns></returns>
        int UpdateDat_Floor_Biz(Dat_Floor_Biz model, int currFxtCompanyId);

        /// <summary>
        /// 删除楼层
        /// </summary>
        /// <param name="floorId">楼层floorId</param>
        /// <returns></returns>
        bool DeleteDat_Floor_Biz(int floorId, string userName, int cityId, int fxtCompanyId, int poductTypeCode, int currFxtCompanyId);

        /// <summary>
        /// 根据楼栋id获取楼层集合
        /// </summary>
        /// <param name="buildId">楼栋ID</param>
        /// <param name="cityId">城市ID</param>
        /// <param name="fxtCompanyId">评估机构ID</param>
        /// <returns></returns>
        IQueryable<Dat_Floor_Biz> GetDat_Floor_BizByBuildId(int buildId, int cityId, int fxtCompanyId);

        /// <summary>
        /// 验证物理层、实际层唯一性
        /// 刘晓博
        /// 2014-09-11
        /// </summary>
        /// <param name="floor">楼层(物理层、实际层)</param>
        /// <param name="dataAttr">属性(物理层:FloorNo、实际层:FloorNum)</param>
        /// <returns></returns>
        bool ValidFloor(string floor, string dataAttr, string buildingId, int valid = 1);
        /// <summary>
        /// form提交验证
        /// </summary>
        /// <param name="floorNo"></param>
        /// <param name="floorNum"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        Dat_Floor_Biz FormValidFloor(string floorNo, string buildingId, int cityId, int fxtCompanyId);

        /// <summary>
        /// 获取楼层Id
        /// </summary>
        /// <param name="buildingId">楼栋Id</param>
        /// <param name="floorName">物理层</param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        long GetFloorIdByName(long buildingId, string floorName, int cityId, int fxtCompanyId);

        /// <summary>
        /// 获取楼层数量
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtcompanyId"></param>
        /// <returns></returns>
        int GetFloorCount(int projectId, int buildingId, int cityId, int fxtcompanyId);

        long IsExistFloor(string floor, string dataAttr, string buildingId);
    }
}
