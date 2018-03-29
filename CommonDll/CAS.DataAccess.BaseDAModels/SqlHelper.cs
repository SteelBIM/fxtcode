using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CAS.Entity.BaseDAModels;

namespace CAS.DataAccess.BaseDAModels
{
    /// <summary>
    /// A wrapper class to encapsulate a sqlconnection and a sqltransaction
    /// </summary>
    public class SqlTransConn : IDisposable
    {
        private SqlConnection _conn = null;

        public SqlConnection Conn
        {
            get { return _conn; }
            set { _conn = value; }
        }

        private SqlTransaction _trans = null;

        public SqlTransaction Trans
        {
            get { return _trans; }
            set { _trans = value; }
        }

        private int _transCount = 0;

        public SqlTransConn(string connectionString)
        {
            _conn = new SqlConnection(connectionString);
            _conn.Open();
        }

        public SqlTransConn Commit()
        {
            _transCount--;
            if (_transCount == 0)
            {
                _trans.Commit();
            }
            return this;
        }

        public SqlTransConn Rollback()
        {
            if (_trans != null && _transCount > 0)
            {
                _trans.Rollback();
                _trans = null;
                _transCount = 0;
            }
            return this;
        }

        public SqlTransConn BeginTransaction()
        {
            if (_transCount == 0)
            {
                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                _trans = _conn.BeginTransaction();
            }
            _transCount++;
            return this;
        }

        public SqlTransConn Close()
        {
            // do not close if transaction is still going on
            if (_transCount > 0) return this;

            if (_conn.State == ConnectionState.Open)
            {
                _conn.Close();
            }
            return this;
        }

        public SqlCommand CreateCommand()
        {
            SqlCommand cmd = _conn.CreateCommand();
            cmd.CommandTimeout = 120;
            if (_trans != null) cmd.Transaction = _trans;
            return cmd;
        }

        public bool IsInTransaction()
        {
            return (_transCount > 0);
        }

        /// <summary>
        /// if this object is disposed in memory, close the underlining connection
        /// </summary>
        public void Dispose()
        {
            if (_conn != null && _conn.State == ConnectionState.Open)
            {
                _conn.Dispose();
            }
        }
    }

    /// <summary>
    /// utility classes related to Sql
    /// </summary>
    public static class SqlHelper
    {
        public static List<T> ExecuteReader<T>(SqlTransConn transConn, CommandType cmdType, string cmdText, List<SqlParameter> parameters) where T : BaseTO, new()
        {
            List<T> list = new List<T>();
            try
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(transConn, cmdType, cmdText, parameters))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            T to = new T();
                            to.Initialize(reader);
                            list.Add(to);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to execute sql:" + cmdText, ex);
            }
            finally
            {
                transConn.Close();
            }

            return list;
        }


        public static SqlDataReader ExecuteReader(SqlTransConn transConn, CommandType cmdType, string cmdText, List<SqlParameter> parameters)
        {
            SqlCommand cmd = new SqlCommand();
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, transConn, cmdType, cmdText, parameters);
                SqlDataReader rdr = cmd.ExecuteReader(!transConn.IsInTransaction() ? CommandBehavior.CloseConnection : CommandBehavior.Default);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// don't clear parameters for outputparameters  
        /// </summary>
        /// <param name="transConn"></param>
        /// <param name="cmdType"></param>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <param name="clear"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(SqlTransConn transConn, CommandType cmdType, string cmdText, List<SqlParameter> parameters,bool clear)
        {
            SqlCommand cmd = new SqlCommand();
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, transConn, cmdType, cmdText, parameters);
                SqlDataReader rdr = cmd.ExecuteReader(!transConn.IsInTransaction() ? CommandBehavior.CloseConnection : CommandBehavior.Default);
                if (clear)
                    cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int ExecuteNonQuery(SqlTransConn transConn, CommandType cmdType, string cmdText, List<SqlParameter> parameters)
        {

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, transConn, cmdType, cmdText, parameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }


        public static object ExecuteScalar(SqlTransConn transConn, CommandType cmdType, string cmdText, List<SqlParameter> parameters)
        {
            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, transConn, cmdType, cmdText, parameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="transConn">SqlTransaction/connection object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(SqlCommand cmd, SqlTransConn transConn, CommandType cmdType, string cmdText, List<SqlParameter> parameters)
        {
            SqlConnection conn = transConn.Conn;

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (transConn.Trans != null)
                cmd.Transaction = transConn.Trans;

            cmd.CommandType = cmdType;
            //设置超时时间
            cmd.CommandTimeout = 120;

            if (parameters != null)
            {
                foreach (SqlParameter parm in parameters)
                    cmd.Parameters.Add(parm);
            }
        }

        public static SqlParameter GetSqlParameter(string key, object value, SqlDbType type)
        {
            SqlParameter param = new SqlParameter(key, type);
            param.Value = value;
            return param;
        }
        public static SqlParameter GetSqlParameter(string key, object value, SqlDbType type, int size)
        {
            SqlParameter param = new SqlParameter(key, type, size);
            param.Value = value;
            return param;
        }
    }
}
