using Kingsun.IBS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.IBLL.IBS2MOD
{
    /// <summary>
    /// IBS用户信息变动
    /// </summary>
    public interface IIBS_MOD_UserInfoBLL
    {
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        bool Update(IBS_UserInfo UserInfo);

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        bool Add(IBS_UserInfo UserInfo);

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="UserID">用户ID</param>
        /// <returns></returns>
        bool Delete(IBS_UserInfo UserInfo);
    }
}
