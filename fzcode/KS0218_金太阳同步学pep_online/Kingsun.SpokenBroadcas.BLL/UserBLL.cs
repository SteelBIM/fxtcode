using KingsSun.SpokenBroadcas.DAL;
using Kingsun.SpokenBroadcas.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.SpokenBroadcas.BLL
{
    public class UserBLL
    {
        UserDAL dal = new UserDAL();
        /// <summary>
        /// 根据条件查询Tb_UserInfo
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IList<Tb_UserInfo> GetUserInfo(string strWhere, string orderby = "")
        {
            return dal.GetUserInfo(strWhere, orderby);
        }
        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddUserInfo(Tb_UserInfo model)
        {
            return dal.AddUserInfo(model);
        }
        /// <summary>
        /// 根据UserID修改用户信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="TrueName"></param>
        /// <param name="TelePhone"></param>
        /// <returns></returns>
        public bool UpdateUserInfo(string UserID, string TrueName, string TelePhone)
        {
            return dal.UpdateUserInfo(UserID, TrueName, TelePhone);
        }
    }
}
