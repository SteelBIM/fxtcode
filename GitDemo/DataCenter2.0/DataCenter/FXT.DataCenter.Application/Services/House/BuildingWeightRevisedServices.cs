using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Application.Interfaces.House;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;


namespace FXT.DataCenter.Application.Services.House
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

        public IQueryable<DatWeightBuilding> GetWeightBuildings(DatWeightBuilding datWeightBuilding, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            return _buildingWeightRevised.GetWeightBuildings(datWeightBuilding,pageIndex,pageSize,out totalCount,self);
        }

        public DatWeightBuilding GetWeightBuilding(int projectId, int buildingId, int cityId, int fxtCompanyId)
        {
            return _buildingWeightRevised.GetWeightBuilding(projectId, buildingId, cityId, fxtCompanyId);
        }

        public long GetWeightBuildingByBuildingId(int projectId, int buildingId, int cityId, int fxtCompanyId)
        {
            return _buildingWeightRevised.GetWeightBuildingByBuildingId(projectId,buildingId,cityId,fxtCompanyId);
        }

        public int UpdateWeightBuilding(DatWeightBuilding datWeightBuilding)
        {
            return _buildingWeightRevised.UpdateWeightBuilding(datWeightBuilding);
        }

        public int AddWeightBuilding(DatWeightBuilding datWeightBuilding)
        {
            return _buildingWeightRevised.AddWeightBuilding(datWeightBuilding);
        }


        public IQueryable<DAT_Building> BuildingSelect(int projectId, int cityId, int fxtCompanyId)
        {
            return _building.GetBuildNameList(cityId, projectId, fxtCompanyId);
        }

        public string GetProjectName(int projectId, int cityId, int fxtCompanyId)
        {
            return _projectWeightRevised.GetProjectName(projectId,cityId,fxtCompanyId);
        }

        public int InsertLog(int cityId, int fxtCompanyId, string userId, string userName, int logType, int eventType, string objectId, string objectName, string remarks, string webIp)
        {
            return _log.InsertLog(cityId,fxtCompanyId,userId,userName,logType,eventType,objectId,objectName,remarks,webIp);
        }

    }
}
