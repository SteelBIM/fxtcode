using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAPI.WebAPI.GeocodingAPI
{
    public class GeocodingResponse
    {
        public string status { get; set; }
        public result result { get; set; }

    }
    public class result
    {
        public Location location { get; set; }
        /// <summary>
        /// 结构化地址信息
        /// </summary>
        public string formatted_address { get; set; }
        /// <summary>
        /// 所在商圈信息，如 "人民大学,中关村,苏州街"
        /// </summary>
        public string business { get; set; }
        /// <summary>
        /// 地址内容
        /// </summary>
        public addressComponent addressComponent { get; set; }
    }
}
