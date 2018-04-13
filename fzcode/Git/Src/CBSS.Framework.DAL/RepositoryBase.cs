using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace CBSS.Framework.DAL
{
     public  class RepositoryBase<TEntity> where TEntity : class,new()
    {
        public string _operatorError = string.Empty;

        private string _connectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ConfigurationManager.ConnectionStrings["KingsunConnectionStr"].ConnectionString;
                }
                return _connectionString;
            }
        }

        private string OutConnectionString = string.Empty;
        public RepositoryBase(string _connectionString)
        {
            OutConnectionString = _connectionString;
        }
        public RepositoryBase()
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
        public object Insert(TEntity entity)
        {
            using (var db = GetInstance())
            {
                var result = db.Insertable<TEntity>(entity).ExecuteReturnIdentity();
                return result;
            }
        }

        public TEntity InsertReturnEntity(TEntity info, string[] array = null)
        {
            using (var db = GetInstance())
            {
                TEntity result = db.Insertable<TEntity>(info).IgnoreColumns(i => array).ExecuteReturnEntity();
                return result;
            }
        }

        public int InsertRange(IEnumerable<TEntity> entities)
        {
            using (var db = GetInstance())
            {
                var result = db.Insertable<TEntity>(entities).Where(false).ExecuteCommand();
                return result;
            }
        }

        public int InsertRange(IEnumerable<TEntity> entities, bool isIdentity)
        {
            using (var db = GetInstance())
            {
                var result = db.Insertable<TEntity>(entities).Where(false, !isIdentity).ExecuteCommand();
                return result;
            }
        }

        public IEnumerable<TEntity> InsertBatch(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Update
        public bool Update(TEntity entity)
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

        public bool Update(TEntity entity, Expression<Func<TEntity, bool>> includes = null)
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

        public bool UpdateWithNull(TEntity entity, Expression<Func<TEntity, bool>> includes = null)
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

        public bool UpdateIgnoreColumns(TEntity entity, Expression<Func<TEntity, object>> includes = null)
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

        public bool Update(TEntity entity, string[] disableUpdateCoulums)
        {
            using (var db = GetInstance())
            {
                var result = db.Updateable<TEntity>(entity).Where(false).IgnoreColumns(it => disableUpdateCoulums).ExecuteCommand();
                return result > 0;
            }
        }

        public bool UpdateColumns(TEntity entity, Expression<Func<TEntity, object>> includes = null)
        {
            using (var db = GetInstance())
            {
                var result = db.Updateable<TEntity>(entity).UpdateColumns(includes).ExecuteCommand();
                return result > 0;
            }
        }

        public bool MyUpdate(TEntity entity, string tableKey, string[] notUpdateColum = null)
        {
            using (var db = GetInstance())
            {
                var result= db.Updateable<TEntity>(entity).IgnoreColumns(it => notUpdateColum).ExecuteCommand();
                return result > 0;
            }
        }

        public bool UpdateAssign(TEntity entity, Expression<Func<TEntity, bool>> includes = null)
        {
            using (var db = GetInstance())
            {
                var result= db.Updateable<TEntity>(entity).Where(includes).ExecuteCommand();
                return result > 0;
            }
        }
        #endregion

        #region Delete
        public bool Delete(object id)
        {
            using (var db = GetInstance())
            {
                var result = db.Deleteable<TEntity>().In(id).ExecuteCommand();
                return result > 0;
            }
        }

        public bool Delete(Expression<Func<TEntity, bool>> expr)
        {
            using (var db = GetInstance())
            {
                var result = db.Deleteable(expr).ExecuteCommand();
                return result > 0;
            }
        }

        public bool DeleteMore(string Ids)
        {
            string[] arrayids = Ids.Split(',');
            using (var db = GetInstance())
            {
                // var result = db.Delete<T, string>(arrayids);

                var result = db.Deleteable<TEntity>(arrayids).ExecuteCommand();
                return result > 0;
            }
        }

        public void DeleteBatch(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Query
        public TEntity GetByID(object id)
        {
            using (var db = GetInstance())
            {
                var t = db.Queryable<TEntity>().InSingle(id);
                return t;
            }
        }

        public IEnumerable<TEntity> SqlQuery(string sql, IEnumerable<SqlParameter> pars = null)
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().Where(sql, pars).ToList();
            }
        }


        public IEnumerable<TEntity> ListAll()
        {
            using (var db = GetInstance())
            {
                var list = db.Queryable<TEntity>().ToList();
                return list;
            }
        }

        public IEnumerable<TEntity> SelectIn(Expression<Func<TEntity, bool>> expr, string field, IEnumerable<string> Ins)
        {
            using (var db = GetInstance())
            {
                var list = db.Queryable<TEntity>().Where(expr).In(field, Ins).ToList();
                return list;
            }
        }

        public IEnumerable<TEntity> SelectIn(string filedName, IEnumerable<string> filedlist)
        {
            using (var db = GetInstance())
            {
                var list = db.Queryable<TEntity>().In(filedName, filedlist).ToList();
                return list;
            }
        }

        public IEnumerable<TEntity> SelectSearch(Expression<Func<TEntity, bool>> expression, int topNumber, string orderby = "")
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

        public IEnumerable<TEntity> SelectAppointField(Expression<Func<TEntity, bool>> expression, string Field)
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().Where(expression).Select(Field).ToList();
            }
        }

        public IEnumerable<TEntity> SelectSearch(string whereSql)
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

        public IEnumerable<TEntity> SelectSearchs(IEnumerable<Expression<Func<TEntity, bool>>> expressions, string Flids = "", IEnumerable<string> InIds = null, string orderfile = "")
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

        public IEnumerable<TEntity> SelectGroupBy(Expression<Func<TEntity, bool>> expression, string groupbyfields)
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().Where(expression).GroupBy(groupbyfields).ToList();
            }
        }

        public IEnumerable<TEntity> SelectGroupBy(Expression<Func<TEntity, bool>> expression, string groupbyfields, string Flide, List<string> Ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> SelectGroupBy(string groupbyfield)
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().GroupBy(groupbyfield).ToList();
            }
        }

        public IEnumerable<TEntity> SelectSearch(Expression<Func<TEntity, bool>> expression)
        {
            using (var db = GetInstance())
            {
                var sql = db.Queryable<TEntity>().Where(expression).ToSql();
                return db.Queryable<TEntity>().Where(expression).ToList();
            }
        }

        public IEnumerable<TEntity> SelectSearch(IEnumerable<Expression<Func<TEntity, bool>>> expression)
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

        public IEnumerable<TEntity> SelectSearch(string whereSql, IEnumerable<Expression<Func<TEntity, bool>>> expression)
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

        public int GetTotalCount(Expression<Func<TEntity, bool>> expression, string sqlwhere = "")
        {
            using (var db = GetInstance())
            {
                if (!string.IsNullOrEmpty(sqlwhere))
                    return db.Queryable<TEntity>().Where(expression).Where(sqlwhere).Count();
                return db.Queryable<TEntity>().Where(expression).Count();
            }
        }

        public int GetTotalCount(string sqlwhere)
        {
            using (var db = GetInstance())
            {
                if (!string.IsNullOrEmpty(sqlwhere))
                    return db.Queryable<TEntity>().Where(sqlwhere).Count();
                return db.Queryable<TEntity>().Count();
            }
        }

        public int GetTotalCount(IEnumerable<string> Ids, string Flide)
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().In(Flide, Ids).Count();
            }
        }

        public int GetTotalCount(Expression<Func<TEntity, bool>> expression, IEnumerable<string> Ids, string Flide)
        {
            using (var db = GetInstance())
            {
                return db.Queryable<TEntity>().Where(expression).In(Flide, Ids).Count();
            }
        }

        public dynamic SelectDynamic(string sqlstr, object param = null)
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

        public TEntity SelectStringBySql(string sqlstr, object param = null)
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

        public IEnumerable<TEntity> SelectSearch<T2>(Expression<Func<TEntity, T2, bool>> whereExpression, Expression<Func<TEntity, T2, bool>> joinOn)
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
    }
}

