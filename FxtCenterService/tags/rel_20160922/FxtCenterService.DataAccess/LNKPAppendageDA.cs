using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.DataAccess.DA;
using CAS.Common;
using System.Data.SqlClient;
using System.Data;
using CAS.DataAccess.BaseDAModels;
using CAS.Entity;
using CAS.Entity.DBEntity;

//创建人:曾智磊,日期:2014-06-26
namespace FxtCenterService.DataAccess
{
    public class LNKPAppendageDA : Base
    {

        public static int Add(LNKPAppendage model)
        {                        
            return InsertFromEntity<LNKPAppendage>(model);
        }
        public static int Update(LNKPAppendage model)
        {
            return UpdateFromEntity<LNKPAppendage>(model);
        }
        /// <summary>
        /// 获取楼盘所有配套信息
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static List<LNKPAppendage> GetPAppendageByProjectId(int projectId, int cityId)
        {
            string sql = SQL.Project.GetPAppendageByProjectId;
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@projectid", projectId, SqlDbType.Int));
            return ExecuteToEntityList<LNKPAppendage>(sql, System.Data.CommandType.Text, param);
        }
    }
}
