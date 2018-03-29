using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CAS.Entity.BaseDAModels;
using CAS.DataAccess.BaseDAModels;
using System.Reflection;
using System.Configuration;
namespace CAS.DataAccess.DA
{
    public class BaseDA
    {           

        public const string RETURN_VALUE_PARAMETER_NAME = "@ReturnValue";

        /// <summary>
        /// 返回分页、排序处理后的SQL
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string HandleSQL<T>(T t, string sql)
        {
            dynamic search = t;
            sql += search.Where;
            if (search.Page)
            {
                return search.PageSelect(sql).ToLower() + search.PageWhere + " order by " + search.OrderBy;
            }
            else
            {
                if (!string.IsNullOrEmpty(search.OrderBy))
                    return sql.ToLower() + " order by " + search.OrderBy;
                else
                    return sql.ToLower();
            }
        }

        /// <summary>
        /// 返回Union数据集分页、排序处理后的SQL
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string HandleSQL<T>(T t)
        {
            dynamic search = t;
            if (search.Page)
            {
                return search.PageSql.ToLower();
            }
            else
            {
                if (!string.IsNullOrEmpty(search.OrderBy))
                    return string.Format("SELECT * FROM({0}) ORDER BY {1}", search.Sql.ToLower(), search.OrderBy);
                else
                    return search.Sql.ToLower();
            }

        }

