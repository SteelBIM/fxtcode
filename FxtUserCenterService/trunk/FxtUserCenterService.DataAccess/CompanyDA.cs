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
    public class CompanyDA : Base
    {

        public static CompanyInfo GetCompanyBySignName(string signName)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string strSql = "select top 1 * from dbo.companyinfo where signname=@signname";
            parameters.Add(SqlHelper.GetSqlParameter("@signname", signName, SqlDbType.NVarChar));
            return ExecuteToEntity<CompanyInfo>(strSql, CommandType.Text, parameters);
        }

        public static CompanyInfo GetByName(string neme)
        {
            string sql = @"select top 1 * from dbo.companyinfo where CompanyName=@CompanyName ";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.GetSqlParameter("@CompanyName", neme, SqlDbType.NVarChar));
            return ExecuteToEntity<CompanyInfo>(sql, CommandType.Text, parameters);
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
        /// 根据公司ID更新微信ID、微信名称
        /// zhoub 20160907
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="wxid"></param>
        /// <param name="wxname"></param>
        /// <returns></returns>
        public static int UpdateWXByCompanyId(int companyid, string wxid, string wxname)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = @"update fxtusercenter.dbo.companyinfo set wxid=@wxid,wxname=@wxname where companyid=@companyid";
            cmd.CommandText = sql;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@companyid", companyid, SqlDbType.Int));
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

            if (productTypeCode > 0)
            {
                sql += " and cp.ProductTypeCode = @ProductTypeCode";
                parameters.Add(SqlHelper.GetSqlParameter("@ProductTypeCode", productTypeCode, SqlDbType.Int));
            }

            if (!string.IsNullOrEmpty(key))
            {
                sql += " and ci.CompanyCode like @key and ci.CompanyName like @key";
                parameters.Add(SqlHelper.GetSqlParameter("@key", "%" + key + "%", SqlDbType.VarChar));
            }
            search.OrderBy = " CompanyID desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<CompanyInfo>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 根据参数查询公司
        /// 修改人 caoq 2014-12-11 增加参数producttypecode（查询已开通指定产品公司）
        /// </summary>
        /// <param name="search"></param>
        /// <param name="companyname">查询公司名称</param>
        /// <param name="companytypecode">公司类型</param>
        /// <param name="producttypecode">产品CODE(查询已开通指定产品公司)</param>
        /// <returns></returns>
        public static List<CompanyInfo> GetCompanyList(SearchBase search, string companyname, int companytypecode, int producttypecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQL.Company.GetCompanyList;

            if (!string.IsNullOrEmpty(companyname))
            {
                sql += " and c.CompanyName like @key";
                parameters.Add(SqlHelper.GetSqlParameter("@key", "%" + companyname + "%", SqlDbType.VarChar));
            }
            if (companytypecode > 0)
            {
                sql += " and c.companytypecode = @companytypecode";
                parameters.Add(SqlHelper.GetSqlParameter("@companytypecode", companytypecode, SqlDbType.Int));
            }
            //查询已开通产品的公司
            if (producttypecode > 0)
            {
                sql = sql.Replace("@prowhere", "inner join dbo.companyproduct cp with(nolock) on cp.companyid=c.companyid and cp.producttypecode=" + producttypecode);
            }
            else
            {
                sql = sql.Replace("@prowhere", "");
            }
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<CompanyInfo>(sql, CommandType.Text, parameters);
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
            string sql = SQL.Company.GetCompanyInfoBySignName;

            sql += @"and  ci.SignName = @signname  ";//and cpf.FunctionName=@functionname  and cp.ProductTypeCode = @producttypecode and cpa.AppId = @appid and cpa.AppPwd = @apppwd
            parameters.Add(SqlHelper.GetSqlParameter("@signname", signname, SqlDbType.VarChar));
            //parameters.Add(SqlHelper.GetSqlParameter("@producttypecode", systypecode, SqlDbType.Int));
            //parameters.Add(SqlHelper.GetSqlParameter("@appid", appid, SqlDbType.Int));
            //parameters.Add(SqlHelper.GetSqlParameter("@apppwd", apppwd, SqlDbType.VarChar));
            //parameters.Add(SqlHelper.GetSqlParameter("@functionname", functionname, SqlDbType.VarChar));
            return ExecuteToEntity<CompanyInfo>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 获得已签约客户的
        /// </summary>
        /// <param name="systypecode">产品code</param>
        /// <param name="signname">公司标示</param>
        /// <param name="appid">appid</param>
        /// <param name="apppwd">apppwd</param>
        /// <returns></returns>
        public static List<CompanyInfo> GetCompanyListIssigned(SearchBase search, int provinceid, int issigned)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string sql = SQL.Company.GetCompanyListIssigned;
                search.OrderBy = " issigned desc,joindate ";
                if (provinceid > 0)
                {
                    sql += " and sp.provinceid = @provinceid";
                    parameters.Add(SqlHelper.GetSqlParameter("@provinceid", provinceid, SqlDbType.Int));

                }
                if (search.CityId > 0)
                {
                    sql += " and sc.cityid = @cityid";
                    parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));

                }
                if (issigned > 0)
                {
                    sql += " and ci.issigned = @issigned";
                    parameters.Add(SqlHelper.GetSqlParameter("@issigned", issigned, SqlDbType.Int));

                }

                if (!string.IsNullOrEmpty(search.Key))
                {
                    sql += " and ci.companyname like @key";
                    parameters.Add(SqlHelper.GetSqlParameter("@key", "%" + search.Key + "%", SqlDbType.VarChar));

                }
                sql = HandleSQL(search, sql);

                return ExecuteToEntityList<CompanyInfo>(sql, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int Add(CompanyInfo model)
        {
            string sql = SQL.Company.AddCompany;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sql;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@CompanyId", model.companyid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@CompanyName", model.companyname, SqlDbType.NVarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@CompanyCode", model.companycode, SqlDbType.NVarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@SignName", model.signname, SqlDbType.NVarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@CompanyTypeCode", model.companytypecode, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@ShortName", model.shortname, SqlDbType.NVarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@CityId", model.cityid, SqlDbType.Int));
            return ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 根据产品code获取已签约且业务数据库连接不为空的公司
        /// zhoub 20160727
        /// </summary>
        /// <param name="systypecode">产品code</param>
        /// <returns></returns>
        public static List<CompanyInfo> GetCompanyBusinessDBList(int systypecode)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                string sql = SQL.Company.GetCompanyBusinessDBList;
                parameters.Add(SqlHelper.GetSqlParameter("@productTypeCode", systypecode, SqlDbType.Int));
                return ExecuteToEntityList<CompanyInfo>(sql, CommandType.Text, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据公司ID获取公司信息（多个ID用逗号隔开）
        /// zhoub 20160908
        /// </summary>
        /// <param name="companyids"></param>
        /// <returns></returns>
        public static List<CompanyInfo> GetCompanyInfoByCompanyIds(string companyids)
        {
            string sql = SQLName.Company.GetCompanyInfoByCompanyIds;
            sql = sql.Replace("@companyids", companyids);
            List<SqlParameter> parameters = new List<SqlParameter>();
            return ExecuteToEntityList<CompanyInfo>(sql, CommandType.Text,null);
        }
    }
}
