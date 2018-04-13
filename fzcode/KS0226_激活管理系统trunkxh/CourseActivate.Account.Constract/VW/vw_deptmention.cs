using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseActivate.Account.Constract.VW
{
    /// <summary>
    /// 部门结算提成
    /// </summary>
    public class vw_deptmention
    {
        public int m_deptid { get; set; }
        public decimal? o_payamount { get; set; }
        public decimal? o_bonus { get; set; }
        public int o_number { get; set; }
        public string m_deptname { get; set; }
        public int enddata { get; set; }
        /// <summary>
        /// 部门负责人
        /// </summary>
        public string principalname { get; set; }
        public string agentid { get; set; }
    }
}
