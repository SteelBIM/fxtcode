using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BaiduAPI
{
    /// <summary>
    /// 详细信息
    /// </summary>
    public class NearPOIInfo
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string telephone { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// 基础分类
        /// </summary>
        public string tag { get; set; }
    }
}
