using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.VW
{
    /// <summary>
    /// 人员策略管理
    /// </summary>
    public class vw_masterbpolicypr
    {
         
        public int id { get; set; }
        public int pid { get; set; }
        public int bid { get; set; }
        public DateTime startdate { get; set; }
        public string pllicyname { get; set; }
        public string productname { get; set; }
        public string mastername { get; set; }
        public string effectivestatus { get; set; }
        public DateTime createtime { get; set; }
        public string grouplogo { get; set; }
    }
}
