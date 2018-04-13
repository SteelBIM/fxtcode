using CourseActivate.Core.Utility;
using CourseActivate.Framework.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CourseActivate.Resource.Constract.Model;
using System.Diagnostics;

//using MySql.Data.MySqlClient;

namespace CourseActivate.Framework.BLL
{
    public class Manage
    {
        public Manage() { }

        Repository repository = new Repository();

        public bool TranAddBook(tb_res_book book, List<tb_res_catalog> firstCatalog, List<tb_res_catalog> secondCatalog, List<tb_res_catalog> thirdCatalog)
        {
            return repository.TranAddBook(book, firstCatalog, secondCatalog, thirdCatalog);
        }

        public bool TranDelBook(int bookID)
        {
            return repository.TranDelBook(bookID);
        }

        public bool TranBatchDelBook(string bookIDs)
        {
            return repository.TranBantchDelBook(bookIDs);
        }

        public IList<T> ExecuteProcedure<T>(string procName, Dictionary<string, object> dis = null)
        {
            List<SqlParameter> pars = new List<SqlParameter>();
            foreach (var item in dis)
            {
                SqlParameter msp = new SqlParameter();
                msp.ParameterName = item.Key;
                msp.Value = item.Value;
                pars.Add(msp);
            }
            return repository.ExecuteProcedure<T>(procName, pars);
        }

        public int Add<T>(T subdata, string[] array = null) where T : class, new()
        {
            object ID = repository.Insert<T>(subdata, array);
            if (ID != null && ID != DBNull.Value && !string.IsNullOrEmpty(ID.ToString()))
                return int.Parse(ID.ToString());
            return 0;
        }
        public object Insert<T>(T subdata) where T : class, new()
        {
            object ID = repository.Insert<T>(subdata, null);
            return ID;
        }
        public string SynInsert<T>(T subdata, string[] array = null) where T : class, new()
        {
            object ID = repository.SynInsert<T>(subdata, array);
            if (ID != null && ID != DBNull.Value && !string.IsNullOrEmpty(ID.ToString()))
                return ID.ToString();
            return "";
        }
        /// <summary>
        /// 批量插入（传入实体集合）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public List<object> InsertRange<T>(List<T> entities) where T : class, new()
        {
            return repository.InsertRange<T>(entities);
        }

        /// <summary>
        /// 批量插入（传入实体集合）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public List<object> InsertRange<T>(List<T> entities, bool isIdentity) where T : class, new()
        {
            return repository.InsertRange<T>(entities, isIdentity);
        }

        /// <summary>
        /// 海量数据批量插入（传入实体集合）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns></returns>
        public bool SqlBulkCopy<T>(List<T> entities) where T : class, new()
        {
            return repository.SqlBulkCopy<T>(entities);
        }

