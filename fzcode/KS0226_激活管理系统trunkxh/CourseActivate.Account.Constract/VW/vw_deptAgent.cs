using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Account.Constract.VW
{
    /// <summary>
    ///根据部门获取代理商ID
    /// </summary>
    public class vw_deptAgent
    {
        public int deptid { get; set; }  
        public int masterid { get; set; }
    }
}
