using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using CourseActivate.Core.Utility;
using SqlSugar;
using System.Data.SqlClient;
using CourseActivate.Resource.Constract.Model;

namespace CourseActivate.Framework.DAL
{
    public class Repository
    {
        public string _operatorError = string.Empty;

        private string _connectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["KingsunConnectionStr"].ConnectionString;
                }
                return _connectionString;
            }
        }

        public string SynConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["KingsunStudyConnectionStr"].ConnectionString;
                }
                return _connectionString;
            }
        }
        private string OutConnectionString = string.Empty;
        public Repository(string _connectionString)
        {
            OutConnectionString = _connectionString;
        }
        public Repository()
        {

        }

        public SqlSugarClient GetSynInstance()
        {
            var db = new SqlSugarClient(SynConnectionString); //SyntacticSugar(ConnectionString);
            return db;

        }


        public SqlSugarClient GetInstance()
        {
            //SyntacticSugar
            if (string.Empty == OutConnectionString)
            {
                var db = new SqlSugarClient(ConnectionString); //SyntacticSugar(ConnectionString);
                return db;
            }
            else
            {
                var db = new SqlSugarClient(OutConnectionString); //SyntacticSugar(ConnectionString);
                return db;
            }
        }
        public SqlSugarClient GetInstance(string connectionString)
        {
            var db = new SqlSugarClient(connectionString);
            return db;
        }

        #region 新增
        /// <summary>
        /// 插入(返回主键值)
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public object Insert<T>(T info, string[] array = null) where T : class, new()
        {
            using (var db = GetInstance())
            {
                if (array != null)
                {
                    db.DisableInsertColumns = array;
                }
                var result = db.Insert<T>(info);
                return result;
            }
        }

        /// <summary>
        /// 插入(返回主键值)
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public object SynInsert<T>(T info, string[] array = null) where T : class, new()
        {
            using (var db = GetSynInstance())
            {
                if (array != null)
                {
                    db.DisableInsertColumns = array;
                }
                var result = db.Insert<T>(info);
                return result;
            }
        }
        /// <summary>
        /// 批量插入（传入实体集合）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public List<object> InsertRange<T>(List<T> entities) where T : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.InsertRange<T>(entities, true);
                return result;
            }
        }

        /// <summary>
        /// 批量插入（传入实体集合）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public List<object> InsertRange<T>(List<T> entities, bool isIdentity) where T : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.InsertRange<T>(entities, isIdentity);
                return result;
            }
        }

        /// <summary>
        /// 批量插入（传入实体集合）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool SqlBulkCopy<T>(List<T> entities) where T : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.SqlBulkCopy<T>(entities);
                return result;
            }
        }

        #endregion


        #region 修改
        /// <summary>
        /// 修改(不需要修改的字段实体中不赋值)
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool Update<T>(T info) where T : class, new()
        {
            using (var db = GetInstance())
            {
                string[] NotColumns = GetNotUpdateCllos<T>(info);
                if (NotColumns != null && NotColumns.Length > 0)
                {
                    db.DisableUpdateColumns = NotColumns;
                }
                var result = db.Update<T>(info);
                return result;

            }
        }
        /// <summary>
        /// 更新指定列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="expr"></param>
        /// <returns></returns>
        public bool Update<T>(object obj, Expression<Func<T, bool>> expr) where T : class,new()
        {
            using (var db = GetInstance())
            {
                var result = db.Update<T>(obj, expr);
                return result;

            }
        }
        /// <summary>
        ///  修改(不需要修改的字段实体中不赋值)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="ex">手动指定忽略字段(o=>o.matername,o.agent_level.toString()非string类型的字段请toString())</param>
        /// <returns></returns>
        public bool Update<T>(T info, params Expression<Func<T, string>>[] ex) where T : class, new()
        {
            using (var db = GetInstance())
            {
                string[] NotColumns = GetNotUpdateCllos<T>(info);
                var IgNoreColumns = ex.Select(o => o.Body.ToString().Split('.')[1]).ToList();
                IgNoreColumns.AddRange(NotColumns);

                if (IgNoreColumns != null && IgNoreColumns.Count > 0)
                {
                    db.DisableUpdateColumns = IgNoreColumns.ToArray();
                }
                var result = db.Update<T>(info);
                return result;
            }
        }

        public bool Update<T>(T info, string[] disableUpdateCoulums) where T : class, new()
        {
            using (var db = GetInstance())
            {
                db.DisableUpdateColumns = disableUpdateCoulums;
                return db.Update<T>(info);
            }
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
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            string where = tableKey + "=@" + tableKey;

            sqlParams.Add(new SqlParameter { DbType = DbType.String, Value = keyProperty.GetValue(info), ParameterName = tableKey });//主键的值参数化.
            foreach (var c in updateProperties)
            {
                if (c.Name == tableKey)
                {
                    continue;//主键不能update
                }

                set += string.Format(c.Name + "=@" + c.Name + ",");
                if (!sqlParams.Select(o => o.ParameterName).Contains(c.Name))
                {
                    sqlParams.Add(new SqlParameter { DbType = DbType.String, Value = c.GetValue(info), ParameterName = c.Name });//参数不能重复
                }
            }

            set = set.Substring(0, set.Length - 1);//去掉最后一个,号

            string sql = String.Format(@"update {0} set {1}  where {2} ", table, set, where);
            using (var db = GetInstance())
            {
                var r = db.SqlQuery<int>(sql, sqlParams);//sql参数化
                return true;
            }
        }


        public bool CustomUpdateRange<T>(Expression<Func<T, string>> key, List<T> infos, params Expression<Func<T, string>>[] ex) where T : class, new()
        {
            var updateColumns = ex.Select(o => o.Body.ToString().Split('.')[1]).ToList();
            var updateProperties = typeof(T).GetProperties().Where(o => updateColumns.Contains(o.Name)).ToList();//需要更新的字段          

            string tableKey = key.Body.ToString().Split('.')[1];//主键
            var keyProperty = typeof(T).GetProperties().FirstOrDefault(o => o.Name == tableKey);
            string table = typeof(T).Name;

            List<SqlParameter> sqlParams = new List<SqlParameter>();
            string sql = "";
            for (int i = 0; i < infos.Count; i++)
            {
                var info = infos[i];
                string set = "";
                string where = tableKey + "=@" + (tableKey + i);
                sqlParams.Add(new SqlParameter { DbType = DbType.String, Value = keyProperty.GetValue(info), ParameterName = (tableKey + i) });//主键的值参数化.
                foreach (var c in updateProperties)
                {
                    if (c.Name == tableKey)
                    {
                        continue;//主键不能update
                    }

                    set += string.Format(c.Name + "=@" + c.Name + i + ",");
                    if (!sqlParams.Select(o => o.ParameterName).Contains(c.Name + i))
                    {//已存在的参数不能重复
                        sqlParams.Add(new SqlParameter { DbType = DbType.String, Value = c.GetValue(info), ParameterName = c.Name + i });
                    }
                }

                set = set.Substring(0, set.Length - 1);//去掉最后一个,号

                sql += String.Format(@"update {0} set {1}  where {2} ;", table, set, where);
            }
            using (var db = GetInstance())
            {
                var r = db.SqlQuery<int>(sql, sqlParams);//sql参数化
                return true;
            }

        }

        /// <summary>
        /// 获取实体中为null的字体返回数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public string[] GetNotUpdateCllos<T>(T t)
        {
            string DisableUpdateColumns = string.Empty;
            if (t == null)
            {
                return null;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (properties.Length <= 0)
            {
                return null;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                if (item.GetValue(t, null) == null)
                {
                    DisableUpdateColumns += item.Name + ",";
                }
            }
            if (!string.IsNullOrEmpty(DisableUpdateColumns))
                return DisableUpdateColumns.TrimEnd(',').Split(new char[] { ',' });
            return null;
        }
        #endregion


        #region 删除
        /// <summary>
        /// 物理删除(按ID)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete<T>(object id) where T : class, new()
        {
            using (var db = GetInstance())
            {
                var obj = db.Queryable<T>().InSingle(id);
                var result = db.Delete(obj);
                return result;
            }
        }
        /// <summary>
        /// 物理删除(按指定条件删除)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete<T>(Expression<Func<T, bool>> expr) where T : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Delete(expr);
                return result;
            }
        }

        /// <summary>
        /// 物理删除(批量删除)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteMore<T>(string Ids) where T : class, new()
        {
            string[] arrayids = Ids.Split(',');
            using (var db = GetInstance())
            {
                var result = db.Delete<T, string>(arrayids);
                return result;
            }
        }
        /// <summary>
        /// 逻辑删除(单个)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public bool LogicDelete<T>(Expression<Func<T, bool>> expr, string field) where T : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.FalseDelete<T>(field, expr);
                return result;
            }
        }

        /// <summary>
        /// 逻辑删除（批量）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public bool LogicDeleteMore<T>(string Ids, string field) where T : class, new()
        {
            string[] arrayids = Ids.Split(',');
            using (var db = GetInstance())
            {
                var result = db.FalseDelete<T, string>(field, arrayids);
                return result;
            }
        }

        #endregion


        #region 查询

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T Select<T>(object ID) where T : class, new()
        {
            using (var db = GetInstance())
            {
                var t = db.Queryable<T>().InSingle(ID);
                return t;
            }
        }

        public List<T> SqlQuery<T>(string sql, List<SqlParameter> pars)
        {
            using (var db = GetInstance())
            {
                return db.SqlQuery<T>(sql, pars);
            }
        }

        public List<T> SqlQuery<T>(string sql)
        {
            using (var db = GetInstance())
            {
                return db.SqlQuery<T>(sql);
            }
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public List<T> SelectAll<T>() where T : class, new()
        {
            using (var db = GetInstance())
            {
                var list = db.Queryable<T>().ToList();
                return list;
            }
        }
        /// <summary>
        /// in条件查询 
        /// </summary>
        /// <returns></returns>
        public List<T> SelectIn<T>(Expression<Func<T, bool>> expr, string field, List<string> Ins) where T : class, new()
        {
            using (var db = GetInstance())
            {
                var list = db.Queryable<T>().Where(expr).In(field, Ins).ToList();
                return list;
            }
        }

        /// <summary>
        /// in条件查询
        /// </summary>
        /// <returns></returns>
        public List<T> SelectIn<T>(string filedName, List<string> filedlist) where T : class, new()
        {
            using (var db = GetInstance())
            {
                var list = db.Queryable<T>().In(filedName, filedlist).ToList();
                return list;
            }
        }
        /// <summary>
        /// 搜索查询(多条件下使用 a=0 and b=1)
        /// </summary>
        /// <returns></returns>
        public List<T> SelectSearch<T>(Expression<Func<T, bool>> expression, int topNumber, string orderby = "") where T : class, new()
        {
            using (var db = GetInstance())
            {
                if (string.IsNullOrEmpty(orderby) && topNumber == 0)
                {
                    return db.Queryable<T>().Where(expression).ToList();
                }
                else if (string.IsNullOrEmpty(orderby) && topNumber > 0)
                {
                    return db.Queryable<T>().Where(expression).Take(topNumber).ToList();
                }
                else if (!string.IsNullOrEmpty(orderby) && topNumber == 0)
                {
                    return db.Queryable<T>().Where(expression).OrderBy(orderby).ToList();
                }
                else
                {
                    return db.Queryable<T>().Where(expression).OrderBy(orderby).Take(topNumber).ToList();
                }
            }
        }
        /// <summary>
        /// 查询指定字段
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="Field"></param>
        /// <returns></returns>
        public List<T> SelectAppointField<T>(Expression<Func<T, bool>> expression, string Field) where T : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<T>().Where(expression).Select(Field).ToList();
            }
        }


        public List<T> SelectSearch<T>(string whereSql) where T : class, new()
        {
            using (var db = GetInstance())
            {
                if (!string.IsNullOrEmpty(whereSql))
                {
                    return db.Queryable<T>().Where(whereSql).ToList();
                }
                else
                {
                    return null;
                }
            }
        }
        public List<T> SelectSearchs<T>(List<Expression<Func<T, bool>>> expressions, string Flids = "", List<string> InIds = null, string orderfile = "") where T : class, new()
        {
            using (var db = GetInstance())
            {
                Queryable<T> queryable;
                queryable = db.Queryable<T>();
                if (expressions != null)
                {
                    foreach (var where in expressions)
                    {
                        queryable = queryable.Where(where);
                    }
                }
                if (!string.IsNullOrEmpty(Flids) && InIds != null && InIds.Count > 0 && !string.IsNullOrEmpty(orderfile))
                    return queryable.In(Flids, InIds).OrderBy(orderfile).ToList();
                else if (!string.IsNullOrEmpty(Flids) && InIds != null && InIds.Count > 0)
                    return queryable.In(Flids, InIds).ToList();
                if (!string.IsNullOrEmpty(orderfile))
                    return queryable.OrderBy(orderfile).ToList();
                return queryable.ToList();
            }
        }

        public List<T> SelectGroupBy<T>(Expression<Func<T, bool>> expression, string groupbyfields) where T : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<T>().Where(expression).GroupBy<T>(groupbyfields).ToList();
            }
        }

        public List<T> SelectGroupBy<T>(Expression<Func<T, bool>> expression, string groupbyfields, string Flide, List<string> Ids) where T : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<T>().Where(expression).GroupBy<T>(groupbyfields).In(Flide, Ids).ToList();
            }
        }
        //Channel
        public List<T> SelectGroupBy<T>(string groupbyfield) where T : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<T>().GroupBy<T>(groupbyfield).ToList();
            }
        }

        public List<T> SelectSearch<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<T>().Where(expression).ToList();
            }
        }

        /// <summary>
        /// 动态查询条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<T> SelectSearch<T>(List<Expression<Func<T, bool>>> expression) where T : class, new()
        {
            using (var db = GetInstance())
            {
                Queryable<T> queryable;
                queryable = db.Queryable<T>();
                foreach (var where in expression)
                {
                    queryable = queryable.Where(where);
                }
                return queryable.ToList();
            }
        }

        /// <summary>
        /// 动态查询条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public List<T> SelectSearch<T>(string whereSql, List<Expression<Func<T, bool>>> expression) where T : class, new()
        {
            using (var db = GetInstance())
            {
                Queryable<T> queryable;
                queryable = db.Queryable<T>();
                queryable = queryable.Where(whereSql);
                foreach (var where in expression)
                {
                    queryable = queryable.Where(where);
                }
                return queryable.ToList();
            }
        }
        public List<T> SelectSearch<T, T2>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T2, object>> joinOn) where T : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<T>().JoinTable<T2>(joinOn).Where(whereExpression).ToList();
            }
        }

        public List<T1> SelectSearch<T1, T2>(Expression<Func<T1, T2, bool>> whereExpression, Expression<Func<T1, T2, object>> joinOn) where T1 : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<T1>().JoinTable<T2>(joinOn).Where(whereExpression).ToList();
            }
        }

        public List<T1> SelectSearch<T1, T2>(Expression<Func<T1, T2, object>> joinOn, Expression<Func<T1, object>> orderColumn, string filedName, List<string> filedlist) where T1 : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<T1>().JoinTable<T2>(joinOn).In(filedName, filedlist).OrderBy<T1>(orderColumn, OrderByType.Desc).ToList();
            }
        }

        public List<T> SelectSearch<T, T2>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T2, object>> joinOn, Expression<Func<T, object>> orderColumn) where T : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<T>().JoinTable<T2>(joinOn).Where(whereExpression).OrderBy(orderColumn, OrderByType.Desc).ToList();
            }
        }

        /// <summary>
        /// 获取总的记录数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTotalCount<T>(Expression<Func<T, bool>> expression, string sqlwhere = "") where T : class, new()
        {
            using (var db = GetInstance())
            {
                if (!string.IsNullOrEmpty(sqlwhere))
                    return db.Queryable<T>().Where(expression).Where(sqlwhere).Count();
                return db.Queryable<T>().Where(expression).Count();
            }
        }
        /// <summary>
        /// 获取总的记录数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTotalCount<T>(string sqlwhere) where T : class, new()
        {
            using (var db = GetInstance())
            {
                if (!string.IsNullOrEmpty(sqlwhere))
                    return db.Queryable<T>().Where(sqlwhere).Count();
                return db.Queryable<T>().Count();
            }
        }

        /// <summary>
        /// 获取总的记录数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTotalCount<T>(List<string> Ids, string Flide) where T : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<T>().In(Flide, Ids).Count();
            }
        }
        /// <summary>
        /// 获取总的记录数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTotalCount<T>(Expression<Func<T, bool>> expression, List<string> Ids, string Flide) where T : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<T>().Where(expression).In(Flide, Ids).Count();
            }
        }


        public IList<T> SelectPage<T>(PageParameter<T> parameter, out int totalCount) where T : class,new()
        {
            using (var db = GetInstance())
            {
                Queryable<T> queryable;
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

        /// <summary>
        /// 连表分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<T> SelectPage<T, T2>(PageParameter<T> parameter, Expression<Func<T, T2, object>> joinOn, out int totalCount) where T : class,new()
        {
            using (var db = GetInstance())
            {
                Queryable<T> queryable;
                List<T> result = null;

                queryable = db.Queryable<T>().JoinTable<T2>(joinOn);

                if (parameter.Where != null)
                {
                    queryable = queryable.Where(parameter.Where);
                }
                if (parameter.OrderColumns == null)
                {
                    throw new Exception("分页必须要排序。");
                }
                if (parameter.IsOrderByASC == 0)
                {
                    queryable = queryable.OrderBy(parameter.OrderColumns, OrderByType.Desc);
                }
                else
                {
                    queryable = queryable.OrderBy(parameter.OrderColumns);
                }
                totalCount = queryable.Count();
                result = queryable.Skip(parameter.PageIndex).Take(parameter.PageSize).ToList();
                return result;
            }
        }

        /// <summary>
        /// 查询动态字段，匿名对象
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public dynamic SelectDynamic(string sqlstr, object param = null)
        {
            using (var db = GetInstance())
            {
                return db.SqlQueryDynamic(sqlstr, param);
            }
        }

        public string SelectString(string sqlstr, object param = null)
        {
            using (var db = GetInstance())
            {
                return db.SqlQuery<string>(sqlstr, param).SingleOrDefault();
            }
        }

        public T SelectString<T>(string sqlstr, object param = null) where T : class,new()
        {
            using (var db = GetInstance())
            {
                if (!string.IsNullOrEmpty(sqlstr))
                {
                    return db.SqlQuery<T>(sqlstr, param).SingleOrDefault();
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 通过sql语句获取datatable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DataTable SelectDataTable(string sql, object obj = null)
        {
            using (var db = GetInstance())
            {
                return db.GetDataTable(sql, obj);
            }
        }
        #endregion

        /// <summary>
        /// 事务执行删除后新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int BusinessAffairs<T>(Expression<Func<T, bool>> expr, List<T> entities) where T : class, new()
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    db.Delete<T>(expr);
                    db.InsertRange<T>(entities, true);
                    db.CommitTran();//提交事务
                    return 2;
                }
                catch (Exception)
                {
                    db.RollbackTran();//回滚事务
                    throw;
                }
            }
        }

        /// <summary>
        /// 事务操作，单库
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TransactionOperate(List<RepositoryAction> actions)
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    foreach (var action in actions)
                    {
                        switch (action.Actions)
                        {
                            case Acitons.Insert:
                                db.DisableInsertColumns = action.DisableColumns;
                                var ri = db.Insert(action.Entity);
                                break;
                            case Acitons.InsertRange:
                                db.DisableInsertColumns = action.DisableColumns;
                                var rir = db.InsertRange(action.Entities);//obj 是否可行(待验证)
                                break;
                            case Acitons.Delete:
                                var rd = db.Delete(action.Entity);
                                break;
                            case Acitons.Update:
                                db.DisableUpdateColumns = action.DisableColumns;
                                var ru = db.Update(action.Entity);
                                break;
                            case Acitons.ExecuteSql:
                                db.ExecuteCommand(action.sql, action.Param);
                                break;
                        }
                    }
                    db.CommitTran();//提交事务
                    return true;
                }
                catch (Exception ex)
                {
                    TestLog4Net.LogHelper.Info("Error:数据库操作失败。" + ex.Message + "。参数信息:" + JsonHelper.EncodeJson(actions));
                    db.RollbackTran();//回滚事务
                    throw;
                }
            }
        }

        /// <summary>
        /// 父表软删除，子表硬删除，（部门特定方法）
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="table1Ids"></param>
        /// <param name="expression"></param>
        /// <param name="table2Ids"></param>
        /// <returns></returns>
        public bool TransactionDelete<T1, T2>(string[] table1Ids, Expression<Func<T2, object>> expression, string[] table2Ids)
            where T1 : class, new()
            where T2 : class, new()
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    db.Delete<T1, string>(table1Ids);//硬删除
                    //db.Update<T1, string>(new { delflg = 1 }, table1Ids);//软删除，固定标识
                    db.Delete<T2, string>(expression, table2Ids);
                    return true;
                }
                catch (Exception)
                {
                    db.RollbackTran();//回滚事务
                    throw;
                }
            }
        }

        /// <summary>
        /// 事务连表插入，父子表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TransactionAdd<T1, T2>(RelationEntity<T1, T2> relationEntity)
            where T1 : class, new()
            where T2 : class, new()
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    db.DisableInsertColumns = relationEntity.ParentDisableColumns;
                    var newParentId = Convert.ToInt32(db.Insert(relationEntity.ParentEntity));
                    foreach (var item in relationEntity.ChildrenEntities)
                    {
                        //设置item对应的父id
                        Type t2 = typeof(T2);
                        t2.GetProperty(relationEntity.ParentIdName).SetValue(item, newParentId);
                        db.DisableInsertColumns = relationEntity.ChildrenDisableColumns;
                        db.Insert(item);
                    }
                    db.CommitTran();//提交事务
                    return true;
                }
                catch (Exception)
                {
                    db.RollbackTran();//回滚事务
                    throw;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public IList<T> ExecuteProcedure<T>(string procName, List<SqlParameter> pars = null)
        {
            using (var db = GetInstance())
            {
                db.CommandType = CommandType.StoredProcedure; //指定为存储过程可比上面少写EXEC和参数
                List<T> spResult = new List<T>();
                if (pars != null)
                {
                    spResult = db.SqlQuery<T>(procName, pars);
                }
                else
                {
                    spResult = db.SqlQuery<T>(procName);
                }
                db.CommandType = CommandType.Text; //还原回默认
                return spResult;
            }
        }

        /// <summary>
        /// 执行操作性的数据库语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int ExecuteCommand(string sql, object obj = null)
        {
            using (var db = GetInstance())
            {
                return db.ExecuteCommand(sql, obj);
            }
        }

        /// <summary>
        /// 用事务包含sql并执行
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public bool ExcuteSqlWithTran(string strSql)
        {
            if (string.IsNullOrEmpty(strSql))
            {
                return true;
            }
            string sql = "set xact_abort on;begin try  begin tran  " + strSql + " select 1 commit tran end try begin catch if xact_state()=-1 rollback tran select -1 end catch";
            int i = ExecuteCommand(sql);
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool TranAddBook(Resource.Constract.Model.tb_res_book book, List<Resource.Constract.Model.tb_res_catalog> firstCatalog, List<Resource.Constract.Model.tb_res_catalog> secondCatalog, List<Resource.Constract.Model.tb_res_catalog> thirdCatalog)
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    int bookID;
                    if (book.BookID == 0)
                    {
                        bookID = Convert.ToInt32(db.Insert(book));
                        book.BookID = bookID;
                    }
                    else
                    {
                        bookID = book.BookID;
                        db.Update(book);
                    }

                    Dictionary<int, int> first = new Dictionary<int, int>();
                    Dictionary<int, int> second = new Dictionary<int, int>();

                    for (int i = 0; i < firstCatalog.Count; i++)
                    {
                        firstCatalog[i].BookID = bookID;
                        firstCatalog[i].Sort = i + 1;
                        var firstCatalogID = db.Insert(firstCatalog[i]);
                        first.Add(i, Convert.ToInt32(firstCatalogID));
                    }

                    if (secondCatalog.Any())
                    {
                        int firstParentID = first[secondCatalog[0].ParentID];
                        for (int i = 0; i < secondCatalog.Count; i++)
                        {
                            secondCatalog[i].BookID = bookID;
                            secondCatalog[i].Sort = i + 1;
                            int key = secondCatalog[i].ParentID;
                            int parentID = first[key];
                            if (parentID != firstParentID)
                            {
                                db.Update<tb_res_catalog>(new { PageNoEnd = secondCatalog[i - 1].PageNoEnd }, x => x.CatalogID == firstParentID);
                                firstParentID = parentID;
                            }
                            secondCatalog[i].ParentID = parentID;
                            var secondCatalogID = db.Insert(secondCatalog[i]);
                            second.Add(i, Convert.ToInt32(secondCatalogID));
                            if (i == secondCatalog.Count - 1)
                            {
                                db.Update<tb_res_catalog>(new { PageNoEnd = secondCatalog[i].PageNoEnd }, x => x.CatalogID == parentID);
                            }
                        }

                        if (thirdCatalog.Any())
                        {
                            for (int i = 0; i < thirdCatalog.Count; i++)
                            {
                                thirdCatalog[i].BookID = bookID;
                                thirdCatalog[i].Sort = i + 1;
                                int key = thirdCatalog[i].ParentID;
                                int parentID = second[key];
                                thirdCatalog[i].ParentID = parentID;
                                var thirdCatalogID = db.Insert(thirdCatalog[i]);
                            }
                        }
                    }


                    db.CommitTran();//提交事务
                    return true;
                }
                catch (Exception)
                {
                    db.RollbackTran();//回滚事务
                    throw;
                }
            }
        }

        public bool TranDelBook(int bookID)
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    db.Delete<tb_res_book>("BookID=" + bookID);
                    db.Delete<tb_res_catalog>("BookID=" + bookID);
                    db.Delete<tb_res_resource>("BookID=" + bookID);
                    db.CommitTran();//提交事务
                    return true;
                }
                catch (Exception)
                {
                    db.RollbackTran();//回滚事务
                    throw;
                }
            }
        }

        public bool TranBantchDelBook(string bookIDs)
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    db.Delete<tb_res_book>("BookID in (" + bookIDs + ")");
                    db.Delete<tb_res_catalog>("BookID in (" + bookIDs + ")");
                    db.Delete<tb_res_resource>("BookID in (" + bookIDs + ")");
                    db.CommitTran();//提交事务
                    return true;
                }
                catch (Exception)
                {
                    db.RollbackTran();//回滚事务
                    throw;
                }
            }
        }
    }
}
