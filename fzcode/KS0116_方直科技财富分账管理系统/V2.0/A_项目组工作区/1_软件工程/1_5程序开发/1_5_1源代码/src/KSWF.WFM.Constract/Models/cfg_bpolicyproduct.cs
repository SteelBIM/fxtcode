using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    public class cfg_bpolicyproduct
    {
        //id  bid pid divided class_divided
        public int id { get; set; }
        public int bid { get; set; }
        public int? categorykey { get; set; }
        public int versionid { get; set; }
        public decimal divided { get; set; }
        public decimal class_divided { get; set; }
    }
}
