using System;
using System.Collections.Generic;
using System.Linq;

namespace CBSS.Framework.Contract
{
    /// <summary>
    /// 分页数据集合，用于后端返回分页好的集合及前端视图分页控件绑定
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>, IPagedList
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PagedList<T>.PagedList(IList<T>, int, int)”的 XML 注释
        public PagedList(IList<T> items, int pageIndex, int pageSize)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PagedList<T>.PagedList(IList<T>, int, int)”的 XML 注释
        {
            PageSize = pageSize;
            TotalItemCount = items.Count;
            CurrentPageIndex = pageIndex;
            for (int i = StartRecordIndex - 1; i < EndRecordIndex; i++)
            {
                Add(items[i]);
            }
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PagedList<T>.PagedList(IEnumerable<T>, int, int, int)”的 XML 注释
        public PagedList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PagedList<T>.PagedList(IEnumerable<T>, int, int, int)”的 XML 注释
        {
            AddRange(items);
            if (pageIndex == 0)
                pageIndex = 1;
            TotalItemCount = totalItemCount;
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PagedList<T>.ExtraCount”的 XML 注释
        public int ExtraCount { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PagedList<T>.ExtraCount”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PagedList<T>.CurrentPageIndex”的 XML 注释
        public int CurrentPageIndex { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PagedList<T>.CurrentPageIndex”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PagedList<T>.PageSize”的 XML 注释
        public int PageSize { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PagedList<T>.PageSize”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PagedList<T>.TotalItemCount”的 XML 注释
        public int TotalItemCount { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PagedList<T>.TotalItemCount”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PagedList<T>.TotalPageCount”的 XML 注释
        public int TotalPageCount { get { return (int)Math.Ceiling(TotalItemCount / (double)PageSize); } }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PagedList<T>.TotalPageCount”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PagedList<T>.StartRecordIndex”的 XML 注释
        public int StartRecordIndex { get { return (CurrentPageIndex - 1) * PageSize + 1; } }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PagedList<T>.StartRecordIndex”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PagedList<T>.EndRecordIndex”的 XML 注释
        public int EndRecordIndex { get { return TotalItemCount > CurrentPageIndex * PageSize ? CurrentPageIndex * PageSize : TotalItemCount; } }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PagedList<T>.EndRecordIndex”的 XML 注释
    }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PageLinqExtensions”的 XML 注释
    public static class PageLinqExtensions
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PageLinqExtensions”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“PageLinqExtensions.ToPagedList<T>(IQueryable<T>, int, int)”的 XML 注释
        public static PagedList<T> ToPagedList<T>
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“PageLinqExtensions.ToPagedList<T>(IQueryable<T>, int, int)”的 XML 注释
            (
                this IQueryable<T> allItems,
                int pageIndex,
                int pageSize
            )
        {
            if (pageIndex < 1)
                pageIndex = 1;
            var itemIndex = (pageIndex - 1) * pageSize;
            var pageOfItems = allItems.Skip(itemIndex).Take(pageSize).ToList();
            var totalItemCount = allItems.Count();
            return new PagedList<T>(pageOfItems, pageIndex, pageSize, totalItemCount);
        }
    }
}
