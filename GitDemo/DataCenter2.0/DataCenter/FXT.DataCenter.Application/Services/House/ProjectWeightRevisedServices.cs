using System.Linq;
using FXT.DataCenter.Application.Interfaces.House;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Application.Services.House
{
    public class ProjectWeightRevisedServices : IProjectWeightRevisedServices
    {
        private readonly IProjectWeightRevised _projectWeightRevised;
        private readonly ILog _log;
        private readonly IProjectCase _projectCase;
        private readonly ICompany _company;
        private readonly IImportTask _importTask;

        public ProjectWeightRevisedServices(IProjectWeightRevised projectWeightRevised, ILog log, IProjectCase projectCase, ICompany company,IImportTask importTask)
        {
            _projectWeightRevised = projectWeightRevised;
            _log = log;
            _projectCase = projectCase;
            this._importTask = importTask;
            this._company = company;
        
        }

        public IQueryable<DatWeightProject> GetWeightProjects(DatWeightProject datWeightProject, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            return _projectWeightRevised.GetWeightProjects(datWeightProject,pageIndex,pageSize,out totalCount,self);
        }

        public DatWeightProject GetWeightProject(int id, int projectId, int cityId, int fxtCompanyId)
        {
            return _projectWeightRevised.GetWeightProject(id,projectId, cityId, fxtCompanyId);
        }


        public long GetWeightProjectByProjectId(int projectId, int cityId, int fxtCompanyId)
        {
            return _projectWeightRevised.GetWeightProjectByProjectId(projectId, cityId, fxtCompanyId);
        }

        public IQueryable<DatWeightProject> GetNotUpdatedAvrPriceProjects(DatWeightProject datWeightProject, bool self = true)
        {
            return _projectWeightRevised.GetNotUpdatedAvrPriceProjects(datWeightProject, self);
        }

        public int UpdateWeightProject(DatWeightProject datWeightProject)
        {
            return _projectWeightRevised.UpdateWeightProject(datWeightProject);
        }

        
        public int UpdateWeightProjectAvgPrice(DatWeightProject datWeightProject)
        {
            return _projectWeightRevised.UpdateWeightProjectAvgPrice(datWeightProject);
        }


        public int AddWeightProject(DatWeightProject datWeightProject)
        {
            return _projectWeightRevised.AddWeightProject(datWeightProject);
        }


        public IQueryable<DAT_Project> GetProjectNameList(int fxtCompanyId, int cityId)
        {
            return _projectCase.GetProjectList(fxtCompanyId, cityId);
        }

        public IQueryable<DAT_Company> GetCompanyNameList(int cityId)
        {
            return _company.GetCompanyNameList(cityId);
        }

        public int AddCompany(DAT_Company datCompany)
        {
            return _company.AddCompany(datCompany);
        }

        public IQueryable<DAT_ImportTask> GetTask(int importType, int cityid, int fxtcompanyid)
        {
            return _importTask.GetTask(importType,cityid,fxtcompanyid);
        }

        public int DeleteTask(long taskId)
        {
            return _importTask.DeleteTask(taskId);
        }

        public int InsertLog(int cityId, int fxtCompanyId, string userId, string userName, int logType, int eventType, string objectId, string objectName, string remarks, string webIp)
        {
            return _log.InsertLog(cityId, fxtCompanyId, userId, userName, logType, eventType, objectId, objectName, remarks, webIp);
        }

    }
}
