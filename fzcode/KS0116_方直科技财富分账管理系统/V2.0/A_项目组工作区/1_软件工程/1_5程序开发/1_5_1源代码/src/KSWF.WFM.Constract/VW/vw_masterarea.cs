using KSWF.WFM.Constract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.VW
{
    /// <summary>
    /// 用户区域
    /// </summary>
    public class vw_masterarea : join_mastertarea
    {
        /// <summary>
        /// 0员工 1代理商
        /// </summary>
        public int mastertype { get; set; }
        public string agentid { get; set; }
        public string agentname { get; set; }
        public int deptid { get; set; }
        public string deptname { get; set; }

        public string truename { get; set; }

    }
}
