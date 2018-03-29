using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
   public interface IOfficeHouse
    {
        /// <summary>
        /// 获取房号数据列表
        /// </summary>
        /// <param name="buildingId">楼栋ID</param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="self">是否是查看自己，true:查看自己，false：查看全部</param>
        /// <returns></returns>
       DataTable GetOfficeHouses(int projectId, int buildingId, int cityId, int fxtCompanyId, bool self);

       /// <summary>
       /// 获取房号数据列表
       /// </summary>
       /// <param name="buildingId">楼栋ID</param>
       /// <param name="cityId"></param>
       /// <param name="fxtCompanyId"></param>
       /// <param name="self">是否是查看自己，true:查看自己，false：查看全部</param>
       /// <returns></returns>
       IQueryable<DatHouseOffice> GetOfficeHouseList(int buildingId, int cityId, int fxtCompanyId, bool self);
      
        /// <summary>
        /// 获取房号数据列表中室号
        /// </summary>
        /// <param name="buildingId">楼栋ID</param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="self">是否是查看自己，true:查看自己，false：查看全部</param>
        /// <returns></returns>
        IQueryable<DatHouseOffice> GetOfficeHouses_UnitNo(int buildingId, int cityId, int fxtCompanyId, bool self);


        /// <summary>
        /// 获取房号数据列表中楼层
        /// </summary>
        /// <param name="buildingId">楼栋ID</param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="self">是否是查看自己，true:查看自己，false：查看全部</param>
        /// <returns></returns>
        IQueryable<int> GetOfficeHouses_FloorNo(int buildingId, int cityId, int fxtCompanyId, bool self);

        /// <summary>
        /// 获取单条房号记录
        /// </summary>
        /// <param name="houseId">房号ID</param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        DatHouseOffice GetOfficeHouse(long houseId, int cityId,int fxtCompanyId);

       /// <summary>
       /// 获取房号ID
       /// </summary>
       /// <param name="buildingId"></param>
       /// <param name="houseName"></param>
       /// <param name="cityId"></param>
       /// <param name="fxtCompanyId"></param>
       /// <returns></returns>
       long GetOfficeHouseId(long buildingId, string houseName, int cityId, int fxtCompanyId);

       /// <summary>
       /// 获取楼盘名称
       /// </summary>
       /// <param name="buildingId"></param>
       /// <param name="fxtCompanyId"></param>
       /// <returns></returns>
       string GetProjectName(long buildingId, int fxtCompanyId);

       /// <summary>
       /// 获取楼栋名称
       /// </summary>
       /// <param name="buildingId"></param>
       /// <param name="fxtCompanyId"></param>
       /// <returns></returns>
       string GetBuildingName(long buildingId, int fxtCompanyId);

        /// <summary>
        /// 新增办公楼栋数据
        /// </summary>
        /// <param name="datHouseOffice">传参模型</param>
        /// <returns>返回成功条数</returns>
        int AddOfficeHouse(DatHouseOffice datHouseOffice);

       /// <summary>
       /// 修改实际层
       /// </summary>
       /// <param name="floorNo">物理层</param>
       /// <param name="floorNum">实际层</param>
       /// <param name="cityId">城市ID</param>
       /// <param name="fxtCompanyId">公司ID</param>
       /// <returns></returns>
        int UpdateFloorNum(int floorNo, string floorNum, int cityId, int fxtCompanyId, string saveUser);

        /// <summary>
        /// 修改办公楼栋
        /// </summary>
        /// <param name="datHouseOffice">传参模型</param>
        /// <param name="currentCompanyId">当前操作者所在的公司ID</param>
        /// <returns>返回成功条数</returns>
        int UpdateOfficeHouse(DatHouseOffice datHouseOffice, int currentCompanyId);

        /// <summary>
        /// 删除办公楼栋
        /// </summary>
        /// <param name="datHouseOffice">传参模型</param>
        /// <param name="currentCompanyId">当前操作者所在的公司ID</param>
        /// <returns>返回成功条数</returns>
        int DeleteOfficeHouse(DatHouseOffice datHouseOffice, int currentCompanyId);

        DataTable HouseSelfDefineExport(DatHouseOffice houseOffice, List<string> houseAttr, int CityId, int FxtCompanyId, bool self = true);
    }
}
