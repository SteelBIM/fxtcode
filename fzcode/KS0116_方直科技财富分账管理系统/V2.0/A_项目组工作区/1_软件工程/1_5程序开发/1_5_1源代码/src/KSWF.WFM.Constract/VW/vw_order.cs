using KSWF.WFM.Constract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.VW
{
    public class vw_order:orderinfo
    {
        public string parentname { get; set; }
        public string p_version { get; set; }
        public string p_versionid { get; set; }
    }
}
