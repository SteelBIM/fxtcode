using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using CAS.Entity.BaseDAModels;

namespace CAS.DataAccess.BaseDAModels
{
    public class BaseDAHelper<T> where T : CAS.Entity.BaseDAModels.BaseTO, new()
    {
        //public static Logger m_log = LogManager.GetCurrentClassLogger();

        //Use generic dictionary instead HttpContext to store SqlTransConn. By Norman.Chen 2012/05/11
        Dictionary<string, SqlTransConn> transConnCollection;
        public BaseDAHelper()
        {
            transConnCollection = new Dictionary<string, SqlTransConn>();
        }

        public BaseDAHelper(string connectionName)
        {
            this.ConnectionName = connectionName;
        }

        protected delegate void FilterDelegate(string value, SqlBuilder builder);

        /// <summary>
        /// A Dictionary Collections of FilterDelegate type delegate
        /// </summary>
        protected Dictionary<string, FilterDelegate> FilterMap;

        /// <summary>
        /// Connection Name: correspond to the name of the connection string inside web.config
        /// </summary>
        protected string ConnectionName = null;

        ///// <summary>
        ///// get connection based on the connectionName at DAO level
        ///// </summary>
        ///// <returns></returns>
        //protected SqlTransConn GetConnection()
        //{
        //    return new SqlTransConn(ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString);
        //}

        /// <summary>
        /// create a new connection (to avoid connection sharing issue) if transaction was not started
        /// return a shared connection (with transaction on) if transaction was started
        /// </summary>
        protected SqlTransConn GetConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;

            SqlTransConn transConn = transConnCollection.ContainsKey("TransConn_" + connHashKey) ? null : transConnCollection["TransConn_" + connHashKey];
            if (transConn == null)
            {
                transConn = new SqlTransConn(connStr);
            }
            else
            {
                if (transConn.Conn.State != ConnectionState.Open)
                {
                    transConnCollection.Remove("TransConn_" + connHashKey);
                    transConn = new SqlTransConn(connStr);
                }
            }

            return transConn;
        }


        /// <summary>
        /// get connection with the input connectionName
        /// shared connection is not suppored
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns></returns>
        protected static SqlTransConn GetConnection(string connectionName)
        {
            return new SqlTransConn(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString);
        }


        /// <summary>
        /// a short version to uniquely identify a DB
        /// </summary>
        private string _connHashKey = null;

        private static string hashConnectionString(string connectionStr)
        {
            return new SqlConnection(connectionStr).Database.ToLower();
        }

        protected string connHashKey
        {
            get
            {
                if (_connHashKey == null) _connHashKey = hashConnectionString(ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString);
                return _connHashKey;
            }
            set
            {
                _connHashKey = value; // only used in case connStr is not a real database
            }
        }


        /// <summary>
        /// Use this if transaction needs to be used ACROSS different DAOs with same DB
        /// Auto get connection if not already done
        /// </summary>
        public SqlTransConn StartSharedConnection()
        {
            SqlTransConn transConn = GetConnection();
            transConnCollection.Add("TransConn_" + connHashKey, transConn);

            return transConn;
        }

        /// <summary>
        /// Close and abort any transaction
        /// </summary>
        public void CloseSharedConnection()
        {
            SqlTransConn transConn = transConnCollection["TransConn_" + connHashKey];
            if (transConn != null)
            {
                transConn.Close();
                transConnCollection.Remove("TransConn_" + connHashKey);
            }
        }

        /// <summary>
        /// Use this BeginTransaction if transaction needs to be used ACROSS different DAOs with same DB
        /// Must use BaseDAO's Commit()/Rollback()
        /// Auto get connection if not already done
        /// </summary>
        public void BeginTransaction()
        {
            SqlTransConn transConn = transConnCollection["TransConn_" + connHashKey];
            transConn.BeginTransaction();
        }

