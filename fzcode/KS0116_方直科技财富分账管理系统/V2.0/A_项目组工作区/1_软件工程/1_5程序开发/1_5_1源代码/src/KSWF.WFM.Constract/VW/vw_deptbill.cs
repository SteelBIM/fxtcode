using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSWF.WFM.Constract.Models;

namespace KSWF.WFM.Constract.VW
{
    /// <summary>
    /// 部门结算
    /// </summary>
    public class vw_deptbill:order_setbonus_dept
    {
        /// <summary>
        /// 部门负责人
        /// </summary>
        public string principalname{get;set;}
        public string addname { get; set; }
    }
}
