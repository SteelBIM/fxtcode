using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.UserOrder.Contract.DataModel
{
    public class UserModuleItem
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int UserModuleItemID { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string PayOrderID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }
        /// <summary>
        /// 书籍ID
        /// </summary>
        public int MarketBookID { get; set; }
        /// <summary>
        /// 模块ID
        /// </summary>
        public int ModuleID { get; set; }
        /// <summary>
        /// 使用时长
        /// </summary>
        public int Months { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 状态：0未启用 1启用 2禁用
        /// </summary>
        public int States { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 用户手机
        /// </summary>
        public string UserPhone { get; set; }
    }
}
