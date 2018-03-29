using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IProjectAvgPrice
    {
        IQueryable<Dat_ProjectAvg> GetProjectAvgPrices(string ProjectName, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime useMonth, int IsPrices, int pageIndex, int pageSize, out int totalCount);
        Dat_ProjectAvg GetProjectAvgPriceByProjectid(int projectId, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime useMonth);
        int UpdateProjectAvg(Dat_ProjectAvg datProjectAvg);
        int AddProjectAvg(Dat_ProjectAvg datProjectAvg);
        IQueryable<Dat_ProjectAvg> ExportProjectAvgPrice(int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, string ProjectName, DateTime useMonth, int IsPrices);

        Dat_ProjectAvg GetProjectHistoryAvgPriceByProjectid(int projectId, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime useMonth);
        int UpdateProjectHistoryAvg(Dat_ProjectAvg datProjectAvg);
        int AddProjectHistoryAvg(Dat_ProjectAvg datProjectAvg);
        IQueryable<Dat_ProjectAvg> ExportProjectHistoryAvgPrice(int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, int projectId, DateTime? useMonth);

        IQueryable<Dat_ProjectAvg> GetProjectHistoryAvgPrices(int projectId, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime? useMonth, int pageIndex, int pageSize, out int totalCount);
    }
}
