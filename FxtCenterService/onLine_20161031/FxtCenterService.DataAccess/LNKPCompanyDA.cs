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
    public class LNKPCompanyDA : Base
    {       
        /// <summary>
        /// 新增公司
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Add(LNKPCompany model)
        {
            return InsertFromEntity<LNKPCompany>(model);
        }
        /// <summary>
        /// 修改关联公司
        /// 创建人:曾智磊,日期:2014-06-26
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int Update(LNKPCompany model)
        {
            model.SetPrimaryKey<LNKPCompany>(new string[] { "projectid", "companytype", "cityid" });
            return UpdateFromEntity<LNKPCompany>(model);
        }
        /// <summary>
        /// 获取楼盘管理的企业信息
        /// 创建人:曾智磊,日期:2014-06-30
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <param name="companyTypeCode"></param>
        /// <returns></returns>
        public static LNKPCompany GetLNKPCompanyByComIdAndProjIdAndType(int companyId, int projectId, int cityId, int companyTypeCode)
        {
            string sql = SQL.PCompany.GetPCompanyByCompanyIdAndProjectId;
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@companyid", companyId, SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@projectid", projectId, SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@companytype", companyTypeCode, SqlDbType.Int));
            return ExecuteToEntity<LNKPCompany>(sql, System.Data.CommandType.Text, param);
        }
        /// <summary>
        /// 获取楼盘管理的企业
        /// 创建人:曾智磊,日期:2014-07-03
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static List<LNKPCompany> GetLNKPCompanyByProjId(int projectId, int cityId)
        {
            string sql = SQL.PCompany.GetPCompanyByProjectId;
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlHelper.GetSqlParameter("@cityid", cityId, SqlDbType.Int));
            param.Add(SqlHelper.GetSqlParameter("@projectid", projectId, SqlDbType.Int));
            return ExecuteToEntityList<LNKPCompany>(sql, System.Data.CommandType.Text, param);
        }
    }
}
