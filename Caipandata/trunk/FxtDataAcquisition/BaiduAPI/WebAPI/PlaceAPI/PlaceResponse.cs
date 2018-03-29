using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAPI
{
    /// <summary>
    /// Place响应结果
    /// </summary>
    public class PlaceResponse
    {
        /// <summary>
        /// 状态,成功返回0
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 消息说明
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 检索总数(用户请求中设置了page_num字段才会出现total字段。当检索总数值大于760时，多次刷新同一请求得到total的值可能稍有不同，属于正常情况)
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 信息点集合
        /// </summary>
        public List<POI> results { get; set; }
    }
}
