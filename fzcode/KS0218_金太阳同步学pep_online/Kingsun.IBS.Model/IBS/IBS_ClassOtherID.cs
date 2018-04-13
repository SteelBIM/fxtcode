using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingsun.IBS.Model
{
    /// <summary>
    /// 手机号/登陆账号/教师邀请码查找UserID
    /// </summary>
    public class IBS_ClassOtherID
    {
        /// <summary>
        /// 班级编码
        /// </summary>
        public string ClassIDOther { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        public string ClassID { get; set; }

        /// <summary>
        /// 查找类型（ClassID=0,ClassNum=1）
        /// </summary>
        public int Type { get; set; }
    }


}
