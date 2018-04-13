using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    /// <summary>
    /// 异常日志
    /// </summary>
    public class abnormalLog
    {
        public int Id { get; set; }
        public string Features { get; set; }
        public string Misrepresentation { get; set; }
        public string Time { get; set; }
        public string MasterName { get; set; }
    }
}
