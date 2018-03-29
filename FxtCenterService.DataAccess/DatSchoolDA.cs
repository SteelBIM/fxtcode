using CAS.DataAccess.BaseDAModels;
using CAS.Entity.DBEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FxtCenterService.DataAccess
{
    public class DatSchoolDA : Base
    {
        public static int Add(DatSchool model)
        {
            return InsertFromEntity<DatSchool>(model);
        }
        public static int Update(DatSchool model)
        {
            return UpdateFromEntity<DatSchool>(model);
        }

        /// <summary>
        /// 获取所有学校信息
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        public static List<DatSchool> GetSchoolByCityId(int cityId)
        {
            string sql = "select * from FxtDataCenter.dbo.Dat_School where cityid = @cityid ";
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.Int));
            return ExecuteToEntityList<DatSchool>(sql, System.Data.CommandType.Text, param);
        }
    }
}
