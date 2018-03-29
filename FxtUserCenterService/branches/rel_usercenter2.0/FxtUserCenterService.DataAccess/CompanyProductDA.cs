using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtUserCenterService.Entity;
using System.Data.SqlClient;
using CAS.DataAccess.BaseDAModels;
using System.Data;
using CAS.Entity;

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
            string sql = @"select p.CompanyId, p.ProductTypeCode, p.CurrentVersion, p.StartDate, p.OverDate, p.WebUrl, p.APIUrl, p.OutAPIUrl, p.MsgServer, p.CreateDate, p.Valid
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
    }
}
