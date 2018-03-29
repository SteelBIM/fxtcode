using System.Linq;
using FXT.DataCenter.Application.Interfaces.VQPrice;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;
using System;

namespace FXT.DataCenter.Application.Services.VQPrice
{
    public class ProjectAvgPriceServices : IProjectAvgPriceServices
    {
        private readonly IProjectAvgPrice _projectAvgPrice;
        private readonly ILog _log;
        private readonly IImportTask _importTask;

        public ProjectAvgPriceServices(IProjectAvgPrice projectAvgPrice, ILog log, IImportTask importTask)
        {
            this._projectAvgPrice = projectAvgPrice;
            this._log = log;
            this._importTask = importTask;
        }
        public IQueryable<Dat_ProjectAvg> GetProjectAvgPrices(string ProjectName, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime useMonth, int IsPrices, int pageIndex, int pageSize, out int totalCount)
        {
            return _projectAvgPrice.GetProjectAvgPrices(ProjectName, CityId, ParentShowDataCompanyId, ParentProductTypeCode, useMonth, IsPrices, pageIndex, pageSize, out totalCount);
        }
        public Dat_ProjectAvg GetProjectAvgPriceByProjectid(int projectId, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime useMonth)
        {
            return _projectAvgPrice.GetProjectAvgPriceByProjectid(projectId, CityId, ParentShowDataCompanyId, ParentProductTypeCode, useMonth);
        }
        public int UpdateProjectAvg(Dat_ProjectAvg datProjectAvg)
        {
            return _projectAvgPrice.UpdateProjectAvg(datProjectAvg);
        }
        public int AddProjectAvg(Dat_ProjectAvg datProjectAvg)
        {
            return _projectAvgPrice.AddProjectAvg(datProjectAvg);
        }
        public IQueryable<Dat_ProjectAvg> ExportProjectAvgPrice(int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, string ProjectName, DateTime useMonth, int IsPrices)
        {
            return _projectAvgPrice.ExportProjectAvgPrice(CityId, ParentShowDataCompanyId, ParentProductTypeCode, ProjectName, useMonth, IsPrices);
        }
        public int InsertLog(int cityId, int fxtCompanyId, string userId, string userName, int logType, int eventType, string objectId, string objectName, string remarks, string webIp)
        {
            return _log.InsertLog(cityId, fxtCompanyId, userId, userName, logType, eventType, objectId, objectName, remarks, webIp);
        }

        public Dat_ProjectAvg GetProjectHistoryAvgPriceByProjectid(int projectId, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime useMonth)
        {
            return _projectAvgPrice.GetProjectHistoryAvgPriceByProjectid(projectId, CityId, ParentShowDataCompanyId, ParentProductTypeCode, useMonth);
        }
        public int UpdateProjectHistoryAvg(Dat_ProjectAvg datProjectAvg)
        {
            return _projectAvgPrice.UpdateProjectHistoryAvg(datProjectAvg);
        }
        public int AddProjectHistoryAvg(Dat_ProjectAvg datProjectAvg)
        {
            return _projectAvgPrice.AddProjectHistoryAvg(datProjectAvg);
        }
        public IQueryable<Dat_ProjectAvg> ExportProjectHistoryAvgPrice(int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, int projectId, DateTime? useMonth)
        {
            return _projectAvgPrice.ExportProjectHistoryAvgPrice(CityId, ParentShowDataCompanyId, ParentProductTypeCode, projectId, useMonth);
        }

        public IQueryable<Dat_ProjectAvg> GetProjectHistoryAvgPrices(int projectId, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, DateTime? useMonth, int pageIndex, int pageSize, out int totalCount)
        {
            return _projectAvgPrice.GetProjectHistoryAvgPrices(projectId, CityId, ParentShowDataCompanyId, ParentProductTypeCode, useMonth, pageIndex, pageSize, out totalCount);
        }
        public IQueryable<DAT_ImportTask> GetTask(int importType, int cityid, int fxtcompanyid)
        {
            return _importTask.GetTask(importType, cityid, fxtcompanyid);
        }
        public int DeleteTask(long taskId)
        {
            return _importTask.DeleteTask(taskId);
        }
    }
}
