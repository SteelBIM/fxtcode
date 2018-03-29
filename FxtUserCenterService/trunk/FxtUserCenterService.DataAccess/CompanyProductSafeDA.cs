using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.Entity;
using CAS.Common;
using System.Data.SqlClient;
using System.Data;
using CAS.DataAccess.BaseDAModels;

namespace FxtUserCenterService.DataAccess
{
    public class CompanyProductSafeDA : Base
    {
        public static int Add(CompanyProductSafe model)
        {
            return InsertFromEntity<CompanyProductSafe>(model);
        }
        public static int Update(CompanyProductSafe model)
        {
            return UpdateFromEntity<CompanyProductSafe>(model);
        }
        //批量更新
        public static int UpdateMul(CompanyProductSafe model, int[] ids)
        {
            return UpdateFromIds<CompanyProductSafe>(model, ids);
        }
        public static int Delete(int id)
        {
            return DeleteByPrimaryKey<CompanyProductSafe>(id);
        }
        public static CompanyProductSafe GetCompanyProductSafeByPK(int id)
        {
            return ExecuteToEntityByPrimaryKey<CompanyProductSafe>(id);
        }
        /// <summary>
        /// 验证应用身份
        /// </summary>
        /// <param name="search"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static CompanyProductSafe ValidateCallIdentity(int productTypeCode, string validate)
		{
			CompanyProductSafe companyProductSafe = new CompanyProductSafe();
            string sql = SQL.CompanyProductSafe.ValidateCallIdentity;
            List<SqlParameter> parameters = new List<SqlParameter>();
			parameters.Add(SqlHelper.GetSqlParameter("@producttypecode", productTypeCode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@validate", validate, SqlDbType.VarChar));
			return ExecuteToEntity<CompanyProductSafe>(sql, System.Data.CommandType.Text, parameters);
		}
    }

}
