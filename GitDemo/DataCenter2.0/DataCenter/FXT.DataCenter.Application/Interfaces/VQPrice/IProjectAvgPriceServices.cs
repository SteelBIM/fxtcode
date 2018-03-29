using System.Linq;
using FXT.DataCenter.Domain.Models;
using System;

namespace FXT.DataCenter.Application.Interfaces.VQPrice
{
    public interface IProjectAvgPriceServices
    {
        IQueryable<Dat_ProjectAvg> GetProjectAvgPrices(string ProjectName, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime useMonth, int IsPrices, int pageIndex, int pageSize, out int totalCount);
        Dat_ProjectAvg GetProjectAvgPriceByProjectid(int projectId, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime useMonth);
        int UpdateProjectAvg(Dat_ProjectAvg datProjectAvg);
        int AddProjectAvg(Dat_ProjectAvg datProjectAvg);
        IQueryable<Dat_ProjectAvg> ExportProjectAvgPrice(int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, string ProjectName, DateTime useMonth, int IsPrices);
        int InsertLog(int cityId, int fxtCompanyId, string userId, string userName, int logType, int eventType, string objectId, string objectName, string remarks, string webIp);

        IQueryable<Dat_ProjectAvg> GetProjectHistoryAvgPrices(int projectId, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime? useMonth, int pageIndex, int pageSize, out int totalCount);
        Dat_ProjectAvg GetProjectHistoryAvgPriceByProjectid(int projectId, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime useMonth);
        int UpdateProjectHistoryAvg(Dat_ProjectAvg datProjectAvg);
        int AddProjectHistoryAvg(Dat_ProjectAvg datProjectAvg);
        IQueryable<Dat_ProjectAvg> ExportProjectHistoryAvgPrice(int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, int projectId, DateTime? useMonth);

        IQueryable<DAT_ImportTask> GetTask(int importType, int cityid, int fxtcompanyid);
        int DeleteTask(long taskId);
    }
}
