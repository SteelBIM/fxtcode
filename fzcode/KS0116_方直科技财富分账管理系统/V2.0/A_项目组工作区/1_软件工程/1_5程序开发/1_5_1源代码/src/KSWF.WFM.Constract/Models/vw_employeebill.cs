using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    /// <summary>
    /// 员工结算单
    /// </summary>
    public class vw_employeebill:order_setbonus
    {
        public string truename{get;set;}
        public string deptname { get; set; }
        public string addname { get; set; }
        public int deptid { get; set; }

        public string sTime { get; set; }
        
    }

}
