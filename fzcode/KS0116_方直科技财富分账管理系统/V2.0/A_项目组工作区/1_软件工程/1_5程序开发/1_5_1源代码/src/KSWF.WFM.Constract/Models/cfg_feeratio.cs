using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    public class cfg_feeratio
    {
        /// <summary>
        /// 自增ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 产品渠道
        /// </summary>
        public int channel { get; set; }

        /// <summary>
        /// 支付类型
        ///0 微信
        /// 1 支付宝
        /// 2 苹果
        /// </summary>
        public int feetype { get; set; }

        /// <summary>
        /// 扣款比例
        /// </summary>
        public decimal divided { get; set; }
    }
}
