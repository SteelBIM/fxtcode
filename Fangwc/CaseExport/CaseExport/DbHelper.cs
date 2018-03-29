using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseExport
{
    class DbHelper
    {

        public static DataTable QueryTable(string sql)
        {
            ConnectionStringSettings connSettings = ConfigurationManager.ConnectionStrings["FxtData_Case"];
            DbProviderFactory factory = null;
            DbConnection conn = null;
            DbCommand command = null;
            try
            {
                factory = DbProviderFactories.GetFactory(connSettings.ProviderName);
                conn = factory.CreateConnection();
                conn.ConnectionString = connSettings.ConnectionString;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                #region read table
                //读取100条
                command.CommandText = sql;
                command.CommandTimeout = 200;
                using (DbDataAdapter reader = factory.CreateDataAdapter())
                {
                    reader.SelectCommand = command;
                    DataSet ds = new DataSet();
                    reader.Fill(ds);
                    reader.Dispose();
                    return ds.Tables[0];
                }
                #endregion
            }
            finally
            {
                #region dispose
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
                if (conn != null)
                {
                    conn.Close();
                    conn = null;
                }
                #endregion
            }
        }

        public static TableQueryResponseModel PageQueryTable(TableQueryCriteria queryCriteria, TableQueryResponseModel queryResult)
        {
            if (queryCriteria.PageNumber < 1)
            {
                queryCriteria.PageNumber = 1;
            }
            queryResult.PageNumber = queryCriteria.PageNumber;
            queryResult.PageSize = ConfigHelper.PageSize;
            
            var countSql = CountSqlBuilder(queryCriteria);
            var pageSql = PageSqlBuilder(queryCriteria);

            #region query code

            #region db server
            string serverName = queryCriteria.DBServer;
            string dbName = queryCriteria.DBName;
            var dbConnList = ConfigHelper.GetConnectionStrings();
            if (dbConnList == null || !dbConnList.ContainsKey(serverName))
            {
                throw new Exception("connectionStrings不存在的数据库服务器名称:" + serverName);
            }
            ConnectionStringSettings connSettings = dbConnList[serverName];
            SqlConnectionStringBuilder cnBuilder = new SqlConnectionStringBuilder(connSettings.ConnectionString);
            if (cnBuilder.InitialCatalog != dbName)
            {
                cnBuilder.InitialCatalog = dbName;
            }
            #endregion

            DbProviderFactory factory = null;
            DbConnection conn = null;
            DbCommand command = null;
            try
            {
                factory = DbProviderFactories.GetFactory(connSettings.ProviderName);
                conn = factory.CreateConnection();
                conn.ConnectionString = cnBuilder.ConnectionString;
                conn.Open();
                command = conn.CreateCommand();
                command.CommandType = CommandType.Text;
                #region read table
                //读取总记录数
                command.CommandText = countSql;
                queryResult.TotalCount = (int)command.ExecuteScalar();
                //分页读取
                command.CommandText = pageSql;
                using (DbDataAdapter reader = factory.CreateDataAdapter())
                {
                    reader.SelectCommand = command;
                    DataSet ds = new DataSet();
                    reader.Fill(ds);
                    reader.Dispose();
                    queryResult.Result = ds.Tables[0];
                }
                //移除rowNumber列
                queryResult.Result.Columns.Remove("rowNumber");
                #endregion
            }
            finally
            {
                #region dispose
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
                if (conn != null)
                {
                    conn.Close();
                    conn = null;
                }
                #endregion
            }
            #endregion

            return queryResult;
        }

        private static string PageSqlBuilder(TableQueryCriteria queryCriteria)
        {
            string colNames = "*";
            int startIndex = (queryCriteria.PageNumber - 1) * ConfigHelper.PageSize + 1;
            int endIndex = queryCriteria.PageNumber * ConfigHelper.PageSize;
            
            string strWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(queryCriteria.Where))
            {
                strWhere = "Where " + queryCriteria.Where;
            }
            string sql = string.Format("Select {0} From (Select {0}, row_number() over(Order By [{1}]) as rowNumber From [{2}].[dbo].[{3}] {4}) XCode_T1 Where rowNumber Between {5} And {6}", colNames, queryCriteria.DefaultOrderByColumn, queryCriteria.DBName, queryCriteria.TableName, strWhere, startIndex, endIndex);
            return sql;
        }

        private static string CountSqlBuilder(TableQueryCriteria queryCriteria)
        {
            string strWhere = string.Empty;
            if (!string.IsNullOrWhiteSpace(queryCriteria.Where))
            {
                strWhere = "Where " + queryCriteria.Where;
            }
            string sql = string.Format("Select Count(*) From [{0}].[dbo].[{1}] {2}", queryCriteria.DBName, queryCriteria.TableName, strWhere);
            return sql;
        }


    }
}
