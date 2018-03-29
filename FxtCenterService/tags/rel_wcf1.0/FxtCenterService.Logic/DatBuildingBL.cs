using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.DBEntity;
using FxtCenterService.DataAccess;
using System.Data;
using System.Collections;
using CAS.Common;

namespace FxtCenterService.Logic
{
    public class DatBuildingBL
    {
        /// <summary>
        /// 根据楼盘ID获取所有楼栋列表（不分页）
        /// </summary>
        public static List<DATBuilding> GetDATBuildingList(SearchBase search, int projectId, int cityid, string buildingName)
        {
            return DatBuildingDA.GetDATBuildingList(search, projectId, cityid, buildingName);
        }
        public static List<DATBuilding> GetDATBuildingList(SearchBase search, int projectId, string key)
        {
            return DatBuildingDA.GetDATBuildingList(search, projectId, key);
        }
        /// <summary>
        /// 楼栋-获取楼栋下拉列表
        /// </summary>
        public static DataSet GetBuildingBaseInfoList(SearchBase search, int projectId, int avgprice)
        {
            return DatBuildingDA.GetBuildingBaseInfoList(search, projectId, avgprice);
        }

        /// <summary>
        /// 楼栋-获取楼栋下拉列表
        /// </summary>
        /// <returns></returns>
        public static List<DATBuilding> GetAutoBuildingInfoList(SearchBase search, int projectId,string key)
        {
            return DatBuildingDA.GetAutoBuildingInfoList(search, projectId,key);
        }
    }
}
