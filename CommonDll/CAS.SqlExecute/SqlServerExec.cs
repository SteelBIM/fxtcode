using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CAS.SqlExecute
{
    internal class SqlServerExec
    {
        /// <summary>
        /// 单件实例。
        /// </summary>
        private static readonly SqlServerExec _sqlServerUtil = new SqlServerExec();

        
        /// <summary>
        /// 隐藏默认构造方法
        /// </summary>
        private SqlServerExec()
        {
        }
        /// <summary>
        /// EVSDataAccess中Sql处理工具实例。
        /// </summary>
        public static SqlServerExec Instance
        {
            get
            {
                lock (_sqlServerUtil)
                {
                    return _sqlServerUtil;
                }
            }
        }

        /// <summary>
        /// 对Sql语句进行分页处理。
        /// </summary>
        /// <param name="dataView">待处理的Sql语句。</param>
        /// <param name="sqlServerVersion">SqlServer版本。</param>
        /// <param name="pageIndex">分页页码。</param>
        /// <param name="dataCount">每页显示数据数量。</param>
        /// <param name="sortBy">排序规则。</param>
        /// <param name="viewName">生成子视图名称。</param>
        /// <param name="rowIndexName">生成行标记序列名称。</param>
        /// <returns>处理后的分页语句。</returns>
        public string PagingDataView(string dataView, SqlServerVersion sqlServerVersion, int pageIndex, int dataCount, string sortBy, string viewName, string rowIndexName)
        {
            if (string.IsNullOrEmpty(dataView))
            {
                return dataView;
            }
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            if (dataCount < 1)
            {
                dataCount = 1;
            }
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = string.Format("View_{0}", Guid.NewGuid().ToString().Replace("-", string.Empty));
            }
            if (string.IsNullOrEmpty(rowIndexName))
            {
                rowIndexName = string.Format("RowIndex_{0}", Guid.NewGuid().ToString().Replace("-", string.Empty));
            }
            switch (sqlServerVersion)
            {
                case SqlServerVersion.SqlServer2000:
                    {
                        return string.Format("select top {3} * from ({0}) as {1}_A where id not in (select top {4} id from ({0}) as {1}_B order by {2}) order by {2};select Count(*) as Amount from ({0}) as {1}_C"
                            , dataView
                            , viewName
                            , sortBy
                            , dataCount
                            , ((pageIndex - 1) * dataCount)
                            );
                    }
                case SqlServerVersion.SqlServer2005:
                    {
                        return string.Format("select * from (select Row_Number() over(order by {3}) as {2},* from ({0}) as {1}_A) as {1}_B where {2} between {4} and {5};select Count(*) as Amount from ({0}) as {1}_C"
                            , dataView
                            , viewName
                            , rowIndexName
                            , sortBy
                            , ((pageIndex - 1) * dataCount + 1)
                            , (pageIndex * dataCount)
                            );
                    }
                default: goto case SqlServerVersion.SqlServer2005;
            }
        }
        /// <summary>
        /// 对Sql语句进行分页处理。
        /// </summary>
        /// <param name="dataView">待处理的Sql语句。</param>
        /// <param name="sqlServerVersion">SqlServer版本。</param>
        /// <param name="pageIndex">分页页码。</param>
        /// <param name="dataCount">每页显示数据数量。</param>
        /// <param name="sortBy">排序规则。</param>
        /// <returns>处理后的分页语句。</returns>
        public string PagingDataView(string dataView, SqlServerVersion sqlServerVersion, int pageIndex, int dataCount, string sortBy)
        {
            return PagingDataView(dataView, sqlServerVersion, pageIndex, dataCount, sortBy, string.Empty, string.Empty);
        }

        /// <summary>
        /// 使用适配器按照指定参数进行数据集填充。
        /// </summary>
        /// <param name="selectSql">需要进行查询的sql字符串。</param>
        /// <param name="conn">需要进行查询的数据库连接字符串。</param>
        /// <returns>填充后的数据集</returns>
        public DataSet ExecuteDataAdapter(string selectSql, string conn)
        {
            return ExecuteDataAdapter(selectSql, conn, new SqlParameter[] { });
        }

        /// <summary>
        /// 使用适配器按照指定参数进行数据集填充。
        /// </summary>
        /// <param name="selectSql">需要进行查询的sql字符串。</param>
        /// <param name="conn">需要进行查询的数据库连接字符串。</param>
        /// <param name="sqlParameter">需要进行查询的sqlParameter参数组。</param>
        /// <returns>填充后的数据集。</returns>
        public DataSet ExecuteDataAdapter(string selectSql, string conn, SqlParameter[] sqlParameter)
        {
            return ExecuteDataAdapter(selectSql, conn, sqlParameter, CommandType.Text);
        }

        /// <summary>
        /// 使用适配器按照指定参数进行数据集填充。
        /// </summary>
        /// <param name="selectSql">需要进行查询的sql字符串。</param>
        /// <param name="conn">需要进行查询的数据库连接字符串。</param>
        /// <param name="sqlParameter">需要进行查询的sqlParameter参数组。</param>
        /// <param name="commandType">数据查询命令Command类型。</param>
        /// <returns>填充后的数据集。</returns>
        public DataSet ExecuteDataAdapter(string selectSql, string conn, SqlParameter[] sqlParameter, CommandType commandType)
        {
            SqlCommand select = new SqlCommand();
            select.CommandText = selectSql;
            select.Connection = new SqlConnection(conn);
            select.CommandType = commandType;
            if (sqlParameter != null)
            {
                foreach (SqlParameter p in sqlParameter)
                {
                    select.Parameters.Add(p);
                }
            }
            FormatSelectCommand(ref select);
            return ExecuteDataAdapter(new DataSet("data")
                , select
                , null
                , null
                , null);
        }

        /// <summary>
        /// 使用适配器按照指定参数进行数据集填充。
        /// </summary>
        /// <param name="data">待填充的数据集。</param>
        /// <param name="select">查询命令集。</param>
        /// <param name="insert">新增命令集。</param>
        /// <param name="delete">删除命令集。</param>
        /// <param name="update">修改命令集。</param>
        /// <returns>填充后的数据集。</returns>
        public DataSet ExecuteDataAdapter(DataSet data, SqlCommand select, SqlCommand insert, SqlCommand delete, SqlCommand update)
        {
            if (data == null
                || (
                    (select == null || string.IsNullOrEmpty(select.CommandText))
                    && (insert == null || string.IsNullOrEmpty(insert.CommandText))
                    && (delete == null || string.IsNullOrEmpty(delete.CommandText))
                    && (update == null || string.IsNullOrEmpty(update.CommandText))
                    )
                )
            {
                return data;
            }
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = select;
            da.InsertCommand = insert;
            da.DeleteCommand = delete;
            da.UpdateCommand = update;
            if ((insert != null && !string.IsNullOrEmpty(insert.CommandText)) || (delete != null && !string.IsNullOrEmpty(delete.CommandText)) || (update != null && !string.IsNullOrEmpty(update.CommandText)))
            {

                bool insertFlag = this.ForeachSqlCommand(ref insert);
                bool deleteFlag = this.ForeachSqlCommand(ref delete);
                bool updateFlag = this.ForeachSqlCommand(ref update);
                SqlTransaction insertTran = null;
                SqlTransaction deleteTran = null;
                SqlTransaction updateTran = null;
                if (insertFlag)
                {
                    if (insert.Connection.State == ConnectionState.Closed)
                    {
                        insert.Connection.Open();
                    }
                    insertTran = insert.Connection.BeginTransaction();
                    insert.Transaction = insertTran;
                }
                if (deleteFlag)
                {
                    if (delete.Connection.State == ConnectionState.Closed)
                    {
                        delete.Connection.Open();
                    }
                    deleteTran = delete.Connection.BeginTransaction();
                    delete.Transaction = deleteTran;
                }
                if (updateFlag)
                {
                    if (update.Connection.State == ConnectionState.Closed)
                    {
                        update.Connection.Open();
                    }
                    updateTran = update.Connection.BeginTransaction();
                    update.Transaction = updateTran;
                }
                try
                {
                    foreach (DataTable dt in data.Tables)
                    {
                        da.Update(dt);
                    }
                }
                catch (Exception ex)
                {
                    if (updateFlag && updateTran != null)
                    {
                        updateTran.Rollback();
                    }
                    if (deleteFlag && deleteTran != null)
                    {
                        deleteTran.Rollback();
                    }
                    if (insertFlag && insertTran != null)
                    {
                        insertTran.Rollback();
                    }
                    throw ex;
                }
                if (updateFlag && updateTran != null)
                {
                    updateTran.Commit();
                }
                if (deleteFlag && deleteTran != null)
                {
                    deleteTran.Commit();
                }
                if (insertFlag && insertTran != null)
                {
                    insertTran.Commit();
                }
            }
            if (select != null && !string.IsNullOrEmpty(select.CommandText))
            {
                bool selectFlag = this.ForeachSqlCommand(ref select);
                SqlTransaction selectTran = null;
                if (selectFlag)
                {
                    if (select.Connection.State == ConnectionState.Closed)
                    {
                        select.Connection.Open();
                    }
                    selectTran = select.Connection.BeginTransaction();
                    select.Transaction = selectTran;
                }
                try
                {
                    select.CommandTimeout = 600;
                    da.Fill(data);
                }
                catch (Exception ex)
                {
                    if (selectFlag && selectTran != null)
                    {
                        selectTran.Rollback();
                    }
                    throw ex;
                }
                if (selectFlag && selectTran != null)
                {
                    selectTran.Commit();
                }
            }
            return data;
        }

        /// <summary>
        /// 重新整理SqlCommand，并且验证是否存在事务参数。
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="sql"></param>
        public bool ForeachSqlCommand(ref SqlCommand comm)
        {
            if (comm == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(comm.CommandText))
            {
                comm.CommandText = string.Empty;
            }
            bool tranFlag = false;
            bool[] logArgsFlag = new bool[] { false, false, false, false, false };

            StringBuilder parametersString = new StringBuilder();
            foreach (SqlParameter p in comm.Parameters)
            {
                if (p.Value == null)
                {
                    p.Value = DBNull.Value;
                }
                if (p.ParameterName.Equals("@Transaction") && (p.Value.Equals(true) || p.Value.ToString().ToLower().Equals("true")))
                {
                    tranFlag = true;
                }
                if ("@LogUserId".Equals(p.ParameterName))
                {
                    logArgsFlag[0] = true;
                }
                if ("@LogCityId".Equals(p.ParameterName))
                {
                    logArgsFlag[1] = true;
                }
                if ("@LogModuleName".Equals(p.ParameterName))
                {
                    logArgsFlag[2] = true;
                }
                if ("@LogOperateType".Equals(p.ParameterName))
                {
                    logArgsFlag[3] = true;
                }
                if ("@LogSerialNumber".Equals(p.ParameterName))
                {
                    logArgsFlag[4] = true;
                }
                if (parametersString.Length > 0)
                {
                    parametersString.Append("&");
                }
                parametersString.Append(string.Format("{0}={1}", p.ParameterName, p.Value.ToString().Replace("&", "&amp;")));
            }
            bool logFlag = logArgsFlag[0] && logArgsFlag[1] && logArgsFlag[2] && logArgsFlag[3] && logArgsFlag[4];
            //日志记录
            //if (logFlag)
            //{
            //    _dataAccessEntity.LogUserActionAdd(
            //        comm.Parameters["@LogUserId"].Value.ToString()
            //        , comm.Parameters["@LogCityId"].Value.ToString()
            //        , comm.Parameters["@LogModuleName"].Value.ToString()
            //        , comm.CommandText
            //        , parametersString.ToString()
            //        , comm.Parameters["@LogOperateType"].Value.ToString()
            //        , comm.Parameters["@LogSerialNumber"].Value.ToString()
            //        );
            //}
            return tranFlag;
        }

        /// <summary>
        /// 重新整理SelectCommand。
        /// </summary>
        /// <param name="selectCommand"></param>
        /// <param name="sql"></param>
        public void FormatSelectCommand(ref SqlCommand selectCommand)
        {
            if (selectCommand == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(selectCommand.CommandText))
            {
                selectCommand.CommandText = string.Empty;
            }
            bool[] pagingArgsFlag = new bool[] { false, false, false };
            foreach (SqlParameter p in selectCommand.Parameters)
            {
                if (p.Value == null)
                {
                    p.Value = DBNull.Value;
                }
                if ("@PageIndex".Equals(p.ParameterName))
                {
                    pagingArgsFlag[0] = true;
                }
                if ("@DataCount".Equals(p.ParameterName))
                {
                    pagingArgsFlag[1] = true;
                }
                if ("@SortBy".Equals(p.ParameterName))
                {
                    pagingArgsFlag[2] = true;
                }
            }
            bool pagingFlag = pagingArgsFlag[0] && pagingArgsFlag[1] && pagingArgsFlag[2];
            SqlServerVersion sv = SqlServerVersion.SqlServer2005;
            if (selectCommand.CommandType == CommandType.Text && selectCommand.Connection != null && pagingFlag)
            {
                if (SqlServerSet.Instance.FXTConnectionString.Equals(selectCommand.Connection.ConnectionString))
                {
                    sv = SqlServerSet.Instance.FXTDataBaseVersion;
                }
                else if (SqlServerSet.Instance.TestConnectionString.Equals(selectCommand.Connection.ConnectionString))
                {
                    sv = SqlServerSet.Instance.TestDataBaseVersion;
                }                
                selectCommand.CommandText = this.PagingDataView(selectCommand.CommandText
                      , sv
                      , Convert.ToInt32(selectCommand.Parameters["@PageIndex"].Value.ToString())
                      , Convert.ToInt32(selectCommand.Parameters["@DataCount"].Value.ToString())
                      , selectCommand.Parameters["@SortBy"].Value.ToString()
                      );
            }
        }

        /// <summary>
        /// 格式化数据库参数值中的特殊符号。escape '\'
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FormatSqlParameterValue(string value)
        {
            return string.Format("%{0}%", new StringBuilder(value).Replace(@"\", @"\\").Replace("[", @"\[").Replace("]", @"\]").Replace(" ", "%"));
        }

        /// <summary>
        /// 转整型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int StringToInt(string str)
        {
            int r = 0;
            if (str.Trim() != "")
            {
                try
                {
                    r = Convert.ToInt32(str.Trim());
                }
                catch (Exception e)
                {
                    return 0;
                }
            }

            return r;
        }
    }
}
