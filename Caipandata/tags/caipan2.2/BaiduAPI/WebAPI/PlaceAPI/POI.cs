using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiduAPI
{
    /// <summary>
    /// 兴趣点
    /// </summary>
    public class POI
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string name  { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string telephone  { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public Location location { get; set; }
        /// <summary>
        /// 详细信息
        /// </summary>
        public DetailInfo detail_info { get; set; }
    }
}
