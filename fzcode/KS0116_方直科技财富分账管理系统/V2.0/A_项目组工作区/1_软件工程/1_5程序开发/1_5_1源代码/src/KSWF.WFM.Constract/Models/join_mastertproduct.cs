using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    /// <summary>
    /// 用户产品表
    /// </summary>
    public class join_mastertproduct
    {
        public int id { get; set; }
        public string mastername { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        public int pid { get; set; }
    }
}
