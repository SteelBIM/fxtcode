using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Pay.Contract
{
    public class CommodityDetails
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppID { get; set; }
        /// <summary>
        /// 平台商品ID
        /// </summary>
        public int GoodID { get; set; }
        /// <summary>
        /// 平台商品名称
        /// </summary>
        public string GoodName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 模块ID
        /// </summary>
        public int ModuleID { get; set; }
        /// <summary>
        /// 有效使用时长 单位(月) 若长期有效 建议用1188(99年
        /// </summary>
        public int GoodsBpolicyMonths { get; set; }
        /// <summary>
        /// 商品原价
        /// </summary>
        public double GoodsOriginalPrice { get; set; }
        /// <summary>
        /// 安卓市场价格
        /// </summary>
        public double AndroidPrice { get; set; }
        /// <summary>
        /// IOS商品ID
        /// </summary>
        public string IOSCommodityID { get; set; }
        /// <summary>
        /// IOS市场价格
        /// </summary>
        public double IOSPrice { get; set; }
    }
}
