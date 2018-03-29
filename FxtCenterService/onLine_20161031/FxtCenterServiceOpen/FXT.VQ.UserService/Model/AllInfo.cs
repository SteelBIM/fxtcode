using System;

namespace FXT.VQ.UserService.Model
{
    [Serializable]
    public class AllInfo
    {
        private UserInfo _uinfo;
        private ApplicationInfo _appinfo;
        public AllInfo(UserInfo uinfo)
        {
            _uinfo = uinfo;
            _appinfo = new ApplicationInfo();
        }
        public ApplicationInfo appinfo
        {
            get
            {
                return _appinfo;
            }
        }

        public UserInfo uinfo
        {
            get
            {
                return _uinfo;
            }
        }
        public dynamic funinfo { get; set; }
    }
}
