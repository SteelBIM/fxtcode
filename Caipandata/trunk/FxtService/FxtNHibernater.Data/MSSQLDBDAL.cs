using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using FxtNHibernate.DATProjectDomain.Entities;
using System.Linq.Expressions;
using NHibernate.Criterion;
using FxtService.Common;
using NHibernate.Transform;
using System.Data;
using System.Collections;

/**
 * 作者: 李晓东
 * 时间: 2013.11.27
 * 摘要: 新建数据库CRUD操作类
 *       2013.12.18 新增声明 MSSQLDBDAL(string factoryName) 修改人:李晓东
 *       2013.12.19 新增方法ICreateSQLQuery(string hql) 修改人:李晓东
 *       2014.03.03 修改人:李晓东
 *                  修改:GetListCustom加上排序
 *       2014.03.10 修改人:李晓东
 *                  新增:GetListCustom重载
 * **/
namespace FxtNHibernater.Data
{
    /// <summary>
    /// Microsoft Sql Server 数据库 CRUDok
    /// </summary>
    public class MSSQLDBDAL : Exceptions
    {
        private NHibernateHelper nhibernateHelper = null;
        protected ISession Session { get; set; }
        public MSSQLDBDAL()
        {
            this.Session = NHibernateHelper.GetSession();
        }

