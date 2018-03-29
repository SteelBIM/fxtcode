using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using CAS.Entity.BaseDAModels;


namespace CAS.DataAccess
{
    public class DAGloble
    {
        //Use generic dictionary instead HttpContext to store SqlTransConn. By Norman.Chen 2012/07/13
        public static Dictionary<string, CAS.DataAccess.BaseDAModels.SqlTransConn> TransactionPool = new Dictionary<string, CAS.DataAccess.BaseDAModels.SqlTransConn>();
    }
}
namespace CAS.DataAccess.BaseDAModels
{
    public class ExceptionNoPrimaryKey : Exception { }
    public class ExceptionErrorPrimaryKeyNumber : Exception { }

    public class BaseDAO<T> where T : CAS.Entity.BaseDAModels.BaseTO, new()
    {
        //public static Logger m_log = LogManager.GetCurrentClassLogger();
        private string _connectionName;
        public BaseDAO()
        {
            _tableName = BaseTO.GetTableName<T>();
            _connectionName = SqlServerSet.ConnectionName;
        }
        public BaseDAO(string connectionName)
        {
            _tableName = BaseTO.GetTableName<T>();
            _connectionName = connectionName;
        }

        protected delegate void FilterDelegate(string value, SqlBuilder builder);

        /// <summary>
        /// A Dictionary Collections of FilterDelegate type delegate
        /// </summary>
        protected Dictionary<string, FilterDelegate> FilterMap;
        
        /// <summary>
        /// create a new connection (to avoid connection sharing issue) if transaction was not started
        /// return a shared connection (with transaction on) if transaction was started
        /// </summary>
        protected static SqlTransConn GetConnection(string connectionName)
        {
            SqlTransConn transConn = DAGloble.TransactionPool.ContainsKey(connectionName) ? DAGloble.TransactionPool.ThreadSafeRead<string, SqlTransConn>(connectionName, "TransactionPool") : null;
            if (transConn == null)
            {                
                transConn = new SqlTransConn(SqlServerSet.GetConnectionString(connectionName));
            }
            else
            {
                if (transConn.Conn.State != ConnectionState.Open)//Transcation is running, open a new connection
                {
                    transConn = new SqlTransConn(SqlServerSet.GetConnectionString(connectionName));
                }
            }

            return transConn;
        }
        
        /// <summary>
        /// Use this if transaction needs to be used ACROSS different DAOs with same DB
        /// Auto get connection if not already done
        /// </summary>
        public static SqlTransConn StartSharedConnection(string connectionName)
        {
            SqlTransConn transConn = GetConnection(connectionName);
            DAGloble.TransactionPool.ThreadSafeWrite<string, SqlTransConn>(connectionName, transConn, "TransactionPool");

            return transConn;
        }

        /// <summary>
        /// Close and abort any transaction
        /// </summary>
        public static void CloseSharedConnection(string connectionName)
        {
            SqlTransConn transConn = DAGloble.TransactionPool.ThreadSafeRead<string, SqlTransConn>(connectionName, "TransactionPool");
            if (transConn != null)
            {
                transConn.Close();
                DAGloble.TransactionPool.ThreadSafeRemove<string, SqlTransConn>(connectionName, "TransactionPool");
            }
        }

        /// <summary>
        /// Use this BeginTransaction if transaction needs to be used ACROSS different DAOs with same DB
        /// Must use BaseDAO's Commit()/Rollback()
        /// Auto get connection if not already done
        /// </summary>
        public static void BeginTransaction(string connectionName)
        {
            SqlTransConn transConn = DAGloble.TransactionPool.ThreadSafeRead<string, SqlTransConn>(connectionName, "TransactionPool");
            transConn.BeginTransaction();
        }

        public static void Commit(string connectionName)
        {
            SqlTransConn transConn = DAGloble.TransactionPool.ThreadSafeRead<string, SqlTransConn>(connectionName, "TransactionPool");
           
            transConn.Commit();            
        }

        public static void Rollback(string connectionName)
        {
            SqlTransConn transConn = DAGloble.TransactionPool.ThreadSafeRead<string, SqlTransConn>(connectionName, "TransactionPool");

            transConn.Rollback();
        }

