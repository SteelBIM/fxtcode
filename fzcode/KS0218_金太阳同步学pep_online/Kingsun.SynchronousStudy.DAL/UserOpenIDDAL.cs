using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.DAL
{
    public class UserOpenIDDAL : BaseManagement
    {
        /// <summary>
        /// 通过用户openid获取用户OpenID相关信息
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public TB_UserOpenID GetUserOpenID(string openid)
        {
            return SelectByCondition<TB_UserOpenID>("OpenID='" + openid + "'");
        }

        /// <summary>
        /// 新增用户OpenID信息
        /// </summary>
        /// <param name="OpenIDInfo"></param>
        /// <returns></returns>
        public bool AddUserOpenIDInfo(TB_UserOpenID openidInfo)
        {
            return Insert<TB_UserOpenID>(openidInfo);
        }

        /// <summary>
        /// 更新用户OpenID信息
        /// </summary>
        /// <param name="openidInfo"></param>
        /// <returns></returns>
        public bool UpdateOpenIDInfo(TB_UserOpenID openidInfo)
        {
            return Update<TB_UserOpenID>(openidInfo);
        }
    }
}
