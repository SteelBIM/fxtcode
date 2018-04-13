using KSWF.Core.Utility;
using KSWF.Framework.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using KSWF.WFM.Constract.Models;

//using MySql.Data.MySqlClient;

namespace KSWF.Framework.BLL
{
    public class Manage
    {
        public Manage() { }

        Repository repository = new Repository();

        public bool AgentDeleteArea<T0, T1, T2>(T0 agent, List<T2> newAgentAreas, List<T1> newbasedetparea, string deptIds, string employeeIds, string agentIds)
            where T0 : class , new()
            where T1 : class , new()
            where T2 : class , new()
        {
            return repository.AgentDeleteArea<T0, T1, T2>(agent, newAgentAreas, newbasedetparea, deptIds, employeeIds, agentIds);
        }

        public bool AgentDeleteArea<T0, T1>(string deptaresIds, List<T0> deptarealist,  string masterareaIds, List<T1> masterarealist)
            where T0 : class , new()
            where T1 : class , new()
        {
            return repository.AgentDeleteArea<T0, T1>(deptaresIds, deptarealist, masterareaIds,masterarealist);
        }

        public IList<T> ExecuteProcedure<T>(string procName, Dictionary<string, object> dis = null)
        {
            List<MySqlParameter> pars = new List<MySqlParameter>();
            foreach (var item in dis)
            {
                MySqlParameter msp = new MySqlParameter();
                msp.ParameterName = item.Key;
                msp.Value = item.Value;
                pars.Add(msp);
            }
            return repository.ExecuteProcedure<T>(procName, pars);
        }

        public bool CarriedOutSql(List<string> sqls)
        {

            return repository.CarriedOutSql(sqls);
        }



        public int Add<T>(T subdata, string[] array = null) where T : class, new()
        {
            object ID = repository.Insert<T>(subdata, array);
            if (ID != null && ID != DBNull.Value && !string.IsNullOrEmpty(ID.ToString()))
                return int.Parse(ID.ToString());
            return 0;
        }

        public void Insert<T>(T subdata) where T : class, new()
        {
            repository.InsertR<T>(subdata);
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

        public bool TranUpdate<T1, T2>(T1 info, string[] disableUpdateCoulums1, List<T2> info2s)
            where T1 : class, new()
            where T2 : class, new()
        {
            return repository.TranUpdate<T1, T2>(info, disableUpdateCoulums1, info2s);
        }

        public bool TranMergeUpLowerLevelDept(string deptname, int deptidA, int deptidB, List<int> deptIDsB, List<int> agentDeptIDsB, List<int> deptIDsA, List<int> agentDeptIDsA)
        {
            return repository.TranMergeUpLowerLevelDept(deptname, deptidA, deptidB, deptIDsB, agentDeptIDsB, deptIDsA, agentDeptIDsA);
        }

        public bool TranMergeBrotherLevelDept(string deptname, int deptidA, int deptidB, List<int> deptIDsB, List<int> agentDeptIDsB, List<base_deptarea> newDeptareas, List<int> deptIDsA, List<int> agentDeptIDsA)
        {
            return repository.TranMergeBrotherLevelDept(deptname, deptidA, deptidB, deptIDsB, agentDeptIDsB, newDeptareas, deptIDsA, agentDeptIDsA);
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
            return repository.CustomUpdate<T>(info, ex);
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
        public List<T> SelectIn<T>(string filedName, List<string> filedlist, string selectFile = "") where T : class, new()
        {
            var list = repository.SelectIn<T>(filedName, filedlist, selectFile);
            return list;
        }

        public List<T> SelectIn<T>(Expression<Func<T, bool>> where, string filedName, List<string> filedlist) where T : class, new()
        {
            return repository.SelectIn<T>(where, filedName, filedlist);
        }

        public List<T> SelectIn<T>(string where, string filedName, List<string> filedlist) where T : class, new()
        {
            return repository.SelectIn<T>(where, filedName, filedlist);
        }

        public List<T1> SelectIn<T1, T2>(string where, string filedName, List<string> filedlist, Expression<Func<T1, T2, object>> joinOn) 
            where T1 : class, new()
            where T2 : class, new()
        {
            return repository.SelectIn<T1,T2>(where, filedName, filedlist,joinOn);
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
        public List<T> SqlQuery<T>(string sql)
        {
            return repository.SqlQuery<T>(sql);
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
        public int GetTotalCount<T>(List<Expression<Func<T, bool>>> expression, string sqlwhere = "") where T : class, new()
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
        public int GetTotalCount<T>(List<Expression<Func<T, bool>>> expression, List<string> Ids, string Flide) where T : class, new()
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
        public List<T> SelectSearchs<T>(List<Expression<Func<T, bool>>> expression, string sqlwhere) where T : class, new()
        {
            return repository.SelectSearchs<T>(expression, sqlwhere);
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
        public int InsertRange<T0, T1>(List<T0> entities0, List<T1> entities1)
            where T0 : class, new()
            where T1 : class, new()
        {
            return repository.InsertRange<T0, T1>(entities0, entities1);
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

        public bool Update<T1, T2, T3>(object obj1, Expression<Func<T1, bool>> expr1, object obj2, Expression<Func<T2, bool>> expr2, object obj3, Expression<Func<T3, bool>> expr3)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
        {
            return repository.Update<T1, T2, T3>(obj1, expr1, obj2, expr2, obj3, expr3);
        }
        public bool UpdateDelete<T1, T2, T3>(Expression<Func<T1, object>> expr1, List<int> Ins1, Expression<Func<T2, object>> expr2, List<string> Ins2, object obj3, Expression<Func<T3, bool>> expr3)
            where T1 : class, new()
            where T2 : class, new()
            where T3 : class, new()
        {
            return repository.UpdateDelete<T1, T2, T3>(expr1, Ins1, expr2, Ins2, obj3, expr3);
        }

        public bool TranMoveDept(List<base_deptarea> newDeptareas, Dictionary<int, List<base_deptarea>> needRemoveDepts, int deptidF, int deptidS,string agentid,out string errorMsg)
        {
            return repository.TranMoveDept(newDeptareas,needRemoveDepts,deptidF,deptidS,agentid,out errorMsg );
        }
    }
}
