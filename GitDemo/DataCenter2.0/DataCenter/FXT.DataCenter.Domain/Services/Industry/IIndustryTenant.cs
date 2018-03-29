using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using System.Data;

namespace FXT.DataCenter.Domain.Services
{
    public interface IIndustryTenant
    {
        IQueryable<DatTenantIndustry> GetTenants(DatTenantIndustry tenant, int pageIndex, int pageSize, out int totalCount, bool self = true);
        DatTenantIndustry GetTenantNameById(int houseTenantId, int fxtCompanyId);
        int UpdateTenantIndustry(DatTenantIndustry tenant, int currentCompanyId);
        int AddTenantIndustry(DatTenantIndustry tenant);
        int DeleteTenantIndustry(DatTenantIndustry tenant, int currentCompanyId);
    }
}
