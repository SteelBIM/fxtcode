using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kingsun.DB;
using NPOI.SS.Formula.Functions;

namespace Kingsun.SynchronousStudy.Common.Base
{

    public class BaseManagement
    {
        public string _operatorError = string.Empty;
        public string bmConnection = string.Empty;

        private static string _connectionString = string.Empty;
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    string k = "KS0210KINGSUNSOFT2008123456789";
                    _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["kingsunconstr"].ConnectionString;
                    //_connectionString = xxtea.Decrypt(_connectionString, k);
                }
                return _connectionString;
            }
        }

        public static string GetConnectionString(string tablename)
        {
            string FZ_HearResources = "TB_BookResource|TB_HearResources|TB_UserHearResources|TB_BookResource_YX|TB_HearResources_YX|TB_UserHearResources_YX";
            string FZ_InterestDubbing = "TB_UserVideoDetails|TB_UserVideoDialogue|TB_VideoDetails|TB_VideoDialogue|TB_UserVideoDetails_YX|TB_UserVideoDialogue_YX|TB_VideoDetails_YX|TB_VideoDialogue_YX";
            if (FZ_HearResources.Contains(tablename))
                return System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_HearResources"].ConnectionString;
            if (FZ_InterestDubbing.Contains(tablename))
                return System.Configuration.ConfigurationManager.ConnectionStrings["kingsun_InterestDubbing"].ConnectionString;
            return System.Configuration.ConfigurationManager.ConnectionStrings["KingsunConnectionStr"].ConnectionString;

        }


        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IList<T> SelectAll<T>() where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(GetConnectionString(typeof(T).Name));
            IList<T> list = dbManage.Search<T>("1=1");
            _operatorError = dbManage.ErrorMsg;
            return list;
        }

        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <returns></returns>
        public IList<T> Search<T>(string where, string orderby = "") where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(GetConnectionString(typeof(T).Name));
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
        /// 查询
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public T Select<T>(Guid id) where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(GetConnectionString(typeof(T).Name));
            T t = dbManage.Select<T>(id);
            _operatorError = dbManage.ErrorMsg;
            return t;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public T Select<T>(object id) where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(GetConnectionString(typeof(T).Name));
            T t = dbManage.Select<T>(id);
            _operatorError = dbManage.ErrorMsg;
            bmConnection = dbManage.DbConnection.ConnectionString;
            return t;
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public T SelectByCondition<T>(string where) where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(GetConnectionString(typeof(T).Name));
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
            IDBManage dbManage = DBFactory.CreateDBManage(GetConnectionString(typeof(T).Name));
            bool result = dbManage.Insert<T>(info);
            _operatorError = dbManage.ErrorMsg;
            bmConnection = dbManage.DbConnection.ConnectionString;
            return result;
        }

      

        public IEnumerable<TEntity> InsertBatch<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public bool Update<T>(T info) where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(GetConnectionString(typeof(T).Name));
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
            IDBManage dbManage = DBFactory.CreateDBManage(GetConnectionString(typeof(T).Name));
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
            IDBManage dbManage = DBFactory.CreateDBManage(GetConnectionString(typeof(T).Name));
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
            IDBManage dbManage = DBFactory.CreateDBManage(GetConnectionString(typeof(T).Name));
            bool result = dbManage.Delete<T>(IDs);
            _operatorError = dbManage.ErrorMsg;
            return result;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public System.Data.DataSet ExecuteSql(string sqlstr)
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            System.Data.DataSet ds = dbManage.ExecuteQuery(sqlstr);
            _operatorError = dbManage.ErrorMsg;
            return ds;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public System.Data.DataSet ExecuteSql(string sqlstr, List<System.Data.Common.DbParameter> list)
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            System.Data.DataSet ds = dbManage.ExecuteQuery(sqlstr, list);
            _operatorError = dbManage.ErrorMsg;
            return ds;
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="ProcName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public System.Data.DataSet ExecuteProcedure(string ProcName, List<System.Data.Common.DbParameter> list)
        {
            IDBManage dbManage = DBFactory.CreateDBManage(AppSetting.ConnectionString);
            System.Data.DataSet ds = dbManage.ExecuteProcedure(ProcName, list);
            _operatorError = dbManage.ErrorMsg;
            return ds;
        }

        /// <summary>
        /// 将table数据转换为list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<T> FillData<T>(System.Data.DataTable dt) where T : Kingsun.DB.Action, new()
        {
            IDBManage dbManage = DBFactory.CreateDBManage(GetConnectionString(typeof(T).Name));
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
            System.Data.DataSet ds = ExecuteSql(sql);
            if (ds == null || ds.Tables.Count == 0)
            {
                return 0;
            }
            return (int)ds.Tables[0].Rows[0][0];
        }

        /// <summary> 
        /// DataSet装换为泛型集合 
        /// </summary> 
        /// <typeparam name="T"></typeparam> 
        /// <param name="ds">DataSet</param> 
        /// <param name="tableIndex">待转换数据表索引</param> 
        /// <returns></returns> 
        public static List<T> DataSetToIList<T>(DataSet ds, int tableIndex)
        {
            if (ds == null || ds.Tables.Count < 0)
                return null;
            if (tableIndex > ds.Tables.Count - 1)
                return null;
            if (tableIndex < 0)
                tableIndex = 0;

            DataTable p_Data = ds.Tables[tableIndex];
            // 返回值初始化 
            List<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, p_Data.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }





    }
}
