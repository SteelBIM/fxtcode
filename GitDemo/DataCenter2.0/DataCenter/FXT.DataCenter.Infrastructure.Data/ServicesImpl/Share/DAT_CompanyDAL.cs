using System;
using System.Linq;
using Dapper;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using System.Data.SqlClient;
using FXT.DataCenter.Infrastructure.Common.DBHelper;
using System.Data;

namespace FXT.DataCenter.Infrastructure.Data.ServicesImpl
{
    public class DAT_CompanyDAL : IDAT_Company
    {
        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <param name="ChineseName">公司名称</param>
        /// <returns></returns>
        public DAT_Company GetDAT_CompanyInfo(string ChineseName)
        {
            try
            {
                string sql = @"select CompanyId, ChineseName, EnglishName, CompanyTypeCode, 
                           CityId, Address, Telephone, Fax, Website, CreateDate, Valid
                           from FXTDataCenter.dbo.DAT_Company with(nolock)
                           where Valid=1 and  ChineseName=@ChineseName";
                SqlParameter par = new SqlParameter("@ChineseName", ChineseName.Trim());
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
                return SqlModelHelper<DAT_Company>.GetSingleObjectBySql(sql, par);
            }
            catch
            {

                return null;
            }

        }
        /// <summary>
        /// 添加公司
        /// </summary>
        /// <param name="ChineseName">公司中文名称</param>
        /// <param name="Protypecode">产品Code</param>
        /// <param name="CityId">城市Id</param>
        /// <returns></returns>
        public int AddDAT_Compandy(string ChineseName, int Protypecode, string CityId, int fxtcompanyid)
        {

            try
            {
                string sql = @"INSERT INTO [FXTDataCenter].[dbo].[DAT_Company] with(rowlock)([ChineseName],[CompanyTypeCode],[CityId],[CreateDate],fxtcompanyid)
                               VALUES(@ChineseName,@CompanyTypeCode,@CityId,@CreateDate,@fxtcompanyid);Select SCOPE_IDENTITY()";
                SqlParameter[] par = { 
                                 new SqlParameter("@ChineseName",ChineseName.Trim()),
                                 new SqlParameter("@CompanyTypeCode",Protypecode),
                                 new SqlParameter("@CityId",CityId),
                                 new SqlParameter("@fxtcompanyid",fxtcompanyid),
                                 new SqlParameter("@CreateDate",DateTime.Now)
                                 };
                DBHelperSql.ConnectionString = ConfigurationHelper.FxtDataCenter;
                object newId = DBHelperSql.ExecuteScalar(sql, par);
                if (newId != null && Convert.ToInt32(newId) > 0)
                    return Convert.ToInt32(newId);
                else return 0;
            }
            catch
            {
                return 0;
            }
        }

    }
}
