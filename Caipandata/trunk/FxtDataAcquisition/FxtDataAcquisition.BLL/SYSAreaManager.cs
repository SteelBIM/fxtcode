using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtDataAcquisition.FxtAPI.FxtDataWcf.Manager;
using FxtDataAcquisition.NHibernate.Entities;
using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;

namespace FxtDataAcquisition.BLL
{
    public static class SYSAreaManager
    {
        public static List<FxtApi_SYSArea> GetAreaByCityId(int cityId, string username, string signname, List<UserCenter_Apps> appList)
        {
            List<FxtApi_SYSArea> list = DataCenterAreaApi.GetAreaByCityId(cityId, username, signname, appList);
            return list;

        }
    }
}
