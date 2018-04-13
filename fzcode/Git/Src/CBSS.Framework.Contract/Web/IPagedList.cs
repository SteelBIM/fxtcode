namespace CBSS.Framework.Contract
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“IPagedList”的 XML 注释
    public interface IPagedList
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“IPagedList”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“IPagedList.CurrentPageIndex”的 XML 注释
        int CurrentPageIndex { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“IPagedList.CurrentPageIndex”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“IPagedList.PageSize”的 XML 注释
        int PageSize { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“IPagedList.PageSize”的 XML 注释
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“IPagedList.TotalItemCount”的 XML 注释
        int TotalItemCount { get; set; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“IPagedList.TotalItemCount”的 XML 注释
    }
}
