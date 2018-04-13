using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    /// <summary>
    /// 策略表
    /// </summary>
    public class cfg_bpolicy
    {
        public int bid { get; set; }
        public int ptype { get; set; }
        public string pllicyname { get; set; }
        public int pid { get; set; }
        public string remark { get; set; }
        public string createname { get; set; }
        public string createtime { get; set; }
        public int delflg { get; set; }
        public string agentid { get; set; }
    }
}
