using KSWF.WFM.Constract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.VW
{
  public   class vw_agentbill : order_setbonus
    {
        public string truename { get; set; }
        public string deptname { get; set; }
        public string addname { get; set; }
        public int deptid { get; set; }

        public string sTime { get; set; }
        /// <summary>
        /// 渠道经理
        /// </summary>
        public string channelmanager { get; set; }
        public string agentname { get; set; }
    }

}
