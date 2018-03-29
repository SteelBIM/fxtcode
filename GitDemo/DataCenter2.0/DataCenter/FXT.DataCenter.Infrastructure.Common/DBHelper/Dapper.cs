using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using FXT.DataCenter.Infrastructure.Common.DBHelper;

namespace FXT.DataCenter.Infrastructure.Common.DBHelper
{
   public class DapperAdapter
    {
        private static string connectionString = ConfigurationHelper.FxtProject;//默认数据库
        public static string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public static SqlConnection OpenConnection(string connString=null)
        {
            var connection = string.IsNullOrEmpty(connString) ? new SqlConnection(connectionString) : new SqlConnection(connString);
             
            connection.Open();
            return connection;
        }
    }
}
