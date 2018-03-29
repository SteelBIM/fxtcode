using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;


namespace FXT.DataCenter.Domain.Services
{
    public interface IHouseWeightRevised
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
        /// <param name="buildingId"></param>
        /// <param name="houseId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        DatWeightHouse GetWeightHouse(int projectId, int buildingId, int houseId, int cityId, int ParentShowDataCompanyId, int ParentProductTypeCode);

        /// <summary>
        /// 获取房号修正系数ID
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="houseId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        //long GetWeightHouseByHouseId(int projectId, int buildingId, int houseId, int cityId, int fxtCompanyId);

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

        /// <summary>
        /// 删除房号修正系数
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="buildingId"></param>
        /// <param name="houseId"></param>
        /// <param name="cityId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <returns></returns>
        //int DeleteWeightHouse(int projectId, int buildingId, int houseId, int cityId, int fxtCompanyId);
    }
}