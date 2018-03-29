using System.Linq;
using FXT.DataCenter.Application.Interfaces.VQPrice;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Application.Services.VQPrice
{
    public class HouseWeightRevisedServices : IHouseWeightRevisedServices
    {
        private readonly IHouseWeightRevised _houseWeightRevised;
        private readonly ILog _log;
        private readonly IDAT_House _house;

        public HouseWeightRevisedServices(IHouseWeightRevised houseWeightRevised, ILog log, IDAT_House house)
        {
            _houseWeightRevised = houseWeightRevised;
            _log = log;
            _house = house;
        }

        public IQueryable<DatWeightHouse> GetWeightHouses(int ProjectId, int BuildingId, string HouseName, int CityId, int ParentShowDataCompanyId, int ParentProductTypeCode, int type, int pageIndex, int pageSize, out int totalCount, bool self = true)
        {
            return _houseWeightRevised.GetWeightHouses(ProjectId, BuildingId, HouseName, CityId, ParentShowDataCompanyId, ParentProductTypeCode, type, pageIndex, pageSize, out totalCount, self);
        }

        public DatWeightHouse GetWeightHouse(int projectId, int buildingId, int houseId, int cityId, int ParentShowDataCompanyId, int ParentProductTypeCode)
        {
            return _houseWeightRevised.GetWeightHouse(projectId, buildingId, houseId, cityId, ParentShowDataCompanyId, ParentProductTypeCode);
        }

        public int UpdateWeightHouse(DatWeightHouse datWeightHouse)
        {
            return _houseWeightRevised.UpdateWeightHouse(datWeightHouse);
        }

        public int AddWeightHouse(DatWeightHouse datWeightHouse)
        {
            return _houseWeightRevised.AddWeightHouse(datWeightHouse);
        }

        public int InsertLog(int cityId, int fxtCompanyId, string userId, string userName, int logType, int eventType, string objectId, string objectName, string remarks, string webIp)
        {
            return _log.InsertLog(cityId, fxtCompanyId, userId, userName, logType, eventType, objectId, objectName, remarks, webIp);
        }
    }
}
