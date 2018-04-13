using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.VW
{
    /// <summary>
    /// 代理商提成结算
    /// </summary>
    public class vw_agentmention
    {
        public string masterid { get; set; }
        public string mastername { get; set; }

        /// <summary>
        /// 未结算销售额
        /// </summary>
        public decimal? payamount { get; set; }
        
        /// <summary>
        /// 未结算订单数
        /// </summary>
        public decimal ordernumber { get; set; }
       
        public decimal? bouns
        {
            get;
            set;
        }
      
       
        public int deptid { get; set; }
        public string pagentid { get; set; }
        public string agentid { get; set; }
        public string agentname { get; set; }
        public string truename { get; set; }
        public string channel { get; set; }
        /// <summary>
        /// 代理商所在部门
        /// </summary>
        public string deptname { get; set; }
        /// <summary>
        /// 上次结算截止日期
        /// </summary>
        public int enddate { get; set; }
        /// <summary>
        /// 代理商签约截止日期
        /// </summary>
        public DateTime agent_enddate { get; set; }

    }
}
