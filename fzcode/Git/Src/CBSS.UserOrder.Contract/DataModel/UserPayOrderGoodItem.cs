using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.UserOrder.Contract.DataModel
{
    public class UserPayOrderGoodItem
    {
        public Guid UserPayOrderGoodItemID { get; set; }
        public Guid UserPayOrderID { get; set; }

        /// <summary>
        /// 商品策略ID
        /// </summary>
        public int GoodID { get; set; }
        /// <summary>
        /// 商品策略名
        /// </summary>
        public string GoodName { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public double OriginalPrice { get; set; }
        /// <summary>
        /// 优惠价
        /// </summary>
        public double PreferentialPrice { get; set; }
        /// <summary>
        /// 商品单价(市场价)
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public double TotalPrice { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
