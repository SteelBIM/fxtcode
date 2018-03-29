using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IOfficeTenant
    {
        /// <summary>
        /// 查询办公楼盘
        /// </summary>
        IQueryable<DatTenantOffice> GetTenants(DatTenantOffice officeTenant, int pageIndex, int pageSize, out int totalCount, bool self = true);

        /// <summary>
        /// 根据houseTenantId获取整个信息
        /// </summary>
        DatTenantOffice GetTenantNameById(int houseTenantId, int fxtCompanyId);

        /// <summary>
        /// 更新
        /// </summary>
        int UpdateTenantOffice(DatTenantOffice officeTenant, int currentCompanyId);

        /// <summary>
        /// 新增
        /// </summary>
        int AddTenantOffice(DatTenantOffice officeTenant);

        /// <summary>
        /// 删除
        /// </summary>
        int DeleteTenantOffice(DatTenantOffice officeTenant, int currentCompanyId);
    }
}
