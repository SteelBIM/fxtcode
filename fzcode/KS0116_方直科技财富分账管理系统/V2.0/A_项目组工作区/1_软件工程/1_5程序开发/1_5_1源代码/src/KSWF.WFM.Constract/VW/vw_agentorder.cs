using KSWF.WFM.Constract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.VW
{
    public class vw_agentorder: orderinfo
    {
        public string agentname { get; set; }
    }
}
