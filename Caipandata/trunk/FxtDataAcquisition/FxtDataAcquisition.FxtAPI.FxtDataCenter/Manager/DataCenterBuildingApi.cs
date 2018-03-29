using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtDataAcquisition.Common;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Domain.DTO.FxtDataCenterDTO;
using CAS.Entity.DBEntity;

namespace FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager
{
    public static class DataCenterBuildingApi
    {
        /// <summary>
        /// 根据楼盘ID获取楼栋列表
        /// </summary>
        /// <param name="key">楼栋关键字,可不填</param>
        /// <param name="cityId">城市</param>
        /// <param name="projectId">楼盘ID</param>
        /// <param name="username">当前调用api的username</param>
        /// <param name="signname">当前调用api的signname</param>
        /// <param name="appList">当前用户拥有的api集合</param>
        /// <returns></returns>
        public static IList<DATBuilding> GetBuildingByProjectId(string key, int cityId, int projectId, string username, string signname, List<Apps> appList)
        {

            var para = new { projectid = projectId, cityid = cityId, key = key, pageindex = 1, pagerecords = 300 };
            DataCenterResult result = Common.PostDataCenter(username, signname, Common.queryautobuildinginfolist, para, appList);
            IList<DATBuilding> list = new List<DATBuilding>();
            if (!string.IsNullOrEmpty(result.data))
            {
                list = result.data.ParseJSONList<DATBuilding>();
            }
            return list;
        }

        /// <summary>
        /// 获取楼栋下拉列表
        /// </summary>
        /// <param name="key">搜索关键字</param>
        /// <param name="cityId">当前城市</param>
        /// <param name="projectId">楼盘</param>
        /// <param name="username">当前调用api的username</param>
        /// <param name="signname">当前调用api的signname</param>
        /// <param name="appList">当前用户拥有的api集合</param>
        /// <returns></returns>
        public static IList<CAS.Entity.DBEntity.DATBuilding> GetBuilding(string key, int cityId, int projectId, int pageindex, int pagerecords, string username, string signname, List<Apps> appList)
        {
            var para = new { cityid = cityId, key = key, projectid = projectId, pageindex = pageindex, pagerecords = pagerecords };
            DataCenterResult result = Common.PostDataCenter(username, signname, Common.queryautobuildinginfolist, para, appList);
            IList<CAS.Entity.DBEntity.DATBuilding> list = new List<CAS.Entity.DBEntity.DATBuilding>();
            if (!string.IsNullOrEmpty(result.data))
            {
                list = result.data.ParseJSONList<CAS.Entity.DBEntity.DATBuilding>();
            }
            return list;
        }
    }
}
