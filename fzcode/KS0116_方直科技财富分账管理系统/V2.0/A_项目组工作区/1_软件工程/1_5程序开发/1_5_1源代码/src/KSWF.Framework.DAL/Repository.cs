using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using KSWF.Core.Utility;
using MySql.Data.MySqlClient;
using MySqlSugar;
using KSWF.WFM.Constract.Models;

namespace KSWF.Framework.DAL
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
        private string OutConnectionString = string.Empty;
        public Repository(string _connectionString)
        {
            OutConnectionString = _connectionString;
        }
        public Repository()
        {

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
        public void InsertR<T>(T info) where T : class, new()
        {
            using (var db = GetInstance())
            {
                db.Insert<T>(info);
            }
        }

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

        public bool TranUpdate<T1, T2>(T1 info, string[] disableUpdateCoulums1, List<T2> info2s)
            where T1 : class, new()
            where T2 : class, new()
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    db.DisableUpdateColumns = disableUpdateCoulums1;
                    db.Update<T1>(info);

                    string deptid = typeof(T1).GetProperty("deptid").GetValue(info).ToString();
                    db.Delete<T2>("deptid = " + deptid);
                    db.InsertRange<T2>(info2s);

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

        public bool TranMergeUpLowerLevelDept(string deptname, int deptidA, int deptidB, List<int> deptIDsB, List<int> agentDeptIDsB, List<int> deptIDsA, List<int> agentDeptIDsA)
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务

                    //新增部门C
                    base_dept dept = db.Queryable<base_dept>().Where<base_dept>(x => x.deptid == deptidA).First();
                    base_dept deptC = new base_dept
                    {
                        deptname=deptname,
                        parentid=dept.parentid,
                        createname=dept.createname,
                        agentid=dept.agentid,
                        level=dept.level
                    };
                    db.DisableInsertColumns = new string[] {  "path", "isend",  "districtid", "schoolid", "schoolname"};
                    var deptidC = Convert.ToInt32(db.Insert<base_dept>(deptC));
                    
                    List<base_deptarea> deptareaC=db.Queryable<base_deptarea>().Where<base_deptarea>(x => x.deptid == deptidA).ToList();
                    foreach (var item in deptareaC)
                    {
                        item.deptid = deptidC;
                    }
                    db.DisableInsertColumns = new string[] { };
                    db.InsertRange<base_deptarea>(deptareaC);

                    //更新部门A，deptname， base_dept
                    //db.Update<base_dept, int>(new { deptname = deptname }, deptidA);
                    db.Delete<base_dept>("deptid=" + deptidA);
                    db.Delete<base_deptarea>("deptid=" + deptidA);
                    if (deptIDsA.Count > 0)
                    {
                        db.Update<base_dept, int>(new { parentid = deptidC }, deptIDsA.ToArray());
                    }
                    db.ExecuteCommand("Update fz_wfs.com_master set deptid =" + deptidC + " where deptid = " + deptidA);
                    if (agentDeptIDsA.Count > 0)
                    {
                        db.Update<base_dept, int>(new { parentid = deptidC }, agentDeptIDsA.ToArray());
                    }

                    //删除部门B，base_dept  base_deptarea    
                    db.Delete<base_dept>("deptid=" + deptidB);
                    db.Delete<base_deptarea>("deptid=" + deptidB);

                    //更新部门B的子级部门（一级）
                    if (deptIDsB.Count > 0)
                    {
                        db.Update<base_dept, int>(new { parentid = deptidC }, deptIDsB.ToArray());
                    }

                    //更新部门B的员工 （一级）
                    db.ExecuteCommand("Update fz_wfs.com_master set deptid =" + deptidC + " where deptid = " + deptidB);

                    //更新部门B的代理商部门（一级）
                    if (agentDeptIDsB.Count > 0)
                    {
                        db.Update<base_dept, int>(new { parentid = deptidC }, agentDeptIDsB.ToArray());
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

        public bool TranMergeBrotherLevelDept(string deptname, int deptidA, int deptidB, List<int> deptIDsB, List<int> agentDeptIDsB, List<base_deptarea> newDeptareas, List<int> deptIDsA, List<int> agentDeptIDsA)
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务

                    //新增部门C
                    base_dept dept = db.Queryable<base_dept>().Where<base_dept>(x => x.deptid == deptidA).First();
                    base_dept deptC = new base_dept
                    {
                        deptname = deptname,
                        parentid = dept.parentid,
                        createname = dept.createname,
                        agentid = dept.agentid,
                        level = dept.level
                    };
                    db.DisableInsertColumns = new string[] { "path", "isend", "districtid", "schoolid", "schoolname"};
                    var deptidC = Convert.ToInt32(db.Insert<base_dept>(deptC));

                    foreach (var item in newDeptareas)
                    {
                        item.deptid = deptidC;
                    }
                    db.DisableInsertColumns = new string[] { };
                    db.InsertRange<base_deptarea>(newDeptareas);

                    //更新部门A，deptname， base_dept
                    //db.Update<T1, int>(new { deptname = deptname }, deptidA);
                    //db.Delete<T2>("deptid=" + deptidA);
                    //db.InsertRange(newDeptareas);

                    db.Delete<base_dept>("deptid=" + deptidA);
                    db.Delete<base_deptarea>("deptid=" + deptidA);
                    if (deptIDsA.Count > 0)
                    {
                        db.Update<base_dept, int>(new { parentid = deptidC }, deptIDsA.ToArray());
                    }
                    db.ExecuteCommand("Update fz_wfs.com_master set deptid =" + deptidC + " where deptid = " + deptidA);
                    if (agentDeptIDsA.Count > 0)
                    {
                        db.Update<base_dept, int>(new { parentid = deptidC }, agentDeptIDsA.ToArray());
                    }

                    //删除部门B，base_dept  base_deptarea
                    db.Delete<base_dept>("deptid=" + deptidB);
                    db.Delete<base_deptarea>("deptid=" + deptidB);

                    //更新部门B的子级部门（一级）
                    if (deptIDsB.Count > 0)
                    {
                        db.Update<base_dept, int>(new { parentid = deptidC }, deptIDsB.ToArray());
                    }

                    //更新部门B的员工 （一级）
                    db.ExecuteCommand("Update fz_wfs.com_master set deptid =" + deptidC + " where deptid = " + deptidB);

                    //更新部门B的代理商部门（一级）
                    if (agentDeptIDsB.Count > 0)
                    {
                        db.Update<base_dept, int>(new { parentid = deptidC }, agentDeptIDsB.ToArray());
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
        /// 更新操作(手动指定要更新的字段)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="ex">o=>o.matername,o.agent_level.toString()</param>
        /// <returns></returns>
        public bool CustomUpdate<T>(T info, params Expression<Func<T, string>>[] ex) where T : class, new()
        {
            var updateColumns = ex.Select(o => o.Body.ToString().Split('.')[1]).ToList();
            var properties = info.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            var disaledColumns = properties.Where(o => !updateColumns.Contains(o.Name)).Select(o => o.Name).ToArray();
            using (var db = GetInstance())
            {
                if (disaledColumns != null && disaledColumns.Length > 0)
                {
                    db.DisableUpdateColumns = disaledColumns;
                }
                var result = db.Update<T>(info);
                return result;
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
        /// 物理删除(批量删除)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteMore<T>(Expression<Func<T, object>> expr, List<string> Ins) where T : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Delete<T, string>(expr, Ins);
                return result;
            }
        }
        public bool DeleteMore<T>(Expression<Func<T, object>> expr, List<int> Ins) where T : class, new()
        {
            using (var db = GetInstance())
            {
                var result = db.Delete<T, int>(expr, Ins);
                return result;
            }
        }

        //  return db.Queryable<T>().In(filedName, filedlist).ToList();

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

        public List<T> SqlQuery<T>(string sql, List<MySqlParameter> pars)
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
                if (Ins.Count > 0)
                {
                    var list = db.Queryable<T>().Where(expr).In(field, Ins).ToList();
                    return list;
                }
                else
                {
                    var list = db.Queryable<T>().Where(expr).ToList();
                    return list;
                }
            }
        }

        public List<T> SelectIn<T>(string expr, string field, List<string> Ins) where T : class, new()
        {
            using (var db = GetInstance())
            {
                if (Ins.Count > 0)
                {
                    var list = db.Queryable<T>().Where(expr).In(field, Ins).ToList();
                    return list;
                }
                else
                {
                    var list = db.Queryable<T>().Where(expr).ToList();
                    return list;
                }
            }
        }

        /// <summary>
        /// in条件查询
        /// </summary>
        /// <returns></returns>
        public List<T> SelectIn<T>(string filedName, List<string> filedlist, string selectfile = "") where T : class, new()
        {
            using (var db = GetInstance())
            {
                if (!string.IsNullOrEmpty(selectfile))
                {

                    var list = db.Queryable<T>().In(filedName, filedlist).Select(selectfile).ToList();
                    return list;
                }

                return db.Queryable<T>().In(filedName, filedlist).ToList();
            }
        }


        /// <summary>
        /// 搜索查询（topnumber 与orderby需组合使用）
        /// </summary>
        /// <returns></returns>
        public List<T> SelectSearch<T>(Expression<Func<T, bool>> expression, int topNumber, string orderby = "") where T : class, new()
        {
            using (var db = GetInstance())
            {
                if (!string.IsNullOrEmpty(orderby) && topNumber == 0)
                {
                    return db.Queryable<T>().Where(expression).OrderBy(orderby).ToList();
                }
                else if (string.IsNullOrEmpty(orderby) && topNumber == 0)
                {
                    return db.Queryable<T>().Where(expression).ToList();
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


        public List<T> SelectSearch<T>(string whereSql, Expression<Func<T, bool>> whereExpression, List<Expression<Func<T, bool>>> expression) where T : class, new()
        {
            using (var db = GetInstance())
            {
                Queryable<T> queryable;
                queryable = db.Queryable<T>();
                queryable = queryable.Where(whereSql);
                queryable = queryable.Where(whereExpression);
                foreach (var where in expression)
                {
                    queryable = queryable.Where(where);
                }
                return queryable.ToList();
            }
        }
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

        public List<T1> SelectIn<T1, T2>(string where, string filedName, List<string> filedlist, Expression<Func<T1, T2, object>> joinOn) where T1 : class, new()
        {
            using (var db = GetInstance())
            {
                return db.Queryable<T1>().JoinTable<T2>(joinOn).Where(where).In(filedName, filedlist).ToList();
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
        public int GetTotalCount<T>(List<Expression<Func<T, bool>>> expression, string sqlwhere = "") where T : class, new()
        {
            using (var db = GetInstance())
            {
                Queryable<T> queryable;
                queryable = db.Queryable<T>();
                if (expression != null)
                {
                    foreach (var where in expression)
                    {
                        queryable = queryable.Where(where);
                    }
                    if (!string.IsNullOrEmpty(sqlwhere))
                        return queryable.Where(sqlwhere).Count();
                    return queryable.Count();
                }
            }
            return 0;
        }

        public List<T> SelectSearchs<T>(List<Expression<Func<T, bool>>> expressions, string sqlwhere) where T : class, new()
        {
            using (var db = GetInstance())
            {
                Queryable<T> queryable = db.Queryable<T>();
                if (expressions != null)
                {
                    foreach (var where in expressions)
                    {
                        queryable = queryable.Where(where);
                    }
                }
                if (!string.IsNullOrEmpty(sqlwhere))
                    return queryable.Where(sqlwhere).ToList();
                return queryable.ToList();
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
        /// <summary>
        /// 获取总的记录数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTotalCount<T>(List<Expression<Func<T, bool>>> expression, List<string> Ids, string Flide) where T : class, new()
        {
            using (var db = GetInstance())
            {
                Queryable<T> queryable;
                queryable = db.Queryable<T>();
                if (expression != null)
                {
                    foreach (var where in expression)
                    {
                        queryable = queryable.Where(where);
                    }
                }
                return queryable.In(Flide, Ids).Count();
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


        #region 事务

        /// <summary>
        /// 事务修改操作，指定三表同时修改（修改指定字段，指定修改条件）
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="obj1"></param>
        /// <param name="expr1"></param>
        /// <param name="obj2"></param>
        /// <param name="expr2"></param>
        /// <param name="obj3"></param>
        /// <param name="expr3"></param>
        /// <returns></returns>
        public bool Update<T1, T2, T3>(object obj1, Expression<Func<T1, bool>> expr1, object obj2, Expression<Func<T2, bool>> expr2, object obj3, Expression<Func<T3, bool>> expr3)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    if (obj1 != null && expr1 != null)
                        db.Update<T1>(obj1, expr1);
                    if (obj2 != null && expr2 != null)
                        db.Update<T2>(obj2, expr2);
                    if (obj3 != null && expr3 != null)
                        db.Update<T3>(obj3, expr3);
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
        /// 事务修改删除操作，指定二表同时修改，一表删除（修改指定字段，指定修改条件）
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="obj1"></param>
        /// <param name="expr1"></param>
        /// <param name="obj2"></param>
        /// <param name="expr2"></param>
        /// <param name="obj3"></param>
        /// <param name="expr3"></param>
        /// <returns></returns>
        public bool UpdateDelete<T1, T2, T3>(Expression<Func<T1, object>> expr1, List<int> Ins1, Expression<Func<T2, object>> expr2, List<string> Ins2, object obj3, Expression<Func<T3, bool>> expr3)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    if (Ins1 != null && Ins1.Count > 0)
                        db.Delete<T1, int>(expr1, Ins1);//删除部门对应区域
                    if (Ins2 != null && Ins2.Count > 0)
                        db.Delete<T2, string>(expr2, Ins2);//删除人员对应区域
                    db.Update<T3>(obj3, expr3);//修改人员状态
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
        /// 事务执行删除后新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public int InsertRange<T0, T1>(List<T0> entities0, List<T1> entities1)
            where T0 : class, new()
            where T1 : class, new()
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    if (entities0 != null)
                        db.InsertRange<T0>(entities0, true);
                    if (entities1 != null)
                        db.InsertRange<T1>(entities1, true);

                    db.CommitTran();//提交事务
                    return 1;
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
                        // SqlQuery<T>(str, new List<MySqlParameter> { });
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

        public bool CarriedOutSql(List<string> sqls)
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    foreach (string rowsql in sqls)
                    {
                        db.SqlQueryJson(rowsql);
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



        public bool AgentDeleteArea<T0, T1, T2>(T0 agent, List<T2> newAgentAreas, List<T1> newbasedetparea, string deptIds, string employees, string childAgents)
            where T0 : class, new()
            where T1 : class, new()
            where T2 : class, new()
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务

                    string[] NotColumns = GetNotUpdateCllos<T0>(agent);
                    if (NotColumns != null && NotColumns.Length > 0)
                    {
                        db.DisableUpdateColumns = NotColumns;
                    }
                    db.Update<T0>(agent);

                    //更新代理商的负责区域
                    string agentMasternmae = typeof(T0).GetProperty("mastername").GetValue(agent).ToString();
                    string agentDeptid = typeof(T0).GetProperty("deptid").GetValue(agent).ToString();

                    db.Delete<T2>("mastername='" + agentMasternmae + "'");
                    db.InsertRange<T2>(newAgentAreas);

                    //更新代理商根部门负责区域
                    if (!string.IsNullOrEmpty(deptIds))
                    {
                        db.Delete<T1>("deptid in " + deptIds);//删除部门区域表 
                    }

                    db.Delete<T1>("deptid = " + agentDeptid);
                    db.InsertRange<T1>(newbasedetparea);

                    if (!string.IsNullOrEmpty(employees))
                    {
                        db.Delete<T2>("mastername in" + employees);//删除员工区域表
                    }

                    if (!string.IsNullOrEmpty(childAgents))
                    {
                        db.Delete<T2>("mastername in" + childAgents);//删除代理商区域表
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


        public bool AgentDeleteArea<T0, T1>(string deptaresIds, List<T0> deptarealist, string masterareaIds, List<T1> masterarealist)
            where T0 : class, new()
            where T1 : class, new()
        {
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务
                    if (!string.IsNullOrEmpty(deptaresIds))
                        db.Delete<T0>("id in( " + deptaresIds + ")");
                    db.InsertRange<T0>(deptarealist);
                    if (!string.IsNullOrEmpty(masterareaIds))
                        db.Delete<T1>("id in( " + masterareaIds + ")");
                    db.InsertRange<T1>(masterarealist);
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

        public bool TranMoveDept(List<base_deptarea> newDeptareas, Dictionary<int, List<base_deptarea>> needRemoveDepts, int deptidF, int deptidS, string agentid, out string errorMsg)
        {
            errorMsg = "";
            using (var db = GetInstance())
            {
                db.CommandTimeOut = 30000;//设置超时时间
                try
                {
                    db.BeginTran();//开启事务

                    //更新原父级的负责区域
                    if (needRemoveDepts.Count > 0)
                    {
                        foreach (var item in needRemoveDepts)
                        {
                            if (item.Value.Count > 0)
                            {
                                db.Delete<base_deptarea>(x => x.deptid == item.Key);
                                db.InsertRange<base_deptarea>(item.Value);
                            }
                        }
                    }

                    //更新新父级的负责区域
                    if (newDeptareas.Count > 0)
                    {
                        db.Delete<base_deptarea>(x => x.deptid == deptidF);
                        newDeptareas.ForEach(x => x.deptid = deptidF);
                        db.InsertRange<base_deptarea>(newDeptareas);
                    }

                    //判断更新后的deptidS的父部门，是否和他的子部门（一个子部门时）区域相等
                    int parentid = db.Queryable<base_dept>().Where(x => x.deptid == deptidS).First().parentid;
                    var pdeptareas = db.Queryable<base_deptarea>().Where(x => x.deptid == parentid).ToList();

                    var childs = db.Queryable<base_dept>().Where(x => x.parentid == parentid && x.agentid == agentid && x.deptid != deptidS).ToList();
                    if (childs.Count == 1)
                    {
                        int childid = childs.First().deptid;
                        var cdeptareas = db.Queryable<base_deptarea>().Where(x => x.deptid == childid).ToList();
                        if (pdeptareas.Count == cdeptareas.Count && pdeptareas.Count(t => !cdeptareas.Contains(t, new CompareDeptArea1())) == 0)
                        {
                            db.RollbackTran();
                            errorMsg = "不能移动！移动后会导致子部门与父部门的市场区域重合！";
                            return false;
                        }
                    }

                    //更新子部门的父部门id
                    db.Update<base_dept, int>(new { parentid = deptidF }, deptidS);
                    //更新子部门的部门层级
                    int levelS = db.Queryable<base_dept>().Where(x => x.deptid == deptidF).First().level + 1;
                    db.Update<base_dept, int>(new { level = levelS }, deptidS);

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
        #endregion


        #region 存储过程

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public IList<T> ExecuteProcedure<T>(string procName, List<MySqlParameter> pars = null)
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
        #endregion


        #region 操作语句
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
        #endregion

    }

    public class CompareDeptArea1 : IEqualityComparer<base_deptarea>
    {

        public bool Equals(base_deptarea x, base_deptarea y)
        {
            if (x.schoolid > 0 || y.schoolid > 0)
            {
                return x.districtid == y.districtid && x.schoolid == y.schoolid;
            }
            else
            {
                return x.districtid == y.districtid;
            }
        }

        public int GetHashCode(base_deptarea obj)
        {
            return obj.GetHashCode();
        }
    }
}
