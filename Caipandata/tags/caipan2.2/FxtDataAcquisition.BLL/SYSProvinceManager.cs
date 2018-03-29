using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager;
using FxtDataAcquisition.NHibernate.Entities;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;

namespace FxtDataAcquisition.BLL
{
    public static class SYSProvinceManager
    {
        public static List<FxtApi_SYSProvince> GetAllProvince()
        {
            List<FxtApi_SYSProvince> list = SYSProvinceApi.GetAllProvince();
            return list;
        }
    }
}
