using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAPI
{
    /// <summary>
    /// 兴趣点
    /// </summary>
    public class NearPOI
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string cityName { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public Location location { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 距离
        /// </summary>
        public string distance { get; set; }
        /// <summary>
        /// 行政区
        /// </summary>
        public string district { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public NearPOIInfo additionalInformation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
    }
}
