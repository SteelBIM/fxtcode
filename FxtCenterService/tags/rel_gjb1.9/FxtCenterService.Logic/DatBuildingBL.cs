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
        /// 新增楼栋信息到主表
        /// 创建人:曾智磊, 日期:2014-06-27
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName">根据城市查询出来的表名</param>
        /// <returns></returns>
        public static int Add(DATBuilding model, string tableName)
        {
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName;
            DATBuilding.SetTableName<DATBuilding>(_tableName);
            return DatBuildingDA.Add(model);

        }
        /// <summary>
        /// 新增楼栋信息到子表
        /// 创建人:曾智磊, 日期:2014-06-27
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static int AddSub(DATBuilding model, string tableName)
        {
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName;
            _tableName = tableName + "_sub";
            DATBuilding.SetTableName<DATBuilding>(_tableName);
            return DatBuildingDA.Add(model);
        }
        /// <summary>
        /// 修改楼栋信息到主表
        /// 创建人:曾智磊, 日期:2014-06-27
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName">根据城市查询出来的表名</param>
        /// <returns></returns>
        public static int Update(DATBuilding model, string tableName)
        {
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName;
            DATBuilding.SetTableName<DATBuilding>(_tableName);
            return DatBuildingDA.Update(model);

        }
        /// <summary>
        /// 修改楼栋信息到子表
        /// 创建人:曾智磊, 日期:2014-06-27
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tableName">根据城市查询出来的表名</param>
        /// <returns></returns>
        public static int UpdateSub(DATBuilding model, string tableName)
        {
            if (model == null || string.IsNullOrEmpty(tableName))
            {
                return 0;
            }
            string _tableName = tableName + "_sub";
            DATBuilding.SetTableName<DATBuilding>(_tableName);
            return DatBuildingDA.Update(model);

        }

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
        /// <summary>
        /// 获取楼栋信息
        /// 创建人:曾智磊,日期:2014-06-30
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="buildingName"></param>
        /// <returns></returns>
        public static DATBuilding GetBuildingByName(int cityId, int projectId, int fxtCompanyId, string buildingName)
        {
            return DatBuildingDA.GetBuildingByName(cityId, projectId, fxtCompanyId, buildingName);
        }
        /// <summary>
        /// 根据ID获取楼栋信息(关联子表)
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="projectId"></param>
        /// <param name="fxtCompanyId"></param>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public static DATBuilding GetBuildingById(int cityId, int projectId, int fxtCompanyId, int buildingId)
        {
            return DatBuildingDA.GetBuildingById(cityId, projectId, fxtCompanyId, buildingId);
        }
    }
}
