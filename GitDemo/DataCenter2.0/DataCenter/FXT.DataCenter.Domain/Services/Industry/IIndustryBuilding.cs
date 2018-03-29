using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using System.Data;

namespace FXT.DataCenter.Domain.Services
{
    public interface IIndustryBuilding
    {
        IQueryable<DatBuildingIndustry> GetIndustryBuildings(DatBuildingIndustry datBuildingIndustry, bool self, int pageIndex, int pageSize, out int totalCount);

        IQueryable<DatBuildingIndustry> GetIndustryBuildings(int cityId, int fxtCompanyId, int areaId, int subAreaId, int projectId = -1, int buildingId = -1, bool self = true);

        IQueryable<DatBuildingIndustry> GetIndustryBuildings(long projectId, int fxtCompanyId);

        DatBuildingIndustry GetIndustryBuilding(int buildingId, int fxtCompanyId);

        long GetIndustryBuildingId(long projectId, long buildingId, string buildingName, int cityId, int companyId);

        int AddIndustryBuilding(DatBuildingIndustry datBuildingIndustry);

        int UpdateIndustryBuilding(DatBuildingIndustry datBuildingIndustry, int currentCompanyId);

        int DeleteIndustryBuilding(DatBuildingIndustry datBuildingIndustry, int currentCompanyId);

        /// <summary>
        /// 复制楼栋
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="companyId"></param>
        /// <param name="buildingName">原楼栋名称</param>
        /// <param name="destBuildingName">目标楼栋名称</param>
        /// <param name="buildingId"></param>
        /// <param name="projectId"></param>
        /// <returns>-1：目标楼栋下已存在房号；-2：楼栋复制失败；大于0：新增的房号数</returns>
        int CopyBuilding(int cityId, int companyId, string buildingName, string destBuildingName, int buildingId, int projectId);

        DataTable BuildingSelfDefineExport(DatBuildingIndustry datBuildingIndustry, List<string> buildingAttr, int CityId, int FxtCompanyId, bool self = true);

        IQueryable<LNKBPhoto> GetIndustryBuildingPhotoes(LNKBPhoto lnkPPhoto, bool self = true);

        LNKBPhoto GetIndustryBuildingPhoto(int id, int fxtCompanyId);

        int AddIndustryBuildingPhoto(LNKBPhoto lnkPPhoto);

        int UpdateIndustryBuildingPhoto(LNKBPhoto lnkPPhoto, int currentCompanyId);

        int DeleteIndustryBuildingPhoto(LNKBPhoto lnkPPhoto, int currentCompanyId);

        int GetHouseCounts(int buildingId, int cityId, int fxtCompanyId, bool self);
    }
}
