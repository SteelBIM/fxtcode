using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    public class base_deptarea
    {
        public int id{get;set;}
        public int deptid { get; set; }
        public int districtid { get; set; }
        public string path{get;set;}

        public int isend { get; set; }

        public string schoolname { get; set; }

        public int schoolid { get; set; }
    }
}
