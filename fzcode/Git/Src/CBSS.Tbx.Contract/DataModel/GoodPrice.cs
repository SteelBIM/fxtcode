using CBSS.Framework.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBSS.Tbx.Contract.DataModel
{
    /// <summary>
    /// 商品价格
    /// </summary>
    [Auditable]
    [Table("GoodPrice")]
    public class GoodPrice
    {
        public int GoodPriceID { get; set; }
        /// <summary>
        /// 商品ID
        /// </summary>
        public int GoodID { get; set; }
        /// <summary>
        /// 有效使用时长 单位(月)
        /// </summary>
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "请输入大于0的正整数")]
        public int GoodsBpolicyMonths { get; set; }
        /// <summary>
        /// 安卓价格
        /// </summary>
        public double AndroidPrice { get; set; }
        /// <summary>
        /// IOS商品ID
        /// </summary>
        public string IOSCommodityID { get; set; }

        /// IOS价格
        /// </summary>
        public double IOSPrice { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "请输入大于0的正整数")]
        [Range(1, 10000)]
        public int Sort { get; set; }

        private DateTime _time = DateTime.Now;
        public DateTime CreateDate
        {
            get { return _time; }
            set { _time = value; }
        }
        public int CreateUser { get; set; }

        /// <summary>
        /// 商品原价
        /// </summary>
        public double GoodsOriginalPrice { get; set; }
    }
}
