using Kingsun.IBS.IBLL.IBS_TBX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.BLL.IBS_TBX
{
    public class IBS_TBXUserBLL : IIBS_TBXUserInfoBLL
    {
        public Model.IBS_UserInfo GetUserInfoByUserId(int userId)
        {
            throw new NotImplementedException();
        }

        public Model.IBS_UserInfo GetUserInfoByPhoneOrUserName(string phoneOrUserName, string Type)
        {
            throw new NotImplementedException();
        }

        public Model.IBS_UserInfo GetUserInfoByTchInvNum(string tchInvNum)
        {
            throw new NotImplementedException();
        }
    }
}
