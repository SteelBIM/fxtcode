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
    public class RepositoryOther
    {
        public string _operatorError = string.Empty;

        private string _connectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    string xmlPath = System.Web.HttpContext.Current.Server.MapPath("~/Config/DaoConfig.xml");
                    XDocument xdc = new XDocument(XDocument.Load(xmlPath));

                    var pElement = xdc.Elements().First();
                    //List<DbEntity> dbs = new List<DbEntity>();
                    foreach (var n in pElement.Elements())
                    {
                        //  dbs.Add(new DbEntity { DbName = n.Name.LocalName, ConnectString = n.Value });
                        if (n.Name.LocalName == "BaseDB")
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
        public RepositoryOther(string _connectionName)
        {
            string xmlPath = System.Web.HttpContext.Current.Server.MapPath("~/Config/DaoConfig.xml");
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

            OutConnectionString = _connectionString;
        }

        public RepositoryOther()
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
        public object Insert<TEntity>(TEntity entity) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Insertable(entity).ExecuteReturnIdentity();
                return result;
            }
        }

        public TEntity InsertReturnEntity<TEntity>(TEntity info, string[] array = null) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                TEntity result = db.Insertable<TEntity>(info).IgnoreColumns(i => array).ExecuteReturnEntity();
                return result;
            }
        }

        public int InsertRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Insertable<TEntity>(entities).Where(false).ExecuteCommand();
                return result;
            }
        }

        public int InsertRange<TEntity>(IEnumerable<TEntity> entities, bool isIdentity) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Insertable<TEntity>(entities).Where(false, !isIdentity).ExecuteCommand();
                return result;
            }
        }

        public IEnumerable<TEntity> InsertBatch<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Update
        public bool Update<TEntity>(TEntity entity) where TEntity : class, new()
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

        public bool Update<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> includes = null) where TEntity : class, new()
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

        public bool UpdateWithNull<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> includes = null) where TEntity : class, new()
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

        public bool UpdateIgnoreColumns<TEntity>(TEntity entity, Expression<Func<TEntity, object>> includes = null) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var sql = db.Updateable<TEntity>(entity).Where(true).IgnoreColumns(includes).ToSql();
                //Kingsun.Core.Log.Log.Info("更新语句：", Core.Utility.JsonHelper.EncodeJson(sql));
                //var result = db.Updateable<T>(info).Where(true).IgnoreColumns(ex).ExecuteCommand().ObjToBool();
                int i = db.Updateable<TEntity>(entity).Where(true).IgnoreColumns(includes).ExecuteCommand();
                return i > 0;
            }
        }

        public bool Update<TEntity>(TEntity entity, string[] disableUpdateCoulums) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Updateable<TEntity>(entity).Where(false).IgnoreColumns(it => disableUpdateCoulums).ExecuteCommand();
                return result > 0;
            }
        }

        public bool UpdateColumns<TEntity>(TEntity entity, Expression<Func<TEntity, object>> includes = null) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Updateable<TEntity>(entity).UpdateColumns(includes).ExecuteCommand();
                return result > 0;
            }
        }

        public bool MyUpdate<TEntity>(TEntity entity, string tableKey, string[] notUpdateColum = null) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Updateable<TEntity>(entity).IgnoreColumns(it => notUpdateColum).ExecuteCommand();
                return result > 0;
            }
        }

        public bool UpdateAssign<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> includes = null) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Updateable<TEntity>(entity).Where(includes).ExecuteCommand();
                return result > 0;
            }
        }
        #endregion

        #region Delete
        public bool Delete<TEntity>(object id) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Deleteable<TEntity>().In(id).ExecuteCommand();
                return result > 0;
            }
        }

        public bool Delete<TEntity>(Expression<Func<TEntity, bool>> expr) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Deleteable(expr).ExecuteCommand();
                return result > 0;
            }
        }

        public bool DeleteMore<TEntity>(string[] Ids) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                // var result = db.Delete<T, string>(arrayids);

                var result = db.Deleteable<TEntity>(Ids).ExecuteCommand();
                return result > 0;
            }
        }

        public void DeleteBatch<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Query
        public TEntity GetByID<TEntity>(object id) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var t = db.Queryable<TEntity>().InSingle(id);
                return t;
            }
        }

        public IEnumerable<TEntity> SqlQuery<TEntity>(string sql, IEnumerable<SqlParameter> pars = null) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().Where(sql, pars).ToList();
            }
        }


        public IEnumerable<TEntity> ListAll<TEntity>() where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var list = db.Queryable<TEntity>().ToList();
                return list;
            }
        }

        public IEnumerable<TEntity> SelectIn<TEntity>(Expression<Func<TEntity, bool>> expr, string field, IEnumerable<string> Ins) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var list = db.Queryable<TEntity>().Where(expr).In(field, Ins).ToList();
                return list;
            }
        }

        public IEnumerable<TEntity> SelectIn<TEntity>(string filedName, IEnumerable<string> filedlist) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                var list = db.Queryable<TEntity>().In(filedName, filedlist).ToList();
                return list;
            }
        }

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
        public IEnumerable<TEntity> SelectSearch<TEntity>(Expression<Func<TEntity, bool>> expression,  string orderby) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
               return db.Queryable<TEntity>().Where(expression).OrderBy(orderby).ToList();   
            }
        }
        public IEnumerable<TEntity> SelectAppointField<TEntity>(Expression<Func<TEntity, bool>> expression, string Field) where TEntity : class,new()
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

        public IEnumerable<TEntity> SelectSearchs<TEntity>(IEnumerable<Expression<Func<TEntity, bool>>> expressions, string Flids = "", IEnumerable<string> InIds = null, string orderfile = "") where TEntity : class, new()
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

        public IEnumerable<TEntity> SelectGroupBy<TEntity>(Expression<Func<TEntity, bool>> expression, string groupbyfields) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().Where(expression).GroupBy(groupbyfields).ToList();
            }
        }

        public IEnumerable<TEntity> SelectGroupBy<TEntity>(Expression<Func<TEntity, bool>> expression, string groupbyfields, string Flide, List<string> Ids) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> SelectGroupBy<TEntity>(string groupbyfield) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().GroupBy(groupbyfield).ToList();
            }
        }

        public IEnumerable<TEntity> SelectSearch<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                //var sql = db.Queryable<TEntity>().Where(expression).ToSql();
                return db.Queryable<TEntity>().Where(expression).ToList();
            }
        }

        public IEnumerable<TEntity> SelectSearch<TEntity>(IEnumerable<Expression<Func<TEntity, bool>>> expression) where TEntity : class, new()
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

        public IEnumerable<TEntity> SelectSearch<TEntity>(string whereSql, IEnumerable<Expression<Func<TEntity, bool>>> expression) where TEntity : class, new()
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

        public int GetTotalCount<TEntity>(Expression<Func<TEntity, bool>> expression, string sqlwhere = "") where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                if (!string.IsNullOrEmpty(sqlwhere))
                    return db.Queryable<TEntity>().Where(expression).Where(sqlwhere).Count();
                return db.Queryable<TEntity>().Where(expression).Count();
            }
        }

        public int GetTotalCount<TEntity>(string sqlwhere) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                if (!string.IsNullOrEmpty(sqlwhere))
                    return db.Queryable<TEntity>().Where(sqlwhere).Count();
                return db.Queryable<TEntity>().Count();
            }
        }

        public int GetTotalCount<TEntity>(IEnumerable<string> Ids, string Flide) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().In(Flide, Ids).Count();
            }
        }

        public int GetTotalCount<TEntity>(Expression<Func<TEntity, bool>> expression, IEnumerable<string> Ids, string Flide) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().Where(expression).In(Flide, Ids).Count();
            }
        }

        public dynamic SelectDynamic<TEntity>(string sqlstr, object param = null) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Ado.SqlQueryDynamic(sqlstr, param);
            }
        }

        public string SelectString(string sqlstr, object param = null)
        {
            using (var db = GetInstance())
            {
                return db.Ado.GetString(sqlstr, param);
            }
        }

        public TEntity SelectStringBySql<TEntity>(string sqlstr, object param = null) where TEntity : class, new()
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

        public DataTable SelectDataTable(string sql, object obj = null)
        {
            using (var db = GetInstance())
            {
                return db.Ado.GetDataTable(sql, obj);
            }
        }

        public DataSet SelectDataSet(string sql, object obj = null)
        {
            using (var db = GetInstance())
            {
                return db.Ado.GetDataSetAll(sql, obj);
            }
        }
        public IEnumerable<TEntity> SelectSearch<T2, TEntity>(Expression<Func<TEntity, T2, bool>> whereExpression, Expression<Func<TEntity, T2, bool>> joinOn) where TEntity : class, new()
        {
            using (var db = GetInstance())
            {
                IEnumerable<TEntity> list = db.Queryable<TEntity, T2>(joinOn).Where(whereExpression).ToList();

                //return db.Queryable<T>().JoinTable<T2>(joinOn).Where(whereExpression).ToList();
                return list;
            }
        }
        #endregion

        public dynamic ExecuteProcedure(string procName, List<SqlParameter> pars = null)
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
