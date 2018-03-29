using FxtDataAcquisition.Domain.DTO.FxtDataWcfDTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Application.Interfaces
{
    public interface ICityService
    {
        List<FxtApi_SYSProvince> GetProvinceCityListBy(string userName,string signName, List<Apps> appList);
    }
}
