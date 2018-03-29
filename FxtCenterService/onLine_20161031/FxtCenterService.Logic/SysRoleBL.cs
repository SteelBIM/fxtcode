using FxtCenterService.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FxtCenterService.Logic
{
    public class SysRoleBL
    {
        public static List<int> GetSysRoleUserIds(string userName, int fxtCompanyId, int sysTypeCode)
        {
            return SysRoleDA.GetSysRoleUserIds(userName, fxtCompanyId, sysTypeCode).Select(m => m.CityID).ToList();
        }
    }
}
