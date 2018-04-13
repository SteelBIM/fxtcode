using CBSS.Tbx.Contract;
using CBSS.Tbx.Contract.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.IBLL
{
  public  interface IUserInfoService
    {
        IEnumerable<TB_UserInfo> GetUserInfoList(out int totalcount, UserInfoRequest request = null);
    }
}
