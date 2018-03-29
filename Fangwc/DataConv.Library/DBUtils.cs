using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataConv.Library
{
    /// <summary>
    /// 数据库接口工具类
    /// </summary>
    public class DBUtils
    {

        /// <summary>
        /// 根据数据源和SQL获取对应数据
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable getData(string datasource, string sql)
        {
            DataTable dt_tmp = new DataTable();
            using (SqlConnection conn = new SqlConnection(datasource))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlDataAdapter sda = new SqlDataAdapter(sql, conn);
                sda.Fill(dt_tmp);
                sda.Dispose();
            }
            return dt_tmp;
        }

        /// <summary>
        /// 执行SQL，返回SqlDataReader
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SqlDataReader getReader(string datasource, string sql)
        {
            SqlDataReader reader = null;
            using (SqlConnection conn = new SqlConnection(datasource))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(sql, conn);
                reader = cmd.ExecuteReader();
            }
            return reader;
        }

        /// <summary>
        /// 插入或更新数据
        /// insert into TableName(Col)
        /// select 1 union all
        /// select 2
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int insertData(string datasource, string sql)
        {
            int insertRowCnt = -1;
            using (SqlConnection conn = new SqlConnection(datasource))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(sql, conn);
                insertRowCnt = cmd.ExecuteNonQuery();
            }
            return insertRowCnt;
        }

        public static int DeleteData(string datasource, string sql)
        {
            int rowCnt = -1;
            using (SqlConnection conn = new SqlConnection(datasource))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(sql, conn);
                rowCnt = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            return rowCnt;
        }

    }
}
