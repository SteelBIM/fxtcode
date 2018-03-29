using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.Transform;
using System.Data;
using System.Collections;
using NHibernate.Type;
using FxtDataAcquisition.DTODomain.NHibernate;

namespace FxtDataAcquisition.Data
{
    public class MSSQLDBDAL
    {
        private NHibernateHelper nhibernateHelper = new NHibernateHelper();
        protected ISession Session { get; set; }
        public MSSQLDBDAL()
        {
            this.Session = nhibernateHelper.GetSession();
        }
        /// <summary>
        /// 初始化MSSQLDBDAL
        /// </summary>
        /// <param name="factoryName">factoryName(配置文件session-factory节点name的值)</param>
        public MSSQLDBDAL(string factoryName)
        {
            this.Session = nhibernateHelper.GetSession(factoryName);
        }

        #region (写入操作)

        public bool Create(object objSysCity, ITransaction transaction = null)
        {
            bool result = true;
            object _robj = null;
            bool existTran = true;
            try
            {
                if (transaction == null)
                {
                    existTran = false;
                    transaction = Session.BeginTransaction();
                }
                _robj = Session.Save(objSysCity);
                if (!existTran)
                {
                    transaction.Commit();
                }
            }
            catch (Exception hexe)
            {
                if (transaction != null)
                {
                    if (!existTran)
                    {
                        transaction.Rollback();
                    }
                }
                _robj = 0;
                result = false;
            }
            return result;
        }
        public bool Create<T>(IList<T> list, ITransaction transaction = null)
        {
            foreach (T obj in list)
            {
                if (!Create(obj, transaction))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">实体</param>
        public bool Update(object model, ITransaction transaction = null)
        {
            bool existTran = true;
            try
            {
                if (transaction == null)
                {
                    existTran = false;
                    transaction = Session.BeginTransaction();
                }
                Session.Update(model);
                if (!existTran)
                {
                    transaction.Commit();
                }
                return true;
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    if (!existTran)
                    {
                        transaction.Rollback();
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="transaction"></param>
        public bool Update<T>(IList<T> list, ITransaction transaction = null)
        {
            foreach (T obj in list)
            {
                if (!Update(obj, transaction))
                {
                    return false;
                }
            }
            return true;
        }

        public bool Update(string sql, ITransaction transaction = null)
        {
            bool existTran = true;
            try
            {
                if (transaction == null)
                {
                    existTran = false;
                    transaction = Session.BeginTransaction();
                }
                ICreateSQLQuery(sql).ExecuteUpdate();
                //IDbCommand com = Session.Connection..CreateCommand();
                //com.CommandText = sql;
                //com.Transaction = transaction as IDbTransaction;
                //com.ExecuteNonQuery();
                if (!existTran)
                {
                    transaction.Commit();
                }
                return true;
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    if (!existTran)
                    {
                        transaction.Rollback();
                    }
                }
                return false;
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model">实体</param>
        public bool Delete(object model, ITransaction transaction = null)
        {
            bool existTran = true;
            try
            {
                if (transaction == null)
                {
                    existTran = false;
                    transaction = Session.BeginTransaction();
                }
                Session.Delete(model);
                if (!existTran)
                {
                    transaction.Commit();
                }
                return true;
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    if (!existTran)
                    {
                        transaction.Rollback();
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// HQL语句删除
        /// </summary>
        /// <param name="query">HQL语句</param>
        /// <returns></returns>
        public bool Delete(string query, ITransaction transaction = null)
        {
            bool existTran = true;
            try
            {
                if (transaction == null)
                {
                    existTran = false;
                    transaction = Session.BeginTransaction();
                }
                Session.Delete(query);
                if (!existTran)
                {
                    transaction.Commit();
                }
                return true;
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    if (!existTran)
                    {
                        transaction.Rollback();
                    }
                }
                return false;
            }
        }
        /// <summary>
        /// SQL语句删除
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public bool DeleteBySQL(string sql, ITransaction transaction = null)
        {
            bool existTran = true;
            try
            {
                if (transaction == null)
                {
                    existTran = false;
                    transaction = Session.BeginTransaction();
                }
                ICreateSQLQuery(sql).ExecuteUpdate();
                //IDbCommand com = Session.Connection..CreateCommand();
                //com.CommandText = sql;
                //com.Transaction = transaction as IDbTransaction;
                //com.ExecuteNonQuery();
                if (!existTran)
                {
                    transaction.Commit();
                }
                return true;
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    if (!existTran)
                    {
                        transaction.Rollback();
                    }
                }
                return false;
            }
        }

        #endregion

        #region (查询操作)

        #region lambda表达式查询
        ///// <summary>
        ///// 自定义条件检索对象信息
        ///// </summary>
        ///// <typeparam name="T">实体</typeparam>
        ///// <param name="where">lambda表达式条件</param>
        ///// <returns>单个对象</returns>
        //public T GetCustom<T>(Expression<Func<T, bool>> where) where T : class
        //{
        //    var query = Session.QueryOver<T>();
        //    var item = query.Where(where).List<T>().FirstOrDefault();
        //    return item;
        //}
        ///// <summary>
        ///// 自定义条件检索对象信息
        ///// </summary>
        ///// <typeparam name="T">实体</typeparam>
        ///// <param name="where">Linq表达式条件</param>
        ///// <returns>集合</returns>
        //public IList<T> GetListCustom<T>(Expression<Func<T, bool>> where) where T : class
        //{
        //    return Session.QueryOver<T>().Where(where).List<T>();
        //}
        ///// <summary>
        ///// 获取表中所有信息
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public IList<T> GetListCustom<T>() where T : class
        //{
        //    return Session.QueryOver<T>().List<T>();
        //}
        ///// <summary>
        ///// 自定义条件检索对象信息
        ///// </summary>
        ///// <typeparam name="T">实体</typeparam>
        ///// <param name="property">实体属性列名</param>
        ///// <param name="whereList">多个值</param>
        ///// <returns></returns>
        //public IList<T> GetListCustom<T>(string property, int[] whereList) where T : class
        //{
        //    IQueryOver<T, T> iQueryOver = Session.QueryOver<T>();
        //    T model = null;
        //    Expression<Func<object>> whereProperty = () => model;
        //    var propertyExpression = System.Linq.Expressions.Expression
        //        .Property(System.Linq.Expressions.Expression.Parameter(typeof(T)), property);
        //    var body = System.Linq.Expressions.Expression.Convert(propertyExpression, typeof(object));
        //    iQueryOver = iQueryOver.WhereRestrictionOn(System.Linq.Expressions.Expression.Lambda<Func<object>>(body))
        //        .IsIn(whereList);
        //    return iQueryOver.List<T>();
        //}
        ///// <summary>
        ///// 自定义条件检索对象信息
        ///// </summary>
        ///// <typeparam name="T">实体</typeparam>
        ///// <param name="where">lambda表达式条件</param>
        ///// <param name="utilityPager">分页信息类</param>
        ///// <param name="orderby">排序字段</param>
        ///// <param name="AscOrDesc">排序方式</param>
        ///// <returns></returns>
        //public IList<T> GetListCustom<T>(Expression<Func<T, bool>> where,
        //    UtilityPager utilityPager,
        //    Expression<Func<T, object>> orderby = null,
        //    string AscOrDesc = null) where T : class
        //{

        //    if (utilityPager == null)
        //    {
        //        utilityPager = new UtilityPager();
        //    }
        //    var query = Session.QueryOver<T>().Where(where);
        //    if (orderby != null && !string.IsNullOrEmpty(AscOrDesc))
        //    {
        //        if (AscOrDesc.Equals("Asc"))
        //            query = query.OrderBy(orderby).Asc;
        //        else if (AscOrDesc.Equals("Desc"))
        //            query = query.OrderBy(orderby).Desc;
        //    }
        //    if (!utilityPager.IsGetCount)
        //    {
        //        return query.Skip(GetPageIndex(utilityPager)).Take(utilityPager.PageSize).List<T>();
        //    }
        //    return query.Skip(GetPageIndex(utilityPager, query.RowCount())).Take(utilityPager.PageSize).List<T>();
        //}
        ///// <summary>
        ///// 获取表中所有信息
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="utilityPager">分页信息</param>
        ///// <param name="orderby">Lamdba表达式排序</param>
        ///// <param name="AscOrDesc">排序类型</param>
        ///// <returns></returns>
        //public IList<T> GetListCustom<T>(UtilityPager utilityPager,
        //    Expression<Func<T, object>> orderby = null,
        //    string AscOrDesc = null) where T : class
        //{

        //    try
        //    {
        //        if (utilityPager == null)
        //        {
        //            utilityPager = new UtilityPager();
        //        }
        //        var query = Session.QueryOver<T>();
        //        if (orderby != null && !string.IsNullOrEmpty(AscOrDesc))
        //        {
        //            if (AscOrDesc.Equals("Asc"))
        //                query = query.OrderBy(orderby).Asc;
        //            else if (AscOrDesc.Equals("Desc"))
        //                query = query.OrderBy(orderby).Desc;
        //        }
        //        if (!utilityPager.IsGetCount)
        //        {
        //            return query.Skip(GetPageIndex(utilityPager)).Take(utilityPager.PageSize).List<T>();
        //        }
        //        return query.Skip(GetPageIndex(utilityPager, query.RowCount())).Take(utilityPager.PageSize).List<T>();
        //    }
        //    catch (Exception)
        //    {
        //        return new List<T>();
        //    }
        //}
        #endregion

        ///// <summary>
        ///// 自定义条件检索对象信息
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="criteria">Criteria(sql中where条件对象)</param>
        ///// <param name="utilityPager"></param>
        ///// <param name="orderby"></param>
        ///// <param name="AscOrDesc"></param>
        ///// <returns></returns>
        //public IList<T> GetListCustom<T>(ICriteria criteria, UtilityPager utilityPager,
        //    string orderby = null,
        //    string AscOrDesc = null)
        //{
        //    var query = criteria;
        //    int count = utilityPager.Count;
        //    if (utilityPager.IsGetCount)
        //    {
        //        var query2 = query.Clone() as ICriteria;
        //        //query2.SetLockMode(LockMode.None);
        //        count = Convert.ToInt32(query2.SetProjection(Projections.RowCount()).UniqueResult());
        //    }
        //    if (orderby != null && !string.IsNullOrEmpty(AscOrDesc))
        //    {
        //        if (AscOrDesc.Equals("Asc"))
        //            query = query.AddOrder(Order.Asc(orderby));
        //        else if (AscOrDesc.Equals("Desc"))
        //            query = query.AddOrder(Order.Desc(orderby));
        //    }
        //    return query.SetFirstResult(GetPageIndex(utilityPager, count))
        //   .SetMaxResults(utilityPager.PageSize)
        //   .List<T>();
        //}
        /// <summary>
        /// 根据sql查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="type"></param>
        /// <param name="parameters"></param>
        /// <param name="isDTO">是否为自定义实体</param>
        /// <returns></returns>
        public ISQLQuery GetCustomSQLQuery<T>(string sql, Type type, List<NHParameter> parameters, bool isDTO = false) where T : class
        {
            ISQLQuery iquery = ICreateSQLQuery(sql);
            if (parameters != null && parameters.Count > 0)
            {
                foreach (NHParameter para in parameters)
                {
                    iquery.SetParameter(para.ParameterName, para.ParameterValue, para.DbType);
                }
            }
            if (!isDTO)
            {
                iquery.AddEntity(type);
            }
            else
            {
                iquery.SetResultTransformer(Transformers.AliasToBean(type));
            }
            return iquery;
        }
        public IQuery GetCustomSQLQuerytest<T>(string sql, Type type, List<NHParameter> parameters) where T : class
        {
            //sql = "select * from Privi_Department where DepartmentName=:Dname and Telephone=:Dname and EMail=:em";
            ISQLQuery iquery = ICreateSQLQuery(sql);
            if (parameters != null && parameters.Count > 0)
            {
                foreach (NHParameter para in parameters)
                {
                    iquery.SetParameter(para.ParameterName, para.ParameterValue, para.DbType);
                }
            }
            //iquery.SetParameter("Dname", "D组", NHibernateUtil.String);
            //iquery.SetParameter("em", "em", NHibernateUtil.String);
            return iquery.SetResultTransformer(Transformers.AliasToBean(type));
        }
        /// <summary>
        /// 自定义SQL语句检索查询
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">实体</param>
        /// <param name="isDTO">是否为自定义实体</param>
        /// <returns>实体</returns>
        public T GetCustomSQLQueryEntity<T>(string sql, List<NHParameter> parameters, bool isDTO = false) where T : class
        {
            return GetCustomSQLQuery<T>(sql, typeof(T), parameters, isDTO).List<T>().FirstOrDefault();
        }
        /// <summary>
        /// 获取单列值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public T GetCustomSQLQueryUniqueResult<T>(string sql, List<NHParameter> parameters) where T : class
        {
            ISQLQuery iquery = ICreateSQLQuery(sql);
            if (parameters != null && parameters.Count > 0)
            {
                foreach (NHParameter para in parameters)
                {
                    iquery.SetParameter(para.ParameterName, para.ParameterValue, para.DbType);
                }
            }
            return iquery.UniqueResult<T>();
        }
        /// <summary>
        /// 自定义SQL语句检索查询
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">实体</param>
        /// <param name="isDTO">是否为自定义实体</param>
        /// <returns>实体</returns>
        public IList<T> GetCustomSQLQueryList<T>(string sql, List<NHParameter> parameters, bool isDTO = false) where T : class
        {
            return GetCustomSQLQuery<T>(sql, typeof(T), parameters, isDTO).List<T>();
        }
        /// <summary>
        /// 自定义SQL语句检索查询
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">实体</param>
        /// <returns>实体</returns>
        public IList GetCustomSQLQueryObjectList(string sql, List<NHParameter> parameters)
        {
            ISQLQuery iquery = ICreateSQLQuery(sql);
            if (parameters != null && parameters.Count > 0)
            {
                foreach (NHParameter para in parameters)
                {
                    iquery.SetParameter(para.ParameterName, para.ParameterValue, para.DbType);
                }
            }
            return iquery.List();
        }
        /// <summary>
        /// 自定义SQL语句检索查询
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">实体</param>
        /// <returns>实体</returns>
        public IList GetCustomSQLQueryObjectList(UtilityPager utilityPager, string sql, List<NHParameter> parameters)
        {
            if (utilityPager == null)
            {
                utilityPager = new UtilityPager();
            }
            var queryList = ICreateSQLQuery(sql);
            if (parameters != null && parameters.Count > 0)
            {
                foreach (NHParameter para in parameters)
                {
                    queryList.SetParameter(para.ParameterName, para.ParameterValue, para.DbType);
                }
            }
            if (!utilityPager.IsGetCount)
            {

                return queryList.SetFirstResult(GetPageIndex(utilityPager))
                    .SetMaxResults(utilityPager.PageSize)
                    .List();
            }
            int count = Convert.ToInt32(GetCustomSQLQueryUniqueResult<object>("select count(*) from (" + sql + ") as countTable", parameters));
            return queryList.SetFirstResult(GetPageIndex(utilityPager, count))
                .SetMaxResults(utilityPager.PageSize)
                .List();
        }

        /// <summary>
        /// SQL分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="utilityPager">分页类</param>
        /// <param name="querysql">SQL语句</param>
        /// <returns></returns>
        public IList<T> PagerList<T>(UtilityPager utilityPager, string querysql, List<NHParameter> parameters, string orderBy = "", string ascOrDesc = "", bool isDTO = false) where T : class
        {
            if (utilityPager == null)
            {
                utilityPager = new UtilityPager();
            }
            string _querysql = querysql;
            if (!string.IsNullOrEmpty(orderBy) && !string.IsNullOrEmpty(ascOrDesc))
            {
                _querysql = querysql + " order by " + orderBy + " " + ascOrDesc;
            }
            var queryList = ICreateSQLQuery(_querysql);
            if (parameters != null && parameters.Count > 0)
            {
                foreach (NHParameter para in parameters)
                {
                    queryList.SetParameter(para.ParameterName, para.ParameterValue, para.DbType);
                }
            }
            if (!isDTO)
            {
                queryList.AddEntity(typeof(T));
            }
            else
            {
                queryList.SetResultTransformer(Transformers.AliasToBean(typeof(T)));
            }
            if (!utilityPager.IsGetCount)
            {
                return queryList.SetFirstResult(GetPageIndex(utilityPager))
                    .SetMaxResults(utilityPager.PageSize)
                    .List<T>();
            }
            int count = Convert.ToInt32(GetCustomSQLQueryUniqueResult<object>("select count(*) from (" + querysql + ") as countTable", parameters));
            return queryList.SetFirstResult(GetPageIndex(utilityPager, count))
                .SetMaxResults(utilityPager.PageSize)
                .List<T>();
        }

        #endregion

        IQuery ICreateQuery(string hql)
        {
            return Session.CreateQuery(hql);
        }

        ISQLQuery ICreateSQLQuery(string hql)
        {
            return Session.CreateSQLQuery(hql);
        }

        //public ICriteria CreateCriteria(Type persistentClass)
        //{            
        //    return Session.CreateCriteria(persistentClass);
        //}



        /// <summary>
        /// 处理页数
        /// </summary>
        /// <param name="utilityPager">分页类</param>
        /// <param name="listCount">总集合数</param>
        /// <returns></returns>
        int GetPageIndex(UtilityPager utilityPager, int listCount)
        {
            if (utilityPager == null)
                utilityPager = new UtilityPager();
            int count = utilityPager.Count == 0 ? listCount : utilityPager.Count;
            utilityPager.PageIndex = utilityPager.PageIndex == 0 ? 1 : utilityPager.PageIndex;
            utilityPager.Count = count;
            return (utilityPager.PageIndex - 1) * utilityPager.PageSize;
        }
        /// <summary>
        /// 处理页数
        /// </summary>
        /// <param name="utilityPager">分页类</param>
        /// <returns></returns>
        int GetPageIndex(UtilityPager utilityPager)
        {
            if (utilityPager == null)
                utilityPager = new UtilityPager();
            utilityPager.PageIndex = utilityPager.PageIndex == 0 ? 1 : utilityPager.PageIndex;
            return (utilityPager.PageIndex - 1) * utilityPager.PageSize;
        }

        public ITransaction BeginTransaction()
        {
            return this.Session.BeginTransaction();
        }
        /// <summary>
        /// 关闭连接备下次使用
        /// </summary>
        public void Close()
        {
            nhibernateHelper.CloseSession();
        }
    }

    /// <summary>
    /// 分页类
    /// </summary>
    public class UtilityPager
    {
        public UtilityPager(int pageSize = 10, int pageIndex = 1, bool isGetCount = true)
        {
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.IsGetCount = isGetCount;
        }
        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 总个数
        /// </summary>
        public int Count { get; set; }
        private bool isGetCount = true;
        /// <summary>
        /// 是否获取总个数
        /// </summary>
        public bool IsGetCount
        {
            get { return isGetCount; }
            set { isGetCount = value; }
        }
    }
}
