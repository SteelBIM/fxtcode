using System;
using System.Collections.Generic;
using System.Linq;

namespace CBSS.Framework.Contract
{
    /// <summary>
    /// ��ҳ���ݼ��ϣ����ں�˷��ط�ҳ�õļ��ϼ�ǰ����ͼ��ҳ�ؼ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>, IPagedList
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.PagedList(IList<T>, int, int)���� XML ע��
        public PagedList(IList<T> items, int pageIndex, int pageSize)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.PagedList(IList<T>, int, int)���� XML ע��
        {
            PageSize = pageSize;
            TotalItemCount = items.Count;
            CurrentPageIndex = pageIndex;
            for (int i = StartRecordIndex - 1; i < EndRecordIndex; i++)
            {
                Add(items[i]);
            }
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.PagedList(IEnumerable<T>, int, int, int)���� XML ע��
        public PagedList(IEnumerable<T> items, int pageIndex, int pageSize, int totalItemCount)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.PagedList(IEnumerable<T>, int, int, int)���� XML ע��
        {
            AddRange(items);
            if (pageIndex == 0)
                pageIndex = 1;
            TotalItemCount = totalItemCount;
            CurrentPageIndex = pageIndex;
            PageSize = pageSize;
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.ExtraCount���� XML ע��
        public int ExtraCount { get; set; }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.ExtraCount���� XML ע��
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.CurrentPageIndex���� XML ע��
        public int CurrentPageIndex { get; set; }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.CurrentPageIndex���� XML ע��
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.PageSize���� XML ע��
        public int PageSize { get; set; }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.PageSize���� XML ע��
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.TotalItemCount���� XML ע��
        public int TotalItemCount { get; set; }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.TotalItemCount���� XML ע��
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.TotalPageCount���� XML ע��
        public int TotalPageCount { get { return (int)Math.Ceiling(TotalItemCount / (double)PageSize); } }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.TotalPageCount���� XML ע��
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.StartRecordIndex���� XML ע��
        public int StartRecordIndex { get { return (CurrentPageIndex - 1) * PageSize + 1; } }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.StartRecordIndex���� XML ע��
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.EndRecordIndex���� XML ע��
        public int EndRecordIndex { get { return TotalItemCount > CurrentPageIndex * PageSize ? CurrentPageIndex * PageSize : TotalItemCount; } }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PagedList<T>.EndRecordIndex���� XML ע��
    }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PageLinqExtensions���� XML ע��
    public static class PageLinqExtensions
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PageLinqExtensions���� XML ע��
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PageLinqExtensions.ToPagedList<T>(IQueryable<T>, int, int)���� XML ע��
        public static PagedList<T> ToPagedList<T>
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��PageLinqExtensions.ToPagedList<T>(IQueryable<T>, int, int)���� XML ע��
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
