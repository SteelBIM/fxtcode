using FxtDataAcquisition.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.Domain;

namespace FxtDataAcquisition.Application.Services
{
    public class SysRoleMenuFunctionService : ISysRoleMenuFunctionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SysRoleMenuFunctionService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public IQueryable<SYS_Role_Menu_Function> GetAllBy(string userName, int companyId, int cityId, string url)
        {
            var function = _unitOfWork.SysRoleMenuFunctionRepository.Get(m => (m.CityID == cityId || m.CityID == 0) && (m.FxtCompanyID == companyId || m.FxtCompanyID == 0) && m.Valid == 1);
            var roleMenu = _unitOfWork.SysRoleMenuRepository.Get(m => (m.CityID == cityId || m.CityID == 0) && (m.FxtCompanyID == companyId || m.FxtCompanyID == 0));
            var roleUser = _unitOfWork.SysRoleUserRepository.Get(m => (m.CityID == cityId || m.CityID == 0) && (m.FxtCompanyID == companyId || m.FxtCompanyID == 0) && m.UserName == userName);
            var menu = _unitOfWork.SysMenuRepository.Get(m => m.URL == url);
            var list = from f in function
                       join rm in roleMenu on f.RoleMenuID equals rm.Id
                       join ru in roleUser on rm.RoleID equals ru.RoleID
                       join m in menu on rm.MenuID equals m.Id
                       select f;
            return list;
        }
    }
}
