using CAS.Common;
using CAS.DataAccess.BaseDAModels;
using CAS.DataAccess.DA;
using CAS.Entity.DBEntity;
using CAS.Entity.FxtProject;
using FxtCenterService.DataAccess.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FxtCenterService.DataAccess
{
    public class ProjectMatchDA : Base
    {
        public static List<ProjectMatch> GetProjectMatchList(SearchBase search, string netName)
        {
            string sql = SQLName.Project.ProjectMatchList;
            CityTable city = CityTableDA.GetCityTable(search.CityId);
            if (null == city)
                return new List<ProjectMatch>();
            sql = sql.Replace("@table_dat_project", city.ProjectTable);
            List<SqlParameter> sqlParams = new List<SqlParameter>() { 
            	SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int),
				SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int),
				SqlHelper.GetSqlParameter("@typecode", search.SysTypeCode, SqlDbType.Int)
            };
            if (!string.IsNullOrEmpty(netName))
            {
                sql += " and pm.NetName = @netName";
                sqlParams.Add(SqlHelper.GetSqlParameter("@netName", netName + "%", SqlDbType.VarChar));
            }
            if (search.Page)
            {
                sql = HandleSQL(search, sql);
            }
            return BaseDA.ExecuteToEntityList<ProjectMatch>(sql, CommandType.Text, sqlParams);
        }
    }
}