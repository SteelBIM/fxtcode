using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Application.Interfaces.VQPrice;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Application.Services.VQPrice
{
    public class BuildingWeightRevisedServices : IBuildingWeightRevisedServices
    {
        private readonly IBuildingWeightRevised _buildingWeightRevised;
        private readonly ILog _log;
        private readonly IDAT_Building _building;
        private readonly IProjectWeightRevised _projectWeightRevised;

        public BuildingWeightRevisedServices(IBuildingWeightRevised buildingWeightRevised, ILog log, IDAT_Building building, IProjectWeightRevised projectWeightRevised)
        {
            _buildingWeightRevised = buildingWeightRevised;
            _log = log;
            _building = building;
            _projectWeightRevised = projectWeightRevised;
        }

        public IQueryable<DatWeightBuilding> GetWeightBuildings(int ProjectId, string BuildingName, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, int type, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            return _buildingWeightRevised.GetWeightBuildings(ProjectId, BuildingName, CityId, ParentShowDataCompanyId, ParentProductTypeCode, type, pageIndex, pageSize, out totalCount, self);
        }

        public int InsertLog(int cityId, int fxtCompanyId, string userId, string userName, int logType, int eventType, string objectId, string objectName, string remarks, string webIp)
        {
            return _log.InsertLog(cityId, fxtCompanyId, userId, userName, logType, eventType, objectId, objectName, remarks, webIp);
        }

    }
}
