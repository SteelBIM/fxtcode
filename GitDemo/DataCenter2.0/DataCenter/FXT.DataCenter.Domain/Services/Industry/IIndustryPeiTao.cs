using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using System.Data;

namespace FXT.DataCenter.Domain.Services
{
    public interface IIndustryPeiTao
    {
        IQueryable<DatPeiTaoIndustry> GetIndustryPeiTaos(DatPeiTaoIndustry peitaoIndustry, int pageIndex, int pageSize, out int totalCount, bool self);

        DatPeiTaoIndustry GetPeiTaoById(long peiTaoId, int fxtCompanyId);

        bool IsExistIndustryPeiTao(long PeiTaoID, string PeiTaoName, long ProjectId, int cityId, int fxtCompanyId);

        int UpdateIndustryPeiTao(DatPeiTaoIndustry peitaoIndustry, int currentCompanyId);

        int AddIndustryPeiTao(DatPeiTaoIndustry peitaoIndustry);

        int DeleteIndustryPeiTao(DatPeiTaoIndustry peitaoIndustry, int currentCompanyId);

        long GetPeiTaoIdByName(string peitaoName, long projectId, int cityId, int companyId);
    }
}
