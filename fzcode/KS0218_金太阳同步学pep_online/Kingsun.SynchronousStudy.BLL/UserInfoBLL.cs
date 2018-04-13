using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.DAL;
using Kingsun.SynchronousStudy.Models;

namespace Kingsun.SynchronousStudy.BLL
{
    public class UserInfoBLL
    {
        UserInfoDAL usermanager = new UserInfoDAL();
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IList<Tb_UserInfo> GetUserList()
        {
            return usermanager.GetUserList();
        }
        /// <summary>
        /// 条件查询
        /// </summary>    
        public IList<Tb_UserInfo> GetUserList(string where)
        {
            return usermanager.GetUserList(where);
        }

        /// <summary>
        /// 条件查询
        /// </summary>    
        public IList<TB_UserStatistics> GetUserStatisticsList(string where)
        {
            return usermanager.GetUserStatisticsList(where);
        }

        /// <summary>
        /// 根据useid 更新该用户是否被禁用
        /// </summary>
        /// <param name="user">用户userid</param>
        /// <param name="isyes">yes，no</param>
        /// <returns></returns>
        public bool UpdateIsUser(string user, string isyes)
        {
            try
            {
                Tb_UserInfo tuser = usermanager.GetUserInfoByID(user);
                if (isyes == "yes")
                {
                    tuser.IsUser = 0;
                }
                else
                {
                    tuser.IsUser = 1;
                }

                if (usermanager.UpdateUser(tuser))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 分页查询用户信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="Where"></param>
        /// <param name="order">排序字段</param>
        /// <param name="orderType">排序规则1-正序，2为倒序</param>
        /// <returns></returns>
        public IList<Tb_UserInfo> QueryUser(int pageIndex, int pageSize, string Where, string order, int orderType, out int totalCount)
        {
            PageParameter param = new PageParameter();
            param.Columns = "*";
            param.TbNames = "Tb_UserInfo";
            param.PageSize = pageSize;
            param.PageIndex = pageIndex;
            param.OrderColumns = order;
            param.IsOrderByASC = orderType;
            param.Where = Where;
            IList<Tb_UserInfo> userlist = usermanager.QueryUser(ref param);
            totalCount = param.TotalRecords;
            return userlist;
        }   


        public Tb_UserInfo GetUserByPhone(string UserPhone)
        {
            Tb_UserInfo TempUser = usermanager.GetUserByPhone(UserPhone);
            return TempUser;
        }

        public bool AppUpdateUserinfo(Tb_UserInfo userinfo)
        {
            return usermanager.UpdateUser(userinfo);
        }

        /// <summary>
        /// 根据userid获取用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Tb_UserInfo GetUserInfoByUserID(string userid) 
        {
            Tb_UserInfo TempUser = usermanager.GetUserByUserID(userid);
            return TempUser;
        }

        /// <summary>
        /// 根据用户信息更新用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool UpdateUserInfo(Tb_UserInfo user) 
        {
           return usermanager.UpdateUser(user);
        }

        /// <summary>
        /// 插入用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool InsertUserInfo(Tb_UserInfo userInfo)
        {
            return usermanager.InsertUserInfo(userInfo);
        }
    }
}
