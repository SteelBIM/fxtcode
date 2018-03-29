using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAPI.WebAPI.GeocodingAPI
{
    public class addressComponent
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 省名
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 城市名
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 区县名
        /// </summary>
        public string district { get; set; }
        /// <summary>
        /// 街道名
        /// </summary>
        public string street { get; set; }
        /// <summary>
        /// 街道门牌号
        /// </summary>
        public string street_number { get; set; }
        /// <summary>
        /// 国家code
        /// </summary>
        public string country_code { get; set; }
        /// <summary>
        /// 和当前坐标点的方向，当有门牌号的时候返回数据
        /// </summary>
        public string direction { get; set; }
        /// <summary>
        /// 和当前坐标点的距离，当有门牌号的时候返回数据
        /// </summary>
        public string distance { get; set; }
    }
}
