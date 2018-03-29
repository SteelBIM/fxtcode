using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtDataCenterDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using CAS.Entity.DBEntity;

namespace FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager
{
    public static class DataCenterHouseApi
    {
        /// <summary>
        /// 获取楼盘下拉列表
        /// </summary>
        /// <param name="key">搜索关键字</param>
        /// <param name="cityId">当前城市</param>
        /// <param name="username">当前调用api的username</param>
        /// <param name="signname">当前调用api的signname</param>
        /// <param name="appList">当前当前用户拥有的api信息集合</param>
        /// <returns></returns>
        public static IList<DATHouse> GetHouseByBuildingIdAndFloorNo(int buildingId, int floorNo, int cityId, string username, string signname, List<Apps> appList)
        {
            var para = new { buildingid = buildingId, floorno = floorNo, cityid = cityId,  pageindex = 1, pagerecords = 300, };
            DataCenterResult result = Common.PostDataCenter(username, signname, Common.queryhousedropdownlist, para, appList);
            IList<DATHouse> list = new List<DATHouse>();
            if (!string.IsNullOrEmpty(result.data))
            {
                list = result.data.ParseJSONList<DATHouse>();
            }
            return list;
        }
    }
}
