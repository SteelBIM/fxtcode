namespace CBSS.Framework.Contract
{
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��IPagedList���� XML ע��
    public interface IPagedList
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��IPagedList���� XML ע��
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��IPagedList.CurrentPageIndex���� XML ע��
        int CurrentPageIndex { get; set; }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��IPagedList.CurrentPageIndex���� XML ע��
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��IPagedList.PageSize���� XML ע��
        int PageSize { get; set; }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��IPagedList.PageSize���� XML ע��
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��IPagedList.TotalItemCount���� XML ע��
        int TotalItemCount { get; set; }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��IPagedList.TotalItemCount���� XML ע��
    }
}
