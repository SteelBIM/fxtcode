using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAPI.WebAPI.GeocodingAPI
{
    public class GeocodingRequest
    {
        /// <summary>
        /// 输出格式
        /// </summary>
        public OutputType output { get; set; }
        /// <summary>
        /// 开发者key
        /// </summary>
        public string ak { get; set; }
        /// <summary>
        /// 38.76623,116.43213 lat(纬度),lng(经度)
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 是否显示指定位置周边的poi，0为不显示，1为显示。当值为1时，显示周边100米内的poi。
        /// </summary>
        public int pois { get; set; }
    }
}
