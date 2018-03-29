using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using OpenPlatform.Framework.Utils;

namespace OpenPlatform.Framework.Data
{
   public class Dapper
    {
        private static readonly string ConnectionString = ConfigurationHelper.ConnString4Bona;//默认数据库
        //private static readonly string FxtSurveyConnString = ConfigurationHelper.FxtSurveyConnString;//默认数据库

        public static MySqlConnection MySqlConnection(string connString = null)
        {
            var connection = string.IsNullOrEmpty(connString) ? new MySqlConnection(ConnectionString) : new MySqlConnection(connString);

            connection.Open();
            return connection;
        }

        //public static SqlConnection SqlConnection(string connString = null)
        //{
        //    var connection = string.IsNullOrEmpty(connString) ? new SqlConnection(FxtSurveyConnString) : new SqlConnection(connString);

        //    connection.Open();
        //    return connection;
        //}
    }
}