        public void Commit()
        {
            SqlTransConn transConn = transConnCollection["TransConn_" + connHashKey];

            transConn.Commit();
            if (!transConn.IsInTransaction()) transConnCollection.Remove("TransConn_" + connHashKey);

        }

        public void Rollback()
        {
            SqlTransConn transConn = transConnCollection["TransConn_" + connHashKey];

            transConn.Rollback();
            transConnCollection.Remove("TransConn_" + connHashKey);
        }



        public string TableAlias = null;
        public string TableName = null;

        private string _primaryKey = null; // field name of the primary key 

        /// <summary>
        /// Default to TableName + Id.
        /// Set if different from default
        /// </summary>
        protected string PrimaryKey
        {
            get
            {
                return _primaryKey ?? (TableName + "Id");
            }
            set
            {
                _primaryKey = value;
            }
        }

        /// <summary>
        /// mapping of select "alias" into corresponding subqueries / selects
        /// e.g. {"Id", "TableId"}
        /// </summary>
        protected Dictionary<string, string> SelectMap;

        protected Dictionary<string, string> UpdateMap;

        protected Dictionary<string, string> UpdateListMap;
        /// <summary>
        /// mapping of alias of storeproc name to real name on DB
        /// </summary>
        protected Dictionary<string, Query.StoreProcType> StoreProcMap;

        /// <summary>
        /// indicate whether to use param binding for joining and where clauses inside sqlbuilding
        /// </summary>
        private bool internalBinding = false;

        protected void SetInternalBinding(bool onoff)
        {
            internalBinding = onoff;
        }

        public List<T> GetItems(Query query)
        {
            if (string.IsNullOrEmpty(query.Procedure))
            {
                return _findAllFromBuilder(query);
            }
            else
            {
                return _findAllFromStoreProc(query);
            }
        }

        public List<T> GetItems(Query query, out int total)
        {
            if (string.IsNullOrEmpty(query.Procedure))
            {

                total = GetTotal(query);
                if (query.IsTotalOnly)
                {
                    return new List<T>();
                }
                else
                {
                    return _findAllFromBuilder(query);
                }
            }
            else
            {
                throw new Exception("Total not supported via Store Proc interface");
                //total = 0;
                //return _findAllFromStoreProc(query);
            }
        }

        public T GetItemByPK(int id)
        {
            return GetItemByPK(id.ToString());

        }

        public T GetItemByPK(string pk)
        {
            Query query = new Query();
            query.Wheres.AddPair(PrimaryKey, pk);
            return GetItem(query);
        }

        public T GetItem(Query query)
        {
            query.Limit = 1;
            List<T> list = GetItems(query);
            T to = null;
            if (list.Count == 1)
            {
                to = list[0] as T;

            }
            return to;
        }

        /// <summary>
        /// </summary>
        /// <param name="to"></param>
        /// <param name="query"></param>
        public virtual void LateBinding(ref T to, Query query) { }

        public string GetSQLFromQuery(Query query, out List<SqlParameter> parameters)
        {
            SqlBuilder builder = InitSqlBuilder(query);
            String sql = builder.Build();
            parameters = builder.GetSqlParams();
            return sql;
        }

        private List<T> _findAllFromBuilder(Query query)
        {


            List<T> list = new List<T>();
            SqlBuilder builder;
            String sql;
            try
            {
                builder = InitSqlBuilder(query);
                sql = builder.Build();
            }
            catch (ExceptionEmptyQuery)
            {
                return new List<T>();
            }

            SqlTransConn transConn = GetConnection();
            try
            {
                using (SqlCommand command = transConn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    List<SqlParameter> parameters = builder.GetSqlParams();

                    foreach (SqlParameter parm in parameters)
                    {
                        command.Parameters.Add(parm);
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                T to = new T();
                                to.Initialize(reader);
                                LateBinding(ref to, query);
                                list.Add(to);
                            }
                        }
                    }
                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(sql, ex);
            }
            finally
            {
                transConn.Close();

            }

