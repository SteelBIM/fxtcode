using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Xml;

namespace FXT.DataCenter.Infrastructure.Common.DBHelper
{
    /// <summary>
    /// 数据库的通用访问代码
    /// 此类为抽象类，不允许实例化，在应用时直接调用即可
    /// </summary>
    public class DBHelperSql
    {
        //数据库连接字符串(web.config来配置)，多数据库可使用DbHelperSQLP来实现.
        private static string connectionString = ConfigurationHelper.FxtProject;//默认数据库
        /// <summary>
        /// 数据库连接
        /// </summary>
        public static string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }
        public DBHelperSql()
        {
        }
        #region 执行操作，返回受影响的行数

        /// <summary>
        /// 执行操作，返回受影响的行数
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">执行命令需要的参数</param>
        /// <returns>返回受影响的行数</returns>
        public static int ExecuteNonQuery(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        /// 执行操作，返回受影响的行数
        /// </summary>
        /// <param name="tran">已存在的事务</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="commandParameters">执行命令需要的参数</param>
        /// <returns>返回受影响的行数</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(trans, CommandType.Text, cmdText, commandParameters);

        }

        /// <summary>
        /// 执行操作，返回受影响的行数(存储过程)
        /// </summary>
        /// <param name="cmdText">存储过程名称</param>
        /// <param name="commandParameters">执行命令需要的参数</param>
        /// <returns>返回受影响的行数</returns>
        public static int ExecuteNonQueryProc(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(CommandType.StoredProcedure, cmdText, commandParameters);
        }

        /// <summary>
        /// 执行操作，返回受影响的行数(存储过程)
        /// </summary>
        /// <param name="tran">已存在的事务</param>
        /// <param name="cmdText">存储过程名称</param>
        /// <param name="commandParameters">执行命令需要的参数</param>
        /// <returns>返回受影响的行数</returns>
        public static int ExecuteNonQueryProc(SqlTransaction trans, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(trans, CommandType.StoredProcedure, cmdText, commandParameters);
        }

        private static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);

                int val = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();

                return val;
            }
        }

        private static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);

            int val = cmd.ExecuteNonQuery();

            cmd.Parameters.Clear();

            return val;

        }

        #endregion

        #region 执行查询，返回SqlDataReader

        /// <summary>
        /// 执行查询，返回SqlDataReader
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="commandParameters">执行命令的参数集</param>
        /// <returns>一个结果集对象SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        /// 执行查询，返回SqlDataReader(存储过程)
        /// </summary>
        /// <param name="commandText">存储过程名称</param>
        /// <param name="commandParameters">执行命令的参数集</param>
        /// <returns>一个结果集对象SqlDataReader</returns>
        public static SqlDataReader ExecuteReaderProc(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(CommandType.StoredProcedure, cmdText, commandParameters);
        }

        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);

                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                cmd.Parameters.Clear();

                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        #endregion

        #region 执行操作，返回表中第一行，第一列的值

        /// <summary>
        /// 执行操作，返回表中第一行，第一列的值
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="commandParameters">执行命令的参数集</param>        
        /// <returns>返回的对象</returns>
        public static object ExecuteScalar(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        /// 执行操作，返回表中第一行，第一列的值
        /// </summary>
        /// <param name="tran">已存在的事务</param>
        /// <param name="commandText">SQL语句</param>
        /// <param name="commandParameters">执行命令的参数集</param>        
        /// <returns>返回的对象</returns>
        public static object ExecuteScalar(SqlTransaction trans, string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(trans, CommandType.Text, cmdText, commandParameters);
        }
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            // Create & open a SqlConnection, and dispose of it after we are done
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
        }
        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();

            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // Execute the command & return the results
            object retval = cmd.ExecuteScalar();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();

            if (mustCloseConnection)
                connection.Close();

            return retval;
        }
        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }
        /// <summary>
        /// 执行操作，返回表中第一行，第一列的值(存储过程)
        /// </summary>
        /// <param name="commandText">存储过程名称</param>
        /// <param name="commandParameters">执行命令的参数集</param>        
        /// <returns>返回的对象</returns>
        public static object ExecuteScalarProc(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(CommandType.StoredProcedure, cmdText, commandParameters);
        }

        private static object ExecuteScalar(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);

            object val = cmd.ExecuteScalar();

            cmd.Parameters.Clear();

            return val;
        }

        private static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);

                object val = cmd.ExecuteScalar();

                cmd.Parameters.Clear();

                return val;
            }
        }

        #endregion

        #region 执行一个命令，返回数据集或数据表

        /// <summary>
        /// 执行一个命令，返回数据表
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="commandParameters">执行命令的参数集</param>
        /// <returns>返回数据表</returns>
        public static DataTable ExecuteDataTable(string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataSet(commandText, commandParameters).Tables[0];
        }

        /// <summary>
        /// 执行一个命令，返回数据表(存储过程)
        /// </summary>
        /// <param name="commandText">存储过程名称</param>
        /// <param name="commandParameters">执行命令的参数集</param>
        /// <returns>返回数据表</returns>
        public static DataTable ExecuteDataTableProc(string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataSetProc(commandText, commandParameters).Tables[0];
        }

        /// <summary>
        /// 执行一个命令，返回数据集
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="commandParameters">执行命令的参数集</param>
        /// <returns>返回数据集</returns>
        public static DataSet ExecuteDataSet(string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataSet(CommandType.Text, commandText, commandParameters);
        }

        /// <summary>
        /// 执行一个命令，返回数据集(存储过程)
        /// </summary>
        /// <param name="commandText">存储过程名称</param>
        /// <param name="commandParameters">执行命令的参数集</param>
        /// <returns>返回数据集</returns>
        public static DataSet ExecuteDataSetProc(string commandText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataSet(CommandType.StoredProcedure, commandText, commandParameters);
        }

        public static DataSet ExecuteDataSet(CommandType cmdType, string commandText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 600;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, commandText, commandParameters);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                cmd.Parameters.Clear();
                return ds;
            }
        }

        #endregion
        /// <summary>
        /// 设置SqlCommand对象
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <param name="conn">SqlConnection 对象</param>
        /// <param name="trans">SqlTransaction 对象</param>
        /// <param name="cmdType">CommandType(执行存储过程或SQL语句)</param>
        /// <param name="cmdText">存储过程名称或SQL语句</param>
        /// <param name="cmdParms">命令中用到的参数集</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            
            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
    }
}
