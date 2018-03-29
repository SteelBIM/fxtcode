using System;
using System.Threading.Tasks;

namespace FXT.VQ.UserService.Model
{
    [Serializable]
    public class UserInfo
    {
        public string username
        {
            get
            {
                return "null";
            }
        }
        public string password
        {
            get
            {
                return "null";
            }
        }
        public int fxtcompanyid
        {
            get
            {
                return  int.Parse(ConfigSettings.mUserCenterCompanyid);
            }
        }
        public int subcompanyid
        {
            get
            {
                return 0;
            }
        }
    }
}