        /// <summary>
        /// 初始化MSSQLDBDAL
        /// </summary>
        /// <param name="factoryName">factoryName(配置文件session-factory节点name的值)</param>
        public MSSQLDBDAL(string factoryName)
        {
            this.Session = NHibernateHelper.GetSession(factoryName);
        }
        public object Create(object objSysCity, ITransaction transaction = null)
        {
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
            }
            return _robj;
        }
        public void Create<T>(IList<T> list, ITransaction transaction = null)
        {
            foreach (T obj in list)
            {
                Create(obj, transaction);
            }
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
        /// 
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public T GetById<T>(int Id) where T : class
        {
            return Session.Get<T>(Id);
        }

        /// <summary>
        /// 自定义条件检索对象信息
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">lambda表达式条件</param>
        /// <returns>单个对象</returns>
        public T GetCustom<T>(Expression<Func<T, bool>> where) where T : class
        {
            var item = Session.QueryOver<T>().Where(where).List<T>().FirstOrDefault();
            return item;
        }
        /// <summary>
        /// 自定义条件检索对象信息
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="hsql">HSQL语句</param>
        /// <returns></returns>
        public T GetCustom<T>(string hsql) where T : class
        {
            var item = ICreateQuery(hsql).List<T>();
            return item.FirstOrDefault();
        }
        /// <summary>
        /// 自定义条件检索对象信息
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <returns></returns>
        public T GetSQLCustom<T>(string sql) where T : class
        {
            var item = ICreateSQLQuery(sql).AddEntity(typeof(T)).List<T>();
            return item.FirstOrDefault();
        }
        IQuery ICreateQuery(string hql)
        {
            return Session.CreateQuery(hql);
        }

        ISQLQuery ICreateSQLQuery(string sql)
        {
            return Session.CreateSQLQuery(sql);
        }
        /// <summary>
        /// 自定义条件检索对象信息
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">Linq表达式条件</param>
        /// <returns>集合</returns>
        public IList<T> GetListCustom<T>(Expression<Func<T, bool>> where) where T : class
        {
            return Session.QueryOver<T>().Where(where).List<T>();
        }

        /// <summary>
        /// 自定义条件检索对象信息
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="property">实体属性列名</param>
        /// <param name="whereList">多个值</param>
        /// <param name="where">Lamdba表达式</param>
        /// <returns></returns>
        public IList<T> GetListCustom<T>(string property, int[] whereList,
            Expression<Func<T, bool>> where = null) where T : class
        {
            IQueryOver<T, T> iQueryOver = Session.QueryOver<T>();
            T model = null;
            Expression<Func<object>> whereProperty = () => model;
            var propertyExpression = System.Linq.Expressions.Expression
                .Property(System.Linq.Expressions.Expression.Parameter(typeof(T)), property);
            var body = System.Linq.Expressions.Expression.Convert(propertyExpression, typeof(object));
            if (where != null)
                iQueryOver = iQueryOver.Where(where);
            iQueryOver = iQueryOver.WhereRestrictionOn(System.Linq.Expressions.Expression.Lambda<Func<object>>(body))
                .IsIn(whereList);
            //if (where != null)
            //{
            //   return iQueryOver.Where(where).List<T>();
            //}
            return iQueryOver.List<T>();
        }

        /// <summary>
        /// 获取表中所有信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IList<T> GetListCustom<T>() where T : class
        {
            return Session.QueryOver<T>().List<T>();
        }
        /// <summary>
        /// 自定义条件检索对象信息
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="utilityPager">分页信息</param>
        /// <returns>集合</returns>
        public IList<T> GetListCustom<T>(Expression<Func<T, bool>> where,
            UtilityPager utilityPager,
            Expression<Func<T, object>> orderby = null,
            string AscOrDesc = null) where T : class
        {

            if (utilityPager == null)
            {
                utilityPager = new UtilityPager();
            }
            var query = Session.QueryOver<T>().Where(where);
            if (orderby != null && !string.IsNullOrEmpty(AscOrDesc))
            {
                if (AscOrDesc.Equals("Asc"))
                    query = query.OrderBy(orderby).Asc;
                else if (AscOrDesc.Equals("Desc"))
                    query = query.OrderBy(orderby).Desc;
            }
            if (!utilityPager.IsGetCount)
            {
                return query.Skip(GetPageIndex(utilityPager)).Take(utilityPager.PageSize).List<T>();
            }
            return query.Skip(GetPageIndex(utilityPager, query.List().Count)).Take(utilityPager.PageSize).List<T>();
        }
        /// <summary>
        /// 获取表中所有信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="utilityPager">分页信息</param>
        /// <param name="orderby">Lamdba表达式排序</param>
        /// <param name="AscOrDesc">排序类型</param>
        /// <returns></returns>
        public IList<T> GetListCustom<T>(UtilityPager utilityPager,
            Expression<Func<T, object>> orderby = null,
            string AscOrDesc = null) where T : class
        {

            try
            {
                if (utilityPager == null)
                {
                    utilityPager = new UtilityPager();
                }
                var query = Session.QueryOver<T>();
                if (orderby != null && !string.IsNullOrEmpty(AscOrDesc))
                {
                    if (AscOrDesc.Equals("Asc"))
                        query = query.OrderBy(orderby).Asc;
                    else if (AscOrDesc.Equals("Desc"))
                        query = query.OrderBy(orderby).Desc;
                }
                if (!utilityPager.IsGetCount)
                {
                    return query.Skip(GetPageIndex(utilityPager)).Take(utilityPager.PageSize).List<T>();
                }
                return query.Skip(GetPageIndex(utilityPager, query.RowCount())).Take(utilityPager.PageSize).List<T>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ICriteria CreateCriteria(Type persistentClass)
        {
            return Session.CreateCriteria(persistentClass);
        }


        /// <summary>
        /// 自定义条件检索对象信息
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="hsql">HSQL语句</param>
        /// <returns></returns>
        public IList<T> GetListCustom<T>(string hsql) where T : class
        {
            return ICreateQuery(hsql).List<T>();
        }

        /// <summary>
        /// 自定义SQL语句检索查询
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">实体</param>
        /// <returns>集合</returns>
        public ISQLQuery GetCustomSQLQuery<T>(string sql, Type type) where T : class
        {
            return ICreateSQLQuery(sql).AddEntity(type);
        }

        /// <summary>
        /// 自定义SQL语句检索查询
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">实体</param>
        /// <returns>实体</returns>
        public T GetCustomSQLQueryEntity<T>(string sql) where T : class
        {
            return GetCustomSQLQuery<T>(sql, typeof(T)).List<T>().FirstOrDefault();
        }
        public T GetCustomSQLQueryUniqueResult<T>(string sql) where T : class
        {
            return ICreateSQLQuery(sql).UniqueResult<T>();
        }
        /// <summary>
        /// 自定义SQL语句检索查询
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">实体</param>
        /// <returns>实体</returns>
        public IList<T> GetCustomSQLQueryList<T>(string sql) where T : class
        {
            return GetCustomSQLQuery<T>(sql, typeof(T)).List<T>();
        }

        /// <summary>
        /// 自定义SQL语句检索查询
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">实体</param>
        /// <returns>实体</returns>
        public IList GetCustomSQLQueryObjectList(string sql)
        {
            return ICreateSQLQuery(sql).List();
        }
        /// <summary>
        /// SQL分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="utilityPager">分页类</param>
        /// <param name="querysql">SQL语句</param>
        /// <returns></returns>
        public IList<T> PagerList<T>(UtilityPager utilityPager, string querysql) where T : class
        {
            if (utilityPager == null)
            {
                utilityPager = new UtilityPager();
            }
            var queryList = ICreateSQLQuery(querysql).AddEntity(typeof(T));
            if (!utilityPager.IsGetCount)
            {
                return queryList.SetFirstResult(GetPageIndex(utilityPager))
                    .SetMaxResults(utilityPager.PageSize)
                    .List<T>();
            }
            int count = Convert.ToInt32(GetCustomSQLQueryUniqueResult<object>("select count(*) from (" + querysql + ") as countTable"));
            return queryList.SetFirstResult(GetPageIndex(utilityPager, count))
                .SetMaxResults(utilityPager.PageSize)
                .List<T>();
        }

        /// <summary>
        /// hsql分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="utilityPager">分页类</param>
        /// <param name="hsql">hsql语句</param>
        /// <returns></returns>
        public IList<T> HQueryPagerList<T>(UtilityPager utilityPager, string hsql) where T : class
        {
            if (utilityPager == null)
            {
                utilityPager = new UtilityPager();
            }
            var queryList = ICreateQuery(hsql);
            if (!utilityPager.IsGetCount)
            {
                return queryList.SetFirstResult(GetPageIndex(utilityPager))
                    .SetMaxResults(utilityPager.PageSize).List<T>();
            }

            return queryList.SetFirstResult(GetPageIndex(utilityPager, queryList.List().Count))
                .SetMaxResults(utilityPager.PageSize).List<T>();
        }
        /// <summary>
        /// hsql集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="hsql">hsql语句</param>
        /// <returns></returns>
        public IList<T> HQueryPagerList<T>(string hsql) where T : class
        {
            var queryList = Session.CreateQuery(hsql);

            return queryList.List<T>();
        }

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

        internal ITransaction BeginTransaction()
        {
            return this.Session.BeginTransaction();
        }
        /// <summary>
        /// 关闭连接备下次使用
        /// </summary>
        public void Close()
        {
            NHibernateHelper.CloseSession();
        }
    }
}
