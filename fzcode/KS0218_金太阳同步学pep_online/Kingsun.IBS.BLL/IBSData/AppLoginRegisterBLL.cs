using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingsun.IBS.BLL.FZUUMS_UserService;
using Kingsun.IBS.IBLL;
using Kingsun.IBS.Model;
using Kingsun.SynchronousStudy.Common;

namespace Kingsun.IBS.BLL.IBSData
{
    public class AppLoginRegisterBLL : IAppLoginRegisterBLL
    {
        static RedisListOtherHelper listRedis = new RedisListOtherHelper();
        static FZUUMS_UserService.FZUUMS_UserServiceSoapClient userService = new FZUUMS_UserService.FZUUMS_UserServiceSoapClient();
        public void IBSRegisterUser2Mod()
        {
            var listCount = listRedis.Count("AppLoginRegisterQueue");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            int Changetype = 0;
            int DataType = 0;
            for (int i = 0; i < Count; i++)
            {
                var model = listRedis.RemoveStartFromList("AppLoginRegisterQueue");
                try
                {
                    IBS_UserInfo user = JsonHelper.DecodeJson<IBS_UserInfo>(model);
                   
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "同步IBS注册用户信息到MOD失败！Data=" + model);
                    listRedis.LPush("SecondAppLoginRegisterQueue",model);
                }
            }
        }



        public void IBSRegisterUser2ModReTryFirstTime()
        {
            var listCount = listRedis.Count("SecondAppLoginRegisterQueue");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            for (int i = 0; i < Count; i++)
            {
                var model = listRedis.RemoveStartFromList("SecondAppLoginRegisterQueue");
                try
                {
                    IBS_UserInfo user = JsonHelper.DecodeJson<IBS_UserInfo>(model);
                    User modUser = new User();
                    modUser.UserID = user.UserID.ToString();
                    modUser.UserName = user.UserName;
                    modUser.UserType = user.UserType;
                    modUser.TrueName = user.TrueName;
                    modUser.Telephone = user.TelePhone;
                    modUser.State = 0;
                    modUser.RegDate = user.Regdate;
                    modUser.PassWord = user.UserPwd;
                    modUser.LoginNum = 1;
                    modUser.LastLoginDate = DateTime.Now;
                    userService.CBSSAddUserInfo(modUser);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "第二次同步IBS注册用户信息到MOD失败！Data=" + model);
                    listRedis.LPush("ThreeAppLoginRegisterQueue", model);
                }
            }
        }

        public void IBSRegisterUser2ModReTrySecondTime()
        {
            var listCount = listRedis.Count("ThreeAppLoginRegisterQueue");
            int Count = Convert.ToInt32(listCount) > 1000 ? 1000 : Convert.ToInt32(listCount);
            for (int i = 0; i < Count; i++)
            {
                var model = listRedis.RemoveStartFromList("ThreeAppLoginRegisterQueue");
                try
                {
                    IBS_UserInfo user = JsonHelper.DecodeJson<IBS_UserInfo>(model);
                    User modUser = new User();
                    modUser.UserID = user.UserID.ToString();
                    modUser.UserName = user.UserName;
                    modUser.UserType = user.UserType;
                    modUser.TrueName = user.TrueName;
                    modUser.Telephone = user.TelePhone;
                    modUser.State = 0;
                    modUser.RegDate = user.Regdate;
                    modUser.PassWord = user.UserPwd;
                    modUser.LoginNum = 1;
                    modUser.LastLoginDate = DateTime.Now;
                    userService.CBSSAddUserInfo(modUser);
                }
                catch (Exception ex)
                {
                    Log4Net.LogHelper.Error(ex, "第三次同步IBS注册用户信息到MOD失败！Data=" + model);
                }
            }
        }
    }
}
