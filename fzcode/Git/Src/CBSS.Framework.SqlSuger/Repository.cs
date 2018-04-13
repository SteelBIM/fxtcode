using CBSS.Core.Log;
using CBSS.Core.Utility;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace CBSS.Framework.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class Repository
    {
        /// <summary>
        /// 
        /// </summary>
        public string _operatorError = string.Empty;

        private string _connectionString = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    string xmlPath = "";
                    System.Web.HttpContext context = System.Web.HttpContext.Current;
                    if (context != null)
                    {
                        xmlPath = context.Server.MapPath("~/Config/DaoConfig.xml");
                    }
                    else
                    {
                        xmlPath = System.Web.HttpRuntime.AppDomainAppPath + "\\Config\\DaoConfig.xml";
                    }
                    XDocument xdc = new XDocument(XDocument.Load(xmlPath));

                    var pElement = xdc.Elements().First();
                    //List<DbEntity> dbs = new List<DbEntity>();
                    foreach (var n in pElement.Elements())
                    {
                        //  dbs.Add(new DbEntity { DbName = n.Name.LocalName, ConnectString = n.Value });
                        if (n.Name.LocalName == "Tbx")
                        {
                            _connectionString = n.Value;
                        }
                    }
                    //_connectionString = ConfigurationManager.ConnectionStrings["KingsunConnectionStr"].ConnectionString;
                }
                return _connectionString;
            }
        }

        private string OutConnectionString = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_connectionName"></param>
        /// <exception cref="Exception"></exception>
        public Repository(string _connectionName)
        {
            string xmlPath = "";
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            if (context != null)
            {
                xmlPath = context.Server.MapPath("~/Config/DaoConfig.xml");
            }
            else
            {
                xmlPath = System.Web.HttpRuntime.AppDomainAppPath + "\\Config\\DaoConfig.xml";
            }

            XDocument xdc = new XDocument(XDocument.Load(xmlPath));

            var pElement = xdc.Elements().First();
            //List<DbEntity> dbs = new List<DbEntity>();
            foreach (var n in pElement.Elements())
            {
                //  dbs.Add(new DbEntity { DbName = n.Name.LocalName, ConnectString = n.Value });
                if (n.Name.LocalName == _connectionName)
                {
                    OutConnectionString = n.Value;
                    return;
                }
            }
            //OutConnectionString = _connectionString;
            if (_connectionName!=null && string.IsNullOrEmpty(OutConnectionString))
            {
                string msg = "未找到连接字符串:" + _connectionName;
                Log4NetHelper.Fatal(LoggerType.ServiceExceptionLog, msg, null);
                throw new Exception(msg);
            }
        }

        public Repository()
        {

        }
        public SqlSugarClient GetInstance()
        {
            string connect = ConnectionString;
            //SyntacticSugar
            if (string.Empty != OutConnectionString)
            {
                connect = OutConnectionString;
            }

            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connect, //必填
                DbType = SqlSugar.DbType.SqlServer, //必填
                IsAutoCloseConnection = true

            }); //默认SystemTable

            return db;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public SqlSugarClient GetInstance(string connectionString)
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString, //必填
                DbType = SqlSugar.DbType.SqlServer, //必填
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.SystemTable
            }); //默认SystemTable
            return db;
        }


        #region Insert
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.Insert<TEntity>(TEntity)”的 XML 注释
        public object Insert<TEntity>(TEntity entity) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.Insert<TEntity>(TEntity)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                var result = db.Insertable(entity).ExecuteReturnIdentity();
                return result;
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.InsertReturnEntity<TEntity>(TEntity, string[])”的 XML 注释
        public TEntity InsertReturnEntity<TEntity>(TEntity info, string[] array = null) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.InsertReturnEntity<TEntity>(TEntity, string[])”的 XML 注释
        {
            using (var db = GetInstance())
            {
                TEntity result = db.Insertable<TEntity>(info).ExecuteReturnEntity();
                return result;
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.InsertRange<TEntity>(IEnumerable<TEntity>)”的 XML 注释
        public int InsertRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.InsertRange<TEntity>(IEnumerable<TEntity>)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                var result = db.Insertable<TEntity>(entities).Where(false).ExecuteCommand();
                return result;
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.InsertRange<TEntity>(IEnumerable<TEntity>, bool)”的 XML 注释
        public int InsertRange<TEntity>(IEnumerable<TEntity> entities, bool isIdentity) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.InsertRange<TEntity>(IEnumerable<TEntity>, bool)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                var result = db.Insertable<TEntity>(entities).Where(false, !isIdentity).ExecuteCommand();
                return result;
            }
        }
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool InsertBatch<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                int result = db.Insertable(entities.ToArray()).ExecuteCommand();
                return result > 0 ? true : false;
            }
        }
        #endregion

        #region Update
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.Update<TEntity>(TEntity)”的 XML 注释
        public bool Update<TEntity>(TEntity entity) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.Update<TEntity>(TEntity)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                var result = db.Updateable<TEntity>(entity).Where(true).ExecuteCommand();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.Update<TEntity>(TEntity, Expression<Func<TEntity, bool>>)”的 XML 注释
        public bool Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> includes = null) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.Update<TEntity>(TEntity, Expression<Func<TEntity, bool>>)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                var result = db.Updateable<TEntity>(entity).Where(true).Where(includes).ExecuteCommand();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.UpdateWithNull<TEntity>(TEntity, Expression<Func<TEntity, bool>>)”的 XML 注释
        public bool UpdateWithNull<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> includes = null) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.UpdateWithNull<TEntity>(TEntity, Expression<Func<TEntity, bool>>)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                var result = db.Updateable<TEntity>(entity).Where(includes).ExecuteCommand();
                if (result > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public int BatchUpdate<TEntity>(object info = null, Expression<Func<TEntity, bool>> includes = null) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Updateable<TEntity>(info).Where(includes).ExecuteCommand();
            }
        }

        /// <summary>
        /// 指定忽略字段更新,hlw封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool CustomIgnoreUpdate<T>(Expression<Func<T, string>> key, T info, params Expression<Func<T, string>>[] ex)
        {
            var updateColumns = ex.Select(o => o.Body.ToString().Split('.')[1]).ToList();
            var updateProperties = typeof(T).GetProperties().Where(o => !updateColumns.Contains(o.Name)).ToList();//需要更新的字段          

            string tableKey = key.Body.ToString().Split('.')[1];//主键
            var keyProperty = typeof(T).GetProperties().FirstOrDefault(o => o.Name == tableKey);
            string table = typeof(T).Name;
            string set = "";
            List<SugarParameter> sqlParams = new List<SugarParameter>();

            string where = tableKey + "=@" + tableKey;

            sqlParams.Add(new SugarParameter(tableKey, keyProperty.GetValue(info)));//主键的值参数化.
            foreach (var c in updateProperties)
            {
                if (c.Name == tableKey)
                {
                    continue;//主键不能update
                }

                set += string.Format(c.Name + "=@" + c.Name + ",");
                if (!sqlParams.Select(o => o.ParameterName).Contains(c.Name))
                {
                    sqlParams.Add(new SugarParameter(c.Name, c.GetValue(info)));//参数不能重复
                }
            }

            set = set.Substring(0, set.Length - 1);//去掉最后一个,号

            string sql = String.Format(@"update {0} set {1}  where {2} ", table, set, where);

            var r = SelectString(sql, sqlParams);//sql参数化
            return true;

        }
        /// <summary>
        /// 指定字段更新,hlw封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="info"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool CustomUpdateEntity<T>(Expression<Func<T, string>> key, T info, params Expression<Func<T, string>>[] ex)
        {
            var updateColumns = ex.Select(o => o.Body.ToString().Split('.')[1]).ToList();
            var updateProperties = typeof(T).GetProperties().Where(o => updateColumns.Contains(o.Name)).ToList();//需要更新的字段          

            string tableKey = key.Body.ToString().Split('.')[1];//主键
            var keyProperty = typeof(T).GetProperties().FirstOrDefault(o => o.Name == tableKey);
            string table = typeof(T).Name;
            string set = "";
            List<SugarParameter> sqlParams = new List<SugarParameter>();

            string where = tableKey + "=@" + tableKey;

            sqlParams.Add(new SugarParameter(tableKey, keyProperty.GetValue(info)));//主键的值参数化.
            foreach (var c in updateProperties)
            {
                if (c.Name == tableKey)
                {
                    continue;//主键不能update
                }

                set += string.Format(c.Name + "=@" + c.Name + ",");
                if (!sqlParams.Select(o => o.ParameterName).Contains(c.Name))
                {
                    sqlParams.Add(new SugarParameter(c.Name, c.GetValue(info)));//参数不能重复
                }
            }

            set = set.Substring(0, set.Length - 1);//去掉最后一个,号

            string sql = String.Format(@"update {0} set {1}  where {2} ", table, set, where);

            var r = SelectString(sql, sqlParams);//sql参数化
            return true;

        }

        /// <summary>
        /// 批量指定字段更新,hlw封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="infos"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool CustomUpdateRange<T>(Expression<Func<T, string>> key, List<T> infos, params Expression<Func<T, string>>[] ex) where T : class, new()
        {
            if (!infos.Any())
            {
                return true;//无数据视为操作成功！
            }

            var updateColumns = ex.Select(o => o.Body.ToString().Split('.')[1]).ToList();
            var updateProperties = typeof(T).GetProperties().Where(o => updateColumns.Contains(o.Name)).ToList();//需要更新的字段          

            string tableKey = key.Body.ToString().Split('.')[1];//主键
            var keyProperty = typeof(T).GetProperties().FirstOrDefault(o => o.Name == tableKey);
            string table = typeof(T).Name;

            List<SugarParameter> sqlParams = new List<SugarParameter>();
            string sql = "";
            for (int i = 0; i < infos.Count; i++)
            {
                var info = infos[i];
                string set = "";
                string where = tableKey + "=@" + (tableKey + i);
                sqlParams.Add(new SugarParameter(tableKey + i, keyProperty.GetValue(info)));//主键的值参数化.
                foreach (var c in updateProperties)
                {
                    if (c.Name == tableKey)
                    {
                        continue;//主键不能update
                    }

                    set += string.Format(c.Name + "=@" + c.Name + i + ",");
                    if (!sqlParams.Select(o => o.ParameterName).Contains(c.Name + i))
                    {//已存在的参数不能重复
                        sqlParams.Add(new SugarParameter(c.Name + i, c.GetValue(info)));
                    }
                }

                set = set.Substring(0, set.Length - 1);//去掉最后一个,号

                sql += String.Format(@"update {0} set {1}  where {2} ;", table, set, where);
            }
            var r = SelectString(sql, sqlParams);//sql参数化
            return true;

        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.UpdateIgnoreColumns<TEntity>(TEntity, Expression<Func<TEntity, object>>)”的 XML 注释
        public bool UpdateIgnoreColumns<TEntity>(TEntity entity, Expression<Func<TEntity, object>> includes = null) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.UpdateIgnoreColumns<TEntity>(TEntity, Expression<Func<TEntity, object>>)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                //  var sql = db.Updateable<TEntity>(entity).IgnoreColumns(includes).ToSql();               
                int i = db.Updateable<TEntity>(entity).IgnoreColumns(includes).ExecuteCommand();
                return i > 0;
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.Update<TEntity>(TEntity, string[])”的 XML 注释
        public bool Update<TEntity>(TEntity entity, string[] disableUpdateCoulums) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.Update<TEntity>(TEntity, string[])”的 XML 注释
        {
            using (var db = GetInstance())
            {
                var result = db.Updateable<TEntity>(entity).Where(false).IgnoreColumns(it => disableUpdateCoulums).ExecuteCommand();
                return result > 0;
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.UpdateColumns<TEntity>(TEntity, Expression<Func<TEntity, object>>)”的 XML 注释
        public bool UpdateColumns<TEntity>(TEntity entity, Expression<Func<TEntity, object>> includes = null) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.UpdateColumns<TEntity>(TEntity, Expression<Func<TEntity, object>>)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                var result = db.Updateable<TEntity>(entity).UpdateColumns(includes).ExecuteCommand();
                return result > 0;
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.MyUpdate<TEntity>(TEntity, string, string[])”的 XML 注释
        public bool MyUpdate<TEntity>(TEntity entity, string tableKey, string[] notUpdateColum = null) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.MyUpdate<TEntity>(TEntity, string, string[])”的 XML 注释
        {
            using (var db = GetInstance())
            {
                var result = db.Updateable<TEntity>(entity).IgnoreColumns(it => notUpdateColum).ExecuteCommand();
                return result > 0;
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.UpdateAssign<TEntity>(TEntity, Expression<Func<TEntity, bool>>)”的 XML 注释
        public bool UpdateAssign<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> includes = null) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.UpdateAssign<TEntity>(TEntity, Expression<Func<TEntity, bool>>)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                var result = db.Updateable<TEntity>(entity).Where(includes).ExecuteCommand();
                return result > 0;
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public bool Delete<TEntity>(object id) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Deleteable<TEntity>().In(id).ExecuteCommand();
                return result > 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public bool Delete<TEntity>(Expression<Func<TEntity, bool>> expr) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Deleteable(expr).ExecuteCommand();
                return result > 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Ids"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public bool DeleteMore<TEntity>(string[] Ids) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                // var result = db.Delete<T, string>(arrayids);

                var result = db.Deleteable<TEntity>(Ids).ExecuteCommand();
                return result > 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public bool DeleteBatch<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Deleteable<TEntity>(entities.ToArray()).ExecuteCommand();
                return result > 0;
            }
        }

        #endregion

        #region Query
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public TEntity GetByID<TEntity>(object id) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var t = db.Queryable<TEntity>().InSingle(id);
                return t;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> SqlQuery<TEntity>(string sql, IEnumerable<SqlParameter> pars = null) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().Where(sql, pars).ToList();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> ListAll<TEntity>() where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var list = db.Queryable<TEntity>().ToList();
                return list;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="field"></param>
        /// <param name="Ins"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> SelectIn<TEntity>(Expression<Func<TEntity, bool>> expr, string field, IEnumerable<string> Ins) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var list = db.Queryable<TEntity>().Where(expr).In(field, Ins).ToList();
                return list;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filedName"></param>
        /// <param name="filedlist"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> SelectIn<TEntity>(string filedName, IEnumerable<string> filedlist) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var list = db.Queryable<TEntity>().In(filedName, filedlist).ToList();
                return list;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="topNumber"></param>
        /// <param name="orderby"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> SelectSearch<TEntity>(Expression<Func<TEntity, bool>> expression, int topNumber, string orderby = "") where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                if (string.IsNullOrEmpty(orderby) && topNumber == 0)
                {
                    return db.Queryable<TEntity>().Where(expression).ToList();
                }
                else if (string.IsNullOrEmpty(orderby) && topNumber > 0)
                {
                    return db.Queryable<TEntity>().Where(expression).Take(topNumber).ToList();
                }
                else if (!string.IsNullOrEmpty(orderby) && topNumber == 0)
                {
                    return db.Queryable<TEntity>().Where(expression).OrderBy(orderby).ToList();
                }
                else
                {
                    return db.Queryable<TEntity>().Where(expression).OrderBy(orderby).Take(topNumber).ToList();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="orderby"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public List<TEntity> SelectSearch<TEntity>(Expression<Func<TEntity, bool>> expression, string orderby) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().Where(expression).OrderBy(orderby).ToList();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="Field"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> SelectAppointField<TEntity>(Expression<Func<TEntity, bool>> expression, string Field) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().Where(expression).Select(Field).ToList();
            }
        }
        /// <summary>
        /// 结果为自定义实体LIST
        /// </summary>
        /// <typeparam name="T">自定义实体</typeparam>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        public IEnumerable<T> CustomEntitySelect<T>(string whereSql) where T : class, new()
        {
            using (var db = GetInstance())
            {
                var dt = db.Ado.GetDataTable(whereSql);
                return ConvertToModel<T>(dt);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereSql"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> SelectSearch<TEntity>(string whereSql) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                if (!string.IsNullOrEmpty(whereSql))
                {
                    return db.Queryable<TEntity>().Where(whereSql).ToList();
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="expressions"></param>
        /// <param name="orderfile"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public IEnumerable<TEntity> SelectSearchs<TEntity>(IEnumerable<Expression<Func<TEntity, bool>>> expressions,   string orderfile  ) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var queryable = db.Queryable<TEntity>();
                if (expressions != null)
                {
                    foreach (var where in expressions)
                    {
                        queryable = queryable.Where(where);
                    }
                }
             return queryable.OrderBy(orderfile).ToList();
            }
        }


#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.SelectSearchs<TEntity>(IEnumerable<Expression<Func<TEntity, bool>>>, string, IEnumerable<string>, string)”的 XML 注释
        public IEnumerable<TEntity> SelectSearchs<TEntity>(IEnumerable<Expression<Func<TEntity, bool>>> expressions, string Flids = "", IEnumerable<string> InIds = null, string orderfile = "") where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.SelectSearchs<TEntity>(IEnumerable<Expression<Func<TEntity, bool>>>, string, IEnumerable<string>, string)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                var queryable = db.Queryable<TEntity>();
                if (expressions != null)
                {
                    foreach (var where in expressions)
                    {
                        queryable = queryable.Where(where);
                    }
                }
                if (!string.IsNullOrEmpty(Flids) && InIds != null && InIds.Count() > 0 && !string.IsNullOrEmpty(orderfile))
                    return queryable.In(Flids, InIds).OrderBy(orderfile).ToList();
                else if (!string.IsNullOrEmpty(Flids) && InIds != null && InIds.Count() > 0)
                    return queryable.In(Flids, InIds).ToList();
                if (!string.IsNullOrEmpty(orderfile))
                    return queryable.OrderBy(orderfile).ToList();
                return queryable.ToList();
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.SelectGroupBy<TEntity>(Expression<Func<TEntity, bool>>, string)”的 XML 注释
        public IEnumerable<TEntity> SelectGroupBy<TEntity>(Expression<Func<TEntity, bool>> expression, string groupbyfields) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.SelectGroupBy<TEntity>(Expression<Func<TEntity, bool>>, string)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().Where(expression).GroupBy(groupbyfields).ToList();
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.SelectGroupBy<TEntity>(Expression<Func<TEntity, bool>>, string, string, List<string>)”的 XML 注释
        public IEnumerable<TEntity> SelectGroupBy<TEntity>(Expression<Func<TEntity, bool>> expression, string groupbyfields, string Flide, List<string> Ids) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.SelectGroupBy<TEntity>(Expression<Func<TEntity, bool>>, string, string, List<string>)”的 XML 注释
        {
            throw new NotImplementedException();
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.SelectGroupBy<TEntity>(string)”的 XML 注释
        public IEnumerable<TEntity> SelectGroupBy<TEntity>(string groupbyfield) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.SelectGroupBy<TEntity>(string)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().GroupBy(groupbyfield).ToList();
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.SelectSearch<TEntity>(Expression<Func<TEntity, bool>>)”的 XML 注释
        public List<TEntity> SelectSearch<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.SelectSearch<TEntity>(Expression<Func<TEntity, bool>>)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                //var sql = db.Queryable<TEntity>().Where(expression).ToSql();
                return db.Queryable<TEntity>().Where(expression).ToList();
            }
        }

        /// <summary>
        /// 查询一条
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TEntity FirstOrDefault<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                //var sql = db.Queryable<TEntity>().Where(expression).ToSql();
                return db.Queryable<TEntity>().Where(expression).First();
            }
        }



