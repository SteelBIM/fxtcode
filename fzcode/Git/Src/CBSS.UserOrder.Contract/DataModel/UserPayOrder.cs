using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.UserOrder.Contract.DataModel
{
    public class UserPayOrder
    {
        /// <summary>
        /// 支付ID
        /// </summary>
        public Guid UserPayOrderID { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 应用版本ID
        /// </summary>
        public int AppVersionID { get; set; }
        /// <summary>
        /// 应用名称
        /// </summary>
        public string AppName { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 用户名称 
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户电话
        /// </summary>
        public string UserPhone { get; set; }

        /// <summary>
        /// 支付方式(1支付宝,2微信,3苹果)
        /// </summary>

        public int PayWay { get; set; }

        /// <summary>
        /// 商品总价
        /// </summary>
        public double TotalPrice { get; set; }

        /// <summary>
        /// 优惠价格
        /// </summary>
        public double PreferentialPrice { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public double PayMoney { get; set; }

        /// <summary>
        /// 状态(0未付款,1已付款,2退货)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 用于标识消息队列是新增还是更新  1-新增  2-更新
        /// </summary>
        public int Type { get; set; }
    }
}
