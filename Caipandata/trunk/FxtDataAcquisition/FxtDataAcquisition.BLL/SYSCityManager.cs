using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager;
using FxtDataAcquisition.NHibernate.Entities;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;

namespace FxtDataAcquisition.BLL
{
    public static class SYSCityManager
    {
        public static List<FxtApi_SYSCity> GetAllCity()
        {
            List<FxtApi_SYSCity> list = SYSCityApi.GetAllCity();
            return list;
        }
        public static FxtApi_SYSCity GetCityByCityName(string cityName)
        {
            FxtApi_SYSCity obj = SYSCityApi.GetCityByCityName(cityName);
            return obj;
        }
    }
}
