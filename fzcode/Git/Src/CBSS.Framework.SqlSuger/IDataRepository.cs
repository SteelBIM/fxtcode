using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CBSS.Framework.Contract;

namespace CBSS.Framework.DAL
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“IDataRepository”的 XML 注释
    public interface IDataRepository
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“IDataRepository”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“IDataRepository.Update<T>(T)”的 XML 注释
        T Update<T>(T entity) where T : ModelBase;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“IDataRepository.Update<T>(T)”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“IDataRepository.Insert<T>(T)”的 XML 注释
        T Insert<T>(T entity) where T : ModelBase;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“IDataRepository.Insert<T>(T)”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“IDataRepository.Delete<T>(T)”的 XML 注释
        void Delete<T>(T entity) where T : ModelBase;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“IDataRepository.Delete<T>(T)”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“IDataRepository.Find<T>(params object[])”的 XML 注释
        T Find<T>(params object[] keyValues) where T : ModelBase;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“IDataRepository.Find<T>(params object[])”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“IDataRepository.FindAll<T>(Expression<Func<T, bool>>)”的 XML 注释
        List<T> FindAll<T>(Expression<Func<T, bool>> conditions = null) where T : ModelBase;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“IDataRepository.FindAll<T>(Expression<Func<T, bool>>)”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“IDataRepository.FindAllByPage<T, S>(Expression<Func<T, bool>>, Expression<Func<T, S>>, int, int)”的 XML 注释
        PagedList<T> FindAllByPage<T, S>(Expression<Func<T, bool>> conditions, Expression<Func<T, S>> orderBy, int pageSize, int pageIndex) where T : ModelBase;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“IDataRepository.FindAllByPage<T, S>(Expression<Func<T, bool>>, Expression<Func<T, S>>, int, int)”的 XML 注释
    }
}