            return list;
        }

        private List<T> _findAllFromStoreProc(Query query)
        {

            List<T> list = new List<T>();



            List<SqlParameter> parameters = new List<SqlParameter>();

            string storeProcName;

            if (StoreProcMap != null && StoreProcMap.ContainsKey(query.Procedure))
            {
                Query.StoreProcType storeproc = this.StoreProcMap[query.Procedure];
                storeProcName = storeproc.Name;

                foreach (string key in storeproc.Parameters.Keys)
                {
                    // use parameter type from storeproc definition
                    SqlParameter parameter = new SqlParameter(key, storeproc.Parameters[key]);
                    parameter.Value = query.Parameters[key].Value;
                    parameters.Add(parameter);
                }
            }
            else
            {
                storeProcName = query.Procedure;
                // generic, no mapping
                // rely on input parameter type
                foreach (string key in query.Parameters.Keys)
                {
                    SqlParameter parameter = new SqlParameter(key, query.Parameters[key].Type);
                    parameter.Value = query.Parameters[key].Value;
                    parameters.Add(parameter);
                }
            }





            SqlTransConn transConn = GetConnection();

            try
            {
                using (SqlDataReader reader = SqlHelper.ExecuteReader(transConn, CommandType.StoredProcedure, storeProcName, parameters))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            T to = new T();
                            to.Initialize(reader);
                            LateBinding(ref to, query);
                            list.Add(to);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to execute store proc:" + storeProcName, ex);
            }
            finally
            {
                transConn.Close();
            }

            return list;
        }

        /// <summary>
        /// Get the count(*) of the SQL built from the filter, without the limit/paging
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual int GetTotal(Query query)
        {
            SetInternalBinding(true);
            int total = 0;
            SqlBuilder builder;

            if (query != null)
            {
                try
                {
                    builder = InitSqlBuilder(query);
                }
                catch (ExceptionEmptyQuery)
                {
                    return 0;
                }
                catch (ExceptionBadFilterSequence)
                {
                    // return empty list
                    return 0;
                }


            }
            else
            {
                throw new Exception("Not supported");
            }

            string sql = builder.BuildCountSql();

            SqlTransConn transConn = GetConnection();

            try
            {
                using (SqlCommand command = transConn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    List<SqlParameter> parameters = builder.GetSqlParams();

                    foreach (SqlParameter param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                    total = (int)command.ExecuteScalar();
                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(sql, ex);
            }
            finally
            {
                transConn.Close();
            }

            // set this so that the value can be used to determine IsValidRange of limit/offset/page
            query.Total = total;

            return total;
        }


        protected SqlBuilder InitSqlBuilder(Query query)
        {

            SqlBuilder builder = new SqlBuilder();
            builder.SetInternalBinding(internalBinding);

            builder.FromTableName = TableName ?? query.TableName;
            builder.FromTableAlias = TableAlias ?? String.Empty;

            builder.Limit = query.Limit;
            builder.Offset = query.Offset;
            builder.Page = query.Page;
            builder.HasGroupBys = query.HasGroupBys;
            builder.UnionSqls = query.UnionSqls;
            if (query.UnionParameters != null && query.UnionParameters.Count > 0)
            {
                foreach (SqlParameter param in query.UnionParameters)
                {
                    builder.AddToBindParams(param.ParameterName, param.Value, param.SqlDbType);
                }
            }

            Dictionary<string, bool> uniqueParameters = new Dictionary<string, bool>();

            for (int i = 0; i < query.Selects.Count; i++)
            {
                Query.SelectItem item = query.Selects[i];

                string key = item.Value;
                if (key.StartsWith("("))
                {
                    // use method-base subquery
                    // this is for more complex subqueries
                    // parameters are passed to handler. up to handler to addtobindparams
                    builder.Selects.Add(SubQuery(key, item.Parameters, builder));
                }
                else
                {
                    // stand subquery. Simple text only
                    string alias;

                    if (SelectMap.ContainsKey(key))
                    {
                        alias = SelectMap[key];
                    }
                    else
                    {
                        alias = key;
                    }

                    bool useFormat = alias.Contains("{");
                    object[] formatInput = null;

                    if (item.Parameters != null && item.Parameters.Length > 0)
                    {
                        if (useFormat) formatInput = new object[item.Parameters.Length];

                        var j = 0;
                        foreach (string paramName in item.Parameters)
                        {
                            if (uniqueParameters.ContainsKey(paramName)) continue;
                            uniqueParameters.Add(paramName, true);
                            Query.ParameterItem param = query.Parameters[paramName.Trim()];

                            builder.AddToBindParams(paramName.Trim(), param.Value, param.Type);

                            if (useFormat)
                            {
                                switch (param.Type)
                                {
                                    case SqlDbType.Int:
                                        formatInput[j] = int.Parse(param.Value);
                                        break;
                                    case SqlDbType.Float:
                                        formatInput[j] = double.Parse(param.Value);
                                        break;
                                    case SqlDbType.NVarChar:
                                        formatInput[j] = param.Value;
                                        break;
                                    default:
                                        throw new Exception("Unsupported parameter type");
                                }
                            }

                            j++;
                        }
                    }
                    builder.Selects.Add(useFormat ? string.Format(alias, formatInput) : alias);
                }
            }



            foreach (KeyValuePair<string, string> item in query.Ands)
            {
                if (FilterMap.ContainsKey(item.Key))
                {
                    FilterMap[item.Key](item.Value, builder);
                }
                else
                {
                    throw new Exception("invalid query key: " + item.Key);
                }

            }

            foreach (Query.SqlWhereItem item in query.Wheres.Items)
            {
                builder.Wheres.Add(item);
                if (item.Parameters != null)
                {
                    foreach (string paramName in item.Parameters)
                    {
                        Query.ParameterItem param = query.Parameters[paramName.Trim()];
                        builder.AddToBindParams(paramName.Trim(), param.Value, param.Type);
                    }
                }
            }
            // merge with query's where parts if any

            if (builder.HasGroupBys)
            {
                BuildGroupBys(query.GroupBys, builder);
            }

            BuildOrderBys(query.OrderBys, builder);
            return builder;

        }

        protected virtual string SubQuery(string field, string[] parameters, SqlBuilder builder)
        {
            throw new Exception("SubQuery not implemented");
        }

        protected virtual void BuildGroupBys(List<string> groupbys, SqlBuilder builder)
        {
            for (var i = 0; i < groupbys.Count; i++)
            {
                BuilderGroupBy(groupbys[i], builder);
            }
        }

        protected void BuilderGroupBy(string groupby, SqlBuilder builder)
        {
            if (!string.IsNullOrEmpty(groupby))
            {
                builder.addGroupByItem(null, groupby);
            }
        }

        /// <summary>
        /// If the DAO has special order criteria, you can override this method to
        /// handle the Query's OrderBy 
        /// </summary>
        /// <param name="orderbys">List of orderby strings</param>
        protected virtual void BuildOrderBys(List<string> orderbys, SqlBuilder builder)
        {
            for (var i = 0; i < orderbys.Count; i++)
            {
                BuildOrderBy(orderbys[i], builder);
            }
        }

        /// <summary>
        /// Generic handler for orderby string
        /// </summary>
        /// <param name="orderby">the orderby string</param>
        protected void BuildOrderBy(string orderby, SqlBuilder builder)
        {
            orderby = orderby.ToLower();

            SqlOrderByType type = SqlOrderByType.ASC;

            if (orderby == "random")
            {
                type = SqlOrderByType.RANDOM;
            }
            else
            {
                int space = orderby.LastIndexOf(' ');
                if (space > -1)
                {
                    string dirType = orderby.Substring(space + 1);
                    orderby = orderby.Substring(0, space);
                    if (dirType == "desc")
                    {
                        type = SqlOrderByType.DESC;
                    }
                    // default to ASC
                }
            }
            builder.AddOrderByItem(null, orderby, type);
        }


        #region UpDate ADD DELETE
        /// <summary>
        /// For UpDate or Add or Delete 
        /// Return Void
        /// ADD BY:Peter
        /// </summary>
        /// <param name="query"></param>
        public void UpDateOrAddOrDeleteFromDB(Query query)
        {
            if (!string.IsNullOrEmpty(query.Procedure))
            {
                _updateOrAddOrDeleteDBFromStoreProc(query);
            }
            else
            {
                _updateOrAddOrDeleteDBFromBuilder(query);
            }
        }

        private void _updateOrAddOrDeleteDBFromStoreProc(Query query)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            string storeProcName;

            if (StoreProcMap != null && StoreProcMap.ContainsKey(query.Procedure))
            {
                Query.StoreProcType storeproc = this.StoreProcMap[query.Procedure];
                storeProcName = storeproc.Name;

                foreach (string key in storeproc.Parameters.Keys)
                {
                    // use parameter type from storeproc definition
                    SqlParameter parameter = new SqlParameter(key, storeproc.Parameters[key]);
                    parameter.Value = query.Parameters[key].Value;
                    parameters.Add(parameter);
                }
            }
            else
            {
                storeProcName = query.Procedure;
                // generic, no mapping
                // rely on input parameter type
                foreach (string key in query.Parameters.Keys)
                {
                    SqlParameter parameter = new SqlParameter(key, query.Parameters[key].Type);
                    parameter.Value = query.Parameters[key].Value;
                    parameters.Add(parameter);
                }
            }

            SqlTransConn transConn = GetConnection();

            try
            {
                SqlHelper.ExecuteNonQuery(transConn, CommandType.StoredProcedure, storeProcName, parameters);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to execute store proc:" + storeProcName, ex);
            }
            finally
            {
                transConn.Close();
            }
        }

        // does not work
        //        public int Delete(Query query)
        //        {
        //            SqlBuilder builder = InitSqlBuilder(query);
        //            String sql = builder.BuildDelete();
        //            int result;
        //#if DEBUG
        //            m_log.Error(sql);
        //#endif
        //            SqlTransConn transConn = GetConnection();

        //            try
        //            {
        //                using (SqlCommand command = transConn.CreateCommand())
        //                {
        //                    command.CommandText = sql;
        //                    command.CommandType = CommandType.Text;
        //                    List<SqlParameter> parameters = builder.GetSqlParams();

        //                    foreach (SqlParameter parm in parameters)
        //                        command.Parameters.Add(parm);

        //                    result = command.ExecuteNonQuery();

        //                    command.Parameters.Clear();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception(sql, ex);
        //            }
        //            finally
        //            {
        //                transConn.Close();
        //            }

        //            return result;
        //        }



        private void _updateOrAddOrDeleteDBFromBuilder(Query query)
        {
            SqlBuilder builder = InitSqlBuilder(query);
            String sql = builder.Build();
            SqlTransConn transConn = GetConnection();

            try
            {
                using (SqlCommand command = transConn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    List<SqlParameter> parameters = builder.GetSqlParams();

                    foreach (SqlParameter parm in parameters)
                    {
                        command.Parameters.Add(parm);
                    }

                    command.ExecuteNonQuery();

                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(sql, ex);
            }
            finally
            {
                transConn.Close();
            }
        }
        /// <summary>
        /// add by peter
        /// date 2010-11-18
        /// </summary>
        /// <param name="v_sQueryString"></param>
        /// <param name="v_oSqlParams"></param>
        public void ExecuteQueryString(string v_sQueryString, SqlParameter[] v_oSqlParams)
        {
            SqlTransConn transConn = GetConnection();

            try
            {
                using (SqlCommand command = transConn.CreateCommand())
                {
                    command.CommandText = v_sQueryString;
                    command.CommandType = CommandType.Text;

                    foreach (SqlParameter parm in v_oSqlParams)
                    {
                        command.Parameters.Add(parm);
                    }
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(v_sQueryString, ex);
            }
            finally
            {
                transConn.Close();
            }
        }

        public void ExecuteQueryString(string v_sQueryString, SqlParameter[] v_oSqlParams, string key)
        {
            SqlTransConn transConn = GetConnection();

            if (!string.IsNullOrEmpty(key))
            {
                if (this.UpdateMap.ContainsKey(key))
                {
                    v_sQueryString = this.UpdateMap[key];
                }
            }


            try
            {
                using (SqlCommand command = transConn.CreateCommand())
                {
                    command.CommandText = v_sQueryString;
                    command.CommandType = CommandType.Text;

                    foreach (SqlParameter parm in v_oSqlParams)
                    {
                        command.Parameters.Add(parm);
                    }

                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(v_sQueryString, ex);
            }
            finally
            {
                transConn.Close();
            }
        }

        public int ExecuteQueryStringAndReturnId(string v_sQueryString, SqlParameter[] v_oSqlParams)
        {
            int nowid = 0;
            DataSet ds = new DataSet();
            SqlDataAdapter dap = new SqlDataAdapter();
            SqlTransConn transConn = GetConnection();
            try
            {
                SqlCommand command = transConn.CreateCommand();
                command.CommandText = v_sQueryString;
                command.CommandType = CommandType.Text;

                foreach (SqlParameter parm in v_oSqlParams)
                {
                    command.Parameters.Add(parm);
                }
                dap.SelectCommand = command;
                dap.Fill(ds);

                if (ds.Tables[0].Rows[0][1].ToString() != "0")
                {
                    return 0;
                }
                else
                {
                    nowid = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                }
                return nowid;
            }
            catch (Exception ex)
            {
                throw new Exception(v_sQueryString, ex);
            }
            finally
            {
                transConn.Close();
            }

        }

        public List<T> ExecuteQueryStringAndReturn(string v_sQueryString, SqlParameter[] v_oSqlParams)
        {
            List<T> list = new List<T>();
            SqlTransConn transConn = GetConnection();
            try
            {
                using (SqlCommand command = transConn.CreateCommand())
                {
                    command.CommandText = v_sQueryString;
                    command.CommandType = CommandType.Text;
                    foreach (SqlParameter parm in v_oSqlParams)
                    {
                        command.Parameters.Add(parm);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
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
                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(v_sQueryString, ex);
            }
            finally
            {
                transConn.Close();
            }
            return list;
        }
        /// <summary>
        /// return the value of the parameter which is sqldbDirection.outPut
        /// </summary>
        /// <param name="v_sStoreProcName"></param>
        /// <param name="v_oSqlParams"></param>
        /// <returns></returns>
        public string ExecuteProcAndGetOutPut(string v_sStoreProcName, SqlParameter[] v_oSqlParams)
        {
            string v_sReturnValue = "";
            int v_iOutputIndex = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlTransConn transConn = GetConnection();

            foreach (SqlParameter pram in v_oSqlParams)
            {
                if (pram.Direction == ParameterDirection.Output || pram.Direction == ParameterDirection.ReturnValue)
                    v_iOutputIndex = parameters.Count;
                parameters.Add(pram);
            }
            try
            {
                SqlHelper.ExecuteNonQuery(transConn, CommandType.StoredProcedure, v_sStoreProcName, parameters);
                v_sReturnValue = parameters[v_iOutputIndex].Value.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to execute store proc:" + v_sStoreProcName, ex);
            }
            finally
            {
                transConn.Close();
            }

            return v_sReturnValue;
        }

        /// <summary>
        /// return the value of the parameter which is sqldbDirection.outPut
        /// </summary>
        /// <param name="v_sStoreProcName"></param>
        /// <param name="v_oSqlParams"></param>
        /// <returns></returns>
        public Q ExecuteProcAndGetOutput<Q>(Query query)
        {
            Q v_sReturnValue;

            int v_iOutputIndex = -1;
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlTransConn transConn = GetConnection();
            string storeProcName = query.Procedure;
            // generic, no mapping
            // rely on input parameter type

            int i = 0;
            foreach (string key in query.Parameters.Keys)
            {
                SqlParameter parameter = new SqlParameter(key, query.Parameters[key].Type);
                parameter.Value = query.Parameters[key].Value;
                parameter.Direction = query.Parameters[key].Direction;
                parameters.Add(parameter);
                if (parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.ReturnValue)
                    v_iOutputIndex = i;
                i++;
            }

            try
            {
                SqlHelper.ExecuteNonQuery(transConn, CommandType.StoredProcedure, storeProcName, parameters);
                if (v_iOutputIndex > -1)
                    v_sReturnValue = (Q)parameters[v_iOutputIndex].Value;
                else
                    v_sReturnValue = default(Q);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to execute store proc:" + storeProcName, ex);
            }
            finally
            {
                transConn.Close();
            }

            return v_sReturnValue;
        }

        /// <summary>
        /// return the value of the parameter which is sqldbDirection.outPut
        /// </summary>
        /// <param name="v_sStoreProcName"></param>
        /// <param name="v_oSqlParams"></param>
        /// <returns></returns>
        public int ExecuteProc(Query query)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlTransConn transConn = GetConnection();
            string storeProcName = query.Procedure;
            // generic, no mapping
            // rely on input parameter type

            foreach (string key in query.Parameters.Keys)
            {
                SqlParameter parameter = new SqlParameter(key, query.Parameters[key].Type);
                parameter.Value = query.Parameters[key].Value;
                parameter.Direction = query.Parameters[key].Direction;
                parameters.Add(parameter);
            }

            int result;
            try
            {
                result = SqlHelper.ExecuteNonQuery(transConn, CommandType.StoredProcedure, storeProcName, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to execute store proc:" + storeProcName, ex);
            }
            finally
            {
                transConn.Close();
            }

            return result;
        }

        public List<T> ExecuteProcAndGetOutput(Query query, out int outParamValue)
        {
            int v_iOutputIndex = -1;
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlTransConn transConn = GetConnection();
            string storeProcName = query.Procedure;

            int i = 0;
            foreach (string key in query.Parameters.Keys)
            {
                SqlParameter parameter = new SqlParameter(key, query.Parameters[key].Type);
                parameter.Value = query.Parameters[key].Value;
                parameter.Direction = query.Parameters[key].Direction;
                parameters.Add(parameter);
                if (parameter.Direction == ParameterDirection.Output || parameter.Direction == ParameterDirection.ReturnValue)
                    v_iOutputIndex = i;
                i++;
            }

            //List<T> result;
            List<T> list = new List<T>();
            try
            {

                using (SqlDataReader reader = SqlHelper.ExecuteReader(transConn, CommandType.StoredProcedure, storeProcName, parameters, false))
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            T to = new T();
                            to.Initialize(reader);
                            LateBinding(ref to, query);
                            list.Add(to);
                        }
                    }
                }
                //reader.Close();// Should close DataReader first so that can get parameters value.
                outParamValue = int.Parse(parameters[v_iOutputIndex].Value.ToString());
                //outParamValue = 1;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to execute store proc:" + storeProcName, ex);
            }
            finally
            {
                transConn.Close();
            }

            return list;
        }



        public bool ExecuteNonQuery(string v_sQueryString, SqlParameter[] v_oSqlParams)
        {
            bool bol = false;

            SqlTransConn transConn = GetConnection();

            try
            {

                int count = SqlHelper.ExecuteNonQuery(transConn, CommandType.Text, v_sQueryString, v_oSqlParams != null ? v_oSqlParams.ToList() : null);
                if (count > 0)
                    bol = true;

            }
            catch (Exception ex)
            {
                throw new Exception(v_sQueryString, ex);
            }
            finally
            {
                transConn.Close();
            }

            return bol;
        }

        public bool ExecuteNonQuery(string v_sQueryString, SqlParameter[] v_oSqlParams, out int count)
        {
            bool bol = false;
            count = 0;

            SqlTransConn transConn = GetConnection();

            try
            {

                count = SqlHelper.ExecuteNonQuery(transConn, CommandType.Text, v_sQueryString, v_oSqlParams != null ? v_oSqlParams.ToList() : null);
                if (count > 0)
                    bol = true;

            }
            catch (Exception ex)
            {
                throw new Exception(v_sQueryString, ex);
            }
            finally
            {
                transConn.Close();
            }

            return bol;
        }
        #endregion

        public int InsertFromTO(BaseTO to)
        {
            Dictionary<string, object> dict = to.GetSQLWriteValues();

            StringBuilder sb = new StringBuilder();
            sb.Append("Insert into ").Append(TableName).Append(" (");
            sb.Append(String.Join(", ", dict.Keys.ToArray()));
            sb.Append(") values (");
            List<SqlParameter> parameters = new List<SqlParameter>();
            int i = 0;
            foreach (string name in dict.Keys)
            {
                if (i > 0) sb.Append(", ");
                sb.Append("@").Append(name);
                parameters.Add(Query.BuilderParam(name, dict[name]));
                i++;

            }
            sb.Append(")");

            SqlTransConn transConn = GetConnection();

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(transConn, CommandType.Text, sb.ToString(), parameters);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ":" + sb.ToString(), ex);
            }
            finally
            {
                transConn.Close();
            }

            return result;
        }

        public int DeleteByPK(object PKValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Delete from ").Append(TableName);
            sb.Append(" where ").Append(PrimaryKey).Append("=@PKValue");
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@PKValue", PKValue));
            SqlTransConn transConn = GetConnection();

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(transConn, CommandType.Text, sb.ToString(), parameters);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ":" + sb.ToString(), ex);
            }
            finally
            {
                transConn.Close();
            }

            return result;
        }

        public int UpdateFromTO(BaseTO to, object PKValue)
        {
            Dictionary<string, object> dict = to.GetSQLWriteValues();

            StringBuilder sb = new StringBuilder();
            sb.Append("update ").Append(TableName).Append(" set ");
            List<SqlParameter> parameters = new List<SqlParameter>();
            int i = 0;
            foreach (string name in dict.Keys)
            {
                if (i > 0) sb.Append(", ");
                sb.Append(name).Append("=@").Append(name);
                parameters.Add(Query.BuilderParam(name, dict[name]));
                i++;

            }

            sb.Append(" where ").Append(PrimaryKey).Append("=@PKValue");
            parameters.Add(new SqlParameter("@PKValue", PKValue));


            SqlTransConn transConn = GetConnection();

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(transConn, CommandType.Text, sb.ToString(), parameters);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ":" + sb.ToString(), ex);
            }
            finally
            {
                transConn.Close();
            }

            return result;
        }

        public bool IsReadOnly()
        {
            string sql = @"SELECT is_read_only FROM sys.databases WHERE name = @name";
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlTransConn transConn = GetConnection();

            int result = 0;
            try
            {
                using (SqlCommand command = transConn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;

                    command.Parameters.Add(new SqlParameter("@name", transConn.Conn.Database));
                    result = System.Convert.ToInt32(command.ExecuteScalar());
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ":" + sql, ex);
            }
            finally
            {
                transConn.Close();
            }

            return result > 0;


        }
    }

}
