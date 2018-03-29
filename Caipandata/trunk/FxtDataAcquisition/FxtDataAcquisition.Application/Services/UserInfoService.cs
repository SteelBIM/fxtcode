using FxtDataAcquisition.Application.Interfaces;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Domain.DTO;
using FxtDataAcquisition.Domain.DTO.FxtUserCenterDTO;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.FxtAPI.FxtUserCenter.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxtDataAcquisition.Application.Services
{
    public class UserInfoService : IUserInfoService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserInfoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<UserInfoDto> GetUserInfo(string keyWord, int? roleId, int? departmentId, int pageIndex, int pageSize, out int count, int cityId, int companyId, string username, string signname, List<Apps> loginAppList)
        {
            List<string> userNames = new List<string>();

            if (roleId.HasValue && roleId != 0 && departmentId.HasValue && departmentId != 0)
            {
                userNames = _unitOfWork.SysRoleUserRepository.Get(m => m.RoleID == roleId && (m.CityID == cityId || m.CityID == 0)).Select(m => m.UserName).Distinct().ToList();

                var us = _unitOfWork.DepartmentUserRepository.Get(m => m.DepartmentID == departmentId && userNames.Contains(m.Department.DepartmentName)).Select(m => m.UserName).Distinct().ToList();

                userNames = userNames.Concat(us).ToList();

                return GetDto(userNames, keyWord, pageIndex, pageSize, out count, cityId, companyId, username, signname, loginAppList, false);
            }
            else if (roleId.HasValue && roleId != 0)
            {
                userNames = _unitOfWork.SysRoleUserRepository.Get(m => m.RoleID == roleId && (m.CityID == cityId || m.CityID == 0)).Select(m => m.UserName).Distinct().ToList();

                return GetDto(userNames, keyWord, pageIndex, pageSize, out count, cityId, companyId, username, signname, loginAppList, false);
            }
            else if (departmentId.HasValue && departmentId != 0)
            {
                userNames = _unitOfWork.DepartmentUserRepository.Get(m => m.DepartmentID == departmentId && (m.CityID == cityId || m.CityID == 0)).Select(m => m.UserName).Distinct().ToList();

                return GetDto(userNames, keyWord, pageIndex, pageSize, out count, cityId, companyId, username, signname, loginAppList, false);
            }
            else
            {
                return GetDto(userNames, keyWord, pageIndex, pageSize, out count, cityId, companyId, username, signname, loginAppList, true);
            }
        }

        private List<UserInfoDto> GetDto(List<string> userNames, string keyWord, int pageIndex, int pageSize, out int count, int cityId, int companyId, string username, string signname, List<Apps> loginAppList, bool isAll)
        {
            if (!isAll)
            {
                if (userNames.Count < 1)
                {
                    count = 0;

                    return new List<UserInfoDto>();
                }
            }

            var userDtos = UserCenterUserInfoApi.GetUserListByCompanyId(companyId, userNames.ToArray(), keyWord, null, pageIndex, pageSize, out count, username, signname, loginAppList)

                .Select((o) =>
                {
                    var departmentUsers = _unitOfWork.DepartmentUserRepository.Get(m => m.UserName == o.UserName && (m.FxtCompanyID == companyId || m.FxtCompanyID == 0) && (m.CityID == cityId || m.CityID == 0))

                        .FirstOrDefault() ?? new DepartmentUser() { Department = new Department() };

                    var roleUsers = _unitOfWork.SysRoleUserRepository.Get(m => m.UserName == o.UserName && (m.FxtCompanyID == companyId || m.FxtCompanyID == 0) && (m.CityID == cityId || m.CityID == 0)).Select(m => m.Role.RoleName).ToList();

                    string roleNames = string.Empty;

                    if (roleUsers != null && roleUsers.Count > 0)
                    {

                        roleNames = string.Join(",", roleUsers);
                    }

                    return new UserInfoDto
                    {
                        DepartmentName = departmentUsers.Department.DepartmentName,
                        RoleNames = roleNames,
                        TrueName = o.TrueName,
                        UserName = o.UserName,
                        UserValid = o.UserValid,
                        Mobile = o.Mobile
                    };
                });

            return userDtos.ToList();
        }

    }
}
