using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using System.Data;

namespace FXT.DataCenter.Domain.Services
{
    public interface IIndustryProject
    {
        IQueryable<DatProjectIndustry> GetProjectIndustrys(DatProjectIndustry projectIndustry, int pageIndex, int pageSize, out int totalCount, bool self = true);

        IQueryable<DatProjectIndustry> GetProjectIndustrys(int cityId, int fxtCompanyId, int areaId = -1, int subAreaId = -1, int projectId = -1, bool self = true);

        DatProjectIndustry GetProjectNameById(long id, int fxtCompanyId);

        bool IsExistProjectIndustry(int areaId, long projectId, string projectName, int cityId, int fxtCompanyId);

        int UpdateProjectIndustry(DatProjectIndustry projectIndustry, int currentCompanyId);

        int AddProjectIndustry(DatProjectIndustry projectIndustry);

        int DeleteProjectIndustry(DatProjectIndustry projectIndustry, int currentCompanyId);

        IQueryable<LNKPPhoto> GetIndustryProjectPhotoes(LNKPPhoto lnkPPhoto, bool self = true);

        LNKPPhoto GetIndustryProjectPhoto(int id, int fxtCompanyId);

        int AddIndustryProjectPhoto(LNKPPhoto lnkPPhoto);

        int UpdateIndustryProjectPhoto(LNKPPhoto lnkPPhoto, int currentCompanyId);

        int DeleteIndustryProjectPhoto(LNKPPhoto lnkPPhoto, int currentCompanyId);

        IQueryable<long> GetProjectIdByName(string projectName, int areaId, int cityId, int companyId);

        int GetBuildingCounts(int projectId, int cityId, int fxtCompanyId, bool self);

        int GetProjectCounts(int subAreaId, int cityId, int fxtCompanyId, bool self);

        DataTable ProjectSelfDefineExport(DatProjectIndustry projectIndustry, List<string> projectAttr, int CityId, int FxtCompanyId, bool self = true);
    }
}
