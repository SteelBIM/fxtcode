using FxtDataAcquisition.Domain.Models;
using System.Linq;

namespace FxtDataAcquisition.Application.Interfaces
{
    public interface ISysRoleMenuFunctionService
    {
        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="companyId"></param>
        /// <param name="cityId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        IQueryable<SYS_Role_Menu_Function> GetAllBy(string userName,int companyId,int cityId,string url);
    }
}
