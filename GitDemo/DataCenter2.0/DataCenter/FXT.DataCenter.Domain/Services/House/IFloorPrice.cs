using FXT.DataCenter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace FXT.DataCenter.Domain.Services
{
    public interface IFloorPrice
    {
        int AddFloorPrice(Sys_FloorPrice model);
        IQueryable<Sys_FloorPrice> FindAll(int cityId, int fxtCompanyId);
        DataTable ExportFloorCode(int cityId, int fxtCompanyId);
        int UpdateFloorPriceInImport(Sys_FloorPrice model);
        int DeleteFloorPrice(int cityId, int fxtcompanyid);
    }
}
