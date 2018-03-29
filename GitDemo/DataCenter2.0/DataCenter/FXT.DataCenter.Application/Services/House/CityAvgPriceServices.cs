using System;
using System.Collections.Generic;
using System.Linq;
using FXT.DataCenter.Application.Interfaces.House;
using FXT.DataCenter.Domain.Models;
using FXT.DataCenter.Domain.Services;

namespace FXT.DataCenter.Application.Services.House
{
    public class CityAvgPriceServices : ICityAvgPriceServices
    {
        private readonly ICityAvgPrice _cityAvgPrice;
        private readonly IDropDownList _dropDownList;
        private readonly ILog _log;

        public CityAvgPriceServices(ICityAvgPrice cityAvgPrice, IDropDownList dropDownList, ILog log)
        {
            _cityAvgPrice = cityAvgPrice;
            _dropDownList = dropDownList;
            _log = log;
        }

        public IQueryable<DAT_AvgPrice_Month> GetCityAvgPrices(DateTime caseDateFrom, DateTime caseDateTo, int areaId, int cityId, int pageSize, int pageIndex, out int totalCount)
        {
            return _cityAvgPrice.GetCityAvgPrices(caseDateFrom, caseDateTo, areaId, cityId, pageSize, pageIndex, out totalCount);
        }

        public DAT_AvgPrice_Month GetCityAvgPrice(int id)
        {
            return _cityAvgPrice.GetCityAvgPrice(id);
        }

        public DAT_AvgPrice_Month IsExistCityAvgPrice(int cityId, int areaId, DateTime AvgPriceDate)
        {
            return _cityAvgPrice.IsExistCityAvgPrice(cityId, areaId, AvgPriceDate);
        }

        public int AddCityAvgPrice(DAT_AvgPrice_Month avgPrice)
        {
            return _cityAvgPrice.AddCityAvgPrice(avgPrice);
        }

        public int UpdateAvgPrice(DAT_AvgPrice_Month avgPrice)
        {
            return _cityAvgPrice.UpdateAvgPrice(avgPrice);
        }

        public int DeleteAvgPrice(int id)
        {
            return _cityAvgPrice.DeleteAvgPrice(id);
        }

        public DAT_AvgPrice_Month GetAvgPrice(DAT_AvgPrice_Month avgPrice)
        {
            return _cityAvgPrice.GetAvgPrice(avgPrice);
        }

        public IList<CompanyProduct_Module> GetAreaName(int cityId)
        {
            return _dropDownList.GetAreaName(cityId);
        }


        public int InsertLog(int cityId, int fxtCompanyId, string userId, string userName, int logType, int eventType, string objectId, string objectName, string remarks, string webIp)
        {
            return _log.InsertLog(cityId, fxtCompanyId, userId, userName, logType, eventType, objectId, objectName, remarks, webIp);
        }
    }
}
