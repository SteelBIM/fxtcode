using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Application.Interfaces
{
    public interface IUserInfoService
    {
        List<UserInfoDto> GetUserInfo(string keyWord, int? roleId, int? departmentId, int pageIndex, int pageSize, out int count, int cityId, int companyId, string username, string signname, List<Apps> loginAppList);
    }
}
