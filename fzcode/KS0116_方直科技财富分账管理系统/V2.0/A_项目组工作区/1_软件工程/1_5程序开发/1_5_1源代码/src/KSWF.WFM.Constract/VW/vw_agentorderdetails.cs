using KSWF.WFM.Constract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.VW
{
    /// <summary>
    /// 代理商订单明细
    /// </summary>
    public class vw_agentorderdetails : orderinfo
    {
        public string agentname { get; set; }
    }
}
