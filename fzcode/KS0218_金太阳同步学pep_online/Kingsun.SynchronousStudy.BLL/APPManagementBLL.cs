using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    public class APPManagementBLL
    {
        APPManagementDAL appmanagement = new APPManagementDAL();
        public TB_APPManagement GetAPPManagement(string id)
        {
            try
            {
                return appmanagement.GetAPPManagement(id);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public TB_APPManagement GetAPPByEditionID(string id)
        {
            try
            {
                return appmanagement.GetAPPByEditionID(id);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 获取版本列表
        /// </summary>
        /// <returns></returns>
        public IList<TB_APPManagement> QueryAPPList()
        {
            IList<TB_APPManagement> list = appmanagement.QueryAPPList();
            return list;
        }

        /// <summary>
        /// 根据userid获取用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public TB_UserEditionInfo GetUserEditionByID(string userid)
        {
            TB_UserEditionInfo TempUser = appmanagement.GetUserEditionByID(userid);
            return TempUser;
        }

        /// <summary>
        /// 根据用户信息更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateUserEditionInfo(TB_UserEditionInfo edition)
        {
            return appmanagement.UpdateUserEditionInfo(edition);
        }

        /// <summary>
        /// 插入用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool InsertUserEditionInfo(TB_UserEditionInfo edition)
        {
            return appmanagement.AddUserEditionInfo(edition);
        }
    }
}
