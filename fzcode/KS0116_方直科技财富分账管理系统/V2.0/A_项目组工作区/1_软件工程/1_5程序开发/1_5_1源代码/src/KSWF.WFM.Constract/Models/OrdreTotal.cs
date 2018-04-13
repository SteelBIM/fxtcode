using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    /// <summary>
    /// 订单金额合计
    /// </summary>
    public class OrdreTotal
    {
        private decimal? _o_bonus = 0;
        /// <summary>
        /// 提成金额
        /// </summary>
        public decimal? o_bonus
        {
            get { return _o_bonus; }
            set { _o_bonus = value; }
        }
        public int o_number { get; set; }

        private decimal? _o_payamount = 0;
        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal? o_payamount
        {
            get { return _o_payamount; }
            set { _o_payamount = value; }
        }
    }
    public class orderdetailed
    {
        public string days { get; set; }
        public int ordernumber { get; set; }
        public decimal? bonus { get; set; }
        public decimal? payamount { get; set; }
    }
}
