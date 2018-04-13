using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Core.Utility
{
    /// <summary>
    /// 系统默认值
    /// </summary>
    public class SystemDefault
    {

       
        /// <summary>
        /// 开通用户模块默认订单号
        /// </summary>
        /// <returns></returns>
        public static string DefaultOrderNo 
        {
            get
            {
                return "00000";
            }
        }
    }
}
