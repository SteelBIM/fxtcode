using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.SynchronousStudy.Common.Base;
using Kingsun.SynchronousStudy.Models;
using Kingsun.IBS.Model;

namespace Kingsun.SynchronousStudy.DAL
{
    public class UserInfoDAL : BaseManagement
    {

        /// <summary>
        /// 查询所有Tb_UserInfo
        /// </summary>
        /// <returns></returns>
        public IList<Tb_UserInfo> GetUserList()
        {
            IList<Tb_UserInfo> list = SelectAll<Tb_UserInfo>();
            return list;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<Tb_UserInfo> GetUserList(string where)
        {
            IList<Tb_UserInfo> list = Search<Tb_UserInfo>(where);
            return list;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IList<TB_UserStatistics> GetUserStatisticsList(string where)
        {
            IList<TB_UserStatistics> list = Search<TB_UserStatistics>(where);
            return list;
        }

        /// <summary>
        /// 调用存储过程分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<Tb_UserInfo> QueryUser(ref PageParameter param)
        {
            List<System.Data.Common.DbParameter> list = new List<System.Data.Common.DbParameter>();
            list = param.getParameterList();
            System.Data.DataSet ds = ExecuteProcedure("proc_pageView", list);
            if (ds == null)
            {
                return null;
            }
            IList<Tb_UserInfo> Userlist = FillData<Tb_UserInfo>(ds.Tables[0]);

            param.TotalRecords = (int)ds.Tables[1].Rows[0][0];
            return Userlist;
        }

        /// <summary>
        /// 通过用户编号获取用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Tb_UserInfo GetUserInfoByID(string userid)
        {
            return SelectByCondition<Tb_UserInfo>("UserID=" + userid + " AND IsUser=1");
        }

        /// <summary>
        /// 新增用户信息
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public bool AddUser(Tb_UserInfo userinfo)
        {
            return Insert<Tb_UserInfo>(userinfo);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public bool UpdateUser(Tb_UserInfo userinfo)
        {
            return Update<Tb_UserInfo>(userinfo);
        }

        /// <summary>
        /// 获取用户信息通过用户名称
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public Tb_UserInfo GetUserByName(string username)
        {
            IList<Tb_UserInfo> list = Search<Tb_UserInfo>("UserName='" + username + "'  AND IsUser=1");
            if (list == null || list.Count == 0)
            {
                return null;
            }
            return list[0];
        }

        /// <summary>
        /// 获取用户信息通过用户电话
        /// </summary>
        /// <param name="UserPhone"></param>
        /// <returns></returns>
        public Tb_UserInfo GetUserByPhone(string UserPhone)
        {
            IList<Tb_UserInfo> list = Search<Tb_UserInfo>("TelePhone='" + UserPhone + "'  AND IsUser=1");
            if (list == null || list.Count == 0)
            {
                return null;
            }
            return list[0];
        }

        /// <summary>
        /// 根据userid获取用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Tb_UserInfo GetUserByUserID(string userid)
        {
            Tb_UserInfo userinfo = SelectByCondition<Tb_UserInfo>("UserID='" + userid + "'  AND IsUser=1");
            if (userid == null)
            {
                return null;
            }
            else
            {
                return userinfo;
            }
        }

        /// <summary>
        /// 插入用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public bool InsertUserInfo(Tb_UserInfo userInfo)
        {
            return Insert<Tb_UserInfo>(userInfo);
        }
    }
}
