using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Framework.Contract.Enums
{
    /// <summary>
    /// 支付状态
    /// </summary>
    public enum PayStatusEnum
    {
        /// <summary>
        /// 待支付
        /// </summary>
        [System.ComponentModel.Description("待支付")]
        Unpaid = 1,
        /// <summary>
        /// 已支付
        /// </summary>
        [System.ComponentModel.Description("已支付")]
        HavePaid = 2,
        /// <summary>
        /// 退货
        /// </summary>
        [System.ComponentModel.Description("退货")]
        SalesReturn = 3
        
    }
}
