using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    public class base_dept
    {
        public int deptid { get; set; }

        public int parentid { get; set; }

        public string deptname { get; set; }

        public string createname { get; set; }

        public string createtime { get; set; }

        public int delflg { get; set; }

        //连表相关参数，用于显示
        public int isend { get; set; }
        public int districtid { get; set; }
        public string path { get; set; }
        public string agentid { get; set; }
        public string schoolid { get; set; }
        public string schoolname { get; set; }
        public int level { get; set; }
    }
}
