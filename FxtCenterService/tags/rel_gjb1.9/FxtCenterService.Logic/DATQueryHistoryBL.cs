using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CAS.Common;
using CAS.Entity.DBEntity;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity.FxtProject;
using FxtCenterService.DataAccess;

namespace FxtCenterService.Logic
{
    public class DATQueryHistoryBL
    {
        public static int Add(DATQueryHistory model)
        {
            return DATQueryHistoryDA.Add(model);
        }
        public static int Update(DATQueryHistory model)
        {
            return DATQueryHistoryDA.Update(model);
        }
        //批量更新
        public static int UpdateMul(DATQueryHistory model, int[] ids)
        {
            return DATQueryHistoryDA.UpdateMul(model, ids);
        }
        public static int Delete(int id)
        {
            return DATQueryHistoryDA.Delete(id);
        }

        public static DATQueryHistory GetDATQueryHistoryByPK(int id)
        {
            return DATQueryHistoryDA.GetDATQueryHistoryByPK(id);
        }
        /// <summary>
        /// 获取自动估价记录
        /// </summary>
        /// <param name="search"></param>
        /// <param name="username">账号</param>
        /// <returns></returns>
        public static List<DATQueryHistory> GetDATQueryHistoryList(SearchBase search, string username, string wxopenid)
        {
            return DATQueryHistoryDA.GetDATQueryHistoryList(search, username, wxopenid);
        }


        /// <summary>
        /// 标准化楼盘楼栋房号匹配
        /// </summary>
        /// <param name="cityid">城市Id</param>
        /// <param name="projectname">楼盘名称</param>
        /// <param name="addresss">地址</param>
        /// <param name="buildingname">楼栋名称</param>
        /// <param name="housename">房号名称</param>
        /// <returns></returns>
        public static DataSet GetMatchingData(int cityid,string projectname, string addresss,
            string buildingname, string housename, int p_projectid, int p_buildingid, int fxtcompanyid)
        {
            return DATQueryHistoryDA.GetMatchingData(cityid, projectname, addresss, buildingname, housename, p_projectid, p_buildingid, fxtcompanyid);
        }

    }

}
