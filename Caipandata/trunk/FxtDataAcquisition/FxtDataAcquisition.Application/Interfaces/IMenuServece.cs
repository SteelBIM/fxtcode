using FxtDataAcquisition.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Application.Interfaces
{
    public interface IMenuServece
    {
        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="fxtCompanyId">机构ID</param>
        /// <param name="cityId">角色cityId</param>
        /// <returns></returns>
        IQueryable<SYS_Menu> GetSysMenuBy(string userName,int fxtCompanyId,int cityId);
    }
}
