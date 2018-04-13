using KSWF.Core.Utility;
using KSWF.Framework.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

//using MySql.Data.MySqlClient;

namespace KSWF.Framework.BLL
{
    public class OrderBaseManage
    {
        static string _connectionString = "";
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["KingsunConnectionOrderStr"].ConnectionString;
                }
                return _connectionString;
            }
        }

        public Repository repository = new Repository(ConnectionString);

        public int Add<T>(T subdata) where T : class, new()
        {
            object ID = repository.Insert<T>(subdata);
            if (ID != null && ID != DBNull.Value && !string.IsNullOrEmpty(ID.ToString()))
                if (ID.ToString().ToLower() == "true")
                    return 1;
            return int.Parse(ID.ToString());
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
        /// 修改（不需要修改的字段实体不赋值）
        /// </summary>
        /// <param name="subdata"></param>
        /// <returns></returns>
        public bool Update<T>(T subdata) where T : class, new()
        {
            return repository.Update<T>(subdata);
        }
        public bool Update<T>(T info, string[] disableUpdateCoulums) where T : class, new()
        {
            return repository.Update<T>(info, disableUpdateCoulums);
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

        /// <summary>
        /// 指定字段更新操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <param name="ex">指定字段(o=>o.matername,o.agent_level.toString())</param>
        /// <returns></returns>
        public bool CustomUpdate<T>(T info, params Expression<Func<T, string>>[] ex) where T : class, new()
        {
            return repository.CustomUpdate(info, ex);
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
        /// 根据ID查询
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public T Select<T>(string ID) where T : class, new()
        {
            return repository.Select<T>(ID);
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

        public List<T> SqlQuery<T>(string sql, List<MySqlParameter> pars)
        {
            return repository.SqlQuery<T>(sql, pars);
        }

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetTotalCount<T>(string expression) where T : class, new()
        {
            return repository.GetTotalCount<T>(expression);
        }
        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="Ids"></param>
        /// <param name="Flide"></param>
        /// <returns></returns>
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

        public List<T> SelectSearchs<T>(List<Expression<Func<T, bool>>> exprs, string Flids = "", List<string> InIds = null,string orderfile="") where T : class, new()
        {
            return repository.SelectSearchs<T>(exprs, Flids, InIds,  orderfile);
        }

        public List<T> SelectSearch<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return repository.SelectSearch<T>(expression);
        }

        public List<T> SelectSearch<T>(List<Expression<Func<T, bool>>> expression) where T : class, new()
        {
            return repository.SelectSearch<T>(expression);
        }

        public List<T> SelectSearch<T>(string whereSql, List<Expression<Func<T, bool>>> expression) where T : class, new()
        {
            return repository.SelectSearch<T>(whereSql, expression);
        }

        public List<T> SelectSearch<T>(string whereSql, Expression<Func<T, bool>> whereExpression, List<Expression<Func<T, bool>>> expression) where T : class, new()
        {
            return repository.SelectSearch<T>(whereSql,whereExpression, expression);
        }
 
        public List<T> SelectSearch<T, T2>(Expression<Func<T, bool>> expression, Expression<Func<T, T2, object>> joinOn) where T : class, new()
        {
            return repository.SelectSearch<T, T2>(expression, joinOn);
        }
        public dynamic SelectDynamic(string sqlstr, object param = null)
        {
            return repository.SelectDynamic(sqlstr, param);
        }
        public string SelectString(string sqlstr, object param = null)
        {
            return repository.SelectString(sqlstr, param);
        }

        public T SelectString<T>(string sqlstr, object param = null) where T : class, new()
        {
            return repository.SelectString<T>(sqlstr, param);
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

        //public List<T> SqlQuery<T>(string sql, List<MySqlParameter> pars)
        //{

        //    return repository.SqlQuery<T>(sql, pars);

        //}
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
    }
}
