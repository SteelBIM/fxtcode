using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using System.Data;

namespace FXT.DataCenter.Domain.Services
{
    public interface IIndustryCase
    {
        IQueryable<DatCaseIndustry> GetIndustryCases(DatCaseIndustry datCase, int pageIndex, int pageSize, out int totalCount, bool self = true);

        DatCaseIndustry GetIndustryCase(int id);

        int AddIndustryCase(DatCaseIndustry datCase);

        int UpdateIndustryCase(DatCaseIndustry datCase);

        int DeleteIndustryCase(DatCaseIndustry datCase);

        int DeleteSameIndustryCase(int fxtCompanyId, int cityId, DateTime caseDateFrom, DateTime caseDateTo, string saveUser);
    }
}