        public string TableAlias = null;
        private string _tableName;
        public void SetTableName(string tableName)
        {
            _tableName = tableName;
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
        public List<T> GetItems(string sqlText, CommandType cmdType, List<SqlParameter> parameters)
        {
            if (CommandType.Text == cmdType)
            {
                return _findAllFromSQLText(sqlText, parameters);
            }
            else
            {
                return _findAllFromStoreProc(sqlText, parameters);
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
            }
        }

        private static Dictionary<string, Query.SelectItemCollection> entityExtendQuery = new Dictionary<string, Query.SelectItemCollection>();
        public static void SetEntityExtendQuery(Query.SelectItemCollection extQuerys)
        {
            string tableName = BaseTO.GetTableName<T>();
            entityExtendQuery.ThreadSafeWrite<string, Query.SelectItemCollection>(tableName, extQuerys, "entityExtendQuery");
        }
        public static Query.SelectItemCollection GetEntityExtendQuery()
        {
            string tableName = BaseTO.GetTableName<T>();     
            return entityExtendQuery.ThreadSafeRead<string, Query.SelectItemCollection>(tableName, "entityExtendQuery");
        }

        public T GetItemByPK(long id)
        {
            Query query = new Query();
            string tableName = BaseTO.GetTableName<T>();
            query.Selects = GetEntityExtendQuery();
            string[] primaryKeys = BaseTO.GetSQLPrimaryKey(tableName);
            if (null == primaryKeys || 0 == primaryKeys.Length)
            {
                throw new ExceptionNoPrimaryKey();
            }
            if (1 < primaryKeys.Length)
            {
                //此方法只用于实体类只能有一个主键的情况
                throw new ExceptionErrorPrimaryKeyNumber();
            }
            query.Wheres.AddEqualPair(primaryKeys[0], id);            
            return GetItem(query);
        }
        /// <summary>
        /// 根据多个主键ID查询
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<T> GetItemByPK(long[] ids)
        {
            Query query = new Query();
            string tableName = BaseTO.GetTableName<T>();
            query.Selects = GetEntityExtendQuery();
            string[] primaryKeys = BaseTO.GetSQLPrimaryKey(tableName);
            if (null == primaryKeys || 0 == primaryKeys.Length)
            {
                throw new ExceptionNoPrimaryKey();
            }
            if (1 < primaryKeys.Length)
            {
                //此方法只用于实体类只能有一个主键的情况
                throw new ExceptionErrorPrimaryKeyNumber();
            }
            if (ids == null || ids.Length < 1)
            {
                ids = new long[] { 0 };
            }
            query.Wheres.AddSql(primaryKeys[0] + " in (" + string.Join(",", ids) + ")");
            //query.Wheres.AddEqualPair(primaryKeys[0], id);            
            return GetItems(query);
        }
        //通过多个主键查询
        public T GetItemByPK(BaseTO to)
        {
            Query query = new Query();
            string tableName = BaseTO.GetTableName<T>();
            query.Selects = GetEntityExtendQuery();
            string[] primaryKeys = to.GetPrimaryKey<T>();
            if (null == primaryKeys || 0 == primaryKeys.Length)
            {
                throw new ExceptionNoPrimaryKey();
            }
            foreach (string pkName in primaryKeys)
            {
                query.Wheres.AddSql(string.Format("{0} = @{0}", pkName), new string[] { "@" + pkName });
                query.Parameters.Add("@" + pkName, new Query.ParameterItem() { 
                    Type = SqlDbType.NVarChar,
                    Value = to.GetPropertyValue(pkName).ToString()                    
                });
                //query.Wheres.AddEqualPair(pkName, to.GetPropertyValue(pkName).ToString());
            }
            return GetItem(query);
        }

        public T GetItemByPK(T t,string pkValue)
        {
            Query query = new Query();
            string tableName = BaseTO.GetTableName<T>();
            query.Selects = GetEntityExtendQuery();
            string[] primaryKeys = t.GetPrimaryKey<T>();
            if (null == primaryKeys || 0 == primaryKeys.Length)
            {
                throw new ExceptionNoPrimaryKey();
            }
            if (1 < primaryKeys.Length)
            {
                //此方法只用于实体类只能有一个主键的情况
                throw new ExceptionErrorPrimaryKeyNumber();
            }
            query.Wheres.AddEqualPair(primaryKeys[0], pkValue);
            return GetItem(query);
        }

        public T GetItem(string sqlText, CommandType cmdType, List<SqlParameter> parameters)
        {
            List<T> list = GetItems(sqlText, cmdType, parameters);
            T to = null;
            if (list.Count == 1)
            {
                to = list[0] as T;

            }
            return to;
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
            catch (ExceptionEmptyQuery )
            {
                return new List<T>();
            }

            SqlTransConn transConn = GetConnection(_connectionName);
            try
            {
                using (SqlCommand command = transConn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 120;
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
                throw new Exception(sql + " " + ex.Message);
            }
            finally
            {
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
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

            SqlTransConn transConn = GetConnection(_connectionName);
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
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
            }

            return list;
        }

        private List<T> _findAllFromSQLText(string sql, List<SqlParameter> parameters)
        {
            List<T> list = new List<T>();
            SqlTransConn transConn = GetConnection(_connectionName);
            try
            {
                using (SqlCommand command = transConn.CreateCommand())
                {
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 120;

                    if (null != parameters)
                    {
                        foreach (SqlParameter parm in parameters)
                        {
                            command.Parameters.Add(parm);
                        }
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                T to = new T();
                                to.Initialize(reader);
                                //LateBinding(ref to, query);
                                list.Add(to);
                            }
                        }   
                    }
                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(sql + ",message:" + ex.Message, ex);
            }
            finally
            {
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
            }

            return list;
        }
        private List<T> _findAllFromStoreProc(string storeProcName, List<SqlParameter> parameters)
        {
            List<T> list = new List<T>();
            SqlTransConn transConn = GetConnection(_connectionName);
            
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
                            //LateBinding(ref to, query);
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
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
            }

            return list;
        }