        /// <summary>
        /// 海量数据批量插入（传入实体集合）_原生ADO方式,HLW封装
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="tableName">表名,可不传,默认T的类名,</param>
        /// <returns></returns>
        public KingResponse MSSqlBulkCopy<T>(List<T> entities, string tableName) where T : class, new()
        {
            try
            {
                var type = typeof(T);
                var entity = entities.FirstOrDefault();
                if (entity == null) return KingResponse.GetErrorResponse("无数据!");
                var properties = type.GetProperties().ToList();
                DataTable dt = new DataTable();

                // dt.Columns.Add("activateid", typeof(int));
                // dt.Columns.Add("activatecode", typeof(string));
                // dt.Columns.Add("batchid", typeof(int));
                //// dt.Columns.Add("createtime", typeof(DateTime));
                // dt.Columns.Add("ismatch", typeof(int));
                foreach (var f in properties)//根据属性名和类型生成列
                {
                    if (f.PropertyType == typeof(Guid) || f.PropertyType == typeof(Guid?))
                    {
                        dt.Columns.Add(f.Name, typeof(Guid));
                    }
                    else
                    {
                        dt.Columns.Add(f.Name, typeof(string));
                    }
                }
                entities.ForEach(o =>
                {
                    DataRow row = dt.NewRow();
                    properties.ForEach(f =>
                    {
                        row[f.Name] = f.GetValue(o);
                    });
                    dt.Rows.Add(row);
                });
                //ado批量insert:
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = type.Name;
                }
                //Stopwatch watch = new Stopwatch();
                //watch.Start();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(repository.ConnectionString, SqlBulkCopyOptions.Default))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.BatchSize = dt.Rows.Count;
                    bulkCopy.WriteToServer(dt);
                }
                // watch.Stop();
                return KingResponse.GetResponse("插入成功");
            }
            catch (Exception ex)
            {
                return KingResponse.GetErrorResponse(ex.Message);
            }
        }
        /// <summary>
        /// 修改（不需要修改的字段实体不赋值）
        /// </summary>
        /// <param name="subdata"></param>
        /// <returns></returns>
        public bool Update<T>(T subdata) where T : class, new()
        {
            return repository.Update<T>(subdata);
        }

        public bool Update<T>(object obj, Expression<Func<T, bool>> expr) where T : class,new()
        {
            return repository.Update<T>(obj, expr);
        }
        /// <summary>
        ///  修改(不需要修改的字段实体中不赋值)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="ex">忽略字段(o=>o.matername,o.agent_level.toString()非string类型的字段请toString())</param>
        /// <returns></returns>
        public bool Update<T>(T info, params Expression<Func<T, string>>[] ex) where T : class, new()
        {
            return repository.Update<T>(info, ex);
        }
        public bool Update<T>(T info, string[] disableUpdateCoulums) where T : class, new()
        {
            return repository.Update<T>(info, disableUpdateCoulums);
        }


        /// <summary>
        /// 指定字段更新(hlw封装)
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="key">主键</param>
        /// <param name="info">实体</param>
        /// <param name="ex">需要更新的参数</param>
        /// <returns></returns>
        public bool CustomUpdateEntity<T>(Expression<Func<T, string>> key, T info, params Expression<Func<T, string>>[] ex)
        {
            return repository.CustomUpdateEntity<T>(key, info, ex);
        }

        /// <summary>
        /// 指定字段批量更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="infos"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public bool CustomUpdateRange<T>(Expression<Func<T, string>> key, List<T> infos, params Expression<Func<T, string>>[] ex) where T : class, new()
        {
            return repository.CustomUpdateRange<T>(key, infos, ex);
        }

        public List<T> SelectGroupBy<T>(Expression<Func<T, bool>> expression, string groupbyfields = "") where T : class, new()
        {
            return repository.SelectGroupBy<T>(expression, groupbyfields);
        }

        public List<T> SelectGroupBy<T>(Expression<Func<T, bool>> expression, string groupbyfields, string Flide, List<string> Ids) where T : class, new()
        {
            return repository.SelectGroupBy<T>(expression, groupbyfields, Flide, Ids);
        }

        public List<T> SelectGroupBy<T>(string groupbyfield) where T : class, new()
        {
            return repository.SelectGroupBy<T>(groupbyfield);
        }
        /// <summary>
        /// 批量删除(物理删除(1,2,3))
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public bool DeleteMore<T>(string Ids) where T : class, new()
        {
            return repository.DeleteMore<T>(Ids);
        }
        /// <summary>
        /// 批量将指定字段修改成true
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="filed"></param>
        /// <returns></returns>
        public bool LogicDeleteMore<T>(string Ids, string filed) where T : class, new()
        {
            return repository.LogicDeleteMore<T>(Ids, filed);
        }
        /// <summary>
        /// 将指定字段修改成true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public bool LogicDelete<T>(Expression<Func<T, bool>> expr, string field) where T : class, new()
        {
            return repository.LogicDelete<T>(expr, field);
        }
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public List<T> SelectAll<T>() where T : class, new()
        {
            return repository.SelectAll<T>();
        }

        /// <summary>
        /// in条件查询
        /// </summary>
        /// <returns></returns>
        public List<T> SelectIn<T>(string filedName, List<string> filedlist) where T : class, new()
        {
            var list = repository.SelectIn<T>(filedName, filedlist);
            return list;
        }

        public List<T> SelectIn<T>(Expression<Func<T, bool>> where, string filedName, List<string> filedlist) where T : class, new()
        {
            return repository.SelectIn<T>(where, filedName, filedlist);
        }

        public List<T1> SelectSearch<T1, T2>(Expression<Func<T1, T2, bool>> whereExpression, Expression<Func<T1, T2, object>> joinOn) where T1 : class, new()
        {
            return repository.SelectSearch<T1, T2>(whereExpression, joinOn);
        }

        public List<T1> SelectSearch<T1, T2>(Expression<Func<T1, T2, object>> joinOn, Expression<Func<T1, object>> orderColumn, string filedName, List<string> filedlist) where T1 : class, new()
        {
            return repository.SelectSearch<T1, T2>(joinOn, orderColumn, filedName, filedlist);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T Select<T>(object ID) where T : class, new()
        {
            return repository.Select<T>(ID);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<T> SelectWhere<T>(string whereSql) where T : class, new()
        {
            return repository.SelectSearch<T>(whereSql);
        }

        public List<T> SqlQuery<T>(string sql, List<SqlParameter> pars)
        {
            return repository.SqlQuery<T>(sql, pars);
        }

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTotalCount<T>(Expression<Func<T, bool>> expression, string sqlwhere = "") where T : class, new()
        {
            return repository.GetTotalCount<T>(expression, sqlwhere);
        }
        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTotalCount<T>(string sqlwhere = "") where T : class, new()
        {
            return repository.GetTotalCount<T>(sqlwhere);
        }



        public int GetTotalCount<T>(List<string> Ids, string Flide) where T : class, new()
        {
            return repository.GetTotalCount<T>(Ids, Flide);
        }
        public int GetTotalCount<T>(Expression<Func<T, bool>> expression, List<string> Ids, string Flide) where T : class, new()
        {
            return repository.GetTotalCount<T>(expression, Ids, Flide);
        }

        /// <summary>
        /// 搜索查询(多条件下使用 a=0 and b=1)
        /// </summary>
        /// <returns></returns>
        public List<T> SelectSearch<T>(Expression<Func<T, bool>> expression, int topNumber, string orderby = "") where T : class, new()
        {
            return repository.SelectSearch<T>(expression, topNumber, orderby);
        }
        /// <summary>
        /// 查询指定字段
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="Field"></param>
        /// <returns></returns>
        public List<T> SelectAppointField<T>(Expression<Func<T, bool>> expression, string Field) where T : class, new()
        {
            return repository.SelectAppointField<T>(expression, Field);
        }

        public List<T> SelectSearchs<T>(List<Expression<Func<T, bool>>> exprs, string Flids = "", List<string> InIds = null, string orderfile = "") where T : class, new()
        {
            return repository.SelectSearchs<T>(exprs, Flids, InIds, orderfile);
        }

        public List<T> SelectSearch<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return repository.SelectSearch<T>(expression);
        }

        public List<T> SelectSearch<T>(string whereSql) where T : class, new()
        {
            return repository.SelectSearch<T>(whereSql);
        }

        public List<T> SelectSearch<T, T2>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T2, object>> joinOn) where T : class, new()
        {
            return repository.SelectSearch<T, T2>(whereExpression, joinOn);
        }



        public List<T> SelectSearch<T, T2>(Expression<Func<T, bool>> whereExpression, Expression<Func<T, T2, object>> joinOn, Expression<Func<T, object>> orderColumn) where T : class, new()
        {
            return repository.SelectSearch<T, T2>(whereExpression, joinOn, orderColumn);
        }

        public dynamic SelectDynamic(string sqlstr, object param = null)
        {
            return repository.SelectDynamic(sqlstr, param);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter"></param>
        /// <param name="TotalCount">总记录数</param>
        /// <returns></returns>
        public IList<T> SelectPage<T>(PageParameter<T> parameter, out int totalCount) where T : class ,new()
        {
            return repository.SelectPage<T>(parameter, out totalCount);
        }

        public IList<T> SelectPage<T, T2>(PageParameter<T> parameter, Expression<Func<T, T2, object>> joinOn, out int totalCount) where T : class ,new()
        {
            return repository.SelectPage<T, T2>(parameter, joinOn, out totalCount);
        }
        public bool Delete<T>(Expression<Func<T, bool>> expr) where T : class, new()
        {
            return repository.Delete<T>(expr); ;
        }

        public bool Delete<T>(object Id) where T : class, new()
        {
            return repository.Delete<T>(Id); ;
        }
        /// <summary>
        /// 修改权限执行事务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public int UpdateActionBusinessAffairs<T>(Expression<Func<T, bool>> expr, List<T> entities) where T : class, new()
        {
            return repository.BusinessAffairs<T>(expr, entities);
        }

        public bool TransactionOperate(List<RepositoryAction> actions)
        {
            return repository.TransactionOperate(actions);
        }

        public bool TransactionAdd<T1, T2>(RelationEntity<T1, T2> relationEntity)
            where T1 : class, new()
            where T2 : class, new()
        {
            return repository.TransactionAdd<T1, T2>(relationEntity);
        }

        public bool TransactionDelete<T1, T2>(string[] table1Ids, Expression<Func<T2, object>> expression, string[] table2Ids)
            where T1 : class, new()
            where T2 : class, new()
        {
            return repository.TransactionDelete<T1, T2>(table1Ids, expression, table2Ids);
        }

        public bool ExcuteSqlWithTran(string strSql)
        {
            return repository.ExcuteSqlWithTran(strSql);
        }

        public DataTable SelectDataTable(string sql, object obj = null)
        {
            return repository.SelectDataTable(sql, obj);
        }

        public string SelectString(string strSql)
        {
            return repository.SelectString(strSql);
        }

        public int ExecuteCommand(string strSql)
        {
            return repository.ExecuteCommand(strSql);
        }
    }
}
