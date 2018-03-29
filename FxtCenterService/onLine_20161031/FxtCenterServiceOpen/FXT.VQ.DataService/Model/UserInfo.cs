using System;
using System.Threading.Tasks;

namespace FXT.VQ.DataService.Model
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
                return int.Parse(ConfigSettings.mDataCenterCompanyid);
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