        public static DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            return ExecuteDataSet(commandText, commandType, SqlServerSet.ConnectionName);
        }
        public static DataSet ExecuteDataSet<T>(string commandText, CommandType commandType) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            return ExecuteDataSet(commandText, commandType, GetConnectionName<T>());
        }
        public static DataSet ExecuteDataSet(string commandText, CommandType commandType, string connectionName)
        {
            string connectionString = SqlServerSet.GetConnectionString(connectionName);
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                DataSet ds;
                ds = ExecuteDataSet(cn, commandText, commandType);
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
            return ds;
        }

        public static DataSet ExecuteDataSet(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;
            DataSet ds;
            ds = ExecuteDataSet(cmd);
            return ds;
        }

        public static DataSet ExecuteDataSet(SqlCommand command)
        {
            return ExecuteDataSet(command, SqlServerSet.ConnectionName);
        }
        public static DataSet ExecuteDataSet<T>(SqlCommand command) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            return ExecuteDataSet(command, GetConnectionName<T>());
        }
        public static DataSet ExecuteDataSet(SqlCommand command, string connectionName)
        {
            string connectionString = SqlServerSet.GetConnectionString(connectionName);
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
            return ExecuteNonQuery(command, includeReturnValueParameter, SqlServerSet.ConnectionName);
        }
        public static Int32 ExecuteNonQuery<T>(SqlCommand command, bool includeReturnValueParameter) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            return ExecuteNonQuery(command, includeReturnValueParameter, GetConnectionName<T>());
        }
        public static Int32 ExecuteNonQuery(SqlCommand command, bool includeReturnValueParameter, string connectionName)
        {
            string connectionString = SqlServerSet.GetConnectionString(connectionName);
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();
                command.Connection = cn;
                command.CommandTimeout = 120;
                int returnValue;
                returnValue = command.ExecuteNonQuery();
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
            return ExecuteNonQuery(command, false, SqlServerSet.ConnectionName);
        }
        public static Int32 ExecuteNonQuery<T>(SqlCommand command) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            return ExecuteNonQuery(command, false, GetConnectionName<T>());
        }
        public static Int32 ExecuteNonQuery(SqlCommand command, string connectionName)
        {
            return ExecuteNonQuery(command, false, connectionName);
        }

        public static Int32 ExecuteNonQuery(string sql)
        {
            return ExecuteNonQuery(sql, SqlServerSet.ConnectionName);
        }
        public static Int32 ExecuteNonQuery<T>(string sql) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            return ExecuteNonQuery(sql, GetConnectionName<T>());
        }
        public static Int32 ExecuteNonQuery(string sql, string connectionName)
        {
            SqlCommand cmd = new SqlCommand(sql);
            cmd.CommandType = CommandType.Text;
            return ExecuteNonQuery(cmd, connectionName);
        }

        public static object ExecuteScalar(string commandText, CommandType commandType)
        {
            return ExecuteScalar(commandText, commandType, SqlServerSet.ConnectionName);
        }
        public static object ExecuteScalar<T>(string commandText, CommandType commandType) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            return ExecuteScalar(commandText, commandType, GetConnectionName<T>());
        }
        public static object ExecuteScalar(string commandText, CommandType commandType, string connectionName)
        {
            string connectionString = SqlServerSet.GetConnectionString(connectionName);
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(commandText, cn))
            {
                cn.Open();
                cmd.CommandType = commandType;
                cmd.CommandTimeout = 120;
                object obj = null;
                obj = cmd.ExecuteScalar();
                cn.Close();
                return obj;
            }
        }

        public static object ExecuteScalar(SqlCommand command)
        {
            return ExecuteScalar(command, SqlServerSet.ConnectionName);
        }
        public static object ExecuteScalar<T>(SqlCommand command) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            return ExecuteScalar(command, GetConnectionName<T>());
        }
        public static object ExecuteScalar(SqlCommand command, string connectionName)
        {
            string connectionString = SqlServerSet.GetConnectionString(connectionName);
            using (SqlConnection cn = new SqlConnection(connectionString))
            {
                cn.Open();

                command.Connection = cn;
                command.CommandTimeout = 120;
                object obj;
                obj = command.ExecuteScalar();
                cn.Close();
                return obj;
            }
        }

        public static SqlDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            return ExecuteReader(commandText, commandType, SqlServerSet.ConnectionName);
        }
        public static SqlDataReader ExecuteReader<T>(string commandText, CommandType commandType) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            return ExecuteReader(commandText, commandType, GetConnectionName<T>());
        }
        public static SqlDataReader ExecuteReader(string commandText, CommandType commandType, string connectionName)
        {
            string connectionString = SqlServerSet.GetConnectionString(connectionName);
            SqlConnection cn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(commandText, cn);

            cn.Open();
            cmd.CommandType = commandType;
            cmd.CommandTimeout = 120;
            SqlDataReader dr = null;
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection); 
            return dr;
        }

        public static SqlDataReader ExecuteReader(SqlCommand command)
        {
            return ExecuteReader(command, SqlServerSet.ConnectionName);
        }
        public static SqlDataReader ExecuteReader<T>(SqlCommand command) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            return ExecuteReader(command, GetConnectionName<T>());
        }
        public static SqlDataReader ExecuteReader(SqlCommand command, string connectionName)
        {
            string connectionString = SqlServerSet.GetConnectionString(connectionName);
            SqlConnection cn = new SqlConnection(connectionString);
            command.Connection = cn;
            command.CommandTimeout = 120;
            cn.Open();

            SqlDataReader dr = null;
            dr = command.ExecuteReader(CommandBehavior.CloseConnection);
            //cn.Close(); 不能关闭
            return dr;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public static void BeginBaseDATransaction(string connectionName)
        {
            BaseDAO<BaseTO>.StartSharedConnection(connectionName);
            BaseDAO<BaseTO>.BeginTransaction(connectionName);
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public static void CommitBaseDATransaction(string connectionName)
        {
            BaseDAO<BaseTO>.Commit(connectionName);
            BaseDAO<BaseTO>.CloseSharedConnection(connectionName);
        }
        /// <summary>
        /// 回滚事物
        /// </summary>
        public static void RollbackBaseDATransaction(string connectionName)
        {
            BaseDAO<BaseTO>.Rollback(connectionName);
            BaseDAO<BaseTO>.CloseSharedConnection(connectionName);
        }

        /// <summary>
        /// 对应于web.config <connectionStrings>节点的"name"属性
        /// FXTProject是默认连接
        /// 如需指定其它连接则设置此属性，如估价宝数据库需设置为OAConnectionName
        /// </summary>
        public static Dictionary<string, string> modelConnName = new Dictionary<string, string>();
        public static void SetConnectionName<T>(string connectionName) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            modelConnName.ThreadSafeWrite<string, string>(typeof(T).Name, connectionName, "modelConnName");
        }
        public static string GetConnectionName<T>() where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            string cName = modelConnName.ThreadSafeRead<string, string>(typeof(T).Name, "modelConnName");
            if (string.IsNullOrEmpty(cName))
            {
                cName = SqlServerSet.ConnectionName;
            }
            return cName;
        }
        /// <summary>
        /// 根据sql查询并转换成实体
        /// </summary>
        /// <typeparam name="V">继承BaseTO的实体类型</typeparam>
        /// <param name="sqlText"></param>
        /// <param name="cmdType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static T ExecuteToEntity<T>(string sqlText, CommandType cmdType, List<SqlParameter> parameters) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            T entity = default(T);
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            entity = da.GetItem(sqlText, cmdType, parameters);
            return entity;
        }
        /// <summary>
        /// 根据主键查询并转换成实体
        /// </summary>
        /// <typeparam name="V">继承BaseTO的实体类型</typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T ExecuteToEntityByPrimaryKey<T>(long id) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            T entity = default(T);
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            string tableName = GetEntityTable<T>();
            if (!string.IsNullOrEmpty(tableName))
            {
                da.SetTableName(tableName);
            }
            da.TableAlias = " t ";
            entity = da.GetItemByPK(id);
            return entity;
        }
        /// <summary>
        /// 根据多个主键ID查询并转换成List
        /// </summary>
        /// <typeparam name="V">继承BaseTO的实体类型</typeparam>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<T> ExecuteToEntityByPrimaryKey<T>(long[] ids) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            List<T> entityList = new List<T>();
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            string tableName = GetEntityTable<T>();
            if (!string.IsNullOrEmpty(tableName))
            {
                da.SetTableName(tableName);
            }
            da.TableAlias = " t ";
            entityList = da.GetItemByPK(ids);
            return entityList;
        }
        //多个主键查询
        public static T ExecuteToEntityByEntity<T>(T t) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            T entity = default(T);
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            try
            {
                string tableName = GetEntityTable<T>();
                if (!string.IsNullOrEmpty(tableName))
                {
                    da.SetTableName(tableName);
                }
                da.TableAlias = " t ";
                entity = da.GetItemByPK(t);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (t.IsSetCustomerFields)
                {
                    t.ClearFileds();
                }
                //自定义主键查询完毕，清除设置
                t.SetPrimaryKey<T>(null);
            }
            return entity;
        }

        /// <summary>
        /// 根据主键删除实体
        /// </summary>
        /// <typeparam name="V">继承BaseTO的实体类型</typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int DeleteByPrimaryKey<T>(long id) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            int result;
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            string tableName = GetEntityTable<T>();
            if (!string.IsNullOrEmpty(tableName))
            {
                da.SetTableName(tableName);
            }
            result = da.DeleteByPK(id);
            return result;
        }
        public static int DeleteByPrimaryKey<T>(string id) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            int result;
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            string tableName = GetEntityTable<T>();
            if (!string.IsNullOrEmpty(tableName))
            {
                da.SetTableName(tableName);
            }
            result = da.DeleteByPK(id);
            return result;
        }
        public static int DeleteByPrimaryKey<T>(T t) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            int result;
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            try
            {
                string tableName = GetEntityTable<T>();
                if (!string.IsNullOrEmpty(tableName))
                {
                    da.SetTableName(tableName);
                }
                result = da.DeleteByPK(t);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (t.IsSetCustomerFields)
                {
                    t.ClearFileds();
                }
                //自定义主键删除完毕，清除设置
                t.SetPrimaryKey<T>(null);
            }
            return result;
        }
        public static int DeleteByPrimaryKeyArray<T>(int[] idArray) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            int result;
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            string tableName = GetEntityTable<T>();
            if (!string.IsNullOrEmpty(tableName))
            {
                da.SetTableName(tableName);
            }
            result = da.DeleteByPKArray(idArray);
            return result;
        }
        public static int DeleteByPrimaryKeyArray<T>(long[] idArray) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            int result;
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            string tableName = GetEntityTable<T>();
            if (!string.IsNullOrEmpty(tableName))
            {
                da.SetTableName(tableName);
            }
            result = da.DeleteByPKArray(idArray);
            return result;
        }
        /// <summary>
        /// 将查询转换至指定类型的实体列表
        /// </summary>
        /// <typeparam name="V">继承BaseTO的实体类型</typeparam>
        /// <param name="sqlText"></param>
        /// <param name="cmdType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static List<T> ExecuteToEntityList<T>(string sqlText, CommandType cmdType, List<SqlParameter> parameters) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            List<T> list = null;
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            list = da.GetItems(sqlText, cmdType, parameters);
            return list;
        }
        /// <summary>
        /// 将实体插入数据库
        /// </summary>
        /// <typeparam name="V">继承BaseTO的实体类型</typeparam>
        /// <returns>插入实体的结果。如果主键Identity=true则返回自增Id, 其它返回SqlCommand.ExecuteNonQuery()的返回值。</returns>
        public static int InsertFromEntity<T>(T t) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            int result = 0;
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            try
            {
                string tableName = GetEntityTable<T>();
                if (!string.IsNullOrEmpty(tableName))
                {
                    da.SetTableName(tableName);
                }
                object objResult = da.InsertFromTO(t);
                if (null != objResult)
                {
                    int.TryParse(objResult.ToString(), out result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (t.IsSetCustomerFields)
                {
                    t.ClearFileds();
                }
                //自定义主键删除
                t.SetPrimaryKey<T>(null);
            }
            return result;
        }
        public static long InsertFromEntityAndReturnLongId<T>(T t) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            long result = 0;
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            try
            {
                string tableName = GetEntityTable<T>();
                if (!string.IsNullOrEmpty(tableName))
                {
                    da.SetTableName(tableName);
                }
                object objResult = da.InsertFromTO(t);
                if (null != objResult)
                {
                    long.TryParse(objResult.ToString(), out result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (t.IsSetCustomerFields)
                {
                    t.ClearFileds();
                }
                //自定义主键删除
                t.SetPrimaryKey<T>(null);
            }
            return result;
        }

        public static int UpdateFromEntity<T>(T t) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            int result;
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            try
            {
                string tableName = GetEntityTable<T>();
                if (!string.IsNullOrEmpty(tableName))
                {
                    da.SetTableName(tableName);
                }
                result = da.UpdateFromTO(t);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (t.IsSetCustomerFields)
                {
                    t.ClearFileds();
                }
                //自定义主键删除
                t.SetPrimaryKey<T>(null);
            }
            return result;
        }

        public static int BaseDAExecuteNonQuery<T>(string sqlText, List<SqlParameter> parameters) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            int result = 0;
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            result = da.ExecuteNonQuery(sqlText, parameters);
            return result;
        }
        public static object ExecuteScalar<T>(string sqlText, List<SqlParameter> parameters) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            object result = null;
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            result = da.ExecuteScalar(sqlText, parameters);
            return result;
        }

        //private static Dictionary<string, string> entityTable = new Dictionary<string, string>();
        /// <summary>
        /// 设置实体对应的表名
        /// </summary>
        /// <param name="tableName"></param>
        public static void SetEntityTable<T>(string tableName)
        {
            BaseTO.SetTableName<T>(tableName);
        }
        public static string GetEntityTable<T>()
        {
            string tName = BaseTO.GetTableName<T>();
            Type type = typeof(T);
            tName = tName.Replace("_" + type.Namespace + "_" + type.Name, "");
            return tName;
        }


        public static void SetEntityExtendQuery<T>(string[] extQueryArray) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            Query.SelectItemCollection extQuerys = new Query.SelectItemCollection();
            extQuerys.Add("*");
            foreach (string query in extQueryArray)
            {
                extQuerys.Add(query);
            }
            BaseDAO<T>.SetEntityExtendQuery(extQuerys);
        }

        /// <summary>
        /// 批量更新 kevin
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static int UpdateFromIds<T>(T t, int[] ids) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            int result;
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            try
            {
                string tableName = GetEntityTable<T>();
                if (!string.IsNullOrEmpty(tableName))
                {
                    da.SetTableName(tableName);
                }
                result = da.UpdateFromIds(t, ids);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (t.IsSetCustomerFields)
                {
                    t.ClearFileds();
                }
                //自定义主键删除
                t.SetPrimaryKey<T>(null);
            }
            return result;
        }
        
        /// <summary>
        /// 批量更新 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="ids"></param>
        public static int UpdateFromIds<T>(T t, long[] ids) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            return UpdateFromIds(t, ids, false);
        }
        /// <summary>
        /// 批量更新 kevin
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="ids"></param>
        /// <param name="enablePKUpdate">是否允许更新主键，用于某些自定义主键需要更新的情况</param>
        /// <returns></returns>
        public static int UpdateFromIds<T>(T t, long[] ids, bool enablePKUpdate) where T : CAS.Entity.BaseDAModels.BaseTO, new()
        {
            int result;
            BaseDAO<T> da = new BaseDAO<T>(GetConnectionName<T>());
            try
            {
                string tableName = GetEntityTable<T>();
                if (!string.IsNullOrEmpty(tableName))
                {
                    da.SetTableName(tableName);
                }
                result = da.UpdateFromIds(t, ids, enablePKUpdate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (t.IsSetCustomerFields)
                {
                    t.ClearFileds();
                }
                //自定义主键删除
                t.SetPrimaryKey<T>(null);
            }
            return result;
        }
    }
}