        public int ExecuteNonQuery(string sqlText, List<SqlParameter> parameters)
        {
            int result = 0;
            SqlTransConn transConn = GetConnection(_connectionName);
            try
            {
                result = SqlHelper.ExecuteNonQuery(transConn, CommandType.Text, sqlText, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ":" + sqlText.ToString(), ex);
            }
            finally
            {
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
            }
            return result;
        }

        public object ExecuteScalar(string sqlText, List<SqlParameter> parameters)
        {
            object result = null;
            SqlTransConn transConn = GetConnection(_connectionName);
            try
            {
                result = SqlHelper.ExecuteScalar(transConn, CommandType.Text, sqlText, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ":" + sqlText, ex);
            }
            finally
            {
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
            }
            return result;
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
                catch (ExceptionEmptyQuery )
                {
                    return 0;
                }
                catch (ExceptionBadFilterSequence )
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

            SqlTransConn transConn = GetConnection(_connectionName);

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
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
            }

            // set this so that the value can be used to determine IsValidRange of limit/offset/page
            query.Total = total;

            return total;
        }

        protected SqlBuilder InitSqlBuilder(Query query)
        {

            SqlBuilder builder = new SqlBuilder();
            builder.SetInternalBinding(internalBinding);

            builder.FromTableName = _tableName ?? query.TableName;
            builder.FromTableAlias = TableAlias ?? String.Empty;

            builder.Limit = query.Limit;
            builder.Offset = query.Offset;
            builder.Page = query.Page;
            builder.HasGroupBys = query.HasGroupBys;
            builder.UnionSqls = query.UnionSqls;
            if (query.UnionParameters != null && query.UnionParameters.Count>0) {
                foreach (SqlParameter param in query.UnionParameters) {
                    builder.AddToBindParams(param.ParameterName, param.Value, param.SqlDbType);
                }
            }

            Dictionary<string, bool> uniqueParameters = new Dictionary<string, bool>();
            if(null == query.Selects)
            {                
                Query.SelectItemCollection extQuerys = new Query.SelectItemCollection();
                extQuerys.Add("*");
                query.Selects = extQuerys;
            }
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

                    if (null != SelectMap && SelectMap.ContainsKey(key))
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
        
        /// <summary>
        /// Insert a entity to database.
        /// </summary>
        /// <param name="to">Entity object</param>
        /// <returns>If entity's primary key is identify field return the new id, else return the return value of SqlCommand.ExecuteNonQuery()</returns>
        public object InsertFromTO(BaseTO to)
        {
            Dictionary<string, object> dict = to.GetSQLWriteValues();
            //我把recordcount放在baseTo里面，用于列表分页时返回记录总数，这里新增修改时需要过滤掉 kevin
            dict.Remove("recordcount");
            bool hasIdentifyPK = false;
            string[] primaryKay = to.GetPrimaryKey<T>();
            foreach (string keyName in primaryKay)
            {
                SQLFieldAttribute[] attrs = to.GetFieldPropertyAttribute<SQLFieldAttribute>(keyName);
                if (null != attrs)
                {
                    foreach (SQLFieldAttribute a in attrs)
                    {
                        hasIdentifyPK = a.IsIdentify;
                        break;
                    }
                }
                if (hasIdentifyPK) break;
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Insert into ").Append(_tableName).Append(" (");
            sb.Append(String.Join(", ", dict.Keys.ToArray()));
            sb.Append(") values (");
            List<SqlParameter> parameters = new List<SqlParameter>();
            int i = 0;
            foreach (string name in dict.Keys)
            {
                if (i>0) sb.Append(", ");
                sb.Append("@").Append(name);
                parameters.Add(Query.BuilderParam(name, dict[name]));
                i++;

            }
            sb.Append(")");
            if (0 == to.CustomPrimaryKeyIdentify ? hasIdentifyPK : (1 == to.CustomPrimaryKeyIdentify))
            {
                sb.Append(";select @@identity");
            }

            SqlTransConn transConn = GetConnection(_connectionName);
            object result = 0;
            try
            {
                if (0 == to.CustomPrimaryKeyIdentify ? hasIdentifyPK : (1 == to.CustomPrimaryKeyIdentify))
                {
                    result = SqlHelper.ExecuteScalar(transConn, CommandType.Text, sb.ToString(), parameters);
                }
                else
                {
                    result = SqlHelper.ExecuteNonQuery(transConn, CommandType.Text, sb.ToString(), parameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ":" + sb.ToString(), ex);
            }
            finally
            {
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
            }

            return result;
        }

        public int DeleteByPK(object PKValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Delete from ").Append(_tableName);
            string[] primaryKeys = BaseTO.GetSQLPrimaryKey(BaseTO.GetTableName<T>());
            if (null == primaryKeys || 0 == primaryKeys.Length)
            {
                throw new ExceptionNoPrimaryKey();
            }
            if (1 < primaryKeys.Length)
            {
                //此方法只用于实体类只能有一个主键的情况
                throw new ExceptionErrorPrimaryKeyNumber();
            }
            sb.Append(" where ").Append(primaryKeys[0]).Append("=@PKValue");
            List<SqlParameter> parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@PKValue", PKValue));
            SqlTransConn transConn = GetConnection(_connectionName);

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
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
            }

            return result;
        }

        public int DeleteByPK(long id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Delete from ").Append(_tableName);
            string[] primaryKeys = BaseTO.GetSQLPrimaryKey(BaseTO.GetTableName<T>());
            if (null == primaryKeys || 0 == primaryKeys.Length)
            {
                throw new ExceptionNoPrimaryKey();
            }
            if (1 < primaryKeys.Length)
            {
                //此方法只用于实体类只能有一个主键的情况
                throw new ExceptionErrorPrimaryKeyNumber();
            }
            sb.Append(" where ").Append(primaryKeys[0]).Append("=@PKValue");
            List<SqlParameter> parameters = new List<SqlParameter>();

            SqlParameter parameter = new SqlParameter("@PKValue", SqlDbType.Int, 0);
            parameter.Value = id;
            parameters.Add(parameter);
            SqlTransConn transConn = GetConnection(_connectionName);

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
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
            }

            return result;
        }
        public int DeleteByPKArray(long[] idArrar)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Delete from ").Append(_tableName);
            string[] primaryKeys = BaseTO.GetSQLPrimaryKey(BaseTO.GetTableName<T>());
            if (null == primaryKeys || 0 == primaryKeys.Length)
            {
                throw new ExceptionNoPrimaryKey();
            }
            if (1 < primaryKeys.Length)
            {
                //此方法只用于实体类只有一个主键的情况
                throw new ExceptionErrorPrimaryKeyNumber();
            }
            string idList = string.Join(",", idArrar);
            sb.Append(" where ")
              .Append(primaryKeys[0])
              .Append(" in (")
              .Append(idList)
              .Append(")");
            List<SqlParameter> parameters = new List<SqlParameter>();

            SqlTransConn transConn = GetConnection(_connectionName);

            int result = 0;
            try
            {
                result = SqlHelper.ExecuteNonQuery(transConn, CommandType.Text, sb.ToString(), null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ":" + sb.ToString(), ex);
            }
            finally
            {
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
            }

            return result;
        }
        public int DeleteByPKArray(int[] idArrar)
        {
            long[] longIdArr = idArrar.Select(t => Convert.ToInt64(t)).ToArray();
            return DeleteByPKArray(longIdArr);
        }

        public int DeleteByPK(T t)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Delete from ").Append(_tableName);
            string[] primaryKeys = t.GetPrimaryKey<T>();
            if (null == primaryKeys || 0 == primaryKeys.Length)
            {
                throw new ExceptionNoPrimaryKey();
            }
            sb.Append(" where 1=1");
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (string pk in primaryKeys)
            {
                sb.Append(" and ").Append(pk).Append("=@" + pk);
                SqlParameter parameter = new SqlParameter("@" + pk, SqlDbType.NVarChar);
                parameter.Value = t.GetPropertyValue(pk);
                parameters.Add(parameter);
            }

            SqlTransConn transConn = GetConnection(_connectionName);

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
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
            }

