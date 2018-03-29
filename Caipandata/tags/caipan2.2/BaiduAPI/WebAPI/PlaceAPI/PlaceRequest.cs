using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiduAPI
{
    /// <summary>
    /// Place请求
    /// </summary>
    public class PlaceRequest
    {
        /// <summary>
        /// 开发者key
        /// </summary>
        public string ak { get; set; }
        /// <summary>
        /// 检索关键字（不同关键字间以$符号分隔，最多支持10个关键字检索）
        /// </summary>
        public string query { get; set; }
        /// <summary>
        /// 标签(与query组合进行检索，以“,”分隔)
        /// </summary>
        public string tag { get; set; }
        /// <summary>
        /// 输出格式
        /// </summary>
        public OutputType output { get; set; }
        /// <summary>
        /// 结果(检索结果详细程度。取值为1 或空，则返回基本信息)
        /// </summary>
        public ScopeType scope { get; set; }
        /// <summary>
        /// 过滤
        /// 示例：filter=industry_type:cater| sort_name:distance|sort_rule:1 
        /// 检索过滤条件，当scope取值为2时，可以设置filter进行排序。
        /// （industry_type：行业类型 注意：设置该字段可提高检索速度和过滤精度。 ）
        /// </summary>
        public string filter  { get; set; }
        public int page_size { get; set; }
        public int page_num { get; set; }
        /// <summary>
        /// 用户的权限签名
        /// </summary>
        public string sn { get; set; }
        /// <summary>
        /// 设置sn后该值必填
        /// </summary>
        public string timestamp  { get; set; }
        /// <summary>
        /// 检索区域
        /// </summary>
        public string region { get; set; }
        /// <summary>
        /// 矩形区域检索参数(38.76623,116.43213,39.54321,116.46773 lat,lng(左下角坐标),lat,lng(右上角坐标) )
        /// </summary>
        public string bounds { get; set; }
        /// <summary>
        /// 圆形区域检索参数(周边检索中心点 38.76623,116.43213 lat(纬度),lng(经度))
        /// </summary>
        public string location { get; set; }
        /// <summary>
        /// 圆形区域检索参数(周边检索半径，单位为米 )
        /// </summary>
        public int radius { get; set; }
    }

    public enum OutputType
    {
        xml, json
    }
    /// <summary>
    /// 结果类型
    /// </summary>
    public enum ScopeType
    {
        /// <summary>
        /// 简单信息
        /// </summary>
        Defult = 1, 
        /// <summary>
        /// 详细信息
        /// </summary>
        Details = 2
    }
}
