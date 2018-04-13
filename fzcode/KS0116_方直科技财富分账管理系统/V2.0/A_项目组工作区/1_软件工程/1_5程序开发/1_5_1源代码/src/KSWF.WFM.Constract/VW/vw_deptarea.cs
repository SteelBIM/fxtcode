using KSWF.WFM.Constract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.VW
{
    public class vw_deptarea :base_deptarea
    {
        public int parentid { get; set; }
        public string deptname { get; set; }
        public string agentid { get; set; }
    }
}