            return result;
        }
        public int UpdateFromTO(BaseTO to)
        {
            Dictionary<string, object> dict;
            dict = to.GetSQLWriteValues();
            //我把recordcount放在baseTo里面，用于列表分页时返回记录总数，这里新增修改时需要过滤掉 kevin
            dict.Remove("recordcount");
            StringBuilder sb = new StringBuilder();
            sb.Append("update ").Append(_tableName).Append(" set ");
            List<SqlParameter> parameters = new List<SqlParameter>();
            int i = 0;
            string[] primaryKeys = to.GetPrimaryKey<T>();
            foreach (string name in dict.Keys)
            {
                //不是主键才添加到update语句的更新字段中
                if (!primaryKeys.Contains<string>(name))
                {
                    if (i > 0) sb.Append(", ");
                    sb.Append(name).Append("=@").Append(name);
                    parameters.Add(Query.BuilderParam(name, dict[name]));
                    i++;
                }
            }

            if (null == primaryKeys || 0 == primaryKeys.Length)
            {
                throw new ExceptionNoPrimaryKey();
            }
            sb.Append(" where 1=1 ");
            foreach (string primaryKeyName in primaryKeys)
            {
                sb.Append(" and ").Append(primaryKeyName).Append("=@" + primaryKeyName);
                parameters.Add(new SqlParameter("@" + primaryKeyName, to.GetPropertyValue(primaryKeyName)));
            }

            SqlTransConn transConn = GetConnection(_connectionName);
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
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 批量更新 kevin
        /// </summary>
        /// <param name="to"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int UpdateFromIds(BaseTO to, int[] ids)
        {
            return UpdateFromIds(to, ids.Select(t => Convert.ToInt64(t)).ToArray(), false);
        }
        /// <summary>
        /// 根据主键数组和给定实体的值更新
        /// </summary>
        public int UpdateFromIds(BaseTO to, long[] ids)
        {
            return UpdateFromIds(to, ids, false);
        }
        /// <summary>
        /// 根据主键数组和给定实体的值更新
        /// </summary>
        /// <param name="to"></param>
        /// <param name="ids"></param>
        /// <param name="enablePKUpdate">是否允许更新主键，用于某些自定义主键需要更新的情况</param>
        public int UpdateFromIds(BaseTO to, long[] ids, bool enablePKUpdate)
        {
            Dictionary<string, object> dict;
            dict = to.GetSQLWriteValues();
            //我把recordcount放在baseTo里面，用于列表分页时返回记录总数，这里新增修改时需要过滤掉 kevin
            dict.Remove("recordcount");
            StringBuilder sb = new StringBuilder();
            sb.Append("update ").Append(_tableName).Append(" set ");
            List<SqlParameter> parameters = new List<SqlParameter>();
            int i = 0;
            string[] primaryKeys = to.GetPrimaryKey<T>();
            foreach (string name in dict.Keys)
            {
                //如果不允许更新主键，不是主键才添加到update语句的更新字段中
                if (enablePKUpdate ? true : !primaryKeys.Contains<string>(name))
                {
                    if (i > 0) sb.Append(", ");
                    sb.Append(name).Append("=@").Append(name);
                    parameters.Add(Query.BuilderParam(name, dict[name]));
                    i++;
                }
            }

            if (null == primaryKeys || 0 == primaryKeys.Length)
            {
                throw new ExceptionNoPrimaryKey();
            }
            string idList = string.Join(",", ids);
            sb.Append(" where " + primaryKeys[0] + " in ( " + idList + ")");

            SqlTransConn transConn = GetConnection(_connectionName);
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
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
            }

            return result;
        }

        public bool IsReadOnly()
        {
            string sql = @"SELECT is_read_only FROM sys.databases WHERE name = @name";
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlTransConn transConn = GetConnection(_connectionName);

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
                throw new Exception("Failed to execute sql:" + sql, ex);
            }
            finally
            {
                if (!transConn.IsInTransaction())
                {
                    transConn.Close();
                }
            }

            return result > 0;
        }
    }

}
