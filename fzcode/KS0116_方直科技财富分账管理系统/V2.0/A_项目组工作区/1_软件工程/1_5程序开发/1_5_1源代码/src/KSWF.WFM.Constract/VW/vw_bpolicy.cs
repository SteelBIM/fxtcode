using KSWF.WFM.Constract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.VW
{
    public class vw_bpolicy : cfg_bpolicy
    {
        public string productname { get; set; }
        public string divided { get; set; }
        public string class_divided { get; set; }
        public string version { get; set; }
        public string category { get; set; }
    }
}
