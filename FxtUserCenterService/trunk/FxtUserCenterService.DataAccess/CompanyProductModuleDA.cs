using CAS.Common;
using CAS.DataAccess.BaseDAModels;
using FxtUserCenterService.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace FxtUserCenterService.DataAccess
{
    public class CompanyProductModuleDA : Base
    {
        /// <summary>
        /// 根据公司signname获取所有产品模块信息
        /// </summary>
        /// <param name="signname"></param>
        /// <param name="producttypecode"></param>
        /// <returns></returns>
        public static List<CompanyProductModule> Get(string signname, int producttypecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = @"select p.*
                from dbo.CompanyProduct p with(nolock)
                join dbo.companyinfo c with(nolock) on c.companyid=p.companyid
                where 1=1 and overdate > getdate() and p.ProductTypeCode = @ProductTypeCode and c.signname = @signname";
            parameters.Add(SqlHelper.GetSqlParameter("@signname", signname, SqlDbType.NVarChar));
            parameters.Add(SqlHelper.GetSqlParameter("@ProductTypeCode", producttypecode, SqlDbType.Int));

            return ExecuteToEntityList<CompanyProductModule>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 查询是否开通产品模块权限
        /// zhoub 20160908
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static List<CompanyProductModule> GetCompanyProductModuleWhetherToOpen(int producttypecode, int cityid, int companyid, int parentmodulecode, int modulecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = @"select * from FxtUserCenter.dbo.CompanyProduct_Module t1 with(nolock) 
where t1.ProductTypeCode=@producttypecode and t1.Valid=1 and t1.CityId=@cityid and t1.CompanyId=@companyid and t1.ParentModuleCode=@parentmodulecode
and t1.ModuleCode=@modulecode and t1.OverDate>GETDATE()";
            parameters.Add(SqlHelper.GetSqlParameter("@producttypecode", producttypecode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", cityid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@companyid", companyid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@parentmodulecode", parentmodulecode, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@modulecode", modulecode, SqlDbType.Int));

            return ExecuteToEntityList<CompanyProductModule>(sql, CommandType.Text, parameters);
        }

        /// <summary>
        /// 根据公司ID和产品code获取公司开通产品模块城市ID(数据中心网站需求)
        /// zhoub 20161026
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="producttypecode"></param>
        /// <returns></returns>
        public static List<CompanyProductModule> GetCompanyProductAndCompanyProductModuleCityIds(SearchBase search,int companyid, int producttypecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQLName.CompanyProduct.GetCompanyProductAndCompanyProductModuleCityIds;
            parameters.Add(SqlHelper.GetSqlParameter("@companyid", companyid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@productCode", producttypecode, SqlDbType.Int));
            search.OrderBy = " CreateDate desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<CompanyProductModule>(sql, CommandType.Text, parameters);
        }


        /// <summary>
        /// 根据公司ID和产品code获取公司开通产品模块详细信息(数据中心网站需求)
        /// zhoub 20161026
        /// </summary>
        /// <param name="companyid"></param>
        /// <param name="producttypecode"></param>
        /// <returns></returns>
        public static List<CompanyProductModule> GetCompanyProductModuleDetails(SearchBase search,int companyid, int producttypecode)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = SQLName.CompanyProduct.GetCompanyProductModuleDetails;
            parameters.Add(SqlHelper.GetSqlParameter("@companyid", companyid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@productCode", producttypecode, SqlDbType.Int));
            search.OrderBy = " CreateDate desc";
            sql = HandleSQL(search, sql);
            return ExecuteToEntityList<CompanyProductModule>(sql, CommandType.Text, parameters);
        }
    }
}
