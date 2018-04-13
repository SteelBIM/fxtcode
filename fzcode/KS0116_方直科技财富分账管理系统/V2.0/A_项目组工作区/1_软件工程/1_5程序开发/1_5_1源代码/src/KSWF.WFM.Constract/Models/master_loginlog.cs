using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    /// <summary>
    /// 登录日期
    /// </summary>
    public class master_loginlog
    {
        public string mastername { get; set; }
        public string loginip { get; set; }
        public DateTime logintime { get; set; }

        public string lastloginip { get; set; }

        public DateTime lastlogintime { get; set; }
    }
}
