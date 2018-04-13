using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// 支付方式
    /// </summary>
    public enum PayWayEnum
    {
        /// <summary>
        /// 支付宝
        /// </summary>
        [System.ComponentModel.Description("支付宝")]
        Alipay = 1,
        /// <summary>
        /// 安卓
        /// </summary>
        [System.ComponentModel.Description("微信")]
        WeChat = 2,
        /// <summary>
        /// 苹果
        /// </summary>
        [System.ComponentModel.Description("苹果")]
        Apple = 3
    }
}
