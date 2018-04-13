using System;
using CBSS.Core.Cache;

namespace CBSS.Web.Admin.Common
{
    public class AdminUserContext : UserContext
    {
        public AdminUserContext()
            : base(AdminCookieContext.Current)
        {
        }

        public AdminUserContext(IAuthCookie authCookie)
            : base(authCookie)
        {
        }

        public static AdminUserContext Current
        {
            get
            {
                return CacheHelper.GetItem<AdminUserContext>();
            }
        }
    }
}
