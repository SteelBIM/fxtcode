using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.Models
{
    /// <summary>
    /// 用户负责区域
    /// </summary>
    public class join_mastertarea
    {
        public int id { get; set; }
        public string mastername { get; set; }
        /// <summary>
        /// 产品key集合
        /// </summary>
        public string pids { get; set; }
        /// <summary>
        /// 区域
        /// </summary>
        public string districtid { get; set; }
        public string path { get; set; }
        public int schoolid { get; set; }
        public string schoolname { get; set; }

    }
}
