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
    public class DatPeitaoDA : Base
    {
        public static int Add(DatPeitao model)
        {
            return InsertFromEntity<DatPeitao>(model);
        }
        public static int Update(DatPeitao model)
        {
            return UpdateFromEntity<DatPeitao>(model);
        }

        /// <summary>
        /// 获取所有配套信息
        /// </summary>
        /// <param name="cityId">城市ID</param>
        /// <returns></returns>
        public static List<DatPeitao> GetPeitaoByCityId(int cityId)
        {
            string sql = "select * from FxtDataCenter.dbo.Dat_PeiTao where cityid = @cityid ";
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.Int));
            return ExecuteToEntityList<DatPeitao>(sql, System.Data.CommandType.Text, param);
        }
    }
}
