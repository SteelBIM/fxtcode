using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CAS.SqlExecute
{
    public class SqlExec
    {
        public const string RETURN_VALUE_PARAMETER_NAME = "@ReturnValue";

        private SqlExec() { }

        public static DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            string connectionString = SqlServerSet.Instance.FXTConnectionString;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                DataSet ds = ExecuteDataSet(cn, commandText, commandType);
                cn.Close();
                return ds;
            }
        }

        public static DataSet ExecuteDataSet(SqlConnection connection, string commandText, CommandType commandType)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            SqlCommand cmd = new SqlCommand(commandText, connection);
            cmd.CommandType = commandType;
            cmd.CommandTimeout = 120;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet("data");
            da.Fill(ds);
            connection.Close();
            return ds;
        }

        public static DataSet ExecuteDataSet(SqlCommand command)
        {
            string connectionString = SqlServerSet.Instance.FXTConnectionString;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                command.Connection = cn;
                command.CommandTimeout = 120;
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataSet ds = new DataSet("data");

                da.Fill(ds);
                cn.Close();
                return ds;
            }
        }

        public static Int32 ExecuteNonQuery(SqlCommand command, bool includeReturnValueParameter)
        {
            string connectionString = SqlServerSet.Instance.FXTConnectionString;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                command.Connection = cn;
                command.CommandTimeout = 120;
                int returnValue = command.ExecuteNonQuery();
                cn.Close();
                if (includeReturnValueParameter)
                {
                    return int.Parse(command.Parameters[RETURN_VALUE_PARAMETER_NAME].Value.ToString());
                }
                else
                {
                    return returnValue;
                }
            }
        }

        public static Int32 ExecuteNonQuery(SqlCommand command)
        {
            return ExecuteNonQuery(command, false);
        }

        public static object ExecuteScalar(string commandText, CommandType commandType)
        {
            string connectionString = SqlServerSet.Instance.FXTConnectionString;
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(commandText, cn))
            {
                cn.Open();
                cmd.CommandType = commandType;
                cmd.CommandTimeout = 120;
                object oj = cmd.ExecuteScalar();
                cn.Close();
                return oj;
            }
        }

        public static object ExecuteScalar(SqlCommand command)
        {
            string connectionString = SqlServerSet.Instance.FXTConnectionString;
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();

                command.Connection = cn;
                command.CommandTimeout = 120;
                try
                {
                    object oj = command.ExecuteScalar();
                    cn.Close();
                    return oj;
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public static SqlDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            string connectionString = SqlServerSet.Instance.FXTConnectionString;
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(commandText, cn);

            cn.Open();
            cmd.CommandType = commandType;
            cmd.CommandTimeout = 120;
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cn.Close();
            return dr;

        }

        public static SqlDataReader ExecuteReader(SqlCommand command)
        {
            string connectionString = SqlServerSet.Instance.FXTConnectionString;
            SqlConnection cn = new SqlConnection(connectionString);
            command.Connection = cn;
            command.CommandTimeout = 120;
            cn.Open();

            SqlDataReader dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            cn.Close();
            return dr;
        }

        /// <summary>
        /// 事务处理
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static int ExecuteTran(SqlCommand command)
        {
            string connectionString = SqlServerSet.Instance.FXTConnectionString;
            SqlConnection cn = new SqlConnection(connectionString);
            //   开启事务   
            SqlTransaction sqlTransaction = cn.BeginTransaction();
            //   将事务应用于Command   
            command.Connection = cn;
            command.Transaction = sqlTransaction;
            cn.Open();
            try
            {
                sqlTransaction.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                //   出错回滚   
                sqlTransaction.Rollback();
                return 0;
            }
            finally {
                cn.Close();
            }
        }

    }
}
