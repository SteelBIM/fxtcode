using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FXT.DataCenter.Domain.Models;

namespace FXT.DataCenter.Domain.Services
{
    public interface IHumanHouse
    {
        IQueryable<DAT_HumanHouse> GetHumanHouses(int? areaId, string key, int cityId, int fxtcompanyId, bool self);

        DAT_HumanHouse GetHumanHouseById(long humanid, long houseid, int fxtcompanyid, int cityid);

        IQueryable<DAT_HumanHouse> GetProjectList(int fxtCompanyId, int cityId, long projectId);

        int AddHumanHouse(DAT_HumanHouse dh);
        int UpdateHumanHouse(DAT_HumanHouse dh);
        int DeleteHumanHouse(long humanid, long houseid, string saver, DateTime savetime);

        IQueryable<DAT_HumanHouse> ExportHumanHouses(int cityId, int fxtcompanyId, bool self);

        IQueryable<DAT_Building> GetBuildings(int cityId, int fxtCompanyId, int projectId, int buildingId = -1, bool self = true);
        IQueryable<DAT_House> GetHouses(int cityId, int fxtCompanyId, int buildingId, int houseId = -1, bool self = true);
    }
}