#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.SelectSearch<TEntity>(IEnumerable<Expression<Func<TEntity, bool>>>)”的 XML 注释
        public IEnumerable<TEntity> SelectSearch<TEntity>(IEnumerable<Expression<Func<TEntity, bool>>> expression) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.SelectSearch<TEntity>(IEnumerable<Expression<Func<TEntity, bool>>>)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                var queryable = db.Queryable<TEntity>();
                foreach (var where in expression)
                {
                    queryable = queryable.Where(where);
                }
                return queryable.ToList();
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.SelectSearch<TEntity>(string, IEnumerable<Expression<Func<TEntity, bool>>>)”的 XML 注释
        public IEnumerable<TEntity> SelectSearch<TEntity>(string whereSql, IEnumerable<Expression<Func<TEntity, bool>>> expression) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.SelectSearch<TEntity>(string, IEnumerable<Expression<Func<TEntity, bool>>>)”的 XML 注释
        {
            using (var db = GetInstance())
            {

                var queryable = db.Queryable<TEntity>();
                queryable = queryable.Where(whereSql);
                foreach (var where in expression)
                {
                    queryable = queryable.Where(where);
                }
                return queryable.ToList();
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.GetTotalCount<TEntity>(Expression<Func<TEntity, bool>>, string)”的 XML 注释
        public int GetTotalCount<TEntity>(Expression<Func<TEntity, bool>> expression, string sqlwhere = "") where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.GetTotalCount<TEntity>(Expression<Func<TEntity, bool>>, string)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                if (!string.IsNullOrEmpty(sqlwhere))
                    return db.Queryable<TEntity>().Where(expression).Where(sqlwhere).Count();
                return db.Queryable<TEntity>().Where(expression).Count();
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.GetTotalCount<TEntity>(string)”的 XML 注释
        public int GetTotalCount<TEntity>(string sqlwhere) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.GetTotalCount<TEntity>(string)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                if (!string.IsNullOrEmpty(sqlwhere))
                    return db.Queryable<TEntity>().Where(sqlwhere).Count();
                return db.Queryable<TEntity>().Count();
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.GetTotalCount<TEntity>(IEnumerable<string>, string)”的 XML 注释
        public int GetTotalCount<TEntity>(IEnumerable<string> Ids, string Flide) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.GetTotalCount<TEntity>(IEnumerable<string>, string)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().In(Flide, Ids).Count();
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.GetTotalCount<TEntity>(Expression<Func<TEntity, bool>>, IEnumerable<string>, string)”的 XML 注释
        public int GetTotalCount<TEntity>(Expression<Func<TEntity, bool>> expression, IEnumerable<string> Ids, string Flide) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.GetTotalCount<TEntity>(Expression<Func<TEntity, bool>>, IEnumerable<string>, string)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().Where(expression).In(Flide, Ids).Count();
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.SelectDynamic<TEntity>(string, object)”的 XML 注释
        public dynamic SelectDynamic<TEntity>(string sqlstr, object param = null) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.SelectDynamic<TEntity>(string, object)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                return db.Ado.SqlQueryDynamic(sqlstr, param);
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.SelectString(string, object)”的 XML 注释
        public string SelectString(string sqlstr, object param = null)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.SelectString(string, object)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                return db.Ado.GetString(sqlstr, param);
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.SelectStringBySql<TEntity>(string, object)”的 XML 注释
        public TEntity SelectStringBySql<TEntity>(string sqlstr, object param = null) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.SelectStringBySql<TEntity>(string, object)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                if (!string.IsNullOrEmpty(sqlstr))
                {
                    return db.Ado.SqlQuery<TEntity>(sqlstr, param).SingleOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.SelectDataTable(string, object)”的 XML 注释
        public DataTable SelectDataTable(string sql, object obj = null)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.SelectDataTable(string, object)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                return db.Ado.GetDataTable(sql, obj);
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.SelectDataSet(string, object)”的 XML 注释
        public DataSet SelectDataSet(string sql, object obj = null)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.SelectDataSet(string, object)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                return db.Ado.GetDataSetAll(sql, obj);
            }
        }
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Repository.SelectSearch<TEntity, T2>(Expression<Func<TEntity, T2, bool>>, Expression<Func<TEntity, T2, bool>>)”的 XML 注释
        public List<TEntity> SelectSearch< TEntity, T2>(Expression<Func<TEntity, T2, bool>> whereExpression, Expression<Func<TEntity, T2, bool>> joinOn) where TEntity : class, new()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Repository.SelectSearch<TEntity, T2>(Expression<Func<TEntity, T2, bool>>, Expression<Func<TEntity, T2, bool>>)”的 XML 注释
        {
            using (var db = GetInstance())
            {
                List<TEntity> list = db.Queryable<TEntity, T2>(joinOn).Where(whereExpression).ToList();
                //return db.Queryable<TEntity, T2>((t1, t2) => new object[] { JoinType.Inner, t1 }).Where(whereExpression).ToList();
                //return db.Queryable<T>().JoinTable<T2>(joinOn).Where(whereExpression).ToList();
                return list;
            }
        }
        /// <summary>
        /// SqlSugar分页查询
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="whereExpression"></param>
        /// <param name="joinOn"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public dynamic SelectPageList<T2, TEntity>(Expression<Func<TEntity, T2, object[]>> joinOn, Expression<Func<TEntity, T2, bool>> whereExpression, int pageIndex, int pageSize, ref int totalCount) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                IEnumerable<TEntity> list = db.Queryable<TEntity, T2>(joinOn).Where(whereExpression).ToPageList(pageIndex, pageSize, ref totalCount);
                return list;
            }
        }

        #endregion

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="pars">参数</param>
        /// <returns></returns>
        public dynamic ExecuteProcedure(string procName, SqlParameter[] pars = null)
        {
            using (var db = GetInstance())
            {
                var result = db.Ado.UseStoredProcedure<dynamic>(() =>
                {
                    return db.Ado.SqlQueryDynamic(procName, pars);
                });
                return result;
            }
        }

        /// <summary>
        /// 分页查询 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<T> SelectPage<T>(PageParameter<T> parameter, out int totalCount) where T : class, new()
        {
            using (var db = GetInstance())
            {
                ISugarQueryable<T> queryable;
                List<T> result = null;
                queryable = db.Queryable<T>();

                if (!string.IsNullOrEmpty(parameter.WhereSql))
                {
                    queryable = queryable.Where(parameter.WhereSql);
                }

                if (parameter.Wheres != null)
                {
                    foreach (var where in parameter.Wheres)
                    {
                        queryable = queryable.Where(where);
                    }
                }
                else
                {
                    if (parameter.Where != null)
                    {
                        queryable = queryable.Where(parameter.Where);
                    }
                }
                if (parameter.OrderColumns == null && string.IsNullOrEmpty(parameter.StrOrderColumns))
                {
                    throw new Exception("分页必须要排序。");
                }

                if (!string.IsNullOrEmpty(parameter.Field) && parameter.In != null && parameter.In.Count > 0)
                {
                    queryable = queryable.In(parameter.Field, parameter.In);
                }

                if (parameter.IsOrderByASC == 0)
                {
                    if (parameter.OrderColumns != null)
                    {
                        queryable = queryable.OrderBy(parameter.OrderColumns, OrderByType.Desc);
                    }
                }
                else
                {
                    if (parameter.OrderColumns != null)
                    {
                        queryable = queryable.OrderBy(parameter.OrderColumns);
                    }
                }
                if (parameter.OrderColumns2 != null)
                {
                    if (parameter.IsOrderByASC2 == 0)
                    {
                        if (parameter.OrderColumns2 != null)
                        {
                            queryable = queryable.OrderBy(parameter.OrderColumns2, OrderByType.Desc);
                        }
                    }
                    else
                    {
                        if (parameter.OrderColumns2 != null)
                        {
                            queryable = queryable.OrderBy(parameter.OrderColumns2);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(parameter.StrOrderColumns))
                {
                    queryable = queryable.OrderBy(parameter.StrOrderColumns);
                }

                totalCount = queryable.Count();
                result = queryable.ToPageList(parameter.PageIndex, parameter.PageSize);
                return result;
            }
        }

        /// <summary>
        /// 分页查询 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<T> SelectPageList<T>(string joinSql, PageParameter<T> parameter, out int totalCount) where T : class, new()
        {
            using (var db = GetInstance())
            {
                ISugarQueryable<T> queryable;
                List<T> result = null;
                queryable = db.SqlQueryable<T>(joinSql);

                if (!string.IsNullOrEmpty(parameter.WhereSql))
                {
                    queryable = queryable.Where(parameter.WhereSql);
                }

                if (parameter.Wheres != null)
                {
                    foreach (var where in parameter.Wheres)
                    {
                        queryable = queryable.Where(where);
                    }
                }
                else
                {
                    if (parameter.Where != null)
                    {
                        queryable = queryable.Where(parameter.Where);
                    }
                }
                if (parameter.OrderColumns == null && string.IsNullOrEmpty(parameter.StrOrderColumns))
                {
                    throw new Exception("分页必须要排序。");
                }

                if (!string.IsNullOrEmpty(parameter.Field) && parameter.In != null && parameter.In.Count > 0)
                {
                    queryable = queryable.In(parameter.Field, parameter.In);
                }

                if (parameter.IsOrderByASC == 0)
                {
                    if (parameter.OrderColumns != null)
                    {
                        queryable = queryable.OrderBy(parameter.OrderColumns, OrderByType.Desc);
                    }
                }
                else
                {
                    if (parameter.OrderColumns != null)
                    {
                        queryable = queryable.OrderBy(parameter.OrderColumns);
                    }
                }

                if (!string.IsNullOrEmpty(parameter.StrOrderColumns))
                {
                    queryable = queryable.OrderBy(parameter.StrOrderColumns);
                }

                totalCount = queryable.Count();
                result = queryable.ToPageList(parameter.PageIndex, parameter.PageSize);
                return result;
            }
        }

        private IList<T> ConvertToModel<T>(DataTable dt) where T : new()
        {

            IList<T> ts = new List<T>();// 定义集合
            Type type = typeof(T); // 获得此模型的类型
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();// 获得此模型的公共属性
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite) continue;
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

    }
}
