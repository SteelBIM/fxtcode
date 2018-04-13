using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    /// <summary>
    /// 用户产品策略
    /// </summary>
    public class join_masterbpolicypr
    {
        public int id { get; set; }
        public string mastername { get; set; }
        //策略ID
        public int bid { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime startdate { get; set; }

        //新增连表字段
        public int pid { get; set; }
    }
}
