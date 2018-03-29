using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.FxtAPI.FxtDataCenter.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.BLL
{
    public static class SYSSubAreaManager
    {
        public static List<FxtApi_SYSSubArea> GetSubAreaByAreaId(int areaId, string username, string signname, List<UserCenter_Apps> appList)
        {
            List<FxtApi_SYSSubArea> list = DataCenterAreaApi.GetSubAreaByAreaId(areaId, username, signname, appList);
            return list;
        }
    }
}
