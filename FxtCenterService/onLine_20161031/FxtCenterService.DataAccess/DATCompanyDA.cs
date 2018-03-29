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
    public class DATCompanyDA : Base
    {
        /// <summary>
        /// 新增公司
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Add(DATCompany model)
        {
            return InsertFromEntity<DATCompany>(model);
        }
        /// <summary>
        /// 根据公司名称获取表信息
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DATCompany GetByName(string name)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.DCompany.GetDATCompanyByName;
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            parameters.Add(SqlHelper.GetSqlParameter("@name", name, SqlDbType.VarChar));
            return ExecuteToEntity<DATCompany>(sql, System.Data.CommandType.Text, parameters);
        }
    }
}
