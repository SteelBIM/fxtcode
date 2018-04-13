using System;
using System.Linq;
using System.Collections.Generic;

namespace CBSS.Core.Utility
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Collection”的 XML 注释
    public static class Collection
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Collection”的 XML 注释
    {
        /// <summary>
        /// 数组或list随机选出几个
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="collection">数组或list</param>
        /// <param name="count">选出数量</param>
        /// <returns></returns>
        public static IEnumerable<T> Random<T>(this IEnumerable<T> collection, int count)
        {
            var rd = new Random();
            return collection.OrderBy(c => rd.Next()).Take(count);
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Collection.Random<T>(IEnumerable<T>)”的 XML 注释
        public static T Random<T>(this IEnumerable<T> collection)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Collection.Random<T>(IEnumerable<T>)”的 XML 注释
        {
            return collection.Random<T>(1).SingleOrDefault();
        }
    }
}
