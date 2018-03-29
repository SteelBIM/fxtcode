using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CAS.Entity;
using System.Data.SqlClient;
using CAS.DataAccess.BaseDAModels;
using CAS.Common;
using CAS.Entity.DBEntity;

// Project - Building - Floor - House 联动功能 kevin 2013-3-21
namespace CAS.DataAccess.DA.PBFH
{
    public class ProjectDA:Base
    {
        /// <summary>
        /// 获取楼盘下拉列表  
        /// </summary>
        public static List<DATProject> GetProjectDropDownList(SearchBase search,bool leftMatch)
        {
            string sql = SQL.PBFH.ProjectDropDownList;
            string projectTable = TableByCity.projecttable;            
            try
            {
                sql = sql.Replace("@projecttable", projectTable);
                sql = sql.Replace("@top", search.Top.ToString());
                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@cityid", search.CityId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
                if (!string.IsNullOrEmpty(search.Key))
			    {
                    string key = SQLFilterHelper.EscapeLikeString(search.Key, "$") + "%";
                    string key1 = key;
                    if (!leftMatch) key = "%" + key;
                    search.Where += " and ( projectname like @key escape '$' or pinyin like @key escape '$' or pinyinall like @key escape '$' )";
                    cmd.Parameters.Add(SqlHelper.GetSqlParameter("@key", key, SqlDbType.NVarChar));
                    if (!leftMatch)
                    {
                        search.Where += " and ( projectname not like @key1 escape '$' and pinyin not like @key1 escape '$' and pinyinall not like @key1 escape '$' ) ";
                        cmd.Parameters.Add(SqlHelper.GetSqlParameter("@key1", key1, SqlDbType.NVarChar));
                    }
			    }
                sql = HandleSQL(search, sql);
                cmd.CommandText = sql;
                return ExecuteToEntityList<DATProject>(cmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获得楼盘基本信息
        /// </summary>
        public static DataTable GetProjectBaseInfo(SearchBase search, int projectId)
        {
            string sql = SQL.PBFH.ProjectBaseInfo;
            SqlCommand cmd = new SqlCommand();
            string projectTable = TableByCity.projecttable;
            try
            {
                sql = sql.Replace("@table_dat_project", projectTable);
                sql = sql.Replace("@table_project", projectTable);
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@fxtcompanyid", search.FxtCompanyId, SqlDbType.Int));
                cmd.Parameters.Add(SqlHelper.GetSqlParameter("@projectid", projectId, SqlDbType.Int));
                sql = HandleSQL(search, sql);
                cmd.CommandText = sql;
                return ExecuteDataSet(cmd).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
