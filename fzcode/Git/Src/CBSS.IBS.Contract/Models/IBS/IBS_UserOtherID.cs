using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.IBS.Contract
{
    /// <summary>
    /// 手机号/登陆账号/教师邀请码对应UserID
    /// </summary>
    public class IBS_UserOtherID
    {
        /// <summary>
        /// 手机号/登陆账号/教师邀请码
        /// </summary>
        public string UserIDOther { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 类型（UserID=0,TelePhone=1,UserName=2,TchInvNum=3）
        /// </summary>
        public int Type { get; set; }
    }


}
