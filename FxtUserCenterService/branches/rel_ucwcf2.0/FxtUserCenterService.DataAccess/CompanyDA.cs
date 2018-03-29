using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.Entity;
using System.Data.SqlClient;
using CAS.DataAccess.BaseDAModels;
using System.Data;
using CAS.Common;

namespace FxtUserCenterService.DataAccess
{
    public class CompanyDA:Base
    {

        public static CompanyInfo GetCompanyBySignName(string signName) 
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string strSql = "select top 1 * from dbo.companyinfo where signname=@signname";
            parameters.Add(SqlHelper.GetSqlParameter("@signName", signName, SqlDbType.NVarChar));
            return ExecuteToEntity<CompanyInfo>(strSql, CommandType.Text, null);   
        }

        public static CompanyInfo Get(string companycode)
        {
            string sql = @"select top 1 * from dbo.companyinfo where CompanyCode=@CompanyCode or wxid=@CompanyCode";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.GetSqlParameter("@CompanyCode", companycode, SqlDbType.NVarChar));
            return ExecuteToEntity<CompanyInfo>(sql, CommandType.Text, parameters);            
        }

        public static CompanyInfo Get(int companyid)
        {
            return ExecuteToEntityByPrimaryKey<CompanyInfo>(companyid);
        }

        public static int update(string companycode, string wxid, string wxname)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = @"update dbo.companyinfo set wxid=@wxid,wxname=@wxname where CompanyCode=@CompanyCode";
            cmd.CommandText = sql;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@CompanyCode", companycode, SqlDbType.NVarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@wxid", wxid, SqlDbType.NVarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@wxname", wxname, SqlDbType.NVarChar));
            return ExecuteNonQuery(cmd);   
        }

        /// <summary>
        /// 通过productTypeCode、key搜索
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<CompanyInfo> GetCompanyInfo(SearchBase search, int productTypeCode, string key) 
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = @"select ci.wxid,ci.CompanyID,ci.CompanyCode,ci.CompanyName,cp.OverDate from dbo.CompanyInfo as ci join dbo.CompanyProduct as cp
            on ci.CompanyID = cp.CompanyId and ci.Valid =1 and cp.Valid =1 where 1=1 and cp.OverDate>=GETDATE() ";

            if (productTypeCode>0)
            {
                sql += " and cp.ProductTypeCode = @ProductTypeCode";
                parameters.Add(SqlHelper.GetSqlParameter("@ProductTypeCode", productTypeCode, SqlDbType.Int));
            }

            if (!string.IsNullOrEmpty(key))
            {
                sql += " and ci.CompanyCode like @key and ci.CompanyName like @key";
                parameters.Add(SqlHelper.GetSqlParameter("@key", "%"+key+"%", SqlDbType.VarChar));
            }
            search.OrderBy = " CompanyID desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<CompanyInfo>(sql, CommandType.Text, parameters);   
        }

        public static List<CompanyInfo> CompanyList()
        {
            string sql = @"
                    SELECT [CompanyID]
                  ,[CompanyName]
                  ,[CompanyCode]
                  ,[CreateDate]
                  ,[Valid]
                  ,[BusinessDB]
                  ,[SMSLoginName]
                  ,[SMSLoginPassword]
                  ,[SMSSendName]
                  ,[wxid]
                  ,[wxname]
                  ,[SignName]
              FROM [dbo].[CompanyInfo]";
            return ExecuteToEntityList<CompanyInfo>(sql, CommandType.Text, new List<SqlParameter>());
        }

        /// <summary>
        /// 获得公司信息通过signname
        /// </summary>
        /// <param name="systypecode">产品code</param>
        /// <param name="signname">公司标示</param>
        /// <param name="appid">appid</param>
        /// <param name="apppwd">apppwd</param>
        /// <returns></returns>
        public static CompanyInfo GetCompanyInfoBySignName(int systypecode, string signname, int appid, string apppwd, string functionname)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql  = SQL.Company.GetCompanyInfoBySignName;

            sql += @"and  ci.SignName = @signname  ";//and cpf.FunctionName=@functionname  and cp.ProductTypeCode = @producttypecode and cpa.AppId = @appid and cpa.AppPwd = @apppwd
            parameters.Add(SqlHelper.GetSqlParameter("@signname",signname,SqlDbType.VarChar));
            //parameters.Add(SqlHelper.GetSqlParameter("@producttypecode", systypecode, SqlDbType.Int));
            //parameters.Add(SqlHelper.GetSqlParameter("@appid", appid, SqlDbType.Int));
            //parameters.Add(SqlHelper.GetSqlParameter("@apppwd", apppwd, SqlDbType.VarChar));
            //parameters.Add(SqlHelper.GetSqlParameter("@functionname", functionname, SqlDbType.VarChar));
            return ExecuteToEntity<CompanyInfo>(sql, CommandType.Text, parameters);   
        }
    }
}
