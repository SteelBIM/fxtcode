using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.Entity.FxtDataCenter;
using System.Data.SqlClient;
using System.Data;
using CAS.DataAccess.BaseDAModels;

namespace FxtCenterService.DataAccess
{
    public class ShareDA : Base
    {
        /// <summary>
        ///  添加
        /// </summary>
        /// <returns></returns>
        public static int AddCompanyShowData(PriviCompanyShowData pcs)
        {
            if (pcs == null) throw new ArgumentException();

            pcs.SetIgnoreFields(new string[] { "id", "CompanyName" });

            return InsertFromEntity<PriviCompanyShowData>(pcs);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public static int UpdateCompanyShowData(PriviCompanyShowData pcs, IEnumerable<string> keys)
        {
            if (pcs == null) throw new ArgumentException();

            if (pcs.cityid == 0 || pcs.fxtcompanyid == 0 || pcs.typecode == 0) throw new ArgumentException("必要参数缺失！");

            var updatedFields = keys.Except(new List<string> { "cityid", "fxtcompanyid", "typecode" })
                                      .ToList()
                                      .ToArray();

            pcs.SetAvailableFields(updatedFields);
            pcs.SetPrimaryKey<PriviCompanyShowData>(new string[] { "cityid", "fxtcompanyid", "typecode" });

            return UpdateFromEntity<PriviCompanyShowData>(pcs);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="pcs"></param>
        /// <returns></returns>
        public static List<PriviCompanyShowData> GetCompanyShowData(PriviCompanyShowData pcs)
        {
            if (pcs == null) 
                throw new ArgumentException();
            if (pcs.cityid == 0 || pcs.typecode == 0) 
                throw new ArgumentException("查询参数缺失！");
            var parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.GetSqlParameter("@cityid", pcs.cityid, SqlDbType.Int));
            parameters.Add(SqlHelper.GetSqlParameter("@typecode", pcs.typecode, SqlDbType.Int));
            var sql = "select cs.*,'' as companyName from  FxtDataCenter.dbo.privi_company_showdata cs with(nolock) where cs.cityId = @cityId and cs.typecode = @typecode";
            if (pcs.fxtcompanyid > 0) {
                sql += " and cs.fxtcompanyId = @fxtcompanyId";
                parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyId", pcs.fxtcompanyid, SqlDbType.Int));
            }
            return ExecuteToEntityList<PriviCompanyShowData>(sql, System.Data.CommandType.Text, parameters);
        }
    }
}
