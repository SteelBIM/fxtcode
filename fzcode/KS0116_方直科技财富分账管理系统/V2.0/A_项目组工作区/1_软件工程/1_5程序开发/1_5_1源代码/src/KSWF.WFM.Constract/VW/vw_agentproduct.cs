using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSWF.WFM.Constract.VW
{
    /// <summary>
    /// 代理商策略对应版本和分类
    /// </summary>
    public class vw_agentproduct
    {
        public string mastername { get; set; }
        public string agentid { get; set; }
        public int categorykey { get; set; }
        public int versionid { get; set; }
        public int pid { get; set; }
        public decimal? divided { get; set; }
        public decimal? class_divided { get; set; }

    }
}
