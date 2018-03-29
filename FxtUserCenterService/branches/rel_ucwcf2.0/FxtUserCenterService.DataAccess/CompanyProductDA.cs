using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.Entity;
using System.Data.SqlClient;
using CAS.DataAccess.BaseDAModels;
using System.Data;
using CAS.Entity;
using FxtUserCenterService.Entity.InheritClass;

namespace FxtUserCenterService.DataAccess
{
    public class CompanyProductDA : Base
    {
        /// <summary>
        /// 根据公司id和产品code获取信息(caoq 2013-7-12)
        /// </summary>
        /// <param name="companyid">公司id</param>
        /// <param name="producttypecode">产品code</param>
        /// <param name="signname">公司标识</param>
        /// <param name="isvalid">是否有效产品 1:仅查询有效产品</param>
        /// <returns></returns>
        public static List<CompanyProduct> Get(int companyid, string producttypecode, string signname, int isvalid)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = @"select p.*
                from dbo.CompanyProduct p with(nolock)
                join dbo.companyinfo c with(nolock) on c.companyid=p.companyid
                where 1=1 ";
            if (companyid > 0)
            {
                sql += " and p.CompanyId=@companyid";
                parameters.Add(SqlHelper.GetSqlParameter("@companyid", companyid, SqlDbType.Int));
            }
            if (!string.IsNullOrEmpty(producttypecode))
            {
                sql += " and p.ProductTypeCode in (" + producttypecode + ")";
            }
            if (!string.IsNullOrEmpty(signname))
            {
                sql += " and c.signname=@signname";
                parameters.Add(SqlHelper.GetSqlParameter("@signname", signname, SqlDbType.VarChar));
            }
            if (isvalid == 1)
            {
                sql += " and OverDate > getdate()";
            }
            return ExecuteToEntityList<CompanyProduct>(sql, CommandType.Text, parameters);
        }


        /// <summary>
        /// 修改产品部分信息:CAS产品LOGO,CAS产品小LOGO，对外显示的产品名称，产品联系电话(hody 2014-04-24)
        /// </summary>
        /// <param name="logoPath">CAS产品LOGO</param>
        /// <param name="smallLogoPath">CAS产品小LOGO</param>
        /// <param name="telephone">对外显示的产品名</param>
        /// <param name="titleName">产品联系电话</param>
        /// <returns></returns>
        public static int UpdateProductPartialInfo(string logoPath, string smallLogoPath, string telephone, string titleName, int companyid, int systypecode, string bgpic, string homepage,string twodimensionalcode)
        {
            SqlCommand cmd = new SqlCommand();
            string sql = SQLName.CompanyProduct.UpdateProductPartialInfo;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText =sql;
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@logopath", logoPath, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@smalllogopath", smallLogoPath, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@telephone", telephone, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@titlename", titleName, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@homepage", homepage, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@twodimensionalcode", twodimensionalcode, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@bgpic", bgpic, SqlDbType.VarChar));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@companyid", companyid, SqlDbType.Int));
            cmd.Parameters.Add(SqlHelper.GetSqlParameter("@systypecode", systypecode, SqlDbType.Int));
            return ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// 根据WebUrl查询产品信息
        /// </summary>
        /// <param name="weburl">网址</param>
        /// <param name="weburl1">备用网址</param>
        /// <returns></returns>
        public static InheritCompanyProduct GetProductInfoByWebUrl(string weburl, string weburl1)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQLName.CompanyProduct.GetProductInfoByWebUrl;
            parameters.Add(SqlHelper.GetSqlParameter("@weburl", weburl, SqlDbType.VarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@weburl1", weburl1, SqlDbType.VarChar));
            return ExecuteToEntity<InheritCompanyProduct>(sql, CommandType.Text, parameters);
        }
    }
}
