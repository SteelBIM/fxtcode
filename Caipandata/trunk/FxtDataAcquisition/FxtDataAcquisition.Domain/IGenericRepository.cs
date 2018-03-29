using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FxtDataAcquisition.Domain
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <param name="orderby">排序</param>
        /// <returns></returns>
        IQueryable<TEntity> Get(
                        Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>> orderby = null);
        TEntity GetBy(Expression<Func<TEntity, bool>> filter = null);
        TEntity GetById(object id);
        object GetByType(Type type,object id);
        TEntity Insert(TEntity entity);
        void Update(TEntity entityToUpdate, IList<string> modifiedProperties = null);
        void Delete(object id);
        void Delete(Expression<Func<TEntity, bool>> filter = null);
        void Include(string include);
        void Include<TProperty>(Expression<Func<TEntity, TProperty>> path);
        /// <summary>
        /// 执行SQL查询
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        IQueryable<TElement> SqlQuery<TElement>(string sql);
    }
}
