using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAPI
{
    /// <summary>
    /// 响应信息
    /// </summary>
    public class NearResponse
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 结果集合
        /// </summary>
        public List<NearPOI> pointList { get; set; }
    }
}
