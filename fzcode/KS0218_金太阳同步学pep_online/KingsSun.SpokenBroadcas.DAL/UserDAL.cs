using Kingsun.SpokenBroadcas.Common;
using Kingsun.SpokenBroadcas.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KingsSun.SpokenBroadcas.DAL
{
    public class UserDAL : BaseManagement
    {
        static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// 根据条件查询Tb_UserInfo
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public IList<Tb_UserInfo> GetUserInfo(string strWhere, string orderby = "")
        {
            try
            {
                return Search<Tb_UserInfo>(strWhere, orderby);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return null;
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddUserInfo(Tb_UserInfo model)
        {
            try
            {
                string sql = string.Format(@"insert into ITSV_Base.[FZ_SynchronousStudy].dbo.[Tb_UserInfo](UserID,UserName,NickName,TrueName,UserImage,UserRoles,TelePhone,isFirstLog) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                    model.UserID, model.UserName, model.NickName, model.TrueName, model.UserImage, model.UserRoles, model.TelePhone, model.isFirstLog);
                int count = SqlHelper.ExecuteNonQuery(AppSetting.ConnectionString, CommandType.Text, sql);
                if (count > 0)
                {
                    return true;
                }
                return false;
                //return Insert<Tb_UserInfo>(model);
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return false;
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
            try
            {
                string sql = string.Format("  update ITSV_Base.[FZ_SynchronousStudy].dbo.[Tb_UserInfo] set TrueName='{1}',TelePhone='{2}',isFirstLog=1 where UserID='{0}'", UserID, TrueName, TelePhone);
                int count = SqlHelper.ExecuteNonQuery(AppSetting.ConnectionString, CommandType.Text, sql);
                if (count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Error("error", ex);
            }
            return false;
        }
    }
}
