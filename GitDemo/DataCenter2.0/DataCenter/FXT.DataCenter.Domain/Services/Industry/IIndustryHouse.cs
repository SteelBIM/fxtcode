using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;
using System.Data;

namespace FXT.DataCenter.Domain.Services
{
    public interface IIndustryHouse
    {
        DataTable GetIndustryHouses(int projectId, int buildingId, int cityId, int fxtCompanyId, bool self);
        IQueryable<DatHouseIndustry> GetIndustryHouseList(int buildingId, int cityId, int fxtCompanyId, bool self);
        IQueryable<DatHouseIndustry> GetIndustryHouses_UnitNo(int buildingId, int cityId, int fxtCompanyId, bool self);
        IQueryable<int> GetIndustryHouses_FloorNo(int buildingId, int cityId, int fxtCompanyId, bool self);
        DatHouseIndustry GetIndustryHouse(long houseId, int cityId, int fxtCompanyId);
        string GetProjectName(long buildingId, int fxtCompanyId);
        string GetBuildingName(long buildingId, int fxtCompanyId);
        int UpdateFloorNum(int floorNo, string floorNum, int cityId, int fxtCompanyId, string saveUser);
        int UpdateIndustryHouse(DatHouseIndustry datHouseIndustry, int currentCompanyId);
        int AddIndustryHouse(DatHouseIndustry datHouseIndustry);
        int DeleteIndustryHouse(DatHouseIndustry datHouseIndustry, int currentCompanyId);
        DataTable HouseSelfDefineExport(DatHouseIndustry datHouse, List<string> houseAttr, int CityId, int FxtCompanyId, bool self = true);
        long GetIndustryHouseId(long buildingId, string houseName, int cityId, int fxtCompanyId);
    }
}
