using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    public class UserOpenIDBLL
    {
        UserOpenIDDAL userOpenIDDAL = new UserOpenIDDAL();

        /// <summary>
        /// 通过用户手机号获取用户OpenID
        /// </summary>
        /// <param name="telephone"></param>
        /// <returns></returns>
        public TB_UserOpenID GetUserOpenIDInfo(string openid)
        {
            return userOpenIDDAL.GetUserOpenID(openid);
        }

        /// <summary>
        /// 新增用户OpenID信息
        /// </summary>
        /// <param name="OpenIDInfo"></param>
        /// <returns></returns>
        public bool AddUserOpenIDInfo(TB_UserOpenID openidInfo)
        {
            return userOpenIDDAL.AddUserOpenIDInfo(openidInfo);
        }

        /// <summary>
        /// 更新用户OpenID信息
        /// </summary>
        /// <param name="openidInfo"></param>
        /// <returns></returns>
        public bool UpdateOpenIDInfo(TB_UserOpenID openidInfo)
        {
            return userOpenIDDAL.UpdateOpenIDInfo(openidInfo);
        }
    }
}
