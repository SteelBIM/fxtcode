using FxtDataAcquisition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(DbContext context)
        {
            this._context = context;
            this._dbSet = _context.Set<T>();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderby = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.AsNoTracking().Where(filter);
            }

            return orderby != null ? query.OrderBy(orderby) : query;
        }

        public T GetBy(Expression<Func<T, bool>> filter = null)
        {
            T query = null;

            if (filter != null)
            {
                query = _dbSet.AsNoTracking().FirstOrDefault(filter);
                //query = _dbSet.AsNoTracking().FirstOrDefault(filter);//不在上下文中缓存
            }

            return query;
        }

        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public object GetByType(Type type, object id)
        {
            return _context.Set(type).Find(id);
        }


        public T Insert(T entity)
        {
            return _dbSet.Add(entity);
        }

        public void Update(T entityToUpdate, IList<string> modifiedProperties = null)
        {
            //_dbSet.Attach(entityToUpdate);

            //if (modifiedProperties != null)
            //{
            //    var stateEntry = ((IObjectContextAdapter)_context).ObjectContext.
            //        ObjectStateManager.GetObjectStateEntry(entityToUpdate);

            //    foreach (var item in modifiedProperties)
            //    {
            //        stateEntry.SetModifiedProperty(item);
            //    }
            //}
            //else
            //{
            //    _context.Entry(entityToUpdate).State = EntityState.Modified;
            //}
            //RemoveHoldingEntityInContext(entityToUpdate);
            if (_context.Entry<T>(entityToUpdate).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToUpdate);

                if (modifiedProperties != null)
                {
                    var stateEntry = ((IObjectContextAdapter)_context).ObjectContext.
                        ObjectStateManager.GetObjectStateEntry(entityToUpdate);

                    foreach (var item in modifiedProperties)
                    {
                        stateEntry.SetModifiedProperty(item);
                    }
                }
                else
                {
                    _context.Entry(entityToUpdate).State = EntityState.Modified;
                }
            }
        }

        public void Delete(object id)
        {
            var entityToDelete = _dbSet.Find(id);
            _dbSet.Attach(entityToDelete);
            _context.Entry(entityToDelete).State = EntityState.Deleted;
        }

        public void Delete(Expression<Func<T, bool>> filter = null)
        {
            var entityToDelete = _dbSet.Where(filter);
            //var entityToDelete = _dbSet.FirstOrDefault(filter);
            if (entityToDelete != null)
            {
                foreach (var item in entityToDelete)
                {
                    _dbSet.Attach(item);
                    _context.Entry(item).State = EntityState.Deleted;
                }
            }
        }

        public void Include(string include)
        {
            _dbSet.Include(include);
        }

        public void Include<TProperty>(Expression<Func<T, TProperty>> path)
        {
            _dbSet.Include(path);
        }

        public IQueryable<TElement> SqlQuery<TElement>(string sql)
        {
            return _context.Database.SqlQuery<TElement>(sql).AsQueryable();
        }


        /// <summary>
        /// 用于监测Context中的Entity是否存在，如果存在，将其Detach，防止出现问题。
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Boolean RemoveHoldingEntityInContext(T entity)
        {
            var objContext = ((IObjectContextAdapter)_context).ObjectContext;
            var objSet = objContext.CreateObjectSet<T>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            Object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);

            if (exists)
            {
                objContext.Detach(foundEntity);
            }

            return (exists);
        }

        /// <summary>
        /// 清空DB上下文中所有缓存的实体对象
        /// </summary>
        private void DetachedAllEntities()
        {
            var objectContext = ((IObjectContextAdapter)_context).ObjectContext;
            List<ObjectStateEntry> entries = new List<ObjectStateEntry>();
            var states = new[] { EntityState.Added, EntityState.Deleted, EntityState.Modified, EntityState.Unchanged };
            foreach (var state in states)
            {
                entries.AddRange(objectContext.ObjectStateManager.GetObjectStateEntries(state));
            }

            foreach (var item in entries)
            {
                objectContext.Detach(item.Entity);
            }
        }
    }
}
