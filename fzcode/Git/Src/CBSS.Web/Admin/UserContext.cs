using System;
using CBSS.Account.Contract;
using CBSS.Core.Cache;

namespace CBSS.Web
{
    public class UserContext
    {
        protected IAuthCookie authCookie;

        public UserContext(IAuthCookie authCookie)
        {
            this.authCookie = authCookie;
        }

        public Sys_LoginInfo LoginInfo
        {
            get
            {
                return CacheHelper.GetItem<Sys_LoginInfo>("LoginInfo", () =>
                {
                    if (authCookie.UserToken == Guid.Empty)
                        return null;
                    
                    var loginInfo = ServiceContext.Current.AccountService.GetLoginInfo(authCookie.UserToken);

                    if (loginInfo != null && loginInfo.UserID > 0 && loginInfo.UserID != this.authCookie.UserId)
                        throw new Exception("非法操作，试图通过网站修改Cookie取得用户信息！");

                    return loginInfo;
                });
            }
        }
    }
}
