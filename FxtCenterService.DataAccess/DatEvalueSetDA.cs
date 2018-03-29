using CAS.Entity.FxtDataCenter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FxtCenterService.DataAccess
{
    public class DatEvalueSetDA : Base
    {
        /// <summary>
        /// 获取自动估价设置表
        /// </summary>
        /// <param name="FxtCompanyId"></param>
        /// <param name="CityId"></param>
        /// <param name="valid"></param>
        public static DatEvalueSet GetEvalueSetBy(int FxtCompanyId, int CityId, int valid = 1)
        {
            string strSql = "SELECT top 1 * FROM [FxtDataCenter].[dbo].[Sys_EvalueSet] where FxtCompanyId = @FxtCompanyId and CityId = @CityId and valid = 1";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@FxtCompanyId", FxtCompanyId));
            parameters.Add(new SqlParameter("@CityId", CityId));
            parameters.Add(new SqlParameter("@valid", valid));
            DatEvalueSet entity = ExecuteToEntity<DatEvalueSet>(strSql, CommandType.Text, parameters);
            return entity;
        }
    }
}
