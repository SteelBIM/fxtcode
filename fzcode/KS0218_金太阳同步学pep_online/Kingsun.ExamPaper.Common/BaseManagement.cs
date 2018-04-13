using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Kingsun.DB;
using System.Linq;
namespace Kingsun.ExamPaper.Common
{
    public class BaseManagement
    {
        protected string _operatorError = string.Empty;

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IList<T> SearchAll<T>() where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            IList<T> list = dbManage.Search<T>("1=1");
            _operatorError = dbManage.ErrorMsg;
            return list;
        }

        /// <summary>
        /// 按条件（和排序规则）查询
        /// </summary>
        /// <returns></returns>
        public IList<T> Search<T>(string where, string orderby = "") where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            if (string.IsNullOrEmpty(orderby))
            {
                IList<T> list = dbManage.Search<T>(where);
                _operatorError = dbManage.ErrorMsg;
                return list;
            }
            else
            {
                IList<T> list = dbManage.Search<T>(where, orderby);
                _operatorError = dbManage.ErrorMsg;
                return list;
            }
        }

        /// <summary>
        /// 按id查询
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public T Select<T>(object id) where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            T t = dbManage.Select<T>(id);
            _operatorError = dbManage.ErrorMsg;
            return t;
        }

        /// <summary>
        /// 按条件单个查询
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public T SelectByCondition<T>(string where) where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            T t = dbManage.SelectByCondition<T>(where);
            _operatorError = dbManage.ErrorMsg;
            return t;
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public bool Insert<T>(T info) where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            bool result = dbManage.Insert<T>(info);
            _operatorError = dbManage.ErrorMsg;
            return result;
        }

        /// <summary>
        /// 插入,同步学数据库
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public bool InsertSync<T>(T info) where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.SyncConnectionString);
            bool result = dbManage.Insert<T>(info);
            _operatorError = dbManage.ErrorMsg;
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public bool Update<T>(T info) where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            bool result = dbManage.Update<T>(info);
            _operatorError = dbManage.ErrorMsg;
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete<T>(Guid id) where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            bool result = dbManage.Delete<T>(id);
            _operatorError = dbManage.ErrorMsg;
            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete<T>(object id) where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            bool result = dbManage.Delete<T>(id);
            _operatorError = dbManage.ErrorMsg;
            return result;
        }

        /// <summary>
        /// 多项删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="IDs"></param>
        /// <returns></returns>
        public bool Delete<T>(List<object> IDs) where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            bool result = dbManage.Delete<T>(IDs);
            _operatorError = dbManage.ErrorMsg;
            return result;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public DataSet ExecuteSql(string sqlstr)
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            DataSet ds = dbManage.ExecuteQuery(sqlstr);
            _operatorError = dbManage.ErrorMsg;
            return ds;
        }


        public List<T> SearchObj<T>(string sql) where T : Kingsun.DB.Action, new()
        {
            var ds = ExecuteSql(sql);
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
            {
                var obj = DataTableHelper.ConvertTo<T>(ds.Tables[0]);
                return obj.ToList();
            }
            return null;
        }

        /// <summary>
        /// 执行SQL语句,对应同步学的数据库
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public DataSet SyncExecuteSql(string sqlstr)
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.SyncConnectionString);
            DataSet ds = dbManage.ExecuteQuery(sqlstr);
            _operatorError = dbManage.ErrorMsg;
            return ds;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public DataSet ExecuteSql(string sqlstr, List<DbParameter> list)
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            DataSet ds = dbManage.ExecuteQuery(sqlstr, list);
            _operatorError = dbManage.ErrorMsg;
            return ds;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ProcName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public DataSet ExecuteProcedure(string ProcName, List<DbParameter> list)
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            DataSet ds = dbManage.ExecuteProcedure(ProcName, list);
            _operatorError = dbManage.ErrorMsg;
            return ds;
        }
        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <typeparam name="T">要查询的Model</typeparam>
        /// <param name="TableName">对应的表名或视图名</param>
        /// <param name="PageIndex">当前页码</param>
        /// <param name="PageSize">每页记录数</param>
        /// <param name="Where">查询条件</param>
        /// <param name="OrderColumn">排序列，多个以逗号分隔</param>
        /// <param name="OrderType">排序类型，1：升序，2：降序</param>
        /// <param name="TotalCount">总条数</param>
        /// <param name="TotalPages">总页数</param>
        /// <returns></returns>
        public IList<T> GetPageList<T>(string TableName, int PageIndex, int PageSize, string Where, string OrderColumn, int OrderType, out int TotalCount, out int TotalPages) where T : Kingsun.DB.Action, new()
        {
            TotalCount = 0;
            TotalPages = 0;
            PageParameter param = new PageParameter();
            param.Columns = "*";
            param.TbNames = TableName;
            param.PageSize = PageSize;
            param.PageIndex = PageIndex;
            param.OrderColumns = OrderColumn;
            param.IsOrderByASC = OrderType;
            param.Where = Where;
            List<DbParameter> list = param.getParameterList();
            DataSet ds = ExecuteProcedure("Proc_pageView", list);
            if (ds == null)
            {
                return null;
            }
            else
            {
                TotalCount = (int)ds.Tables[1].Rows[0][0];
                TotalPages = (int)ds.Tables[1].Rows[0][1];
                return FillData<T>(ds.Tables[0]);
            }
        }

        /// <summary>
        /// 将table数据转换为list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<T> FillData<T>(DataTable dt) where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            return dbManage.FillData<T>(dt);
        }

        /// <summary>
        /// 获取总的记录数
        /// </summary>
        /// <param name="tablename">表名</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public int GetTotalCount(string tablename, string where)
        {
            string sql = "select count(1) from " + tablename + " where " + where;
            DataSet ds = ExecuteSql(sql);
            if (ds == null || ds.Tables.Count == 0)
            {
                return 0;
            }
            return (int)ds.Tables[0].Rows[0][0];
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
            DataSet ds = ExecuteSql(sql);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows[0][0].ToString() == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}