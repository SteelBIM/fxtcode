using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiduAPI
{
    /// <summary>
    /// 兴趣点详细信息
    /// </summary>
    public class DetailInfo
    {
        /// <summary>
        /// 距离中心点距离
        /// </summary>
        public int distance { get; set; }
        /// <summary>
        /// 所属分类，如’hotel’、’cater’。
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string tag { get; set; }
    }
}
