using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Linq;
using FxtNHibernate.DATProjectDomain.Entities;
using System.Linq.Expressions;
using NHibernate.Criterion;
using FxtService.Common;


/**
 * 作者: 李晓东
 * 时间: 2013.11.27
 * 摘要: 新建数据库CRUD操作类
 *       2013.12.18 新增声明 MSSQLDBDAL(string factoryName) 修改人:李晓东
 *       2013.12.19 新增方法ICreateSQLQuery(string hql) 修改人:李晓东
 *       2014.01.21 修改整个类,优化性能 修改人:李晓东
 * **/
namespace FxtNHibernater.Data
{
    /// <summary>
    /// Microsoft Sql Server 数据库 CRUD
    /// </summary>
    public class MSSQLDBDAL_bak : Exceptions
    {
        private NHibernateHelper nhibernate = null;
        protected ISession Session { get; set; }

        
        /// <summary>
        /// 初始化MSSQLDBDAL
        /// </summary>
        /// <param name="factoryName">factoryName(配置文件session-factory节点name的值)</param>
        public MSSQLDBDAL_bak(string factoryName = null)
        {
            //nhibernate = new NHibernateHelper(factoryName);
            //Session = nhibernate.GetSession();            
        }
        public object Create(object objSysCity)
        {
            ITransaction transaction = null;
            object _robj = null;
            try
            {
                transaction = Session.BeginTransaction();
                _robj = Session.Save(objSysCity);
                Session.Flush();
                Session.Clear();
                transaction.Commit();
            }
            catch (Exception hexe)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                _robj = 0;
            }
            return _robj;
        }
        public void Create<T>(IList<T> list)
        {
            foreach (T obj in list)
            {
                Create(obj);
            }
        }
        public object Create(object objModel, string entityName)
        {
            ITransaction transaction = null;
            object _robj = null;
            try
            {
                transaction = Session.BeginTransaction();
                _robj = Session.Save(entityName, objModel);
                Session.Flush();
                Session.Clear();
                transaction.Commit();
            }
            catch (Exception hexe)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                _robj = 0;
            }
            return _robj;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">实体</param>
        public bool Update(object model)
        {
            ITransaction transaction = null;
            try
            {
                transaction = Session.BeginTransaction();
                Session.Update(model);
                Session.Flush();
                Session.Clear();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return false;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model">实体</param>
        public bool Delete(object model)
        {
            ITransaction transaction = null;
            try
            {
                transaction = Session.BeginTransaction();
                Session.Delete(model);
                Session.Flush();
                Session.Clear();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return false;
            }
        }
        /// <summary>
        /// HQL语句删除
        /// </summary>
        /// <param name="query">HQL语句</param>
        /// <returns></returns>
        public bool Delete(string query)
        {
            ITransaction transaction = null;
            try
            {
                transaction = Session.BeginTransaction();
                Session.Delete(query);
                Session.Flush();
                Session.Clear();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
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
        public T GetCustom<T>(Expression<Func<T, bool>> wh) where T : class
        {
            var item = Session.Query<T>().Where(wh);
            return item.FirstOrDefault<T>();
        }
        /// <summary>
        /// 自定义条件检索对象信息
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="hsql">HSQL语句</param>
        /// <returns></returns>
        public T GetCustom<T>(string hsql) where T : class
        {
            var item = ICreateQuery(hsql).Enumerable<T>();
            return item.FirstOrDefault();
        }
        /// <summary>
        /// 自定义条件检索对象信息
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="hsql">HSQL语句</param>
        /// <returns></returns>
        public T GetSQLCustom<T>(string hsql) where T : class
        {
            var item = ICreateSQLQuery(hsql).AddEntity(typeof(T)).Enumerable<T>();
            return item.FirstOrDefault();
        }
        IQuery ICreateQuery(string hql)
        {
            return Session.CreateQuery(hql);
        }

        ISQLQuery ICreateSQLQuery(string hql)
        {
            return Session.CreateSQLQuery(hql);
        }

        /// <summary>
        /// 自定义条件检索对象信息
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">lambda表达式条件</param>
        /// <returns>集合</returns>
        public IQueryable<T> GetListCustom<T>(Expression<Func<T, bool>> w) where T : class
        {
            return Session.Query<T>().Where(w);
        }
        /// <summary>
        /// 自定义条件检索对象信息
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="where">lambda表达式条件</param>
        /// <returns>集合</returns>
        public IQueryable<T> GetListCustom<T>(Expression<Func<T, bool>> where, UtilityPager utilityPager) where T : class
        {

            var query = Session.Query<T>().Where(where);
            return query.Skip(GetPageIndex(utilityPager, query.Count<T>())).Take(utilityPager.PageSize);
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
        public IQueryable<T> GetListCustom<T>(string hsql) where T : class
        {
            return ICreateQuery(hsql).Future<T>().AsQueryable<T>();
        }

        /// <summary>
        /// 自定义SQL语句检索查询
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">实体</param>
        /// <returns>集合</returns>
        public IQueryable<T> GetCustomSQLQuery<T>(string sql, Type type) where T : class
        {
            return ICreateSQLQuery(sql).AddEntity(type).Enumerable<T>().AsQueryable<T>();
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
            return GetCustomSQLQuery<T>(sql, typeof(T)).FirstOrDefault<T>();
        }
        /// <summary>
        /// 自定义SQL语句检索查询
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="type">实体</param>
        /// <returns>实体</returns>
        public IQueryable<T> GetCustomSQLQueryList<T>(string sql) where T : class
        {
            return GetCustomSQLQuery<T>(sql, typeof(T)).AsQueryable<T>();
        }

        /// <summary>
        /// SQL分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="utilityPager">分页类</param>
        /// <param name="querysql">SQL语句</param>
        /// <returns></returns>
        public IQueryable<T> PagerList<T>(UtilityPager utilityPager, string querysql) where T : class
        {
            var queryList = ICreateSQLQuery(querysql).AddEntity(typeof(T));
            return queryList.SetFirstResult(GetPageIndex(utilityPager, queryList.List().Count))
                .SetMaxResults(utilityPager.PageSize).Enumerable<T>().AsQueryable<T>();
        }

        /// <summary>
        /// hsql分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="utilityPager">分页类</param>
        /// <param name="hsql">hsql语句</param>
        /// <returns></returns>
        public IQueryable<T> HQueryPagerList<T>(UtilityPager utilityPager, string hsql) where T : class
        {
            var queryList = ICreateQuery(hsql);

            return queryList.SetFirstResult(GetPageIndex(utilityPager, queryList.List().Count))
                .SetMaxResults(utilityPager.PageSize).Enumerable<T>().AsQueryable<T>();
        }
        /// <summary>
        /// hsql集合
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="hsql">hsql语句</param>
        /// <returns></returns>
        public IQueryable<T> HQueryPagerList<T>(string hsql) where T : class
        {
            var queryList = Session.CreateQuery(hsql);

            return queryList.Future<T>().AsQueryable<T>();
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
        /// 关闭连接备下次使用
        /// </summary>
        public void Close()
        {
            Session.Close();            
        }
    }
}
