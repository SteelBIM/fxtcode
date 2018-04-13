using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    /// <summary>
    /// 订单提成结算记录
    /// </summary>
    public class order_setbonus
    {
        public Guid? osid { get; set; }
        public string os_no { get; set; }
        public string mastername_t { get; set; }
        public int startdate { get; set; }
        public int enddate { get; set; }
        public int total_count { get; set; }
        public decimal? total_amount { get; set; }
        public decimal? total_bonus { get; set; }
        public decimal? adjust_amount { get; set; }
        public string adjust_reason { get; set; }
        /// <summary>
        /// 0：员工  1：代理商 
        /// </summary>
        public int os_type { get; set; }
        public int state { get; set; }
        public string mastername { get; set; }
        public DateTime createtime { get; set; }
        public string agentid { get; set; }
    }
}
