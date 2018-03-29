using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAPI
{
    /// <summary>
    /// 请求参数
    /// </summary>
    public class NearRequest
    {
        /// <summary>
        /// API密钥
        /// </summary>
        public string apikey { get; set; }
        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string keyWord { get; set; }
        /// <summary>
        /// 搜索的坐标
        /// </summary>
        public Location location { get; set; }
        /// <summary>
        /// 基础分类
        /// </summary>
        public TagType tag { get; set; }
        /// <summary>
        /// 搜索半径（默认为3000m）
        /// </summary>
        public int radius { get; set; }
        /// <summary>
        /// 搜索的城市名
        /// </summary>
        public string cityName { get; set; }
        /// <summary>
        /// 与POI点距离排序（0.从近到远，1.从远到近）
        /// </summary>
        public int sort_rule { get; set; }
        /// <summary>
        /// 单页上所获取的数据的数目
        /// </summary>
        public int number { get; set; }
        /// <summary>
        /// 获取指定页面的数据
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 输出的数据格式，默认为xml格
        /// </summary>
        public OutputType output { get; set; }
        /// <summary>
        /// 输入坐标类型，默认为gcj02
        /// </summary>
        public CoordType coord_type { get; set; }
        /// <summary>
        /// 输出坐标类型，默认为gcj02
        /// </summary>
        public CoordType out_coord_type { get; set; }
    }
}
