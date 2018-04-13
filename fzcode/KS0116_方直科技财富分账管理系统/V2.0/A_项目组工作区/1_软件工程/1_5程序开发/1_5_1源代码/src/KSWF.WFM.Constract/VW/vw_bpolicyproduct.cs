using KSWF.WFM.Constract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.VW
{
    public class vw_bpolicyproduct
    {
        public int id { get; set; }
        /// <summary>
        /// 策略id
        /// </summary>
        /// 
        public int bid { get; set; }
        public int versionid { get; set; }

        public string version { get; set; }
        public string category { get; set; }
        public int categorykey { get; set; }

        public decimal divided { get; set; }

        public decimal class_divided { get; set; }

    }
}
