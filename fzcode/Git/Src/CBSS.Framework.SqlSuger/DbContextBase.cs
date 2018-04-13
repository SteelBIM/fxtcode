using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;
using CBSS.Framework.Contract;
using System.Threading.Tasks;

namespace CBSS.Framework.DAL
{
    /// <summary>
    /// DAL基类，实现Repository通用泛型数据访问模式
    /// </summary>
    public class DbContextBase : DbContext, IDataRepository, IDisposable
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“DbContextBase.DbContextBase(string)”的 XML 注释
        public DbContextBase(string connectionString)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“DbContextBase.DbContextBase(string)”的 XML 注释
        {
            //var objectContext = (this as IObjectContextAdapter).ObjectContext;
            //objectContext.CommandTimeout = 500;

            this.Database.Connection.ConnectionString = connectionString;
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“DbContextBase.DbContextBase(string, IAuditable)”的 XML 注释
        public DbContextBase(string connectionString, IAuditable auditLogger)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“DbContextBase.DbContextBase(string, IAuditable)”的 XML 注释
            : this(connectionString)
        {
            this.AuditLogger = auditLogger;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“DbContextBase.AuditLogger”的 XML 注释
        public IAuditable AuditLogger { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“DbContextBase.AuditLogger”的 XML 注释

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“DbContextBase.Update<T>(T)”的 XML 注释
        public T Update<T>(T entity) where T : ModelBase
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“DbContextBase.Update<T>(T)”的 XML 注释
        {
            var set = this.Set<T>();
            set.Attach(entity);
            this.Entry<T>(entity).State = EntityState.Modified;
            this.SaveChanges();

            return entity;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“DbContextBase.Insert<T>(T)”的 XML 注释
        public T Insert<T>(T entity) where T : ModelBase
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“DbContextBase.Insert<T>(T)”的 XML 注释
        {
            this.Set<T>().Add(entity);
            this.SaveChanges();
            return entity;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“DbContextBase.Delete<T>(T)”的 XML 注释
        public void Delete<T>(T entity) where T : ModelBase
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“DbContextBase.Delete<T>(T)”的 XML 注释
        {
            this.Entry<T>(entity).State = EntityState.Deleted;
            this.SaveChanges();
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“DbContextBase.Find<T>(params object[])”的 XML 注释
        public T Find<T>(params object[] keyValues) where T : ModelBase
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“DbContextBase.Find<T>(params object[])”的 XML 注释
        {
            return this.Set<T>().Find(keyValues);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“DbContextBase.FindAll<T>(Expression<Func<T, bool>>)”的 XML 注释
        public List<T> FindAll<T>(Expression<Func<T, bool>> conditions = null) where T : ModelBase
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“DbContextBase.FindAll<T>(Expression<Func<T, bool>>)”的 XML 注释
        {
            if (conditions == null)
                return this.Set<T>().ToList();
            else
                return this.Set<T>().Where(conditions).ToList();
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“DbContextBase.FindAllByPage<T, S>(Expression<Func<T, bool>>, Expression<Func<T, S>>, int, int)”的 XML 注释
        public PagedList<T> FindAllByPage<T, S>(Expression<Func<T, bool>> conditions, Expression<Func<T, S>> orderBy, int pageSize, int pageIndex) where T : ModelBase
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“DbContextBase.FindAllByPage<T, S>(Expression<Func<T, bool>>, Expression<Func<T, S>>, int, int)”的 XML 注释
        {
            var queryList = conditions == null ? this.Set<T>() : this.Set<T>().Where(conditions) as IQueryable<T>;

            return queryList.OrderByDescending(orderBy).ToPagedList(pageIndex, pageSize);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“DbContextBase.SaveChanges()”的 XML 注释
        public override int SaveChanges()
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“DbContextBase.SaveChanges()”的 XML 注释
        {
            this.WriteAuditLog();

            var result = base.SaveChanges();
            return result;
        }

        internal void WriteAuditLog()
        {
            if (this.AuditLogger == null)
                return;

            foreach (var dbEntry in this.ChangeTracker.Entries<ModelBase>().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified))
            {
                var auditableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(AuditableAttribute), false).SingleOrDefault() as AuditableAttribute;
                if (auditableAttr == null)
                    continue;

                var operaterName = WCFContext.Current.Operater.Name;

                Task.Factory.StartNew(() =>
                {
                    var tableAttr = dbEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
                    string tableName = tableAttr != null ? tableAttr.Name : dbEntry.Entity.GetType().Name;
                    var moduleName = dbEntry.Entity.GetType().FullName.Split('.').Skip(1).FirstOrDefault();

                    this.AuditLogger.WriteLog(dbEntry.Entity.ID, operaterName, moduleName, tableName, dbEntry.State.ToString(), dbEntry.Entity);
                });
            }

        }
    }
}
