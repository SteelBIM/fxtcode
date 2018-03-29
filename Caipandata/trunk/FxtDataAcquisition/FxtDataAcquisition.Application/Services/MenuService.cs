using FxtDataAcquisition.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FxtDataAcquisition.Domain.Models;
using FxtDataAcquisition.Domain;
using FxtDataAcquisition.Common;

namespace FxtDataAcquisition.Application.Services
{
    public class MenuService : IMenuServece
    {
        private readonly IUnitOfWork _unitOfWork;
        public MenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IQueryable<SYS_Menu> GetSysMenuBy(string userName, int fxtCompanyId, int cityId)
        {
            var menu = _unitOfWork.SysMenuRepository.Get(m => m.TypeCode == SYSCodeManager.MENUTYPECODE_1);
            var menuRole = _unitOfWork.SysRoleMenuRepository.Get();
            var roleUser = _unitOfWork.SysRoleUserRepository.Get(m => m.CityID == cityId && m.FxtCompanyID == fxtCompanyId && m.UserName == userName);
            var menuList = from r in roleUser
                           join mr in menuRole on r.RoleID equals mr.RoleID
                           join m in menu on mr.MenuID equals m.Id
                           select m;
            return menuList;
        }
    }
}
